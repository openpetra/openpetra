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
namespace Ict.Petra.Shared.MPersonnel.Units.Data
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

    /// This is a listing of the different position which exist within our organisation, e.g. Field Leader, Book Keeper, Computer support.
    [Serializable()]
    public class PtPositionTable : TTypedDataTable
    {
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of this position.
        public DataColumn ColumnPositionScope;
        /// Describes the position.
        public DataColumn ColumnPositionDescr;
        /// Can this position be assigned?
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
        public static short TableId = 252;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PtPosition", "pt_position",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(1, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(2, "PositionDescr", "pt_position_descr_c", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(3, "UnassignableFlag", "pt_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "UnassignableDate", "pt_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "DeletableFlag", "pt_deletable_flag_l", OdbcType.Bit, -1, false),
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
        public PtPositionTable() :
                base("PtPosition")
        {
        }

        /// constructor
        public PtPositionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PtPositionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_descr_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pt_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnPositionDescr = this.Columns["pt_position_descr_c"];
            this.ColumnUnassignableFlag = this.Columns["pt_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pt_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pt_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PtPositionRow this[int i]
        {
            get
            {
                return ((PtPositionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PtPositionRow NewRowTyped(bool AWithDefaultValues)
        {
            PtPositionRow ret = ((PtPositionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PtPositionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PtPositionRow(builder);
        }

        /// get typed set of changes
        public PtPositionTable GetChangesTyped()
        {
            return ((PtPositionTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionDescrDBName()
        {
            return "pt_position_descr_c";
        }

        /// get character length for column
        public static short GetPositionDescrLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pt_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pt_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pt_deletable_flag_l";
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

    /// This is a listing of the different position which exist within our organisation, e.g. Field Leader, Book Keeper, Computer support.
    [Serializable()]
    public class PtPositionRow : System.Data.DataRow
    {
        private PtPositionTable myTable;

        /// Constructor
        public PtPositionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PtPositionTable)(this.Table));
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of this position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        /// Describes the position.
        public String PositionDescr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionDescr.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionDescr)
                            || (((String)(this[this.myTable.ColumnPositionDescr])) != value)))
                {
                    this[this.myTable.ColumnPositionDescr] = value;
                }
            }
        }

        /// Can this position be assigned?
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
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnPositionDescr);
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
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsPositionDescrNull()
        {
            return this.IsNull(this.myTable.ColumnPositionDescr);
        }

        /// assign NULL value
        public void SetPositionDescrNull()
        {
            this.SetNull(this.myTable.ColumnPositionDescr);
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

    /// This table contains information concerning jobs within the unit.
    [Serializable()]
    public class UmJobTable : TTypedDataTable
    {
        /// This is the partner key of the unit. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnUnitKey;
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of the position.
        public DataColumn ColumnPositionScope;
        /// To make sure we can have two jobs in difference time-frames
        public DataColumn ColumnJobKey;
        /// Indicates the normal length of commitment, eg. short-term.
        public DataColumn ColumnJobType;
        /// Date from um_training_period.
        public DataColumn ColumnFromDate;
        /// Date the job posting is to.
        public DataColumn ColumnToDate;
        /// Indicates the minimum number of staff required.
        public DataColumn ColumnMinimum;
        /// Indicates the maximum number of staff required.
        public DataColumn ColumnMaximum;
        /// Indicates the present number on staff.
        public DataColumn ColumnPresent;
        /// Number of part-timers acceptable.
        public DataColumn ColumnPartTimers;
        /// Number of applications on file for this position. This field is driven from the pm_job_assignment.
        public DataColumn ColumnApplications;
        /// Indicates if part-timers can be accepted for this position.
        public DataColumn ColumnPartTimeFlag;
        /// Length of training required for this position.
        public DataColumn ColumnTrainingPeriod;
        /// Length of commitment required for this position.
        public DataColumn ColumnCommitmentPeriod;
        /// Is this position available to other systems.
        public DataColumn ColumnPublicFlag;
        /// Describes where you want to advertise about a job opening, only within the Unit, to the whole organisation, or outside our organisation.
        public DataColumn ColumnJobPublicity;
        /// Indicates whether previous experience with our organisation is required for this job.
        public DataColumn ColumnPreviousInternalExpReq;
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
        public static short TableId = 253;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmJob", "um_job",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "UnitKey", "pm_unit_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(2, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "JobKey", "um_job_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "JobType", "um_job_type_c", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(5, "FromDate", "um_from_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ToDate", "um_to_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "Minimum", "um_minimum_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "Maximum", "um_maximum_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "Present", "um_present_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(10, "PartTimers", "um_part_timers_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "Applications", "um_applications_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(12, "PartTimeFlag", "um_part_time_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(13, "TrainingPeriod", "um_training_period_c", OdbcType.VarChar, 30, true),
                    new TTypedColumnInfo(14, "CommitmentPeriod", "um_commitment_period_c", OdbcType.VarChar, 30, true),
                    new TTypedColumnInfo(15, "PublicFlag", "um_public_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(16, "JobPublicity", "um_job_publicity_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(17, "PreviousInternalExpReq", "um_previous_internal_exp_req_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(18, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(19, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(20, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(21, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(22, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public UmJobTable() :
                base("UmJob")
        {
        }

        /// constructor
        public UmJobTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmJobTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pm_unit_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_job_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_job_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_from_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("um_to_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("um_minimum_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_maximum_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_present_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_part_timers_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_applications_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_part_time_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("um_training_period_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_commitment_period_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_public_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("um_job_publicity_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_previous_internal_exp_req_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnUnitKey = this.Columns["pm_unit_key_n"];
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnJobKey = this.Columns["um_job_key_i"];
            this.ColumnJobType = this.Columns["um_job_type_c"];
            this.ColumnFromDate = this.Columns["um_from_date_d"];
            this.ColumnToDate = this.Columns["um_to_date_d"];
            this.ColumnMinimum = this.Columns["um_minimum_i"];
            this.ColumnMaximum = this.Columns["um_maximum_i"];
            this.ColumnPresent = this.Columns["um_present_i"];
            this.ColumnPartTimers = this.Columns["um_part_timers_i"];
            this.ColumnApplications = this.Columns["um_applications_i"];
            this.ColumnPartTimeFlag = this.Columns["um_part_time_flag_l"];
            this.ColumnTrainingPeriod = this.Columns["um_training_period_c"];
            this.ColumnCommitmentPeriod = this.Columns["um_commitment_period_c"];
            this.ColumnPublicFlag = this.Columns["um_public_flag_l"];
            this.ColumnJobPublicity = this.Columns["um_job_publicity_i"];
            this.ColumnPreviousInternalExpReq = this.Columns["um_previous_internal_exp_req_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmJobRow this[int i]
        {
            get
            {
                return ((UmJobRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmJobRow NewRowTyped(bool AWithDefaultValues)
        {
            UmJobRow ret = ((UmJobRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmJobRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmJobRow(builder);
        }

        /// get typed set of changes
        public UmJobTable GetChangesTyped()
        {
            return ((UmJobTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitKeyDBName()
        {
            return "pm_unit_key_n";
        }

        /// get character length for column
        public static short GetUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetJobKeyDBName()
        {
            return "um_job_key_i";
        }

        /// get character length for column
        public static short GetJobKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetJobTypeDBName()
        {
            return "um_job_type_c";
        }

        /// get character length for column
        public static short GetJobTypeLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetFromDateDBName()
        {
            return "um_from_date_d";
        }

        /// get character length for column
        public static short GetFromDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetToDateDBName()
        {
            return "um_to_date_d";
        }

        /// get character length for column
        public static short GetToDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMinimumDBName()
        {
            return "um_minimum_i";
        }

        /// get character length for column
        public static short GetMinimumLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMaximumDBName()
        {
            return "um_maximum_i";
        }

        /// get character length for column
        public static short GetMaximumLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPresentDBName()
        {
            return "um_present_i";
        }

        /// get character length for column
        public static short GetPresentLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartTimersDBName()
        {
            return "um_part_timers_i";
        }

        /// get character length for column
        public static short GetPartTimersLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetApplicationsDBName()
        {
            return "um_applications_i";
        }

        /// get character length for column
        public static short GetApplicationsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartTimeFlagDBName()
        {
            return "um_part_time_flag_l";
        }

        /// get character length for column
        public static short GetPartTimeFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTrainingPeriodDBName()
        {
            return "um_training_period_c";
        }

        /// get character length for column
        public static short GetTrainingPeriodLength()
        {
            return 30;
        }

        /// get the name of the field in the database for this column
        public static string GetCommitmentPeriodDBName()
        {
            return "um_commitment_period_c";
        }

        /// get character length for column
        public static short GetCommitmentPeriodLength()
        {
            return 30;
        }

        /// get the name of the field in the database for this column
        public static string GetPublicFlagDBName()
        {
            return "um_public_flag_l";
        }

        /// get character length for column
        public static short GetPublicFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetJobPublicityDBName()
        {
            return "um_job_publicity_i";
        }

        /// get character length for column
        public static short GetJobPublicityLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPreviousInternalExpReqDBName()
        {
            return "um_previous_internal_exp_req_l";
        }

        /// get character length for column
        public static short GetPreviousInternalExpReqLength()
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

    /// This table contains information concerning jobs within the unit.
    [Serializable()]
    public class UmJobRow : System.Data.DataRow
    {
        private UmJobTable myTable;

        /// Constructor
        public UmJobRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmJobTable)(this.Table));
        }

        /// This is the partner key of the unit. It consists of the fund id followed by a computer generated six digit number.
        public Int64 UnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitKey)
                            || (((Int64)(this[this.myTable.ColumnUnitKey])) != value)))
                {
                    this[this.myTable.ColumnUnitKey] = value;
                }
            }
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of the position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        /// To make sure we can have two jobs in difference time-frames
        public Int32 JobKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobKey)
                            || (((Int32)(this[this.myTable.ColumnJobKey])) != value)))
                {
                    this[this.myTable.ColumnJobKey] = value;
                }
            }
        }

        /// Indicates the normal length of commitment, eg. short-term.
        public String JobType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnJobType)
                            || (((String)(this[this.myTable.ColumnJobType])) != value)))
                {
                    this[this.myTable.ColumnJobType] = value;
                }
            }
        }

        /// Date from um_training_period.
        public System.DateTime FromDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFromDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFromDate)
                            || (((System.DateTime)(this[this.myTable.ColumnFromDate])) != value)))
                {
                    this[this.myTable.ColumnFromDate] = value;
                }
            }
        }

        /// Date the job posting is to.
        public System.DateTime ToDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnToDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnToDate)
                            || (((System.DateTime)(this[this.myTable.ColumnToDate])) != value)))
                {
                    this[this.myTable.ColumnToDate] = value;
                }
            }
        }

        /// Indicates the minimum number of staff required.
        public Int32 Minimum
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMinimum.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMinimum)
                            || (((Int32)(this[this.myTable.ColumnMinimum])) != value)))
                {
                    this[this.myTable.ColumnMinimum] = value;
                }
            }
        }

        /// Indicates the maximum number of staff required.
        public Int32 Maximum
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMaximum.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMaximum)
                            || (((Int32)(this[this.myTable.ColumnMaximum])) != value)))
                {
                    this[this.myTable.ColumnMaximum] = value;
                }
            }
        }

        /// Indicates the present number on staff.
        public Int32 Present
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPresent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPresent)
                            || (((Int32)(this[this.myTable.ColumnPresent])) != value)))
                {
                    this[this.myTable.ColumnPresent] = value;
                }
            }
        }

        /// Number of part-timers acceptable.
        public Int32 PartTimers
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartTimers.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartTimers)
                            || (((Int32)(this[this.myTable.ColumnPartTimers])) != value)))
                {
                    this[this.myTable.ColumnPartTimers] = value;
                }
            }
        }

        /// Number of applications on file for this position. This field is driven from the pm_job_assignment.
        public Int32 Applications
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApplications.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApplications)
                            || (((Int32)(this[this.myTable.ColumnApplications])) != value)))
                {
                    this[this.myTable.ColumnApplications] = value;
                }
            }
        }

        /// Indicates if part-timers can be accepted for this position.
        public Boolean PartTimeFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartTimeFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartTimeFlag)
                            || (((Boolean)(this[this.myTable.ColumnPartTimeFlag])) != value)))
                {
                    this[this.myTable.ColumnPartTimeFlag] = value;
                }
            }
        }

        /// Length of training required for this position.
        public String TrainingPeriod
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTrainingPeriod.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTrainingPeriod)
                            || (((String)(this[this.myTable.ColumnTrainingPeriod])) != value)))
                {
                    this[this.myTable.ColumnTrainingPeriod] = value;
                }
            }
        }

        /// Length of commitment required for this position.
        public String CommitmentPeriod
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommitmentPeriod.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommitmentPeriod)
                            || (((String)(this[this.myTable.ColumnCommitmentPeriod])) != value)))
                {
                    this[this.myTable.ColumnCommitmentPeriod] = value;
                }
            }
        }

        /// Is this position available to other systems.
        public Boolean PublicFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicFlag)
                            || (((Boolean)(this[this.myTable.ColumnPublicFlag])) != value)))
                {
                    this[this.myTable.ColumnPublicFlag] = value;
                }
            }
        }

        /// Describes where you want to advertise about a job opening, only within the Unit, to the whole organisation, or outside our organisation.
        public Int32 JobPublicity
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobPublicity.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobPublicity)
                            || (((Int32)(this[this.myTable.ColumnJobPublicity])) != value)))
                {
                    this[this.myTable.ColumnJobPublicity] = value;
                }
            }
        }

        /// Indicates whether previous experience with our organisation is required for this job.
        public Boolean PreviousInternalExpReq
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPreviousInternalExpReq.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPreviousInternalExpReq)
                            || (((Boolean)(this[this.myTable.ColumnPreviousInternalExpReq])) != value)))
                {
                    this[this.myTable.ColumnPreviousInternalExpReq] = value;
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
            this[this.myTable.ColumnUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnJobKey);
            this[this.myTable.ColumnJobType.Ordinal] = "Short Term";
            this.SetNull(this.myTable.ColumnFromDate);
            this.SetNull(this.myTable.ColumnToDate);
            this[this.myTable.ColumnMinimum.Ordinal] = 0;
            this[this.myTable.ColumnMaximum.Ordinal] = 0;
            this[this.myTable.ColumnPresent.Ordinal] = 0;
            this[this.myTable.ColumnPartTimers.Ordinal] = 0;
            this[this.myTable.ColumnApplications.Ordinal] = 0;
            this[this.myTable.ColumnPartTimeFlag.Ordinal] = false;
            this[this.myTable.ColumnTrainingPeriod.Ordinal] = "One month";
            this[this.myTable.ColumnCommitmentPeriod.Ordinal] = "Three months";
            this[this.myTable.ColumnPublicFlag.Ordinal] = false;
            this[this.myTable.ColumnJobPublicity.Ordinal] = 0;
            this[this.myTable.ColumnPreviousInternalExpReq.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnUnitKey);
        }

        /// assign NULL value
        public void SetUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnUnitKey);
        }

        /// test for NULL value
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsJobKeyNull()
        {
            return this.IsNull(this.myTable.ColumnJobKey);
        }

        /// assign NULL value
        public void SetJobKeyNull()
        {
            this.SetNull(this.myTable.ColumnJobKey);
        }

        /// test for NULL value
        public bool IsJobTypeNull()
        {
            return this.IsNull(this.myTable.ColumnJobType);
        }

        /// assign NULL value
        public void SetJobTypeNull()
        {
            this.SetNull(this.myTable.ColumnJobType);
        }

        /// test for NULL value
        public bool IsFromDateNull()
        {
            return this.IsNull(this.myTable.ColumnFromDate);
        }

        /// assign NULL value
        public void SetFromDateNull()
        {
            this.SetNull(this.myTable.ColumnFromDate);
        }

        /// test for NULL value
        public bool IsToDateNull()
        {
            return this.IsNull(this.myTable.ColumnToDate);
        }

        /// assign NULL value
        public void SetToDateNull()
        {
            this.SetNull(this.myTable.ColumnToDate);
        }

        /// test for NULL value
        public bool IsMinimumNull()
        {
            return this.IsNull(this.myTable.ColumnMinimum);
        }

        /// assign NULL value
        public void SetMinimumNull()
        {
            this.SetNull(this.myTable.ColumnMinimum);
        }

        /// test for NULL value
        public bool IsMaximumNull()
        {
            return this.IsNull(this.myTable.ColumnMaximum);
        }

        /// assign NULL value
        public void SetMaximumNull()
        {
            this.SetNull(this.myTable.ColumnMaximum);
        }

        /// test for NULL value
        public bool IsPresentNull()
        {
            return this.IsNull(this.myTable.ColumnPresent);
        }

        /// assign NULL value
        public void SetPresentNull()
        {
            this.SetNull(this.myTable.ColumnPresent);
        }

        /// test for NULL value
        public bool IsPartTimersNull()
        {
            return this.IsNull(this.myTable.ColumnPartTimers);
        }

        /// assign NULL value
        public void SetPartTimersNull()
        {
            this.SetNull(this.myTable.ColumnPartTimers);
        }

        /// test for NULL value
        public bool IsApplicationsNull()
        {
            return this.IsNull(this.myTable.ColumnApplications);
        }

        /// assign NULL value
        public void SetApplicationsNull()
        {
            this.SetNull(this.myTable.ColumnApplications);
        }

        /// test for NULL value
        public bool IsPartTimeFlagNull()
        {
            return this.IsNull(this.myTable.ColumnPartTimeFlag);
        }

        /// assign NULL value
        public void SetPartTimeFlagNull()
        {
            this.SetNull(this.myTable.ColumnPartTimeFlag);
        }

        /// test for NULL value
        public bool IsTrainingPeriodNull()
        {
            return this.IsNull(this.myTable.ColumnTrainingPeriod);
        }

        /// assign NULL value
        public void SetTrainingPeriodNull()
        {
            this.SetNull(this.myTable.ColumnTrainingPeriod);
        }

        /// test for NULL value
        public bool IsCommitmentPeriodNull()
        {
            return this.IsNull(this.myTable.ColumnCommitmentPeriod);
        }

        /// assign NULL value
        public void SetCommitmentPeriodNull()
        {
            this.SetNull(this.myTable.ColumnCommitmentPeriod);
        }

        /// test for NULL value
        public bool IsPublicFlagNull()
        {
            return this.IsNull(this.myTable.ColumnPublicFlag);
        }

        /// assign NULL value
        public void SetPublicFlagNull()
        {
            this.SetNull(this.myTable.ColumnPublicFlag);
        }

        /// test for NULL value
        public bool IsJobPublicityNull()
        {
            return this.IsNull(this.myTable.ColumnJobPublicity);
        }

        /// assign NULL value
        public void SetJobPublicityNull()
        {
            this.SetNull(this.myTable.ColumnJobPublicity);
        }

        /// test for NULL value
        public bool IsPreviousInternalExpReqNull()
        {
            return this.IsNull(this.myTable.ColumnPreviousInternalExpReq);
        }

        /// assign NULL value
        public void SetPreviousInternalExpReqNull()
        {
            this.SetNull(this.myTable.ColumnPreviousInternalExpReq);
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

    /// Lists abilities and experience required for various positions.
    [Serializable()]
    public class UmJobRequirementTable : TTypedDataTable
    {
        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnUnitKey;
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of the position.
        public DataColumn ColumnPositionScope;
        ///
        public DataColumn ColumnJobKey;
        /// Name of the area of ability
        public DataColumn ColumnAbilityAreaName;
        /// Years of experience required for this position..
        public DataColumn ColumnYearsOfExperience;
        /// This field is a numeric representation of level of ability.
        public DataColumn ColumnAbilityLevel;
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
        public static short TableId = 254;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmJobRequirement", "um_job_requirement",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "UnitKey", "pm_unit_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(2, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "JobKey", "um_job_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "AbilityAreaName", "pt_ability_area_name_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(5, "YearsOfExperience", "um_years_of_experience_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(6, "AbilityLevel", "pt_ability_level_i", OdbcType.Int, -1, false),
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
        public UmJobRequirementTable() :
                base("UmJobRequirement")
        {
        }

        /// constructor
        public UmJobRequirementTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmJobRequirementTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pm_unit_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_job_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_ability_area_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_years_of_experience_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_ability_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnUnitKey = this.Columns["pm_unit_key_n"];
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnJobKey = this.Columns["um_job_key_i"];
            this.ColumnAbilityAreaName = this.Columns["pt_ability_area_name_c"];
            this.ColumnYearsOfExperience = this.Columns["um_years_of_experience_i"];
            this.ColumnAbilityLevel = this.Columns["pt_ability_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmJobRequirementRow this[int i]
        {
            get
            {
                return ((UmJobRequirementRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmJobRequirementRow NewRowTyped(bool AWithDefaultValues)
        {
            UmJobRequirementRow ret = ((UmJobRequirementRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmJobRequirementRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmJobRequirementRow(builder);
        }

        /// get typed set of changes
        public UmJobRequirementTable GetChangesTyped()
        {
            return ((UmJobRequirementTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitKeyDBName()
        {
            return "pm_unit_key_n";
        }

        /// get character length for column
        public static short GetUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetJobKeyDBName()
        {
            return "um_job_key_i";
        }

        /// get character length for column
        public static short GetJobKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAbilityAreaNameDBName()
        {
            return "pt_ability_area_name_c";
        }

        /// get character length for column
        public static short GetAbilityAreaNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetYearsOfExperienceDBName()
        {
            return "um_years_of_experience_i";
        }

        /// get character length for column
        public static short GetYearsOfExperienceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAbilityLevelDBName()
        {
            return "pt_ability_level_i";
        }

        /// get character length for column
        public static short GetAbilityLevelLength()
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

    /// Lists abilities and experience required for various positions.
    [Serializable()]
    public class UmJobRequirementRow : System.Data.DataRow
    {
        private UmJobRequirementTable myTable;

        /// Constructor
        public UmJobRequirementRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmJobRequirementTable)(this.Table));
        }

        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public Int64 UnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitKey)
                            || (((Int64)(this[this.myTable.ColumnUnitKey])) != value)))
                {
                    this[this.myTable.ColumnUnitKey] = value;
                }
            }
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of the position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        ///
        public Int32 JobKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobKey)
                            || (((Int32)(this[this.myTable.ColumnJobKey])) != value)))
                {
                    this[this.myTable.ColumnJobKey] = value;
                }
            }
        }

        /// Name of the area of ability
        public String AbilityAreaName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAbilityAreaName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAbilityAreaName)
                            || (((String)(this[this.myTable.ColumnAbilityAreaName])) != value)))
                {
                    this[this.myTable.ColumnAbilityAreaName] = value;
                }
            }
        }

        /// Years of experience required for this position..
        public Int32 YearsOfExperience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnYearsOfExperience.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnYearsOfExperience)
                            || (((Int32)(this[this.myTable.ColumnYearsOfExperience])) != value)))
                {
                    this[this.myTable.ColumnYearsOfExperience] = value;
                }
            }
        }

        /// This field is a numeric representation of level of ability.
        public Int32 AbilityLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAbilityLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAbilityLevel)
                            || (((Int32)(this[this.myTable.ColumnAbilityLevel])) != value)))
                {
                    this[this.myTable.ColumnAbilityLevel] = value;
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
            this[this.myTable.ColumnUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnJobKey);
            this.SetNull(this.myTable.ColumnAbilityAreaName);
            this[this.myTable.ColumnYearsOfExperience.Ordinal] = 99;
            this[this.myTable.ColumnAbilityLevel.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnUnitKey);
        }

        /// assign NULL value
        public void SetUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnUnitKey);
        }

        /// test for NULL value
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsJobKeyNull()
        {
            return this.IsNull(this.myTable.ColumnJobKey);
        }

        /// assign NULL value
        public void SetJobKeyNull()
        {
            this.SetNull(this.myTable.ColumnJobKey);
        }

        /// test for NULL value
        public bool IsAbilityAreaNameNull()
        {
            return this.IsNull(this.myTable.ColumnAbilityAreaName);
        }

        /// assign NULL value
        public void SetAbilityAreaNameNull()
        {
            this.SetNull(this.myTable.ColumnAbilityAreaName);
        }

        /// test for NULL value
        public bool IsYearsOfExperienceNull()
        {
            return this.IsNull(this.myTable.ColumnYearsOfExperience);
        }

        /// assign NULL value
        public void SetYearsOfExperienceNull()
        {
            this.SetNull(this.myTable.ColumnYearsOfExperience);
        }

        /// test for NULL value
        public bool IsAbilityLevelNull()
        {
            return this.IsNull(this.myTable.ColumnAbilityLevel);
        }

        /// assign NULL value
        public void SetAbilityLevelNull()
        {
            this.SetNull(this.myTable.ColumnAbilityLevel);
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

    /// Language used on this job.
    [Serializable()]
    public class UmJobLanguageTable : TTypedDataTable
    {
        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnUnitKey;
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of the position.
        public DataColumn ColumnPositionScope;
        ///
        public DataColumn ColumnJobKey;
        /// Name of the language(s) spoken.
        public DataColumn ColumnLanguageCode;
        /// Years of experience required using this language.
        public DataColumn ColumnYearsOfExperience;
        /// This field is a numeric representation of level of language.
        public DataColumn ColumnLanguageLevel;
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
        public static short TableId = 255;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmJobLanguage", "um_job_language",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "UnitKey", "pm_unit_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(2, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "JobKey", "um_job_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "LanguageCode", "p_language_code_c", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(5, "YearsOfExperience", "um_years_of_experience_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(6, "LanguageLevel", "pt_language_level_i", OdbcType.Int, -1, false),
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
        public UmJobLanguageTable() :
                base("UmJobLanguage")
        {
        }

        /// constructor
        public UmJobLanguageTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmJobLanguageTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pm_unit_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_job_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_language_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_years_of_experience_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_language_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnUnitKey = this.Columns["pm_unit_key_n"];
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnJobKey = this.Columns["um_job_key_i"];
            this.ColumnLanguageCode = this.Columns["p_language_code_c"];
            this.ColumnYearsOfExperience = this.Columns["um_years_of_experience_i"];
            this.ColumnLanguageLevel = this.Columns["pt_language_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmJobLanguageRow this[int i]
        {
            get
            {
                return ((UmJobLanguageRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmJobLanguageRow NewRowTyped(bool AWithDefaultValues)
        {
            UmJobLanguageRow ret = ((UmJobLanguageRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmJobLanguageRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmJobLanguageRow(builder);
        }

        /// get typed set of changes
        public UmJobLanguageTable GetChangesTyped()
        {
            return ((UmJobLanguageTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitKeyDBName()
        {
            return "pm_unit_key_n";
        }

        /// get character length for column
        public static short GetUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetJobKeyDBName()
        {
            return "um_job_key_i";
        }

        /// get character length for column
        public static short GetJobKeyLength()
        {
            return -1;
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
        public static string GetYearsOfExperienceDBName()
        {
            return "um_years_of_experience_i";
        }

        /// get character length for column
        public static short GetYearsOfExperienceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLanguageLevelDBName()
        {
            return "pt_language_level_i";
        }

        /// get character length for column
        public static short GetLanguageLevelLength()
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

    /// Language used on this job.
    [Serializable()]
    public class UmJobLanguageRow : System.Data.DataRow
    {
        private UmJobLanguageTable myTable;

        /// Constructor
        public UmJobLanguageRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmJobLanguageTable)(this.Table));
        }

        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public Int64 UnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitKey)
                            || (((Int64)(this[this.myTable.ColumnUnitKey])) != value)))
                {
                    this[this.myTable.ColumnUnitKey] = value;
                }
            }
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of the position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        ///
        public Int32 JobKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobKey)
                            || (((Int32)(this[this.myTable.ColumnJobKey])) != value)))
                {
                    this[this.myTable.ColumnJobKey] = value;
                }
            }
        }

        /// Name of the language(s) spoken.
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

        /// Years of experience required using this language.
        public Int32 YearsOfExperience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnYearsOfExperience.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnYearsOfExperience)
                            || (((Int32)(this[this.myTable.ColumnYearsOfExperience])) != value)))
                {
                    this[this.myTable.ColumnYearsOfExperience] = value;
                }
            }
        }

        /// This field is a numeric representation of level of language.
        public Int32 LanguageLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLanguageLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLanguageLevel)
                            || (((Int32)(this[this.myTable.ColumnLanguageLevel])) != value)))
                {
                    this[this.myTable.ColumnLanguageLevel] = value;
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
            this[this.myTable.ColumnUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnJobKey);
            this.SetNull(this.myTable.ColumnLanguageCode);
            this[this.myTable.ColumnYearsOfExperience.Ordinal] = 99;
            this[this.myTable.ColumnLanguageLevel.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnUnitKey);
        }

        /// assign NULL value
        public void SetUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnUnitKey);
        }

        /// test for NULL value
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsJobKeyNull()
        {
            return this.IsNull(this.myTable.ColumnJobKey);
        }

        /// assign NULL value
        public void SetJobKeyNull()
        {
            this.SetNull(this.myTable.ColumnJobKey);
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
        public bool IsYearsOfExperienceNull()
        {
            return this.IsNull(this.myTable.ColumnYearsOfExperience);
        }

        /// assign NULL value
        public void SetYearsOfExperienceNull()
        {
            this.SetNull(this.myTable.ColumnYearsOfExperience);
        }

        /// test for NULL value
        public bool IsLanguageLevelNull()
        {
            return this.IsNull(this.myTable.ColumnLanguageLevel);
        }

        /// assign NULL value
        public void SetLanguageLevelNull()
        {
            this.SetNull(this.myTable.ColumnLanguageLevel);
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

    /// Details of qualifications required for individual jobs.
    [Serializable()]
    public class UmJobQualificationTable : TTypedDataTable
    {
        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnUnitKey;
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of the position.
        public DataColumn ColumnPositionScope;
        ///
        public DataColumn ColumnJobKey;
        /// Name of the area of qualification.
        public DataColumn ColumnQualificationAreaName;
        /// Years of experience required using this qualification.
        public DataColumn ColumnYearsOfExperience;
        /// This field indicate whether the qualifications can be the result of
        /// informal training.
        public DataColumn ColumnInformalFlag;
        /// This field is a numeric representation of level of qualification.
        public DataColumn ColumnQualificationLevel;
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
        public static short TableId = 256;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmJobQualification", "um_job_qualification",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "UnitKey", "pm_unit_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(2, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "JobKey", "um_job_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "QualificationAreaName", "pt_qualification_area_name_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(5, "YearsOfExperience", "um_years_of_experience_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "InformalFlag", "pm_informal_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(7, "QualificationLevel", "pt_qualification_level_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(12, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3, 4
                }));
            return true;
        }

        /// constructor
        public UmJobQualificationTable() :
                base("UmJobQualification")
        {
        }

        /// constructor
        public UmJobQualificationTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmJobQualificationTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pm_unit_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_job_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_qualification_area_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_years_of_experience_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pm_informal_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pt_qualification_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnUnitKey = this.Columns["pm_unit_key_n"];
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnJobKey = this.Columns["um_job_key_i"];
            this.ColumnQualificationAreaName = this.Columns["pt_qualification_area_name_c"];
            this.ColumnYearsOfExperience = this.Columns["um_years_of_experience_i"];
            this.ColumnInformalFlag = this.Columns["pm_informal_flag_l"];
            this.ColumnQualificationLevel = this.Columns["pt_qualification_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmJobQualificationRow this[int i]
        {
            get
            {
                return ((UmJobQualificationRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmJobQualificationRow NewRowTyped(bool AWithDefaultValues)
        {
            UmJobQualificationRow ret = ((UmJobQualificationRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmJobQualificationRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmJobQualificationRow(builder);
        }

        /// get typed set of changes
        public UmJobQualificationTable GetChangesTyped()
        {
            return ((UmJobQualificationTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitKeyDBName()
        {
            return "pm_unit_key_n";
        }

        /// get character length for column
        public static short GetUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetJobKeyDBName()
        {
            return "um_job_key_i";
        }

        /// get character length for column
        public static short GetJobKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetQualificationAreaNameDBName()
        {
            return "pt_qualification_area_name_c";
        }

        /// get character length for column
        public static short GetQualificationAreaNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetYearsOfExperienceDBName()
        {
            return "um_years_of_experience_i";
        }

        /// get character length for column
        public static short GetYearsOfExperienceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetInformalFlagDBName()
        {
            return "pm_informal_flag_l";
        }

        /// get character length for column
        public static short GetInformalFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetQualificationLevelDBName()
        {
            return "pt_qualification_level_i";
        }

        /// get character length for column
        public static short GetQualificationLevelLength()
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

    /// Details of qualifications required for individual jobs.
    [Serializable()]
    public class UmJobQualificationRow : System.Data.DataRow
    {
        private UmJobQualificationTable myTable;

        /// Constructor
        public UmJobQualificationRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmJobQualificationTable)(this.Table));
        }

        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public Int64 UnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitKey)
                            || (((Int64)(this[this.myTable.ColumnUnitKey])) != value)))
                {
                    this[this.myTable.ColumnUnitKey] = value;
                }
            }
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of the position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        ///
        public Int32 JobKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobKey)
                            || (((Int32)(this[this.myTable.ColumnJobKey])) != value)))
                {
                    this[this.myTable.ColumnJobKey] = value;
                }
            }
        }

        /// Name of the area of qualification.
        public String QualificationAreaName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnQualificationAreaName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnQualificationAreaName)
                            || (((String)(this[this.myTable.ColumnQualificationAreaName])) != value)))
                {
                    this[this.myTable.ColumnQualificationAreaName] = value;
                }
            }
        }

        /// Years of experience required using this qualification.
        public Int32 YearsOfExperience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnYearsOfExperience.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnYearsOfExperience)
                            || (((Int32)(this[this.myTable.ColumnYearsOfExperience])) != value)))
                {
                    this[this.myTable.ColumnYearsOfExperience] = value;
                }
            }
        }

        /// This field indicate whether the qualifications can be the result of
        /// informal training.
        public Boolean InformalFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInformalFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInformalFlag)
                            || (((Boolean)(this[this.myTable.ColumnInformalFlag])) != value)))
                {
                    this[this.myTable.ColumnInformalFlag] = value;
                }
            }
        }

        /// This field is a numeric representation of level of qualification.
        public Int32 QualificationLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnQualificationLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnQualificationLevel)
                            || (((Int32)(this[this.myTable.ColumnQualificationLevel])) != value)))
                {
                    this[this.myTable.ColumnQualificationLevel] = value;
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
            this[this.myTable.ColumnUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnJobKey);
            this.SetNull(this.myTable.ColumnQualificationAreaName);
            this[this.myTable.ColumnYearsOfExperience.Ordinal] = 99;
            this[this.myTable.ColumnInformalFlag.Ordinal] = false;
            this[this.myTable.ColumnQualificationLevel.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnUnitKey);
        }

        /// assign NULL value
        public void SetUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnUnitKey);
        }

        /// test for NULL value
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsJobKeyNull()
        {
            return this.IsNull(this.myTable.ColumnJobKey);
        }

        /// assign NULL value
        public void SetJobKeyNull()
        {
            this.SetNull(this.myTable.ColumnJobKey);
        }

        /// test for NULL value
        public bool IsQualificationAreaNameNull()
        {
            return this.IsNull(this.myTable.ColumnQualificationAreaName);
        }

        /// assign NULL value
        public void SetQualificationAreaNameNull()
        {
            this.SetNull(this.myTable.ColumnQualificationAreaName);
        }

        /// test for NULL value
        public bool IsYearsOfExperienceNull()
        {
            return this.IsNull(this.myTable.ColumnYearsOfExperience);
        }

        /// assign NULL value
        public void SetYearsOfExperienceNull()
        {
            this.SetNull(this.myTable.ColumnYearsOfExperience);
        }

        /// test for NULL value
        public bool IsInformalFlagNull()
        {
            return this.IsNull(this.myTable.ColumnInformalFlag);
        }

        /// assign NULL value
        public void SetInformalFlagNull()
        {
            this.SetNull(this.myTable.ColumnInformalFlag);
        }

        /// test for NULL value
        public bool IsQualificationLevelNull()
        {
            return this.IsNull(this.myTable.ColumnQualificationLevel);
        }

        /// assign NULL value
        public void SetQualificationLevelNull()
        {
            this.SetNull(this.myTable.ColumnQualificationLevel);
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

    /// Details regarding the vision associated with various jobs.
    [Serializable()]
    public class UmJobVisionTable : TTypedDataTable
    {
        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnUnitKey;
        /// Name of the position.
        public DataColumn ColumnPositionName;
        /// Scope of the position.
        public DataColumn ColumnPositionScope;
        ///
        public DataColumn ColumnJobKey;
        /// Name of the area of vision
        public DataColumn ColumnVisionAreaName;
        /// This field is a numeric representation of level of vision.
        public DataColumn ColumnVisionLevel;
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
        public static short TableId = 257;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmJobVision", "um_job_vision",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "UnitKey", "pm_unit_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PositionName", "pt_position_name_c", OdbcType.VarChar, 60, true),
                    new TTypedColumnInfo(2, "PositionScope", "pt_position_scope_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(3, "JobKey", "um_job_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "VisionAreaName", "pt_vision_area_name_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(5, "VisionLevel", "pt_vision_level_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3, 4
                }));
            return true;
        }

        /// constructor
        public UmJobVisionTable() :
                base("UmJobVision")
        {
        }

        /// constructor
        public UmJobVisionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmJobVisionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pm_unit_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_position_scope_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_job_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_vision_area_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_vision_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnUnitKey = this.Columns["pm_unit_key_n"];
            this.ColumnPositionName = this.Columns["pt_position_name_c"];
            this.ColumnPositionScope = this.Columns["pt_position_scope_c"];
            this.ColumnJobKey = this.Columns["um_job_key_i"];
            this.ColumnVisionAreaName = this.Columns["pt_vision_area_name_c"];
            this.ColumnVisionLevel = this.Columns["pt_vision_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmJobVisionRow this[int i]
        {
            get
            {
                return ((UmJobVisionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmJobVisionRow NewRowTyped(bool AWithDefaultValues)
        {
            UmJobVisionRow ret = ((UmJobVisionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmJobVisionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmJobVisionRow(builder);
        }

        /// get typed set of changes
        public UmJobVisionTable GetChangesTyped()
        {
            return ((UmJobVisionTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitKeyDBName()
        {
            return "pm_unit_key_n";
        }

        /// get character length for column
        public static short GetUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionNameDBName()
        {
            return "pt_position_name_c";
        }

        /// get character length for column
        public static short GetPositionNameLength()
        {
            return 60;
        }

        /// get the name of the field in the database for this column
        public static string GetPositionScopeDBName()
        {
            return "pt_position_scope_c";
        }

        /// get character length for column
        public static short GetPositionScopeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetJobKeyDBName()
        {
            return "um_job_key_i";
        }

        /// get character length for column
        public static short GetJobKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetVisionAreaNameDBName()
        {
            return "pt_vision_area_name_c";
        }

        /// get character length for column
        public static short GetVisionAreaNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetVisionLevelDBName()
        {
            return "pt_vision_level_i";
        }

        /// get character length for column
        public static short GetVisionLevelLength()
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

    /// Details regarding the vision associated with various jobs.
    [Serializable()]
    public class UmJobVisionRow : System.Data.DataRow
    {
        private UmJobVisionTable myTable;

        /// Constructor
        public UmJobVisionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmJobVisionTable)(this.Table));
        }

        /// This is the partner key of the unit to which this person is assigned. It consists of the fund id followed by a computer generated six digit number.
        public Int64 UnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitKey)
                            || (((Int64)(this[this.myTable.ColumnUnitKey])) != value)))
                {
                    this[this.myTable.ColumnUnitKey] = value;
                }
            }
        }

        /// Name of the position.
        public String PositionName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionName)
                            || (((String)(this[this.myTable.ColumnPositionName])) != value)))
                {
                    this[this.myTable.ColumnPositionName] = value;
                }
            }
        }

        /// Scope of the position.
        public String PositionScope
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPositionScope.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPositionScope)
                            || (((String)(this[this.myTable.ColumnPositionScope])) != value)))
                {
                    this[this.myTable.ColumnPositionScope] = value;
                }
            }
        }

        ///
        public Int32 JobKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJobKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJobKey)
                            || (((Int32)(this[this.myTable.ColumnJobKey])) != value)))
                {
                    this[this.myTable.ColumnJobKey] = value;
                }
            }
        }

        /// Name of the area of vision
        public String VisionAreaName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVisionAreaName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnVisionAreaName)
                            || (((String)(this[this.myTable.ColumnVisionAreaName])) != value)))
                {
                    this[this.myTable.ColumnVisionAreaName] = value;
                }
            }
        }

        /// This field is a numeric representation of level of vision.
        public Int32 VisionLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVisionLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVisionLevel)
                            || (((Int32)(this[this.myTable.ColumnVisionLevel])) != value)))
                {
                    this[this.myTable.ColumnVisionLevel] = value;
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
            this[this.myTable.ColumnUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPositionName);
            this.SetNull(this.myTable.ColumnPositionScope);
            this.SetNull(this.myTable.ColumnJobKey);
            this.SetNull(this.myTable.ColumnVisionAreaName);
            this[this.myTable.ColumnVisionLevel.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnUnitKey);
        }

        /// assign NULL value
        public void SetUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnUnitKey);
        }

        /// test for NULL value
        public bool IsPositionNameNull()
        {
            return this.IsNull(this.myTable.ColumnPositionName);
        }

        /// assign NULL value
        public void SetPositionNameNull()
        {
            this.SetNull(this.myTable.ColumnPositionName);
        }

        /// test for NULL value
        public bool IsPositionScopeNull()
        {
            return this.IsNull(this.myTable.ColumnPositionScope);
        }

        /// assign NULL value
        public void SetPositionScopeNull()
        {
            this.SetNull(this.myTable.ColumnPositionScope);
        }

        /// test for NULL value
        public bool IsJobKeyNull()
        {
            return this.IsNull(this.myTable.ColumnJobKey);
        }

        /// assign NULL value
        public void SetJobKeyNull()
        {
            this.SetNull(this.myTable.ColumnJobKey);
        }

        /// test for NULL value
        public bool IsVisionAreaNameNull()
        {
            return this.IsNull(this.myTable.ColumnVisionAreaName);
        }

        /// assign NULL value
        public void SetVisionAreaNameNull()
        {
            this.SetNull(this.myTable.ColumnVisionAreaName);
        }

        /// test for NULL value
        public bool IsVisionLevelNull()
        {
            return this.IsNull(this.myTable.ColumnVisionLevel);
        }

        /// assign NULL value
        public void SetVisionLevelNull()
        {
            this.SetNull(this.myTable.ColumnVisionLevel);
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

    /// Describes whether a person is full-time, part-time, etc.
    [Serializable()]
    public class PtAssignmentTypeTable : TTypedDataTable
    {
        /// Indicates the type of assignment .
        public DataColumn ColumnAssignmentTypeCode;
        /// This describes the one-letter assignment code.
        public DataColumn ColumnAssignmentCodeDescr;
        /// Can this qualification level be assigned?
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
        public static short TableId = 258;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PtAssignmentType", "pt_assignment_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "AssignmentTypeCode", "pt_assignment_type_code_c", OdbcType.VarChar, 2, true),
                    new TTypedColumnInfo(1, "AssignmentCodeDescr", "pt_assignment_code_descr_c", OdbcType.VarChar, 70, true),
                    new TTypedColumnInfo(2, "UnassignableFlag", "pt_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "UnassignableDate", "pt_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "DeletableFlag", "pt_deletable_flag_l", OdbcType.Bit, -1, false),
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
        public PtAssignmentTypeTable() :
                base("PtAssignmentType")
        {
        }

        /// constructor
        public PtAssignmentTypeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PtAssignmentTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pt_assignment_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_assignment_code_descr_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pt_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnAssignmentTypeCode = this.Columns["pt_assignment_type_code_c"];
            this.ColumnAssignmentCodeDescr = this.Columns["pt_assignment_code_descr_c"];
            this.ColumnUnassignableFlag = this.Columns["pt_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pt_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pt_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PtAssignmentTypeRow this[int i]
        {
            get
            {
                return ((PtAssignmentTypeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PtAssignmentTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            PtAssignmentTypeRow ret = ((PtAssignmentTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PtAssignmentTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PtAssignmentTypeRow(builder);
        }

        /// get typed set of changes
        public PtAssignmentTypeTable GetChangesTyped()
        {
            return ((PtAssignmentTypeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetAssignmentTypeCodeDBName()
        {
            return "pt_assignment_type_code_c";
        }

        /// get character length for column
        public static short GetAssignmentTypeCodeLength()
        {
            return 2;
        }

        /// get the name of the field in the database for this column
        public static string GetAssignmentCodeDescrDBName()
        {
            return "pt_assignment_code_descr_c";
        }

        /// get character length for column
        public static short GetAssignmentCodeDescrLength()
        {
            return 70;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pt_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pt_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pt_deletable_flag_l";
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

    /// Describes whether a person is full-time, part-time, etc.
    [Serializable()]
    public class PtAssignmentTypeRow : System.Data.DataRow
    {
        private PtAssignmentTypeTable myTable;

        /// Constructor
        public PtAssignmentTypeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PtAssignmentTypeTable)(this.Table));
        }

        /// Indicates the type of assignment .
        public String AssignmentTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAssignmentTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAssignmentTypeCode)
                            || (((String)(this[this.myTable.ColumnAssignmentTypeCode])) != value)))
                {
                    this[this.myTable.ColumnAssignmentTypeCode] = value;
                }
            }
        }

        /// This describes the one-letter assignment code.
        public String AssignmentCodeDescr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAssignmentCodeDescr.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAssignmentCodeDescr)
                            || (((String)(this[this.myTable.ColumnAssignmentCodeDescr])) != value)))
                {
                    this[this.myTable.ColumnAssignmentCodeDescr] = value;
                }
            }
        }

        /// Can this qualification level be assigned?
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
            this.SetNull(this.myTable.ColumnAssignmentTypeCode);
            this.SetNull(this.myTable.ColumnAssignmentCodeDescr);
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
        public bool IsAssignmentTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAssignmentTypeCode);
        }

        /// assign NULL value
        public void SetAssignmentTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnAssignmentTypeCode);
        }

        /// test for NULL value
        public bool IsAssignmentCodeDescrNull()
        {
            return this.IsNull(this.myTable.ColumnAssignmentCodeDescr);
        }

        /// assign NULL value
        public void SetAssignmentCodeDescrNull()
        {
            this.SetNull(this.myTable.ColumnAssignmentCodeDescr);
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

    /// This describes the reason a person left a particular position.
    [Serializable()]
    public class PtLeavingCodeTable : TTypedDataTable
    {
        /// This is the one letter code that indicates why a person left
        /// a particular position.
        public DataColumn ColumnLeavingCodeInd;
        /// This describes the one letter leaving code.
        public DataColumn ColumnLeavingCodeDescr;
        /// Can this qualification level be assigned?
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
        public static short TableId = 259;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PtLeavingCode", "pt_leaving_code",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LeavingCodeInd", "pt_leaving_code_ind_c", OdbcType.VarChar, 4, true),
                    new TTypedColumnInfo(1, "LeavingCodeDescr", "pt_leaving_code_descr_c", OdbcType.VarChar, 70, true),
                    new TTypedColumnInfo(2, "UnassignableFlag", "pt_unassignable_flag_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "UnassignableDate", "pt_unassignable_date_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "DeletableFlag", "pt_deletable_flag_l", OdbcType.Bit, -1, false),
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
        public PtLeavingCodeTable() :
                base("PtLeavingCode")
        {
        }

        /// constructor
        public PtLeavingCodeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PtLeavingCodeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pt_leaving_code_ind_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_leaving_code_descr_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pt_unassignable_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pt_deletable_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLeavingCodeInd = this.Columns["pt_leaving_code_ind_c"];
            this.ColumnLeavingCodeDescr = this.Columns["pt_leaving_code_descr_c"];
            this.ColumnUnassignableFlag = this.Columns["pt_unassignable_flag_l"];
            this.ColumnUnassignableDate = this.Columns["pt_unassignable_date_d"];
            this.ColumnDeletableFlag = this.Columns["pt_deletable_flag_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public PtLeavingCodeRow this[int i]
        {
            get
            {
                return ((PtLeavingCodeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PtLeavingCodeRow NewRowTyped(bool AWithDefaultValues)
        {
            PtLeavingCodeRow ret = ((PtLeavingCodeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PtLeavingCodeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PtLeavingCodeRow(builder);
        }

        /// get typed set of changes
        public PtLeavingCodeTable GetChangesTyped()
        {
            return ((PtLeavingCodeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLeavingCodeIndDBName()
        {
            return "pt_leaving_code_ind_c";
        }

        /// get character length for column
        public static short GetLeavingCodeIndLength()
        {
            return 4;
        }

        /// get the name of the field in the database for this column
        public static string GetLeavingCodeDescrDBName()
        {
            return "pt_leaving_code_descr_c";
        }

        /// get character length for column
        public static short GetLeavingCodeDescrLength()
        {
            return 70;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableFlagDBName()
        {
            return "pt_unassignable_flag_l";
        }

        /// get character length for column
        public static short GetUnassignableFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnassignableDateDBName()
        {
            return "pt_unassignable_date_d";
        }

        /// get character length for column
        public static short GetUnassignableDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableFlagDBName()
        {
            return "pt_deletable_flag_l";
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

    /// This describes the reason a person left a particular position.
    [Serializable()]
    public class PtLeavingCodeRow : System.Data.DataRow
    {
        private PtLeavingCodeTable myTable;

        /// Constructor
        public PtLeavingCodeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PtLeavingCodeTable)(this.Table));
        }

        /// This is the one letter code that indicates why a person left
        /// a particular position.
        public String LeavingCodeInd
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLeavingCodeInd.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLeavingCodeInd)
                            || (((String)(this[this.myTable.ColumnLeavingCodeInd])) != value)))
                {
                    this[this.myTable.ColumnLeavingCodeInd] = value;
                }
            }
        }

        /// This describes the one letter leaving code.
        public String LeavingCodeDescr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLeavingCodeDescr.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLeavingCodeDescr)
                            || (((String)(this[this.myTable.ColumnLeavingCodeDescr])) != value)))
                {
                    this[this.myTable.ColumnLeavingCodeDescr] = value;
                }
            }
        }

        /// Can this qualification level be assigned?
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
            this.SetNull(this.myTable.ColumnLeavingCodeInd);
            this.SetNull(this.myTable.ColumnLeavingCodeDescr);
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
        public bool IsLeavingCodeIndNull()
        {
            return this.IsNull(this.myTable.ColumnLeavingCodeInd);
        }

        /// assign NULL value
        public void SetLeavingCodeIndNull()
        {
            this.SetNull(this.myTable.ColumnLeavingCodeInd);
        }

        /// test for NULL value
        public bool IsLeavingCodeDescrNull()
        {
            return this.IsNull(this.myTable.ColumnLeavingCodeDescr);
        }

        /// assign NULL value
        public void SetLeavingCodeDescrNull()
        {
            this.SetNull(this.myTable.ColumnLeavingCodeDescr);
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

    /// Details of  the abilities within the unit.
    [Serializable()]
    public class UmUnitAbilityTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the area of ability
        public DataColumn ColumnAbilityAreaName;
        /// Years of experience this required for this ability.
        public DataColumn ColumnYearsOfExperience;
        /// This field is a numeric representation of level of ability.
        public DataColumn ColumnAbilityLevel;
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
        public static short TableId = 261;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmUnitAbility", "um_unit_ability",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "AbilityAreaName", "pt_ability_area_name_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "YearsOfExperience", "um_years_of_experience_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "AbilityLevel", "pt_ability_level_i", OdbcType.Int, -1, true),
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
        public UmUnitAbilityTable() :
                base("UmUnitAbility")
        {
        }

        /// constructor
        public UmUnitAbilityTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmUnitAbilityTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_ability_area_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_years_of_experience_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pt_ability_level_i", typeof(Int32)));
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
            this.ColumnAbilityAreaName = this.Columns["pt_ability_area_name_c"];
            this.ColumnYearsOfExperience = this.Columns["um_years_of_experience_i"];
            this.ColumnAbilityLevel = this.Columns["pt_ability_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmUnitAbilityRow this[int i]
        {
            get
            {
                return ((UmUnitAbilityRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmUnitAbilityRow NewRowTyped(bool AWithDefaultValues)
        {
            UmUnitAbilityRow ret = ((UmUnitAbilityRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmUnitAbilityRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmUnitAbilityRow(builder);
        }

        /// get typed set of changes
        public UmUnitAbilityTable GetChangesTyped()
        {
            return ((UmUnitAbilityTable)(base.GetChangesTypedInternal()));
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
        public static string GetAbilityAreaNameDBName()
        {
            return "pt_ability_area_name_c";
        }

        /// get character length for column
        public static short GetAbilityAreaNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetYearsOfExperienceDBName()
        {
            return "um_years_of_experience_i";
        }

        /// get character length for column
        public static short GetYearsOfExperienceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAbilityLevelDBName()
        {
            return "pt_ability_level_i";
        }

        /// get character length for column
        public static short GetAbilityLevelLength()
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

    /// Details of  the abilities within the unit.
    [Serializable()]
    public class UmUnitAbilityRow : System.Data.DataRow
    {
        private UmUnitAbilityTable myTable;

        /// Constructor
        public UmUnitAbilityRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmUnitAbilityTable)(this.Table));
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

        /// Name of the area of ability
        public String AbilityAreaName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAbilityAreaName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAbilityAreaName)
                            || (((String)(this[this.myTable.ColumnAbilityAreaName])) != value)))
                {
                    this[this.myTable.ColumnAbilityAreaName] = value;
                }
            }
        }

        /// Years of experience this required for this ability.
        public Int32 YearsOfExperience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnYearsOfExperience.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnYearsOfExperience)
                            || (((Int32)(this[this.myTable.ColumnYearsOfExperience])) != value)))
                {
                    this[this.myTable.ColumnYearsOfExperience] = value;
                }
            }
        }

        /// This field is a numeric representation of level of ability.
        public Int32 AbilityLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAbilityLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAbilityLevel)
                            || (((Int32)(this[this.myTable.ColumnAbilityLevel])) != value)))
                {
                    this[this.myTable.ColumnAbilityLevel] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAbilityAreaName);
            this[this.myTable.ColumnYearsOfExperience.Ordinal] = 99;
            this[this.myTable.ColumnAbilityLevel.Ordinal] = 0;
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
        public bool IsAbilityAreaNameNull()
        {
            return this.IsNull(this.myTable.ColumnAbilityAreaName);
        }

        /// assign NULL value
        public void SetAbilityAreaNameNull()
        {
            this.SetNull(this.myTable.ColumnAbilityAreaName);
        }

        /// test for NULL value
        public bool IsYearsOfExperienceNull()
        {
            return this.IsNull(this.myTable.ColumnYearsOfExperience);
        }

        /// assign NULL value
        public void SetYearsOfExperienceNull()
        {
            this.SetNull(this.myTable.ColumnYearsOfExperience);
        }

        /// test for NULL value
        public bool IsAbilityLevelNull()
        {
            return this.IsNull(this.myTable.ColumnAbilityLevel);
        }

        /// assign NULL value
        public void SetAbilityLevelNull()
        {
            this.SetNull(this.myTable.ColumnAbilityLevel);
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

    /// Details of the language used within this unit.
    [Serializable()]
    public class UmUnitLanguageTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the language(s) spoken.
        public DataColumn ColumnLanguageCode;
        /// This field is a numeric representation of level of language.
        public DataColumn ColumnLanguageLevel;
        /// Years of experience required using this language.
        public DataColumn ColumnYearsOfExperience;
        /// Contains comments pertaining to the language of the unit.
        public DataColumn ColumnUnitLangComment;
        /// Lists whether the languare is required or desired.
        public DataColumn ColumnUnitLanguageReq;
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
        public static short TableId = 262;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmUnitLanguage", "um_unit_language",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "LanguageCode", "p_language_code_c", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(2, "LanguageLevel", "pt_language_level_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "YearsOfExperience", "um_years_of_experience_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "UnitLangComment", "um_unit_lang_comment_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(5, "UnitLanguageReq", "um_unit_language_req_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public UmUnitLanguageTable() :
                base("UmUnitLanguage")
        {
        }

        /// constructor
        public UmUnitLanguageTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmUnitLanguageTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_language_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_language_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_years_of_experience_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("um_unit_lang_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_unit_language_req_c", typeof(String)));
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
            this.ColumnLanguageCode = this.Columns["p_language_code_c"];
            this.ColumnLanguageLevel = this.Columns["pt_language_level_i"];
            this.ColumnYearsOfExperience = this.Columns["um_years_of_experience_i"];
            this.ColumnUnitLangComment = this.Columns["um_unit_lang_comment_c"];
            this.ColumnUnitLanguageReq = this.Columns["um_unit_language_req_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmUnitLanguageRow this[int i]
        {
            get
            {
                return ((UmUnitLanguageRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmUnitLanguageRow NewRowTyped(bool AWithDefaultValues)
        {
            UmUnitLanguageRow ret = ((UmUnitLanguageRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmUnitLanguageRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmUnitLanguageRow(builder);
        }

        /// get typed set of changes
        public UmUnitLanguageTable GetChangesTyped()
        {
            return ((UmUnitLanguageTable)(base.GetChangesTypedInternal()));
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
        public static string GetLanguageLevelDBName()
        {
            return "pt_language_level_i";
        }

        /// get character length for column
        public static short GetLanguageLevelLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetYearsOfExperienceDBName()
        {
            return "um_years_of_experience_i";
        }

        /// get character length for column
        public static short GetYearsOfExperienceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUnitLangCommentDBName()
        {
            return "um_unit_lang_comment_c";
        }

        /// get character length for column
        public static short GetUnitLangCommentLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetUnitLanguageReqDBName()
        {
            return "um_unit_language_req_c";
        }

        /// get character length for column
        public static short GetUnitLanguageReqLength()
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

    /// Details of the language used within this unit.
    [Serializable()]
    public class UmUnitLanguageRow : System.Data.DataRow
    {
        private UmUnitLanguageTable myTable;

        /// Constructor
        public UmUnitLanguageRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmUnitLanguageTable)(this.Table));
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

        /// Name of the language(s) spoken.
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

        /// This field is a numeric representation of level of language.
        public Int32 LanguageLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLanguageLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLanguageLevel)
                            || (((Int32)(this[this.myTable.ColumnLanguageLevel])) != value)))
                {
                    this[this.myTable.ColumnLanguageLevel] = value;
                }
            }
        }

        /// Years of experience required using this language.
        public Int32 YearsOfExperience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnYearsOfExperience.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnYearsOfExperience)
                            || (((Int32)(this[this.myTable.ColumnYearsOfExperience])) != value)))
                {
                    this[this.myTable.ColumnYearsOfExperience] = value;
                }
            }
        }

        /// Contains comments pertaining to the language of the unit.
        public String UnitLangComment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitLangComment.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnUnitLangComment)
                            || (((String)(this[this.myTable.ColumnUnitLangComment])) != value)))
                {
                    this[this.myTable.ColumnUnitLangComment] = value;
                }
            }
        }

        /// Lists whether the languare is required or desired.
        public String UnitLanguageReq
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitLanguageReq.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnUnitLanguageReq)
                            || (((String)(this[this.myTable.ColumnUnitLanguageReq])) != value)))
                {
                    this[this.myTable.ColumnUnitLanguageReq] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnLanguageCode);
            this[this.myTable.ColumnLanguageLevel.Ordinal] = 0;
            this[this.myTable.ColumnYearsOfExperience.Ordinal] = 99;
            this.SetNull(this.myTable.ColumnUnitLangComment);
            this.SetNull(this.myTable.ColumnUnitLanguageReq);
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
        public bool IsLanguageLevelNull()
        {
            return this.IsNull(this.myTable.ColumnLanguageLevel);
        }

        /// assign NULL value
        public void SetLanguageLevelNull()
        {
            this.SetNull(this.myTable.ColumnLanguageLevel);
        }

        /// test for NULL value
        public bool IsYearsOfExperienceNull()
        {
            return this.IsNull(this.myTable.ColumnYearsOfExperience);
        }

        /// assign NULL value
        public void SetYearsOfExperienceNull()
        {
            this.SetNull(this.myTable.ColumnYearsOfExperience);
        }

        /// test for NULL value
        public bool IsUnitLangCommentNull()
        {
            return this.IsNull(this.myTable.ColumnUnitLangComment);
        }

        /// assign NULL value
        public void SetUnitLangCommentNull()
        {
            this.SetNull(this.myTable.ColumnUnitLangComment);
        }

        /// test for NULL value
        public bool IsUnitLanguageReqNull()
        {
            return this.IsNull(this.myTable.ColumnUnitLanguageReq);
        }

        /// assign NULL value
        public void SetUnitLanguageReqNull()
        {
            this.SetNull(this.myTable.ColumnUnitLanguageReq);
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

    /// Details of the vision required on this unit.
    [Serializable()]
    public class UmUnitVisionTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the area of vision
        public DataColumn ColumnVisionAreaName;
        /// This field is a numeric representation of level of vision.
        public DataColumn ColumnVisionLevel;
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
        public static short TableId = 263;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmUnitVision", "um_unit_vision",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "VisionAreaName", "pt_vision_area_name_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(2, "VisionLevel", "pt_vision_level_i", OdbcType.Int, -1, true),
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
        public UmUnitVisionTable() :
                base("UmUnitVision")
        {
        }

        /// constructor
        public UmUnitVisionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmUnitVisionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pt_vision_area_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pt_vision_level_i", typeof(Int32)));
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
            this.ColumnVisionAreaName = this.Columns["pt_vision_area_name_c"];
            this.ColumnVisionLevel = this.Columns["pt_vision_level_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmUnitVisionRow this[int i]
        {
            get
            {
                return ((UmUnitVisionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmUnitVisionRow NewRowTyped(bool AWithDefaultValues)
        {
            UmUnitVisionRow ret = ((UmUnitVisionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmUnitVisionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmUnitVisionRow(builder);
        }

        /// get typed set of changes
        public UmUnitVisionTable GetChangesTyped()
        {
            return ((UmUnitVisionTable)(base.GetChangesTypedInternal()));
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
        public static string GetVisionAreaNameDBName()
        {
            return "pt_vision_area_name_c";
        }

        /// get character length for column
        public static short GetVisionAreaNameLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetVisionLevelDBName()
        {
            return "pt_vision_level_i";
        }

        /// get character length for column
        public static short GetVisionLevelLength()
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

    /// Details of the vision required on this unit.
    [Serializable()]
    public class UmUnitVisionRow : System.Data.DataRow
    {
        private UmUnitVisionTable myTable;

        /// Constructor
        public UmUnitVisionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmUnitVisionTable)(this.Table));
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

        /// Name of the area of vision
        public String VisionAreaName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVisionAreaName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnVisionAreaName)
                            || (((String)(this[this.myTable.ColumnVisionAreaName])) != value)))
                {
                    this[this.myTable.ColumnVisionAreaName] = value;
                }
            }
        }

        /// This field is a numeric representation of level of vision.
        public Int32 VisionLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVisionLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVisionLevel)
                            || (((Int32)(this[this.myTable.ColumnVisionLevel])) != value)))
                {
                    this[this.myTable.ColumnVisionLevel] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnVisionAreaName);
            this[this.myTable.ColumnVisionLevel.Ordinal] = 0;
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
        public bool IsVisionAreaNameNull()
        {
            return this.IsNull(this.myTable.ColumnVisionAreaName);
        }

        /// assign NULL value
        public void SetVisionAreaNameNull()
        {
            this.SetNull(this.myTable.ColumnVisionAreaName);
        }

        /// test for NULL value
        public bool IsVisionLevelNull()
        {
            return this.IsNull(this.myTable.ColumnVisionLevel);
        }

        /// assign NULL value
        public void SetVisionLevelNull()
        {
            this.SetNull(this.myTable.ColumnVisionLevel);
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

    /// Details pertaining to the costs of being on in the unit.
    [Serializable()]
    public class UmUnitCostTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Date from which these costs are applicable.
        public DataColumn ColumnValidFromDate;
        /// Indicates amount it costs a single to be on the team.
        public DataColumn ColumnSingleCostsPeriodIntl;
        /// Indicates amount it costs a couple to be on the team.
        public DataColumn ColumnCoupleCostsPeriodIntl;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild1CostsPeriodIntl;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild2CostsPeriodIntl;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild3CostsPeriodIntl;
        /// Indicates the joining charge for adults.
        public DataColumn ColumnAdultJoiningChargeIntl;
        /// Indicates the joining charge for couples.
        public DataColumn ColumnCoupleJoiningChargeIntl;
        /// Indicates the joining charge for a child.
        public DataColumn ColumnChildJoiningChargeIntl;
        /// Indicates the local currency.
        public DataColumn ColumnLocalCurrencyCode;
        /// The charge period for the unit, eg. monthly, quarterly.
        public DataColumn ColumnChargePeriod;
        /// Indicates amount it costs a single to be on the team.
        public DataColumn ColumnSingleCostsPeriodBase;
        /// Indicates amount it costs a couple to be on the team.
        public DataColumn ColumnCoupleCostsPeriodBase;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild1CostsPeriodBase;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild2CostsPeriodBase;
        /// Indicates amount it costs a child to be on the team.
        public DataColumn ColumnChild3CostsPeriodBase;
        /// Indicates the joining charge for adults.
        public DataColumn ColumnAdultJoiningChargeBase;
        /// Indicates the joining charge for couples.
        public DataColumn ColumnCoupleJoiningChargeBase;
        /// Indicates the joining charge for a child.
        public DataColumn ColumnChildJoiningChargeBase;
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
        public static short TableId = 264;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmUnitCost", "um_unit_cost",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "ValidFromDate", "um_valid_from_date_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "SingleCostsPeriodIntl", "um_single_costs_period_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(3, "CoupleCostsPeriodIntl", "um_couple_costs_period_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(4, "Child1CostsPeriodIntl", "um_child1_costs_period_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(5, "Child2CostsPeriodIntl", "um_child2_costs_period_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "Child3CostsPeriodIntl", "um_child3_costs_period_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(7, "AdultJoiningChargeIntl", "um_adult_joining_charge_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(8, "CoupleJoiningChargeIntl", "um_couple_joining_charge_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(9, "ChildJoiningChargeIntl", "um_child_joining_charge_intl_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(10, "LocalCurrencyCode", "a_local_currency_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(11, "ChargePeriod", "um_charge_period_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(12, "SingleCostsPeriodBase", "um_single_costs_period_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(13, "CoupleCostsPeriodBase", "um_couple_costs_period_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(14, "Child1CostsPeriodBase", "um_child1_costs_period_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(15, "Child2CostsPeriodBase", "um_child2_costs_period_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(16, "Child3CostsPeriodBase", "um_child3_costs_period_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(17, "AdultJoiningChargeBase", "um_adult_joining_charge_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(18, "CoupleJoiningChargeBase", "um_couple_joining_charge_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(19, "ChildJoiningChargeBase", "um_child_joining_charge_base_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(20, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(21, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(22, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(23, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(24, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public UmUnitCostTable() :
                base("UmUnitCost")
        {
        }

        /// constructor
        public UmUnitCostTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmUnitCostTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("um_valid_from_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("um_single_costs_period_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_couple_costs_period_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child1_costs_period_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child2_costs_period_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child3_costs_period_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_adult_joining_charge_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_couple_joining_charge_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child_joining_charge_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_local_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_charge_period_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_single_costs_period_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_couple_costs_period_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child1_costs_period_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child2_costs_period_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child3_costs_period_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_adult_joining_charge_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_couple_joining_charge_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("um_child_joining_charge_base_n", typeof(Double)));
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
            this.ColumnValidFromDate = this.Columns["um_valid_from_date_d"];
            this.ColumnSingleCostsPeriodIntl = this.Columns["um_single_costs_period_intl_n"];
            this.ColumnCoupleCostsPeriodIntl = this.Columns["um_couple_costs_period_intl_n"];
            this.ColumnChild1CostsPeriodIntl = this.Columns["um_child1_costs_period_intl_n"];
            this.ColumnChild2CostsPeriodIntl = this.Columns["um_child2_costs_period_intl_n"];
            this.ColumnChild3CostsPeriodIntl = this.Columns["um_child3_costs_period_intl_n"];
            this.ColumnAdultJoiningChargeIntl = this.Columns["um_adult_joining_charge_intl_n"];
            this.ColumnCoupleJoiningChargeIntl = this.Columns["um_couple_joining_charge_intl_n"];
            this.ColumnChildJoiningChargeIntl = this.Columns["um_child_joining_charge_intl_n"];
            this.ColumnLocalCurrencyCode = this.Columns["a_local_currency_code_c"];
            this.ColumnChargePeriod = this.Columns["um_charge_period_c"];
            this.ColumnSingleCostsPeriodBase = this.Columns["um_single_costs_period_base_n"];
            this.ColumnCoupleCostsPeriodBase = this.Columns["um_couple_costs_period_base_n"];
            this.ColumnChild1CostsPeriodBase = this.Columns["um_child1_costs_period_base_n"];
            this.ColumnChild2CostsPeriodBase = this.Columns["um_child2_costs_period_base_n"];
            this.ColumnChild3CostsPeriodBase = this.Columns["um_child3_costs_period_base_n"];
            this.ColumnAdultJoiningChargeBase = this.Columns["um_adult_joining_charge_base_n"];
            this.ColumnCoupleJoiningChargeBase = this.Columns["um_couple_joining_charge_base_n"];
            this.ColumnChildJoiningChargeBase = this.Columns["um_child_joining_charge_base_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmUnitCostRow this[int i]
        {
            get
            {
                return ((UmUnitCostRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmUnitCostRow NewRowTyped(bool AWithDefaultValues)
        {
            UmUnitCostRow ret = ((UmUnitCostRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmUnitCostRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmUnitCostRow(builder);
        }

        /// get typed set of changes
        public UmUnitCostTable GetChangesTyped()
        {
            return ((UmUnitCostTable)(base.GetChangesTypedInternal()));
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
        public static string GetValidFromDateDBName()
        {
            return "um_valid_from_date_d";
        }

        /// get character length for column
        public static short GetValidFromDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSingleCostsPeriodIntlDBName()
        {
            return "um_single_costs_period_intl_n";
        }

        /// get character length for column
        public static short GetSingleCostsPeriodIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCoupleCostsPeriodIntlDBName()
        {
            return "um_couple_costs_period_intl_n";
        }

        /// get character length for column
        public static short GetCoupleCostsPeriodIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild1CostsPeriodIntlDBName()
        {
            return "um_child1_costs_period_intl_n";
        }

        /// get character length for column
        public static short GetChild1CostsPeriodIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild2CostsPeriodIntlDBName()
        {
            return "um_child2_costs_period_intl_n";
        }

        /// get character length for column
        public static short GetChild2CostsPeriodIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild3CostsPeriodIntlDBName()
        {
            return "um_child3_costs_period_intl_n";
        }

        /// get character length for column
        public static short GetChild3CostsPeriodIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAdultJoiningChargeIntlDBName()
        {
            return "um_adult_joining_charge_intl_n";
        }

        /// get character length for column
        public static short GetAdultJoiningChargeIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCoupleJoiningChargeIntlDBName()
        {
            return "um_couple_joining_charge_intl_n";
        }

        /// get character length for column
        public static short GetCoupleJoiningChargeIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChildJoiningChargeIntlDBName()
        {
            return "um_child_joining_charge_intl_n";
        }

        /// get character length for column
        public static short GetChildJoiningChargeIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetLocalCurrencyCodeDBName()
        {
            return "a_local_currency_code_c";
        }

        /// get character length for column
        public static short GetLocalCurrencyCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetChargePeriodDBName()
        {
            return "um_charge_period_c";
        }

        /// get character length for column
        public static short GetChargePeriodLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetSingleCostsPeriodBaseDBName()
        {
            return "um_single_costs_period_base_n";
        }

        /// get character length for column
        public static short GetSingleCostsPeriodBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCoupleCostsPeriodBaseDBName()
        {
            return "um_couple_costs_period_base_n";
        }

        /// get character length for column
        public static short GetCoupleCostsPeriodBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild1CostsPeriodBaseDBName()
        {
            return "um_child1_costs_period_base_n";
        }

        /// get character length for column
        public static short GetChild1CostsPeriodBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild2CostsPeriodBaseDBName()
        {
            return "um_child2_costs_period_base_n";
        }

        /// get character length for column
        public static short GetChild2CostsPeriodBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChild3CostsPeriodBaseDBName()
        {
            return "um_child3_costs_period_base_n";
        }

        /// get character length for column
        public static short GetChild3CostsPeriodBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAdultJoiningChargeBaseDBName()
        {
            return "um_adult_joining_charge_base_n";
        }

        /// get character length for column
        public static short GetAdultJoiningChargeBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetCoupleJoiningChargeBaseDBName()
        {
            return "um_couple_joining_charge_base_n";
        }

        /// get character length for column
        public static short GetCoupleJoiningChargeBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetChildJoiningChargeBaseDBName()
        {
            return "um_child_joining_charge_base_n";
        }

        /// get character length for column
        public static short GetChildJoiningChargeBaseLength()
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

    /// Details pertaining to the costs of being on in the unit.
    [Serializable()]
    public class UmUnitCostRow : System.Data.DataRow
    {
        private UmUnitCostTable myTable;

        /// Constructor
        public UmUnitCostRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmUnitCostTable)(this.Table));
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

        /// Date from which these costs are applicable.
        public System.DateTime ValidFromDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidFromDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValidFromDate)
                            || (((System.DateTime)(this[this.myTable.ColumnValidFromDate])) != value)))
                {
                    this[this.myTable.ColumnValidFromDate] = value;
                }
            }
        }

        /// Indicates amount it costs a single to be on the team.
        public Double SingleCostsPeriodIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSingleCostsPeriodIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSingleCostsPeriodIntl)
                            || (((Double)(this[this.myTable.ColumnSingleCostsPeriodIntl])) != value)))
                {
                    this[this.myTable.ColumnSingleCostsPeriodIntl] = value;
                }
            }
        }

        /// Indicates amount it costs a couple to be on the team.
        public Double CoupleCostsPeriodIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCoupleCostsPeriodIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCoupleCostsPeriodIntl)
                            || (((Double)(this[this.myTable.ColumnCoupleCostsPeriodIntl])) != value)))
                {
                    this[this.myTable.ColumnCoupleCostsPeriodIntl] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child1CostsPeriodIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild1CostsPeriodIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild1CostsPeriodIntl)
                            || (((Double)(this[this.myTable.ColumnChild1CostsPeriodIntl])) != value)))
                {
                    this[this.myTable.ColumnChild1CostsPeriodIntl] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child2CostsPeriodIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild2CostsPeriodIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild2CostsPeriodIntl)
                            || (((Double)(this[this.myTable.ColumnChild2CostsPeriodIntl])) != value)))
                {
                    this[this.myTable.ColumnChild2CostsPeriodIntl] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child3CostsPeriodIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild3CostsPeriodIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild3CostsPeriodIntl)
                            || (((Double)(this[this.myTable.ColumnChild3CostsPeriodIntl])) != value)))
                {
                    this[this.myTable.ColumnChild3CostsPeriodIntl] = value;
                }
            }
        }

        /// Indicates the joining charge for adults.
        public Double AdultJoiningChargeIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAdultJoiningChargeIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAdultJoiningChargeIntl)
                            || (((Double)(this[this.myTable.ColumnAdultJoiningChargeIntl])) != value)))
                {
                    this[this.myTable.ColumnAdultJoiningChargeIntl] = value;
                }
            }
        }

        /// Indicates the joining charge for couples.
        public Double CoupleJoiningChargeIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCoupleJoiningChargeIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCoupleJoiningChargeIntl)
                            || (((Double)(this[this.myTable.ColumnCoupleJoiningChargeIntl])) != value)))
                {
                    this[this.myTable.ColumnCoupleJoiningChargeIntl] = value;
                }
            }
        }

        /// Indicates the joining charge for a child.
        public Double ChildJoiningChargeIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChildJoiningChargeIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChildJoiningChargeIntl)
                            || (((Double)(this[this.myTable.ColumnChildJoiningChargeIntl])) != value)))
                {
                    this[this.myTable.ColumnChildJoiningChargeIntl] = value;
                }
            }
        }

        /// Indicates the local currency.
        public String LocalCurrencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocalCurrencyCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLocalCurrencyCode)
                            || (((String)(this[this.myTable.ColumnLocalCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnLocalCurrencyCode] = value;
                }
            }
        }

        /// The charge period for the unit, eg. monthly, quarterly.
        public String ChargePeriod
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChargePeriod.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnChargePeriod)
                            || (((String)(this[this.myTable.ColumnChargePeriod])) != value)))
                {
                    this[this.myTable.ColumnChargePeriod] = value;
                }
            }
        }

        /// Indicates amount it costs a single to be on the team.
        public Double SingleCostsPeriodBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSingleCostsPeriodBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSingleCostsPeriodBase)
                            || (((Double)(this[this.myTable.ColumnSingleCostsPeriodBase])) != value)))
                {
                    this[this.myTable.ColumnSingleCostsPeriodBase] = value;
                }
            }
        }

        /// Indicates amount it costs a couple to be on the team.
        public Double CoupleCostsPeriodBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCoupleCostsPeriodBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCoupleCostsPeriodBase)
                            || (((Double)(this[this.myTable.ColumnCoupleCostsPeriodBase])) != value)))
                {
                    this[this.myTable.ColumnCoupleCostsPeriodBase] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child1CostsPeriodBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild1CostsPeriodBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild1CostsPeriodBase)
                            || (((Double)(this[this.myTable.ColumnChild1CostsPeriodBase])) != value)))
                {
                    this[this.myTable.ColumnChild1CostsPeriodBase] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child2CostsPeriodBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild2CostsPeriodBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild2CostsPeriodBase)
                            || (((Double)(this[this.myTable.ColumnChild2CostsPeriodBase])) != value)))
                {
                    this[this.myTable.ColumnChild2CostsPeriodBase] = value;
                }
            }
        }

        /// Indicates amount it costs a child to be on the team.
        public Double Child3CostsPeriodBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChild3CostsPeriodBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChild3CostsPeriodBase)
                            || (((Double)(this[this.myTable.ColumnChild3CostsPeriodBase])) != value)))
                {
                    this[this.myTable.ColumnChild3CostsPeriodBase] = value;
                }
            }
        }

        /// Indicates the joining charge for adults.
        public Double AdultJoiningChargeBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAdultJoiningChargeBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAdultJoiningChargeBase)
                            || (((Double)(this[this.myTable.ColumnAdultJoiningChargeBase])) != value)))
                {
                    this[this.myTable.ColumnAdultJoiningChargeBase] = value;
                }
            }
        }

        /// Indicates the joining charge for couples.
        public Double CoupleJoiningChargeBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCoupleJoiningChargeBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCoupleJoiningChargeBase)
                            || (((Double)(this[this.myTable.ColumnCoupleJoiningChargeBase])) != value)))
                {
                    this[this.myTable.ColumnCoupleJoiningChargeBase] = value;
                }
            }
        }

        /// Indicates the joining charge for a child.
        public Double ChildJoiningChargeBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChildJoiningChargeBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChildJoiningChargeBase)
                            || (((Double)(this[this.myTable.ColumnChildJoiningChargeBase])) != value)))
                {
                    this[this.myTable.ColumnChildJoiningChargeBase] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnValidFromDate);
            this[this.myTable.ColumnSingleCostsPeriodIntl.Ordinal] = 0;
            this[this.myTable.ColumnCoupleCostsPeriodIntl.Ordinal] = 0;
            this[this.myTable.ColumnChild1CostsPeriodIntl.Ordinal] = 0;
            this[this.myTable.ColumnChild2CostsPeriodIntl.Ordinal] = 0;
            this[this.myTable.ColumnChild3CostsPeriodIntl.Ordinal] = 0;
            this[this.myTable.ColumnAdultJoiningChargeIntl.Ordinal] = 0;
            this[this.myTable.ColumnCoupleJoiningChargeIntl.Ordinal] = 0;
            this[this.myTable.ColumnChildJoiningChargeIntl.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnLocalCurrencyCode);
            this.SetNull(this.myTable.ColumnChargePeriod);
            this[this.myTable.ColumnSingleCostsPeriodBase.Ordinal] = 0;
            this[this.myTable.ColumnCoupleCostsPeriodBase.Ordinal] = 0;
            this[this.myTable.ColumnChild1CostsPeriodBase.Ordinal] = 0;
            this[this.myTable.ColumnChild2CostsPeriodBase.Ordinal] = 0;
            this[this.myTable.ColumnChild3CostsPeriodBase.Ordinal] = 0;
            this[this.myTable.ColumnAdultJoiningChargeBase.Ordinal] = 0;
            this[this.myTable.ColumnCoupleJoiningChargeBase.Ordinal] = 0;
            this[this.myTable.ColumnChildJoiningChargeBase.Ordinal] = 0;
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
        public bool IsValidFromDateNull()
        {
            return this.IsNull(this.myTable.ColumnValidFromDate);
        }

        /// assign NULL value
        public void SetValidFromDateNull()
        {
            this.SetNull(this.myTable.ColumnValidFromDate);
        }

        /// test for NULL value
        public bool IsSingleCostsPeriodIntlNull()
        {
            return this.IsNull(this.myTable.ColumnSingleCostsPeriodIntl);
        }

        /// assign NULL value
        public void SetSingleCostsPeriodIntlNull()
        {
            this.SetNull(this.myTable.ColumnSingleCostsPeriodIntl);
        }

        /// test for NULL value
        public bool IsCoupleCostsPeriodIntlNull()
        {
            return this.IsNull(this.myTable.ColumnCoupleCostsPeriodIntl);
        }

        /// assign NULL value
        public void SetCoupleCostsPeriodIntlNull()
        {
            this.SetNull(this.myTable.ColumnCoupleCostsPeriodIntl);
        }

        /// test for NULL value
        public bool IsChild1CostsPeriodIntlNull()
        {
            return this.IsNull(this.myTable.ColumnChild1CostsPeriodIntl);
        }

        /// assign NULL value
        public void SetChild1CostsPeriodIntlNull()
        {
            this.SetNull(this.myTable.ColumnChild1CostsPeriodIntl);
        }

        /// test for NULL value
        public bool IsChild2CostsPeriodIntlNull()
        {
            return this.IsNull(this.myTable.ColumnChild2CostsPeriodIntl);
        }

        /// assign NULL value
        public void SetChild2CostsPeriodIntlNull()
        {
            this.SetNull(this.myTable.ColumnChild2CostsPeriodIntl);
        }

        /// test for NULL value
        public bool IsChild3CostsPeriodIntlNull()
        {
            return this.IsNull(this.myTable.ColumnChild3CostsPeriodIntl);
        }

        /// assign NULL value
        public void SetChild3CostsPeriodIntlNull()
        {
            this.SetNull(this.myTable.ColumnChild3CostsPeriodIntl);
        }

        /// test for NULL value
        public bool IsAdultJoiningChargeIntlNull()
        {
            return this.IsNull(this.myTable.ColumnAdultJoiningChargeIntl);
        }

        /// assign NULL value
        public void SetAdultJoiningChargeIntlNull()
        {
            this.SetNull(this.myTable.ColumnAdultJoiningChargeIntl);
        }

        /// test for NULL value
        public bool IsCoupleJoiningChargeIntlNull()
        {
            return this.IsNull(this.myTable.ColumnCoupleJoiningChargeIntl);
        }

        /// assign NULL value
        public void SetCoupleJoiningChargeIntlNull()
        {
            this.SetNull(this.myTable.ColumnCoupleJoiningChargeIntl);
        }

        /// test for NULL value
        public bool IsChildJoiningChargeIntlNull()
        {
            return this.IsNull(this.myTable.ColumnChildJoiningChargeIntl);
        }

        /// assign NULL value
        public void SetChildJoiningChargeIntlNull()
        {
            this.SetNull(this.myTable.ColumnChildJoiningChargeIntl);
        }

        /// test for NULL value
        public bool IsLocalCurrencyCodeNull()
        {
            return this.IsNull(this.myTable.ColumnLocalCurrencyCode);
        }

        /// assign NULL value
        public void SetLocalCurrencyCodeNull()
        {
            this.SetNull(this.myTable.ColumnLocalCurrencyCode);
        }

        /// test for NULL value
        public bool IsChargePeriodNull()
        {
            return this.IsNull(this.myTable.ColumnChargePeriod);
        }

        /// assign NULL value
        public void SetChargePeriodNull()
        {
            this.SetNull(this.myTable.ColumnChargePeriod);
        }

        /// test for NULL value
        public bool IsSingleCostsPeriodBaseNull()
        {
            return this.IsNull(this.myTable.ColumnSingleCostsPeriodBase);
        }

        /// assign NULL value
        public void SetSingleCostsPeriodBaseNull()
        {
            this.SetNull(this.myTable.ColumnSingleCostsPeriodBase);
        }

        /// test for NULL value
        public bool IsCoupleCostsPeriodBaseNull()
        {
            return this.IsNull(this.myTable.ColumnCoupleCostsPeriodBase);
        }

        /// assign NULL value
        public void SetCoupleCostsPeriodBaseNull()
        {
            this.SetNull(this.myTable.ColumnCoupleCostsPeriodBase);
        }

        /// test for NULL value
        public bool IsChild1CostsPeriodBaseNull()
        {
            return this.IsNull(this.myTable.ColumnChild1CostsPeriodBase);
        }

        /// assign NULL value
        public void SetChild1CostsPeriodBaseNull()
        {
            this.SetNull(this.myTable.ColumnChild1CostsPeriodBase);
        }

        /// test for NULL value
        public bool IsChild2CostsPeriodBaseNull()
        {
            return this.IsNull(this.myTable.ColumnChild2CostsPeriodBase);
        }

        /// assign NULL value
        public void SetChild2CostsPeriodBaseNull()
        {
            this.SetNull(this.myTable.ColumnChild2CostsPeriodBase);
        }

        /// test for NULL value
        public bool IsChild3CostsPeriodBaseNull()
        {
            return this.IsNull(this.myTable.ColumnChild3CostsPeriodBase);
        }

        /// assign NULL value
        public void SetChild3CostsPeriodBaseNull()
        {
            this.SetNull(this.myTable.ColumnChild3CostsPeriodBase);
        }

        /// test for NULL value
        public bool IsAdultJoiningChargeBaseNull()
        {
            return this.IsNull(this.myTable.ColumnAdultJoiningChargeBase);
        }

        /// assign NULL value
        public void SetAdultJoiningChargeBaseNull()
        {
            this.SetNull(this.myTable.ColumnAdultJoiningChargeBase);
        }

        /// test for NULL value
        public bool IsCoupleJoiningChargeBaseNull()
        {
            return this.IsNull(this.myTable.ColumnCoupleJoiningChargeBase);
        }

        /// assign NULL value
        public void SetCoupleJoiningChargeBaseNull()
        {
            this.SetNull(this.myTable.ColumnCoupleJoiningChargeBase);
        }

        /// test for NULL value
        public bool IsChildJoiningChargeBaseNull()
        {
            return this.IsNull(this.myTable.ColumnChildJoiningChargeBase);
        }

        /// assign NULL value
        public void SetChildJoiningChargeBaseNull()
        {
            this.SetNull(this.myTable.ColumnChildJoiningChargeBase);
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

    /// Details pertaining to evaluation of the unit.
    [Serializable()]
    public class UmUnitEvaluationTable : TTypedDataTable
    {
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Indicates the date of the evaluation.
        public DataColumn ColumnDateOfEvaluation;
        /// The evaluation number is generated from a database sequence
        public DataColumn ColumnEvaluationNumber;
        /// Indicates whether the evaluator is married, single, etc.
        public DataColumn ColumnEvaluatorFamilyStatus;
        /// The name of the evaluator's home country.
        public DataColumn ColumnEvaluatorHomeCountry;
        /// Age of the person conduction the unit evaluation.
        public DataColumn ColumnEvaluatorAge;
        ///
        public DataColumn ColumnEvaluatorSex;
        /// Data regarding the unit evaluation.
        public DataColumn ColumnUnitEvaluationData;
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
        public static short TableId = 265;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UmUnitEvaluation", "um_unit_evaluation",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "DateOfEvaluation", "um_date_of_evaluation_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "EvaluationNumber", "um_evaluation_number_n", OdbcType.Decimal, 14, true),
                    new TTypedColumnInfo(3, "EvaluatorFamilyStatus", "um_evaluator_family_status_c", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(4, "EvaluatorHomeCountry", "p_evaluator_home_country_c", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(5, "EvaluatorAge", "um_evaluator_age_n", OdbcType.Decimal, 4, true),
                    new TTypedColumnInfo(6, "EvaluatorSex", "um_evaluator_sex_c", OdbcType.VarChar, 2, true),
                    new TTypedColumnInfo(7, "UnitEvaluationData", "um_unit_evaluation_data_c", OdbcType.VarChar, 160, false),
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
        public UmUnitEvaluationTable() :
                base("UmUnitEvaluation")
        {
        }

        /// constructor
        public UmUnitEvaluationTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public UmUnitEvaluationTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("um_date_of_evaluation_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("um_evaluation_number_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("um_evaluator_family_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_evaluator_home_country_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_evaluator_age_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("um_evaluator_sex_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("um_unit_evaluation_data_c", typeof(String)));
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
            this.ColumnDateOfEvaluation = this.Columns["um_date_of_evaluation_d"];
            this.ColumnEvaluationNumber = this.Columns["um_evaluation_number_n"];
            this.ColumnEvaluatorFamilyStatus = this.Columns["um_evaluator_family_status_c"];
            this.ColumnEvaluatorHomeCountry = this.Columns["p_evaluator_home_country_c"];
            this.ColumnEvaluatorAge = this.Columns["um_evaluator_age_n"];
            this.ColumnEvaluatorSex = this.Columns["um_evaluator_sex_c"];
            this.ColumnUnitEvaluationData = this.Columns["um_unit_evaluation_data_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public UmUnitEvaluationRow this[int i]
        {
            get
            {
                return ((UmUnitEvaluationRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public UmUnitEvaluationRow NewRowTyped(bool AWithDefaultValues)
        {
            UmUnitEvaluationRow ret = ((UmUnitEvaluationRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public UmUnitEvaluationRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new UmUnitEvaluationRow(builder);
        }

        /// get typed set of changes
        public UmUnitEvaluationTable GetChangesTyped()
        {
            return ((UmUnitEvaluationTable)(base.GetChangesTypedInternal()));
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
        public static string GetDateOfEvaluationDBName()
        {
            return "um_date_of_evaluation_d";
        }

        /// get character length for column
        public static short GetDateOfEvaluationLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEvaluationNumberDBName()
        {
            return "um_evaluation_number_n";
        }

        /// get character length for column
        public static short GetEvaluationNumberLength()
        {
            return 14;
        }

        /// get the name of the field in the database for this column
        public static string GetEvaluatorFamilyStatusDBName()
        {
            return "um_evaluator_family_status_c";
        }

        /// get character length for column
        public static short GetEvaluatorFamilyStatusLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetEvaluatorHomeCountryDBName()
        {
            return "p_evaluator_home_country_c";
        }

        /// get character length for column
        public static short GetEvaluatorHomeCountryLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetEvaluatorAgeDBName()
        {
            return "um_evaluator_age_n";
        }

        /// get character length for column
        public static short GetEvaluatorAgeLength()
        {
            return 4;
        }

        /// get the name of the field in the database for this column
        public static string GetEvaluatorSexDBName()
        {
            return "um_evaluator_sex_c";
        }

        /// get character length for column
        public static short GetEvaluatorSexLength()
        {
            return 2;
        }

        /// get the name of the field in the database for this column
        public static string GetUnitEvaluationDataDBName()
        {
            return "um_unit_evaluation_data_c";
        }

        /// get character length for column
        public static short GetUnitEvaluationDataLength()
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

    /// Details pertaining to evaluation of the unit.
    [Serializable()]
    public class UmUnitEvaluationRow : System.Data.DataRow
    {
        private UmUnitEvaluationTable myTable;

        /// Constructor
        public UmUnitEvaluationRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((UmUnitEvaluationTable)(this.Table));
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

        /// Indicates the date of the evaluation.
        public System.DateTime DateOfEvaluation
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfEvaluation.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateOfEvaluation)
                            || (((System.DateTime)(this[this.myTable.ColumnDateOfEvaluation])) != value)))
                {
                    this[this.myTable.ColumnDateOfEvaluation] = value;
                }
            }
        }

        /// The evaluation number is generated from a database sequence
        public Decimal EvaluationNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEvaluationNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEvaluationNumber)
                            || (((Decimal)(this[this.myTable.ColumnEvaluationNumber])) != value)))
                {
                    this[this.myTable.ColumnEvaluationNumber] = value;
                }
            }
        }

        /// Indicates whether the evaluator is married, single, etc.
        public String EvaluatorFamilyStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEvaluatorFamilyStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnEvaluatorFamilyStatus)
                            || (((String)(this[this.myTable.ColumnEvaluatorFamilyStatus])) != value)))
                {
                    this[this.myTable.ColumnEvaluatorFamilyStatus] = value;
                }
            }
        }

        /// The name of the evaluator's home country.
        public String EvaluatorHomeCountry
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEvaluatorHomeCountry.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnEvaluatorHomeCountry)
                            || (((String)(this[this.myTable.ColumnEvaluatorHomeCountry])) != value)))
                {
                    this[this.myTable.ColumnEvaluatorHomeCountry] = value;
                }
            }
        }

        /// Age of the person conduction the unit evaluation.
        public Decimal EvaluatorAge
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEvaluatorAge.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEvaluatorAge)
                            || (((Decimal)(this[this.myTable.ColumnEvaluatorAge])) != value)))
                {
                    this[this.myTable.ColumnEvaluatorAge] = value;
                }
            }
        }

        ///
        public String EvaluatorSex
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEvaluatorSex.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnEvaluatorSex)
                            || (((String)(this[this.myTable.ColumnEvaluatorSex])) != value)))
                {
                    this[this.myTable.ColumnEvaluatorSex] = value;
                }
            }
        }

        /// Data regarding the unit evaluation.
        public String UnitEvaluationData
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitEvaluationData.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnUnitEvaluationData)
                            || (((String)(this[this.myTable.ColumnUnitEvaluationData])) != value)))
                {
                    this[this.myTable.ColumnUnitEvaluationData] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnDateOfEvaluation);
            this[this.myTable.ColumnEvaluationNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnEvaluatorFamilyStatus);
            this.SetNull(this.myTable.ColumnEvaluatorHomeCountry);
            this[this.myTable.ColumnEvaluatorAge.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnEvaluatorSex);
            this.SetNull(this.myTable.ColumnUnitEvaluationData);
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
        public bool IsDateOfEvaluationNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfEvaluation);
        }

        /// assign NULL value
        public void SetDateOfEvaluationNull()
        {
            this.SetNull(this.myTable.ColumnDateOfEvaluation);
        }

        /// test for NULL value
        public bool IsEvaluationNumberNull()
        {
            return this.IsNull(this.myTable.ColumnEvaluationNumber);
        }

        /// assign NULL value
        public void SetEvaluationNumberNull()
        {
            this.SetNull(this.myTable.ColumnEvaluationNumber);
        }

        /// test for NULL value
        public bool IsEvaluatorFamilyStatusNull()
        {
            return this.IsNull(this.myTable.ColumnEvaluatorFamilyStatus);
        }

        /// assign NULL value
        public void SetEvaluatorFamilyStatusNull()
        {
            this.SetNull(this.myTable.ColumnEvaluatorFamilyStatus);
        }

        /// test for NULL value
        public bool IsEvaluatorHomeCountryNull()
        {
            return this.IsNull(this.myTable.ColumnEvaluatorHomeCountry);
        }

        /// assign NULL value
        public void SetEvaluatorHomeCountryNull()
        {
            this.SetNull(this.myTable.ColumnEvaluatorHomeCountry);
        }

        /// test for NULL value
        public bool IsEvaluatorAgeNull()
        {
            return this.IsNull(this.myTable.ColumnEvaluatorAge);
        }

        /// assign NULL value
        public void SetEvaluatorAgeNull()
        {
            this.SetNull(this.myTable.ColumnEvaluatorAge);
        }

        /// test for NULL value
        public bool IsEvaluatorSexNull()
        {
            return this.IsNull(this.myTable.ColumnEvaluatorSex);
        }

        /// assign NULL value
        public void SetEvaluatorSexNull()
        {
            this.SetNull(this.myTable.ColumnEvaluatorSex);
        }

        /// test for NULL value
        public bool IsUnitEvaluationDataNull()
        {
            return this.IsNull(this.myTable.ColumnUnitEvaluationData);
        }

        /// assign NULL value
        public void SetUnitEvaluationDataNull()
        {
            this.SetNull(this.myTable.ColumnUnitEvaluationData);
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