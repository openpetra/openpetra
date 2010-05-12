// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//

using Ict.Common;
using Ict.Common.Data;
using System;
using System.Data;
using System.Data.Odbc;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance.AP.Data
{

     /// auto generated
    [Serializable()]
    public class AccountsPayableTDS : TTypedDataSet
    {

        private AApSupplierTable TableAApSupplier;
        private AccountsPayableTDSAApDocumentTable TableAApDocument;
        private AApDocumentDetailTable TableAApDocumentDetail;
        private AccountsPayableTDSAApDocumentPaymentTable TableAApDocumentPayment;
        private AccountsPayableTDSAApPaymentTable TableAApPayment;
        private AApAnalAttribTable TableAApAnalAttrib;

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
        public AccountsPayableTDSAApDocumentPaymentTable AApDocumentPayment
        {
            get
            {
                return this.TableAApDocumentPayment;
            }
        }

        /// auto generated
        public AccountsPayableTDSAApPaymentTable AApPayment
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
            this.Tables.Add(new AccountsPayableTDSAApDocumentPaymentTable("AApDocumentPayment"));
            this.Tables.Add(new AccountsPayableTDSAApPaymentTable("AApPayment"));
            this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
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
                this.Tables.Add(new AccountsPayableTDSAApDocumentPaymentTable("AApDocumentPayment"));
            }
            if ((ds.Tables.IndexOf("AApPayment") != -1))
            {
                this.Tables.Add(new AccountsPayableTDSAApPaymentTable("AApPayment"));
            }
            if ((ds.Tables.IndexOf("AApAnalAttrib") != -1))
            {
                this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
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
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "AccountsPayableTDS";
            this.TableAApSupplier = ((AApSupplierTable)(this.Tables["AApSupplier"]));
            this.TableAApDocument = ((AccountsPayableTDSAApDocumentTable)(this.Tables["AApDocument"]));
            this.TableAApDocumentDetail = ((AApDocumentDetailTable)(this.Tables["AApDocumentDetail"]));
            this.TableAApDocumentPayment = ((AccountsPayableTDSAApDocumentPaymentTable)(this.Tables["AApDocumentPayment"]));
            this.TableAApPayment = ((AccountsPayableTDSAApPaymentTable)(this.Tables["AApPayment"]));
            this.TableAApAnalAttrib = ((AApAnalAttribTable)(this.Tables["AApAnalAttrib"]));
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
        public new static short TableId = 175;
        /// used for generic TTypedDataTable functions
        public static short ColumnTaggedId = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateDueId = 22;

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

    /// This table links the different payments to actual invoices and credit notes.
    [Serializable()]
    public class AccountsPayableTDSAApDocumentPaymentTable : AApDocumentPaymentTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 179;
        /// used for generic TTypedDataTable functions
        public static short ColumnTotalAmountToPayId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnUseDiscountId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnPayFullInvoiceId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnHasValidDiscountId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnDiscountPercentageId = 13;

        /// constructor
        public AccountsPayableTDSAApDocumentPaymentTable() :
                base("AApDocumentPayment")
        {
        }

        /// constructor
        public AccountsPayableTDSAApDocumentPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AccountsPayableTDSAApDocumentPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

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
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ap_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_payment_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("TotalAmountToPay", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("UseDiscount", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("PayFullInvoice", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("HasValidDiscount", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("DiscountPercentage", typeof(Double)));
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
            this.ColumnTotalAmountToPay = this.Columns["TotalAmountToPay"];
            this.ColumnUseDiscount = this.Columns["UseDiscount"];
            this.ColumnPayFullInvoice = this.Columns["PayFullInvoice"];
            this.ColumnHasValidDiscount = this.Columns["HasValidDiscount"];
            this.ColumnDiscountPercentage = this.Columns["DiscountPercentage"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnLedgerNumber,ColumnApNumber,ColumnPaymentNumber};
        }

        /// Access a typed row by index
        public new AccountsPayableTDSAApDocumentPaymentRow this[int i]
        {
            get
            {
                return ((AccountsPayableTDSAApDocumentPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new AccountsPayableTDSAApDocumentPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AccountsPayableTDSAApDocumentPaymentRow ret = ((AccountsPayableTDSAApDocumentPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new AccountsPayableTDSAApDocumentPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AccountsPayableTDSAApDocumentPaymentRow(builder);
        }

        /// get typed set of changes
        public new AccountsPayableTDSAApDocumentPaymentTable GetChangesTyped()
        {
            return ((AccountsPayableTDSAApDocumentPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AApDocumentPayment";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_ap_document_payment";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
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

    /// This table links the different payments to actual invoices and credit notes.
    [Serializable()]
    public class AccountsPayableTDSAApDocumentPaymentRow : AApDocumentPaymentRow
    {
        private AccountsPayableTDSAApDocumentPaymentTable myTable;

        /// Constructor
        public AccountsPayableTDSAApDocumentPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AccountsPayableTDSAApDocumentPaymentTable)(this.Table));
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
        public override void InitValues()
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
            this.SetNull(this.myTable.ColumnTotalAmountToPay);
            this.SetNull(this.myTable.ColumnUseDiscount);
            this.SetNull(this.myTable.ColumnPayFullInvoice);
            this.SetNull(this.myTable.ColumnHasValidDiscount);
            this.SetNull(this.myTable.ColumnDiscountPercentage);
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

    /// Records all payments that have been made against an accounts payable detail.
    [Serializable()]
    public class AccountsPayableTDSAApPaymentTable : AApPaymentTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 178;
        /// used for generic TTypedDataTable functions
        public static short ColumnSupplierKeyId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnSupplierNameId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnCurrencyCodeId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnListLabelId = 17;

        /// constructor
        public AccountsPayableTDSAApPaymentTable() :
                base("AApPayment")
        {
        }

        /// constructor
        public AccountsPayableTDSAApPaymentTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public AccountsPayableTDSAApPaymentTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnSupplierKey;
        ///
        public DataColumn ColumnSupplierName;
        ///
        public DataColumn ColumnCurrencyCode;
        ///
        public DataColumn ColumnListLabel;

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
            this.Columns.Add(new System.Data.DataColumn("SupplierKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("SupplierName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("CurrencyCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("ListLabel", typeof(string)));
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
            this.ColumnSupplierKey = this.Columns["SupplierKey"];
            this.ColumnSupplierName = this.Columns["SupplierName"];
            this.ColumnCurrencyCode = this.Columns["CurrencyCode"];
            this.ColumnListLabel = this.Columns["ListLabel"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnPaymentNumber};
        }

        /// Access a typed row by index
        public new AccountsPayableTDSAApPaymentRow this[int i]
        {
            get
            {
                return ((AccountsPayableTDSAApPaymentRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new AccountsPayableTDSAApPaymentRow NewRowTyped(bool AWithDefaultValues)
        {
            AccountsPayableTDSAApPaymentRow ret = ((AccountsPayableTDSAApPaymentRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new AccountsPayableTDSAApPaymentRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AccountsPayableTDSAApPaymentRow(builder);
        }

        /// get typed set of changes
        public new AccountsPayableTDSAApPaymentTable GetChangesTyped()
        {
            return ((AccountsPayableTDSAApPaymentTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AApPayment";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_ap_payment";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
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
        public static string GetCurrencyCodeDBName()
        {
            return "CurrencyCode";
        }

        /// get character length for column
        public static short GetCurrencyCodeLength()
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

    /// Records all payments that have been made against an accounts payable detail.
    [Serializable()]
    public class AccountsPayableTDSAApPaymentRow : AApPaymentRow
    {
        private AccountsPayableTDSAApPaymentTable myTable;

        /// Constructor
        public AccountsPayableTDSAApPaymentRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((AccountsPayableTDSAApPaymentTable)(this.Table));
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
        public string CurrencyCode
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyCode)
                            || (((string)(this[this.myTable.ColumnCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnCurrencyCode] = value;
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
        public override void InitValues()
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
            this.SetNull(this.myTable.ColumnSupplierKey);
            this.SetNull(this.myTable.ColumnSupplierName);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnListLabel);
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
}
