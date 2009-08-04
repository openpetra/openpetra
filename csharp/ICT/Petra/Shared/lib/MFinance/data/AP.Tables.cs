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
namespace Ict.Petra.Shared.MFinance.AP.Data
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

    /// This table defines the concept of a supplier in the AP system and is the centre of the AP system.
    [Serializable()]
    public class AApSupplierTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 175;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPreferredScreenDisplayId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultBankAccountId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentTypeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCurrencyCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultApAccountId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultCreditTermsId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultDiscountPercentageId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultDiscountDaysId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnSupplierTypeId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultExpAccountId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnDefaultCostCentreId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 16;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApSupplier", "a_ap_supplier",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PreferredScreenDisplay", "a_preferred_screen_display_i", "Preferred Screen Display", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "DefaultBankAccount", "a_default_bank_account_c", "Default Bank Account", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(3, "PaymentType", "a_payment_type_c", "Default Payment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(4, "CurrencyCode", "a_currency_code_c", "Currency", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "DefaultApAccount", "a_default_ap_account_c", "Default AP Account", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "DefaultCreditTerms", "a_default_credit_terms_i", "Default Credit Terms", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "DefaultDiscountPercentage", "a_default_discount_percentage_n", "Default Discount Percentage", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(8, "DefaultDiscountDays", "a_default_discount_days_i", "Default Discount Days", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "SupplierType", "a_supplier_type_c", "Supplier Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(10, "DefaultExpAccount", "a_default_exp_account_c", "Default Expense Account", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(11, "DefaultCostCentre", "a_default_cost_centre_c", "Default Cost Centre", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(12, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

        /// constructor
        public AApSupplierTable() :
                base("AApSupplier")
        {
        }

        /// constructor
        public AApSupplierTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApSupplierTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// Reference to the partner key for this supplier
        public DataColumn ColumnPartnerKey;
        /// Number of months to display invoices and credit notes
        public DataColumn ColumnPreferredScreenDisplay;
        /// Reference to default bank account to use to pay supplier with.
        public DataColumn ColumnDefaultBankAccount;
        /// The default type of payment to use when paying this supplier.
        public DataColumn ColumnPaymentType;
        /// The currency code to use for this supplier.
        public DataColumn ColumnCurrencyCode;
        /// The default AP Account to use when paying this supplier.
        public DataColumn ColumnDefaultApAccount;
        /// Default credit terms to use for invoices from this supplier.
        public DataColumn ColumnDefaultCreditTerms;
        /// Default percentage discount to receive for early payments.
        public DataColumn ColumnDefaultDiscountPercentage;
        /// Default number of days in which the discount percentage has effect.
        public DataColumn ColumnDefaultDiscountDays;
        /// What type of supplier this is - normal, credit card, maybe something else.
        public DataColumn ColumnSupplierType;
        /// Reference to the default expense Account to use for invoice details.
        public DataColumn ColumnDefaultExpAccount;
        /// Reference to the default cost centre to use for invoice details.
        public DataColumn ColumnDefaultCostCentre;
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
            this.Columns.Add(new System.Data.DataColumn("a_preferred_screen_display_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_default_bank_account_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_payment_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_default_ap_account_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_default_credit_terms_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_default_discount_percentage_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_default_discount_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_supplier_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_default_exp_account_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_default_cost_centre_c", typeof(String)));
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
            this.ColumnPreferredScreenDisplay = this.Columns["a_preferred_screen_display_i"];
            this.ColumnDefaultBankAccount = this.Columns["a_default_bank_account_c"];
            this.ColumnPaymentType = this.Columns["a_payment_type_c"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnDefaultApAccount = this.Columns["a_default_ap_account_c"];
            this.ColumnDefaultCreditTerms = this.Columns["a_default_credit_terms_i"];
            this.ColumnDefaultDiscountPercentage = this.Columns["a_default_discount_percentage_n"];
            this.ColumnDefaultDiscountDays = this.Columns["a_default_discount_days_i"];
            this.ColumnSupplierType = this.Columns["a_supplier_type_c"];
            this.ColumnDefaultExpAccount = this.Columns["a_default_exp_account_c"];
            this.ColumnDefaultCostCentre = this.Columns["a_default_cost_centre_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApSupplierRow this[int i]
        {
            get
            {
                return ((AApSupplierRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApSupplierRow NewRowTyped(bool AWithDefaultValues)
        {
            AApSupplierRow ret = ((AApSupplierRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApSupplierRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApSupplierRow(builder);
        }

        /// get typed set of changes
        public AApSupplierTable GetChangesTyped()
        {
            return ((AApSupplierTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApSupplier";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_supplier";
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
        public static string GetPreferredScreenDisplayDBName()
        {
            return "a_preferred_screen_display_i";
        }

        /// get character length for column
        public static short GetPreferredScreenDisplayLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultBankAccountDBName()
        {
            return "a_default_bank_account_c";
        }

        /// get character length for column
        public static short GetDefaultBankAccountLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetPaymentTypeDBName()
        {
            return "a_payment_type_c";
        }

        /// get character length for column
        public static short GetPaymentTypeLength()
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
        public static string GetDefaultApAccountDBName()
        {
            return "a_default_ap_account_c";
        }

        /// get character length for column
        public static short GetDefaultApAccountLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultCreditTermsDBName()
        {
            return "a_default_credit_terms_i";
        }

        /// get character length for column
        public static short GetDefaultCreditTermsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultDiscountPercentageDBName()
        {
            return "a_default_discount_percentage_n";
        }

        /// get character length for column
        public static short GetDefaultDiscountPercentageLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultDiscountDaysDBName()
        {
            return "a_default_discount_days_i";
        }

        /// get character length for column
        public static short GetDefaultDiscountDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSupplierTypeDBName()
        {
            return "a_supplier_type_c";
        }

        /// get character length for column
        public static short GetSupplierTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultExpAccountDBName()
        {
            return "a_default_exp_account_c";
        }

        /// get character length for column
        public static short GetDefaultExpAccountLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDefaultCostCentreDBName()
        {
            return "a_default_cost_centre_c";
        }

        /// get character length for column
        public static short GetDefaultCostCentreLength()
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

    /// This table defines the concept of a supplier in the AP system and is the centre of the AP system.
    [Serializable()]
    public class AApSupplierRow : System.Data.DataRow
    {
        private AApSupplierTable myTable;

        /// Constructor
        public AApSupplierRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApSupplierTable)(this.Table));
        }

        /// Reference to the partner key for this supplier
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

        /// Number of months to display invoices and credit notes
        public Int32 PreferredScreenDisplay
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPreferredScreenDisplay.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPreferredScreenDisplay)
                            || (((Int32)(this[this.myTable.ColumnPreferredScreenDisplay])) != value)))
                {
                    this[this.myTable.ColumnPreferredScreenDisplay] = value;
                }
            }
        }

        /// Reference to default bank account to use to pay supplier with.
        public String DefaultBankAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultBankAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDefaultBankAccount)
                            || (((String)(this[this.myTable.ColumnDefaultBankAccount])) != value)))
                {
                    this[this.myTable.ColumnDefaultBankAccount] = value;
                }
            }
        }

        /// The default type of payment to use when paying this supplier.
        public String PaymentType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPaymentType)
                            || (((String)(this[this.myTable.ColumnPaymentType])) != value)))
                {
                    this[this.myTable.ColumnPaymentType] = value;
                }
            }
        }

        /// The currency code to use for this supplier.
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

        /// The default AP Account to use when paying this supplier.
        public String DefaultApAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultApAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDefaultApAccount)
                            || (((String)(this[this.myTable.ColumnDefaultApAccount])) != value)))
                {
                    this[this.myTable.ColumnDefaultApAccount] = value;
                }
            }
        }

        /// Default credit terms to use for invoices from this supplier.
        public Int32 DefaultCreditTerms
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultCreditTerms.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDefaultCreditTerms)
                            || (((Int32)(this[this.myTable.ColumnDefaultCreditTerms])) != value)))
                {
                    this[this.myTable.ColumnDefaultCreditTerms] = value;
                }
            }
        }

        /// Default percentage discount to receive for early payments.
        public Double DefaultDiscountPercentage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultDiscountPercentage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDefaultDiscountPercentage)
                            || (((Double)(this[this.myTable.ColumnDefaultDiscountPercentage])) != value)))
                {
                    this[this.myTable.ColumnDefaultDiscountPercentage] = value;
                }
            }
        }

        /// Default number of days in which the discount percentage has effect.
        public Int32 DefaultDiscountDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultDiscountDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDefaultDiscountDays)
                            || (((Int32)(this[this.myTable.ColumnDefaultDiscountDays])) != value)))
                {
                    this[this.myTable.ColumnDefaultDiscountDays] = value;
                }
            }
        }

        /// What type of supplier this is - normal, credit card, maybe something else.
        public String SupplierType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSupplierType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSupplierType)
                            || (((String)(this[this.myTable.ColumnSupplierType])) != value)))
                {
                    this[this.myTable.ColumnSupplierType] = value;
                }
            }
        }

        /// Reference to the default expense Account to use for invoice details.
        public String DefaultExpAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultExpAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDefaultExpAccount)
                            || (((String)(this[this.myTable.ColumnDefaultExpAccount])) != value)))
                {
                    this[this.myTable.ColumnDefaultExpAccount] = value;
                }
            }
        }

        /// Reference to the default cost centre to use for invoice details.
        public String DefaultCostCentre
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultCostCentre.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDefaultCostCentre)
                            || (((String)(this[this.myTable.ColumnDefaultCostCentre])) != value)))
                {
                    this[this.myTable.ColumnDefaultCostCentre] = value;
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
            this.SetNull(this.myTable.ColumnPartnerKey);
            this.SetNull(this.myTable.ColumnPreferredScreenDisplay);
            this.SetNull(this.myTable.ColumnDefaultBankAccount);
            this.SetNull(this.myTable.ColumnPaymentType);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnDefaultApAccount);
            this.SetNull(this.myTable.ColumnDefaultCreditTerms);
            this.SetNull(this.myTable.ColumnDefaultDiscountPercentage);
            this.SetNull(this.myTable.ColumnDefaultDiscountDays);
            this.SetNull(this.myTable.ColumnSupplierType);
            this.SetNull(this.myTable.ColumnDefaultExpAccount);
            this.SetNull(this.myTable.ColumnDefaultCostCentre);
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
        public bool IsPreferredScreenDisplayNull()
        {
            return this.IsNull(this.myTable.ColumnPreferredScreenDisplay);
        }

        /// assign NULL value
        public void SetPreferredScreenDisplayNull()
        {
            this.SetNull(this.myTable.ColumnPreferredScreenDisplay);
        }

        /// test for NULL value
        public bool IsDefaultBankAccountNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultBankAccount);
        }

        /// assign NULL value
        public void SetDefaultBankAccountNull()
        {
            this.SetNull(this.myTable.ColumnDefaultBankAccount);
        }

        /// test for NULL value
        public bool IsPaymentTypeNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentType);
        }

        /// assign NULL value
        public void SetPaymentTypeNull()
        {
            this.SetNull(this.myTable.ColumnPaymentType);
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
        public bool IsDefaultApAccountNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultApAccount);
        }

        /// assign NULL value
        public void SetDefaultApAccountNull()
        {
            this.SetNull(this.myTable.ColumnDefaultApAccount);
        }

        /// test for NULL value
        public bool IsDefaultCreditTermsNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultCreditTerms);
        }

        /// assign NULL value
        public void SetDefaultCreditTermsNull()
        {
            this.SetNull(this.myTable.ColumnDefaultCreditTerms);
        }

        /// test for NULL value
        public bool IsDefaultDiscountPercentageNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultDiscountPercentage);
        }

        /// assign NULL value
        public void SetDefaultDiscountPercentageNull()
        {
            this.SetNull(this.myTable.ColumnDefaultDiscountPercentage);
        }

        /// test for NULL value
        public bool IsDefaultDiscountDaysNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultDiscountDays);
        }

        /// assign NULL value
        public void SetDefaultDiscountDaysNull()
        {
            this.SetNull(this.myTable.ColumnDefaultDiscountDays);
        }

        /// test for NULL value
        public bool IsSupplierTypeNull()
        {
            return this.IsNull(this.myTable.ColumnSupplierType);
        }

        /// assign NULL value
        public void SetSupplierTypeNull()
        {
            this.SetNull(this.myTable.ColumnSupplierType);
        }

        /// test for NULL value
        public bool IsDefaultExpAccountNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultExpAccount);
        }

        /// assign NULL value
        public void SetDefaultExpAccountNull()
        {
            this.SetNull(this.myTable.ColumnDefaultExpAccount);
        }

        /// test for NULL value
        public bool IsDefaultCostCentreNull()
        {
            return this.IsNull(this.myTable.ColumnDefaultCostCentre);
        }

        /// assign NULL value
        public void SetDefaultCostCentreNull()
        {
            this.SetNull(this.myTable.ColumnDefaultCostCentre);
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

    /// This is either an invoice or a credit note in the Accounts Payable system.
    [Serializable()]
    public class AApDocumentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 176;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreditNoteFlagId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDocumentCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnReferenceId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateIssuedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEnteredId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreditTermsId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnTotalAmountId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnExchangeRateToBaseId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnDiscountPercentageId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnDiscountDaysId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnApAccountId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastDetailNumberId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnDocumentStatusId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 20;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApDocument", "a_ap_document",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ApNumber", "a_ap_number_i", "AP Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "PartnerKey", "p_partner_key_n", "Supplier Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(3, "CreditNoteFlag", "a_credit_note_flag_l", "Credit Note Flag", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "DocumentCode", "a_document_code_c", "Invoice Number", OdbcType.VarChar, 30, false),
                    new TTypedColumnInfo(5, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(6, "DateIssued", "a_date_issued_d", "Date Issued", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(7, "DateEntered", "a_date_entered_d", "Date Entered", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(8, "CreditTerms", "a_credit_terms_i", "Credit Terms", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "TotalAmount", "a_total_amount_n", "Total Amount", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(10, "ExchangeRateToBase", "a_exchange_rate_to_base_n", "Exchange Rate to Base", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(11, "DiscountPercentage", "a_discount_percentage_n", "Discount Percentage", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(12, "DiscountDays", "a_discount_days_i", "Discount Days", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "ApAccount", "a_ap_account_c", "AP Account", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(14, "LastDetailNumber", "a_last_detail_number_i", "Last Detail Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(15, "DocumentStatus", "a_document_status_c", "Document Status", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(16, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(19, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(20, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public AApDocumentTable() :
                base("AApDocument")
        {
        }

        /// constructor
        public AApDocumentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApDocumentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// Reference to the ledger for this invoice.
        public DataColumn ColumnLedgerNumber;
        /// A unique key (together with the ledger number) to identify this document.
        public DataColumn ColumnApNumber;
        /// Reference to the supplier that sent this invoice.
        public DataColumn ColumnPartnerKey;
        /// A flag to indicate if this document is an invoice or a credit note.
        public DataColumn ColumnCreditNoteFlag;
        /// The code given on the document itself (be it invoice or credit note). This will have to be unique for each supplier.
        public DataColumn ColumnDocumentCode;
        /// Some kind of other reference needed.
        public DataColumn ColumnReference;
        /// The date when this document was issued.
        public DataColumn ColumnDateIssued;
        /// The date when this document was entered into the system.
        public DataColumn ColumnDateEntered;
        /// Credit terms allowed for this invoice.
        public DataColumn ColumnCreditTerms;
        /// The total amount of money that this document is worth.
        public DataColumn ColumnTotalAmount;
        /// The exchange rate to the base currency at the time that the document was issued.
        public DataColumn ColumnExchangeRateToBase;
        /// The percentage discount you get for early payment of this document in the case that it is an invoice.
        public DataColumn ColumnDiscountPercentage;
        /// The number of days that the discount is valid for (0 for none).
        public DataColumn ColumnDiscountDays;
        /// Reference to the AP Account to debit/credit when posting/paying the document.
        public DataColumn ColumnApAccount;
        /// The number of the last item for this document. This is used simply to quickly get the next number if items are added.
        public DataColumn ColumnLastDetailNumber;
        /// The current status of the invoice. The value can (for now) be one of: OPEN, APPROVED, POSTED, PARTPAID, or PAID.
        public DataColumn ColumnDocumentStatus;
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
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_credit_note_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_document_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_date_issued_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_date_entered_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_credit_terms_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_total_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_exchange_rate_to_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_discount_percentage_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_discount_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ap_account_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_last_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_document_status_c", typeof(String)));
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnCreditNoteFlag = this.Columns["a_credit_note_flag_l"];
            this.ColumnDocumentCode = this.Columns["a_document_code_c"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnDateIssued = this.Columns["a_date_issued_d"];
            this.ColumnDateEntered = this.Columns["a_date_entered_d"];
            this.ColumnCreditTerms = this.Columns["a_credit_terms_i"];
            this.ColumnTotalAmount = this.Columns["a_total_amount_n"];
            this.ColumnExchangeRateToBase = this.Columns["a_exchange_rate_to_base_n"];
            this.ColumnDiscountPercentage = this.Columns["a_discount_percentage_n"];
            this.ColumnDiscountDays = this.Columns["a_discount_days_i"];
            this.ColumnApAccount = this.Columns["a_ap_account_c"];
            this.ColumnLastDetailNumber = this.Columns["a_last_detail_number_i"];
            this.ColumnDocumentStatus = this.Columns["a_document_status_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApDocumentRow this[int i]
        {
            get
            {
                return ((AApDocumentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApDocumentRow NewRowTyped(bool AWithDefaultValues)
        {
            AApDocumentRow ret = ((AApDocumentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApDocumentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApDocumentRow(builder);
        }

        /// get typed set of changes
        public AApDocumentTable GetChangesTyped()
        {
            return ((AApDocumentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApDocument";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_document";
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }

        /// get character length for column
        public static short GetApNumberLength()
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
        public static string GetCreditNoteFlagDBName()
        {
            return "a_credit_note_flag_l";
        }

        /// get character length for column
        public static short GetCreditNoteFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDocumentCodeDBName()
        {
            return "a_document_code_c";
        }

        /// get character length for column
        public static short GetDocumentCodeLength()
        {
            return 30;
        }

        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }

        /// get character length for column
        public static short GetReferenceLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetDateIssuedDBName()
        {
            return "a_date_issued_d";
        }

        /// get character length for column
        public static short GetDateIssuedLength()
        {
            return -1;
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
        public static string GetCreditTermsDBName()
        {
            return "a_credit_terms_i";
        }

        /// get character length for column
        public static short GetCreditTermsLength()
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
        public static string GetDiscountPercentageDBName()
        {
            return "a_discount_percentage_n";
        }

        /// get character length for column
        public static short GetDiscountPercentageLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountDaysDBName()
        {
            return "a_discount_days_i";
        }

        /// get character length for column
        public static short GetDiscountDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetApAccountDBName()
        {
            return "a_ap_account_c";
        }

        /// get character length for column
        public static short GetApAccountLength()
        {
            return 16;
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
        public static string GetDocumentStatusDBName()
        {
            return "a_document_status_c";
        }

        /// get character length for column
        public static short GetDocumentStatusLength()
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

    /// This is either an invoice or a credit note in the Accounts Payable system.
    [Serializable()]
    public class AApDocumentRow : System.Data.DataRow
    {
        private AApDocumentTable myTable;

        /// Constructor
        public AApDocumentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApDocumentTable)(this.Table));
        }

        /// Reference to the ledger for this invoice.
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

        /// A unique key (together with the ledger number) to identify this document.
        public Int32 ApNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApNumber)
                            || (((Int32)(this[this.myTable.ColumnApNumber])) != value)))
                {
                    this[this.myTable.ColumnApNumber] = value;
                }
            }
        }

        /// Reference to the supplier that sent this invoice.
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

        /// A flag to indicate if this document is an invoice or a credit note.
        public Boolean CreditNoteFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreditNoteFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCreditNoteFlag)
                            || (((Boolean)(this[this.myTable.ColumnCreditNoteFlag])) != value)))
                {
                    this[this.myTable.ColumnCreditNoteFlag] = value;
                }
            }
        }

        /// The code given on the document itself (be it invoice or credit note). This will have to be unique for each supplier.
        public String DocumentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDocumentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDocumentCode)
                            || (((String)(this[this.myTable.ColumnDocumentCode])) != value)))
                {
                    this[this.myTable.ColumnDocumentCode] = value;
                }
            }
        }

        /// Some kind of other reference needed.
        public String Reference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// The date when this document was issued.
        public System.DateTime DateIssued
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateIssued.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateIssued)
                            || (((System.DateTime)(this[this.myTable.ColumnDateIssued])) != value)))
                {
                    this[this.myTable.ColumnDateIssued] = value;
                }
            }
        }

        /// The date when this document was entered into the system.
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

        /// Credit terms allowed for this invoice.
        public Int32 CreditTerms
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreditTerms.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCreditTerms)
                            || (((Int32)(this[this.myTable.ColumnCreditTerms])) != value)))
                {
                    this[this.myTable.ColumnCreditTerms] = value;
                }
            }
        }

        /// The total amount of money that this document is worth.
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

        /// The exchange rate to the base currency at the time that the document was issued.
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

        /// The percentage discount you get for early payment of this document in the case that it is an invoice.
        public Double DiscountPercentage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscountPercentage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscountPercentage)
                            || (((Double)(this[this.myTable.ColumnDiscountPercentage])) != value)))
                {
                    this[this.myTable.ColumnDiscountPercentage] = value;
                }
            }
        }

        /// The number of days that the discount is valid for (0 for none).
        public Int32 DiscountDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDiscountDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDiscountDays)
                            || (((Int32)(this[this.myTable.ColumnDiscountDays])) != value)))
                {
                    this[this.myTable.ColumnDiscountDays] = value;
                }
            }
        }

        /// Reference to the AP Account to debit/credit when posting/paying the document.
        public String ApAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnApAccount)
                            || (((String)(this[this.myTable.ColumnApAccount])) != value)))
                {
                    this[this.myTable.ColumnApAccount] = value;
                }
            }
        }

        /// The number of the last item for this document. This is used simply to quickly get the next number if items are added.
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

        /// The current status of the invoice. The value can (for now) be one of: OPEN, APPROVED, POSTED, PARTPAID, or PAID.
        public String DocumentStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDocumentStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDocumentStatus)
                            || (((String)(this[this.myTable.ColumnDocumentStatus])) != value)))
                {
                    this[this.myTable.ColumnDocumentStatus] = value;
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
            this.SetNull(this.myTable.ColumnLedgerNumber);
            this.SetNull(this.myTable.ColumnApNumber);
            this.SetNull(this.myTable.ColumnPartnerKey);
            this[this.myTable.ColumnCreditNoteFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnDocumentCode);
            this.SetNull(this.myTable.ColumnReference);
            this[this.myTable.ColumnDateIssued.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnDateEntered.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnCreditTerms.Ordinal] = 0;
            this[this.myTable.ColumnTotalAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnExchangeRateToBase);
            this.SetNull(this.myTable.ColumnDiscountPercentage);
            this.SetNull(this.myTable.ColumnDiscountDays);
            this.SetNull(this.myTable.ColumnApAccount);
            this[this.myTable.ColumnLastDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnDocumentStatus);
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
        public bool IsApNumberNull()
        {
            return this.IsNull(this.myTable.ColumnApNumber);
        }

        /// assign NULL value
        public void SetApNumberNull()
        {
            this.SetNull(this.myTable.ColumnApNumber);
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
        public bool IsCreditNoteFlagNull()
        {
            return this.IsNull(this.myTable.ColumnCreditNoteFlag);
        }

        /// assign NULL value
        public void SetCreditNoteFlagNull()
        {
            this.SetNull(this.myTable.ColumnCreditNoteFlag);
        }

        /// test for NULL value
        public bool IsDocumentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnDocumentCode);
        }

        /// assign NULL value
        public void SetDocumentCodeNull()
        {
            this.SetNull(this.myTable.ColumnDocumentCode);
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
        public bool IsDateIssuedNull()
        {
            return this.IsNull(this.myTable.ColumnDateIssued);
        }

        /// assign NULL value
        public void SetDateIssuedNull()
        {
            this.SetNull(this.myTable.ColumnDateIssued);
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
        public bool IsCreditTermsNull()
        {
            return this.IsNull(this.myTable.ColumnCreditTerms);
        }

        /// assign NULL value
        public void SetCreditTermsNull()
        {
            this.SetNull(this.myTable.ColumnCreditTerms);
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
        public bool IsDiscountPercentageNull()
        {
            return this.IsNull(this.myTable.ColumnDiscountPercentage);
        }

        /// assign NULL value
        public void SetDiscountPercentageNull()
        {
            this.SetNull(this.myTable.ColumnDiscountPercentage);
        }

        /// test for NULL value
        public bool IsDiscountDaysNull()
        {
            return this.IsNull(this.myTable.ColumnDiscountDays);
        }

        /// assign NULL value
        public void SetDiscountDaysNull()
        {
            this.SetNull(this.myTable.ColumnDiscountDays);
        }

        /// test for NULL value
        public bool IsApAccountNull()
        {
            return this.IsNull(this.myTable.ColumnApAccount);
        }

        /// assign NULL value
        public void SetApAccountNull()
        {
            this.SetNull(this.myTable.ColumnApAccount);
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
        public bool IsDocumentStatusNull()
        {
            return this.IsNull(this.myTable.ColumnDocumentStatus);
        }

        /// assign NULL value
        public void SetDocumentStatusNull()
        {
            this.SetNull(this.myTable.ColumnDocumentStatus);
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

    /// This table receives a new entry when a credit note is applied to an invoice. Since the invoices and credit notes share the same table, we need a way to link the two, and this is the role of this table.
    [Serializable()]
    public class ACrdtNoteInvoiceLinkTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 177;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreditNoteNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnInvoiceNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAppliedDateId = 3;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ACrdtNoteInvoiceLink", "a_crdt_note_invoice_link",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "CreditNoteNumber", "a_credit_note_number_i", "Credit Note Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "InvoiceNumber", "a_invoice_number_i", "Invoice Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "AppliedDate", "a_applied_date_d", "Applied Date", OdbcType.Date, -1, false),
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
        public ACrdtNoteInvoiceLinkTable() :
                base("ACrdtNoteInvoiceLink")
        {
        }

        /// constructor
        public ACrdtNoteInvoiceLinkTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public ACrdtNoteInvoiceLinkTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// Reference to the ledger number.
        public DataColumn ColumnLedgerNumber;
        /// Reference to the credit note.
        public DataColumn ColumnCreditNoteNumber;
        /// Reference to the invoice.
        public DataColumn ColumnInvoiceNumber;
        ///
        public DataColumn ColumnAppliedDate;
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
            this.Columns.Add(new System.Data.DataColumn("a_credit_note_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_invoice_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_applied_date_d", typeof(System.DateTime)));
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
            this.ColumnCreditNoteNumber = this.Columns["a_credit_note_number_i"];
            this.ColumnInvoiceNumber = this.Columns["a_invoice_number_i"];
            this.ColumnAppliedDate = this.Columns["a_applied_date_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public ACrdtNoteInvoiceLinkRow this[int i]
        {
            get
            {
                return ((ACrdtNoteInvoiceLinkRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public ACrdtNoteInvoiceLinkRow NewRowTyped(bool AWithDefaultValues)
        {
            ACrdtNoteInvoiceLinkRow ret = ((ACrdtNoteInvoiceLinkRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public ACrdtNoteInvoiceLinkRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ACrdtNoteInvoiceLinkRow(builder);
        }

        /// get typed set of changes
        public ACrdtNoteInvoiceLinkTable GetChangesTyped()
        {
            return ((ACrdtNoteInvoiceLinkTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ACrdtNoteInvoiceLink";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_crdt_note_invoice_link";
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
        public static string GetCreditNoteNumberDBName()
        {
            return "a_credit_note_number_i";
        }

        /// get character length for column
        public static short GetCreditNoteNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetInvoiceNumberDBName()
        {
            return "a_invoice_number_i";
        }

        /// get character length for column
        public static short GetInvoiceNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAppliedDateDBName()
        {
            return "a_applied_date_d";
        }

        /// get character length for column
        public static short GetAppliedDateLength()
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

    /// This table receives a new entry when a credit note is applied to an invoice. Since the invoices and credit notes share the same table, we need a way to link the two, and this is the role of this table.
    [Serializable()]
    public class ACrdtNoteInvoiceLinkRow : System.Data.DataRow
    {
        private ACrdtNoteInvoiceLinkTable myTable;

        /// Constructor
        public ACrdtNoteInvoiceLinkRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((ACrdtNoteInvoiceLinkTable)(this.Table));
        }

        /// Reference to the ledger number.
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

        /// Reference to the credit note.
        public Int32 CreditNoteNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreditNoteNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCreditNoteNumber)
                            || (((Int32)(this[this.myTable.ColumnCreditNoteNumber])) != value)))
                {
                    this[this.myTable.ColumnCreditNoteNumber] = value;
                }
            }
        }

        /// Reference to the invoice.
        public Int32 InvoiceNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInvoiceNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInvoiceNumber)
                            || (((Int32)(this[this.myTable.ColumnInvoiceNumber])) != value)))
                {
                    this[this.myTable.ColumnInvoiceNumber] = value;
                }
            }
        }

        ///
        public System.DateTime AppliedDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAppliedDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAppliedDate)
                            || (((System.DateTime)(this[this.myTable.ColumnAppliedDate])) != value)))
                {
                    this[this.myTable.ColumnAppliedDate] = value;
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
            this.SetNull(this.myTable.ColumnLedgerNumber);
            this.SetNull(this.myTable.ColumnCreditNoteNumber);
            this.SetNull(this.myTable.ColumnInvoiceNumber);
            this.SetNull(this.myTable.ColumnAppliedDate);
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
        public bool IsCreditNoteNumberNull()
        {
            return this.IsNull(this.myTable.ColumnCreditNoteNumber);
        }

        /// assign NULL value
        public void SetCreditNoteNumberNull()
        {
            this.SetNull(this.myTable.ColumnCreditNoteNumber);
        }

        /// test for NULL value
        public bool IsInvoiceNumberNull()
        {
            return this.IsNull(this.myTable.ColumnInvoiceNumber);
        }

        /// assign NULL value
        public void SetInvoiceNumberNull()
        {
            this.SetNull(this.myTable.ColumnInvoiceNumber);
        }

        /// test for NULL value
        public bool IsAppliedDateNull()
        {
            return this.IsNull(this.myTable.ColumnAppliedDate);
        }

        /// assign NULL value
        public void SetAppliedDateNull()
        {
            this.SetNull(this.myTable.ColumnAppliedDate);
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

    /// An invoice or credit note consists out of several items, or details. This table contains all these details.
    [Serializable()]
    public class AApDocumentDetailTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 178;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDetailNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDetailApprovedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCostCentreCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnAccountCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemRefId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnNarrativeId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnApprovalDateId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 14;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApDocumentDetail", "a_ap_document_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ApNumber", "a_ap_number_i", "AP Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "DetailNumber", "a_detail_number_i", "Detail Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "DetailApproved", "a_detail_approved_l", "Approved Flag", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(4, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(5, "AccountCode", "a_account_code_c", "Account Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "ItemRef", "a_item_ref_c", "Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(7, "Narrative", "a_narrative_c", "Narrative", OdbcType.VarChar, 200, false),
                    new TTypedColumnInfo(8, "Amount", "a_amount_n", "Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(9, "ApprovalDate", "a_approval_date_d", "Date Approved", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(12, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

        /// constructor
        public AApDocumentDetailTable() :
                base("AApDocumentDetail")
        {
        }

        /// constructor
        public AApDocumentDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApDocumentDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// Reference to the ledger
        public DataColumn ColumnLedgerNumber;
        /// Reference to the document
        public DataColumn ColumnApNumber;
        /// A unique number for this detail for its document.
        public DataColumn ColumnDetailNumber;
        /// Indicates if this detail has been approved or not.
        public DataColumn ColumnDetailApproved;
        /// Reference to the cost centre to use for this detail.
        public DataColumn ColumnCostCentreCode;
        /// Reference to the account to use for this detail
        public DataColumn ColumnAccountCode;
        /// Some other reference to the item.
        public DataColumn ColumnItemRef;
        /// A narrative about what this is.
        public DataColumn ColumnNarrative;
        /// The amount of money this detail is worth.
        public DataColumn ColumnAmount;
        /// The date when this detail was approved.
        public DataColumn ColumnApprovalDate;
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
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_approved_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_item_ref_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_narrative_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_approval_date_d", typeof(System.DateTime)));
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnDetailApproved = this.Columns["a_detail_approved_l"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnItemRef = this.Columns["a_item_ref_c"];
            this.ColumnNarrative = this.Columns["a_narrative_c"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnApprovalDate = this.Columns["a_approval_date_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApDocumentDetailRow this[int i]
        {
            get
            {
                return ((AApDocumentDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApDocumentDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            AApDocumentDetailRow ret = ((AApDocumentDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApDocumentDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApDocumentDetailRow(builder);
        }

        /// get typed set of changes
        public AApDocumentDetailTable GetChangesTyped()
        {
            return ((AApDocumentDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApDocumentDetail";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_document_detail";
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }

        /// get character length for column
        public static short GetApNumberLength()
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
        public static string GetDetailApprovedDBName()
        {
            return "a_detail_approved_l";
        }

        /// get character length for column
        public static short GetDetailApprovedLength()
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
        public static string GetItemRefDBName()
        {
            return "a_item_ref_c";
        }

        /// get character length for column
        public static short GetItemRefLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetNarrativeDBName()
        {
            return "a_narrative_c";
        }

        /// get character length for column
        public static short GetNarrativeLength()
        {
            return 200;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetApprovalDateDBName()
        {
            return "a_approval_date_d";
        }

        /// get character length for column
        public static short GetApprovalDateLength()
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

    /// An invoice or credit note consists out of several items, or details. This table contains all these details.
    [Serializable()]
    public class AApDocumentDetailRow : System.Data.DataRow
    {
        private AApDocumentDetailTable myTable;

        /// Constructor
        public AApDocumentDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApDocumentDetailTable)(this.Table));
        }

        /// Reference to the ledger
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

        /// Reference to the document
        public Int32 ApNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApNumber)
                            || (((Int32)(this[this.myTable.ColumnApNumber])) != value)))
                {
                    this[this.myTable.ColumnApNumber] = value;
                }
            }
        }

        /// A unique number for this detail for its document.
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

        /// Indicates if this detail has been approved or not.
        public Boolean DetailApproved
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDetailApproved.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDetailApproved)
                            || (((Boolean)(this[this.myTable.ColumnDetailApproved])) != value)))
                {
                    this[this.myTable.ColumnDetailApproved] = value;
                }
            }
        }

        /// Reference to the cost centre to use for this detail.
        public String CostCentreCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostCentreCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// Reference to the account to use for this detail
        public String AccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// Some other reference to the item.
        public String ItemRef
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemRef.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnItemRef)
                            || (((String)(this[this.myTable.ColumnItemRef])) != value)))
                {
                    this[this.myTable.ColumnItemRef] = value;
                }
            }
        }

        /// A narrative about what this is.
        public String Narrative
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNarrative.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnNarrative)
                            || (((String)(this[this.myTable.ColumnNarrative])) != value)))
                {
                    this[this.myTable.ColumnNarrative] = value;
                }
            }
        }

        /// The amount of money this detail is worth.
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

        /// The date when this detail was approved.
        public System.DateTime ApprovalDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApprovalDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApprovalDate)
                            || (((System.DateTime)(this[this.myTable.ColumnApprovalDate])) != value)))
                {
                    this[this.myTable.ColumnApprovalDate] = value;
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
            this.SetNull(this.myTable.ColumnLedgerNumber);
            this.SetNull(this.myTable.ColumnApNumber);
            this.SetNull(this.myTable.ColumnDetailNumber);
            this[this.myTable.ColumnDetailApproved.Ordinal] = false;
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnItemRef);
            this.SetNull(this.myTable.ColumnNarrative);
            this.SetNull(this.myTable.ColumnAmount);
            this.SetNull(this.myTable.ColumnApprovalDate);
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
        public bool IsApNumberNull()
        {
            return this.IsNull(this.myTable.ColumnApNumber);
        }

        /// assign NULL value
        public void SetApNumberNull()
        {
            this.SetNull(this.myTable.ColumnApNumber);
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
        public bool IsDetailApprovedNull()
        {
            return this.IsNull(this.myTable.ColumnDetailApproved);
        }

        /// assign NULL value
        public void SetDetailApprovedNull()
        {
            this.SetNull(this.myTable.ColumnDetailApproved);
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
        public bool IsItemRefNull()
        {
            return this.IsNull(this.myTable.ColumnItemRef);
        }

        /// assign NULL value
        public void SetItemRefNull()
        {
            this.SetNull(this.myTable.ColumnItemRef);
        }

        /// test for NULL value
        public bool IsNarrativeNull()
        {
            return this.IsNull(this.myTable.ColumnNarrative);
        }

        /// assign NULL value
        public void SetNarrativeNull()
        {
            this.SetNull(this.myTable.ColumnNarrative);
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
        public bool IsApprovalDateNull()
        {
            return this.IsNull(this.myTable.ColumnApprovalDate);
        }

        /// assign NULL value
        public void SetApprovalDateNull()
        {
            this.SetNull(this.myTable.ColumnApprovalDate);
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

    /// Records all payments that have been made against an accounts payable detail.
    [Serializable()]
    public class AApPaymentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 179;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnExchangeRateToBaseId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentDateId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnUserIdId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnReferenceId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountId = 8;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApPayment", "a_ap_payment",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "PaymentNumber", "a_payment_number_i", "Payment Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "Amount", "a_amount_n", "Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(3, "ExchangeRateToBase", "a_exchange_rate_to_base_n", "Exchange Rate To Base", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(4, "PaymentDate", "a_payment_date_d", "Payment Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "UserId", "s_user_id_c", "User ID", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "MethodOfPayment", "a_method_of_payment_c", "Method Of Payment", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(8, "BankAccount", "a_bank_account_c", "Bank Account", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(9, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(13, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public AApPaymentTable() :
                base("AApPayment")
        {
        }

        /// constructor
        public AApPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// Unique number to identify each payment batch.
        public DataColumn ColumnPaymentNumber;
        /// The amount of money that was paid
        public DataColumn ColumnAmount;
        /// The exchange rate to the base currency at the time of payment.
        public DataColumn ColumnExchangeRateToBase;
        /// Date that the payment for an accounts payable was made.
        public DataColumn ColumnPaymentDate;
        /// This is the system user id of the person who made the payment.
        public DataColumn ColumnUserId;
        /// Method that was used to make the payment - cheque, cash, ep, credit card, etc.
        public DataColumn ColumnMethodOfPayment;
        /// The source or reference for the accounts payable payment.  This could be a cheque number.
        public DataColumn ColumnReference;
        /// Bank account from which to make the payment
        public DataColumn ColumnBankAccount;
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
            this.Columns.Add(new System.Data.DataColumn("a_payment_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_exchange_rate_to_base_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_payment_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_user_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_account_c", typeof(String)));
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
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnExchangeRateToBase = this.Columns["a_exchange_rate_to_base_n"];
            this.ColumnPaymentDate = this.Columns["a_payment_date_d"];
            this.ColumnUserId = this.Columns["s_user_id_c"];
            this.ColumnMethodOfPayment = this.Columns["a_method_of_payment_c"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnBankAccount = this.Columns["a_bank_account_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApPaymentRow this[int i]
        {
            get
            {
                return ((AApPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AApPaymentRow ret = ((AApPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApPaymentRow(builder);
        }

        /// get typed set of changes
        public AApPaymentTable GetChangesTyped()
        {
            return ((AApPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApPayment";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_payment";
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
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }

        /// get character length for column
        public static short GetPaymentNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
        {
            return 24;
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
        public static string GetPaymentDateDBName()
        {
            return "a_payment_date_d";
        }

        /// get character length for column
        public static short GetPaymentDateLength()
        {
            return -1;
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
        public static string GetMethodOfPaymentDBName()
        {
            return "a_method_of_payment_c";
        }

        /// get character length for column
        public static short GetMethodOfPaymentLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }

        /// get character length for column
        public static short GetReferenceLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountDBName()
        {
            return "a_bank_account_c";
        }

        /// get character length for column
        public static short GetBankAccountLength()
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

    /// Records all payments that have been made against an accounts payable detail.
    [Serializable()]
    public class AApPaymentRow : System.Data.DataRow
    {
        private AApPaymentTable myTable;

        /// Constructor
        public AApPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApPaymentTable)(this.Table));
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

        /// Unique number to identify each payment batch.
        public Int32 PaymentNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPaymentNumber)
                            || (((Int32)(this[this.myTable.ColumnPaymentNumber])) != value)))
                {
                    this[this.myTable.ColumnPaymentNumber] = value;
                }
            }
        }

        /// The amount of money that was paid
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

        /// The exchange rate to the base currency at the time of payment.
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

        /// Date that the payment for an accounts payable was made.
        public System.DateTime PaymentDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPaymentDate)
                            || (((System.DateTime)(this[this.myTable.ColumnPaymentDate])) != value)))
                {
                    this[this.myTable.ColumnPaymentDate] = value;
                }
            }
        }

        /// This is the system user id of the person who made the payment.
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

        /// Method that was used to make the payment - cheque, cash, ep, credit card, etc.
        public String MethodOfPayment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPayment.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPayment)
                            || (((String)(this[this.myTable.ColumnMethodOfPayment])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPayment] = value;
                }
            }
        }

        /// The source or reference for the accounts payable payment.  This could be a cheque number.
        public String Reference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// Bank account from which to make the payment
        public String BankAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccount)
                            || (((String)(this[this.myTable.ColumnBankAccount])) != value)))
                {
                    this[this.myTable.ColumnBankAccount] = value;
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
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnPaymentNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAmount);
            this.SetNull(this.myTable.ColumnExchangeRateToBase);
            this.SetNull(this.myTable.ColumnPaymentDate);
            this.SetNull(this.myTable.ColumnUserId);
            this.SetNull(this.myTable.ColumnMethodOfPayment);
            this.SetNull(this.myTable.ColumnReference);
            this.SetNull(this.myTable.ColumnBankAccount);
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
        public bool IsPaymentNumberNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentNumber);
        }

        /// assign NULL value
        public void SetPaymentNumberNull()
        {
            this.SetNull(this.myTable.ColumnPaymentNumber);
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
        public bool IsPaymentDateNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentDate);
        }

        /// assign NULL value
        public void SetPaymentDateNull()
        {
            this.SetNull(this.myTable.ColumnPaymentDate);
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
        public bool IsMethodOfPaymentNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPayment);
        }

        /// assign NULL value
        public void SetMethodOfPaymentNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPayment);
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
        public bool IsBankAccountNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccount);
        }

        /// assign NULL value
        public void SetBankAccountNull()
        {
            this.SetNull(this.myTable.ColumnBankAccount);
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

    /// This table links the different payments to actual invoices and credit notes.
    [Serializable()]
    public class AApDocumentPaymentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 180;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 3;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApDocumentPayment", "a_ap_document_payment",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ApNumber", "a_ap_number_i", "AP Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "PaymentNumber", "a_payment_number_i", "Payment Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "Amount", "a_amount_n", "Amount", OdbcType.Decimal, 24, false),
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
        public AApDocumentPaymentTable() :
                base("AApDocumentPayment")
        {
        }

        /// constructor
        public AApDocumentPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApDocumentPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// The ledger that the attribute value is associated with.
        public DataColumn ColumnLedgerNumber;
        /// Accounts Payable Sequence Number
        public DataColumn ColumnApNumber;
        /// Unique number to identify each payment batch.
        public DataColumn ColumnPaymentNumber;
        /// The amount of money that was paid
        public DataColumn ColumnAmount;
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
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_payment_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApDocumentPaymentRow this[int i]
        {
            get
            {
                return ((AApDocumentPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApDocumentPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AApDocumentPaymentRow ret = ((AApDocumentPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApDocumentPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApDocumentPaymentRow(builder);
        }

        /// get typed set of changes
        public AApDocumentPaymentTable GetChangesTyped()
        {
            return ((AApDocumentPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApDocumentPayment";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_document_payment";
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }

        /// get character length for column
        public static short GetApNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }

        /// get character length for column
        public static short GetPaymentNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
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

    /// This table links the different payments to actual invoices and credit notes.
    [Serializable()]
    public class AApDocumentPaymentRow : System.Data.DataRow
    {
        private AApDocumentPaymentTable myTable;

        /// Constructor
        public AApDocumentPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApDocumentPaymentTable)(this.Table));
        }

        /// The ledger that the attribute value is associated with.
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

        /// Accounts Payable Sequence Number
        public Int32 ApNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApNumber)
                            || (((Int32)(this[this.myTable.ColumnApNumber])) != value)))
                {
                    this[this.myTable.ColumnApNumber] = value;
                }
            }
        }

        /// Unique number to identify each payment batch.
        public Int32 PaymentNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPaymentNumber)
                            || (((Int32)(this[this.myTable.ColumnPaymentNumber])) != value)))
                {
                    this[this.myTable.ColumnPaymentNumber] = value;
                }
            }
        }

        /// The amount of money that was paid
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
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnApNumber.Ordinal] = 0;
            this[this.myTable.ColumnPaymentNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAmount);
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
        public bool IsApNumberNull()
        {
            return this.IsNull(this.myTable.ColumnApNumber);
        }

        /// assign NULL value
        public void SetApNumberNull()
        {
            this.SetNull(this.myTable.ColumnApNumber);
        }

        /// test for NULL value
        public bool IsPaymentNumberNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentNumber);
        }

        /// assign NULL value
        public void SetPaymentNumberNull()
        {
            this.SetNull(this.myTable.ColumnPaymentNumber);
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

    /// This table acts as a queue for electronic payments. If an invoice is paid electronically, the payment is added to this table. A EP program will go through this table paying all entries to GL and moving them to the a_ap_payment table.
    [Serializable()]
    public class AEpPaymentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 181;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnUserIdId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnReferenceId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountId = 5;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AEpPayment", "a_ep_payment",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "PaymentNumber", "a_payment_number_i", "Payment Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "Amount", "a_amount_n", "Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(3, "UserId", "s_user_id_c", "User ID", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(5, "BankAccount", "a_bank_account_c", "Bank Account", OdbcType.VarChar, 16, true),
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
        public AEpPaymentTable() :
                base("AEpPayment")
        {
        }

        /// constructor
        public AEpPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AEpPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// Unique number to identify each payment batch.
        public DataColumn ColumnPaymentNumber;
        /// The amount of money that was paid
        public DataColumn ColumnAmount;
        /// This is the system user id of the person who made the payment.
        public DataColumn ColumnUserId;
        /// The source or reference for the accounts payable payment.  This could be a cheque number.
        public DataColumn ColumnReference;
        /// Bank account from which to make the payment
        public DataColumn ColumnBankAccount;
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
            this.Columns.Add(new System.Data.DataColumn("a_payment_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("s_user_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_account_c", typeof(String)));
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
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnUserId = this.Columns["s_user_id_c"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnBankAccount = this.Columns["a_bank_account_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AEpPaymentRow this[int i]
        {
            get
            {
                return ((AEpPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AEpPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AEpPaymentRow ret = ((AEpPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AEpPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AEpPaymentRow(builder);
        }

        /// get typed set of changes
        public AEpPaymentTable GetChangesTyped()
        {
            return ((AEpPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AEpPayment";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ep_payment";
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
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }

        /// get character length for column
        public static short GetPaymentNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
        {
            return 24;
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
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }

        /// get character length for column
        public static short GetReferenceLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountDBName()
        {
            return "a_bank_account_c";
        }

        /// get character length for column
        public static short GetBankAccountLength()
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

    /// This table acts as a queue for electronic payments. If an invoice is paid electronically, the payment is added to this table. A EP program will go through this table paying all entries to GL and moving them to the a_ap_payment table.
    [Serializable()]
    public class AEpPaymentRow : System.Data.DataRow
    {
        private AEpPaymentTable myTable;

        /// Constructor
        public AEpPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AEpPaymentTable)(this.Table));
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

        /// Unique number to identify each payment batch.
        public Int32 PaymentNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPaymentNumber)
                            || (((Int32)(this[this.myTable.ColumnPaymentNumber])) != value)))
                {
                    this[this.myTable.ColumnPaymentNumber] = value;
                }
            }
        }

        /// The amount of money that was paid
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

        /// This is the system user id of the person who made the payment.
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

        /// The source or reference for the accounts payable payment.  This could be a cheque number.
        public String Reference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// Bank account from which to make the payment
        public String BankAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccount)
                            || (((String)(this[this.myTable.ColumnBankAccount])) != value)))
                {
                    this[this.myTable.ColumnBankAccount] = value;
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
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnPaymentNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAmount);
            this.SetNull(this.myTable.ColumnUserId);
            this.SetNull(this.myTable.ColumnReference);
            this.SetNull(this.myTable.ColumnBankAccount);
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
        public bool IsPaymentNumberNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentNumber);
        }

        /// assign NULL value
        public void SetPaymentNumberNull()
        {
            this.SetNull(this.myTable.ColumnPaymentNumber);
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
        public bool IsBankAccountNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccount);
        }

        /// assign NULL value
        public void SetBankAccountNull()
        {
            this.SetNull(this.myTable.ColumnBankAccount);
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

    /// This table links the different EP payments to actual invoices and credit notes.
    [Serializable()]
    public class AEpDocumentPaymentTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 182;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 3;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AEpDocumentPayment", "a_ep_document_payment",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ApNumber", "a_ap_number_i", "AP Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "PaymentNumber", "a_payment_number_i", "Payment Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "Amount", "a_amount_n", "Amount", OdbcType.Decimal, 24, false),
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
        public AEpDocumentPaymentTable() :
                base("AEpDocumentPayment")
        {
        }

        /// constructor
        public AEpDocumentPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AEpDocumentPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// The ledger that the attribute value is associated with.
        public DataColumn ColumnLedgerNumber;
        /// Accounts Payable Sequence Number
        public DataColumn ColumnApNumber;
        /// Unique number to identify each payment batch.
        public DataColumn ColumnPaymentNumber;
        /// The amount of money that was paid
        public DataColumn ColumnAmount;
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
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_payment_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AEpDocumentPaymentRow this[int i]
        {
            get
            {
                return ((AEpDocumentPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AEpDocumentPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AEpDocumentPaymentRow ret = ((AEpDocumentPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AEpDocumentPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AEpDocumentPaymentRow(builder);
        }

        /// get typed set of changes
        public AEpDocumentPaymentTable GetChangesTyped()
        {
            return ((AEpDocumentPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AEpDocumentPayment";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ep_document_payment";
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }

        /// get character length for column
        public static short GetApNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }

        /// get character length for column
        public static short GetPaymentNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }

        /// get character length for column
        public static short GetAmountLength()
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

    /// This table links the different EP payments to actual invoices and credit notes.
    [Serializable()]
    public class AEpDocumentPaymentRow : System.Data.DataRow
    {
        private AEpDocumentPaymentTable myTable;

        /// Constructor
        public AEpDocumentPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AEpDocumentPaymentTable)(this.Table));
        }

        /// The ledger that the attribute value is associated with.
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

        /// Accounts Payable Sequence Number
        public Int32 ApNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApNumber)
                            || (((Int32)(this[this.myTable.ColumnApNumber])) != value)))
                {
                    this[this.myTable.ColumnApNumber] = value;
                }
            }
        }

        /// Unique number to identify each payment batch.
        public Int32 PaymentNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPaymentNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPaymentNumber)
                            || (((Int32)(this[this.myTable.ColumnPaymentNumber])) != value)))
                {
                    this[this.myTable.ColumnPaymentNumber] = value;
                }
            }
        }

        /// The amount of money that was paid
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
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnApNumber.Ordinal] = 0;
            this[this.myTable.ColumnPaymentNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAmount);
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
        public bool IsApNumberNull()
        {
            return this.IsNull(this.myTable.ColumnApNumber);
        }

        /// assign NULL value
        public void SetApNumberNull()
        {
            this.SetNull(this.myTable.ColumnApNumber);
        }

        /// test for NULL value
        public bool IsPaymentNumberNull()
        {
            return this.IsNull(this.myTable.ColumnPaymentNumber);
        }

        /// assign NULL value
        public void SetPaymentNumberNull()
        {
            this.SetNull(this.myTable.ColumnPaymentNumber);
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

    /// Analysis Attributes applied to an AP for posting to the GL.
    [Serializable()]
    public class AApAnalAttribTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 183;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDetailNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnalysisTypeCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnAccountCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnalysisAttributeValueId = 5;
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
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AApAnalAttrib", "a_ap_anal_attrib",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ApNumber", "a_ap_number_i", "AP Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "DetailNumber", "a_detail_number_i", "Detail Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "AnalysisTypeCode", "a_analysis_type_code_c", "Analysis Type Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(4, "AccountCode", "a_account_code_c", "Account Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "AnalysisAttributeValue", "a_analysis_attribute_value_c", "Analysis Attribute Value", OdbcType.VarChar, 80, false),
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
        public AApAnalAttribTable() :
                base("AApAnalAttrib")
        {
        }

        /// constructor
        public AApAnalAttribTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AApAnalAttribTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// The ledger that the attribute value is associated with.
        public DataColumn ColumnLedgerNumber;
        /// Accounts Payable Sequence Number
        public DataColumn ColumnApNumber;
        /// The detail number within the invoice/ accounts payable.
        public DataColumn ColumnDetailNumber;
        ///
        public DataColumn ColumnAnalysisTypeCode;
        /// This identifies the account the financial transaction must be stored against
        public DataColumn ColumnAccountCode;
        ///
        public DataColumn ColumnAnalysisAttributeValue;
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
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_analysis_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_analysis_attribute_value_c", typeof(String)));
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnAnalysisTypeCode = this.Columns["a_analysis_type_code_c"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnAnalysisAttributeValue = this.Columns["a_analysis_attribute_value_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
        }

        /// Access a typed row by index
        public AApAnalAttribRow this[int i]
        {
            get
            {
                return ((AApAnalAttribRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AApAnalAttribRow NewRowTyped(bool AWithDefaultValues)
        {
            AApAnalAttribRow ret = ((AApAnalAttribRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AApAnalAttribRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AApAnalAttribRow(builder);
        }

        /// get typed set of changes
        public AApAnalAttribTable GetChangesTyped()
        {
            return ((AApAnalAttribTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AApAnalAttrib";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "a_ap_anal_attrib";
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }

        /// get character length for column
        public static short GetApNumberLength()
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
        public static string GetAnalysisTypeCodeDBName()
        {
            return "a_analysis_type_code_c";
        }

        /// get character length for column
        public static short GetAnalysisTypeCodeLength()
        {
            return 16;
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
        public static string GetAnalysisAttributeValueDBName()
        {
            return "a_analysis_attribute_value_c";
        }

        /// get character length for column
        public static short GetAnalysisAttributeValueLength()
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

    /// Analysis Attributes applied to an AP for posting to the GL.
    [Serializable()]
    public class AApAnalAttribRow : System.Data.DataRow
    {
        private AApAnalAttribTable myTable;

        /// Constructor
        public AApAnalAttribRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AApAnalAttribTable)(this.Table));
        }

        /// The ledger that the attribute value is associated with.
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

        /// Accounts Payable Sequence Number
        public Int32 ApNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnApNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnApNumber)
                            || (((Int32)(this[this.myTable.ColumnApNumber])) != value)))
                {
                    this[this.myTable.ColumnApNumber] = value;
                }
            }
        }

        /// The detail number within the invoice/ accounts payable.
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

        ///
        public String AnalysisTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnalysisTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnalysisTypeCode)
                            || (((String)(this[this.myTable.ColumnAnalysisTypeCode])) != value)))
                {
                    this[this.myTable.ColumnAnalysisTypeCode] = value;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        ///
        public String AnalysisAttributeValue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnalysisAttributeValue.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnalysisAttributeValue)
                            || (((String)(this[this.myTable.ColumnAnalysisAttributeValue])) != value)))
                {
                    this[this.myTable.ColumnAnalysisAttributeValue] = value;
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
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnApNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAnalysisTypeCode);
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnAnalysisAttributeValue);
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
        public bool IsApNumberNull()
        {
            return this.IsNull(this.myTable.ColumnApNumber);
        }

        /// assign NULL value
        public void SetApNumberNull()
        {
            this.SetNull(this.myTable.ColumnApNumber);
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
        public bool IsAnalysisTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAnalysisTypeCode);
        }

        /// assign NULL value
        public void SetAnalysisTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnAnalysisTypeCode);
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
        public bool IsAnalysisAttributeValueNull()
        {
            return this.IsNull(this.myTable.ColumnAnalysisAttributeValue);
        }

        /// assign NULL value
        public void SetAnalysisAttributeValueNull()
        {
            this.SetNull(this.myTable.ColumnAnalysisAttributeValue);
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