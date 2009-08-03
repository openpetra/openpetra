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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 106;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ATaxType", "a_tax_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "TaxTypeCode", "a_tax_type_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "TaxTypeDescription", "a_tax_type_description_c", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public ATaxTypeRow this[int i]
        {
            get
            {
                return ((ATaxTypeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public ATaxTypeTable GetChangesTyped()
        {
            return ((ATaxTypeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }

        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxTypeDescriptionDBName()
        {
            return "a_tax_type_description_c";
        }

        /// get character length for column
        public static short GetTaxTypeDescriptionLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
            this.SetNull(this.myTable.ColumnTaxTypeCode);
            this.SetNull(this.myTable.ColumnTaxTypeDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }

        /// test for NULL value
        public bool IsTaxTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxTypeCode);
        }

        /// assign NULL value
        public void SetTaxTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxTypeCode);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 108;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ATaxTable", "a_tax_table",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "TaxTypeCode", "a_tax_type_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "TaxRateCode", "a_tax_rate_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "TaxValidFrom", "a_tax_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(4, "TaxRateDescription", "a_tax_rate_description_c", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(5, "TaxRate", "a_tax_rate_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "Active", "a_active_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(7, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public ATaxTableRow this[int i]
        {
            get
            {
                return ((ATaxTableRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public ATaxTableTable GetChangesTyped()
        {
            return ((ATaxTableTable)(base.GetChangesTypedInternal()));
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
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }

        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxRateCodeDBName()
        {
            return "a_tax_rate_code_c";
        }

        /// get character length for column
        public static short GetTaxRateCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxValidFromDBName()
        {
            return "a_tax_valid_from_d";
        }

        /// get character length for column
        public static short GetTaxValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxRateDescriptionDBName()
        {
            return "a_tax_rate_description_c";
        }

        /// get character length for column
        public static short GetTaxRateDescriptionLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxRateDBName()
        {
            return "a_tax_rate_n";
        }

        /// get character length for column
        public static short GetTaxRateLength()
        {
            return 24;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// This is a short description which is 32 charcters long
        public String TaxRateDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxRateDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsTaxTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxTypeCode);
        }

        /// assign NULL value
        public void SetTaxTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxTypeCode);
        }

        /// test for NULL value
        public bool IsTaxRateCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxRateCode);
        }

        /// assign NULL value
        public void SetTaxRateCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxRateCode);
        }

        /// test for NULL value
        public bool IsTaxValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnTaxValidFrom);
        }

        /// assign NULL value
        public void SetTaxValidFromNull()
        {
            this.SetNull(this.myTable.ColumnTaxValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 184;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArCategory", "a_ar_category",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArCategoryCode", "a_ar_category_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArDescription", "a_ar_description_c", OdbcType.VarChar, 300, false),
                    new TTypedColumnInfo(2, "ArLocalDescription", "a_ar_local_description_c", OdbcType.VarChar, 300, false),
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
        }

        /// Access a typed row by index
        public AArCategoryRow this[int i]
        {
            get
            {
                return ((AArCategoryRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArCategoryTable GetChangesTyped()
        {
            return ((AArCategoryTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }

        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDescriptionDBName()
        {
            return "a_ar_description_c";
        }

        /// get character length for column
        public static short GetArDescriptionLength()
        {
            return 300;
        }

        /// get the name of the field in the database for this column
        public static string GetArLocalDescriptionDBName()
        {
            return "a_ar_local_description_c";
        }

        /// get character length for column
        public static short GetArLocalDescriptionLength()
        {
            return 300;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArCategoryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArCategoryCode);
        }

        /// assign NULL value
        public void SetArCategoryCodeNull()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 185;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArArticle", "a_ar_article",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArArticleCode", "a_ar_article_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArCategoryCode", "a_ar_category_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(2, "TaxTypeCode", "a_tax_type_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(3, "ArSpecificArticle", "a_ar_specific_article_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "ArDescription", "a_ar_description_c", OdbcType.VarChar, 300, false),
                    new TTypedColumnInfo(5, "ArLocalDescription", "a_ar_local_description_c", OdbcType.VarChar, 300, false),
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
        }

        /// Access a typed row by index
        public AArArticleRow this[int i]
        {
            get
            {
                return ((AArArticleRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArArticleTable GetChangesTyped()
        {
            return ((AArArticleTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }

        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }

        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }

        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArSpecificArticleDBName()
        {
            return "a_ar_specific_article_l";
        }

        /// get character length for column
        public static short GetArSpecificArticleLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArDescriptionDBName()
        {
            return "a_ar_description_c";
        }

        /// get character length for column
        public static short GetArDescriptionLength()
        {
            return 300;
        }

        /// get the name of the field in the database for this column
        public static string GetArLocalDescriptionDBName()
        {
            return "a_ar_local_description_c";
        }

        /// get character length for column
        public static short GetArLocalDescriptionLength()
        {
            return 300;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArCategoryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArCategoryCode);
        }

        /// assign NULL value
        public void SetArCategoryCodeNull()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
        }

        /// test for NULL value
        public bool IsTaxTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxTypeCode);
        }

        /// assign NULL value
        public void SetTaxTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxTypeCode);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 186;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArArticlePrice", "a_ar_article_price",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArArticleCode", "a_ar_article_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArDateValidFrom", "a_ar_date_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "ArAmount", "a_ar_amount_n", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(3, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
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
        }

        /// Access a typed row by index
        public AArArticlePriceRow this[int i]
        {
            get
            {
                return ((AArArticlePriceRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArArticlePriceTable GetChangesTyped()
        {
            return ((AArArticlePriceTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }

        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDateValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArAmountDBName()
        {
            return "a_ar_amount_n";
        }

        /// get character length for column
        public static short GetArAmountLength()
        {
            return 24;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDateValidFrom);
        }

        /// assign NULL value
        public void SetArDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDateValidFrom);
        }

        /// test for NULL value
        public bool IsArAmountNull()
        {
            return this.IsNull(this.myTable.ColumnArAmount);
        }

        /// assign NULL value
        public void SetArAmountNull()
        {
            this.SetNull(this.myTable.ColumnArAmount);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 187;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArDiscount", "a_ar_discount",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArDiscountCode", "a_ar_discount_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArDateValidFrom", "a_ar_date_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(2, "ArAdhoc", "a_ar_adhoc_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "Active", "a_active_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "ArDiscountPercentage", "a_ar_discount_percentage_n", OdbcType.Decimal, 5, false),
                    new TTypedColumnInfo(5, "ArDiscountAbsolute", "a_ar_discount_absolute_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "ArAbsoluteAmount", "a_ar_absolute_amount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(7, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "ArNumberOfItems", "a_ar_number_of_items_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "ArMinimumNumberOfItems", "a_ar_minimum_number_of_items_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(10, "ArNumberOfNights", "a_ar_number_of_nights_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "ArMinimumNumberOfNights", "a_ar_minimum_number_of_nights_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(12, "ArWholeRoom", "a_ar_whole_room_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(13, "ArChildren", "a_ar_children_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "ArEarlyBookingDays", "a_ar_early_booking_days_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(15, "ArEarlyPaymentDays", "a_ar_early_payment_days_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(16, "ArArticleCode", "a_ar_article_code_c", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(17, "PartnerTypeCode", "p_partner_type_code_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(19, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(20, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(21, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(22, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AArDiscountRow this[int i]
        {
            get
            {
                return ((AArDiscountRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArDiscountTable GetChangesTyped()
        {
            return ((AArDiscountTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }

        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDateValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArAdhocDBName()
        {
            return "a_ar_adhoc_l";
        }

        /// get character length for column
        public static short GetArAdhocLength()
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
        public static string GetArDiscountPercentageDBName()
        {
            return "a_ar_discount_percentage_n";
        }

        /// get character length for column
        public static short GetArDiscountPercentageLength()
        {
            return 5;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountAbsoluteDBName()
        {
            return "a_ar_discount_absolute_n";
        }

        /// get character length for column
        public static short GetArDiscountAbsoluteLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetArAbsoluteAmountDBName()
        {
            return "a_ar_absolute_amount_n";
        }

        /// get character length for column
        public static short GetArAbsoluteAmountLength()
        {
            return 24;
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
        public static string GetArNumberOfItemsDBName()
        {
            return "a_ar_number_of_items_i";
        }

        /// get character length for column
        public static short GetArNumberOfItemsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArMinimumNumberOfItemsDBName()
        {
            return "a_ar_minimum_number_of_items_i";
        }

        /// get character length for column
        public static short GetArMinimumNumberOfItemsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArNumberOfNightsDBName()
        {
            return "a_ar_number_of_nights_i";
        }

        /// get character length for column
        public static short GetArNumberOfNightsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArMinimumNumberOfNightsDBName()
        {
            return "a_ar_minimum_number_of_nights_i";
        }

        /// get character length for column
        public static short GetArMinimumNumberOfNightsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArWholeRoomDBName()
        {
            return "a_ar_whole_room_l";
        }

        /// get character length for column
        public static short GetArWholeRoomLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArChildrenDBName()
        {
            return "a_ar_children_l";
        }

        /// get character length for column
        public static short GetArChildrenLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArEarlyBookingDaysDBName()
        {
            return "a_ar_early_booking_days_i";
        }

        /// get character length for column
        public static short GetArEarlyBookingDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArEarlyPaymentDaysDBName()
        {
            return "a_ar_early_payment_days_i";
        }

        /// get character length for column
        public static short GetArEarlyPaymentDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }

        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerTypeCodeDBName()
        {
            return "p_partner_type_code_c";
        }

        /// get character length for column
        public static short GetPartnerTypeCodeLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArDiscountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountCode);
        }

        /// assign NULL value
        public void SetArDiscountCodeNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
        }

        /// test for NULL value
        public bool IsArDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDateValidFrom);
        }

        /// assign NULL value
        public void SetArDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDateValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 188;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArDiscountPerCategory", "a_ar_discount_per_category",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArCategoryCode", "a_ar_category_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArDiscountCode", "a_ar_discount_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(2, "ArDateValidFrom", "a_ar_date_valid_from_d", OdbcType.Date, -1, true),
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
        }

        /// Access a typed row by index
        public AArDiscountPerCategoryRow this[int i]
        {
            get
            {
                return ((AArDiscountPerCategoryRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArDiscountPerCategoryTable GetChangesTyped()
        {
            return ((AArDiscountPerCategoryTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }

        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }

        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDateValidFromLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArCategoryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArCategoryCode);
        }

        /// assign NULL value
        public void SetArCategoryCodeNull()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
        }

        /// test for NULL value
        public bool IsArDiscountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountCode);
        }

        /// assign NULL value
        public void SetArDiscountCodeNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
        }

        /// test for NULL value
        public bool IsArDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDateValidFrom);
        }

        /// assign NULL value
        public void SetArDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDateValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 189;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArDefaultDiscount", "a_ar_default_discount",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ArCategoryCode", "a_ar_category_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(1, "ArDiscountCode", "a_ar_discount_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(2, "ArDiscountDateValidFrom", "a_ar_discount_date_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(3, "ArDateValidFrom", "a_ar_date_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(4, "ArDateValidTo", "a_ar_date_valid_to_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 3
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AArDefaultDiscountRow this[int i]
        {
            get
            {
                return ((AArDefaultDiscountRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArDefaultDiscountTable GetChangesTyped()
        {
            return ((AArDefaultDiscountTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }

        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }

        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDiscountDateValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDateValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArDateValidToDBName()
        {
            return "a_ar_date_valid_to_d";
        }

        /// get character length for column
        public static short GetArDateValidToLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArCategoryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArCategoryCode);
        }

        /// assign NULL value
        public void SetArCategoryCodeNull()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
        }

        /// test for NULL value
        public bool IsArDiscountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountCode);
        }

        /// assign NULL value
        public void SetArDiscountCodeNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
        }

        /// test for NULL value
        public bool IsArDiscountDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountDateValidFrom);
        }

        /// assign NULL value
        public void SetArDiscountDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
        }

        /// test for NULL value
        public bool IsArDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDateValidFrom);
        }

        /// assign NULL value
        public void SetArDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDateValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 190;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArInvoice", "a_ar_invoice",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "Key", "a_key_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "Status", "a_status_c", OdbcType.VarChar, 32, true),
                    new TTypedColumnInfo(3, "PartnerKey", "p_partner_key_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(4, "DateEffective", "a_date_effective_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "Offer", "a_offer_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "Taxing", "a_taxing_c", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(7, "SpecialTaxTypeCode", "a_special_tax_type_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(8, "SpecialTaxRateCode", "a_special_tax_rate_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(9, "SpecialTaxValidFrom", "a_special_tax_valid_from_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "TotalAmount", "a_total_amount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(11, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(12, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AArInvoiceRow this[int i]
        {
            get
            {
                return ((AArInvoiceRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArInvoiceTable GetChangesTyped()
        {
            return ((AArInvoiceTable)(base.GetChangesTypedInternal()));
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
        public static string GetKeyDBName()
        {
            return "a_key_i";
        }

        /// get character length for column
        public static short GetKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetStatusDBName()
        {
            return "a_status_c";
        }

        /// get character length for column
        public static short GetStatusLength()
        {
            return 32;
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
        public static string GetDateEffectiveDBName()
        {
            return "a_date_effective_d";
        }

        /// get character length for column
        public static short GetDateEffectiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetOfferDBName()
        {
            return "a_offer_i";
        }

        /// get character length for column
        public static short GetOfferLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxingDBName()
        {
            return "a_taxing_c";
        }

        /// get character length for column
        public static short GetTaxingLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetSpecialTaxTypeCodeDBName()
        {
            return "a_special_tax_type_code_c";
        }

        /// get character length for column
        public static short GetSpecialTaxTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetSpecialTaxRateCodeDBName()
        {
            return "a_special_tax_rate_code_c";
        }

        /// get character length for column
        public static short GetSpecialTaxRateCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetSpecialTaxValidFromDBName()
        {
            return "a_special_tax_valid_from_d";
        }

        /// get character length for column
        public static short GetSpecialTaxValidFromLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTotalAmountDBName()
        {
            return "a_total_amount_n";
        }

        /// get character length for column
        public static short GetTotalAmountLength()
        {
            return 24;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsStatusNull()
        {
            return this.IsNull(this.myTable.ColumnStatus);
        }

        /// assign NULL value
        public void SetStatusNull()
        {
            this.SetNull(this.myTable.ColumnStatus);
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
        public bool IsTaxingNull()
        {
            return this.IsNull(this.myTable.ColumnTaxing);
        }

        /// assign NULL value
        public void SetTaxingNull()
        {
            this.SetNull(this.myTable.ColumnTaxing);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 191;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArInvoiceDetail", "a_ar_invoice_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "InvoiceKey", "a_invoice_key_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "DetailNumber", "a_detail_number_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "ArArticleCode", "a_ar_article_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(4, "ArReference", "a_ar_reference_c", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(5, "ArNumberOfItem", "a_ar_number_of_item_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(6, "ArArticlePrice", "a_ar_article_price_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(7, "CalculatedAmount", "a_calculated_amount_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(8, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(9, "TaxTypeCode", "a_tax_type_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(10, "TaxRateCode", "a_tax_rate_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(11, "TaxValidFrom", "a_tax_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(12, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AArInvoiceDetailRow this[int i]
        {
            get
            {
                return ((AArInvoiceDetailRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArInvoiceDetailTable GetChangesTyped()
        {
            return ((AArInvoiceDetailTable)(base.GetChangesTypedInternal()));
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
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }

        /// get character length for column
        public static short GetInvoiceKeyLength()
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
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }

        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArReferenceDBName()
        {
            return "a_ar_reference_c";
        }

        /// get character length for column
        public static short GetArReferenceLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArNumberOfItemDBName()
        {
            return "a_ar_number_of_item_i";
        }

        /// get character length for column
        public static short GetArNumberOfItemLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArArticlePriceDBName()
        {
            return "a_ar_article_price_d";
        }

        /// get character length for column
        public static short GetArArticlePriceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCalculatedAmountDBName()
        {
            return "a_calculated_amount_n";
        }

        /// get character length for column
        public static short GetCalculatedAmountLength()
        {
            return 24;
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
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }

        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxRateCodeDBName()
        {
            return "a_tax_rate_code_c";
        }

        /// get character length for column
        public static short GetTaxRateCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetTaxValidFromDBName()
        {
            return "a_tax_valid_from_d";
        }

        /// get character length for column
        public static short GetTaxValidFromLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The tax type is always the same, e.g. VAT
        public String TaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArNumberOfItemNull()
        {
            return this.IsNull(this.myTable.ColumnArNumberOfItem);
        }

        /// assign NULL value
        public void SetArNumberOfItemNull()
        {
            this.SetNull(this.myTable.ColumnArNumberOfItem);
        }

        /// test for NULL value
        public bool IsArArticlePriceNull()
        {
            return this.IsNull(this.myTable.ColumnArArticlePrice);
        }

        /// assign NULL value
        public void SetArArticlePriceNull()
        {
            this.SetNull(this.myTable.ColumnArArticlePrice);
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
        public bool IsTaxTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxTypeCode);
        }

        /// assign NULL value
        public void SetTaxTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxTypeCode);
        }

        /// test for NULL value
        public bool IsTaxRateCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTaxRateCode);
        }

        /// assign NULL value
        public void SetTaxRateCodeNull()
        {
            this.SetNull(this.myTable.ColumnTaxRateCode);
        }

        /// test for NULL value
        public bool IsTaxValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnTaxValidFrom);
        }

        /// assign NULL value
        public void SetTaxValidFromNull()
        {
            this.SetNull(this.myTable.ColumnTaxValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 192;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArInvoiceDiscount", "a_ar_invoice_discount",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "InvoiceKey", "a_invoice_key_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "ArDiscountCode", "a_ar_discount_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(3, "ArDiscountDateValidFrom", "a_ar_discount_date_valid_from_d", OdbcType.Date, -1, true),
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
        }

        /// Access a typed row by index
        public AArInvoiceDiscountRow this[int i]
        {
            get
            {
                return ((AArInvoiceDiscountRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArInvoiceDiscountTable GetChangesTyped()
        {
            return ((AArInvoiceDiscountTable)(base.GetChangesTypedInternal()));
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
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }

        /// get character length for column
        public static short GetInvoiceKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }

        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDiscountDateValidFromLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArDiscountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountCode);
        }

        /// assign NULL value
        public void SetArDiscountCodeNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
        }

        /// test for NULL value
        public bool IsArDiscountDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountDateValidFrom);
        }

        /// assign NULL value
        public void SetArDiscountDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 193;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AArInvoiceDetailDiscount", "a_ar_invoice_detail_discount",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "InvoiceKey", "a_invoice_key_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "DetailNumber", "a_detail_number_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "ArDiscountCode", "a_ar_discount_code_c", OdbcType.VarChar, 100, true),
                    new TTypedColumnInfo(4, "ArDiscountDateValidFrom", "a_ar_discount_date_valid_from_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(5, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3, 4
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AArInvoiceDetailDiscountRow this[int i]
        {
            get
            {
                return ((AArInvoiceDetailDiscountRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AArInvoiceDetailDiscountTable GetChangesTyped()
        {
            return ((AArInvoiceDetailDiscountTable)(base.GetChangesTypedInternal()));
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
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }

        /// get character length for column
        public static short GetInvoiceKeyLength()
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
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }

        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }

        /// get character length for column
        public static short GetArDiscountDateValidFromLength()
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsArDiscountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountCode);
        }

        /// assign NULL value
        public void SetArDiscountCodeNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
        }

        /// test for NULL value
        public bool IsArDiscountDateValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountDateValidFrom);
        }

        /// assign NULL value
        public void SetArDiscountDateValidFromNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
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