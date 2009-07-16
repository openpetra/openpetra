/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
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
        
        /// Access a typed row by index
        public AApSupplierRow this[int i]
        {
            get
            {
                return ((AApSupplierRow)(this.Rows[i]));
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
            return "Reference to the partner key for this supplier";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPreferredScreenDisplayDBName()
        {
            return "a_preferred_screen_display_i";
        }
        
        /// get help text for column
        public static string GetPreferredScreenDisplayHelp()
        {
            return "Number of months to display invoices and credit notes";
        }
        
        /// get label of column
        public static string GetPreferredScreenDisplayLabel()
        {
            return "Preferred Screen Display";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultBankAccountDBName()
        {
            return "a_default_bank_account_c";
        }
        
        /// get help text for column
        public static string GetDefaultBankAccountHelp()
        {
            return "Reference to default bank account to use to pay supplier with.";
        }
        
        /// get label of column
        public static string GetDefaultBankAccountLabel()
        {
            return "Default Bank Account";
        }
        
        /// get character length for column
        public static short GetDefaultBankAccountLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPaymentTypeDBName()
        {
            return "a_payment_type_c";
        }
        
        /// get help text for column
        public static string GetPaymentTypeHelp()
        {
            return "The default type of payment to use when paying this supplier.";
        }
        
        /// get label of column
        public static string GetPaymentTypeLabel()
        {
            return "Default Payment Type";
        }
        
        /// get character length for column
        public static short GetPaymentTypeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "The currency code to use for this supplier.";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "Currency";
        }
        
        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultApAccountDBName()
        {
            return "a_default_ap_account_c";
        }
        
        /// get help text for column
        public static string GetDefaultApAccountHelp()
        {
            return "The default AP Account to use when paying this supplier.";
        }
        
        /// get label of column
        public static string GetDefaultApAccountLabel()
        {
            return "Default AP Account";
        }
        
        /// get character length for column
        public static short GetDefaultApAccountLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultCreditTermsDBName()
        {
            return "a_default_credit_terms_i";
        }
        
        /// get help text for column
        public static string GetDefaultCreditTermsHelp()
        {
            return "Default credit terms to use for invoices from this supplier.";
        }
        
        /// get label of column
        public static string GetDefaultCreditTermsLabel()
        {
            return "Default Credit Terms";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultDiscountPercentageDBName()
        {
            return "a_default_discount_percentage_n";
        }
        
        /// get help text for column
        public static string GetDefaultDiscountPercentageHelp()
        {
            return "Default percentage discount to receive for early payments.";
        }
        
        /// get label of column
        public static string GetDefaultDiscountPercentageLabel()
        {
            return "Default Discount Percentage";
        }
        
        /// get display format for column
        public static short GetDefaultDiscountPercentageLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultDiscountDaysDBName()
        {
            return "a_default_discount_days_i";
        }
        
        /// get help text for column
        public static string GetDefaultDiscountDaysHelp()
        {
            return "Default number of days in which the discount percentage has effect.";
        }
        
        /// get label of column
        public static string GetDefaultDiscountDaysLabel()
        {
            return "Default Discount Days";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSupplierTypeDBName()
        {
            return "a_supplier_type_c";
        }
        
        /// get help text for column
        public static string GetSupplierTypeHelp()
        {
            return "What type of supplier this is - normal, credit card, maybe something else.";
        }
        
        /// get label of column
        public static string GetSupplierTypeLabel()
        {
            return "Supplier Type";
        }
        
        /// get character length for column
        public static short GetSupplierTypeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultExpAccountDBName()
        {
            return "a_default_exp_account_c";
        }
        
        /// get help text for column
        public static string GetDefaultExpAccountHelp()
        {
            return "Reference to the default expense Account to use for invoice details.";
        }
        
        /// get label of column
        public static string GetDefaultExpAccountLabel()
        {
            return "Default Expense Account";
        }
        
        /// get character length for column
        public static short GetDefaultExpAccountLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDefaultCostCentreDBName()
        {
            return "a_default_cost_centre_c";
        }
        
        /// get help text for column
        public static string GetDefaultCostCentreHelp()
        {
            return "Reference to the default cost centre to use for invoice details.";
        }
        
        /// get label of column
        public static string GetDefaultCostCentreLabel()
        {
            return "Default Cost Centre";
        }
        
        /// get character length for column
        public static short GetDefaultCostCentreLength()
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
            return "AApSupplier";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_supplier";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "a_ap_supplier";
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
                    "p_partner_key_n",
                    "a_preferred_screen_display_i",
                    "a_default_bank_account_c",
                    "a_payment_type_c",
                    "a_currency_code_c",
                    "a_default_ap_account_c",
                    "a_default_credit_terms_i",
                    "a_default_discount_percentage_n",
                    "a_default_discount_days_i",
                    "a_supplier_type_c",
                    "a_default_exp_account_c",
                    "a_default_cost_centre_c",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey};
        }
        
        /// get typed set of changes
        public AApSupplierTable GetChangesTyped()
        {
            return ((AApSupplierTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPreferredScreenDisplay))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDefaultBankAccount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnPaymentType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDefaultApAccount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDefaultCreditTerms))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDefaultDiscountPercentage))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnDefaultDiscountDays))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnSupplierType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnDefaultExpAccount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDefaultCostCentre))
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
                    return String.Empty;
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
                    return String.Empty;
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
        
        /// The default AP Account to use when paying this supplier.
        public String DefaultApAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDefaultApAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
                    return String.Empty;
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
                    return String.Empty;
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
                    return String.Empty;
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
        
        /// Access a typed row by index
        public AApDocumentRow this[int i]
        {
            get
            {
                return ((AApDocumentRow)(this.Rows[i]));
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
            return "Reference to the ledger for this invoice.";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }
        
        /// get help text for column
        public static string GetApNumberHelp()
        {
            return "A unique key (together with the ledger number) to identify this document.";
        }
        
        /// get label of column
        public static string GetApNumberLabel()
        {
            return "AP Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Reference to the supplier that sent this invoice.";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Supplier Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreditNoteFlagDBName()
        {
            return "a_credit_note_flag_l";
        }
        
        /// get help text for column
        public static string GetCreditNoteFlagHelp()
        {
            return "A flag to indicate if this document is an invoice or a credit note.";
        }
        
        /// get label of column
        public static string GetCreditNoteFlagLabel()
        {
            return "Credit Note Flag";
        }
        
        /// get display format for column
        public static short GetCreditNoteFlagLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDocumentCodeDBName()
        {
            return "a_document_code_c";
        }
        
        /// get help text for column
        public static string GetDocumentCodeHelp()
        {
            return "The code given on the document itself (be it invoice or credit note). This will h" +
                "ave to be unique for each supplier.";
        }
        
        /// get label of column
        public static string GetDocumentCodeLabel()
        {
            return "Invoice Number";
        }
        
        /// get character length for column
        public static short GetDocumentCodeLength()
        {
            return 15;
        }
        
        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }
        
        /// get help text for column
        public static string GetReferenceHelp()
        {
            return "Some kind of other reference needed.";
        }
        
        /// get label of column
        public static string GetReferenceLabel()
        {
            return "Reference";
        }
        
        /// get character length for column
        public static short GetReferenceLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateIssuedDBName()
        {
            return "a_date_issued_d";
        }
        
        /// get help text for column
        public static string GetDateIssuedHelp()
        {
            return "The date when this document was issued.";
        }
        
        /// get label of column
        public static string GetDateIssuedLabel()
        {
            return "Date Issued";
        }
        
        /// get display format for column
        public static short GetDateIssuedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateEnteredDBName()
        {
            return "a_date_entered_d";
        }
        
        /// get help text for column
        public static string GetDateEnteredHelp()
        {
            return "The date when this document was entered into the system.";
        }
        
        /// get label of column
        public static string GetDateEnteredLabel()
        {
            return "Date Entered";
        }
        
        /// get display format for column
        public static short GetDateEnteredLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreditTermsDBName()
        {
            return "a_credit_terms_i";
        }
        
        /// get help text for column
        public static string GetCreditTermsHelp()
        {
            return "Credit terms allowed for this invoice.";
        }
        
        /// get label of column
        public static string GetCreditTermsLabel()
        {
            return "Credit Terms";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTotalAmountDBName()
        {
            return "a_total_amount_n";
        }
        
        /// get help text for column
        public static string GetTotalAmountHelp()
        {
            return "The total amount of money that this document is worth.";
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
        public static string GetExchangeRateToBaseDBName()
        {
            return "a_exchange_rate_to_base_n";
        }
        
        /// get help text for column
        public static string GetExchangeRateToBaseHelp()
        {
            return "The exchange rate to the base currency at the time that the document was issued.";
        }
        
        /// get label of column
        public static string GetExchangeRateToBaseLabel()
        {
            return "Exchange Rate to Base";
        }
        
        /// get display format for column
        public static short GetExchangeRateToBaseLength()
        {
            return 18;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDiscountPercentageDBName()
        {
            return "a_discount_percentage_n";
        }
        
        /// get help text for column
        public static string GetDiscountPercentageHelp()
        {
            return "The percentage discount you get for early payment of this document in the case th" +
                "at it is an invoice.";
        }
        
        /// get label of column
        public static string GetDiscountPercentageLabel()
        {
            return "Discount Percentage";
        }
        
        /// get display format for column
        public static short GetDiscountPercentageLength()
        {
            return 18;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDiscountDaysDBName()
        {
            return "a_discount_days_i";
        }
        
        /// get help text for column
        public static string GetDiscountDaysHelp()
        {
            return "The number of days that the discount is valid for (0 for none).";
        }
        
        /// get label of column
        public static string GetDiscountDaysLabel()
        {
            return "Discount Days";
        }
        
        /// get the name of the field in the database for this column
        public static string GetApAccountDBName()
        {
            return "a_ap_account_c";
        }
        
        /// get help text for column
        public static string GetApAccountHelp()
        {
            return "Reference to the AP Account to debit/credit when posting/paying the document.";
        }
        
        /// get label of column
        public static string GetApAccountLabel()
        {
            return "AP Account";
        }
        
        /// get character length for column
        public static short GetApAccountLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastDetailNumberDBName()
        {
            return "a_last_detail_number_i";
        }
        
        /// get help text for column
        public static string GetLastDetailNumberHelp()
        {
            return "The number of the last item for this document. This is used simply to quickly get" +
                " the next number if items are added.";
        }
        
        /// get label of column
        public static string GetLastDetailNumberLabel()
        {
            return "Last Detail Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDocumentStatusDBName()
        {
            return "a_document_status_c";
        }
        
        /// get help text for column
        public static string GetDocumentStatusHelp()
        {
            return "The current status of the invoice. The value can (for now) be one of: OPEN, APPRO" +
                "VED, POSTED, PARTPAID, or PAID.";
        }
        
        /// get label of column
        public static string GetDocumentStatusLabel()
        {
            return "Document Status";
        }
        
        /// get character length for column
        public static short GetDocumentStatusLength()
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
            return "AApDocument";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_document";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "a_ap_document";
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
                    "a_ap_number_i",
                    "p_partner_key_n",
                    "a_credit_note_flag_l",
                    "a_document_code_c",
                    "a_reference_c",
                    "a_date_issued_d",
                    "a_date_entered_d",
                    "a_credit_terms_i",
                    "a_total_amount_n",
                    "a_exchange_rate_to_base_n",
                    "a_discount_percentage_n",
                    "a_discount_days_i",
                    "a_ap_account_c",
                    "a_last_detail_number_i",
                    "a_document_status_c",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnApNumber};
        }
        
        /// get typed set of changes
        public AApDocumentTable GetChangesTyped()
        {
            return ((AApDocumentTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnCreditNoteFlag))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDocumentCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 30);
            }
            if ((ACol == ColumnReference))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnDateIssued))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateEntered))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreditTerms))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTotalAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnExchangeRateToBase))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnDiscountPercentage))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnDiscountDays))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApAccount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnLastDetailNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDocumentStatus))
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
                    return String.Empty;
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
        
        /// The date when this document was issued.
        public System.DateTime DateIssued
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateIssued.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateIssued) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateIssued])) != value)))
                {
                    this[this.myTable.ColumnDateIssued] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateIssuedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateIssued], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateIssuedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateIssued.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateEnteredLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateEntered], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateEnteredHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateEntered.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
                    return String.Empty;
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
                    return String.Empty;
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
            this.SetNull(this.myTable.ColumnLedgerNumber);
            this.SetNull(this.myTable.ColumnApNumber);
            this.SetNull(this.myTable.ColumnPartnerKey);
            this[this.myTable.ColumnCreditNoteFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnDocumentCode);
            this.SetNull(this.myTable.ColumnReference);
            this[this.myTable.ColumnDateIssued.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnDateEntered.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnCreditTerms.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTotalAmount);
            this.SetNull(this.myTable.ColumnExchangeRateToBase);
            this.SetNull(this.myTable.ColumnDiscountPercentage);
            this.SetNull(this.myTable.ColumnDiscountDays);
            this.SetNull(this.myTable.ColumnApAccount);
            this.SetNull(this.myTable.ColumnLastDetailNumber);
            this.SetNull(this.myTable.ColumnDocumentStatus);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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
        
        /// Access a typed row by index
        public ACrdtNoteInvoiceLinkRow this[int i]
        {
            get
            {
                return ((ACrdtNoteInvoiceLinkRow)(this.Rows[i]));
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
            return "Reference to the ledger number.";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreditNoteNumberDBName()
        {
            return "a_credit_note_number_i";
        }
        
        /// get help text for column
        public static string GetCreditNoteNumberHelp()
        {
            return "Reference to the credit note.";
        }
        
        /// get label of column
        public static string GetCreditNoteNumberLabel()
        {
            return "Credit Note Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetInvoiceNumberDBName()
        {
            return "a_invoice_number_i";
        }
        
        /// get help text for column
        public static string GetInvoiceNumberHelp()
        {
            return "Reference to the invoice.";
        }
        
        /// get label of column
        public static string GetInvoiceNumberLabel()
        {
            return "Invoice Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAppliedDateDBName()
        {
            return "a_applied_date_d";
        }
        
        /// get help text for column
        public static string GetAppliedDateHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetAppliedDateLabel()
        {
            return "Applied Date";
        }
        
        /// get display format for column
        public static short GetAppliedDateLength()
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
            return "ACrdtNoteInvoiceLink";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_crdt_note_invoice_link";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "a_crdt_note_invoice_link";
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
                    "a_credit_note_number_i",
                    "a_invoice_number_i",
                    "a_applied_date_d",
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
            this.ColumnCreditNoteNumber = this.Columns["a_credit_note_number_i"];
            this.ColumnInvoiceNumber = this.Columns["a_invoice_number_i"];
            this.ColumnAppliedDate = this.Columns["a_applied_date_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnCreditNoteNumber,
                    this.ColumnInvoiceNumber};
        }
        
        /// get typed set of changes
        public ACrdtNoteInvoiceLinkTable GetChangesTyped()
        {
            return ((ACrdtNoteInvoiceLinkTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnCreditNoteNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnInvoiceNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAppliedDate))
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime AppliedDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnAppliedDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime AppliedDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnAppliedDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
        
        /// Access a typed row by index
        public AApDocumentDetailRow this[int i]
        {
            get
            {
                return ((AApDocumentDetailRow)(this.Rows[i]));
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
            return "Reference to the ledger";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }
        
        /// get help text for column
        public static string GetApNumberHelp()
        {
            return "Reference to the document";
        }
        
        /// get label of column
        public static string GetApNumberLabel()
        {
            return "AP Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }
        
        /// get help text for column
        public static string GetDetailNumberHelp()
        {
            return "A unique number for this detail for its document.";
        }
        
        /// get label of column
        public static string GetDetailNumberLabel()
        {
            return "Detail Number";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDetailApprovedDBName()
        {
            return "a_detail_approved_l";
        }
        
        /// get help text for column
        public static string GetDetailApprovedHelp()
        {
            return "Indicates if this detail has been approved or not.";
        }
        
        /// get label of column
        public static string GetDetailApprovedLabel()
        {
            return "Approved Flag";
        }
        
        /// get display format for column
        public static short GetDetailApprovedLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCostCentreCodeDBName()
        {
            return "a_cost_centre_code_c";
        }
        
        /// get help text for column
        public static string GetCostCentreCodeHelp()
        {
            return "Reference to the cost centre to use for this detail.";
        }
        
        /// get label of column
        public static string GetCostCentreCodeLabel()
        {
            return "Cost Centre";
        }
        
        /// get character length for column
        public static short GetCostCentreCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAccountCodeDBName()
        {
            return "a_account_code_c";
        }
        
        /// get help text for column
        public static string GetAccountCodeHelp()
        {
            return "Reference to the account to use for this detail";
        }
        
        /// get label of column
        public static string GetAccountCodeLabel()
        {
            return "Account Code";
        }
        
        /// get character length for column
        public static short GetAccountCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemRefDBName()
        {
            return "a_item_ref_c";
        }
        
        /// get help text for column
        public static string GetItemRefHelp()
        {
            return "Some other reference to the item.";
        }
        
        /// get label of column
        public static string GetItemRefLabel()
        {
            return "Reference";
        }
        
        /// get character length for column
        public static short GetItemRefLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNarrativeDBName()
        {
            return "a_narrative_c";
        }
        
        /// get help text for column
        public static string GetNarrativeHelp()
        {
            return "A narrative about what this is.";
        }
        
        /// get label of column
        public static string GetNarrativeLabel()
        {
            return "Narrative";
        }
        
        /// get character length for column
        public static short GetNarrativeLength()
        {
            return 100;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }
        
        /// get help text for column
        public static string GetAmountHelp()
        {
            return "The amount of money this detail is worth.";
        }
        
        /// get label of column
        public static string GetAmountLabel()
        {
            return "Amount";
        }
        
        /// get display format for column
        public static short GetAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetApprovalDateDBName()
        {
            return "a_approval_date_d";
        }
        
        /// get help text for column
        public static string GetApprovalDateHelp()
        {
            return "The date when this detail was approved.";
        }
        
        /// get label of column
        public static string GetApprovalDateLabel()
        {
            return "Date Approved";
        }
        
        /// get display format for column
        public static short GetApprovalDateLength()
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
            return "AApDocumentDetail";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_document_detail";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "a_ap_document_detail";
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
                    "a_ap_number_i",
                    "a_detail_number_i",
                    "a_detail_approved_l",
                    "a_cost_centre_code_c",
                    "a_account_code_c",
                    "a_item_ref_c",
                    "a_narrative_c",
                    "a_amount_n",
                    "a_approval_date_d",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnApNumber,
                    this.ColumnDetailNumber};
        }
        
        /// get typed set of changes
        public AApDocumentDetailTable GetChangesTyped()
        {
            return ((AApDocumentDetailTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDetailNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDetailApproved))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnCostCentreCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnAccountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnItemRef))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnNarrative))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 200);
            }
            if ((ACol == ColumnAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnApprovalDate))
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
        
        /// Reference to the account to use for this detail
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
        
        /// Some other reference to the item.
        public String ItemRef
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemRef.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
                    return String.Empty;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ApprovalDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnApprovalDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ApprovalDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnApprovalDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
        
        /// Access a typed row by index
        public AApPaymentRow this[int i]
        {
            get
            {
                return ((AApPaymentRow)(this.Rows[i]));
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
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }
        
        /// get help text for column
        public static string GetPaymentNumberHelp()
        {
            return "Unique number to identify each payment batch.";
        }
        
        /// get label of column
        public static string GetPaymentNumberLabel()
        {
            return "Payment Number";
        }
        
        /// get display format for column
        public static short GetPaymentNumberLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }
        
        /// get help text for column
        public static string GetAmountHelp()
        {
            return "The amount of money that was paid";
        }
        
        /// get label of column
        public static string GetAmountLabel()
        {
            return "Amount";
        }
        
        /// get display format for column
        public static short GetAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExchangeRateToBaseDBName()
        {
            return "a_exchange_rate_to_base_n";
        }
        
        /// get help text for column
        public static string GetExchangeRateToBaseHelp()
        {
            return "The exchange rate to the base currency at the time of payment.";
        }
        
        /// get label of column
        public static string GetExchangeRateToBaseLabel()
        {
            return "Exchange Rate To Base";
        }
        
        /// get display format for column
        public static short GetExchangeRateToBaseLength()
        {
            return 18;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPaymentDateDBName()
        {
            return "a_payment_date_d";
        }
        
        /// get help text for column
        public static string GetPaymentDateHelp()
        {
            return "Enter the date of payment";
        }
        
        /// get label of column
        public static string GetPaymentDateLabel()
        {
            return "Payment Date";
        }
        
        /// get display format for column
        public static short GetPaymentDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }
        
        /// get help text for column
        public static string GetUserIdHelp()
        {
            return "This is the system user id of the person who made the payment.";
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
        public static string GetMethodOfPaymentDBName()
        {
            return "a_method_of_payment_c";
        }
        
        /// get help text for column
        public static string GetMethodOfPaymentHelp()
        {
            return "Method that was used to make the payment - cheque, cash, ep, credit card, etc.";
        }
        
        /// get label of column
        public static string GetMethodOfPaymentLabel()
        {
            return "Method Of Payment";
        }
        
        /// get character length for column
        public static short GetMethodOfPaymentLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }
        
        /// get help text for column
        public static string GetReferenceHelp()
        {
            return "Enter the reference for this accounts payable payment";
        }
        
        /// get label of column
        public static string GetReferenceLabel()
        {
            return "Reference";
        }
        
        /// get character length for column
        public static short GetReferenceLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBankAccountDBName()
        {
            return "a_bank_account_c";
        }
        
        /// get help text for column
        public static string GetBankAccountHelp()
        {
            return "Choose the Bank account to make the payment from";
        }
        
        /// get label of column
        public static string GetBankAccountLabel()
        {
            return "Bank Account";
        }
        
        /// get character length for column
        public static short GetBankAccountLength()
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
            return "AApPayment";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_payment";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "AP Payment";
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
                    "a_payment_number_i",
                    "a_amount_n",
                    "a_exchange_rate_to_base_n",
                    "a_payment_date_d",
                    "s_user_id_c",
                    "a_method_of_payment_c",
                    "a_reference_c",
                    "a_bank_account_c",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnPaymentNumber};
        }
        
        /// get typed set of changes
        public AApPaymentTable GetChangesTyped()
        {
            return ((AApPaymentTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPaymentNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnExchangeRateToBase))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnPaymentDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnUserId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnMethodOfPayment))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnReference))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnBankAccount))
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime PaymentDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnPaymentDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime PaymentDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnPaymentDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
        
        /// Method that was used to make the payment - cheque, cash, ep, credit card, etc.
        public String MethodOfPayment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPayment.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
        
        /// Bank account from which to make the payment
        public String BankAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
        
        /// Access a typed row by index
        public AApDocumentPaymentRow this[int i]
        {
            get
            {
                return ((AApDocumentPaymentRow)(this.Rows[i]));
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }
        
        /// get help text for column
        public static string GetApNumberHelp()
        {
            return "Accounts Payable Sequence Number";
        }
        
        /// get label of column
        public static string GetApNumberLabel()
        {
            return "AP Number";
        }
        
        /// get display format for column
        public static short GetApNumberLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }
        
        /// get help text for column
        public static string GetPaymentNumberHelp()
        {
            return "Unique number to identify each payment batch.";
        }
        
        /// get label of column
        public static string GetPaymentNumberLabel()
        {
            return "Payment Number";
        }
        
        /// get display format for column
        public static short GetPaymentNumberLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }
        
        /// get help text for column
        public static string GetAmountHelp()
        {
            return "The amount of money that was paid";
        }
        
        /// get label of column
        public static string GetAmountLabel()
        {
            return "Amount";
        }
        
        /// get display format for column
        public static short GetAmountLength()
        {
            return 19;
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
            return "AApDocumentPayment";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_document_payment";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "AP Document Payment";
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
                    "a_ap_number_i",
                    "a_payment_number_i",
                    "a_amount_n",
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnApNumber,
                    this.ColumnPaymentNumber};
        }
        
        /// get typed set of changes
        public AApDocumentPaymentTable GetChangesTyped()
        {
            return ((AApDocumentPaymentTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPaymentNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
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
        
        /// Access a typed row by index
        public AEpPaymentRow this[int i]
        {
            get
            {
                return ((AEpPaymentRow)(this.Rows[i]));
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
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }
        
        /// get help text for column
        public static string GetPaymentNumberHelp()
        {
            return "Unique number to identify each payment batch.";
        }
        
        /// get label of column
        public static string GetPaymentNumberLabel()
        {
            return "Payment Number";
        }
        
        /// get display format for column
        public static short GetPaymentNumberLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }
        
        /// get help text for column
        public static string GetAmountHelp()
        {
            return "The amount of money that was paid";
        }
        
        /// get label of column
        public static string GetAmountLabel()
        {
            return "Amount";
        }
        
        /// get display format for column
        public static short GetAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }
        
        /// get help text for column
        public static string GetUserIdHelp()
        {
            return "This is the system user id of the person who made the payment.";
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
        public static string GetReferenceDBName()
        {
            return "a_reference_c";
        }
        
        /// get help text for column
        public static string GetReferenceHelp()
        {
            return "Enter the reference for this accounts payable payment";
        }
        
        /// get label of column
        public static string GetReferenceLabel()
        {
            return "Reference";
        }
        
        /// get character length for column
        public static short GetReferenceLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBankAccountDBName()
        {
            return "a_bank_account_c";
        }
        
        /// get help text for column
        public static string GetBankAccountHelp()
        {
            return "Choose the Bank account to make the payment from";
        }
        
        /// get label of column
        public static string GetBankAccountLabel()
        {
            return "Bank Account";
        }
        
        /// get character length for column
        public static short GetBankAccountLength()
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
            return "AEpPayment";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ep_payment";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Electronic Payment";
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
                    "a_payment_number_i",
                    "a_amount_n",
                    "s_user_id_c",
                    "a_reference_c",
                    "a_bank_account_c",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnPaymentNumber};
        }
        
        /// get typed set of changes
        public AEpPaymentTable GetChangesTyped()
        {
            return ((AEpPaymentTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPaymentNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnUserId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnReference))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnBankAccount))
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
        
        /// The source or reference for the accounts payable payment.  This could be a cheque number.
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
        
        /// Bank account from which to make the payment
        public String BankAccount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
        
        /// Access a typed row by index
        public AEpDocumentPaymentRow this[int i]
        {
            get
            {
                return ((AEpDocumentPaymentRow)(this.Rows[i]));
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }
        
        /// get help text for column
        public static string GetApNumberHelp()
        {
            return "Accounts Payable Sequence Number";
        }
        
        /// get label of column
        public static string GetApNumberLabel()
        {
            return "AP Number";
        }
        
        /// get display format for column
        public static short GetApNumberLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPaymentNumberDBName()
        {
            return "a_payment_number_i";
        }
        
        /// get help text for column
        public static string GetPaymentNumberHelp()
        {
            return "Unique number to identify each payment batch.";
        }
        
        /// get label of column
        public static string GetPaymentNumberLabel()
        {
            return "Payment Number";
        }
        
        /// get display format for column
        public static short GetPaymentNumberLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "a_amount_n";
        }
        
        /// get help text for column
        public static string GetAmountHelp()
        {
            return "The amount of money that was paid";
        }
        
        /// get label of column
        public static string GetAmountLabel()
        {
            return "Amount";
        }
        
        /// get display format for column
        public static short GetAmountLength()
        {
            return 19;
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
            return "AEpDocumentPayment";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ep_document_payment";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Electronic Document Payment";
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
                    "a_ap_number_i",
                    "a_payment_number_i",
                    "a_amount_n",
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
            this.ColumnApNumber = this.Columns["a_ap_number_i"];
            this.ColumnPaymentNumber = this.Columns["a_payment_number_i"];
            this.ColumnAmount = this.Columns["a_amount_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnApNumber,
                    this.ColumnPaymentNumber};
        }
        
        /// get typed set of changes
        public AEpDocumentPaymentTable GetChangesTyped()
        {
            return ((AEpDocumentPaymentTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPaymentNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
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
        
        /// Access a typed row by index
        public AApAnalAttribRow this[int i]
        {
            get
            {
                return ((AApAnalAttribRow)(this.Rows[i]));
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
        public static string GetApNumberDBName()
        {
            return "a_ap_number_i";
        }
        
        /// get help text for column
        public static string GetApNumberHelp()
        {
            return "Accounts Payable Sequence Number";
        }
        
        /// get label of column
        public static string GetApNumberLabel()
        {
            return "AP Number";
        }
        
        /// get display format for column
        public static short GetApNumberLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }
        
        /// get help text for column
        public static string GetDetailNumberHelp()
        {
            return "The detail number within the invoice/ accounts payable.";
        }
        
        /// get label of column
        public static string GetDetailNumberLabel()
        {
            return "Detail Number";
        }
        
        /// get display format for column
        public static short GetDetailNumberLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnalysisTypeCodeDBName()
        {
            return "a_analysis_type_code_c";
        }
        
        /// get help text for column
        public static string GetAnalysisTypeCodeHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetAnalysisTypeCodeLabel()
        {
            return "Analysis Type Code";
        }
        
        /// get character length for column
        public static short GetAnalysisTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAccountCodeDBName()
        {
            return "a_account_code_c";
        }
        
        /// get help text for column
        public static string GetAccountCodeHelp()
        {
            return "Enter an account code";
        }
        
        /// get label of column
        public static string GetAccountCodeLabel()
        {
            return "Account Code";
        }
        
        /// get character length for column
        public static short GetAccountCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnalysisAttributeValueDBName()
        {
            return "a_analysis_attribute_value_c";
        }
        
        /// get help text for column
        public static string GetAnalysisAttributeValueHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetAnalysisAttributeValueLabel()
        {
            return "Analysis Attribute Value";
        }
        
        /// get character length for column
        public static short GetAnalysisAttributeValueLength()
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
            return "AApAnalAttrib";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ap_anal_attrib";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "AP Analysis Attributes";
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
                    "a_ap_number_i",
                    "a_detail_number_i",
                    "a_analysis_type_code_c",
                    "a_account_code_c",
                    "a_analysis_attribute_value_c",
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
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnApNumber,
                    this.ColumnDetailNumber,
                    this.ColumnAnalysisTypeCode};
        }
        
        /// get typed set of changes
        public AApAnalAttribTable GetChangesTyped()
        {
            return ((AApAnalAttribTable)(base.GetChangesTypedInternal()));
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
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnApNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDetailNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnalysisTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnAccountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnAnalysisAttributeValue))
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
                    return String.Empty;
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
        
        /// 
        public String AnalysisAttributeValue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnalysisAttributeValue.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
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
