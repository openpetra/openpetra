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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 75;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractDescId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastRefId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDeletableId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnFrozenId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnKeyCountId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnManualModicationId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnManualModicationDateId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnManualModById = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractTypeCodeId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnTemplateId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnRestrictedId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 18;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "MExtractMaster", "m_extract_master",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ExtractId", "m_extract_id_i", "Extract Id", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ExtractName", "m_extract_name_c", "Extract Name", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(2, "ExtractDesc", "m_extract_desc_c", "Description", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(3, "LastRef", "m_last_ref_d", "Last Referenced", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "Deletable", "m_deletable_l", "Deletable", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(5, "Frozen", "m_frozen_l", "Frozen", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(6, "KeyCount", "m_key_count_i", "Key Count", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "Public", "m_public_l", "Public", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(8, "ManualModication", "m_manual_mod_l", "m_manual_mod_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(9, "ManualModicationDate", "m_manual_mod_d", "Date Edited", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "ManualModBy", "m_manual_mod_by_c", "Edited By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "ExtractTypeCode", "m_extract_type_code_c", "Extract Type", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(12, "Template", "m_template_l", "Template", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(13, "Restricted", "m_restricted_l", "Extract Restricted", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public MExtractMasterRow this[int i]
        {
            get
            {
                return ((MExtractMasterRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public MExtractMasterTable GetChangesTyped()
        {
            return ((MExtractMasterTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "MExtractMaster";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "m_extract_master";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }

        /// get character length for column
        public static short GetExtractIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetExtractNameDBName()
        {
            return "m_extract_name_c";
        }

        /// get character length for column
        public static short GetExtractNameLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetExtractDescDBName()
        {
            return "m_extract_desc_c";
        }

        /// get character length for column
        public static short GetExtractDescLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetLastRefDBName()
        {
            return "m_last_ref_d";
        }

        /// get character length for column
        public static short GetLastRefLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "m_deletable_l";
        }

        /// get character length for column
        public static short GetDeletableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetFrozenDBName()
        {
            return "m_frozen_l";
        }

        /// get character length for column
        public static short GetFrozenLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetKeyCountDBName()
        {
            return "m_key_count_i";
        }

        /// get character length for column
        public static short GetKeyCountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicDBName()
        {
            return "m_public_l";
        }

        /// get character length for column
        public static short GetPublicLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetManualModicationDBName()
        {
            return "m_manual_mod_l";
        }

        /// get character length for column
        public static short GetManualModicationLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetManualModicationDateDBName()
        {
            return "m_manual_mod_d";
        }

        /// get character length for column
        public static short GetManualModicationDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetManualModByDBName()
        {
            return "m_manual_mod_by_c";
        }

        /// get character length for column
        public static short GetManualModByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetExtractTypeCodeDBName()
        {
            return "m_extract_type_code_c";
        }

        /// get character length for column
        public static short GetExtractTypeCodeLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetTemplateDBName()
        {
            return "m_template_l";
        }

        /// get character length for column
        public static short GetTemplateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "m_restricted_l";
        }

        /// get character length for column
        public static short GetRestrictedLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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

        /// Who made the last manual modification ?
        public String ManualModBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnManualModBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsExtractIdNull()
        {
            return this.IsNull(this.myTable.ColumnExtractId);
        }

        /// assign NULL value
        public void SetExtractIdNull()
        {
            this.SetNull(this.myTable.ColumnExtractId);
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
        public bool IsFrozenNull()
        {
            return this.IsNull(this.myTable.ColumnFrozen);
        }

        /// assign NULL value
        public void SetFrozenNull()
        {
            this.SetNull(this.myTable.ColumnFrozen);
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
        public bool IsPublicNull()
        {
            return this.IsNull(this.myTable.ColumnPublic);
        }

        /// assign NULL value
        public void SetPublicNull()
        {
            this.SetNull(this.myTable.ColumnPublic);
        }

        /// test for NULL value
        public bool IsManualModicationNull()
        {
            return this.IsNull(this.myTable.ColumnManualModication);
        }

        /// assign NULL value
        public void SetManualModicationNull()
        {
            this.SetNull(this.myTable.ColumnManualModication);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 76;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "MExtract", "m_extract",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ExtractId", "m_extract_id_i", "Extract Id", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "SiteKey", "p_site_key_n", "Site Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(3, "LocationKey", "p_location_key_i", "Location Key", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public MExtractRow this[int i]
        {
            get
            {
                return ((MExtractRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public MExtractTable GetChangesTyped()
        {
            return ((MExtractTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "MExtract";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "m_extract";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }

        /// get character length for column
        public static short GetExtractIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }

        /// get character length for column
        public static short GetSiteKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }

        /// get character length for column
        public static short GetLocationKeyLength()
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsExtractIdNull()
        {
            return this.IsNull(this.myTable.ColumnExtractId);
        }

        /// assign NULL value
        public void SetExtractIdNull()
        {
            this.SetNull(this.myTable.ColumnExtractId);
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
        public bool IsSiteKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKey);
        }

        /// assign NULL value
        public void SetSiteKeyNull()
        {
            this.SetNull(this.myTable.ColumnSiteKey);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 77;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnFunctionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "MExtractType", "m_extract_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "m_code_c", "Code", OdbcType.VarChar, 50, true),
                    new TTypedColumnInfo(1, "Function", "m_function_c", "Function", OdbcType.VarChar, 500, false),
                    new TTypedColumnInfo(2, "Description", "m_description_c", "Description", OdbcType.VarChar, 200, false),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public MExtractTypeRow this[int i]
        {
            get
            {
                return ((MExtractTypeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public MExtractTypeTable GetChangesTyped()
        {
            return ((MExtractTypeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "MExtractType";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "m_extract_type";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "m_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetFunctionDBName()
        {
            return "m_function_c";
        }

        /// get character length for column
        public static short GetFunctionLength()
        {
            return 500;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "m_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 200;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }

        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 78;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnParameterCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnValueIndexId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnParameterValueId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "MExtractParameter", "m_extract_parameter",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ExtractId", "m_extract_id_i", "Extract Id", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ParameterCode", "m_parameter_code_c", "Code", OdbcType.VarChar, 50, true),
                    new TTypedColumnInfo(2, "ValueIndex", "m_value_index_i", "Value Index", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "ParameterValue", "m_parameter_value_c", "Value", OdbcType.VarChar, 200, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public MExtractParameterRow this[int i]
        {
            get
            {
                return ((MExtractParameterRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public MExtractParameterTable GetChangesTyped()
        {
            return ((MExtractParameterTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "MExtractParameter";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "m_extract_parameter";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }

        /// get character length for column
        public static short GetExtractIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetParameterCodeDBName()
        {
            return "m_parameter_code_c";
        }

        /// get character length for column
        public static short GetParameterCodeLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetValueIndexDBName()
        {
            return "m_value_index_i";
        }

        /// get character length for column
        public static short GetValueIndexLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetParameterValueDBName()
        {
            return "m_parameter_value_c";
        }

        /// get character length for column
        public static short GetParameterValueLength()
        {
            return 200;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsExtractIdNull()
        {
            return this.IsNull(this.myTable.ColumnExtractId);
        }

        /// assign NULL value
        public void SetExtractIdNull()
        {
            this.SetNull(this.myTable.ColumnExtractId);
        }

        /// test for NULL value
        public bool IsParameterCodeNull()
        {
            return this.IsNull(this.myTable.ColumnParameterCode);
        }

        /// assign NULL value
        public void SetParameterCodeNull()
        {
            this.SetNull(this.myTable.ColumnParameterCode);
        }

        /// test for NULL value
        public bool IsValueIndexNull()
        {
            return this.IsNull(this.myTable.ColumnValueIndex);
        }

        /// assign NULL value
        public void SetValueIndexNull()
        {
            this.SetNull(this.myTable.ColumnValueIndex);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 79;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingDateId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCostId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingAttributedAmountId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnViewableId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnViewableUntilId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 13;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PMailing", "p_mailing",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, true),
                    new TTypedColumnInfo(1, "MailingDescription", "p_mailing_description_c", "Mailing Description", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "MailingDate", "p_mailing_date_d", "Mailing Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(5, "MailingCost", "p_mailing_cost_n", "Mailing Cost", OdbcType.Decimal, 19, false),
                    new TTypedColumnInfo(6, "MailingAttributedAmount", "p_mailing_attributed_amount_n", "Attributed Amount", OdbcType.Decimal, 19, false),
                    new TTypedColumnInfo(7, "Viewable", "p_viewable_l", "Viewable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(8, "ViewableUntil", "p_viewable_until_d", "Viewable until Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(13, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PMailingRow this[int i]
        {
            get
            {
                return ((PMailingRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PMailingTable GetChangesTyped()
        {
            return ((PMailingTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PMailing";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_mailing";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMailingCodeDBName()
        {
            return "p_mailing_code_c";
        }

        /// get character length for column
        public static short GetMailingCodeLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingDescriptionDBName()
        {
            return "p_mailing_description_c";
        }

        /// get character length for column
        public static short GetMailingDescriptionLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingDateDBName()
        {
            return "p_mailing_date_d";
        }

        /// get character length for column
        public static short GetMailingDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationGroupCodeDBName()
        {
            return "a_motivation_group_code_c";
        }

        /// get character length for column
        public static short GetMotivationGroupCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationDetailCodeDBName()
        {
            return "a_motivation_detail_code_c";
        }

        /// get character length for column
        public static short GetMotivationDetailCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingCostDBName()
        {
            return "p_mailing_cost_n";
        }

        /// get character length for column
        public static short GetMailingCostLength()
        {
            return 19;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingAttributedAmountDBName()
        {
            return "p_mailing_attributed_amount_n";
        }

        /// get character length for column
        public static short GetMailingAttributedAmountLength()
        {
            return 19;
        }

        /// get the name of the field in the database for this column
        public static string GetViewableDBName()
        {
            return "p_viewable_l";
        }

        /// get character length for column
        public static short GetViewableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetViewableUntilDBName()
        {
            return "p_viewable_until_d";
        }

        /// get character length for column
        public static short GetViewableUntilLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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

        /// This defines a motivation group.
        public String MotivationGroupCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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

        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 80;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDisplayIndexId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PAddressLayoutCode", "p_address_layout_code",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "p_code_c", "Address Layout Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", "Description", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(2, "DisplayIndex", "p_display_index_i", "Display Order Index", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "Comment", "p_comment_c", "Comment", OdbcType.VarChar, 600, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PAddressLayoutCodeRow this[int i]
        {
            get
            {
                return ((PAddressLayoutCodeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PAddressLayoutCodeTable GetChangesTyped()
        {
            return ((PAddressLayoutCodeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PAddressLayoutCode";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_address_layout_code";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetDisplayIndexDBName()
        {
            return "p_display_index_i";
        }

        /// get character length for column
        public static short GetDisplayIndexLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentDBName()
        {
            return "p_comment_c";
        }

        /// get character length for column
        public static short GetCommentLength()
        {
            return 600;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }

        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
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
        public bool IsDisplayIndexNull()
        {
            return this.IsNull(this.myTable.ColumnDisplayIndex);
        }

        /// assign NULL value
        public void SetDisplayIndexNull()
        {
            this.SetNull(this.myTable.ColumnDisplayIndex);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 81;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressLayoutCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressLineNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressLineCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressPromptId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnLockedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 10;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PAddressLayout", "p_address_layout",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "CountryCode", "p_country_code_c", "Country Code", OdbcType.VarChar, 8, true),
                    new TTypedColumnInfo(1, "AddressLayoutCode", "p_address_layout_code_c", "Address Layout Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "AddressLineNumber", "p_address_line_number_i", "Line Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "AddressLineCode", "p_address_line_code_c", "Address Line Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(4, "AddressPrompt", "p_address_prompt_c", "Address Prompt", OdbcType.VarChar, 30, false),
                    new TTypedColumnInfo(5, "Locked", "p_locked_l", "p_locked_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PAddressLayoutRow this[int i]
        {
            get
            {
                return ((PAddressLayoutRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PAddressLayoutTable GetChangesTyped()
        {
            return ((PAddressLayoutTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PAddressLayout";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_address_layout";
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
        public static string GetAddressLayoutCodeDBName()
        {
            return "p_address_layout_code_c";
        }

        /// get character length for column
        public static short GetAddressLayoutCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressLineNumberDBName()
        {
            return "p_address_line_number_i";
        }

        /// get character length for column
        public static short GetAddressLineNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressLineCodeDBName()
        {
            return "p_address_line_code_c";
        }

        /// get character length for column
        public static short GetAddressLineCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressPromptDBName()
        {
            return "p_address_prompt_c";
        }

        /// get character length for column
        public static short GetAddressPromptLength()
        {
            return 30;
        }

        /// get the name of the field in the database for this column
        public static string GetLockedDBName()
        {
            return "p_locked_l";
        }

        /// get character length for column
        public static short GetLockedLength()
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

        ///
        public String AddressLayoutCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLayoutCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsAddressLayoutCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressLayoutCode);
        }

        /// assign NULL value
        public void SetAddressLayoutCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressLayoutCode);
        }

        /// test for NULL value
        public bool IsAddressLineNumberNull()
        {
            return this.IsNull(this.myTable.ColumnAddressLineNumber);
        }

        /// assign NULL value
        public void SetAddressLineNumberNull()
        {
            this.SetNull(this.myTable.ColumnAddressLineNumber);
        }

        /// test for NULL value
        public bool IsAddressLineCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressLineCode);
        }

        /// assign NULL value
        public void SetAddressLineCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressLineCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 82;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementFieldNameId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementTextId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PAddressElement", "p_address_element",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "AddressElementCode", "p_address_element_code_c", "Element Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(1, "AddressElementDescription", "p_address_element_description_c", "Description", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "AddressElementFieldName", "p_address_element_field_name_c", "Field Name", OdbcType.VarChar, 60, false),
                    new TTypedColumnInfo(3, "AddressElementText", "p_address_element_text_c", "Element Text", OdbcType.VarChar, 2, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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

        /// This Code is used to identify the address element.
        public DataColumn ColumnAddressElementCode;
        ///
        public DataColumn ColumnAddressElementDescription;
        /// Address element field name
        public DataColumn ColumnAddressElementFieldName;
        /// This is usually a ""."" or a "";"" or a "","" etc
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
        }

        /// Access a typed row by index
        public PAddressElementRow this[int i]
        {
            get
            {
                return ((PAddressElementRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PAddressElementTable GetChangesTyped()
        {
            return ((PAddressElementTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PAddressElement";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_address_element";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementCodeDBName()
        {
            return "p_address_element_code_c";
        }

        /// get character length for column
        public static short GetAddressElementCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementDescriptionDBName()
        {
            return "p_address_element_description_c";
        }

        /// get character length for column
        public static short GetAddressElementDescriptionLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementFieldNameDBName()
        {
            return "p_address_element_field_name_c";
        }

        /// get character length for column
        public static short GetAddressElementFieldNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementTextDBName()
        {
            return "p_address_element_text_c";
        }

        /// get character length for column
        public static short GetAddressElementTextLength()
        {
            return 2;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// This is usually a ""."" or a "";"" or a "","" etc
        public String AddressElementText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementText.Ordinal];
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsAddressElementCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementCode);
        }

        /// assign NULL value
        public void SetAddressElementCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 83;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressLineCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementPositionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressElementCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PAddressLine", "p_address_line",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "AddressLineCode", "p_address_line_code_c", "Address Line Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "AddressElementPosition", "p_address_element_position_i", "Element Postition", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "AddressElementCode", "p_address_element_code_c", "Element Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PAddressLineRow this[int i]
        {
            get
            {
                return ((PAddressLineRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PAddressLineTable GetChangesTyped()
        {
            return ((PAddressLineTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PAddressLine";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_address_line";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetAddressLineCodeDBName()
        {
            return "p_address_line_code_c";
        }

        /// get character length for column
        public static short GetAddressLineCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementPositionDBName()
        {
            return "p_address_element_position_i";
        }

        /// get character length for column
        public static short GetAddressElementPositionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressElementCodeDBName()
        {
            return "p_address_element_code_c";
        }

        /// get character length for column
        public static short GetAddressElementCodeLength()
        {
            return 24;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsAddressLineCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressLineCode);
        }

        /// assign NULL value
        public void SetAddressLineCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressLineCode);
        }

        /// test for NULL value
        public bool IsAddressElementPositionNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementPosition);
        }

        /// assign NULL value
        public void SetAddressElementPositionNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementPosition);
        }

        /// test for NULL value
        public bool IsAddressElementCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementCode);
        }

        /// assign NULL value
        public void SetAddressElementCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementCode);
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
    /// Eg      German     Herr   Herrn
    /// ""Sehr geehrter Herr Starling"" in the letter and ""Herrn Starling"" in the address.
    [Serializable()]
    public class PAddresseeTitleOverrideTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 84;
        /// used for generic TTypedDataTable functions
        public static short ColumnLanguageCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnTitleId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnTitleOverrideId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PAddresseeTitleOverride", "p_addressee_title_override",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LanguageCode", "p_language_code_c", "Language Code", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "Title", "p_title_c", "Title", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "TitleOverride", "p_title_override_c", "Title Override", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PAddresseeTitleOverrideRow this[int i]
        {
            get
            {
                return ((PAddresseeTitleOverrideRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PAddresseeTitleOverrideTable GetChangesTyped()
        {
            return ((PAddresseeTitleOverrideTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PAddresseeTitleOverride";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_addressee_title_override";
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
        public static string GetTitleDBName()
        {
            return "p_title_c";
        }

        /// get character length for column
        public static short GetTitleLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetTitleOverrideDBName()
        {
            return "p_title_override_c";
        }

        /// get character length for column
        public static short GetTitleOverrideLength()
        {
            return 64;
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

    /// This is used to override titles that might be different in the address than that in the letter.
    /// Eg      German     Herr   Herrn
    /// ""Sehr geehrter Herr Starling"" in the letter and ""Herrn Starling"" in the address.
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

        /// The partner's title
        public String Title
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTitle.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsTitleOverrideNull()
        {
            return this.IsNull(this.myTable.ColumnTitleOverride);
        }

        /// assign NULL value
        public void SetTitleOverrideNull()
        {
            this.SetNull(this.myTable.ColumnTitleOverride);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 85;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnUserIdId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnCustomisedGreetingTextId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCustomisedClosingTextId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PCustomisedGreeting", "p_customised_greeting",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "UserId", "s_user_id_c", "User ID", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(2, "CustomisedGreetingText", "p_customised_greeting_text_c", "Customised Greeting", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(3, "CustomisedClosingText", "p_customised_closing_text_c", "Customised Closing", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PCustomisedGreetingRow this[int i]
        {
            get
            {
                return ((PCustomisedGreetingRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PCustomisedGreetingTable GetChangesTyped()
        {
            return ((PCustomisedGreetingTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PCustomisedGreeting";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_customised_greeting";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }

        /// get character length for column
        public static short GetUserIdLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetCustomisedGreetingTextDBName()
        {
            return "p_customised_greeting_text_c";
        }

        /// get character length for column
        public static short GetCustomisedGreetingTextLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetCustomisedClosingTextDBName()
        {
            return "p_customised_closing_text_c";
        }

        /// get character length for column
        public static short GetCustomisedClosingTextLength()
        {
            return 64;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 86;
        /// used for generic TTypedDataTable functions
        public static short ColumnLanguageCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddresseeTypeCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnFormalityLevelId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnSalutationTextId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnTitleId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnComplimentaryClosingTextId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnPersonalPronounId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 12;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PFormality", "p_formality",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LanguageCode", "p_language_code_c", "Language Code", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "CountryCode", "p_country_code_c", "Country Code", OdbcType.VarChar, 8, true),
                    new TTypedColumnInfo(2, "AddresseeTypeCode", "p_addressee_type_code_c", "Addressee Type Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "FormalityLevel", "p_formality_level_i", "Formality Level", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "SalutationText", "p_salutation_text_c", "Salutation Text", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(5, "Title", "p_title_c", "Title", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(6, "ComplimentaryClosingText", "p_complimentary_closing_text_c", "Closing Text", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(7, "PersonalPronoun", "p_personal_pronoun_c", "Personal Pronoun", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(8, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(12, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

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

        /// This is the code used to identify a language.
        public DataColumn ColumnLanguageCode;
        /// This is a code which identifies a country.
        /// It is taken from the ISO 3166-1-alpha-2 code elements.
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
        }

        /// Access a typed row by index
        public PFormalityRow this[int i]
        {
            get
            {
                return ((PFormalityRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PFormalityTable GetChangesTyped()
        {
            return ((PFormalityTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PFormality";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_formality";
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
        public static string GetAddresseeTypeCodeDBName()
        {
            return "p_addressee_type_code_c";
        }

        /// get character length for column
        public static short GetAddresseeTypeCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetFormalityLevelDBName()
        {
            return "p_formality_level_i";
        }

        /// get character length for column
        public static short GetFormalityLevelLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSalutationTextDBName()
        {
            return "p_salutation_text_c";
        }

        /// get character length for column
        public static short GetSalutationTextLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetTitleDBName()
        {
            return "p_title_c";
        }

        /// get character length for column
        public static short GetTitleLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetComplimentaryClosingTextDBName()
        {
            return "p_complimentary_closing_text_c";
        }

        /// get character length for column
        public static short GetComplimentaryClosingTextLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetPersonalPronounDBName()
        {
            return "p_personal_pronoun_c";
        }

        /// get character length for column
        public static short GetPersonalPronounLength()
        {
            return 24;
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

        /// This is a code which identifies a country.
        /// It is taken from the ISO 3166-1-alpha-2 code elements.
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

        ///
        public String AddresseeTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddresseeTypeCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsAddresseeTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddresseeTypeCode);
        }

        /// assign NULL value
        public void SetAddresseeTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddresseeTypeCode);
        }

        /// test for NULL value
        public bool IsFormalityLevelNull()
        {
            return this.IsNull(this.myTable.ColumnFormalityLevel);
        }

        /// assign NULL value
        public void SetFormalityLevelNull()
        {
            this.SetNull(this.myTable.ColumnFormalityLevel);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 87;
        /// used for generic TTypedDataTable functions
        public static short ColumnBodyNameId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBodyTextId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPhysicalFileId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnOwnerId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PFormLetterBody", "p_form_letter_body",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "BodyName", "p_body_name_c", "Body Name", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(1, "BodyText", "p_body_text_c", "Body Text", OdbcType.VarChar, 30000, false),
                    new TTypedColumnInfo(2, "Description", "p_description_c", "Description", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(3, "PhysicalFile", "p_physical_file_c", "Physical File", OdbcType.VarChar, 48, false),
                    new TTypedColumnInfo(4, "Owner", "p_owner_c", "Owner", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PFormLetterBodyRow this[int i]
        {
            get
            {
                return ((PFormLetterBodyRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PFormLetterBodyTable GetChangesTyped()
        {
            return ((PFormLetterBodyTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PFormLetterBody";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_body";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }

        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetBodyTextDBName()
        {
            return "p_body_text_c";
        }

        /// get character length for column
        public static short GetBodyTextLength()
        {
            return 30000;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetPhysicalFileDBName()
        {
            return "p_physical_file_c";
        }

        /// get character length for column
        public static short GetPhysicalFileLength()
        {
            return 48;
        }

        /// get the name of the field in the database for this column
        public static string GetOwnerDBName()
        {
            return "p_owner_c";
        }

        /// get character length for column
        public static short GetOwnerLength()
        {
            return 20;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        ///
        public String PhysicalFile
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPhysicalFile.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 88;
        /// used for generic TTypedDataTable functions
        public static short ColumnDesignNameId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddressLayoutCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnFormalityLevelId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnBodyNameId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PFormLetterDesign", "p_form_letter_design",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "DesignName", "p_design_name_c", "Design Name", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", "Description", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "AddressLayoutCode", "p_address_layout_code_c", "Address Layout Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(3, "FormalityLevel", "p_formality_level_i", "Formality Level", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "BodyName", "p_body_name_c", "Body Name", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PFormLetterDesignRow this[int i]
        {
            get
            {
                return ((PFormLetterDesignRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PFormLetterDesignTable GetChangesTyped()
        {
            return ((PFormLetterDesignTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PFormLetterDesign";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_design";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetDesignNameDBName()
        {
            return "p_design_name_c";
        }

        /// get character length for column
        public static short GetDesignNameLength()
        {
            return 40;
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
        public static string GetAddressLayoutCodeDBName()
        {
            return "p_address_layout_code_c";
        }

        /// get character length for column
        public static short GetAddressLayoutCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetFormalityLevelDBName()
        {
            return "p_formality_level_i";
        }

        /// get character length for column
        public static short GetFormalityLevelLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }

        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 40;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        ///
        public String AddressLayoutCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLayoutCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsDesignNameNull()
        {
            return this.IsNull(this.myTable.ColumnDesignName);
        }

        /// assign NULL value
        public void SetDesignNameNull()
        {
            this.SetNull(this.myTable.ColumnDesignName);
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
        public bool IsAddressLayoutCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddressLayoutCode);
        }

        /// assign NULL value
        public void SetAddressLayoutCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddressLayoutCode);
        }

        /// test for NULL value
        public bool IsFormalityLevelNull()
        {
            return this.IsNull(this.myTable.ColumnFormalityLevel);
        }

        /// assign NULL value
        public void SetFormalityLevelNull()
        {
            this.SetNull(this.myTable.ColumnFormalityLevel);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 89;
        /// used for generic TTypedDataTable functions
        public static short ColumnSequenceId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtractIdId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnBodyNameId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnInsertId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PFormLetterInsert", "p_form_letter_insert",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Sequence", "p_sequence_i", "p_sequence_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "ExtractId", "m_extract_id_i", "Extract Id", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "BodyName", "p_body_name_c", "Body Name", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(4, "Insert", "p_insert_c", "Inserts", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PFormLetterInsertRow this[int i]
        {
            get
            {
                return ((PFormLetterInsertRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PFormLetterInsertTable GetChangesTyped()
        {
            return ((PFormLetterInsertTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PFormLetterInsert";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_insert";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetSequenceDBName()
        {
            return "p_sequence_i";
        }

        /// get character length for column
        public static short GetSequenceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }

        /// get character length for column
        public static short GetExtractIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }

        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetInsertDBName()
        {
            return "p_insert_c";
        }

        /// get character length for column
        public static short GetInsertLength()
        {
            return 512;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsSequenceNull()
        {
            return this.IsNull(this.myTable.ColumnSequence);
        }

        /// assign NULL value
        public void SetSequenceNull()
        {
            this.SetNull(this.myTable.ColumnSequence);
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
        public bool IsExtractIdNull()
        {
            return this.IsNull(this.myTable.ColumnExtractId);
        }

        /// assign NULL value
        public void SetExtractIdNull()
        {
            this.SetNull(this.myTable.ColumnExtractId);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 90;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnFormNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGapLinesId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnHeightId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnWidthId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnGapColumnsId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnLabelsAcrossId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnLabelsDownId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnStartColumnId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnStartLineId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 15;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PLabel", "p_label",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "p_code_c", "Label Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "FormName", "s_form_name_c", "Form Name", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(2, "GapLines", "p_gap_lines_i", "Lines between labels", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "Height", "p_height_i", "Height in Lines", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "Width", "p_width_i", "Width in Characters", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(5, "GapColumns", "p_gap_columns_i", "Columns between Labels", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "LabelsAcross", "p_labels_across_i", "Number of labels across", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(7, "LabelsDown", "p_labels_down_i", "Number of labels down", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(8, "Description", "p_description_c", "Label Description", OdbcType.VarChar, 70, true),
                    new TTypedColumnInfo(9, "StartColumn", "p_start_column_i", "Column to start printing in", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(10, "StartLine", "p_start_line_i", "Line to start printing on", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(13, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(14, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(15, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PLabelRow this[int i]
        {
            get
            {
                return ((PLabelRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PLabelTable GetChangesTyped()
        {
            return ((PLabelTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PLabel";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_label";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetFormNameDBName()
        {
            return "s_form_name_c";
        }

        /// get character length for column
        public static short GetFormNameLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetGapLinesDBName()
        {
            return "p_gap_lines_i";
        }

        /// get character length for column
        public static short GetGapLinesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHeightDBName()
        {
            return "p_height_i";
        }

        /// get character length for column
        public static short GetHeightLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetWidthDBName()
        {
            return "p_width_i";
        }

        /// get character length for column
        public static short GetWidthLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGapColumnsDBName()
        {
            return "p_gap_columns_i";
        }

        /// get character length for column
        public static short GetGapColumnsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLabelsAcrossDBName()
        {
            return "p_labels_across_i";
        }

        /// get character length for column
        public static short GetLabelsAcrossLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLabelsDownDBName()
        {
            return "p_labels_down_i";
        }

        /// get character length for column
        public static short GetLabelsDownLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 70;
        }

        /// get the name of the field in the database for this column
        public static string GetStartColumnDBName()
        {
            return "p_start_column_i";
        }

        /// get character length for column
        public static short GetStartColumnLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetStartLineDBName()
        {
            return "p_start_line_i";
        }

        /// get character length for column
        public static short GetStartLineLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }

        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
        }

        /// test for NULL value
        public bool IsFormNameNull()
        {
            return this.IsNull(this.myTable.ColumnFormName);
        }

        /// assign NULL value
        public void SetFormNameNull()
        {
            this.SetNull(this.myTable.ColumnFormName);
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
        public bool IsHeightNull()
        {
            return this.IsNull(this.myTable.ColumnHeight);
        }

        /// assign NULL value
        public void SetHeightNull()
        {
            this.SetNull(this.myTable.ColumnHeight);
        }

        /// test for NULL value
        public bool IsWidthNull()
        {
            return this.IsNull(this.myTable.ColumnWidth);
        }

        /// assign NULL value
        public void SetWidthNull()
        {
            this.SetNull(this.myTable.ColumnWidth);
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
        public bool IsLabelsAcrossNull()
        {
            return this.IsNull(this.myTable.ColumnLabelsAcross);
        }

        /// assign NULL value
        public void SetLabelsAcrossNull()
        {
            this.SetNull(this.myTable.ColumnLabelsAcross);
        }

        /// test for NULL value
        public bool IsLabelsDownNull()
        {
            return this.IsNull(this.myTable.ColumnLabelsDown);
        }

        /// assign NULL value
        public void SetLabelsDownNull()
        {
            this.SetNull(this.myTable.ColumnLabelsDown);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 91;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeFormNameId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeFormDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PMergeForm", "p_merge_form",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MergeFormName", "p_merge_form_name_c", "Merge Form", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "MergeFormDescription", "p_merge_form_description_c", "Description", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PMergeFormRow this[int i]
        {
            get
            {
                return ((PMergeFormRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PMergeFormTable GetChangesTyped()
        {
            return ((PMergeFormTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PMergeForm";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_merge_form";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMergeFormNameDBName()
        {
            return "p_merge_form_name_c";
        }

        /// get character length for column
        public static short GetMergeFormNameLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMergeFormDescriptionDBName()
        {
            return "p_merge_form_description_c";
        }

        /// get character length for column
        public static short GetMergeFormDescriptionLength()
        {
            return 64;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
            this.SetNull(this.myTable.ColumnMergeFormName);
            this.SetNull(this.myTable.ColumnMergeFormDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsMergeFormNameNull()
        {
            return this.IsNull(this.myTable.ColumnMergeFormName);
        }

        /// assign NULL value
        public void SetMergeFormNameNull()
        {
            this.SetNull(this.myTable.ColumnMergeFormName);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 92;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeFormNameId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeFieldNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeFieldPositionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeTypeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnMergeParametersId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PMergeField", "p_merge_field",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MergeFormName", "p_merge_form_name_c", "Merge Form", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "MergeFieldName", "p_merge_field_name_c", "Field Name", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "MergeFieldPosition", "p_merge_field_position_i", "p_merge_field_position_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "MergeType", "p_merge_type_c", "Merge Type", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(4, "MergeParameters", "p_merge_parameters_c", "Parameters", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PMergeFieldRow this[int i]
        {
            get
            {
                return ((PMergeFieldRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PMergeFieldTable GetChangesTyped()
        {
            return ((PMergeFieldTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PMergeField";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_merge_field";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMergeFormNameDBName()
        {
            return "p_merge_form_name_c";
        }

        /// get character length for column
        public static short GetMergeFormNameLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMergeFieldNameDBName()
        {
            return "p_merge_field_name_c";
        }

        /// get character length for column
        public static short GetMergeFieldNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetMergeFieldPositionDBName()
        {
            return "p_merge_field_position_i";
        }

        /// get character length for column
        public static short GetMergeFieldPositionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMergeTypeDBName()
        {
            return "p_merge_type_c";
        }

        /// get character length for column
        public static short GetMergeTypeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetMergeParametersDBName()
        {
            return "p_merge_parameters_c";
        }

        /// get character length for column
        public static short GetMergeParametersLength()
        {
            return 512;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsMergeFormNameNull()
        {
            return this.IsNull(this.myTable.ColumnMergeFormName);
        }

        /// assign NULL value
        public void SetMergeFormNameNull()
        {
            this.SetNull(this.myTable.ColumnMergeFormName);
        }

        /// test for NULL value
        public bool IsMergeFieldNameNull()
        {
            return this.IsNull(this.myTable.ColumnMergeFieldName);
        }

        /// assign NULL value
        public void SetMergeFieldNameNull()
        {
            this.SetNull(this.myTable.ColumnMergeFieldName);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 93;
        /// used for generic TTypedDataTable functions
        public static short ColumnRangeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnFromId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnToId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPostcodeRange", "p_postcode_range",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Range", "p_range_c", "Range Name", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(1, "From", "p_from_c", "From", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(2, "To", "p_to_c", "To", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPostcodeRangeRow this[int i]
        {
            get
            {
                return ((PPostcodeRangeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPostcodeRangeTable GetChangesTyped()
        {
            return ((PPostcodeRangeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPostcodeRange";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_postcode_range";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetRangeDBName()
        {
            return "p_range_c";
        }

        /// get character length for column
        public static short GetRangeLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetFromDBName()
        {
            return "p_from_c";
        }

        /// get character length for column
        public static short GetFromLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetToDBName()
        {
            return "p_to_c";
        }

        /// get character length for column
        public static short GetToLength()
        {
            return 40;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsRangeNull()
        {
            return this.IsNull(this.myTable.ColumnRange);
        }

        /// assign NULL value
        public void SetRangeNull()
        {
            this.SetNull(this.myTable.ColumnRange);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 94;
        /// used for generic TTypedDataTable functions
        public static short ColumnRegionId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnRangeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPostcodeRegion", "p_postcode_region",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Region", "p_region_c", "Region Name", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(1, "Range", "p_range_c", "Range Name", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPostcodeRegionRow this[int i]
        {
            get
            {
                return ((PPostcodeRegionRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPostcodeRegionTable GetChangesTyped()
        {
            return ((PPostcodeRegionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPostcodeRegion";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_postcode_region";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetRegionDBName()
        {
            return "p_region_c";
        }

        /// get character length for column
        public static short GetRegionLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetRangeDBName()
        {
            return "p_range_c";
        }

        /// get character length for column
        public static short GetRangeLength()
        {
            return 64;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
            this.SetNull(this.myTable.ColumnRegion);
            this.SetNull(this.myTable.ColumnRange);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsRegionNull()
        {
            return this.IsNull(this.myTable.ColumnRegion);
        }

        /// assign NULL value
        public void SetRegionNull()
        {
            this.SetNull(this.myTable.ColumnRegion);
        }

        /// test for NULL value
        public bool IsRangeNull()
        {
            return this.IsNull(this.myTable.ColumnRange);
        }

        /// assign NULL value
        public void SetRangeNull()
        {
            this.SetNull(this.myTable.ColumnRange);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 95;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfIssuesId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfRemindersId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationDescriptionId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnValidPublicationId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnFrequencyCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationLabelCodeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationLanguageId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 12;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPublication", "p_publication",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PublicationCode", "p_publication_code_c", "Publication Code", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "NumberOfIssues", "p_number_of_issues_i", "Number of Issues", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "NumberOfReminders", "p_number_of_reminders_i", "Reminders", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "PublicationDescription", "p_publication_description_c", "Description", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(4, "ValidPublication", "p_valid_publication_l", "Valid Publication", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(5, "FrequencyCode", "a_frequency_code_c", "Frequency", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(6, "PublicationLabelCode", "p_publication_label_code_c", "Publication Label Code", OdbcType.VarChar, 6, false),
                    new TTypedColumnInfo(7, "PublicationLanguage", "p_publication_language_c", "Language", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(12, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPublicationRow this[int i]
        {
            get
            {
                return ((PPublicationRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPublicationTable GetChangesTyped()
        {
            return ((PPublicationTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPublication";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_publication";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }

        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfIssuesDBName()
        {
            return "p_number_of_issues_i";
        }

        /// get character length for column
        public static short GetNumberOfIssuesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfRemindersDBName()
        {
            return "p_number_of_reminders_i";
        }

        /// get character length for column
        public static short GetNumberOfRemindersLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationDescriptionDBName()
        {
            return "p_publication_description_c";
        }

        /// get character length for column
        public static short GetPublicationDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetValidPublicationDBName()
        {
            return "p_valid_publication_l";
        }

        /// get character length for column
        public static short GetValidPublicationLength()
        {
            return -1;
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
        public static string GetPublicationLabelCodeDBName()
        {
            return "p_publication_label_code_c";
        }

        /// get character length for column
        public static short GetPublicationLabelCodeLength()
        {
            return 6;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationLanguageDBName()
        {
            return "p_publication_language_c";
        }

        /// get character length for column
        public static short GetPublicationLanguageLength()
        {
            return 20;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The publication short code that is used on an address label
        public String PublicationLabelCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationLabelCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsPublicationCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationCode);
        }

        /// assign NULL value
        public void SetPublicationCodeNull()
        {
            this.SetNull(this.myTable.ColumnPublicationCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 96;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEffectiveId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationCostId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPostageCostId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCurrencyCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPublicationCost", "p_publication_cost",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PublicationCode", "p_publication_code_c", "Publication Code", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "DateEffective", "p_date_effective_d", "Date Effective", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "PublicationCost", "p_publication_cost_n", "Publication Cost", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(3, "PostageCost", "p_postage_cost_n", "Postage", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(4, "CurrencyCode", "p_currency_code_c", "Currency Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPublicationCostRow this[int i]
        {
            get
            {
                return ((PPublicationCostRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPublicationCostTable GetChangesTyped()
        {
            return ((PPublicationCostTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPublicationCost";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_publication_cost";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }

        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateEffectiveDBName()
        {
            return "p_date_effective_d";
        }

        /// get character length for column
        public static short GetDateEffectiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationCostDBName()
        {
            return "p_publication_cost_n";
        }

        /// get character length for column
        public static short GetPublicationCostLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetPostageCostDBName()
        {
            return "p_postage_cost_n";
        }

        /// get character length for column
        public static short GetPostageCostLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "p_currency_code_c";
        }

        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 16;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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

        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsPublicationCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationCode);
        }

        /// assign NULL value
        public void SetPublicationCodeNull()
        {
            this.SetNull(this.myTable.ColumnPublicationCode);
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
        public bool IsPublicationCostNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationCost);
        }

        /// assign NULL value
        public void SetPublicationCostNull()
        {
            this.SetNull(this.myTable.ColumnPublicationCost);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 97;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PReasonSubscriptionGiven", "p_reason_subscription_given",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "p_code_c", "Reason Given Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", "Description", OdbcType.VarChar, 160, true),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PReasonSubscriptionGivenRow this[int i]
        {
            get
            {
                return ((PReasonSubscriptionGivenRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PReasonSubscriptionGivenTable GetChangesTyped()
        {
            return ((PReasonSubscriptionGivenTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PReasonSubscriptionGiven";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_reason_subscription_given";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 160;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }

        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
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

    /// List of reasons for cancelling a subscription
    [Serializable()]
    public class PReasonSubscriptionCancelledTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 98;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PReasonSubscriptionCancelled", "p_reason_subscription_cancelled",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "p_code_c", "Reason Cancelled Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", "Reason", OdbcType.VarChar, 160, true),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PReasonSubscriptionCancelledRow this[int i]
        {
            get
            {
                return ((PReasonSubscriptionCancelledRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PReasonSubscriptionCancelledTable GetChangesTyped()
        {
            return ((PReasonSubscriptionCancelledTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PReasonSubscriptionCancelled";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_reason_subscription_cancelled";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 160;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }

        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
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

    /// Details of which partners receive which publications.
    [Serializable()]
    public class PSubscriptionTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 99;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPublicationCopiesId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnReasonSubsGivenCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnReasonSubsCancelledCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnExpiryDateId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnProvisionalExpiryDateId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnGratisSubscriptionId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateNoticeSentId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCancelledId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnStartDateId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberIssuesReceivedId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberComplimentaryId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnSubscriptionRenewalDateId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnSubscriptionStatusId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnFirstIssueId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastIssueId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftFromKeyId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 22;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PSubscription", "p_subscription",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PublicationCode", "p_publication_code_c", "Publication Code", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "PublicationCopies", "p_publication_copies_i", "Copies", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "ReasonSubsGivenCode", "p_reason_subs_given_code_c", "Reason Given", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(4, "ReasonSubsCancelledCode", "p_reason_subs_cancelled_code_c", "Reason Cancelled", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(5, "ExpiryDate", "p_expiry_date_d", "Expiry Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ProvisionalExpiryDate", "p_provisional_expiry_date_d", "Provisional Expiry Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "GratisSubscription", "p_gratis_subscription_l", "Free Subscription", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(8, "DateNoticeSent", "p_date_notice_sent_d", "Date Notice Sent", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "DateCancelled", "p_date_cancelled_d", "Date Cancelled", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "StartDate", "p_start_date_d", "Start Date", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(11, "NumberIssuesReceived", "p_number_issues_received_i", "Issues Received", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(12, "NumberComplimentary", "p_number_complimentary_i", "Complimentary", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(13, "SubscriptionRenewalDate", "p_subscription_renewal_date_d", "Renewal Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(14, "SubscriptionStatus", "p_subscription_status_c", "Status", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(15, "FirstIssue", "p_first_issue_d", "First Issue", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(16, "LastIssue", "p_last_issue_d", "Last Issue", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "GiftFromKey", "p_gift_from_key_n", "Gift Given By", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(18, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(19, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(20, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(21, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(22, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PSubscriptionRow this[int i]
        {
            get
            {
                return ((PSubscriptionRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PSubscriptionTable GetChangesTyped()
        {
            return ((PSubscriptionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PSubscription";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_subscription";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }

        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicationCopiesDBName()
        {
            return "p_publication_copies_i";
        }

        /// get character length for column
        public static short GetPublicationCopiesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReasonSubsGivenCodeDBName()
        {
            return "p_reason_subs_given_code_c";
        }

        /// get character length for column
        public static short GetReasonSubsGivenCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetReasonSubsCancelledCodeDBName()
        {
            return "p_reason_subs_cancelled_code_c";
        }

        /// get character length for column
        public static short GetReasonSubsCancelledCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetExpiryDateDBName()
        {
            return "p_expiry_date_d";
        }

        /// get character length for column
        public static short GetExpiryDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetProvisionalExpiryDateDBName()
        {
            return "p_provisional_expiry_date_d";
        }

        /// get character length for column
        public static short GetProvisionalExpiryDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGratisSubscriptionDBName()
        {
            return "p_gratis_subscription_l";
        }

        /// get character length for column
        public static short GetGratisSubscriptionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateNoticeSentDBName()
        {
            return "p_date_notice_sent_d";
        }

        /// get character length for column
        public static short GetDateNoticeSentLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCancelledDBName()
        {
            return "p_date_cancelled_d";
        }

        /// get character length for column
        public static short GetDateCancelledLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetStartDateDBName()
        {
            return "p_start_date_d";
        }

        /// get character length for column
        public static short GetStartDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberIssuesReceivedDBName()
        {
            return "p_number_issues_received_i";
        }

        /// get character length for column
        public static short GetNumberIssuesReceivedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberComplimentaryDBName()
        {
            return "p_number_complimentary_i";
        }

        /// get character length for column
        public static short GetNumberComplimentaryLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSubscriptionRenewalDateDBName()
        {
            return "p_subscription_renewal_date_d";
        }

        /// get character length for column
        public static short GetSubscriptionRenewalDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSubscriptionStatusDBName()
        {
            return "p_subscription_status_c";
        }

        /// get character length for column
        public static short GetSubscriptionStatusLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetFirstIssueDBName()
        {
            return "p_first_issue_d";
        }

        /// get character length for column
        public static short GetFirstIssueLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastIssueDBName()
        {
            return "p_last_issue_d";
        }

        /// get character length for column
        public static short GetLastIssueLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftFromKeyDBName()
        {
            return "p_gift_from_key_n";
        }

        /// get character length for column
        public static short GetGiftFromKeyLength()
        {
            return 10;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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

        /// Provisional date on which the subscription may expire
        public System.DateTime ProvisionalExpiryDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnProvisionalExpiryDate.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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

        ///
        public System.DateTime DateCancelled
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCancelled.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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

        ///
        public System.DateTime StartDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStartDate.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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

        ///
        public String SubscriptionStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSubscriptionStatus.Ordinal];
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
                    return DateTime.MinValue;
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

        ///
        public System.DateTime LastIssue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastIssue.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsPublicationCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationCode);
        }

        /// assign NULL value
        public void SetPublicationCodeNull()
        {
            this.SetNull(this.myTable.ColumnPublicationCode);
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
        public bool IsReasonSubsGivenCodeNull()
        {
            return this.IsNull(this.myTable.ColumnReasonSubsGivenCode);
        }

        /// assign NULL value
        public void SetReasonSubsGivenCodeNull()
        {
            this.SetNull(this.myTable.ColumnReasonSubsGivenCode);
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
        public bool IsGratisSubscriptionNull()
        {
            return this.IsNull(this.myTable.ColumnGratisSubscription);
        }

        /// assign NULL value
        public void SetGratisSubscriptionNull()
        {
            this.SetNull(this.myTable.ColumnGratisSubscription);
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
        public bool IsStartDateNull()
        {
            return this.IsNull(this.myTable.ColumnStartDate);
        }

        /// assign NULL value
        public void SetStartDateNull()
        {
            this.SetNull(this.myTable.ColumnStartDate);
        }

        /// test for NULL value
        public bool IsNumberIssuesReceivedNull()
        {
            return this.IsNull(this.myTable.ColumnNumberIssuesReceived);
        }

        /// assign NULL value
        public void SetNumberIssuesReceivedNull()
        {
            this.SetNull(this.myTable.ColumnNumberIssuesReceived);
        }

        /// test for NULL value
        public bool IsNumberComplimentaryNull()
        {
            return this.IsNull(this.myTable.ColumnNumberComplimentary);
        }

        /// assign NULL value
        public void SetNumberComplimentaryNull()
        {
            this.SetNull(this.myTable.ColumnNumberComplimentary);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 100;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttributeCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttributeDescrId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnActiveId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PContactAttribute", "p_contact_attribute",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ContactAttributeCode", "p_contact_attribute_code_c", "Contact Attribute Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(1, "ContactAttributeDescr", "p_contact_attribute_descr_c", "Description", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "Active", "p_active_l", "Active", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PContactAttributeRow this[int i]
        {
            get
            {
                return ((PContactAttributeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PContactAttributeTable GetChangesTyped()
        {
            return ((PContactAttributeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PContactAttribute";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_contact_attribute";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }

        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttributeDescrDBName()
        {
            return "p_contact_attribute_descr_c";
        }

        /// get character length for column
        public static short GetContactAttributeDescrLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "p_active_l";
        }

        /// get character length for column
        public static short GetActiveLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsContactAttributeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttributeCode);
        }

        /// assign NULL value
        public void SetContactAttributeCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactAttributeCode);
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

    /// Possible attribute details for each contact attribute.  Breaks down the attribute into more specifice information that applies to a contact with a partner.
    [Serializable()]
    public class PContactAttributeDetailTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 101;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttributeCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttrDetailCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttrDetailDescrId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnActiveId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PContactAttributeDetail", "p_contact_attribute_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ContactAttributeCode", "p_contact_attribute_code_c", "Contact Attribute Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(1, "ContactAttrDetailCode", "p_contact_attr_detail_code_c", "Attribute Detail Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "ContactAttrDetailDescr", "p_contact_attr_detail_descr_c", "Description", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(3, "Active", "p_active_l", "Active", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "Comment", "p_comment_c", "Comment", OdbcType.VarChar, 8000, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PContactAttributeDetailRow this[int i]
        {
            get
            {
                return ((PContactAttributeDetailRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PContactAttributeDetailTable GetChangesTyped()
        {
            return ((PContactAttributeDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PContactAttributeDetail";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_contact_attribute_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }

        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailCodeDBName()
        {
            return "p_contact_attr_detail_code_c";
        }

        /// get character length for column
        public static short GetContactAttrDetailCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailDescrDBName()
        {
            return "p_contact_attr_detail_descr_c";
        }

        /// get character length for column
        public static short GetContactAttrDetailDescrLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "p_active_l";
        }

        /// get character length for column
        public static short GetActiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentDBName()
        {
            return "p_comment_c";
        }

        /// get character length for column
        public static short GetCommentLength()
        {
            return 8000;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsContactAttributeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttributeCode);
        }

        /// assign NULL value
        public void SetContactAttributeCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactAttributeCode);
        }

        /// test for NULL value
        public bool IsContactAttrDetailCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttrDetailCode);
        }

        /// assign NULL value
        public void SetContactAttrDetailCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactAttrDetailCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 102;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfContactCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescriptionId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactTypeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnValidMethodId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDeletableId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PMethodOfContact", "p_method_of_contact",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MethodOfContactCode", "p_method_of_contact_code_c", "Method of Contact", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", "Description", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "ContactType", "p_contact_type_c", "Contact Type", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(3, "ValidMethod", "p_valid_method_l", "Valid Method", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "Deletable", "p_deletable_l", "Deletable", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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

        ///
        public DataColumn ColumnMethodOfContactCode;
        ///
        public DataColumn ColumnDescription;
        ///
        public DataColumn ColumnContactType;
        ///
        public DataColumn ColumnValidMethod;
        /// This defines if the method of contact code can be deleted.
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
        }

        /// Access a typed row by index
        public PMethodOfContactRow this[int i]
        {
            get
            {
                return ((PMethodOfContactRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PMethodOfContactTable GetChangesTyped()
        {
            return ((PMethodOfContactTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PMethodOfContact";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_method_of_contact";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfContactCodeDBName()
        {
            return "p_method_of_contact_code_c";
        }

        /// get character length for column
        public static short GetMethodOfContactCodeLength()
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
        public static string GetContactTypeDBName()
        {
            return "p_contact_type_c";
        }

        /// get character length for column
        public static short GetContactTypeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetValidMethodDBName()
        {
            return "p_valid_method_l";
        }

        /// get character length for column
        public static short GetValidMethodLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        ///
        public String ContactType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactType.Ordinal];
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsMethodOfContactCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfContactCode);
        }

        /// assign NULL value
        public void SetMethodOfContactCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfContactCode);
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
        public bool IsValidMethodNull()
        {
            return this.IsNull(this.myTable.ColumnValidMethod);
        }

        /// assign NULL value
        public void SetValidMethodNull()
        {
            this.SetNull(this.myTable.ColumnValidMethod);
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

    /// Details of contacts with partners
    [Serializable()]
    public class PPartnerContactTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 103;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactDateId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactTimeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactorId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactMessageIdId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactCommentId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModuleIdId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnUserIdId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCodeId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnRestrictedId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactLocationId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 17;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPartnerContact", "p_partner_contact",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ContactId", "p_contact_id_i", "Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "ContactDate", "s_contact_date_d", "Contact Date", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(3, "ContactTime", "s_contact_time_i", "s_contact_time_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "ContactCode", "p_contact_code_c", "Contact Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "Contactor", "p_contactor_c", "User ID", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ContactMessageId", "p_contact_message_id_c", "Message ID", OdbcType.VarChar, 200, false),
                    new TTypedColumnInfo(7, "ContactComment", "p_contact_comment_c", "Description", OdbcType.VarChar, 30000, false),
                    new TTypedColumnInfo(8, "ModuleId", "s_module_id_c", "Module ID", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "UserId", "s_user_id_c", "", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(11, "Restricted", "p_restricted_l", "Contact Restricted", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "ContactLocation", "p_contact_location_c", "Location", OdbcType.VarChar, 8000, false),
                    new TTypedColumnInfo(13, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(14, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(15, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(16, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(17, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPartnerContactRow this[int i]
        {
            get
            {
                return ((PPartnerContactRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPartnerContactTable GetChangesTyped()
        {
            return ((PPartnerContactTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPartnerContact";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_partner_contact";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetContactIdDBName()
        {
            return "p_contact_id_i";
        }

        /// get character length for column
        public static short GetContactIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetContactDateDBName()
        {
            return "s_contact_date_d";
        }

        /// get character length for column
        public static short GetContactDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetContactTimeDBName()
        {
            return "s_contact_time_i";
        }

        /// get character length for column
        public static short GetContactTimeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetContactCodeDBName()
        {
            return "p_contact_code_c";
        }

        /// get character length for column
        public static short GetContactCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetContactorDBName()
        {
            return "p_contactor_c";
        }

        /// get character length for column
        public static short GetContactorLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetContactMessageIdDBName()
        {
            return "p_contact_message_id_c";
        }

        /// get character length for column
        public static short GetContactMessageIdLength()
        {
            return 200;
        }

        /// get the name of the field in the database for this column
        public static string GetContactCommentDBName()
        {
            return "p_contact_comment_c";
        }

        /// get character length for column
        public static short GetContactCommentLength()
        {
            return 30000;
        }

        /// get the name of the field in the database for this column
        public static string GetModuleIdDBName()
        {
            return "s_module_id_c";
        }

        /// get character length for column
        public static short GetModuleIdLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }

        /// get character length for column
        public static short GetUserIdLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingCodeDBName()
        {
            return "p_mailing_code_c";
        }

        /// get character length for column
        public static short GetMailingCodeLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "p_restricted_l";
        }

        /// get character length for column
        public static short GetRestrictedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetContactLocationDBName()
        {
            return "p_contact_location_c";
        }

        /// get character length for column
        public static short GetContactLocationLength()
        {
            return 8000;
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
                    return DateTime.MinValue;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsContactIdNull()
        {
            return this.IsNull(this.myTable.ColumnContactId);
        }

        /// assign NULL value
        public void SetContactIdNull()
        {
            this.SetNull(this.myTable.ColumnContactId);
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
        public bool IsContactDateNull()
        {
            return this.IsNull(this.myTable.ColumnContactDate);
        }

        /// assign NULL value
        public void SetContactDateNull()
        {
            this.SetNull(this.myTable.ColumnContactDate);
        }

        /// test for NULL value
        public bool IsContactTimeNull()
        {
            return this.IsNull(this.myTable.ColumnContactTime);
        }

        /// assign NULL value
        public void SetContactTimeNull()
        {
            this.SetNull(this.myTable.ColumnContactTime);
        }

        /// test for NULL value
        public bool IsContactCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactCode);
        }

        /// assign NULL value
        public void SetContactCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 104;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttributeCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactAttrDetailCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PPartnerContactAttribute", "p_partner_contact_attribute",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ContactId", "p_contact_id_i", "Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ContactAttributeCode", "p_contact_attribute_code_c", "Contact Attribute Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "ContactAttrDetailCode", "p_contact_attr_detail_code_c", "Attribute Detail Code", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PPartnerContactAttributeRow this[int i]
        {
            get
            {
                return ((PPartnerContactAttributeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PPartnerContactAttributeTable GetChangesTyped()
        {
            return ((PPartnerContactAttributeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PPartnerContactAttribute";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "p_partner_contact_attribute";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetContactIdDBName()
        {
            return "p_contact_id_i";
        }

        /// get character length for column
        public static short GetContactIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }

        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailCodeDBName()
        {
            return "p_contact_attr_detail_code_c";
        }

        /// get character length for column
        public static short GetContactAttrDetailCodeLength()
        {
            return 32;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    return DateTime.MinValue;
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
                    return DateTime.MinValue;
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
        public bool IsContactIdNull()
        {
            return this.IsNull(this.myTable.ColumnContactId);
        }

        /// assign NULL value
        public void SetContactIdNull()
        {
            this.SetNull(this.myTable.ColumnContactId);
        }

        /// test for NULL value
        public bool IsContactAttributeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttributeCode);
        }

        /// assign NULL value
        public void SetContactAttributeCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactAttributeCode);
        }

        /// test for NULL value
        public bool IsContactAttrDetailCodeNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttrDetailCode);
        }

        /// assign NULL value
        public void SetContactAttrDetailCodeNull()
        {
            this.SetNull(this.myTable.ColumnContactAttrDetailCode);
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