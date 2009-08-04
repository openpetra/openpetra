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
namespace Ict.Petra.Shared.MFinance.Gift.Data
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

    /// Media"" types of money received.  Eg: Cash, Check Credit Card.
    [Serializable()]
    public class AMethodOfPaymentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 142;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentDescId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentTypeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnProcessToCallId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnSpecialMethodOfPmtId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnActiveId = 5;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AMethodOfPayment", "a_method_of_payment",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MethodOfPaymentCode", "a_method_of_payment_code_c", "Method of Payment", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "MethodOfPaymentDesc", "a_method_of_payment_desc_c", "Description", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "MethodOfPaymentType", "a_method_of_payment_type_c", "a_method_of_payment_type_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(3, "ProcessToCall", "a_process_to_call_c", "Process to Call", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(4, "SpecialMethodOfPmt", "a_special_method_of_pmt_l", "Special Method of Payment", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(5, "Active", "a_active_l", "Active", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

        /// constructor
        public AMethodOfPaymentTable() :
                base("AMethodOfPayment")
        {
        }

        /// constructor
        public AMethodOfPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AMethodOfPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is how the partner paid. EgCash, Cheque etc
        public DataColumn ColumnMethodOfPaymentCode;
        /// This is a short description which is 32 charcters long
        public DataColumn ColumnMethodOfPaymentDesc;
        ///
        public DataColumn ColumnMethodOfPaymentType;
        /// The filename of the process to call
        public DataColumn ColumnProcessToCall;
        ///
        public DataColumn ColumnSpecialMethodOfPmt;
        /// Shows whether this code is active
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
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_process_to_call_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_special_method_of_pmt_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnMethodOfPaymentDesc = this.Columns["a_method_of_payment_desc_c"];
            this.ColumnMethodOfPaymentType = this.Columns["a_method_of_payment_type_c"];
            this.ColumnProcessToCall = this.Columns["a_process_to_call_c"];
            this.ColumnSpecialMethodOfPmt = this.Columns["a_special_method_of_pmt_l"];
            this.ColumnActive = this.Columns["a_active_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnMethodOfPaymentCode};
        }

        /// Access a typed row by index
        public AMethodOfPaymentRow this[int i]
        {
            get
            {
                return ((AMethodOfPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AMethodOfPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AMethodOfPaymentRow ret = ((AMethodOfPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AMethodOfPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AMethodOfPaymentRow(builder);
        }

        /// get typed set of changes
        public AMethodOfPaymentTable GetChangesTyped()
        {
            return ((AMethodOfPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AMethodOfPayment";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_method_of_payment";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "a_method_of_payment_code_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentDescDBName()
        {
            return "a_method_of_payment_desc_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentDescLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentTypeDBName()
        {
            return "a_method_of_payment_type_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentTypeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetProcessToCallDBName()
        {
            return "a_process_to_call_c";
        }

        /// get character length for column
        public static short GetProcessToCallLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetSpecialMethodOfPmtDBName()
        {
            return "a_special_method_of_pmt_l";
        }

        /// get character length for column
        public static short GetSpecialMethodOfPmtLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "a_active_l";
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

    /// Media"" types of money received.  Eg: Cash, Check Credit Card.
    [Serializable()]
    public class AMethodOfPaymentRow : System.Data.DataRow
    {
        private AMethodOfPaymentTable myTable;

        /// Constructor
        public AMethodOfPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AMethodOfPaymentTable)(this.Table));
        }

        /// This is how the partner paid. EgCash, Cheque etc
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
                }
            }
        }

        /// This is a short description which is 32 charcters long
        public String MethodOfPaymentDesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentDesc.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentDesc)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentDesc])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentDesc] = value;
                }
            }
        }

        ///
        public String MethodOfPaymentType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentType)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentType])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentType] = value;
                }
            }
        }

        /// The filename of the process to call
        public String ProcessToCall
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnProcessToCall.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnProcessToCall)
                            || (((String)(this[this.myTable.ColumnProcessToCall])) != value)))
                {
                    this[this.myTable.ColumnProcessToCall] = value;
                }
            }
        }

        ///
        public Boolean SpecialMethodOfPmt
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSpecialMethodOfPmt.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSpecialMethodOfPmt)
                            || (((Boolean)(this[this.myTable.ColumnSpecialMethodOfPmt])) != value)))
                {
                    this[this.myTable.ColumnSpecialMethodOfPmt] = value;
                }
            }
        }

        /// Shows whether this code is active
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
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentDesc);
            this.SetNull(this.myTable.ColumnMethodOfPaymentType);
            this.SetNull(this.myTable.ColumnProcessToCall);
            this[this.myTable.ColumnSpecialMethodOfPmt.Ordinal] = false;
            this[this.myTable.ColumnActive.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentDescNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentDesc);
        }

        /// assign NULL value
        public void SetMethodOfPaymentDescNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentDesc);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentTypeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentType);
        }

        /// assign NULL value
        public void SetMethodOfPaymentTypeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentType);
        }

        /// test for NULL value
        public bool IsProcessToCallNull()
        {
            return this.IsNull(this.myTable.ColumnProcessToCall);
        }

        /// assign NULL value
        public void SetProcessToCallNull()
        {
            this.SetNull(this.myTable.ColumnProcessToCall);
        }

        /// test for NULL value
        public bool IsSpecialMethodOfPmtNull()
        {
            return this.IsNull(this.myTable.ColumnSpecialMethodOfPmt);
        }

        /// assign NULL value
        public void SetSpecialMethodOfPmtNull()
        {
            this.SetNull(this.myTable.ColumnSpecialMethodOfPmt);
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

    /// This is used to track a partner's reason for contacting the organisation/sending money. Divided into Motivation Detail codes.
    [Serializable()]
    public class AMotivationGroupTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 143;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnGroupStatusId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupDescLocalId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnRestrictedId = 5;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AMotivationGroup", "a_motivation_group",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "MotivationGroupDescription", "a_motivation_group_description_c", "Description", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(3, "GroupStatus", "a_group_status_l", "Motivation Group Status", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "MotivationGroupDescLocal", "a_motivation_group_desc_local_c", "Description (local)", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(5, "Restricted", "a_restricted_l", "Motivation Group Restricted", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public AMotivationGroupTable() :
                base("AMotivationGroup")
        {
        }

        /// constructor
        public AMotivationGroupTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AMotivationGroupTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// This defines a motivation group.
        public DataColumn ColumnMotivationGroupCode;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnMotivationGroupDescription;
        /// Defines whether the motivation group is still in use
        public DataColumn ColumnGroupStatus;
        /// This is a long description and is 80 characters long in the local language.
        public DataColumn ColumnMotivationGroupDescLocal;
        /// Indicates whether or not the motivation has restricted access. If it does then the access will be controlled by s_group_motivation
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_group_status_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_desc_local_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationGroupDescription = this.Columns["a_motivation_group_description_c"];
            this.ColumnGroupStatus = this.Columns["a_group_status_l"];
            this.ColumnMotivationGroupDescLocal = this.Columns["a_motivation_group_desc_local_c"];
            this.ColumnRestricted = this.Columns["a_restricted_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnMotivationGroupCode};
        }

        /// Access a typed row by index
        public AMotivationGroupRow this[int i]
        {
            get
            {
                return ((AMotivationGroupRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AMotivationGroupRow NewRowTyped(bool AWithDefaultValues)
        {
            AMotivationGroupRow ret = ((AMotivationGroupRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AMotivationGroupRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AMotivationGroupRow(builder);
        }

        /// get typed set of changes
        public AMotivationGroupTable GetChangesTyped()
        {
            return ((AMotivationGroupTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AMotivationGroup";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_motivation_group";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
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
        public static string GetMotivationGroupDescriptionDBName()
        {
            return "a_motivation_group_description_c";
        }

        /// get character length for column
        public static short GetMotivationGroupDescriptionLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetGroupStatusDBName()
        {
            return "a_group_status_l";
        }

        /// get character length for column
        public static short GetGroupStatusLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationGroupDescLocalDBName()
        {
            return "a_motivation_group_desc_local_c";
        }

        /// get character length for column
        public static short GetMotivationGroupDescLocalLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "a_restricted_l";
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

    /// This is used to track a partner's reason for contacting the organisation/sending money. Divided into Motivation Detail codes.
    [Serializable()]
    public class AMotivationGroupRow : System.Data.DataRow
    {
        private AMotivationGroupTable myTable;

        /// Constructor
        public AMotivationGroupRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AMotivationGroupTable)(this.Table));
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

        /// This is a long description and is 80 characters long.
        public String MotivationGroupDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMotivationGroupDescription)
                            || (((String)(this[this.myTable.ColumnMotivationGroupDescription])) != value)))
                {
                    this[this.myTable.ColumnMotivationGroupDescription] = value;
                }
            }
        }

        /// Defines whether the motivation group is still in use
        public Boolean GroupStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGroupStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGroupStatus)
                            || (((Boolean)(this[this.myTable.ColumnGroupStatus])) != value)))
                {
                    this[this.myTable.ColumnGroupStatus] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long in the local language.
        public String MotivationGroupDescLocal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupDescLocal.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMotivationGroupDescLocal)
                            || (((String)(this[this.myTable.ColumnMotivationGroupDescLocal])) != value)))
                {
                    this[this.myTable.ColumnMotivationGroupDescLocal] = value;
                }
            }
        }

        /// Indicates whether or not the motivation has restricted access. If it does then the access will be controlled by s_group_motivation
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
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationGroupDescription);
            this[this.myTable.ColumnGroupStatus.Ordinal] = true;
            this.SetNull(this.myTable.ColumnMotivationGroupDescLocal);
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
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
        public bool IsMotivationGroupDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationGroupDescription);
        }

        /// assign NULL value
        public void SetMotivationGroupDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnMotivationGroupDescription);
        }

        /// test for NULL value
        public bool IsGroupStatusNull()
        {
            return this.IsNull(this.myTable.ColumnGroupStatus);
        }

        /// assign NULL value
        public void SetGroupStatusNull()
        {
            this.SetNull(this.myTable.ColumnGroupStatus);
        }

        /// test for NULL value
        public bool IsMotivationGroupDescLocalNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationGroupDescLocal);
        }

        /// assign NULL value
        public void SetMotivationGroupDescLocalNull()
        {
            this.SetNull(this.myTable.ColumnMotivationGroupDescLocal);
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

    /// Used as a subdvision of motivation group. Details of the reason money has been received, where it is going (cost centre and account), and fees to be charged on it.
    [Serializable()]
    public class AMotivationDetailTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 144;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailAudienceId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailDescId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnAccountCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnCostCentreCodeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationStatusId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCostId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnBulkRateFlagId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnNextResponseStatusId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnActivatePartnerFlagId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberSentId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfResponsesId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnTargetNumberOfResponsesId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnTargetAmountId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountReceivedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientKeyId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnAutopopdescId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnTaxDeductableId = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailDescLocalId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnShortCodeId = 22;
        /// used for generic TTypedDataTable functions
        public static short ColumnRestrictedId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnExportToIntranetId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 26;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 27;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 28;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 29;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AMotivationDetail", "a_motivation_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", " Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "MotivationDetailAudience", "a_motivation_detail_audience_c", "Audience", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(4, "MotivationDetailDesc", "a_motivation_detail_desc_c", "Detail Description", OdbcType.VarChar, 160, true),
                    new TTypedColumnInfo(5, "AccountCode", "a_account_code_c", "Account Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(6, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(7, "MotivationStatus", "a_motivation_status_l", "Motivation Status", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(8, "MailingCost", "a_mailing_cost_n", "Mailing Cost", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(9, "BulkRateFlag", "a_bulk_rate_flag_l", "Bulk Rate Flag", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(10, "NextResponseStatus", "a_next_response_status_c", "Next Response Status", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(11, "ActivatePartnerFlag", "a_activate_partner_flag_l", "Activate Partner", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(12, "NumberSent", "a_number_sent_i", "Number Sent", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "NumberOfResponses", "a_number_of_responses_i", "Number of Responses", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(14, "TargetNumberOfResponses", "a_target_number_of_responses_i", "Target Number of Responses", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(15, "TargetAmount", "a_target_amount_n", "Target Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(16, "AmountReceived", "a_amount_received_n", "Amount Received", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(17, "RecipientKey", "p_recipient_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(18, "Autopopdesc", "a_autopopdesc_l", "Auto Populate Description", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(19, "Receipt", "a_receipt_l", "Print Receipt", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(20, "TaxDeductable", "a_tax_deductable_l", "Tax Deductable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(21, "MotivationDetailDescLocal", "a_motivation_detail_desc_local_c", "Detail Description (Local Language)", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(22, "ShortCode", "a_short_code_c", "Short Code", OdbcType.VarChar, 8, false),
                    new TTypedColumnInfo(23, "Restricted", "a_restricted_l", "Motivation Detail Restricted", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(24, "ExportToIntranet", "a_export_to_intranet_l", "Export to Intranet?", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(25, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(26, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(27, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(28, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(29, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public AMotivationDetailTable() :
                base("AMotivationDetail")
        {
        }

        /// constructor
        public AMotivationDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AMotivationDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// This defines a motivation group.
        public DataColumn ColumnMotivationGroupCode;
        /// This defines the motivation detail within a motivation group.
        public DataColumn ColumnMotivationDetailCode;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnMotivationDetailAudience;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnMotivationDetailDesc;
        /// This identifies the account the financial transaction must be stored against
        public DataColumn ColumnAccountCode;
        /// This identifies which cost centre an account is applied to
        public DataColumn ColumnCostCentreCode;
        /// Defines whether the motivation code is still in use
        public DataColumn ColumnMotivationStatus;
        /// This is a number of currency units
        public DataColumn ColumnMailingCost;
        /// Used to get a yes no response from the user
        public DataColumn ColumnBulkRateFlag;
        /// This defines what should happen next
        public DataColumn ColumnNextResponseStatus;
        /// Used to get a yes no response from the user
        public DataColumn ColumnActivatePartnerFlag;
        /// The number of items sent out in a mailing
        public DataColumn ColumnNumberSent;
        /// The number of items returned from a mailing
        public DataColumn ColumnNumberOfResponses;
        /// The target number of items returned from a mailing
        public DataColumn ColumnTargetNumberOfResponses;
        /// This is a number of currency units
        public DataColumn ColumnTargetAmount;
        /// This is a number of currency units
        public DataColumn ColumnAmountReceived;
        /// This is the partner key assigned to each partner. It consists of the ledger id followed by a computer generated six digit number.
        public DataColumn ColumnRecipientKey;
        /// A flag to automatically populate the description in the gift comment
        public DataColumn ColumnAutopopdesc;
        /// Whether receipts should be printed
        public DataColumn ColumnReceipt;
        /// Whether this gift is tax deductable
        public DataColumn ColumnTaxDeductable;
        /// This is a long description and is 80 characters long in the local language.
        public DataColumn ColumnMotivationDetailDescLocal;
        /// A short code for the motivation which can then be used on receipts
        public DataColumn ColumnShortCode;
        /// Indicates whether or not the motivation has restricted access. If it does then the access will be controlled by s_group_motivation
        public DataColumn ColumnRestricted;
        /// Whether or not gifts with this motivation should be exported to the worldwide Intranet (to help distinguish non-gifts like sales)
        public DataColumn ColumnExportToIntranet;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_audience_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_status_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_mailing_cost_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_bulk_rate_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_next_response_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_activate_partner_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_number_sent_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_responses_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_target_number_of_responses_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_target_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_received_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_autopopdesc_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_desc_local_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_short_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_export_to_intranet_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnMotivationDetailAudience = this.Columns["a_motivation_detail_audience_c"];
            this.ColumnMotivationDetailDesc = this.Columns["a_motivation_detail_desc_c"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnMotivationStatus = this.Columns["a_motivation_status_l"];
            this.ColumnMailingCost = this.Columns["a_mailing_cost_n"];
            this.ColumnBulkRateFlag = this.Columns["a_bulk_rate_flag_l"];
            this.ColumnNextResponseStatus = this.Columns["a_next_response_status_c"];
            this.ColumnActivatePartnerFlag = this.Columns["a_activate_partner_flag_l"];
            this.ColumnNumberSent = this.Columns["a_number_sent_i"];
            this.ColumnNumberOfResponses = this.Columns["a_number_of_responses_i"];
            this.ColumnTargetNumberOfResponses = this.Columns["a_target_number_of_responses_i"];
            this.ColumnTargetAmount = this.Columns["a_target_amount_n"];
            this.ColumnAmountReceived = this.Columns["a_amount_received_n"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnAutopopdesc = this.Columns["a_autopopdesc_l"];
            this.ColumnReceipt = this.Columns["a_receipt_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnMotivationDetailDescLocal = this.Columns["a_motivation_detail_desc_local_c"];
            this.ColumnShortCode = this.Columns["a_short_code_c"];
            this.ColumnRestricted = this.Columns["a_restricted_l"];
            this.ColumnExportToIntranet = this.Columns["a_export_to_intranet_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnLedgerNumber,ColumnMotivationGroupCode,ColumnMotivationDetailCode};
        }

        /// Access a typed row by index
        public AMotivationDetailRow this[int i]
        {
            get
            {
                return ((AMotivationDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AMotivationDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            AMotivationDetailRow ret = ((AMotivationDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AMotivationDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AMotivationDetailRow(builder);
        }

        /// get typed set of changes
        public AMotivationDetailTable GetChangesTyped()
        {
            return ((AMotivationDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AMotivationDetail";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_motivation_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
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
        public static string GetMotivationDetailAudienceDBName()
        {
            return "a_motivation_detail_audience_c";
        }

        /// get character length for column
        public static short GetMotivationDetailAudienceLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationDetailDescDBName()
        {
            return "a_motivation_detail_desc_c";
        }

        /// get character length for column
        public static short GetMotivationDetailDescLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetAccountCodeDBName()
        {
            return "a_account_code_c";
        }

        /// get character length for column
        public static short GetAccountCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetCostCentreCodeDBName()
        {
            return "a_cost_centre_code_c";
        }

        /// get character length for column
        public static short GetCostCentreCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationStatusDBName()
        {
            return "a_motivation_status_l";
        }

        /// get character length for column
        public static short GetMotivationStatusLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingCostDBName()
        {
            return "a_mailing_cost_n";
        }

        /// get character length for column
        public static short GetMailingCostLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBulkRateFlagDBName()
        {
            return "a_bulk_rate_flag_l";
        }

        /// get character length for column
        public static short GetBulkRateFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNextResponseStatusDBName()
        {
            return "a_next_response_status_c";
        }

        /// get character length for column
        public static short GetNextResponseStatusLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetActivatePartnerFlagDBName()
        {
            return "a_activate_partner_flag_l";
        }

        /// get character length for column
        public static short GetActivatePartnerFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberSentDBName()
        {
            return "a_number_sent_i";
        }

        /// get character length for column
        public static short GetNumberSentLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfResponsesDBName()
        {
            return "a_number_of_responses_i";
        }

        /// get character length for column
        public static short GetNumberOfResponsesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTargetNumberOfResponsesDBName()
        {
            return "a_target_number_of_responses_i";
        }

        /// get character length for column
        public static short GetTargetNumberOfResponsesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTargetAmountDBName()
        {
            return "a_target_amount_n";
        }

        /// get character length for column
        public static short GetTargetAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountReceivedDBName()
        {
            return "a_amount_received_n";
        }

        /// get character length for column
        public static short GetAmountReceivedLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientKeyDBName()
        {
            return "p_recipient_key_n";
        }

        /// get character length for column
        public static short GetRecipientKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetAutopopdescDBName()
        {
            return "a_autopopdesc_l";
        }

        /// get character length for column
        public static short GetAutopopdescLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptDBName()
        {
            return "a_receipt_l";
        }

        /// get character length for column
        public static short GetReceiptLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxDeductableDBName()
        {
            return "a_tax_deductable_l";
        }

        /// get character length for column
        public static short GetTaxDeductableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationDetailDescLocalDBName()
        {
            return "a_motivation_detail_desc_local_c";
        }

        /// get character length for column
        public static short GetMotivationDetailDescLocalLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetShortCodeDBName()
        {
            return "a_short_code_c";
        }

        /// get character length for column
        public static short GetShortCodeLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "a_restricted_l";
        }

        /// get character length for column
        public static short GetRestrictedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetExportToIntranetDBName()
        {
            return "a_export_to_intranet_l";
        }

        /// get character length for column
        public static short GetExportToIntranetLength()
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

    /// Used as a subdvision of motivation group. Details of the reason money has been received, where it is going (cost centre and account), and fees to be charged on it.
    [Serializable()]
    public class AMotivationDetailRow : System.Data.DataRow
    {
        private AMotivationDetailTable myTable;

        /// Constructor
        public AMotivationDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AMotivationDetailTable)(this.Table));
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

        /// This is a long description and is 80 characters long.
        public String MotivationDetailAudience
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailAudience.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMotivationDetailAudience)
                            || (((String)(this[this.myTable.ColumnMotivationDetailAudience])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailAudience] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String MotivationDetailDesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailDesc.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMotivationDetailDesc)
                            || (((String)(this[this.myTable.ColumnMotivationDetailDesc])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailDesc] = value;
                }
            }
        }

        /// This identifies the account the financial transaction must be stored against
        public String AccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAccountCode)
                            || (((String)(this[this.myTable.ColumnAccountCode])) != value)))
                {
                    this[this.myTable.ColumnAccountCode] = value;
                }
            }
        }

        /// This identifies which cost centre an account is applied to
        public String CostCentreCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostCentreCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCostCentreCode)
                            || (((String)(this[this.myTable.ColumnCostCentreCode])) != value)))
                {
                    this[this.myTable.ColumnCostCentreCode] = value;
                }
            }
        }

        /// Defines whether the motivation code is still in use
        public Boolean MotivationStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationStatus)
                            || (((Boolean)(this[this.myTable.ColumnMotivationStatus])) != value)))
                {
                    this[this.myTable.ColumnMotivationStatus] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double MailingCost
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
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMailingCost)
                            || (((Double)(this[this.myTable.ColumnMailingCost])) != value)))
                {
                    this[this.myTable.ColumnMailingCost] = value;
                }
            }
        }

        /// Used to get a yes no response from the user
        public Boolean BulkRateFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBulkRateFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBulkRateFlag)
                            || (((Boolean)(this[this.myTable.ColumnBulkRateFlag])) != value)))
                {
                    this[this.myTable.ColumnBulkRateFlag] = value;
                }
            }
        }

        /// This defines what should happen next
        public String NextResponseStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNextResponseStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnNextResponseStatus)
                            || (((String)(this[this.myTable.ColumnNextResponseStatus])) != value)))
                {
                    this[this.myTable.ColumnNextResponseStatus] = value;
                }
            }
        }

        /// Used to get a yes no response from the user
        public Boolean ActivatePartnerFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnActivatePartnerFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnActivatePartnerFlag)
                            || (((Boolean)(this[this.myTable.ColumnActivatePartnerFlag])) != value)))
                {
                    this[this.myTable.ColumnActivatePartnerFlag] = value;
                }
            }
        }

        /// The number of items sent out in a mailing
        public Int32 NumberSent
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberSent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberSent)
                            || (((Int32)(this[this.myTable.ColumnNumberSent])) != value)))
                {
                    this[this.myTable.ColumnNumberSent] = value;
                }
            }
        }

        /// The number of items returned from a mailing
        public Int32 NumberOfResponses
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfResponses.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfResponses)
                            || (((Int32)(this[this.myTable.ColumnNumberOfResponses])) != value)))
                {
                    this[this.myTable.ColumnNumberOfResponses] = value;
                }
            }
        }

        /// The target number of items returned from a mailing
        public Int32 TargetNumberOfResponses
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTargetNumberOfResponses.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTargetNumberOfResponses)
                            || (((Int32)(this[this.myTable.ColumnTargetNumberOfResponses])) != value)))
                {
                    this[this.myTable.ColumnTargetNumberOfResponses] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double TargetAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTargetAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTargetAmount)
                            || (((Double)(this[this.myTable.ColumnTargetAmount])) != value)))
                {
                    this[this.myTable.ColumnTargetAmount] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double AmountReceived
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAmountReceived.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAmountReceived)
                            || (((Double)(this[this.myTable.ColumnAmountReceived])) != value)))
                {
                    this[this.myTable.ColumnAmountReceived] = value;
                }
            }
        }

        /// This is the partner key assigned to each partner. It consists of the ledger id followed by a computer generated six digit number.
        public Int64 RecipientKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientKey)
                            || (((Int64)(this[this.myTable.ColumnRecipientKey])) != value)))
                {
                    this[this.myTable.ColumnRecipientKey] = value;
                }
            }
        }

        /// A flag to automatically populate the description in the gift comment
        public Boolean Autopopdesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAutopopdesc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAutopopdesc)
                            || (((Boolean)(this[this.myTable.ColumnAutopopdesc])) != value)))
                {
                    this[this.myTable.ColumnAutopopdesc] = value;
                }
            }
        }

        /// Whether receipts should be printed
        public Boolean Receipt
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceipt.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReceipt)
                            || (((Boolean)(this[this.myTable.ColumnReceipt])) != value)))
                {
                    this[this.myTable.ColumnReceipt] = value;
                }
            }
        }

        /// Whether this gift is tax deductable
        public Boolean TaxDeductable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxDeductable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTaxDeductable)
                            || (((Boolean)(this[this.myTable.ColumnTaxDeductable])) != value)))
                {
                    this[this.myTable.ColumnTaxDeductable] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long in the local language.
        public String MotivationDetailDescLocal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailDescLocal.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMotivationDetailDescLocal)
                            || (((String)(this[this.myTable.ColumnMotivationDetailDescLocal])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailDescLocal] = value;
                }
            }
        }

        /// A short code for the motivation which can then be used on receipts
        public String ShortCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnShortCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnShortCode)
                            || (((String)(this[this.myTable.ColumnShortCode])) != value)))
                {
                    this[this.myTable.ColumnShortCode] = value;
                }
            }
        }

        /// Indicates whether or not the motivation has restricted access. If it does then the access will be controlled by s_group_motivation
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

        /// Whether or not gifts with this motivation should be exported to the worldwide Intranet (to help distinguish non-gifts like sales)
        public Boolean ExportToIntranet
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExportToIntranet.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExportToIntranet)
                            || (((Boolean)(this[this.myTable.ColumnExportToIntranet])) != value)))
                {
                    this[this.myTable.ColumnExportToIntranet] = value;
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
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnMotivationDetailAudience);
            this.SetNull(this.myTable.ColumnMotivationDetailDesc);
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this[this.myTable.ColumnMotivationStatus.Ordinal] = true;
            this[this.myTable.ColumnMailingCost.Ordinal] = 0;
            this[this.myTable.ColumnBulkRateFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnNextResponseStatus);
            this[this.myTable.ColumnActivatePartnerFlag.Ordinal] = false;
            this[this.myTable.ColumnNumberSent.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfResponses.Ordinal] = 0;
            this[this.myTable.ColumnTargetNumberOfResponses.Ordinal] = 0;
            this[this.myTable.ColumnTargetAmount.Ordinal] = 0;
            this[this.myTable.ColumnAmountReceived.Ordinal] = 0;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnAutopopdesc.Ordinal] = false;
            this[this.myTable.ColumnReceipt.Ordinal] = true;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this.SetNull(this.myTable.ColumnMotivationDetailDescLocal);
            this.SetNull(this.myTable.ColumnShortCode);
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnExportToIntranet.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
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
        public bool IsMotivationDetailAudienceNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailAudience);
        }

        /// assign NULL value
        public void SetMotivationDetailAudienceNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailAudience);
        }

        /// test for NULL value
        public bool IsMotivationDetailDescNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailDesc);
        }

        /// assign NULL value
        public void SetMotivationDetailDescNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailDesc);
        }

        /// test for NULL value
        public bool IsAccountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAccountCode);
        }

        /// assign NULL value
        public void SetAccountCodeNull()
        {
            this.SetNull(this.myTable.ColumnAccountCode);
        }

        /// test for NULL value
        public bool IsCostCentreCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCostCentreCode);
        }

        /// assign NULL value
        public void SetCostCentreCodeNull()
        {
            this.SetNull(this.myTable.ColumnCostCentreCode);
        }

        /// test for NULL value
        public bool IsMotivationStatusNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationStatus);
        }

        /// assign NULL value
        public void SetMotivationStatusNull()
        {
            this.SetNull(this.myTable.ColumnMotivationStatus);
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
        public bool IsBulkRateFlagNull()
        {
            return this.IsNull(this.myTable.ColumnBulkRateFlag);
        }

        /// assign NULL value
        public void SetBulkRateFlagNull()
        {
            this.SetNull(this.myTable.ColumnBulkRateFlag);
        }

        /// test for NULL value
        public bool IsNextResponseStatusNull()
        {
            return this.IsNull(this.myTable.ColumnNextResponseStatus);
        }

        /// assign NULL value
        public void SetNextResponseStatusNull()
        {
            this.SetNull(this.myTable.ColumnNextResponseStatus);
        }

        /// test for NULL value
        public bool IsActivatePartnerFlagNull()
        {
            return this.IsNull(this.myTable.ColumnActivatePartnerFlag);
        }

        /// assign NULL value
        public void SetActivatePartnerFlagNull()
        {
            this.SetNull(this.myTable.ColumnActivatePartnerFlag);
        }

        /// test for NULL value
        public bool IsNumberSentNull()
        {
            return this.IsNull(this.myTable.ColumnNumberSent);
        }

        /// assign NULL value
        public void SetNumberSentNull()
        {
            this.SetNull(this.myTable.ColumnNumberSent);
        }

        /// test for NULL value
        public bool IsNumberOfResponsesNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfResponses);
        }

        /// assign NULL value
        public void SetNumberOfResponsesNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfResponses);
        }

        /// test for NULL value
        public bool IsTargetNumberOfResponsesNull()
        {
            return this.IsNull(this.myTable.ColumnTargetNumberOfResponses);
        }

        /// assign NULL value
        public void SetTargetNumberOfResponsesNull()
        {
            this.SetNull(this.myTable.ColumnTargetNumberOfResponses);
        }

        /// test for NULL value
        public bool IsTargetAmountNull()
        {
            return this.IsNull(this.myTable.ColumnTargetAmount);
        }

        /// assign NULL value
        public void SetTargetAmountNull()
        {
            this.SetNull(this.myTable.ColumnTargetAmount);
        }

        /// test for NULL value
        public bool IsAmountReceivedNull()
        {
            return this.IsNull(this.myTable.ColumnAmountReceived);
        }

        /// assign NULL value
        public void SetAmountReceivedNull()
        {
            this.SetNull(this.myTable.ColumnAmountReceived);
        }

        /// test for NULL value
        public bool IsRecipientKeyNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientKey);
        }

        /// assign NULL value
        public void SetRecipientKeyNull()
        {
            this.SetNull(this.myTable.ColumnRecipientKey);
        }

        /// test for NULL value
        public bool IsAutopopdescNull()
        {
            return this.IsNull(this.myTable.ColumnAutopopdesc);
        }

        /// assign NULL value
        public void SetAutopopdescNull()
        {
            this.SetNull(this.myTable.ColumnAutopopdesc);
        }

        /// test for NULL value
        public bool IsReceiptNull()
        {
            return this.IsNull(this.myTable.ColumnReceipt);
        }

        /// assign NULL value
        public void SetReceiptNull()
        {
            this.SetNull(this.myTable.ColumnReceipt);
        }

        /// test for NULL value
        public bool IsTaxDeductableNull()
        {
            return this.IsNull(this.myTable.ColumnTaxDeductable);
        }

        /// assign NULL value
        public void SetTaxDeductableNull()
        {
            this.SetNull(this.myTable.ColumnTaxDeductable);
        }

        /// test for NULL value
        public bool IsMotivationDetailDescLocalNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailDescLocal);
        }

        /// assign NULL value
        public void SetMotivationDetailDescLocalNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailDescLocal);
        }

        /// test for NULL value
        public bool IsShortCodeNull()
        {
            return this.IsNull(this.myTable.ColumnShortCode);
        }

        /// assign NULL value
        public void SetShortCodeNull()
        {
            this.SetNull(this.myTable.ColumnShortCode);
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
        public bool IsExportToIntranetNull()
        {
            return this.IsNull(this.myTable.ColumnExportToIntranet);
        }

        /// assign NULL value
        public void SetExportToIntranetNull()
        {
            this.SetNull(this.myTable.ColumnExportToIntranet);
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

    /// motivation details can have several fees
    [Serializable()]
    public class AMotivationDetailFeeTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 145;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnFeeCodeId = 3;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AMotivationDetailFee", "a_motivation_detail_fee",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "MotivationGroupCode", "a_motivation_group_code_c", "a_motivation_group_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "MotivationDetailCode", "a_motivation_detail_code_c", "a_motivation_detail_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "FeeCode", "a_fee_code_c", "a_fee_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public AMotivationDetailFeeTable() :
                base("AMotivationDetailFee")
        {
        }

        /// constructor
        public AMotivationDetailFeeTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AMotivationDetailFeeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnLedgerNumber;
        ///
        public DataColumn ColumnMotivationGroupCode;
        ///
        public DataColumn ColumnMotivationDetailCode;
        ///
        public DataColumn ColumnFeeCode;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_fee_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnFeeCode = this.Columns["a_fee_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnMotivationGroupCode,ColumnMotivationDetailCode,ColumnFeeCode};
        }

        /// Access a typed row by index
        public AMotivationDetailFeeRow this[int i]
        {
            get
            {
                return ((AMotivationDetailFeeRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AMotivationDetailFeeRow NewRowTyped(bool AWithDefaultValues)
        {
            AMotivationDetailFeeRow ret = ((AMotivationDetailFeeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AMotivationDetailFeeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AMotivationDetailFeeRow(builder);
        }

        /// get typed set of changes
        public AMotivationDetailFeeTable GetChangesTyped()
        {
            return ((AMotivationDetailFeeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AMotivationDetailFee";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_motivation_detail_fee";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
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
        public static string GetFeeCodeDBName()
        {
            return "a_fee_code_c";
        }

        /// get character length for column
        public static short GetFeeCodeLength()
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

    /// motivation details can have several fees
    [Serializable()]
    public class AMotivationDetailFeeRow : System.Data.DataRow
    {
        private AMotivationDetailFeeTable myTable;

        /// Constructor
        public AMotivationDetailFeeRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AMotivationDetailFeeTable)(this.Table));
        }

        ///
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

        ///
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

        ///
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

        ///
        public String FeeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFeeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnFeeCode)
                            || (((String)(this[this.myTable.ColumnFeeCode])) != value)))
                {
                    this[this.myTable.ColumnFeeCode] = value;
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
            this.SetNull(this.myTable.ColumnLedgerNumber);
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnFeeCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
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
        public bool IsFeeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnFeeCode);
        }

        /// assign NULL value
        public void SetFeeCodeNull()
        {
            this.SetNull(this.myTable.ColumnFeeCode);
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

    /// Templates of gift batches which can be copied into the gift system.
    [Serializable()]
    public class ARecurringGiftBatchTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 161;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnHashTotalId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchTotalId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastGiftNumberId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnCurrencyCodeId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankCostCentreId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTypeId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 10;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ARecurringGiftBatch", "a_recurring_gift_batch",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "BatchDescription", "a_batch_description_c", "Batch description", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(3, "HashTotal", "a_hash_total_n", "Hash Total", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(4, "BatchTotal", "a_batch_total_n", "Batch Total", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(5, "BankAccountCode", "a_bank_account_code_c", "Bank Account", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(6, "LastGiftNumber", "a_last_gift_number_i", "Last Gift", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "CurrencyCode", "a_currency_code_c", "Gift Transaction Currency", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "BankCostCentre", "a_bank_cost_centre_c", "Cost Centre Code", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "GiftType", "a_gift_type_c", "Gift Type", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(10, "MethodOfPaymentCode", "a_method_of_payment_code_c", "Method of Payment", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(11, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(13, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(14, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(15, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public ARecurringGiftBatchTable() :
                base("ARecurringGiftBatch")
        {
        }

        /// constructor
        public ARecurringGiftBatchTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public ARecurringGiftBatchTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// ledger number
        public DataColumn ColumnLedgerNumber;
        /// Gift batch number
        public DataColumn ColumnBatchNumber;
        /// gift batch description
        public DataColumn ColumnBatchDescription;
        /// hash total for the gift batch
        public DataColumn ColumnHashTotal;
        /// total for the gift batch
        public DataColumn ColumnBatchTotal;
        /// bank account code which this batch is for
        public DataColumn ColumnBankAccountCode;
        /// last gift number of the batch
        public DataColumn ColumnLastGiftNumber;
        /// This defines which currency is being used
        public DataColumn ColumnCurrencyCode;
        /// This identifies which cost centre is applied to the bank
        public DataColumn ColumnBankCostCentre;
        /// What type of gift is this? a gift or a gift in kind generally
        public DataColumn ColumnGiftType;
        /// This is how the partner paid. EgCash, Cheque etc
        public DataColumn ColumnMethodOfPaymentCode;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_hash_total_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_total_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_last_gift_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_cost_centre_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnBatchDescription = this.Columns["a_batch_description_c"];
            this.ColumnHashTotal = this.Columns["a_hash_total_n"];
            this.ColumnBatchTotal = this.Columns["a_batch_total_n"];
            this.ColumnBankAccountCode = this.Columns["a_bank_account_code_c"];
            this.ColumnLastGiftNumber = this.Columns["a_last_gift_number_i"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnBankCostCentre = this.Columns["a_bank_cost_centre_c"];
            this.ColumnGiftType = this.Columns["a_gift_type_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnBatchNumber};
        }

        /// Access a typed row by index
        public ARecurringGiftBatchRow this[int i]
        {
            get
            {
                return ((ARecurringGiftBatchRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public ARecurringGiftBatchRow NewRowTyped(bool AWithDefaultValues)
        {
            ARecurringGiftBatchRow ret = ((ARecurringGiftBatchRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public ARecurringGiftBatchRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ARecurringGiftBatchRow(builder);
        }

        /// get typed set of changes
        public ARecurringGiftBatchTable GetChangesTyped()
        {
            return ((ARecurringGiftBatchTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ARecurringGiftBatch";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_recurring_gift_batch";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchDescriptionDBName()
        {
            return "a_batch_description_c";
        }

        /// get character length for column
        public static short GetBatchDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetHashTotalDBName()
        {
            return "a_hash_total_n";
        }

        /// get character length for column
        public static short GetHashTotalLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchTotalDBName()
        {
            return "a_batch_total_n";
        }

        /// get character length for column
        public static short GetBatchTotalLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountCodeDBName()
        {
            return "a_bank_account_code_c";
        }

        /// get character length for column
        public static short GetBankAccountCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetLastGiftNumberDBName()
        {
            return "a_last_gift_number_i";
        }

        /// get character length for column
        public static short GetLastGiftNumberLength()
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
        public static string GetBankCostCentreDBName()
        {
            return "a_bank_cost_centre_c";
        }

        /// get character length for column
        public static short GetBankCostCentreLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTypeDBName()
        {
            return "a_gift_type_c";
        }

        /// get character length for column
        public static short GetGiftTypeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "a_method_of_payment_code_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
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

    /// Templates of gift batches which can be copied into the gift system.
    [Serializable()]
    public class ARecurringGiftBatchRow : System.Data.DataRow
    {
        private ARecurringGiftBatchTable myTable;

        /// Constructor
        public ARecurringGiftBatchRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((ARecurringGiftBatchTable)(this.Table));
        }

        /// ledger number
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

        /// Gift batch number
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// gift batch description
        public String BatchDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBatchDescription)
                            || (((String)(this[this.myTable.ColumnBatchDescription])) != value)))
                {
                    this[this.myTable.ColumnBatchDescription] = value;
                }
            }
        }

        /// hash total for the gift batch
        public Double HashTotal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHashTotal.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHashTotal)
                            || (((Double)(this[this.myTable.ColumnHashTotal])) != value)))
                {
                    this[this.myTable.ColumnHashTotal] = value;
                }
            }
        }

        /// total for the gift batch
        public Double BatchTotal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchTotal.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchTotal)
                            || (((Double)(this[this.myTable.ColumnBatchTotal])) != value)))
                {
                    this[this.myTable.ColumnBatchTotal] = value;
                }
            }
        }

        /// bank account code which this batch is for
        public String BankAccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccountCode)
                            || (((String)(this[this.myTable.ColumnBankAccountCode])) != value)))
                {
                    this[this.myTable.ColumnBankAccountCode] = value;
                }
            }
        }

        /// last gift number of the batch
        public Int32 LastGiftNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastGiftNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastGiftNumber)
                            || (((Int32)(this[this.myTable.ColumnLastGiftNumber])) != value)))
                {
                    this[this.myTable.ColumnLastGiftNumber] = value;
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

        /// This identifies which cost centre is applied to the bank
        public String BankCostCentre
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankCostCentre.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankCostCentre)
                            || (((String)(this[this.myTable.ColumnBankCostCentre])) != value)))
                {
                    this[this.myTable.ColumnBankCostCentre] = value;
                }
            }
        }

        /// What type of gift is this? a gift or a gift in kind generally
        public String GiftType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftType)
                            || (((String)(this[this.myTable.ColumnGiftType])) != value)))
                {
                    this[this.myTable.ColumnGiftType] = value;
                }
            }
        }

        /// This is how the partner paid. EgCash, Cheque etc
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBatchDescription);
            this[this.myTable.ColumnHashTotal.Ordinal] = 0;
            this[this.myTable.ColumnBatchTotal.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBankAccountCode);
            this[this.myTable.ColumnLastGiftNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnBankCostCentre);
            this[this.myTable.ColumnGiftType.Ordinal] = "Gift";
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsBatchDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnBatchDescription);
        }

        /// assign NULL value
        public void SetBatchDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnBatchDescription);
        }

        /// test for NULL value
        public bool IsHashTotalNull()
        {
            return this.IsNull(this.myTable.ColumnHashTotal);
        }

        /// assign NULL value
        public void SetHashTotalNull()
        {
            this.SetNull(this.myTable.ColumnHashTotal);
        }

        /// test for NULL value
        public bool IsBatchTotalNull()
        {
            return this.IsNull(this.myTable.ColumnBatchTotal);
        }

        /// assign NULL value
        public void SetBatchTotalNull()
        {
            this.SetNull(this.myTable.ColumnBatchTotal);
        }

        /// test for NULL value
        public bool IsBankAccountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccountCode);
        }

        /// assign NULL value
        public void SetBankAccountCodeNull()
        {
            this.SetNull(this.myTable.ColumnBankAccountCode);
        }

        /// test for NULL value
        public bool IsLastGiftNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLastGiftNumber);
        }

        /// assign NULL value
        public void SetLastGiftNumberNull()
        {
            this.SetNull(this.myTable.ColumnLastGiftNumber);
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
        public bool IsBankCostCentreNull()
        {
            return this.IsNull(this.myTable.ColumnBankCostCentre);
        }

        /// assign NULL value
        public void SetBankCostCentreNull()
        {
            this.SetNull(this.myTable.ColumnBankCostCentre);
        }

        /// test for NULL value
        public bool IsGiftTypeNull()
        {
            return this.IsNull(this.myTable.ColumnGiftType);
        }

        /// assign NULL value
        public void SetGiftTypeNull()
        {
            this.SetNull(this.myTable.ColumnGiftType);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
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

    /// Templates of donor gift information which can be copied into the gift system with recurring gift batches.
    [Serializable()]
    public class ARecurringGiftTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 162;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTransactionNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptLetterCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfGivingCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastDetailNumberId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnReferenceId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankingDetailsKeyId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnChargeStatusId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastDebitId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnDebitDayId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnActiveId = 13;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ARecurringGift", "a_recurring_gift",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "ReceiptLetterCode", "a_receipt_letter_code_c", "Receipt Letter Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "MethodOfGivingCode", "a_method_of_giving_code_c", "Method Of Giving", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(5, "MethodOfPaymentCode", "a_method_of_payment_code_c", "Method of Payment", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "DonorKey", "p_donor_key_n", "Donor", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(7, "LastDetailNumber", "a_last_detail_number_i", "Last Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(8, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(9, "BankingDetailsKey", "p_banking_details_key_i", "Bank or Credit Card", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(10, "ChargeStatus", "a_charge_status_c", "CC Charge Status", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "LastDebit", "a_last_debit_d", "Date of last successfull donation", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "DebitDay", "a_debit_day_i", "Day of month to debit", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "Active", "a_active_l", "Active", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(14, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public ARecurringGiftTable() :
                base("ARecurringGift")
        {
        }

        /// constructor
        public ARecurringGiftTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public ARecurringGiftTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// identifes which batch a transaction belongs to
        public DataColumn ColumnBatchNumber;
        /// Identifies a transaction within a journal within a batch within a ledger
        public DataColumn ColumnGiftTransactionNumber;
        ///
        public DataColumn ColumnReceiptLetterCode;
        /// Defines how a gift is given
        public DataColumn ColumnMethodOfGivingCode;
        /// This is how the partner paid. Eg cash, Cheque etc
        public DataColumn ColumnMethodOfPaymentCode;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnDonorKey;
        /// Identifies the last gift detail entered
        public DataColumn ColumnLastDetailNumber;
        /// Reference number/code for the transaction
        public DataColumn ColumnReference;
        /// Bank or credit card account to use for making this gift transaction.
        public DataColumn ColumnBankingDetailsKey;
        /// Status of the credit card transaction
        public DataColumn ColumnChargeStatus;
        /// The last date that a successfull direct debit or credit card charge occurred for this gift
        public DataColumn ColumnLastDebit;
        /// The day of the month to make the recurring gift
        public DataColumn ColumnDebitDay;
        /// Whether the recurring gift should be made
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_letter_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_giving_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_donor_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_last_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_banking_details_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_last_debit_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_debit_day_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnReceiptLetterCode = this.Columns["a_receipt_letter_code_c"];
            this.ColumnMethodOfGivingCode = this.Columns["a_method_of_giving_code_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDonorKey = this.Columns["p_donor_key_n"];
            this.ColumnLastDetailNumber = this.Columns["a_last_detail_number_i"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnBankingDetailsKey = this.Columns["p_banking_details_key_i"];
            this.ColumnChargeStatus = this.Columns["a_charge_status_c"];
            this.ColumnLastDebit = this.Columns["a_last_debit_d"];
            this.ColumnDebitDay = this.Columns["a_debit_day_i"];
            this.ColumnActive = this.Columns["a_active_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber};
        }

        /// Access a typed row by index
        public ARecurringGiftRow this[int i]
        {
            get
            {
                return ((ARecurringGiftRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public ARecurringGiftRow NewRowTyped(bool AWithDefaultValues)
        {
            ARecurringGiftRow ret = ((ARecurringGiftRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public ARecurringGiftRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ARecurringGiftRow(builder);
        }

        /// get typed set of changes
        public ARecurringGiftTable GetChangesTyped()
        {
            return ((ARecurringGiftTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ARecurringGift";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_recurring_gift";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTransactionNumberDBName()
        {
            return "a_gift_transaction_number_i";
        }

        /// get character length for column
        public static short GetGiftTransactionNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptLetterCodeDBName()
        {
            return "a_receipt_letter_code_c";
        }

        /// get character length for column
        public static short GetReceiptLetterCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfGivingCodeDBName()
        {
            return "a_method_of_giving_code_c";
        }

        /// get character length for column
        public static short GetMethodOfGivingCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "a_method_of_payment_code_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDonorKeyDBName()
        {
            return "p_donor_key_n";
        }

        /// get character length for column
        public static short GetDonorKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetLastDetailNumberDBName()
        {
            return "a_last_detail_number_i";
        }

        /// get character length for column
        public static short GetLastDetailNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }

        /// get character length for column
        public static short GetReferenceLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetBankingDetailsKeyDBName()
        {
            return "p_banking_details_key_i";
        }

        /// get character length for column
        public static short GetBankingDetailsKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetChargeStatusDBName()
        {
            return "a_charge_status_c";
        }

        /// get character length for column
        public static short GetChargeStatusLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetLastDebitDBName()
        {
            return "a_last_debit_d";
        }

        /// get character length for column
        public static short GetLastDebitLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDebitDayDBName()
        {
            return "a_debit_day_i";
        }

        /// get character length for column
        public static short GetDebitDayLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "a_active_l";
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

    /// Templates of donor gift information which can be copied into the gift system with recurring gift batches.
    [Serializable()]
    public class ARecurringGiftRow : System.Data.DataRow
    {
        private ARecurringGiftTable myTable;

        /// Constructor
        public ARecurringGiftRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((ARecurringGiftTable)(this.Table));
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

        /// identifes which batch a transaction belongs to
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// Identifies a transaction within a journal within a batch within a ledger
        public Int32 GiftTransactionNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTransactionNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftTransactionNumber)
                            || (((Int32)(this[this.myTable.ColumnGiftTransactionNumber])) != value)))
                {
                    this[this.myTable.ColumnGiftTransactionNumber] = value;
                }
            }
        }

        ///
        public String ReceiptLetterCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptLetterCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReceiptLetterCode)
                            || (((String)(this[this.myTable.ColumnReceiptLetterCode])) != value)))
                {
                    this[this.myTable.ColumnReceiptLetterCode] = value;
                }
            }
        }

        /// Defines how a gift is given
        public String MethodOfGivingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfGivingCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfGivingCode)
                            || (((String)(this[this.myTable.ColumnMethodOfGivingCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfGivingCode] = value;
                }
            }
        }

        /// This is how the partner paid. Eg cash, Cheque etc
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
                }
            }
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 DonorKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorKey)
                            || (((Int64)(this[this.myTable.ColumnDonorKey])) != value)))
                {
                    this[this.myTable.ColumnDonorKey] = value;
                }
            }
        }

        /// Identifies the last gift detail entered
        public Int32 LastDetailNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastDetailNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastDetailNumber)
                            || (((Int32)(this[this.myTable.ColumnLastDetailNumber])) != value)))
                {
                    this[this.myTable.ColumnLastDetailNumber] = value;
                }
            }
        }

        /// Reference number/code for the transaction
        public String Reference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReference)
                            || (((String)(this[this.myTable.ColumnReference])) != value)))
                {
                    this[this.myTable.ColumnReference] = value;
                }
            }
        }

        /// Bank or credit card account to use for making this gift transaction.
        public Int32 BankingDetailsKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankingDetailsKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBankingDetailsKey)
                            || (((Int32)(this[this.myTable.ColumnBankingDetailsKey])) != value)))
                {
                    this[this.myTable.ColumnBankingDetailsKey] = value;
                }
            }
        }

        /// Status of the credit card transaction
        public String ChargeStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChargeStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnChargeStatus)
                            || (((String)(this[this.myTable.ColumnChargeStatus])) != value)))
                {
                    this[this.myTable.ColumnChargeStatus] = value;
                }
            }
        }

        /// The last date that a successfull direct debit or credit card charge occurred for this gift
        public System.DateTime LastDebit
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastDebit.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastDebit)
                            || (((System.DateTime)(this[this.myTable.ColumnLastDebit])) != value)))
                {
                    this[this.myTable.ColumnLastDebit] = value;
                }
            }
        }

        /// The day of the month to make the recurring gift
        public Int32 DebitDay
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDebitDay.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDebitDay)
                            || (((Int32)(this[this.myTable.ColumnDebitDay])) != value)))
                {
                    this[this.myTable.ColumnDebitDay] = value;
                }
            }
        }

        /// Whether the recurring gift should be made
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDonorKey.Ordinal] = 0;
            this[this.myTable.ColumnLastDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnReference);
            this[this.myTable.ColumnBankingDetailsKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnChargeStatus);
            this.SetNull(this.myTable.ColumnLastDebit);
            this[this.myTable.ColumnDebitDay.Ordinal] = 0;
            this[this.myTable.ColumnActive.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsGiftTransactionNumberNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// assign NULL value
        public void SetGiftTransactionNumberNull()
        {
            this.SetNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// test for NULL value
        public bool IsReceiptLetterCodeNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptLetterCode);
        }

        /// assign NULL value
        public void SetReceiptLetterCodeNull()
        {
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
        }

        /// test for NULL value
        public bool IsMethodOfGivingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// assign NULL value
        public void SetMethodOfGivingCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// test for NULL value
        public bool IsDonorKeyNull()
        {
            return this.IsNull(this.myTable.ColumnDonorKey);
        }

        /// assign NULL value
        public void SetDonorKeyNull()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
        }

        /// test for NULL value
        public bool IsLastDetailNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLastDetailNumber);
        }

        /// assign NULL value
        public void SetLastDetailNumberNull()
        {
            this.SetNull(this.myTable.ColumnLastDetailNumber);
        }

        /// test for NULL value
        public bool IsReferenceNull()
        {
            return this.IsNull(this.myTable.ColumnReference);
        }

        /// assign NULL value
        public void SetReferenceNull()
        {
            this.SetNull(this.myTable.ColumnReference);
        }

        /// test for NULL value
        public bool IsBankingDetailsKeyNull()
        {
            return this.IsNull(this.myTable.ColumnBankingDetailsKey);
        }

        /// assign NULL value
        public void SetBankingDetailsKeyNull()
        {
            this.SetNull(this.myTable.ColumnBankingDetailsKey);
        }

        /// test for NULL value
        public bool IsChargeStatusNull()
        {
            return this.IsNull(this.myTable.ColumnChargeStatus);
        }

        /// assign NULL value
        public void SetChargeStatusNull()
        {
            this.SetNull(this.myTable.ColumnChargeStatus);
        }

        /// test for NULL value
        public bool IsLastDebitNull()
        {
            return this.IsNull(this.myTable.ColumnLastDebit);
        }

        /// assign NULL value
        public void SetLastDebitNull()
        {
            this.SetNull(this.myTable.ColumnLastDebit);
        }

        /// test for NULL value
        public bool IsDebitDayNull()
        {
            return this.IsNull(this.myTable.ColumnDebitDay);
        }

        /// assign NULL value
        public void SetDebitDayNull()
        {
            this.SetNull(this.myTable.ColumnDebitDay);
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

    /// Store recipient information for the recurring gift.
    [Serializable()]
    public class ARecurringGiftDetailTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 163;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTransactionNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDetailNumberId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientLedgerNumberId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftAmountId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentOneTypeId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentOneId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnConfidentialGiftFlagId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnTaxDeductableId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientKeyId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnChargeFlagId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCodeId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentTwoTypeId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentTwoId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentThreeTypeId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentThreeId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnStartDonationsId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnEndDonationsId = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 22;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 25;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ARecurringGiftDetail", "a_recurring_gift_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Gift Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Gift Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "DetailNumber", "a_detail_number_i", "Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "RecipientLedgerNumber", "a_recipient_ledger_number_n", "Recipient Ledger", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(5, "GiftAmount", "a_gift_amount_n", "Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(7, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "CommentOneType", "a_comment_one_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "GiftCommentOne", "a_gift_comment_one_c", "Comment One", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(10, "ConfidentialGiftFlag", "a_confidential_gift_flag_l", "Confidential Gift", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(11, "TaxDeductable", "a_tax_deductable_l", "Tax Deductable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "RecipientKey", "p_recipient_key_n", "Recipient", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(13, "ChargeFlag", "a_charge_flag_l", "Charge Fee", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(15, "CommentTwoType", "a_comment_two_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(16, "GiftCommentTwo", "a_gift_comment_two_c", "Comment Two", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(17, "CommentThreeType", "a_comment_three_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(18, "GiftCommentThree", "a_gift_comment_three_c", "Comment Three", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(19, "StartDonations", "a_start_donations_d", "Date to begin donations", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(20, "EndDonations", "a_end_donations_d", "Date to end donations", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(21, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(22, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(23, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(24, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(25, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public ARecurringGiftDetailTable() :
                base("ARecurringGiftDetail")
        {
        }

        /// constructor
        public ARecurringGiftDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public ARecurringGiftDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// Number of the gift batch containing this detail.
        public DataColumn ColumnBatchNumber;
        /// Identifies a gift transaction within a gift batch.
        public DataColumn ColumnGiftTransactionNumber;
        /// Identifies a gift
        public DataColumn ColumnDetailNumber;
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnRecipientLedgerNumber;
        /// This is a number of currency units
        public DataColumn ColumnGiftAmount;
        /// This defines a motivation group.
        public DataColumn ColumnMotivationGroupCode;
        /// This defines the motivation detail within a motivation group.
        public DataColumn ColumnMotivationDetailCode;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentOneType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentOne;
        /// Defines whether the donor wishes the recipient to know who gave the gift
        public DataColumn ColumnConfidentialGiftFlag;
        /// Whether this gift is tax deductable
        public DataColumn ColumnTaxDeductable;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnRecipientKey;
        /// To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction.
        public DataColumn ColumnChargeFlag;
        /// Mailing Code
        public DataColumn ColumnMailingCode;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentTwoType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentTwo;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentThreeType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentThree;
        /// Date that donor wants to begin giving this recurring donation
        public DataColumn ColumnStartDonations;
        /// Date that donor wants to stop giving this recurring donation
        public DataColumn ColumnEndDonations;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_two_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_two_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_three_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_three_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_start_donations_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_end_donations_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnRecipientLedgerNumber = this.Columns["a_recipient_ledger_number_n"];
            this.ColumnGiftAmount = this.Columns["a_gift_amount_n"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnCommentOneType = this.Columns["a_comment_one_type_c"];
            this.ColumnGiftCommentOne = this.Columns["a_gift_comment_one_c"];
            this.ColumnConfidentialGiftFlag = this.Columns["a_confidential_gift_flag_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnChargeFlag = this.Columns["a_charge_flag_l"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnCommentTwoType = this.Columns["a_comment_two_type_c"];
            this.ColumnGiftCommentTwo = this.Columns["a_gift_comment_two_c"];
            this.ColumnCommentThreeType = this.Columns["a_comment_three_type_c"];
            this.ColumnGiftCommentThree = this.Columns["a_gift_comment_three_c"];
            this.ColumnStartDonations = this.Columns["a_start_donations_d"];
            this.ColumnEndDonations = this.Columns["a_end_donations_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber,ColumnDetailNumber};
        }

        /// Access a typed row by index
        public ARecurringGiftDetailRow this[int i]
        {
            get
            {
                return ((ARecurringGiftDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public ARecurringGiftDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            ARecurringGiftDetailRow ret = ((ARecurringGiftDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public ARecurringGiftDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ARecurringGiftDetailRow(builder);
        }

        /// get typed set of changes
        public ARecurringGiftDetailTable GetChangesTyped()
        {
            return ((ARecurringGiftDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ARecurringGiftDetail";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_recurring_gift_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTransactionNumberDBName()
        {
            return "a_gift_transaction_number_i";
        }

        /// get character length for column
        public static short GetGiftTransactionNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }

        /// get character length for column
        public static short GetDetailNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientLedgerNumberDBName()
        {
            return "a_recipient_ledger_number_n";
        }

        /// get character length for column
        public static short GetRecipientLedgerNumberLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftAmountDBName()
        {
            return "a_gift_amount_n";
        }

        /// get character length for column
        public static short GetGiftAmountLength()
        {
            return 24;
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
        public static string GetCommentOneTypeDBName()
        {
            return "a_comment_one_type_c";
        }

        /// get character length for column
        public static short GetCommentOneTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentOneDBName()
        {
            return "a_gift_comment_one_c";
        }

        /// get character length for column
        public static short GetGiftCommentOneLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetConfidentialGiftFlagDBName()
        {
            return "a_confidential_gift_flag_l";
        }

        /// get character length for column
        public static short GetConfidentialGiftFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxDeductableDBName()
        {
            return "a_tax_deductable_l";
        }

        /// get character length for column
        public static short GetTaxDeductableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientKeyDBName()
        {
            return "p_recipient_key_n";
        }

        /// get character length for column
        public static short GetRecipientKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetChargeFlagDBName()
        {
            return "a_charge_flag_l";
        }

        /// get character length for column
        public static short GetChargeFlagLength()
        {
            return -1;
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
        public static string GetCommentTwoTypeDBName()
        {
            return "a_comment_two_type_c";
        }

        /// get character length for column
        public static short GetCommentTwoTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentTwoDBName()
        {
            return "a_gift_comment_two_c";
        }

        /// get character length for column
        public static short GetGiftCommentTwoLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentThreeTypeDBName()
        {
            return "a_comment_three_type_c";
        }

        /// get character length for column
        public static short GetCommentThreeTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentThreeDBName()
        {
            return "a_gift_comment_three_c";
        }

        /// get character length for column
        public static short GetGiftCommentThreeLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetStartDonationsDBName()
        {
            return "a_start_donations_d";
        }

        /// get character length for column
        public static short GetStartDonationsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEndDonationsDBName()
        {
            return "a_end_donations_d";
        }

        /// get character length for column
        public static short GetEndDonationsLength()
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

    /// Store recipient information for the recurring gift.
    [Serializable()]
    public class ARecurringGiftDetailRow : System.Data.DataRow
    {
        private ARecurringGiftDetailTable myTable;

        /// Constructor
        public ARecurringGiftDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((ARecurringGiftDetailTable)(this.Table));
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

        /// Number of the gift batch containing this detail.
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// Identifies a gift transaction within a gift batch.
        public Int32 GiftTransactionNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTransactionNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftTransactionNumber)
                            || (((Int32)(this[this.myTable.ColumnGiftTransactionNumber])) != value)))
                {
                    this[this.myTable.ColumnGiftTransactionNumber] = value;
                }
            }
        }

        /// Identifies a gift
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

        /// This is used as a key field in most of the accounting system files
        public Int64 RecipientLedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientLedgerNumber)
                            || (((Int64)(this[this.myTable.ColumnRecipientLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnRecipientLedgerNumber] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double GiftAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftAmount)
                            || (((Double)(this[this.myTable.ColumnGiftAmount])) != value)))
                {
                    this[this.myTable.ColumnGiftAmount] = value;
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

        /// Used to decide whose reports will see this comment
        public String CommentOneType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentOneType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentOneType)
                            || (((String)(this[this.myTable.ColumnCommentOneType])) != value)))
                {
                    this[this.myTable.ColumnCommentOneType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentOne
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentOne.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentOne)
                            || (((String)(this[this.myTable.ColumnGiftCommentOne])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentOne] = value;
                }
            }
        }

        /// Defines whether the donor wishes the recipient to know who gave the gift
        public Boolean ConfidentialGiftFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConfidentialGiftFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConfidentialGiftFlag)
                            || (((Boolean)(this[this.myTable.ColumnConfidentialGiftFlag])) != value)))
                {
                    this[this.myTable.ColumnConfidentialGiftFlag] = value;
                }
            }
        }

        /// Whether this gift is tax deductable
        public Boolean TaxDeductable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxDeductable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTaxDeductable)
                            || (((Boolean)(this[this.myTable.ColumnTaxDeductable])) != value)))
                {
                    this[this.myTable.ColumnTaxDeductable] = value;
                }
            }
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 RecipientKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientKey)
                            || (((Int64)(this[this.myTable.ColumnRecipientKey])) != value)))
                {
                    this[this.myTable.ColumnRecipientKey] = value;
                }
            }
        }

        /// To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction.
        public Boolean ChargeFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChargeFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChargeFlag)
                            || (((Boolean)(this[this.myTable.ColumnChargeFlag])) != value)))
                {
                    this[this.myTable.ColumnChargeFlag] = value;
                }
            }
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

        /// Used to decide whose reports will see this comment
        public String CommentTwoType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentTwoType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentTwoType)
                            || (((String)(this[this.myTable.ColumnCommentTwoType])) != value)))
                {
                    this[this.myTable.ColumnCommentTwoType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentTwo
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentTwo.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentTwo)
                            || (((String)(this[this.myTable.ColumnGiftCommentTwo])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentTwo] = value;
                }
            }
        }

        /// Used to decide whose reports will see this comment
        public String CommentThreeType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentThreeType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentThreeType)
                            || (((String)(this[this.myTable.ColumnCommentThreeType])) != value)))
                {
                    this[this.myTable.ColumnCommentThreeType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentThree
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentThree.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentThree)
                            || (((String)(this[this.myTable.ColumnGiftCommentThree])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentThree] = value;
                }
            }
        }

        /// Date that donor wants to begin giving this recurring donation
        public System.DateTime StartDonations
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStartDonations.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStartDonations)
                            || (((System.DateTime)(this[this.myTable.ColumnStartDonations])) != value)))
                {
                    this[this.myTable.ColumnStartDonations] = value;
                }
            }
        }

        /// Date that donor wants to stop giving this recurring donation
        public System.DateTime EndDonations
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEndDonations.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEndDonations)
                            || (((System.DateTime)(this[this.myTable.ColumnEndDonations])) != value)))
                {
                    this[this.myTable.ColumnEndDonations] = value;
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this[this.myTable.ColumnRecipientLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnCommentOneType);
            this.SetNull(this.myTable.ColumnGiftCommentOne);
            this[this.myTable.ColumnConfidentialGiftFlag.Ordinal] = false;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnChargeFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnCommentTwoType);
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
            this.SetNull(this.myTable.ColumnCommentThreeType);
            this.SetNull(this.myTable.ColumnGiftCommentThree);
            this.SetNull(this.myTable.ColumnStartDonations);
            this.SetNull(this.myTable.ColumnEndDonations);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsGiftTransactionNumberNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// assign NULL value
        public void SetGiftTransactionNumberNull()
        {
            this.SetNull(this.myTable.ColumnGiftTransactionNumber);
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
        public bool IsRecipientLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientLedgerNumber);
        }

        /// assign NULL value
        public void SetRecipientLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnRecipientLedgerNumber);
        }

        /// test for NULL value
        public bool IsGiftAmountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftAmount);
        }

        /// assign NULL value
        public void SetGiftAmountNull()
        {
            this.SetNull(this.myTable.ColumnGiftAmount);
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
        public bool IsCommentOneTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentOneType);
        }

        /// assign NULL value
        public void SetCommentOneTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentOneType);
        }

        /// test for NULL value
        public bool IsGiftCommentOneNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentOne);
        }

        /// assign NULL value
        public void SetGiftCommentOneNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentOne);
        }

        /// test for NULL value
        public bool IsConfidentialGiftFlagNull()
        {
            return this.IsNull(this.myTable.ColumnConfidentialGiftFlag);
        }

        /// assign NULL value
        public void SetConfidentialGiftFlagNull()
        {
            this.SetNull(this.myTable.ColumnConfidentialGiftFlag);
        }

        /// test for NULL value
        public bool IsTaxDeductableNull()
        {
            return this.IsNull(this.myTable.ColumnTaxDeductable);
        }

        /// assign NULL value
        public void SetTaxDeductableNull()
        {
            this.SetNull(this.myTable.ColumnTaxDeductable);
        }

        /// test for NULL value
        public bool IsRecipientKeyNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientKey);
        }

        /// assign NULL value
        public void SetRecipientKeyNull()
        {
            this.SetNull(this.myTable.ColumnRecipientKey);
        }

        /// test for NULL value
        public bool IsChargeFlagNull()
        {
            return this.IsNull(this.myTable.ColumnChargeFlag);
        }

        /// assign NULL value
        public void SetChargeFlagNull()
        {
            this.SetNull(this.myTable.ColumnChargeFlag);
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
        public bool IsCommentTwoTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentTwoType);
        }

        /// assign NULL value
        public void SetCommentTwoTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentTwoType);
        }

        /// test for NULL value
        public bool IsGiftCommentTwoNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentTwo);
        }

        /// assign NULL value
        public void SetGiftCommentTwoNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
        }

        /// test for NULL value
        public bool IsCommentThreeTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentThreeType);
        }

        /// assign NULL value
        public void SetCommentThreeTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentThreeType);
        }

        /// test for NULL value
        public bool IsGiftCommentThreeNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentThree);
        }

        /// assign NULL value
        public void SetGiftCommentThreeNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentThree);
        }

        /// test for NULL value
        public bool IsStartDonationsNull()
        {
            return this.IsNull(this.myTable.ColumnStartDonations);
        }

        /// assign NULL value
        public void SetStartDonationsNull()
        {
            this.SetNull(this.myTable.ColumnStartDonations);
        }

        /// test for NULL value
        public bool IsEndDonationsNull()
        {
            return this.IsNull(this.myTable.ColumnEndDonations);
        }

        /// assign NULL value
        public void SetEndDonationsNull()
        {
            this.SetNull(this.myTable.ColumnEndDonations);
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

    /// Information describing groups (batches) of gifts.
    [Serializable()]
    public class AGiftBatchTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 164;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationDateId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnHashTotalId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchTotalId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountCodeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastGiftNumberId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchStatusId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchPeriodId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchYearId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnGlEffectiveDateId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnCurrencyCodeId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnExchangeRateToBaseId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankCostCentreId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTypeId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 21;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AGiftBatch", "a_gift_batch",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "BatchDescription", "a_batch_description_c", "Batch description", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(3, "ModificationDate", "s_modification_date_d", "Modification Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "HashTotal", "a_hash_total_n", "Hash Total", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(5, "BatchTotal", "a_batch_total_n", "Batch Total", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "BankAccountCode", "a_bank_account_code_c", "Bank Account", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(7, "LastGiftNumber", "a_last_gift_number_i", "Last Gift", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "BatchStatus", "a_batch_status_c", "Batch Status", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(9, "BatchPeriod", "a_batch_period_i", "Batch Period Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(10, "BatchYear", "a_batch_year_i", "a_batch_year_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(11, "GlEffectiveDate", "a_gl_effective_date_d", "GL Effective Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "CurrencyCode", "a_currency_code_c", "Gift Transaction Currency", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(13, "ExchangeRateToBase", "a_exchange_rate_to_base_n", "Exchange Rate to Base", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(14, "BankCostCentre", "a_bank_cost_centre_c", "Cost Centre Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(15, "GiftType", "a_gift_type_c", "Gift Type", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(16, "MethodOfPaymentCode", "a_method_of_payment_code_c", "Method of Payment", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(17, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(18, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(19, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(20, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(21, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public AGiftBatchTable() :
                base("AGiftBatch")
        {
        }

        /// constructor
        public AGiftBatchTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AGiftBatchTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// ledger number
        public DataColumn ColumnLedgerNumber;
        /// Gift batch number
        public DataColumn ColumnBatchNumber;
        /// gift batch description
        public DataColumn ColumnBatchDescription;
        /// date of user entry or last modification.
        public DataColumn ColumnModificationDate;
        /// hash total for the gift batch
        public DataColumn ColumnHashTotal;
        /// total for the gift batch
        public DataColumn ColumnBatchTotal;
        /// bank account code which this batch is for
        public DataColumn ColumnBankAccountCode;
        /// last gift number of the batch
        public DataColumn ColumnLastGiftNumber;
        /// Status of a gift batch: unposted, posted, cancelled.
        public DataColumn ColumnBatchStatus;
        /// The accounting period that the batch belongs to.  Must be &lt;= 20.
        public DataColumn ColumnBatchPeriod;
        /// The financial year that the batch belongs to.
        public DataColumn ColumnBatchYear;
        /// Effective date when posted to the general ledger.
        public DataColumn ColumnGlEffectiveDate;
        /// This defines which currency is being used
        public DataColumn ColumnCurrencyCode;
        /// The rate of exchange
        public DataColumn ColumnExchangeRateToBase;
        /// This identifies which cost centre is applied to the bank
        public DataColumn ColumnBankCostCentre;
        /// What type of gift is this? a gift or a gift in kind generally
        public DataColumn ColumnGiftType;
        /// This is how the partner paid. EgCash, Cheque etc
        public DataColumn ColumnMethodOfPaymentCode;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_hash_total_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_total_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_last_gift_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_period_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_year_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gl_effective_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_exchange_rate_to_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_cost_centre_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnBatchDescription = this.Columns["a_batch_description_c"];
            this.ColumnModificationDate = this.Columns["s_modification_date_d"];
            this.ColumnHashTotal = this.Columns["a_hash_total_n"];
            this.ColumnBatchTotal = this.Columns["a_batch_total_n"];
            this.ColumnBankAccountCode = this.Columns["a_bank_account_code_c"];
            this.ColumnLastGiftNumber = this.Columns["a_last_gift_number_i"];
            this.ColumnBatchStatus = this.Columns["a_batch_status_c"];
            this.ColumnBatchPeriod = this.Columns["a_batch_period_i"];
            this.ColumnBatchYear = this.Columns["a_batch_year_i"];
            this.ColumnGlEffectiveDate = this.Columns["a_gl_effective_date_d"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnExchangeRateToBase = this.Columns["a_exchange_rate_to_base_n"];
            this.ColumnBankCostCentre = this.Columns["a_bank_cost_centre_c"];
            this.ColumnGiftType = this.Columns["a_gift_type_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnBatchNumber};
        }

        /// Access a typed row by index
        public AGiftBatchRow this[int i]
        {
            get
            {
                return ((AGiftBatchRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AGiftBatchRow NewRowTyped(bool AWithDefaultValues)
        {
            AGiftBatchRow ret = ((AGiftBatchRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AGiftBatchRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AGiftBatchRow(builder);
        }

        /// get typed set of changes
        public AGiftBatchTable GetChangesTyped()
        {
            return ((AGiftBatchTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AGiftBatch";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_gift_batch";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchDescriptionDBName()
        {
            return "a_batch_description_c";
        }

        /// get character length for column
        public static short GetBatchDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationDateDBName()
        {
            return "s_modification_date_d";
        }

        /// get character length for column
        public static short GetModificationDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHashTotalDBName()
        {
            return "a_hash_total_n";
        }

        /// get character length for column
        public static short GetHashTotalLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchTotalDBName()
        {
            return "a_batch_total_n";
        }

        /// get character length for column
        public static short GetBatchTotalLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountCodeDBName()
        {
            return "a_bank_account_code_c";
        }

        /// get character length for column
        public static short GetBankAccountCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetLastGiftNumberDBName()
        {
            return "a_last_gift_number_i";
        }

        /// get character length for column
        public static short GetLastGiftNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchStatusDBName()
        {
            return "a_batch_status_c";
        }

        /// get character length for column
        public static short GetBatchStatusLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchPeriodDBName()
        {
            return "a_batch_period_i";
        }

        /// get character length for column
        public static short GetBatchPeriodLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchYearDBName()
        {
            return "a_batch_year_i";
        }

        /// get character length for column
        public static short GetBatchYearLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGlEffectiveDateDBName()
        {
            return "a_gl_effective_date_d";
        }

        /// get character length for column
        public static short GetGlEffectiveDateLength()
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
        public static string GetExchangeRateToBaseDBName()
        {
            return "a_exchange_rate_to_base_n";
        }

        /// get character length for column
        public static short GetExchangeRateToBaseLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBankCostCentreDBName()
        {
            return "a_bank_cost_centre_c";
        }

        /// get character length for column
        public static short GetBankCostCentreLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTypeDBName()
        {
            return "a_gift_type_c";
        }

        /// get character length for column
        public static short GetGiftTypeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "a_method_of_payment_code_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
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

    /// Information describing groups (batches) of gifts.
    [Serializable()]
    public class AGiftBatchRow : System.Data.DataRow
    {
        private AGiftBatchTable myTable;

        /// Constructor
        public AGiftBatchRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AGiftBatchTable)(this.Table));
        }

        /// ledger number
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

        /// Gift batch number
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// gift batch description
        public String BatchDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBatchDescription)
                            || (((String)(this[this.myTable.ColumnBatchDescription])) != value)))
                {
                    this[this.myTable.ColumnBatchDescription] = value;
                }
            }
        }

        /// date of user entry or last modification.
        public System.DateTime ModificationDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnModificationDate)
                            || (((System.DateTime)(this[this.myTable.ColumnModificationDate])) != value)))
                {
                    this[this.myTable.ColumnModificationDate] = value;
                }
            }
        }

        /// hash total for the gift batch
        public Double HashTotal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHashTotal.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHashTotal)
                            || (((Double)(this[this.myTable.ColumnHashTotal])) != value)))
                {
                    this[this.myTable.ColumnHashTotal] = value;
                }
            }
        }

        /// total for the gift batch
        public Double BatchTotal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchTotal.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchTotal)
                            || (((Double)(this[this.myTable.ColumnBatchTotal])) != value)))
                {
                    this[this.myTable.ColumnBatchTotal] = value;
                }
            }
        }

        /// bank account code which this batch is for
        public String BankAccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccountCode)
                            || (((String)(this[this.myTable.ColumnBankAccountCode])) != value)))
                {
                    this[this.myTable.ColumnBankAccountCode] = value;
                }
            }
        }

        /// last gift number of the batch
        public Int32 LastGiftNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastGiftNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastGiftNumber)
                            || (((Int32)(this[this.myTable.ColumnLastGiftNumber])) != value)))
                {
                    this[this.myTable.ColumnLastGiftNumber] = value;
                }
            }
        }

        /// Status of a gift batch: unposted, posted, cancelled.
        public String BatchStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBatchStatus)
                            || (((String)(this[this.myTable.ColumnBatchStatus])) != value)))
                {
                    this[this.myTable.ColumnBatchStatus] = value;
                }
            }
        }

        /// The accounting period that the batch belongs to.  Must be &lt;= 20.
        public Int32 BatchPeriod
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchPeriod.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchPeriod)
                            || (((Int32)(this[this.myTable.ColumnBatchPeriod])) != value)))
                {
                    this[this.myTable.ColumnBatchPeriod] = value;
                }
            }
        }

        /// The financial year that the batch belongs to.
        public Int32 BatchYear
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchYear.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchYear)
                            || (((Int32)(this[this.myTable.ColumnBatchYear])) != value)))
                {
                    this[this.myTable.ColumnBatchYear] = value;
                }
            }
        }

        /// Effective date when posted to the general ledger.
        public System.DateTime GlEffectiveDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGlEffectiveDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGlEffectiveDate)
                            || (((System.DateTime)(this[this.myTable.ColumnGlEffectiveDate])) != value)))
                {
                    this[this.myTable.ColumnGlEffectiveDate] = value;
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

        /// The rate of exchange
        public Double ExchangeRateToBase
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExchangeRateToBase.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExchangeRateToBase)
                            || (((Double)(this[this.myTable.ColumnExchangeRateToBase])) != value)))
                {
                    this[this.myTable.ColumnExchangeRateToBase] = value;
                }
            }
        }

        /// This identifies which cost centre is applied to the bank
        public String BankCostCentre
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankCostCentre.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankCostCentre)
                            || (((String)(this[this.myTable.ColumnBankCostCentre])) != value)))
                {
                    this[this.myTable.ColumnBankCostCentre] = value;
                }
            }
        }

        /// What type of gift is this? a gift or a gift in kind generally
        public String GiftType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftType)
                            || (((String)(this[this.myTable.ColumnGiftType])) != value)))
                {
                    this[this.myTable.ColumnGiftType] = value;
                }
            }
        }

        /// This is how the partner paid. EgCash, Cheque etc
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBatchDescription);
            this[this.myTable.ColumnModificationDate.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnHashTotal.Ordinal] = 0;
            this[this.myTable.ColumnBatchTotal.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBankAccountCode);
            this[this.myTable.ColumnLastGiftNumber.Ordinal] = 0;
            this[this.myTable.ColumnBatchStatus.Ordinal] = "Unposted";
            this[this.myTable.ColumnBatchPeriod.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBatchYear);
            this[this.myTable.ColumnGlEffectiveDate.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this[this.myTable.ColumnExchangeRateToBase.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBankCostCentre);
            this[this.myTable.ColumnGiftType.Ordinal] = "Gift";
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsBatchDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnBatchDescription);
        }

        /// assign NULL value
        public void SetBatchDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnBatchDescription);
        }

        /// test for NULL value
        public bool IsModificationDateNull()
        {
            return this.IsNull(this.myTable.ColumnModificationDate);
        }

        /// assign NULL value
        public void SetModificationDateNull()
        {
            this.SetNull(this.myTable.ColumnModificationDate);
        }

        /// test for NULL value
        public bool IsHashTotalNull()
        {
            return this.IsNull(this.myTable.ColumnHashTotal);
        }

        /// assign NULL value
        public void SetHashTotalNull()
        {
            this.SetNull(this.myTable.ColumnHashTotal);
        }

        /// test for NULL value
        public bool IsBatchTotalNull()
        {
            return this.IsNull(this.myTable.ColumnBatchTotal);
        }

        /// assign NULL value
        public void SetBatchTotalNull()
        {
            this.SetNull(this.myTable.ColumnBatchTotal);
        }

        /// test for NULL value
        public bool IsBankAccountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccountCode);
        }

        /// assign NULL value
        public void SetBankAccountCodeNull()
        {
            this.SetNull(this.myTable.ColumnBankAccountCode);
        }

        /// test for NULL value
        public bool IsLastGiftNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLastGiftNumber);
        }

        /// assign NULL value
        public void SetLastGiftNumberNull()
        {
            this.SetNull(this.myTable.ColumnLastGiftNumber);
        }

        /// test for NULL value
        public bool IsBatchStatusNull()
        {
            return this.IsNull(this.myTable.ColumnBatchStatus);
        }

        /// assign NULL value
        public void SetBatchStatusNull()
        {
            this.SetNull(this.myTable.ColumnBatchStatus);
        }

        /// test for NULL value
        public bool IsBatchPeriodNull()
        {
            return this.IsNull(this.myTable.ColumnBatchPeriod);
        }

        /// assign NULL value
        public void SetBatchPeriodNull()
        {
            this.SetNull(this.myTable.ColumnBatchPeriod);
        }

        /// test for NULL value
        public bool IsBatchYearNull()
        {
            return this.IsNull(this.myTable.ColumnBatchYear);
        }

        /// assign NULL value
        public void SetBatchYearNull()
        {
            this.SetNull(this.myTable.ColumnBatchYear);
        }

        /// test for NULL value
        public bool IsGlEffectiveDateNull()
        {
            return this.IsNull(this.myTable.ColumnGlEffectiveDate);
        }

        /// assign NULL value
        public void SetGlEffectiveDateNull()
        {
            this.SetNull(this.myTable.ColumnGlEffectiveDate);
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
        public bool IsExchangeRateToBaseNull()
        {
            return this.IsNull(this.myTable.ColumnExchangeRateToBase);
        }

        /// assign NULL value
        public void SetExchangeRateToBaseNull()
        {
            this.SetNull(this.myTable.ColumnExchangeRateToBase);
        }

        /// test for NULL value
        public bool IsBankCostCentreNull()
        {
            return this.IsNull(this.myTable.ColumnBankCostCentre);
        }

        /// assign NULL value
        public void SetBankCostCentreNull()
        {
            this.SetNull(this.myTable.ColumnBankCostCentre);
        }

        /// test for NULL value
        public bool IsGiftTypeNull()
        {
            return this.IsNull(this.myTable.ColumnGiftType);
        }

        /// assign NULL value
        public void SetGiftTypeNull()
        {
            this.SetNull(this.myTable.ColumnGiftType);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
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

    /// Information on the donor's giving. Points to the gift detail records.
    [Serializable()]
    public class AGiftTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 165;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTransactionNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftStatusId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEnteredId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnHomeAdminChargesFlagId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnIltAdminChargesFlagId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptLetterCodeId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfGivingCodeId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnAdminChargeId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptNumberId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastDetailNumberId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnReferenceId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnFirstTimeGiftId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptPrintedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnRestrictedId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankingDetailsKeyId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 22;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 23;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AGift", "a_gift",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "GiftStatus", "a_gift_status_c", "Gift Status", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(4, "DateEntered", "a_date_entered_d", "Date Entered", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(5, "HomeAdminChargesFlag", "a_home_admin_charges_flag_l", "Local Admin Charges", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(6, "IltAdminChargesFlag", "a_ilt_admin_charges_flag_l", "IT Admin Charges", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(7, "ReceiptLetterCode", "a_receipt_letter_code_c", "Receipt Letter Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(8, "MethodOfGivingCode", "a_method_of_giving_code_c", "Method Of Giving", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "MethodOfPaymentCode", "a_method_of_payment_code_c", "Method of Payment", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(10, "DonorKey", "p_donor_key_n", "Donor", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(11, "AdminCharge", "a_admin_charge_l", "Admin Charge", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "ReceiptNumber", "a_receipt_number_i", "Receipt Number", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "LastDetailNumber", "a_last_detail_number_i", "Last Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(14, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(15, "FirstTimeGift", "a_first_time_gift_l", "Donors first gift flag", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(16, "ReceiptPrinted", "a_receipt_printed_l", "Receipt Printed", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(17, "Restricted", "a_restricted_l", "Gift Restricted", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(18, "BankingDetailsKey", "p_banking_details_key_i", "Bank or Credit Card", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(19, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(20, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(21, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(22, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(23, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public AGiftTable() :
                base("AGift")
        {
        }

        /// constructor
        public AGiftTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AGiftTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files.  The four digit ledger number of the gift.
        public DataColumn ColumnLedgerNumber;
        /// identifes which batch a transaction belongs to
        public DataColumn ColumnBatchNumber;
        /// Identifies a transaction within a journal within a batch within a ledger
        public DataColumn ColumnGiftTransactionNumber;
        ///
        public DataColumn ColumnGiftStatus;
        ///
        public DataColumn ColumnDateEntered;
        /// Used to get a yes no response from the user
        public DataColumn ColumnHomeAdminChargesFlag;
        /// Used to get a yes no response from the user
        public DataColumn ColumnIltAdminChargesFlag;
        ///
        public DataColumn ColumnReceiptLetterCode;
        /// Defines how a gift is given.
        public DataColumn ColumnMethodOfGivingCode;
        /// This is how the partner paid. Eg cash, Cheque etc
        public DataColumn ColumnMethodOfPaymentCode;
        /// This is the partner key of the donor.
        public DataColumn ColumnDonorKey;
        /// NOT USED AT ALL
        public DataColumn ColumnAdminCharge;
        /// Gift Receipt Number
        public DataColumn ColumnReceiptNumber;
        /// Identifies the last gift detail entered
        public DataColumn ColumnLastDetailNumber;
        /// Reference number/code for the transaction
        public DataColumn ColumnReference;
        /// Flag to indicate Donors first gift
        public DataColumn ColumnFirstTimeGift;
        /// Indicates whether or not the receipt has been printed for this gift
        public DataColumn ColumnReceiptPrinted;
        /// Indicates whether or not the gift has restricted access. If it does then the access will be controlled by s_group_gift
        public DataColumn ColumnRestricted;
        /// Bank or credit card account used for making this gift transaction.
        public DataColumn ColumnBankingDetailsKey;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_date_entered_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_home_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ilt_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_letter_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_giving_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_donor_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_admin_charge_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_last_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_first_time_gift_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_printed_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_banking_details_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnGiftStatus = this.Columns["a_gift_status_c"];
            this.ColumnDateEntered = this.Columns["a_date_entered_d"];
            this.ColumnHomeAdminChargesFlag = this.Columns["a_home_admin_charges_flag_l"];
            this.ColumnIltAdminChargesFlag = this.Columns["a_ilt_admin_charges_flag_l"];
            this.ColumnReceiptLetterCode = this.Columns["a_receipt_letter_code_c"];
            this.ColumnMethodOfGivingCode = this.Columns["a_method_of_giving_code_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDonorKey = this.Columns["p_donor_key_n"];
            this.ColumnAdminCharge = this.Columns["a_admin_charge_l"];
            this.ColumnReceiptNumber = this.Columns["a_receipt_number_i"];
            this.ColumnLastDetailNumber = this.Columns["a_last_detail_number_i"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnFirstTimeGift = this.Columns["a_first_time_gift_l"];
            this.ColumnReceiptPrinted = this.Columns["a_receipt_printed_l"];
            this.ColumnRestricted = this.Columns["a_restricted_l"];
            this.ColumnBankingDetailsKey = this.Columns["p_banking_details_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber};
        }

        /// Access a typed row by index
        public AGiftRow this[int i]
        {
            get
            {
                return ((AGiftRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AGiftRow NewRowTyped(bool AWithDefaultValues)
        {
            AGiftRow ret = ((AGiftRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AGiftRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AGiftRow(builder);
        }

        /// get typed set of changes
        public AGiftTable GetChangesTyped()
        {
            return ((AGiftTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AGift";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_gift";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTransactionNumberDBName()
        {
            return "a_gift_transaction_number_i";
        }

        /// get character length for column
        public static short GetGiftTransactionNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftStatusDBName()
        {
            return "a_gift_status_c";
        }

        /// get character length for column
        public static short GetGiftStatusLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDateEnteredDBName()
        {
            return "a_date_entered_d";
        }

        /// get character length for column
        public static short GetDateEnteredLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHomeAdminChargesFlagDBName()
        {
            return "a_home_admin_charges_flag_l";
        }

        /// get character length for column
        public static short GetHomeAdminChargesFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetIltAdminChargesFlagDBName()
        {
            return "a_ilt_admin_charges_flag_l";
        }

        /// get character length for column
        public static short GetIltAdminChargesFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptLetterCodeDBName()
        {
            return "a_receipt_letter_code_c";
        }

        /// get character length for column
        public static short GetReceiptLetterCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfGivingCodeDBName()
        {
            return "a_method_of_giving_code_c";
        }

        /// get character length for column
        public static short GetMethodOfGivingCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "a_method_of_payment_code_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDonorKeyDBName()
        {
            return "p_donor_key_n";
        }

        /// get character length for column
        public static short GetDonorKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetAdminChargeDBName()
        {
            return "a_admin_charge_l";
        }

        /// get character length for column
        public static short GetAdminChargeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptNumberDBName()
        {
            return "a_receipt_number_i";
        }

        /// get character length for column
        public static short GetReceiptNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastDetailNumberDBName()
        {
            return "a_last_detail_number_i";
        }

        /// get character length for column
        public static short GetLastDetailNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }

        /// get character length for column
        public static short GetReferenceLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetFirstTimeGiftDBName()
        {
            return "a_first_time_gift_l";
        }

        /// get character length for column
        public static short GetFirstTimeGiftLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptPrintedDBName()
        {
            return "a_receipt_printed_l";
        }

        /// get character length for column
        public static short GetReceiptPrintedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "a_restricted_l";
        }

        /// get character length for column
        public static short GetRestrictedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBankingDetailsKeyDBName()
        {
            return "p_banking_details_key_i";
        }

        /// get character length for column
        public static short GetBankingDetailsKeyLength()
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

    /// Information on the donor's giving. Points to the gift detail records.
    [Serializable()]
    public class AGiftRow : System.Data.DataRow
    {
        private AGiftTable myTable;

        /// Constructor
        public AGiftRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AGiftTable)(this.Table));
        }

        /// This is used as a key field in most of the accounting system files.  The four digit ledger number of the gift.
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

        /// identifes which batch a transaction belongs to
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// Identifies a transaction within a journal within a batch within a ledger
        public Int32 GiftTransactionNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTransactionNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftTransactionNumber)
                            || (((Int32)(this[this.myTable.ColumnGiftTransactionNumber])) != value)))
                {
                    this[this.myTable.ColumnGiftTransactionNumber] = value;
                }
            }
        }

        ///
        public String GiftStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftStatus)
                            || (((String)(this[this.myTable.ColumnGiftStatus])) != value)))
                {
                    this[this.myTable.ColumnGiftStatus] = value;
                }
            }
        }

        ///
        public System.DateTime DateEntered
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateEntered.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateEntered)
                            || (((System.DateTime)(this[this.myTable.ColumnDateEntered])) != value)))
                {
                    this[this.myTable.ColumnDateEntered] = value;
                }
            }
        }

        /// Used to get a yes no response from the user
        public Boolean HomeAdminChargesFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHomeAdminChargesFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHomeAdminChargesFlag)
                            || (((Boolean)(this[this.myTable.ColumnHomeAdminChargesFlag])) != value)))
                {
                    this[this.myTable.ColumnHomeAdminChargesFlag] = value;
                }
            }
        }

        /// Used to get a yes no response from the user
        public Boolean IltAdminChargesFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnIltAdminChargesFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnIltAdminChargesFlag)
                            || (((Boolean)(this[this.myTable.ColumnIltAdminChargesFlag])) != value)))
                {
                    this[this.myTable.ColumnIltAdminChargesFlag] = value;
                }
            }
        }

        ///
        public String ReceiptLetterCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptLetterCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReceiptLetterCode)
                            || (((String)(this[this.myTable.ColumnReceiptLetterCode])) != value)))
                {
                    this[this.myTable.ColumnReceiptLetterCode] = value;
                }
            }
        }

        /// Defines how a gift is given.
        public String MethodOfGivingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfGivingCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfGivingCode)
                            || (((String)(this[this.myTable.ColumnMethodOfGivingCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfGivingCode] = value;
                }
            }
        }

        /// This is how the partner paid. Eg cash, Cheque etc
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
                }
            }
        }

        /// This is the partner key of the donor.
        public Int64 DonorKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorKey)
                            || (((Int64)(this[this.myTable.ColumnDonorKey])) != value)))
                {
                    this[this.myTable.ColumnDonorKey] = value;
                }
            }
        }

        /// NOT USED AT ALL
        public Boolean AdminCharge
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAdminCharge.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAdminCharge)
                            || (((Boolean)(this[this.myTable.ColumnAdminCharge])) != value)))
                {
                    this[this.myTable.ColumnAdminCharge] = value;
                }
            }
        }

        /// Gift Receipt Number
        public Int32 ReceiptNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReceiptNumber)
                            || (((Int32)(this[this.myTable.ColumnReceiptNumber])) != value)))
                {
                    this[this.myTable.ColumnReceiptNumber] = value;
                }
            }
        }

        /// Identifies the last gift detail entered
        public Int32 LastDetailNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastDetailNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastDetailNumber)
                            || (((Int32)(this[this.myTable.ColumnLastDetailNumber])) != value)))
                {
                    this[this.myTable.ColumnLastDetailNumber] = value;
                }
            }
        }

        /// Reference number/code for the transaction
        public String Reference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReference)
                            || (((String)(this[this.myTable.ColumnReference])) != value)))
                {
                    this[this.myTable.ColumnReference] = value;
                }
            }
        }

        /// Flag to indicate Donors first gift
        public Boolean FirstTimeGift
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFirstTimeGift.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFirstTimeGift)
                            || (((Boolean)(this[this.myTable.ColumnFirstTimeGift])) != value)))
                {
                    this[this.myTable.ColumnFirstTimeGift] = value;
                }
            }
        }

        /// Indicates whether or not the receipt has been printed for this gift
        public Boolean ReceiptPrinted
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptPrinted.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReceiptPrinted)
                            || (((Boolean)(this[this.myTable.ColumnReceiptPrinted])) != value)))
                {
                    this[this.myTable.ColumnReceiptPrinted] = value;
                }
            }
        }

        /// Indicates whether or not the gift has restricted access. If it does then the access will be controlled by s_group_gift
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

        /// Bank or credit card account used for making this gift transaction.
        public Int32 BankingDetailsKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankingDetailsKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBankingDetailsKey)
                            || (((Int32)(this[this.myTable.ColumnBankingDetailsKey])) != value)))
                {
                    this[this.myTable.ColumnBankingDetailsKey] = value;
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnGiftStatus);
            this[this.myTable.ColumnDateEntered.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnHomeAdminChargesFlag.Ordinal] = true;
            this[this.myTable.ColumnIltAdminChargesFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDonorKey.Ordinal] = 0;
            this[this.myTable.ColumnAdminCharge.Ordinal] = false;
            this[this.myTable.ColumnReceiptNumber.Ordinal] = 0;
            this[this.myTable.ColumnLastDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnReference);
            this[this.myTable.ColumnFirstTimeGift.Ordinal] = false;
            this[this.myTable.ColumnReceiptPrinted.Ordinal] = false;
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnBankingDetailsKey.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsGiftTransactionNumberNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// assign NULL value
        public void SetGiftTransactionNumberNull()
        {
            this.SetNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// test for NULL value
        public bool IsGiftStatusNull()
        {
            return this.IsNull(this.myTable.ColumnGiftStatus);
        }

        /// assign NULL value
        public void SetGiftStatusNull()
        {
            this.SetNull(this.myTable.ColumnGiftStatus);
        }

        /// test for NULL value
        public bool IsDateEnteredNull()
        {
            return this.IsNull(this.myTable.ColumnDateEntered);
        }

        /// assign NULL value
        public void SetDateEnteredNull()
        {
            this.SetNull(this.myTable.ColumnDateEntered);
        }

        /// test for NULL value
        public bool IsHomeAdminChargesFlagNull()
        {
            return this.IsNull(this.myTable.ColumnHomeAdminChargesFlag);
        }

        /// assign NULL value
        public void SetHomeAdminChargesFlagNull()
        {
            this.SetNull(this.myTable.ColumnHomeAdminChargesFlag);
        }

        /// test for NULL value
        public bool IsIltAdminChargesFlagNull()
        {
            return this.IsNull(this.myTable.ColumnIltAdminChargesFlag);
        }

        /// assign NULL value
        public void SetIltAdminChargesFlagNull()
        {
            this.SetNull(this.myTable.ColumnIltAdminChargesFlag);
        }

        /// test for NULL value
        public bool IsReceiptLetterCodeNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptLetterCode);
        }

        /// assign NULL value
        public void SetReceiptLetterCodeNull()
        {
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
        }

        /// test for NULL value
        public bool IsMethodOfGivingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// assign NULL value
        public void SetMethodOfGivingCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// test for NULL value
        public bool IsDonorKeyNull()
        {
            return this.IsNull(this.myTable.ColumnDonorKey);
        }

        /// assign NULL value
        public void SetDonorKeyNull()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
        }

        /// test for NULL value
        public bool IsAdminChargeNull()
        {
            return this.IsNull(this.myTable.ColumnAdminCharge);
        }

        /// assign NULL value
        public void SetAdminChargeNull()
        {
            this.SetNull(this.myTable.ColumnAdminCharge);
        }

        /// test for NULL value
        public bool IsReceiptNumberNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptNumber);
        }

        /// assign NULL value
        public void SetReceiptNumberNull()
        {
            this.SetNull(this.myTable.ColumnReceiptNumber);
        }

        /// test for NULL value
        public bool IsLastDetailNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLastDetailNumber);
        }

        /// assign NULL value
        public void SetLastDetailNumberNull()
        {
            this.SetNull(this.myTable.ColumnLastDetailNumber);
        }

        /// test for NULL value
        public bool IsReferenceNull()
        {
            return this.IsNull(this.myTable.ColumnReference);
        }

        /// assign NULL value
        public void SetReferenceNull()
        {
            this.SetNull(this.myTable.ColumnReference);
        }

        /// test for NULL value
        public bool IsFirstTimeGiftNull()
        {
            return this.IsNull(this.myTable.ColumnFirstTimeGift);
        }

        /// assign NULL value
        public void SetFirstTimeGiftNull()
        {
            this.SetNull(this.myTable.ColumnFirstTimeGift);
        }

        /// test for NULL value
        public bool IsReceiptPrintedNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptPrinted);
        }

        /// assign NULL value
        public void SetReceiptPrintedNull()
        {
            this.SetNull(this.myTable.ColumnReceiptPrinted);
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
        public bool IsBankingDetailsKeyNull()
        {
            return this.IsNull(this.myTable.ColumnBankingDetailsKey);
        }

        /// assign NULL value
        public void SetBankingDetailsKeyNull()
        {
            this.SetNull(this.myTable.ColumnBankingDetailsKey);
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

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class AGiftDetailTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 166;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTransactionNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDetailNumberId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientLedgerNumberId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftAmountId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentOneTypeId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentOneId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnConfidentialGiftFlagId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnTaxDeductableId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientKeyId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnChargeFlagId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnCostCentreCodeId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftAmountIntlId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedDetailId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTransactionAmountId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnIchNumberId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingCodeId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentTwoTypeId = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentTwoId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnCommentThreeTypeId = 22;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftCommentThreeId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 26;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 27;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 28;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AGiftDetail", "a_gift_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Gift Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Gift Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "DetailNumber", "a_detail_number_i", "Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "RecipientLedgerNumber", "a_recipient_ledger_number_n", "Recipient Ledger", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(5, "GiftAmount", "a_gift_amount_n", "Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(7, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "CommentOneType", "a_comment_one_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "GiftCommentOne", "a_gift_comment_one_c", "Comment One", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(10, "ConfidentialGiftFlag", "a_confidential_gift_flag_l", "Confidential Gift", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(11, "TaxDeductable", "a_tax_deductable_l", "Tax Deductable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "RecipientKey", "p_recipient_key_n", "Recipient", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(13, "ChargeFlag", "a_charge_flag_l", "Charge Fee", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre Code", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(15, "GiftAmountIntl", "a_gift_amount_intl_n", "International Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(16, "ModifiedDetail", "a_modified_detail_l", "Part of a gift detail modification", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(17, "GiftTransactionAmount", "a_gift_transaction_amount_n", "Transaction Gift Amount", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(18, "IchNumber", "a_ich_number_i", "ICH Process Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(19, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(20, "CommentTwoType", "a_comment_two_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(21, "GiftCommentTwo", "a_gift_comment_two_c", "Comment Two", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(22, "CommentThreeType", "a_comment_three_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(23, "GiftCommentThree", "a_gift_comment_three_c", "Comment Three", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(24, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(25, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(26, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(27, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(28, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public AGiftDetailTable() :
                base("AGiftDetail")
        {
        }

        /// constructor
        public AGiftDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AGiftDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// The four digit ledger number of the gift.
        public DataColumn ColumnLedgerNumber;
        /// Number of the gift batch containing this detail.
        public DataColumn ColumnBatchNumber;
        /// Identifies a gift transaction within a gift batch.
        public DataColumn ColumnGiftTransactionNumber;
        /// Identifies a gift detail within a gift transaction.   When a donor gives a donation to multiple recipients (a split gift), a gift detail record is created for each recipient.
        public DataColumn ColumnDetailNumber;
        /// The partner key of the commitment field (the unit) of the recipient of the gift.  This is not the ledger number but rather the partner key of the unit associated with the ledger.
        public DataColumn ColumnRecipientLedgerNumber;
        /// This is a number of currency units of the ledger base currency.
        public DataColumn ColumnGiftAmount;
        /// This defines a motivation group.
        public DataColumn ColumnMotivationGroupCode;
        /// This defines the motivation detail within a motivation group.
        public DataColumn ColumnMotivationDetailCode;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentOneType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentOne;
        /// Defines whether the donor wishes the recipient to know who gave the gift
        public DataColumn ColumnConfidentialGiftFlag;
        /// Whether this gift is tax deductable
        public DataColumn ColumnTaxDeductable;
        /// The partner key of the recipient of the gift.
        public DataColumn ColumnRecipientKey;
        /// To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction.
        public DataColumn ColumnChargeFlag;
        /// This identifies which cost centre an account is applied to. A cost centre can be a partner.
        public DataColumn ColumnCostCentreCode;
        /// This is a number of currency units in the International Currency
        public DataColumn ColumnGiftAmountIntl;
        /// Indicates whether this gift detail has a matching inverse detail record because a modification was made
        public DataColumn ColumnModifiedDetail;
        /// This is a number of currency units in the entered Currency
        public DataColumn ColumnGiftTransactionAmount;
        /// identifes the ICH process number
        public DataColumn ColumnIchNumber;
        /// Mailing Code of the mailing that the gift was a response to.
        public DataColumn ColumnMailingCode;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentTwoType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentTwo;
        /// Used to decide whose reports will see this comment
        public DataColumn ColumnCommentThreeType;
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnGiftCommentThree;
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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_modified_detail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_ich_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_two_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_two_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_three_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_three_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnRecipientLedgerNumber = this.Columns["a_recipient_ledger_number_n"];
            this.ColumnGiftAmount = this.Columns["a_gift_amount_n"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnCommentOneType = this.Columns["a_comment_one_type_c"];
            this.ColumnGiftCommentOne = this.Columns["a_gift_comment_one_c"];
            this.ColumnConfidentialGiftFlag = this.Columns["a_confidential_gift_flag_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnChargeFlag = this.Columns["a_charge_flag_l"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnGiftAmountIntl = this.Columns["a_gift_amount_intl_n"];
            this.ColumnModifiedDetail = this.Columns["a_modified_detail_l"];
            this.ColumnGiftTransactionAmount = this.Columns["a_gift_transaction_amount_n"];
            this.ColumnIchNumber = this.Columns["a_ich_number_i"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnCommentTwoType = this.Columns["a_comment_two_type_c"];
            this.ColumnGiftCommentTwo = this.Columns["a_gift_comment_two_c"];
            this.ColumnCommentThreeType = this.Columns["a_comment_three_type_c"];
            this.ColumnGiftCommentThree = this.Columns["a_gift_comment_three_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber,ColumnDetailNumber};
        }

        /// Access a typed row by index
        public AGiftDetailRow this[int i]
        {
            get
            {
                return ((AGiftDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AGiftDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            AGiftDetailRow ret = ((AGiftDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AGiftDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AGiftDetailRow(builder);
        }

        /// get typed set of changes
        public AGiftDetailTable GetChangesTyped()
        {
            return ((AGiftDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AGiftDetail";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_gift_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }

        /// get character length for column
        public static short GetLedgerNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchNumberDBName()
        {
            return "a_batch_number_i";
        }

        /// get character length for column
        public static short GetBatchNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTransactionNumberDBName()
        {
            return "a_gift_transaction_number_i";
        }

        /// get character length for column
        public static short GetGiftTransactionNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }

        /// get character length for column
        public static short GetDetailNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientLedgerNumberDBName()
        {
            return "a_recipient_ledger_number_n";
        }

        /// get character length for column
        public static short GetRecipientLedgerNumberLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftAmountDBName()
        {
            return "a_gift_amount_n";
        }

        /// get character length for column
        public static short GetGiftAmountLength()
        {
            return 24;
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
        public static string GetCommentOneTypeDBName()
        {
            return "a_comment_one_type_c";
        }

        /// get character length for column
        public static short GetCommentOneTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentOneDBName()
        {
            return "a_gift_comment_one_c";
        }

        /// get character length for column
        public static short GetGiftCommentOneLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetConfidentialGiftFlagDBName()
        {
            return "a_confidential_gift_flag_l";
        }

        /// get character length for column
        public static short GetConfidentialGiftFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxDeductableDBName()
        {
            return "a_tax_deductable_l";
        }

        /// get character length for column
        public static short GetTaxDeductableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientKeyDBName()
        {
            return "p_recipient_key_n";
        }

        /// get character length for column
        public static short GetRecipientKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetChargeFlagDBName()
        {
            return "a_charge_flag_l";
        }

        /// get character length for column
        public static short GetChargeFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCostCentreCodeDBName()
        {
            return "a_cost_centre_code_c";
        }

        /// get character length for column
        public static short GetCostCentreCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftAmountIntlDBName()
        {
            return "a_gift_amount_intl_n";
        }

        /// get character length for column
        public static short GetGiftAmountIntlLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedDetailDBName()
        {
            return "a_modified_detail_l";
        }

        /// get character length for column
        public static short GetModifiedDetailLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTransactionAmountDBName()
        {
            return "a_gift_transaction_amount_n";
        }

        /// get character length for column
        public static short GetGiftTransactionAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetIchNumberDBName()
        {
            return "a_ich_number_i";
        }

        /// get character length for column
        public static short GetIchNumberLength()
        {
            return -1;
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
        public static string GetCommentTwoTypeDBName()
        {
            return "a_comment_two_type_c";
        }

        /// get character length for column
        public static short GetCommentTwoTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentTwoDBName()
        {
            return "a_gift_comment_two_c";
        }

        /// get character length for column
        public static short GetGiftCommentTwoLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetCommentThreeTypeDBName()
        {
            return "a_comment_three_type_c";
        }

        /// get character length for column
        public static short GetCommentThreeTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftCommentThreeDBName()
        {
            return "a_gift_comment_three_c";
        }

        /// get character length for column
        public static short GetGiftCommentThreeLength()
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

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class AGiftDetailRow : System.Data.DataRow
    {
        private AGiftDetailTable myTable;

        /// Constructor
        public AGiftDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AGiftDetailTable)(this.Table));
        }

        /// The four digit ledger number of the gift.
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

        /// Number of the gift batch containing this detail.
        public Int32 BatchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchNumber)
                            || (((Int32)(this[this.myTable.ColumnBatchNumber])) != value)))
                {
                    this[this.myTable.ColumnBatchNumber] = value;
                }
            }
        }

        /// Identifies a gift transaction within a gift batch.
        public Int32 GiftTransactionNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTransactionNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftTransactionNumber)
                            || (((Int32)(this[this.myTable.ColumnGiftTransactionNumber])) != value)))
                {
                    this[this.myTable.ColumnGiftTransactionNumber] = value;
                }
            }
        }

        /// Identifies a gift detail within a gift transaction.   When a donor gives a donation to multiple recipients (a split gift), a gift detail record is created for each recipient.
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

        /// The partner key of the commitment field (the unit) of the recipient of the gift.  This is not the ledger number but rather the partner key of the unit associated with the ledger.
        public Int64 RecipientLedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientLedgerNumber)
                            || (((Int64)(this[this.myTable.ColumnRecipientLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnRecipientLedgerNumber] = value;
                }
            }
        }

        /// This is a number of currency units of the ledger base currency.
        public Double GiftAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftAmount)
                            || (((Double)(this[this.myTable.ColumnGiftAmount])) != value)))
                {
                    this[this.myTable.ColumnGiftAmount] = value;
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

        /// Used to decide whose reports will see this comment
        public String CommentOneType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentOneType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentOneType)
                            || (((String)(this[this.myTable.ColumnCommentOneType])) != value)))
                {
                    this[this.myTable.ColumnCommentOneType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentOne
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentOne.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentOne)
                            || (((String)(this[this.myTable.ColumnGiftCommentOne])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentOne] = value;
                }
            }
        }

        /// Defines whether the donor wishes the recipient to know who gave the gift
        public Boolean ConfidentialGiftFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConfidentialGiftFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConfidentialGiftFlag)
                            || (((Boolean)(this[this.myTable.ColumnConfidentialGiftFlag])) != value)))
                {
                    this[this.myTable.ColumnConfidentialGiftFlag] = value;
                }
            }
        }

        /// Whether this gift is tax deductable
        public Boolean TaxDeductable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxDeductable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTaxDeductable)
                            || (((Boolean)(this[this.myTable.ColumnTaxDeductable])) != value)))
                {
                    this[this.myTable.ColumnTaxDeductable] = value;
                }
            }
        }

        /// The partner key of the recipient of the gift.
        public Int64 RecipientKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientKey)
                            || (((Int64)(this[this.myTable.ColumnRecipientKey])) != value)))
                {
                    this[this.myTable.ColumnRecipientKey] = value;
                }
            }
        }

        /// To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction.
        public Boolean ChargeFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChargeFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChargeFlag)
                            || (((Boolean)(this[this.myTable.ColumnChargeFlag])) != value)))
                {
                    this[this.myTable.ColumnChargeFlag] = value;
                }
            }
        }

        /// This identifies which cost centre an account is applied to. A cost centre can be a partner.
        public String CostCentreCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostCentreCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCostCentreCode)
                            || (((String)(this[this.myTable.ColumnCostCentreCode])) != value)))
                {
                    this[this.myTable.ColumnCostCentreCode] = value;
                }
            }
        }

        /// This is a number of currency units in the International Currency
        public Double GiftAmountIntl
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftAmountIntl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftAmountIntl)
                            || (((Double)(this[this.myTable.ColumnGiftAmountIntl])) != value)))
                {
                    this[this.myTable.ColumnGiftAmountIntl] = value;
                }
            }
        }

        /// Indicates whether this gift detail has a matching inverse detail record because a modification was made
        public Boolean ModifiedDetail
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedDetail.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnModifiedDetail)
                            || (((Boolean)(this[this.myTable.ColumnModifiedDetail])) != value)))
                {
                    this[this.myTable.ColumnModifiedDetail] = value;
                }
            }
        }

        /// This is a number of currency units in the entered Currency
        public Double GiftTransactionAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTransactionAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftTransactionAmount)
                            || (((Double)(this[this.myTable.ColumnGiftTransactionAmount])) != value)))
                {
                    this[this.myTable.ColumnGiftTransactionAmount] = value;
                }
            }
        }

        /// identifes the ICH process number
        public Int32 IchNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnIchNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnIchNumber)
                            || (((Int32)(this[this.myTable.ColumnIchNumber])) != value)))
                {
                    this[this.myTable.ColumnIchNumber] = value;
                }
            }
        }

        /// Mailing Code of the mailing that the gift was a response to.
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

        /// Used to decide whose reports will see this comment
        public String CommentTwoType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentTwoType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentTwoType)
                            || (((String)(this[this.myTable.ColumnCommentTwoType])) != value)))
                {
                    this[this.myTable.ColumnCommentTwoType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentTwo
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentTwo.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentTwo)
                            || (((String)(this[this.myTable.ColumnGiftCommentTwo])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentTwo] = value;
                }
            }
        }

        /// Used to decide whose reports will see this comment
        public String CommentThreeType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCommentThreeType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCommentThreeType)
                            || (((String)(this[this.myTable.ColumnCommentThreeType])) != value)))
                {
                    this[this.myTable.ColumnCommentThreeType] = value;
                }
            }
        }

        /// This is a long description and is 80 characters long.
        public String GiftCommentThree
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftCommentThree.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftCommentThree)
                            || (((String)(this[this.myTable.ColumnGiftCommentThree])) != value)))
                {
                    this[this.myTable.ColumnGiftCommentThree] = value;
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
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this[this.myTable.ColumnRecipientLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnCommentOneType);
            this.SetNull(this.myTable.ColumnGiftCommentOne);
            this[this.myTable.ColumnConfidentialGiftFlag.Ordinal] = false;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnChargeFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this[this.myTable.ColumnGiftAmountIntl.Ordinal] = 0;
            this[this.myTable.ColumnModifiedDetail.Ordinal] = false;
            this[this.myTable.ColumnGiftTransactionAmount.Ordinal] = 0;
            this[this.myTable.ColumnIchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnCommentTwoType);
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
            this.SetNull(this.myTable.ColumnCommentThreeType);
            this.SetNull(this.myTable.ColumnGiftCommentThree);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumber);
        }

        /// assign NULL value
        public void SetLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumber);
        }

        /// test for NULL value
        public bool IsBatchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBatchNumber);
        }

        /// assign NULL value
        public void SetBatchNumberNull()
        {
            this.SetNull(this.myTable.ColumnBatchNumber);
        }

        /// test for NULL value
        public bool IsGiftTransactionNumberNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTransactionNumber);
        }

        /// assign NULL value
        public void SetGiftTransactionNumberNull()
        {
            this.SetNull(this.myTable.ColumnGiftTransactionNumber);
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
        public bool IsRecipientLedgerNumberNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientLedgerNumber);
        }

        /// assign NULL value
        public void SetRecipientLedgerNumberNull()
        {
            this.SetNull(this.myTable.ColumnRecipientLedgerNumber);
        }

        /// test for NULL value
        public bool IsGiftAmountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftAmount);
        }

        /// assign NULL value
        public void SetGiftAmountNull()
        {
            this.SetNull(this.myTable.ColumnGiftAmount);
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
        public bool IsCommentOneTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentOneType);
        }

        /// assign NULL value
        public void SetCommentOneTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentOneType);
        }

        /// test for NULL value
        public bool IsGiftCommentOneNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentOne);
        }

        /// assign NULL value
        public void SetGiftCommentOneNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentOne);
        }

        /// test for NULL value
        public bool IsConfidentialGiftFlagNull()
        {
            return this.IsNull(this.myTable.ColumnConfidentialGiftFlag);
        }

        /// assign NULL value
        public void SetConfidentialGiftFlagNull()
        {
            this.SetNull(this.myTable.ColumnConfidentialGiftFlag);
        }

        /// test for NULL value
        public bool IsTaxDeductableNull()
        {
            return this.IsNull(this.myTable.ColumnTaxDeductable);
        }

        /// assign NULL value
        public void SetTaxDeductableNull()
        {
            this.SetNull(this.myTable.ColumnTaxDeductable);
        }

        /// test for NULL value
        public bool IsRecipientKeyNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientKey);
        }

        /// assign NULL value
        public void SetRecipientKeyNull()
        {
            this.SetNull(this.myTable.ColumnRecipientKey);
        }

        /// test for NULL value
        public bool IsChargeFlagNull()
        {
            return this.IsNull(this.myTable.ColumnChargeFlag);
        }

        /// assign NULL value
        public void SetChargeFlagNull()
        {
            this.SetNull(this.myTable.ColumnChargeFlag);
        }

        /// test for NULL value
        public bool IsCostCentreCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCostCentreCode);
        }

        /// assign NULL value
        public void SetCostCentreCodeNull()
        {
            this.SetNull(this.myTable.ColumnCostCentreCode);
        }

        /// test for NULL value
        public bool IsGiftAmountIntlNull()
        {
            return this.IsNull(this.myTable.ColumnGiftAmountIntl);
        }

        /// assign NULL value
        public void SetGiftAmountIntlNull()
        {
            this.SetNull(this.myTable.ColumnGiftAmountIntl);
        }

        /// test for NULL value
        public bool IsModifiedDetailNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedDetail);
        }

        /// assign NULL value
        public void SetModifiedDetailNull()
        {
            this.SetNull(this.myTable.ColumnModifiedDetail);
        }

        /// test for NULL value
        public bool IsGiftTransactionAmountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTransactionAmount);
        }

        /// assign NULL value
        public void SetGiftTransactionAmountNull()
        {
            this.SetNull(this.myTable.ColumnGiftTransactionAmount);
        }

        /// test for NULL value
        public bool IsIchNumberNull()
        {
            return this.IsNull(this.myTable.ColumnIchNumber);
        }

        /// assign NULL value
        public void SetIchNumberNull()
        {
            this.SetNull(this.myTable.ColumnIchNumber);
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
        public bool IsCommentTwoTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentTwoType);
        }

        /// assign NULL value
        public void SetCommentTwoTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentTwoType);
        }

        /// test for NULL value
        public bool IsGiftCommentTwoNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentTwo);
        }

        /// assign NULL value
        public void SetGiftCommentTwoNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
        }

        /// test for NULL value
        public bool IsCommentThreeTypeNull()
        {
            return this.IsNull(this.myTable.ColumnCommentThreeType);
        }

        /// assign NULL value
        public void SetCommentThreeTypeNull()
        {
            this.SetNull(this.myTable.ColumnCommentThreeType);
        }

        /// test for NULL value
        public bool IsGiftCommentThreeNull()
        {
            return this.IsNull(this.myTable.ColumnGiftCommentThree);
        }

        /// assign NULL value
        public void SetGiftCommentThreeNull()
        {
            this.SetNull(this.myTable.ColumnGiftCommentThree);
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