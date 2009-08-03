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
namespace Ict.Petra.Shared.MConference.Data
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

    /// Basic details about a conference
    [Serializable()]
    public class PcConferenceTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        ///
        public DataColumn ColumnXyzTbdPrefix;
        ///
        public DataColumn ColumnStart;
        ///
        public DataColumn ColumnEnd;
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 266;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcConference", "pc_conference",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "XyzTbdPrefix", "pc_xyz_tbd_prefix_c", OdbcType.VarChar, 10, false),
                    new TTypedColumnInfo(2, "Start", "pc_start_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "End", "pc_end_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

        /// constructor
        public PcConferenceTable() :
                base("PcConference")
        {
        }

        /// constructor
        public PcConferenceTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcConferenceTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_xyz_tbd_prefix_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_start_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_end_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnXyzTbdPrefix = this.Columns["pc_xyz_tbd_prefix_c"];
            this.ColumnStart = this.Columns["pc_start_d"];
            this.ColumnEnd = this.Columns["pc_end_d"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcConferenceRow this[int i]
        {
            get
            {
                return ((PcConferenceRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcConferenceRow NewRowTyped(bool AWithDefaultValues)
        {
            PcConferenceRow ret = ((PcConferenceRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcConferenceRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcConferenceRow(builder);
        }

        /// get typed set of changes
        public PcConferenceTable GetChangesTyped()
        {
            return ((PcConferenceTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetXyzTbdPrefixDBName()
        {
            return "pc_xyz_tbd_prefix_c";
        }

        /// get character length for column
        public static short GetXyzTbdPrefixLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetStartDBName()
        {
            return "pc_start_d";
        }

        /// get character length for column
        public static short GetStartLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEndDBName()
        {
            return "pc_end_d";
        }

        /// get character length for column
        public static short GetEndLength()
        {
            return -1;
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

    /// Basic details about a conference
    [Serializable()]
    public class PcConferenceRow : System.Data.DataRow
    {
        private PcConferenceTable myTable;

        /// Constructor
        public PcConferenceRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcConferenceTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        ///
        public String XyzTbdPrefix
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnXyzTbdPrefix.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnXyzTbdPrefix)
                            || (((String)(this[this.myTable.ColumnXyzTbdPrefix])) != value)))
                {
                    this[this.myTable.ColumnXyzTbdPrefix] = value;
                }
            }
        }

        ///
        public System.DateTime Start
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStart.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStart)
                            || (((System.DateTime)(this[this.myTable.ColumnStart])) != value)))
                {
                    this[this.myTable.ColumnStart] = value;
                }
            }
        }

        ///
        public System.DateTime End
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEnd.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEnd)
                            || (((System.DateTime)(this[this.myTable.ColumnEnd])) != value)))
                {
                    this[this.myTable.ColumnEnd] = value;
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
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnXyzTbdPrefix);
            this.SetNull(this.myTable.ColumnStart);
            this.SetNull(this.myTable.ColumnEnd);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsXyzTbdPrefixNull()
        {
            return this.IsNull(this.myTable.ColumnXyzTbdPrefix);
        }

        /// assign NULL value
        public void SetXyzTbdPrefixNull()
        {
            this.SetNull(this.myTable.ColumnXyzTbdPrefix);
        }

        /// test for NULL value
        public bool IsStartNull()
        {
            return this.IsNull(this.myTable.ColumnStart);
        }

        /// assign NULL value
        public void SetStartNull()
        {
            this.SetNull(this.myTable.ColumnStart);
        }

        /// test for NULL value
        public bool IsEndNull()
        {
            return this.IsNull(this.myTable.ColumnEnd);
        }

        /// assign NULL value
        public void SetEndNull()
        {
            this.SetNull(this.myTable.ColumnEnd);
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

    /// Cost types to be used for conference (extra) charges
    [Serializable()]
    public class PcCostTypeTable : TTypedDataTable
    {
        /// Unique name of the cost type
        public DataColumn ColumnCostTypeCode;
        /// Description of the cost type
        public DataColumn ColumnCostTypeDescription;
        /// Can this cost type be assigned?
        public DataColumn ColumnUnassignableFlag;
        /// This is the date the record was last updated.
        public DataColumn ColumnUnassignableDate;
        /// Indicates if a record can be deleted.
        public DataColumn ColumnDeletableFlag;
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
        public static short TableId = 267;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcCostType", "pc_cost_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "CostTypeCode", "pc_cost_type_code_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(1, "CostTypeDescription", "pc_cost_type_description_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(2, "UnassignableFlag", "pc_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "UnassignableDate", "pc_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "DeletableFlag", "pc_deletable_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

        /// constructor
        public PcCostTypeTable() :
                base("PcCostType")
        {
        }

        /// constructor
        public PcCostTypeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcCostTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_cost_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_cost_type_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCostTypeCode = this.Columns["pc_cost_type_code_c"];
            this.ColumnCostTypeDescription = this.Columns["pc_cost_type_description_c"];
            this.ColumnUnassignableFlag = this.Columns["pc_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pc_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pc_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcCostTypeRow this[int i]
        {
            get
            {
                return ((PcCostTypeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcCostTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            PcCostTypeRow ret = ((PcCostTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcCostTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcCostTypeRow(builder);
        }

        /// get typed set of changes
        public PcCostTypeTable GetChangesTyped()
        {
            return ((PcCostTypeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCostTypeCodeDBName()
        {
            return "pc_cost_type_code_c";
        }

        /// get character length for column
        public static short GetCostTypeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetCostTypeDescriptionDBName()
        {
            return "pc_cost_type_description_c";
        }

        /// get character length for column
        public static short GetCostTypeDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pc_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pc_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pc_deletable_flag_l";
        }

        /// get character length for column
        public static short GetDeletableFlagLength()
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

    /// Cost types to be used for conference (extra) charges
    [Serializable()]
    public class PcCostTypeRow : System.Data.DataRow
    {
        private PcCostTypeTable myTable;

        /// Constructor
        public PcCostTypeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcCostTypeTable)(this.Table));
        }

        /// Unique name of the cost type
        public String CostTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostTypeCode)
                            || (((String)(this[this.myTable.ColumnCostTypeCode])) != value)))
                {
                    this[this.myTable.ColumnCostTypeCode] = value;
                }
            }
        }

        /// Description of the cost type
        public String CostTypeDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostTypeDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostTypeDescription)
                            || (((String)(this[this.myTable.ColumnCostTypeDescription])) != value)))
                {
                    this[this.myTable.ColumnCostTypeDescription] = value;
                }
            }
        }

        /// Can this cost type be assigned?
        public Boolean UnassignableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableFlag)
                            || (((Boolean)(this[this.myTable.ColumnUnassignableFlag])) != value)))
                {
                    this[this.myTable.ColumnUnassignableFlag] = value;
                }
            }
        }

        /// This is the date the record was last updated.
        public System.DateTime UnassignableDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableDate)
                            || (((System.DateTime)(this[this.myTable.ColumnUnassignableDate])) != value)))
                {
                    this[this.myTable.ColumnUnassignableDate] = value;
                }
            }
        }

        /// Indicates if a record can be deleted.
        public Boolean DeletableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletableFlag)
                            || (((Boolean)(this[this.myTable.ColumnDeletableFlag])) != value)))
                {
                    this[this.myTable.ColumnDeletableFlag] = value;
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
            this.SetNull(this.myTable.ColumnCostTypeCode);
            this.SetNull(this.myTable.ColumnCostTypeDescription);
            this[this.myTable.ColumnUnassignableFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnUnassignableDate);
            this[this.myTable.ColumnDeletableFlag.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsCostTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCostTypeCode);
        }

        /// assign NULL value
        public void SetCostTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnCostTypeCode);
        }

        /// test for NULL value
        public bool IsCostTypeDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnCostTypeDescription);
        }

        /// assign NULL value
        public void SetCostTypeDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnCostTypeDescription);
        }

        /// test for NULL value
        public bool IsUnassignableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableFlag);
        }

        /// assign NULL value
        public void SetUnassignableFlagNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableFlag);
        }

        /// test for NULL value
        public bool IsUnassignableDateNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableDate);
        }

        /// assign NULL value
        public void SetUnassignableDateNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableDate);
        }

        /// test for NULL value
        public bool IsDeletableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnDeletableFlag);
        }

        /// assign NULL value
        public void SetDeletableFlagNull()
        {
            this.SetNull(this.myTable.ColumnDeletableFlag);
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

    /// Lists types of options that can be used for a conference
    [Serializable()]
    public class PcConferenceOptionTypeTable : TTypedDataTable
    {
        /// Unique name of the cost type
        public DataColumn ColumnOptionTypeCode;
        /// Description of the option type
        public DataColumn ColumnOptionTypeDescription;
        ///
        public DataColumn ColumnOptionTypeComment;
        /// Can this option type be assigned?
        public DataColumn ColumnUnassignableFlag;
        /// This is the date the record was last updated.
        public DataColumn ColumnUnassignableDate;
        /// Indicates if a record can be deleted.
        public DataColumn ColumnDeletableFlag;
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
        public static short TableId = 268;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcConferenceOptionType", "pc_conference_option_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "OptionTypeCode", "pc_option_type_code_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(1, "OptionTypeDescription", "pc_option_type_description_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(2, "OptionTypeComment", "pc_option_type_comment_c", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(3, "UnassignableFlag", "pc_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "UnassignableDate", "pc_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "DeletableFlag", "pc_deletable_flag_l", OdbcType.Bit, -1, false),
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
        public PcConferenceOptionTypeTable() :
                base("PcConferenceOptionType")
        {
        }

        /// constructor
        public PcConferenceOptionTypeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcConferenceOptionTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_option_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_option_type_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_option_type_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnOptionTypeCode = this.Columns["pc_option_type_code_c"];
            this.ColumnOptionTypeDescription = this.Columns["pc_option_type_description_c"];
            this.ColumnOptionTypeComment = this.Columns["pc_option_type_comment_c"];
            this.ColumnUnassignableFlag = this.Columns["pc_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pc_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pc_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcConferenceOptionTypeRow this[int i]
        {
            get
            {
                return ((PcConferenceOptionTypeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcConferenceOptionTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            PcConferenceOptionTypeRow ret = ((PcConferenceOptionTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcConferenceOptionTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcConferenceOptionTypeRow(builder);
        }

        /// get typed set of changes
        public PcConferenceOptionTypeTable GetChangesTyped()
        {
            return ((PcConferenceOptionTypeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetOptionTypeCodeDBName()
        {
            return "pc_option_type_code_c";
        }

        /// get character length for column
        public static short GetOptionTypeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetOptionTypeDescriptionDBName()
        {
            return "pc_option_type_description_c";
        }

        /// get character length for column
        public static short GetOptionTypeDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetOptionTypeCommentDBName()
        {
            return "pc_option_type_comment_c";
        }

        /// get character length for column
        public static short GetOptionTypeCommentLength()
        {
            return 512;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pc_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pc_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pc_deletable_flag_l";
        }

        /// get character length for column
        public static short GetDeletableFlagLength()
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

    /// Lists types of options that can be used for a conference
    [Serializable()]
    public class PcConferenceOptionTypeRow : System.Data.DataRow
    {
        private PcConferenceOptionTypeTable myTable;

        /// Constructor
        public PcConferenceOptionTypeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcConferenceOptionTypeTable)(this.Table));
        }

        /// Unique name of the cost type
        public String OptionTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionTypeCode)
                            || (((String)(this[this.myTable.ColumnOptionTypeCode])) != value)))
                {
                    this[this.myTable.ColumnOptionTypeCode] = value;
                }
            }
        }

        /// Description of the option type
        public String OptionTypeDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionTypeDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionTypeDescription)
                            || (((String)(this[this.myTable.ColumnOptionTypeDescription])) != value)))
                {
                    this[this.myTable.ColumnOptionTypeDescription] = value;
                }
            }
        }

        ///
        public String OptionTypeComment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionTypeComment.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionTypeComment)
                            || (((String)(this[this.myTable.ColumnOptionTypeComment])) != value)))
                {
                    this[this.myTable.ColumnOptionTypeComment] = value;
                }
            }
        }

        /// Can this option type be assigned?
        public Boolean UnassignableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableFlag)
                            || (((Boolean)(this[this.myTable.ColumnUnassignableFlag])) != value)))
                {
                    this[this.myTable.ColumnUnassignableFlag] = value;
                }
            }
        }

        /// This is the date the record was last updated.
        public System.DateTime UnassignableDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableDate)
                            || (((System.DateTime)(this[this.myTable.ColumnUnassignableDate])) != value)))
                {
                    this[this.myTable.ColumnUnassignableDate] = value;
                }
            }
        }

        /// Indicates if a record can be deleted.
        public Boolean DeletableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletableFlag)
                            || (((Boolean)(this[this.myTable.ColumnDeletableFlag])) != value)))
                {
                    this[this.myTable.ColumnDeletableFlag] = value;
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
            this.SetNull(this.myTable.ColumnOptionTypeCode);
            this.SetNull(this.myTable.ColumnOptionTypeDescription);
            this.SetNull(this.myTable.ColumnOptionTypeComment);
            this[this.myTable.ColumnUnassignableFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnUnassignableDate);
            this[this.myTable.ColumnDeletableFlag.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsOptionTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnOptionTypeCode);
        }

        /// assign NULL value
        public void SetOptionTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnOptionTypeCode);
        }

        /// test for NULL value
        public bool IsOptionTypeDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnOptionTypeDescription);
        }

        /// assign NULL value
        public void SetOptionTypeDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnOptionTypeDescription);
        }

        /// test for NULL value
        public bool IsOptionTypeCommentNull()
        {
            return this.IsNull(this.myTable.ColumnOptionTypeComment);
        }

        /// assign NULL value
        public void SetOptionTypeCommentNull()
        {
            this.SetNull(this.myTable.ColumnOptionTypeComment);
        }

        /// test for NULL value
        public bool IsUnassignableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableFlag);
        }

        /// assign NULL value
        public void SetUnassignableFlagNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableFlag);
        }

        /// test for NULL value
        public bool IsUnassignableDateNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableDate);
        }

        /// assign NULL value
        public void SetUnassignableDateNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableDate);
        }

        /// test for NULL value
        public bool IsDeletableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnDeletableFlag);
        }

        /// assign NULL value
        public void SetDeletableFlagNull()
        {
            this.SetNull(this.myTable.ColumnDeletableFlag);
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

    /// Lists options that are set for a conference
    [Serializable()]
    public class PcConferenceOptionTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// Unique name of the cost type
        public DataColumn ColumnOptionTypeCode;
        /// Description of the option type
        public DataColumn ColumnOptionSet;
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
        public static short TableId = 269;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcConferenceOption", "pc_conference_option",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "OptionTypeCode", "pc_option_type_code_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "OptionSet", "pc_option_set_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcConferenceOptionTable() :
                base("PcConferenceOption")
        {
        }

        /// constructor
        public PcConferenceOptionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcConferenceOptionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_option_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_option_set_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnOptionTypeCode = this.Columns["pc_option_type_code_c"];
            this.ColumnOptionSet = this.Columns["pc_option_set_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcConferenceOptionRow this[int i]
        {
            get
            {
                return ((PcConferenceOptionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcConferenceOptionRow NewRowTyped(bool AWithDefaultValues)
        {
            PcConferenceOptionRow ret = ((PcConferenceOptionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcConferenceOptionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcConferenceOptionRow(builder);
        }

        /// get typed set of changes
        public PcConferenceOptionTable GetChangesTyped()
        {
            return ((PcConferenceOptionTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetOptionTypeCodeDBName()
        {
            return "pc_option_type_code_c";
        }

        /// get character length for column
        public static short GetOptionTypeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetOptionSetDBName()
        {
            return "pc_option_set_l";
        }

        /// get character length for column
        public static short GetOptionSetLength()
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

    /// Lists options that are set for a conference
    [Serializable()]
    public class PcConferenceOptionRow : System.Data.DataRow
    {
        private PcConferenceOptionTable myTable;

        /// Constructor
        public PcConferenceOptionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcConferenceOptionTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        /// Unique name of the cost type
        public String OptionTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionTypeCode)
                            || (((String)(this[this.myTable.ColumnOptionTypeCode])) != value)))
                {
                    this[this.myTable.ColumnOptionTypeCode] = value;
                }
            }
        }

        /// Description of the option type
        public Boolean OptionSet
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionSet.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionSet)
                            || (((Boolean)(this[this.myTable.ColumnOptionSet])) != value)))
                {
                    this[this.myTable.ColumnOptionSet] = value;
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
            this.SetNull(this.myTable.ColumnConferenceKey);
            this.SetNull(this.myTable.ColumnOptionTypeCode);
            this.SetNull(this.myTable.ColumnOptionSet);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsOptionTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnOptionTypeCode);
        }

        /// assign NULL value
        public void SetOptionTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnOptionTypeCode);
        }

        /// test for NULL value
        public bool IsOptionSetNull()
        {
            return this.IsNull(this.myTable.ColumnOptionSet);
        }

        /// assign NULL value
        public void SetOptionSetNull()
        {
            this.SetNull(this.myTable.ColumnOptionSet);
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

    /// Lists possible criterias that must be met for discounts to be applied
    [Serializable()]
    public class PcDiscountCriteriaTable : TTypedDataTable
    {
        /// Unique name of the discount criteria
        public DataColumn ColumnDiscountCriteriaCode;
        /// Description of the discount criteria
        public DataColumn ColumnDiscountCriteriaDesc;
        /// Can this discount criteria be assigned?
        public DataColumn ColumnUnassignableFlag;
        /// This is the date the record was last updated.
        public DataColumn ColumnUnassignableDate;
        /// Indicates if a record can be deleted.
        public DataColumn ColumnDeletableFlag;
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
        public static short TableId = 270;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcDiscountCriteria", "pc_discount_criteria",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "DiscountCriteriaCode", "pc_discount_criteria_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "DiscountCriteriaDesc", "pc_discount_criteria_desc_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(2, "UnassignableFlag", "pc_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "UnassignableDate", "pc_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "DeletableFlag", "pc_deletable_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

        /// constructor
        public PcDiscountCriteriaTable() :
                base("PcDiscountCriteria")
        {
        }

        /// constructor
        public PcDiscountCriteriaTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcDiscountCriteriaTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_discount_criteria_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_discount_criteria_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnDiscountCriteriaCode = this.Columns["pc_discount_criteria_code_c"];
            this.ColumnDiscountCriteriaDesc = this.Columns["pc_discount_criteria_desc_c"];
            this.ColumnUnassignableFlag = this.Columns["pc_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pc_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pc_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcDiscountCriteriaRow this[int i]
        {
            get
            {
                return ((PcDiscountCriteriaRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcDiscountCriteriaRow NewRowTyped(bool AWithDefaultValues)
        {
            PcDiscountCriteriaRow ret = ((PcDiscountCriteriaRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcDiscountCriteriaRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcDiscountCriteriaRow(builder);
        }

        /// get typed set of changes
        public PcDiscountCriteriaTable GetChangesTyped()
        {
            return ((PcDiscountCriteriaTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountCriteriaCodeDBName()
        {
            return "pc_discount_criteria_code_c";
        }

        /// get character length for column
        public static short GetDiscountCriteriaCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountCriteriaDescDBName()
        {
            return "pc_discount_criteria_desc_c";
        }

        /// get character length for column
        public static short GetDiscountCriteriaDescLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pc_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pc_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pc_deletable_flag_l";
        }

        /// get character length for column
        public static short GetDeletableFlagLength()
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

    /// Lists possible criterias that must be met for discounts to be applied
    [Serializable()]
    public class PcDiscountCriteriaRow : System.Data.DataRow
    {
        private PcDiscountCriteriaTable myTable;

        /// Constructor
        public PcDiscountCriteriaRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcDiscountCriteriaTable)(this.Table));
        }

        /// Unique name of the discount criteria
        public String DiscountCriteriaCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscountCriteriaCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscountCriteriaCode)
                            || (((String)(this[this.myTable.ColumnDiscountCriteriaCode])) != value)))
                {
                    this[this.myTable.ColumnDiscountCriteriaCode] = value;
                }
            }
        }

        /// Description of the discount criteria
        public String DiscountCriteriaDesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscountCriteriaDesc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscountCriteriaDesc)
                            || (((String)(this[this.myTable.ColumnDiscountCriteriaDesc])) != value)))
                {
                    this[this.myTable.ColumnDiscountCriteriaDesc] = value;
                }
            }
        }

        /// Can this discount criteria be assigned?
        public Boolean UnassignableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableFlag)
                            || (((Boolean)(this[this.myTable.ColumnUnassignableFlag])) != value)))
                {
                    this[this.myTable.ColumnUnassignableFlag] = value;
                }
            }
        }

        /// This is the date the record was last updated.
        public System.DateTime UnassignableDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnassignableDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnassignableDate)
                            || (((System.DateTime)(this[this.myTable.ColumnUnassignableDate])) != value)))
                {
                    this[this.myTable.ColumnUnassignableDate] = value;
                }
            }
        }

        /// Indicates if a record can be deleted.
        public Boolean DeletableFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletableFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletableFlag)
                            || (((Boolean)(this[this.myTable.ColumnDeletableFlag])) != value)))
                {
                    this[this.myTable.ColumnDeletableFlag] = value;
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
            this.SetNull(this.myTable.ColumnDiscountCriteriaCode);
            this.SetNull(this.myTable.ColumnDiscountCriteriaDesc);
            this[this.myTable.ColumnUnassignableFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnUnassignableDate);
            this[this.myTable.ColumnDeletableFlag.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsDiscountCriteriaCodeNull()
        {
            return this.IsNull(this.myTable.ColumnDiscountCriteriaCode);
        }

        /// assign NULL value
        public void SetDiscountCriteriaCodeNull()
        {
            this.SetNull(this.myTable.ColumnDiscountCriteriaCode);
        }

        /// test for NULL value
        public bool IsDiscountCriteriaDescNull()
        {
            return this.IsNull(this.myTable.ColumnDiscountCriteriaDesc);
        }

        /// assign NULL value
        public void SetDiscountCriteriaDescNull()
        {
            this.SetNull(this.myTable.ColumnDiscountCriteriaDesc);
        }

        /// test for NULL value
        public bool IsUnassignableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableFlag);
        }

        /// assign NULL value
        public void SetUnassignableFlagNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableFlag);
        }

        /// test for NULL value
        public bool IsUnassignableDateNull()
        {
            return this.IsNull(this.myTable.ColumnUnassignableDate);
        }

        /// assign NULL value
        public void SetUnassignableDateNull()
        {
            this.SetNull(this.myTable.ColumnUnassignableDate);
        }

        /// test for NULL value
        public bool IsDeletableFlagNull()
        {
            return this.IsNull(this.myTable.ColumnDeletableFlag);
        }

        /// assign NULL value
        public void SetDeletableFlagNull()
        {
            this.SetNull(this.myTable.ColumnDeletableFlag);
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

    /// Lists optional discounts for a conference
    [Serializable()]
    public class PcDiscountTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// Unique name of the criteria that a person has to meet to get the discount
        public DataColumn ColumnDiscountCriteriaCode;
        /// Unique name of the cost type
        public DataColumn ColumnCostTypeCode;
        /// When is this discount valid (PRE, CONF, POST, ALWAYS)
        public DataColumn ColumnValidity;
        /// For discounts up to a certain age (mainly child discount). If age does not matter, set to -1.
        public DataColumn ColumnUpToAge;
        /// Is the discount value given in percent (or total otherwise)
        public DataColumn ColumnPercentage;
        /// Amount of discount (in percent or total)
        public DataColumn ColumnDiscount;
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
        public static short TableId = 271;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcDiscount", "pc_discount",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "DiscountCriteriaCode", "pc_discount_criteria_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "CostTypeCode", "pc_cost_type_code_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(3, "Validity", "pc_validity_c", OdbcType.VarChar, 6, true),
                    new TTypedColumnInfo(4, "UpToAge", "pc_up_to_age_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "Percentage", "pc_percentage_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "Discount", "pc_discount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(7, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3, 4
                }));
            return true;
        }

        /// constructor
        public PcDiscountTable() :
                base("PcDiscount")
        {
        }

        /// constructor
        public PcDiscountTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcDiscountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_discount_criteria_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_cost_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_validity_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_up_to_age_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_percentage_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_discount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnDiscountCriteriaCode = this.Columns["pc_discount_criteria_code_c"];
            this.ColumnCostTypeCode = this.Columns["pc_cost_type_code_c"];
            this.ColumnValidity = this.Columns["pc_validity_c"];
            this.ColumnUpToAge = this.Columns["pc_up_to_age_i"];
            this.ColumnPercentage = this.Columns["pc_percentage_l"];
            this.ColumnDiscount = this.Columns["pc_discount_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcDiscountRow this[int i]
        {
            get
            {
                return ((PcDiscountRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcDiscountRow NewRowTyped(bool AWithDefaultValues)
        {
            PcDiscountRow ret = ((PcDiscountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcDiscountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcDiscountRow(builder);
        }

        /// get typed set of changes
        public PcDiscountTable GetChangesTyped()
        {
            return ((PcDiscountTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountCriteriaCodeDBName()
        {
            return "pc_discount_criteria_code_c";
        }

        /// get character length for column
        public static short GetDiscountCriteriaCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetCostTypeCodeDBName()
        {
            return "pc_cost_type_code_c";
        }

        /// get character length for column
        public static short GetCostTypeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetValidityDBName()
        {
            return "pc_validity_c";
        }

        /// get character length for column
        public static short GetValidityLength()
        {
            return 6;
        }

        /// get the name of the field in the database for this column
        public static string GetUpToAgeDBName()
        {
            return "pc_up_to_age_i";
        }

        /// get character length for column
        public static short GetUpToAgeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPercentageDBName()
        {
            return "pc_percentage_l";
        }

        /// get character length for column
        public static short GetPercentageLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountDBName()
        {
            return "pc_discount_n";
        }

        /// get character length for column
        public static short GetDiscountLength()
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

    /// Lists optional discounts for a conference
    [Serializable()]
    public class PcDiscountRow : System.Data.DataRow
    {
        private PcDiscountTable myTable;

        /// Constructor
        public PcDiscountRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcDiscountTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        /// Unique name of the criteria that a person has to meet to get the discount
        public String DiscountCriteriaCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscountCriteriaCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscountCriteriaCode)
                            || (((String)(this[this.myTable.ColumnDiscountCriteriaCode])) != value)))
                {
                    this[this.myTable.ColumnDiscountCriteriaCode] = value;
                }
            }
        }

        /// Unique name of the cost type
        public String CostTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostTypeCode)
                            || (((String)(this[this.myTable.ColumnCostTypeCode])) != value)))
                {
                    this[this.myTable.ColumnCostTypeCode] = value;
                }
            }
        }

        /// When is this discount valid (PRE, CONF, POST, ALWAYS)
        public String Validity
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidity.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValidity)
                            || (((String)(this[this.myTable.ColumnValidity])) != value)))
                {
                    this[this.myTable.ColumnValidity] = value;
                }
            }
        }

        /// For discounts up to a certain age (mainly child discount). If age does not matter, set to -1.
        public Int32 UpToAge
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUpToAge.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUpToAge)
                            || (((Int32)(this[this.myTable.ColumnUpToAge])) != value)))
                {
                    this[this.myTable.ColumnUpToAge] = value;
                }
            }
        }

        /// Is the discount value given in percent (or total otherwise)
        public Boolean Percentage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPercentage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPercentage)
                            || (((Boolean)(this[this.myTable.ColumnPercentage])) != value)))
                {
                    this[this.myTable.ColumnPercentage] = value;
                }
            }
        }

        /// Amount of discount (in percent or total)
        public Double Discount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscount)
                            || (((Double)(this[this.myTable.ColumnDiscount])) != value)))
                {
                    this[this.myTable.ColumnDiscount] = value;
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
            this.SetNull(this.myTable.ColumnConferenceKey);
            this.SetNull(this.myTable.ColumnDiscountCriteriaCode);
            this.SetNull(this.myTable.ColumnCostTypeCode);
            this.SetNull(this.myTable.ColumnValidity);
            this.SetNull(this.myTable.ColumnUpToAge);
            this.SetNull(this.myTable.ColumnPercentage);
            this.SetNull(this.myTable.ColumnDiscount);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsDiscountCriteriaCodeNull()
        {
            return this.IsNull(this.myTable.ColumnDiscountCriteriaCode);
        }

        /// assign NULL value
        public void SetDiscountCriteriaCodeNull()
        {
            this.SetNull(this.myTable.ColumnDiscountCriteriaCode);
        }

        /// test for NULL value
        public bool IsCostTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCostTypeCode);
        }

        /// assign NULL value
        public void SetCostTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnCostTypeCode);
        }

        /// test for NULL value
        public bool IsValidityNull()
        {
            return this.IsNull(this.myTable.ColumnValidity);
        }

        /// assign NULL value
        public void SetValidityNull()
        {
            this.SetNull(this.myTable.ColumnValidity);
        }

        /// test for NULL value
        public bool IsUpToAgeNull()
        {
            return this.IsNull(this.myTable.ColumnUpToAge);
        }

        /// assign NULL value
        public void SetUpToAgeNull()
        {
            this.SetNull(this.myTable.ColumnUpToAge);
        }

        /// test for NULL value
        public bool IsPercentageNull()
        {
            return this.IsNull(this.myTable.ColumnPercentage);
        }

        /// assign NULL value
        public void SetPercentageNull()
        {
            this.SetNull(this.myTable.ColumnPercentage);
        }

        /// test for NULL value
        public bool IsDiscountNull()
        {
            return this.IsNull(this.myTable.ColumnDiscount);
        }

        /// assign NULL value
        public void SetDiscountNull()
        {
            this.SetNull(this.myTable.ColumnDiscount);
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

    /// Lists the attendees at a conference
    [Serializable()]
    public class PcAttendeeTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnHomeOfficeKey;
        ///
        public DataColumn ColumnXyzTbdType;
        ///
        public DataColumn ColumnActualArr;
        ///
        public DataColumn ColumnActualDep;
        ///
        public DataColumn ColumnBadgePrint;
        ///
        public DataColumn ColumnDetailsPrint;
        ///
        public DataColumn ColumnComments;
        ///
        public DataColumn ColumnDiscoveryGroup;
        ///
        public DataColumn ColumnWorkGroup;
        ///
        public DataColumn ColumnRegistered;
        ///
        public DataColumn ColumnArrivalGroup;
        ///
        public DataColumn ColumnDepartureGroup;
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
        public static short TableId = 272;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcAttendee", "pc_attendee",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "HomeOfficeKey", "pc_home_office_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(3, "XyzTbdType", "pc_xyz_tbd_type_c", OdbcType.VarChar, 12, false),
                    new TTypedColumnInfo(4, "ActualArr", "pc_actual_arr_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ActualDep", "pc_actual_dep_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "BadgePrint", "pc_badge_print_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "DetailsPrint", "pc_details_print_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "Comments", "pc_comments_c", OdbcType.VarChar, 1000, false),
                    new TTypedColumnInfo(9, "DiscoveryGroup", "pc_discovery_group_c", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(10, "WorkGroup", "pc_work_group_c", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(11, "Registered", "pc_registered_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "ArrivalGroup", "pc_arrival_group_c", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(13, "DepartureGroup", "pc_departure_group_c", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(14, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcAttendeeTable() :
                base("PcAttendee")
        {
        }

        /// constructor
        public PcAttendeeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcAttendeeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_home_office_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_xyz_tbd_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_actual_arr_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_actual_dep_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_badge_print_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_details_print_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_comments_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_discovery_group_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_work_group_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_registered_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_arrival_group_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_departure_group_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnHomeOfficeKey = this.Columns["pc_home_office_key_n"];
            this.ColumnXyzTbdType = this.Columns["pc_xyz_tbd_type_c"];
            this.ColumnActualArr = this.Columns["pc_actual_arr_d"];
            this.ColumnActualDep = this.Columns["pc_actual_dep_d"];
            this.ColumnBadgePrint = this.Columns["pc_badge_print_d"];
            this.ColumnDetailsPrint = this.Columns["pc_details_print_d"];
            this.ColumnComments = this.Columns["pc_comments_c"];
            this.ColumnDiscoveryGroup = this.Columns["pc_discovery_group_c"];
            this.ColumnWorkGroup = this.Columns["pc_work_group_c"];
            this.ColumnRegistered = this.Columns["pc_registered_d"];
            this.ColumnArrivalGroup = this.Columns["pc_arrival_group_c"];
            this.ColumnDepartureGroup = this.Columns["pc_departure_group_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcAttendeeRow this[int i]
        {
            get
            {
                return ((PcAttendeeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcAttendeeRow NewRowTyped(bool AWithDefaultValues)
        {
            PcAttendeeRow ret = ((PcAttendeeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcAttendeeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcAttendeeRow(builder);
        }

        /// get typed set of changes
        public PcAttendeeTable GetChangesTyped()
        {
            return ((PcAttendeeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
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
        public static string GetHomeOfficeKeyDBName()
        {
            return "pc_home_office_key_n";
        }

        /// get character length for column
        public static short GetHomeOfficeKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetXyzTbdTypeDBName()
        {
            return "pc_xyz_tbd_type_c";
        }

        /// get character length for column
        public static short GetXyzTbdTypeLength()
        {
            return 12;
        }

        /// get the name of the field in the database for this column
        public static string GetActualArrDBName()
        {
            return "pc_actual_arr_d";
        }

        /// get character length for column
        public static short GetActualArrLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetActualDepDBName()
        {
            return "pc_actual_dep_d";
        }

        /// get character length for column
        public static short GetActualDepLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBadgePrintDBName()
        {
            return "pc_badge_print_d";
        }

        /// get character length for column
        public static short GetBadgePrintLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDetailsPrintDBName()
        {
            return "pc_details_print_d";
        }

        /// get character length for column
        public static short GetDetailsPrintLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentsDBName()
        {
            return "pc_comments_c";
        }

        /// get character length for column
        public static short GetCommentsLength()
        {
            return 1000;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscoveryGroupDBName()
        {
            return "pc_discovery_group_c";
        }

        /// get character length for column
        public static short GetDiscoveryGroupLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetWorkGroupDBName()
        {
            return "pc_work_group_c";
        }

        /// get character length for column
        public static short GetWorkGroupLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetRegisteredDBName()
        {
            return "pc_registered_d";
        }

        /// get character length for column
        public static short GetRegisteredLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArrivalGroupDBName()
        {
            return "pc_arrival_group_c";
        }

        /// get character length for column
        public static short GetArrivalGroupLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetDepartureGroupDBName()
        {
            return "pc_departure_group_c";
        }

        /// get character length for column
        public static short GetDepartureGroupLength()
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

    /// Lists the attendees at a conference
    [Serializable()]
    public class PcAttendeeRow : System.Data.DataRow
    {
        private PcAttendeeTable myTable;

        /// Constructor
        public PcAttendeeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcAttendeeTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 HomeOfficeKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHomeOfficeKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHomeOfficeKey)
                            || (((Int64)(this[this.myTable.ColumnHomeOfficeKey])) != value)))
                {
                    this[this.myTable.ColumnHomeOfficeKey] = value;
                }
            }
        }

        ///
        public String XyzTbdType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnXyzTbdType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnXyzTbdType)
                            || (((String)(this[this.myTable.ColumnXyzTbdType])) != value)))
                {
                    this[this.myTable.ColumnXyzTbdType] = value;
                }
            }
        }

        ///
        public System.DateTime ActualArr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnActualArr.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnActualArr)
                            || (((System.DateTime)(this[this.myTable.ColumnActualArr])) != value)))
                {
                    this[this.myTable.ColumnActualArr] = value;
                }
            }
        }

        ///
        public System.DateTime ActualDep
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnActualDep.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnActualDep)
                            || (((System.DateTime)(this[this.myTable.ColumnActualDep])) != value)))
                {
                    this[this.myTable.ColumnActualDep] = value;
                }
            }
        }

        ///
        public System.DateTime BadgePrint
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBadgePrint.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBadgePrint)
                            || (((System.DateTime)(this[this.myTable.ColumnBadgePrint])) != value)))
                {
                    this[this.myTable.ColumnBadgePrint] = value;
                }
            }
        }

        ///
        public System.DateTime DetailsPrint
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDetailsPrint.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDetailsPrint)
                            || (((System.DateTime)(this[this.myTable.ColumnDetailsPrint])) != value)))
                {
                    this[this.myTable.ColumnDetailsPrint] = value;
                }
            }
        }

        ///
        public String Comments
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnComments.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnComments)
                            || (((String)(this[this.myTable.ColumnComments])) != value)))
                {
                    this[this.myTable.ColumnComments] = value;
                }
            }
        }

        ///
        public String DiscoveryGroup
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscoveryGroup.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscoveryGroup)
                            || (((String)(this[this.myTable.ColumnDiscoveryGroup])) != value)))
                {
                    this[this.myTable.ColumnDiscoveryGroup] = value;
                }
            }
        }

        ///
        public String WorkGroup
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnWorkGroup.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnWorkGroup)
                            || (((String)(this[this.myTable.ColumnWorkGroup])) != value)))
                {
                    this[this.myTable.ColumnWorkGroup] = value;
                }
            }
        }

        ///
        public System.DateTime Registered
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRegistered.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRegistered)
                            || (((System.DateTime)(this[this.myTable.ColumnRegistered])) != value)))
                {
                    this[this.myTable.ColumnRegistered] = value;
                }
            }
        }

        ///
        public String ArrivalGroup
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArrivalGroup.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArrivalGroup)
                            || (((String)(this[this.myTable.ColumnArrivalGroup])) != value)))
                {
                    this[this.myTable.ColumnArrivalGroup] = value;
                }
            }
        }

        ///
        public String DepartureGroup
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDepartureGroup.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDepartureGroup)
                            || (((String)(this[this.myTable.ColumnDepartureGroup])) != value)))
                {
                    this[this.myTable.ColumnDepartureGroup] = value;
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
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnHomeOfficeKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnXyzTbdType);
            this.SetNull(this.myTable.ColumnActualArr);
            this.SetNull(this.myTable.ColumnActualDep);
            this.SetNull(this.myTable.ColumnBadgePrint);
            this.SetNull(this.myTable.ColumnDetailsPrint);
            this.SetNull(this.myTable.ColumnComments);
            this.SetNull(this.myTable.ColumnDiscoveryGroup);
            this.SetNull(this.myTable.ColumnWorkGroup);
            this.SetNull(this.myTable.ColumnRegistered);
            this.SetNull(this.myTable.ColumnArrivalGroup);
            this.SetNull(this.myTable.ColumnDepartureGroup);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
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
        public bool IsHomeOfficeKeyNull()
        {
            return this.IsNull(this.myTable.ColumnHomeOfficeKey);
        }

        /// assign NULL value
        public void SetHomeOfficeKeyNull()
        {
            this.SetNull(this.myTable.ColumnHomeOfficeKey);
        }

        /// test for NULL value
        public bool IsXyzTbdTypeNull()
        {
            return this.IsNull(this.myTable.ColumnXyzTbdType);
        }

        /// assign NULL value
        public void SetXyzTbdTypeNull()
        {
            this.SetNull(this.myTable.ColumnXyzTbdType);
        }

        /// test for NULL value
        public bool IsActualArrNull()
        {
            return this.IsNull(this.myTable.ColumnActualArr);
        }

        /// assign NULL value
        public void SetActualArrNull()
        {
            this.SetNull(this.myTable.ColumnActualArr);
        }

        /// test for NULL value
        public bool IsActualDepNull()
        {
            return this.IsNull(this.myTable.ColumnActualDep);
        }

        /// assign NULL value
        public void SetActualDepNull()
        {
            this.SetNull(this.myTable.ColumnActualDep);
        }

        /// test for NULL value
        public bool IsBadgePrintNull()
        {
            return this.IsNull(this.myTable.ColumnBadgePrint);
        }

        /// assign NULL value
        public void SetBadgePrintNull()
        {
            this.SetNull(this.myTable.ColumnBadgePrint);
        }

        /// test for NULL value
        public bool IsDetailsPrintNull()
        {
            return this.IsNull(this.myTable.ColumnDetailsPrint);
        }

        /// assign NULL value
        public void SetDetailsPrintNull()
        {
            this.SetNull(this.myTable.ColumnDetailsPrint);
        }

        /// test for NULL value
        public bool IsCommentsNull()
        {
            return this.IsNull(this.myTable.ColumnComments);
        }

        /// assign NULL value
        public void SetCommentsNull()
        {
            this.SetNull(this.myTable.ColumnComments);
        }

        /// test for NULL value
        public bool IsDiscoveryGroupNull()
        {
            return this.IsNull(this.myTable.ColumnDiscoveryGroup);
        }

        /// assign NULL value
        public void SetDiscoveryGroupNull()
        {
            this.SetNull(this.myTable.ColumnDiscoveryGroup);
        }

        /// test for NULL value
        public bool IsWorkGroupNull()
        {
            return this.IsNull(this.myTable.ColumnWorkGroup);
        }

        /// assign NULL value
        public void SetWorkGroupNull()
        {
            this.SetNull(this.myTable.ColumnWorkGroup);
        }

        /// test for NULL value
        public bool IsRegisteredNull()
        {
            return this.IsNull(this.myTable.ColumnRegistered);
        }

        /// assign NULL value
        public void SetRegisteredNull()
        {
            this.SetNull(this.myTable.ColumnRegistered);
        }

        /// test for NULL value
        public bool IsArrivalGroupNull()
        {
            return this.IsNull(this.myTable.ColumnArrivalGroup);
        }

        /// assign NULL value
        public void SetArrivalGroupNull()
        {
            this.SetNull(this.myTable.ColumnArrivalGroup);
        }

        /// test for NULL value
        public bool IsDepartureGroupNull()
        {
            return this.IsNull(this.myTable.ColumnDepartureGroup);
        }

        /// assign NULL value
        public void SetDepartureGroupNull()
        {
            this.SetNull(this.myTable.ColumnDepartureGroup);
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

    /// Charges for the various xyz_tbd options from a conference (currency held in conference master)
    [Serializable()]
    public class PcConferenceCostTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// 9999999999
        public DataColumn ColumnOptionDays;
        ///
        public DataColumn ColumnCharge;
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
        public static short TableId = 273;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcConferenceCost", "pc_conference_cost",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "OptionDays", "pc_option_days_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "Charge", "pc_charge_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcConferenceCostTable() :
                base("PcConferenceCost")
        {
        }

        /// constructor
        public PcConferenceCostTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcConferenceCostTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_option_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_charge_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnOptionDays = this.Columns["pc_option_days_i"];
            this.ColumnCharge = this.Columns["pc_charge_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcConferenceCostRow this[int i]
        {
            get
            {
                return ((PcConferenceCostRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcConferenceCostRow NewRowTyped(bool AWithDefaultValues)
        {
            PcConferenceCostRow ret = ((PcConferenceCostRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcConferenceCostRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcConferenceCostRow(builder);
        }

        /// get typed set of changes
        public PcConferenceCostTable GetChangesTyped()
        {
            return ((PcConferenceCostTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetOptionDaysDBName()
        {
            return "pc_option_days_i";
        }

        /// get character length for column
        public static short GetOptionDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetChargeDBName()
        {
            return "pc_charge_n";
        }

        /// get character length for column
        public static short GetChargeLength()
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

    /// Charges for the various xyz_tbd options from a conference (currency held in conference master)
    [Serializable()]
    public class PcConferenceCostRow : System.Data.DataRow
    {
        private PcConferenceCostTable myTable;

        /// Constructor
        public PcConferenceCostRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcConferenceCostTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        /// 9999999999
        public Int32 OptionDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOptionDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOptionDays)
                            || (((Int32)(this[this.myTable.ColumnOptionDays])) != value)))
                {
                    this[this.myTable.ColumnOptionDays] = value;
                }
            }
        }

        ///
        public Double Charge
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCharge.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCharge)
                            || (((Double)(this[this.myTable.ColumnCharge])) != value)))
                {
                    this[this.myTable.ColumnCharge] = value;
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
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this[this.myTable.ColumnOptionDays.Ordinal] = 0;
            this[this.myTable.ColumnCharge.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsOptionDaysNull()
        {
            return this.IsNull(this.myTable.ColumnOptionDays);
        }

        /// assign NULL value
        public void SetOptionDaysNull()
        {
            this.SetNull(this.myTable.ColumnOptionDays);
        }

        /// test for NULL value
        public bool IsChargeNull()
        {
            return this.IsNull(this.myTable.ColumnCharge);
        }

        /// assign NULL value
        public void SetChargeNull()
        {
            this.SetNull(this.myTable.ColumnCharge);
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

    /// Contains extra conference costs for individual attendees
    [Serializable()]
    public class PcExtraCostTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Key to identify the extra cost, along with conference and partner key
        public DataColumn ColumnExtraCostKey;
        ///
        public DataColumn ColumnCostTypeCode;
        ///
        public DataColumn ColumnCostAmount;
        ///
        public DataColumn ColumnComment;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnAuthorisingField;
        /// Indicate who authorised the extra cost.
        public DataColumn ColumnAuthorisingPerson;
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
        public static short TableId = 274;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcExtraCost", "pc_extra_cost",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "ExtraCostKey", "pc_extra_cost_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "CostTypeCode", "pc_cost_type_code_c", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(4, "CostAmount", "pc_cost_amount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(5, "Comment", "pc_comment_c", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(6, "AuthorisingField", "pc_authorising_field_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(7, "AuthorisingPerson", "pc_authorising_person_c", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(8, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(12, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public PcExtraCostTable() :
                base("PcExtraCost")
        {
        }

        /// constructor
        public PcExtraCostTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcExtraCostTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_extra_cost_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_cost_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_cost_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("pc_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_authorising_field_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_authorising_person_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnExtraCostKey = this.Columns["pc_extra_cost_key_i"];
            this.ColumnCostTypeCode = this.Columns["pc_cost_type_code_c"];
            this.ColumnCostAmount = this.Columns["pc_cost_amount_n"];
            this.ColumnComment = this.Columns["pc_comment_c"];
            this.ColumnAuthorisingField = this.Columns["pc_authorising_field_n"];
            this.ColumnAuthorisingPerson = this.Columns["pc_authorising_person_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcExtraCostRow this[int i]
        {
            get
            {
                return ((PcExtraCostRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcExtraCostRow NewRowTyped(bool AWithDefaultValues)
        {
            PcExtraCostRow ret = ((PcExtraCostRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcExtraCostRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcExtraCostRow(builder);
        }

        /// get typed set of changes
        public PcExtraCostTable GetChangesTyped()
        {
            return ((PcExtraCostTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
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
        public static string GetExtraCostKeyDBName()
        {
            return "pc_extra_cost_key_i";
        }

        /// get character length for column
        public static short GetExtraCostKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCostTypeCodeDBName()
        {
            return "pc_cost_type_code_c";
        }

        /// get character length for column
        public static short GetCostTypeCodeLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetCostAmountDBName()
        {
            return "pc_cost_amount_n";
        }

        /// get character length for column
        public static short GetCostAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentDBName()
        {
            return "pc_comment_c";
        }

        /// get character length for column
        public static short GetCommentLength()
        {
            return 512;
        }

        /// get the name of the field in the database for this column
        public static string GetAuthorisingFieldDBName()
        {
            return "pc_authorising_field_n";
        }

        /// get character length for column
        public static short GetAuthorisingFieldLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetAuthorisingPersonDBName()
        {
            return "pc_authorising_person_c";
        }

        /// get character length for column
        public static short GetAuthorisingPersonLength()
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

    /// Contains extra conference costs for individual attendees
    [Serializable()]
    public class PcExtraCostRow : System.Data.DataRow
    {
        private PcExtraCostTable myTable;

        /// Constructor
        public PcExtraCostRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcExtraCostTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
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

        /// Key to identify the extra cost, along with conference and partner key
        public Int32 ExtraCostKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtraCostKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtraCostKey)
                            || (((Int32)(this[this.myTable.ColumnExtraCostKey])) != value)))
                {
                    this[this.myTable.ColumnExtraCostKey] = value;
                }
            }
        }

        ///
        public String CostTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostTypeCode)
                            || (((String)(this[this.myTable.ColumnCostTypeCode])) != value)))
                {
                    this[this.myTable.ColumnCostTypeCode] = value;
                }
            }
        }

        ///
        public Double CostAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostAmount)
                            || (((Double)(this[this.myTable.ColumnCostAmount])) != value)))
                {
                    this[this.myTable.ColumnCostAmount] = value;
                }
            }
        }

        ///
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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 AuthorisingField
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAuthorisingField.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAuthorisingField)
                            || (((Int64)(this[this.myTable.ColumnAuthorisingField])) != value)))
                {
                    this[this.myTable.ColumnAuthorisingField] = value;
                }
            }
        }

        /// Indicate who authorised the extra cost.
        public String AuthorisingPerson
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAuthorisingPerson.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAuthorisingPerson)
                            || (((String)(this[this.myTable.ColumnAuthorisingPerson])) != value)))
                {
                    this[this.myTable.ColumnAuthorisingPerson] = value;
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
            this.SetNull(this.myTable.ColumnConferenceKey);
            this.SetNull(this.myTable.ColumnPartnerKey);
            this[this.myTable.ColumnExtraCostKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnCostTypeCode);
            this.SetNull(this.myTable.ColumnCostAmount);
            this.SetNull(this.myTable.ColumnComment);
            this.SetNull(this.myTable.ColumnAuthorisingField);
            this.SetNull(this.myTable.ColumnAuthorisingPerson);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
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
        public bool IsExtraCostKeyNull()
        {
            return this.IsNull(this.myTable.ColumnExtraCostKey);
        }

        /// assign NULL value
        public void SetExtraCostKeyNull()
        {
            this.SetNull(this.myTable.ColumnExtraCostKey);
        }

        /// test for NULL value
        public bool IsCostTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCostTypeCode);
        }

        /// assign NULL value
        public void SetCostTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnCostTypeCode);
        }

        /// test for NULL value
        public bool IsCostAmountNull()
        {
            return this.IsNull(this.myTable.ColumnCostAmount);
        }

        /// assign NULL value
        public void SetCostAmountNull()
        {
            this.SetNull(this.myTable.ColumnCostAmount);
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
        public bool IsAuthorisingFieldNull()
        {
            return this.IsNull(this.myTable.ColumnAuthorisingField);
        }

        /// assign NULL value
        public void SetAuthorisingFieldNull()
        {
            this.SetNull(this.myTable.ColumnAuthorisingField);
        }

        /// test for NULL value
        public bool IsAuthorisingPersonNull()
        {
            return this.IsNull(this.myTable.ColumnAuthorisingPerson);
        }

        /// assign NULL value
        public void SetAuthorisingPersonNull()
        {
            this.SetNull(this.myTable.ColumnAuthorisingPerson);
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

    /// Discounts and Supplements for early or late registration
    [Serializable()]
    public class PcEarlyLateTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        ///
        public DataColumn ColumnApplicable;
        ///
        public DataColumn ColumnType;
        ///
        public DataColumn ColumnAmountPercent;
        ///
        public DataColumn ColumnAmount;
        ///
        public DataColumn ColumnPercent;
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
        public static short TableId = 275;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcEarlyLate", "pc_early_late",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "Applicable", "pc_applicable_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "Type", "pc_type_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "AmountPercent", "pc_amount_percent_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "Amount", "pc_amount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(5, "Percent", "pc_percent_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcEarlyLateTable() :
                base("PcEarlyLate")
        {
        }

        /// constructor
        public PcEarlyLateTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcEarlyLateTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_applicable_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_type_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_amount_percent_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("pc_percent_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnApplicable = this.Columns["pc_applicable_d"];
            this.ColumnType = this.Columns["pc_type_l"];
            this.ColumnAmountPercent = this.Columns["pc_amount_percent_l"];
            this.ColumnAmount = this.Columns["pc_amount_n"];
            this.ColumnPercent = this.Columns["pc_percent_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcEarlyLateRow this[int i]
        {
            get
            {
                return ((PcEarlyLateRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcEarlyLateRow NewRowTyped(bool AWithDefaultValues)
        {
            PcEarlyLateRow ret = ((PcEarlyLateRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcEarlyLateRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcEarlyLateRow(builder);
        }

        /// get typed set of changes
        public PcEarlyLateTable GetChangesTyped()
        {
            return ((PcEarlyLateTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetApplicableDBName()
        {
            return "pc_applicable_d";
        }

        /// get character length for column
        public static short GetApplicableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTypeDBName()
        {
            return "pc_type_l";
        }

        /// get character length for column
        public static short GetTypeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountPercentDBName()
        {
            return "pc_amount_percent_l";
        }

        /// get character length for column
        public static short GetAmountPercentLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "pc_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetPercentDBName()
        {
            return "pc_percent_i";
        }

        /// get character length for column
        public static short GetPercentLength()
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

    /// Discounts and Supplements for early or late registration
    [Serializable()]
    public class PcEarlyLateRow : System.Data.DataRow
    {
        private PcEarlyLateTable myTable;

        /// Constructor
        public PcEarlyLateRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcEarlyLateTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        ///
        public System.DateTime Applicable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApplicable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApplicable)
                            || (((System.DateTime)(this[this.myTable.ColumnApplicable])) != value)))
                {
                    this[this.myTable.ColumnApplicable] = value;
                }
            }
        }

        ///
        public Boolean Type
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnType)
                            || (((Boolean)(this[this.myTable.ColumnType])) != value)))
                {
                    this[this.myTable.ColumnType] = value;
                }
            }
        }

        ///
        public Boolean AmountPercent
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAmountPercent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAmountPercent)
                            || (((Boolean)(this[this.myTable.ColumnAmountPercent])) != value)))
                {
                    this[this.myTable.ColumnAmountPercent] = value;
                }
            }
        }

        ///
        public Double Amount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAmount)
                            || (((Double)(this[this.myTable.ColumnAmount])) != value)))
                {
                    this[this.myTable.ColumnAmount] = value;
                }
            }
        }

        ///
        public Int32 Percent
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPercent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPercent)
                            || (((Int32)(this[this.myTable.ColumnPercent])) != value)))
                {
                    this[this.myTable.ColumnPercent] = value;
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
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnApplicable);
            this[this.myTable.ColumnType.Ordinal] = true;
            this[this.myTable.ColumnAmountPercent.Ordinal] = true;
            this[this.myTable.ColumnAmount.Ordinal] = 0;
            this[this.myTable.ColumnPercent.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsApplicableNull()
        {
            return this.IsNull(this.myTable.ColumnApplicable);
        }

        /// assign NULL value
        public void SetApplicableNull()
        {
            this.SetNull(this.myTable.ColumnApplicable);
        }

        /// test for NULL value
        public bool IsTypeNull()
        {
            return this.IsNull(this.myTable.ColumnType);
        }

        /// assign NULL value
        public void SetTypeNull()
        {
            this.SetNull(this.myTable.ColumnType);
        }

        /// test for NULL value
        public bool IsAmountPercentNull()
        {
            return this.IsNull(this.myTable.ColumnAmountPercent);
        }

        /// assign NULL value
        public void SetAmountPercentNull()
        {
            this.SetNull(this.myTable.ColumnAmountPercent);
        }

        /// test for NULL value
        public bool IsAmountNull()
        {
            return this.IsNull(this.myTable.ColumnAmount);
        }

        /// assign NULL value
        public void SetAmountNull()
        {
            this.SetNull(this.myTable.ColumnAmount);
        }

        /// test for NULL value
        public bool IsPercentNull()
        {
            return this.IsNull(this.myTable.ColumnPercent);
        }

        /// assign NULL value
        public void SetPercentNull()
        {
            this.SetNull(this.myTable.ColumnPercent);
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

    /// Contains information about which groups individual attendees are assigned to
    [Serializable()]
    public class PcGroupTable : TTypedDataTable
    {
        /// This is the partner key of the conference. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnGroupType;
        ///
        public DataColumn ColumnGroupName;
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
        public static short TableId = 276;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcGroup", "pc_group",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "GroupType", "pc_group_type_c", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(3, "GroupName", "pc_group_name_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public PcGroupTable() :
                base("PcGroup")
        {
        }

        /// constructor
        public PcGroupTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcGroupTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_group_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_group_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnGroupType = this.Columns["pc_group_type_c"];
            this.ColumnGroupName = this.Columns["pc_group_name_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcGroupRow this[int i]
        {
            get
            {
                return ((PcGroupRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcGroupRow NewRowTyped(bool AWithDefaultValues)
        {
            PcGroupRow ret = ((PcGroupRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcGroupRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcGroupRow(builder);
        }

        /// get typed set of changes
        public PcGroupTable GetChangesTyped()
        {
            return ((PcGroupTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
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
        public static string GetGroupTypeDBName()
        {
            return "pc_group_type_c";
        }

        /// get character length for column
        public static short GetGroupTypeLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetGroupNameDBName()
        {
            return "pc_group_name_c";
        }

        /// get character length for column
        public static short GetGroupNameLength()
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

    /// Contains information about which groups individual attendees are assigned to
    [Serializable()]
    public class PcGroupRow : System.Data.DataRow
    {
        private PcGroupTable myTable;

        /// Constructor
        public PcGroupRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcGroupTable)(this.Table));
        }

        /// This is the partner key of the conference. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
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
        public String GroupType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGroupType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGroupType)
                            || (((String)(this[this.myTable.ColumnGroupType])) != value)))
                {
                    this[this.myTable.ColumnGroupType] = value;
                }
            }
        }

        ///
        public String GroupName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGroupName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGroupName)
                            || (((String)(this[this.myTable.ColumnGroupName])) != value)))
                {
                    this[this.myTable.ColumnGroupName] = value;
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
            this.SetNull(this.myTable.ColumnConferenceKey);
            this.SetNull(this.myTable.ColumnPartnerKey);
            this.SetNull(this.myTable.ColumnGroupType);
            this.SetNull(this.myTable.ColumnGroupName);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
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
        public bool IsGroupTypeNull()
        {
            return this.IsNull(this.myTable.ColumnGroupType);
        }

        /// assign NULL value
        public void SetGroupTypeNull()
        {
            this.SetNull(this.myTable.ColumnGroupType);
        }

        /// test for NULL value
        public bool IsGroupNameNull()
        {
            return this.IsNull(this.myTable.ColumnGroupName);
        }

        /// assign NULL value
        public void SetGroupNameNull()
        {
            this.SetNull(this.myTable.ColumnGroupName);
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

    /// Xyz_tbd travel supplements (by xyz_tbd ID)
    [Serializable()]
    public class PcSupplementTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        ///
        public DataColumn ColumnXyzTbdType;
        ///
        public DataColumn ColumnSupplement;
        /// Apply conference fee discounts to this supplement
        public DataColumn ColumnApplyDiscounts;
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
        public static short TableId = 277;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcSupplement", "pc_supplement",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "XyzTbdType", "pc_xyz_tbd_type_c", OdbcType.VarChar, 12, true),
                    new TTypedColumnInfo(2, "Supplement", "pc_supplement_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(3, "ApplyDiscounts", "pc_apply_discounts_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcSupplementTable() :
                base("PcSupplement")
        {
        }

        /// constructor
        public PcSupplementTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcSupplementTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_xyz_tbd_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_supplement_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("pc_apply_discounts_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnXyzTbdType = this.Columns["pc_xyz_tbd_type_c"];
            this.ColumnSupplement = this.Columns["pc_supplement_n"];
            this.ColumnApplyDiscounts = this.Columns["pc_apply_discounts_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcSupplementRow this[int i]
        {
            get
            {
                return ((PcSupplementRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcSupplementRow NewRowTyped(bool AWithDefaultValues)
        {
            PcSupplementRow ret = ((PcSupplementRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcSupplementRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcSupplementRow(builder);
        }

        /// get typed set of changes
        public PcSupplementTable GetChangesTyped()
        {
            return ((PcSupplementTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetXyzTbdTypeDBName()
        {
            return "pc_xyz_tbd_type_c";
        }

        /// get character length for column
        public static short GetXyzTbdTypeLength()
        {
            return 12;
        }

        /// get the name of the field in the database for this column
        public static string GetSupplementDBName()
        {
            return "pc_supplement_n";
        }

        /// get character length for column
        public static short GetSupplementLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetApplyDiscountsDBName()
        {
            return "pc_apply_discounts_l";
        }

        /// get character length for column
        public static short GetApplyDiscountsLength()
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

    /// Xyz_tbd travel supplements (by xyz_tbd ID)
    [Serializable()]
    public class PcSupplementRow : System.Data.DataRow
    {
        private PcSupplementTable myTable;

        /// Constructor
        public PcSupplementRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcSupplementTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        ///
        public String XyzTbdType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnXyzTbdType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnXyzTbdType)
                            || (((String)(this[this.myTable.ColumnXyzTbdType])) != value)))
                {
                    this[this.myTable.ColumnXyzTbdType] = value;
                }
            }
        }

        ///
        public Double Supplement
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSupplement.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSupplement)
                            || (((Double)(this[this.myTable.ColumnSupplement])) != value)))
                {
                    this[this.myTable.ColumnSupplement] = value;
                }
            }
        }

        /// Apply conference fee discounts to this supplement
        public Boolean ApplyDiscounts
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApplyDiscounts.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApplyDiscounts)
                            || (((Boolean)(this[this.myTable.ColumnApplyDiscounts])) != value)))
                {
                    this[this.myTable.ColumnApplyDiscounts] = value;
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
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnXyzTbdType);
            this[this.myTable.ColumnSupplement.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnApplyDiscounts);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsXyzTbdTypeNull()
        {
            return this.IsNull(this.myTable.ColumnXyzTbdType);
        }

        /// assign NULL value
        public void SetXyzTbdTypeNull()
        {
            this.SetNull(this.myTable.ColumnXyzTbdType);
        }

        /// test for NULL value
        public bool IsSupplementNull()
        {
            return this.IsNull(this.myTable.ColumnSupplement);
        }

        /// assign NULL value
        public void SetSupplementNull()
        {
            this.SetNull(this.myTable.ColumnSupplement);
        }

        /// test for NULL value
        public bool IsApplyDiscountsNull()
        {
            return this.IsNull(this.myTable.ColumnApplyDiscounts);
        }

        /// assign NULL value
        public void SetApplyDiscountsNull()
        {
            this.SetNull(this.myTable.ColumnApplyDiscounts);
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

    /// Links venues to conferences
    [Serializable()]
    public class PcConferenceVenueTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnConferenceKey;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnVenueKey;
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
        public static short TableId = 283;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcConferenceVenue", "pc_conference_venue",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ConferenceKey", "pc_conference_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "VenueKey", "p_venue_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public PcConferenceVenueTable() :
                base("PcConferenceVenue")
        {
        }

        /// constructor
        public PcConferenceVenueTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PcConferenceVenueTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnVenueKey = this.Columns["p_venue_key_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PcConferenceVenueRow this[int i]
        {
            get
            {
                return ((PcConferenceVenueRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PcConferenceVenueRow NewRowTyped(bool AWithDefaultValues)
        {
            PcConferenceVenueRow ret = ((PcConferenceVenueRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PcConferenceVenueRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcConferenceVenueRow(builder);
        }

        /// get typed set of changes
        public PcConferenceVenueTable GetChangesTyped()
        {
            return ((PcConferenceVenueTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }

        /// get character length for column
        public static short GetVenueKeyLength()
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

    /// Links venues to conferences
    [Serializable()]
    public class PcConferenceVenueRow : System.Data.DataRow
    {
        private PcConferenceVenueTable myTable;

        /// Constructor
        public PcConferenceVenueRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PcConferenceVenueTable)(this.Table));
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey)
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 VenueKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVenueKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVenueKey)
                            || (((Int64)(this[this.myTable.ColumnVenueKey])) != value)))
                {
                    this[this.myTable.ColumnVenueKey] = value;
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
            this.SetNull(this.myTable.ColumnConferenceKey);
            this.SetNull(this.myTable.ColumnVenueKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }

        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }

        /// test for NULL value
        public bool IsVenueKeyNull()
        {
            return this.IsNull(this.myTable.ColumnVenueKey);
        }

        /// assign NULL value
        public void SetVenueKeyNull()
        {
            this.SetNull(this.myTable.ColumnVenueKey);
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