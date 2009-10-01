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
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MFinance.Account.Data;

     /// auto generated
    [Serializable()]
    public class AccountsPayableTDS : TTypedDataSet
    {

        private AApSupplierTable TableAApSupplier;
        private AccountsPayableTDSAApDocumentTable TableAApDocument;
        private AApDocumentDetailTable TableAApDocumentDetail;
        private AApDocumentPaymentTable TableAApDocumentPayment;
        private AApPaymentTable TableAApPayment;
        private AApAnalAttribTable TableAApAnalAttrib;
        private AccountsPayableTDSSupplierPaymentsTable TableSupplierPayments;
        private AccountsPayableTDSPaymentDetailsTable TablePaymentDetails;

        /// auto generated
        public AccountsPayableTDS() :
                base("AccountsPayableTDS")
        {
        }

        /// auto generated for serialization
        public AccountsPayableTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public AccountsPayableTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public AApSupplierTable AApSupplier
        {
            get
            {
                return this.TableAApSupplier;
            }
        }

        /// auto generated
        public AccountsPayableTDSAApDocumentTable AApDocument
        {
            get
            {
                return this.TableAApDocument;
            }
        }

        /// auto generated
        public AApDocumentDetailTable AApDocumentDetail
        {
            get
            {
                return this.TableAApDocumentDetail;
            }
        }

        /// auto generated
        public AApDocumentPaymentTable AApDocumentPayment
        {
            get
            {
                return this.TableAApDocumentPayment;
            }
        }

        /// auto generated
        public AApPaymentTable AApPayment
        {
            get
            {
                return this.TableAApPayment;
            }
        }

        /// auto generated
        public AApAnalAttribTable AApAnalAttrib
        {
            get
            {
                return this.TableAApAnalAttrib;
            }
        }

        /// auto generated
        public AccountsPayableTDSSupplierPaymentsTable SupplierPayments
        {
            get
            {
                return this.TableSupplierPayments;
            }
        }

        /// auto generated
        public AccountsPayableTDSPaymentDetailsTable PaymentDetails
        {
            get
            {
                return this.TablePaymentDetails;
            }
        }

        /// auto generated
        public new virtual AccountsPayableTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((AccountsPayableTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new AApSupplierTable("AApSupplier"));
            this.Tables.Add(new AccountsPayableTDSAApDocumentTable("AApDocument"));
            this.Tables.Add(new AApDocumentDetailTable("AApDocumentDetail"));
            this.Tables.Add(new AApDocumentPaymentTable("AApDocumentPayment"));
            this.Tables.Add(new AApPaymentTable("AApPayment"));
            this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
            this.Tables.Add(new AccountsPayableTDSSupplierPaymentsTable("SupplierPayments"));
            this.Tables.Add(new AccountsPayableTDSPaymentDetailsTable("PaymentDetails"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AApSupplier") != -1))
            {
                this.Tables.Add(new AApSupplierTable("AApSupplier"));
            }
            if ((ds.Tables.IndexOf("AApDocument") != -1))
            {
                this.Tables.Add(new AccountsPayableTDSAApDocumentTable("AApDocument"));
            }
            if ((ds.Tables.IndexOf("AApDocumentDetail") != -1))
            {
                this.Tables.Add(new AApDocumentDetailTable("AApDocumentDetail"));
            }
            if ((ds.Tables.IndexOf("AApDocumentPayment") != -1))
            {
                this.Tables.Add(new AApDocumentPaymentTable("AApDocumentPayment"));
            }
            if ((ds.Tables.IndexOf("AApPayment") != -1))
            {
                this.Tables.Add(new AApPaymentTable("AApPayment"));
            }
            if ((ds.Tables.IndexOf("AApAnalAttrib") != -1))
            {
                this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
            }
            if ((ds.Tables.IndexOf("SupplierPayments") != -1))
            {
                this.Tables.Add(new AccountsPayableTDSSupplierPaymentsTable("SupplierPayments"));
            }
            if ((ds.Tables.IndexOf("PaymentDetails") != -1))
            {
                this.Tables.Add(new AccountsPayableTDSPaymentDetailsTable("PaymentDetails"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableAApSupplier != null))
            {
                this.TableAApSupplier.InitVars();
            }
            if ((this.TableAApDocument != null))
            {
                this.TableAApDocument.InitVars();
            }
            if ((this.TableAApDocumentDetail != null))
            {
                this.TableAApDocumentDetail.InitVars();
            }
            if ((this.TableAApDocumentPayment != null))
            {
                this.TableAApDocumentPayment.InitVars();
            }
            if ((this.TableAApPayment != null))
            {
                this.TableAApPayment.InitVars();
            }
            if ((this.TableAApAnalAttrib != null))
            {
                this.TableAApAnalAttrib.InitVars();
            }
            if ((this.TableSupplierPayments != null))
            {
                this.TableSupplierPayments.InitVars();
            }
            if ((this.TablePaymentDetails != null))
            {
                this.TablePaymentDetails.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "AccountsPayableTDS";
            this.TableAApSupplier = ((AApSupplierTable)(this.Tables["AApSupplier"]));
            this.TableAApDocument = ((AccountsPayableTDSAApDocumentTable)(this.Tables["AApDocument"]));
            this.TableAApDocumentDetail = ((AApDocumentDetailTable)(this.Tables["AApDocumentDetail"]));
            this.TableAApDocumentPayment = ((AApDocumentPaymentTable)(this.Tables["AApDocumentPayment"]));
            this.TableAApPayment = ((AApPaymentTable)(this.Tables["AApPayment"]));
            this.TableAApAnalAttrib = ((AApAnalAttribTable)(this.Tables["AApAnalAttrib"]));
            this.TableSupplierPayments = ((AccountsPayableTDSSupplierPaymentsTable)(this.Tables["SupplierPayments"]));
            this.TablePaymentDetails = ((AccountsPayableTDSPaymentDetailsTable)(this.Tables["PaymentDetails"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableAApDocumentDetail != null)
                        && (this.TableAApAnalAttrib != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApAnalAttrib1", "AApDocumentDetail", new string[] {
                                "a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"}, "AApAnalAttrib", new string[] {
                                "a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"}));
            }
            if (((this.TableAApSupplier != null)
                        && (this.TableAApDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocument2", "AApSupplier", new string[] {
                                "p_partner_key_n"}, "AApDocument", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TableAApDocument != null)
                        && (this.TableAApDocumentDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentDetail2", "AApDocument", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}, "AApDocumentDetail", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}));
            }
            if (((this.TableAApDocument != null)
                        && (this.TableAApDocumentPayment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentPayment2", "AApDocument", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}, "AApDocumentPayment", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}));
            }
            if (((this.TableAApPayment != null)
                        && (this.TableAApDocumentPayment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentPayment3", "AApPayment", new string[] {
                                "a_ledger_number_i", "a_payment_number_i"}, "AApDocumentPayment", new string[] {
                                "a_ledger_number_i", "a_payment_number_i"}));
            }
        }
    }

    /// This is either an invoice or a credit note in the Accounts Payable system.
    [Serializable()]
    public class AccountsPayableTDSAApDocumentTable : AApDocumentTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 5400;
        /// used for generic TTypedDataTable functions
        public static short ColumnTaggedId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateDueId = 22;

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
                    new TTypedColumnInfo(20, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false),
                    new TTypedColumnInfo(21, "Tagged", "Tagged", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(22, "DateDue", "DateDue", "", OdbcType.Int, -1, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

        /// constructor
        public AccountsPayableTDSAApDocumentTable() :
                base("AApDocument")
        {
        }

        /// constructor
        public AccountsPayableTDSAApDocumentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AccountsPayableTDSAApDocumentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnTagged;
        ///
        public DataColumn ColumnDateDue;

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
            this.Columns.Add(new System.Data.DataColumn("Tagged", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("DateDue", typeof(DateTime)));
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
            this.ColumnTagged = this.Columns["Tagged"];
            this.ColumnDateDue = this.Columns["DateDue"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnApNumber};
        }

        /// Access a typed row by index
        public new AccountsPayableTDSAApDocumentRow this[int i]
        {
            get
            {
                return ((AccountsPayableTDSAApDocumentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new AccountsPayableTDSAApDocumentRow NewRowTyped(bool AWithDefaultValues)
        {
            AccountsPayableTDSAApDocumentRow ret = ((AccountsPayableTDSAApDocumentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new AccountsPayableTDSAApDocumentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AccountsPayableTDSAApDocumentRow(builder);
        }

        /// get typed set of changes
        public new AccountsPayableTDSAApDocumentTable GetChangesTyped()
        {
            return ((AccountsPayableTDSAApDocumentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AApDocument";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_ap_document";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetTaggedDBName()
        {
            return "Tagged";
        }

        /// get character length for column
        public static short GetTaggedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateDueDBName()
        {
            return "DateDue";
        }

        /// get character length for column
        public static short GetDateDueLength()
        {
            return -1;
        }

    }

    /// This is either an invoice or a credit note in the Accounts Payable system.
    [Serializable()]
    public class AccountsPayableTDSAApDocumentRow : AApDocumentRow
    {
        private AccountsPayableTDSAApDocumentTable myTable;

        /// Constructor
        public AccountsPayableTDSAApDocumentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AccountsPayableTDSAApDocumentTable)(this.Table));
        }

        ///
        public Boolean Tagged
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTagged.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTagged)
                            || (((Boolean)(this[this.myTable.ColumnTagged])) != value)))
                {
                    this[this.myTable.ColumnTagged] = value;
                }
            }
        }

        ///
        public DateTime DateDue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateDue.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateDue)
                            || (((DateTime)(this[this.myTable.ColumnDateDue])) != value)))
                {
                    this[this.myTable.ColumnDateDue] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
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
            this.SetNull(this.myTable.ColumnTagged);
            this.SetNull(this.myTable.ColumnDateDue);
        }

        /// test for NULL value
        public bool IsTaggedNull()
        {
            return this.IsNull(this.myTable.ColumnTagged);
        }

        /// assign NULL value
        public void SetTaggedNull()
        {
            this.SetNull(this.myTable.ColumnTagged);
        }

        /// test for NULL value
        public bool IsDateDueNull()
        {
            return this.IsNull(this.myTable.ColumnDateDue);
        }

        /// assign NULL value
        public void SetDateDueNull()
        {
            this.SetNull(this.myTable.ColumnDateDue);
        }
    }

    ///
    [Serializable()]
    public class AccountsPayableTDSSupplierPaymentsTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5401;
        /// used for generic TTypedDataTable functions
        public static short ColumnIdId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnSupplierKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnSupplierNameId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPaymentTypeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDocumentNumberCSVId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnListLabelId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "SupplierPayments", "AccountsPayableTDSSupplierPayments",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Id", "Id", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(1, "SupplierKey", "SupplierKey", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "SupplierName", "SupplierName", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "BankAccount", "BankAccount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "PaymentType", "PaymentType", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "DocumentNumberCSV", "DocumentNumberCSV", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "ListLabel", "ListLabel", "", OdbcType.Int, -1, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public AccountsPayableTDSSupplierPaymentsTable() :
                base("SupplierPayments")
        {
        }

        /// constructor
        public AccountsPayableTDSSupplierPaymentsTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AccountsPayableTDSSupplierPaymentsTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnId;
        ///
        public DataColumn ColumnSupplierKey;
        ///
        public DataColumn ColumnSupplierName;
        ///
        public DataColumn ColumnBankAccount;
        ///
        public DataColumn ColumnPaymentType;
        ///
        public DataColumn ColumnDocumentNumberCSV;
        ///
        public DataColumn ColumnListLabel;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("Id", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("SupplierKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("SupplierName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("BankAccount", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PaymentType", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("DocumentNumberCSV", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("ListLabel", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnId = this.Columns["Id"];
            this.ColumnSupplierKey = this.Columns["SupplierKey"];
            this.ColumnSupplierName = this.Columns["SupplierName"];
            this.ColumnBankAccount = this.Columns["BankAccount"];
            this.ColumnPaymentType = this.Columns["PaymentType"];
            this.ColumnDocumentNumberCSV = this.Columns["DocumentNumberCSV"];
            this.ColumnListLabel = this.Columns["ListLabel"];
        }

        /// Access a typed row by index
        public AccountsPayableTDSSupplierPaymentsRow this[int i]
        {
            get
            {
                return ((AccountsPayableTDSSupplierPaymentsRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AccountsPayableTDSSupplierPaymentsRow NewRowTyped(bool AWithDefaultValues)
        {
            AccountsPayableTDSSupplierPaymentsRow ret = ((AccountsPayableTDSSupplierPaymentsRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AccountsPayableTDSSupplierPaymentsRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AccountsPayableTDSSupplierPaymentsRow(builder);
        }

        /// get typed set of changes
        public AccountsPayableTDSSupplierPaymentsTable GetChangesTyped()
        {
            return ((AccountsPayableTDSSupplierPaymentsTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "SupplierPayments";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "AccountsPayableTDSSupplierPayments";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetIdDBName()
        {
            return "Id";
        }

        /// get character length for column
        public static short GetIdLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSupplierKeyDBName()
        {
            return "SupplierKey";
        }

        /// get character length for column
        public static short GetSupplierKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSupplierNameDBName()
        {
            return "SupplierName";
        }

        /// get character length for column
        public static short GetSupplierNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountDBName()
        {
            return "BankAccount";
        }

        /// get character length for column
        public static short GetBankAccountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPaymentTypeDBName()
        {
            return "PaymentType";
        }

        /// get character length for column
        public static short GetPaymentTypeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDocumentNumberCSVDBName()
        {
            return "DocumentNumberCSV";
        }

        /// get character length for column
        public static short GetDocumentNumberCSVLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetListLabelDBName()
        {
            return "ListLabel";
        }

        /// get character length for column
        public static short GetListLabelLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class AccountsPayableTDSSupplierPaymentsRow : System.Data.DataRow
    {
        private AccountsPayableTDSSupplierPaymentsTable myTable;

        /// Constructor
        public AccountsPayableTDSSupplierPaymentsRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AccountsPayableTDSSupplierPaymentsTable)(this.Table));
        }

        ///
        public Int32 Id
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnId)
                            || (((Int32)(this[this.myTable.ColumnId])) != value)))
                {
                    this[this.myTable.ColumnId] = value;
                }
            }
        }

        ///
        public Int64 SupplierKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSupplierKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSupplierKey)
                            || (((Int64)(this[this.myTable.ColumnSupplierKey])) != value)))
                {
                    this[this.myTable.ColumnSupplierKey] = value;
                }
            }
        }

        ///
        public string SupplierName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSupplierName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSupplierName)
                            || (((string)(this[this.myTable.ColumnSupplierName])) != value)))
                {
                    this[this.myTable.ColumnSupplierName] = value;
                }
            }
        }

        ///
        public string BankAccount
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccount)
                            || (((string)(this[this.myTable.ColumnBankAccount])) != value)))
                {
                    this[this.myTable.ColumnBankAccount] = value;
                }
            }
        }

        ///
        public string PaymentType
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPaymentType)
                            || (((string)(this[this.myTable.ColumnPaymentType])) != value)))
                {
                    this[this.myTable.ColumnPaymentType] = value;
                }
            }
        }

        ///
        public string DocumentNumberCSV
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDocumentNumberCSV.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDocumentNumberCSV)
                            || (((string)(this[this.myTable.ColumnDocumentNumberCSV])) != value)))
                {
                    this[this.myTable.ColumnDocumentNumberCSV] = value;
                }
            }
        }

        ///
        public string ListLabel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnListLabel.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnListLabel)
                            || (((string)(this[this.myTable.ColumnListLabel])) != value)))
                {
                    this[this.myTable.ColumnListLabel] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnId);
            this.SetNull(this.myTable.ColumnSupplierKey);
            this.SetNull(this.myTable.ColumnSupplierName);
            this.SetNull(this.myTable.ColumnBankAccount);
            this.SetNull(this.myTable.ColumnPaymentType);
            this.SetNull(this.myTable.ColumnDocumentNumberCSV);
            this.SetNull(this.myTable.ColumnListLabel);
        }

        /// test for NULL value
        public bool IsIdNull()
        {
            return this.IsNull(this.myTable.ColumnId);
        }

        /// assign NULL value
        public void SetIdNull()
        {
            this.SetNull(this.myTable.ColumnId);
        }

        /// test for NULL value
        public bool IsSupplierKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSupplierKey);
        }

        /// assign NULL value
        public void SetSupplierKeyNull()
        {
            this.SetNull(this.myTable.ColumnSupplierKey);
        }

        /// test for NULL value
        public bool IsSupplierNameNull()
        {
            return this.IsNull(this.myTable.ColumnSupplierName);
        }

        /// assign NULL value
        public void SetSupplierNameNull()
        {
            this.SetNull(this.myTable.ColumnSupplierName);
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
        public bool IsDocumentNumberCSVNull()
        {
            return this.IsNull(this.myTable.ColumnDocumentNumberCSV);
        }

        /// assign NULL value
        public void SetDocumentNumberCSVNull()
        {
            this.SetNull(this.myTable.ColumnDocumentNumberCSV);
        }

        /// test for NULL value
        public bool IsListLabelNull()
        {
            return this.IsNull(this.myTable.ColumnListLabel);
        }

        /// assign NULL value
        public void SetListLabelNull()
        {
            this.SetNull(this.myTable.ColumnListLabel);
        }
    }

    ///
    [Serializable()]
    public class AccountsPayableTDSPaymentDetailsTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5402;
        /// used for generic TTypedDataTable functions
        public static short ColumnApNumberId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnAmountId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnTotalAmountToPayId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnUseDiscountId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPayFullInvoiceId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnHasValidDiscountId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDiscountPercentageId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PaymentDetails", "AccountsPayableTDSPaymentDetails",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ApNumber", "ApNumber", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(1, "Amount", "Amount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "TotalAmountToPay", "TotalAmountToPay", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "UseDiscount", "UseDiscount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "PayFullInvoice", "PayFullInvoice", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "HasValidDiscount", "HasValidDiscount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "DiscountPercentage", "DiscountPercentage", "", OdbcType.Int, -1, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public AccountsPayableTDSPaymentDetailsTable() :
                base("PaymentDetails")
        {
        }

        /// constructor
        public AccountsPayableTDSPaymentDetailsTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AccountsPayableTDSPaymentDetailsTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnApNumber;
        ///
        public DataColumn ColumnAmount;
        ///
        public DataColumn ColumnTotalAmountToPay;
        ///
        public DataColumn ColumnUseDiscount;
        ///
        public DataColumn ColumnPayFullInvoice;
        ///
        public DataColumn ColumnHasValidDiscount;
        ///
        public DataColumn ColumnDiscountPercentage;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("ApNumber", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("Amount", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("TotalAmountToPay", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("UseDiscount", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("PayFullInvoice", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("HasValidDiscount", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("DiscountPercentage", typeof(Double)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnApNumber = this.Columns["ApNumber"];
            this.ColumnAmount = this.Columns["Amount"];
            this.ColumnTotalAmountToPay = this.Columns["TotalAmountToPay"];
            this.ColumnUseDiscount = this.Columns["UseDiscount"];
            this.ColumnPayFullInvoice = this.Columns["PayFullInvoice"];
            this.ColumnHasValidDiscount = this.Columns["HasValidDiscount"];
            this.ColumnDiscountPercentage = this.Columns["DiscountPercentage"];
        }

        /// Access a typed row by index
        public AccountsPayableTDSPaymentDetailsRow this[int i]
        {
            get
            {
                return ((AccountsPayableTDSPaymentDetailsRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public AccountsPayableTDSPaymentDetailsRow NewRowTyped(bool AWithDefaultValues)
        {
            AccountsPayableTDSPaymentDetailsRow ret = ((AccountsPayableTDSPaymentDetailsRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public AccountsPayableTDSPaymentDetailsRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AccountsPayableTDSPaymentDetailsRow(builder);
        }

        /// get typed set of changes
        public AccountsPayableTDSPaymentDetailsTable GetChangesTyped()
        {
            return ((AccountsPayableTDSPaymentDetailsTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PaymentDetails";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "AccountsPayableTDSPaymentDetails";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetApNumberDBName()
        {
            return "ApNumber";
        }

        /// get character length for column
        public static short GetApNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountDBName()
        {
            return "Amount";
        }

        /// get character length for column
        public static short GetAmountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTotalAmountToPayDBName()
        {
            return "TotalAmountToPay";
        }

        /// get character length for column
        public static short GetTotalAmountToPayLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUseDiscountDBName()
        {
            return "UseDiscount";
        }

        /// get character length for column
        public static short GetUseDiscountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPayFullInvoiceDBName()
        {
            return "PayFullInvoice";
        }

        /// get character length for column
        public static short GetPayFullInvoiceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHasValidDiscountDBName()
        {
            return "HasValidDiscount";
        }

        /// get character length for column
        public static short GetHasValidDiscountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDiscountPercentageDBName()
        {
            return "DiscountPercentage";
        }

        /// get character length for column
        public static short GetDiscountPercentageLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class AccountsPayableTDSPaymentDetailsRow : System.Data.DataRow
    {
        private AccountsPayableTDSPaymentDetailsTable myTable;

        /// Constructor
        public AccountsPayableTDSPaymentDetailsRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AccountsPayableTDSPaymentDetailsTable)(this.Table));
        }

        ///
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
        public Double TotalAmountToPay
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTotalAmountToPay.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTotalAmountToPay)
                            || (((Double)(this[this.myTable.ColumnTotalAmountToPay])) != value)))
                {
                    this[this.myTable.ColumnTotalAmountToPay] = value;
                }
            }
        }

        ///
        public Boolean UseDiscount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUseDiscount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUseDiscount)
                            || (((Boolean)(this[this.myTable.ColumnUseDiscount])) != value)))
                {
                    this[this.myTable.ColumnUseDiscount] = value;
                }
            }
        }

        ///
        public Boolean PayFullInvoice
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPayFullInvoice.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPayFullInvoice)
                            || (((Boolean)(this[this.myTable.ColumnPayFullInvoice])) != value)))
                {
                    this[this.myTable.ColumnPayFullInvoice] = value;
                }
            }
        }

        ///
        public Boolean HasValidDiscount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHasValidDiscount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHasValidDiscount)
                            || (((Boolean)(this[this.myTable.ColumnHasValidDiscount])) != value)))
                {
                    this[this.myTable.ColumnHasValidDiscount] = value;
                }
            }
        }

        ///
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

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnApNumber);
            this.SetNull(this.myTable.ColumnAmount);
            this.SetNull(this.myTable.ColumnTotalAmountToPay);
            this.SetNull(this.myTable.ColumnUseDiscount);
            this.SetNull(this.myTable.ColumnPayFullInvoice);
            this.SetNull(this.myTable.ColumnHasValidDiscount);
            this.SetNull(this.myTable.ColumnDiscountPercentage);
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
        public bool IsTotalAmountToPayNull()
        {
            return this.IsNull(this.myTable.ColumnTotalAmountToPay);
        }

        /// assign NULL value
        public void SetTotalAmountToPayNull()
        {
            this.SetNull(this.myTable.ColumnTotalAmountToPay);
        }

        /// test for NULL value
        public bool IsUseDiscountNull()
        {
            return this.IsNull(this.myTable.ColumnUseDiscount);
        }

        /// assign NULL value
        public void SetUseDiscountNull()
        {
            this.SetNull(this.myTable.ColumnUseDiscount);
        }

        /// test for NULL value
        public bool IsPayFullInvoiceNull()
        {
            return this.IsNull(this.myTable.ColumnPayFullInvoice);
        }

        /// assign NULL value
        public void SetPayFullInvoiceNull()
        {
            this.SetNull(this.myTable.ColumnPayFullInvoice);
        }

        /// test for NULL value
        public bool IsHasValidDiscountNull()
        {
            return this.IsNull(this.myTable.ColumnHasValidDiscount);
        }

        /// assign NULL value
        public void SetHasValidDiscountNull()
        {
            this.SetNull(this.myTable.ColumnHasValidDiscount);
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
    }
}