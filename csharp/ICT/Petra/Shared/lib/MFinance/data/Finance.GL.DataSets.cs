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
namespace Ict.Petra.Shared.MFinance.GL.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MFinance.Account.Data;

     /// auto generated
    [Serializable()]
    public class GLBatchTDS : TTypedDataSet
    {

        private ALedgerTable TableALedger;
        private ABatchTable TableABatch;
        private AJournalTable TableAJournal;
        private GLBatchTDSATransactionTable TableATransaction;

        /// auto generated
        public GLBatchTDS() :
                base("GLBatchTDS")
        {
        }

        /// auto generated for serialization
        public GLBatchTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public GLBatchTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public ALedgerTable ALedger
        {
            get
            {
                return this.TableALedger;
            }
        }

        /// auto generated
        public ABatchTable ABatch
        {
            get
            {
                return this.TableABatch;
            }
        }

        /// auto generated
        public AJournalTable AJournal
        {
            get
            {
                return this.TableAJournal;
            }
        }

        /// auto generated
        public GLBatchTDSATransactionTable ATransaction
        {
            get
            {
                return this.TableATransaction;
            }
        }

        /// auto generated
        public new virtual GLBatchTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((GLBatchTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new ALedgerTable("ALedger"));
            this.Tables.Add(new ABatchTable("ABatch"));
            this.Tables.Add(new AJournalTable("AJournal"));
            this.Tables.Add(new GLBatchTDSATransactionTable("ATransaction"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("ALedger") != -1))
            {
                this.Tables.Add(new ALedgerTable("ALedger"));
            }
            if ((ds.Tables.IndexOf("ABatch") != -1))
            {
                this.Tables.Add(new ABatchTable("ABatch"));
            }
            if ((ds.Tables.IndexOf("AJournal") != -1))
            {
                this.Tables.Add(new AJournalTable("AJournal"));
            }
            if ((ds.Tables.IndexOf("ATransaction") != -1))
            {
                this.Tables.Add(new GLBatchTDSATransactionTable("ATransaction"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableALedger != null))
            {
                this.TableALedger.InitVars();
            }
            if ((this.TableABatch != null))
            {
                this.TableABatch.InitVars();
            }
            if ((this.TableAJournal != null))
            {
                this.TableAJournal.InitVars();
            }
            if ((this.TableATransaction != null))
            {
                this.TableATransaction.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "GLBatchTDS";
            this.TableALedger = ((ALedgerTable)(this.Tables["ALedger"]));
            this.TableABatch = ((ABatchTable)(this.Tables["ABatch"]));
            this.TableAJournal = ((AJournalTable)(this.Tables["AJournal"]));
            this.TableATransaction = ((GLBatchTDSATransactionTable)(this.Tables["ATransaction"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableALedger != null)
                        && (this.TableABatch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBatch1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ABatch", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableABatch != null)
                        && (this.TableAJournal != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJournal1", "ABatch", new string[] {
                                "a_ledger_number_i", "a_batch_number_i"}, "AJournal", new string[] {
                                "a_ledger_number_i", "a_batch_number_i"}));
            }
            if (((this.TableAJournal != null)
                        && (this.TableATransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransaction1", "AJournal", new string[] {
                                "a_ledger_number_i", "a_batch_number_i", "a_journal_number_i"}, "ATransaction", new string[] {
                                "a_ledger_number_i", "a_batch_number_i", "a_journal_number_i"}));
            }

        }
    }

    /// Detailed information for each debit and credit in a general ledger journal.
    [Serializable()]
    public class GLBatchTDSATransactionTable : TTypedDataTable
    {
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        /// identifes which batch a transaction belongs to
        public DataColumn ColumnBatchNumber;
        ///
        public DataColumn ColumnJournalNumber;
        /// Identifies a transaction within a journal within a batch within a ledger
        public DataColumn ColumnTransactionNumber;
        /// This identifies the account the financial transaction must be stored against
        public DataColumn ColumnAccountCode;
        /// This identifies the account the financial transaction must be stored against [NOT USED]
        public DataColumn ColumnPrimaryAccountCode;
        /// This identifies which cost centre an account is applied to
        public DataColumn ColumnCostCentreCode;
        /// This identifies which cost centre an account is applied to [NOT USED]
        public DataColumn ColumnPrimaryCostCentreCode;
        /// Date the transaction took place
        public DataColumn ColumnTransactionDate;
        /// This is a number of currency units
        public DataColumn ColumnTransactionAmount;
        /// This is a number of currency units
        public DataColumn ColumnAmountInBaseCurrency;
        /// Used to get a yes no response from the user
        public DataColumn ColumnAnalysisIndicator;
        /// shows if the transaction has been reconciled or not
        public DataColumn ColumnReconciledStatus;
        ///
        public DataColumn ColumnNarrative;
        ///
        public DataColumn ColumnDebitCreditIndicator;
        /// Has a transaction been posted yet
        public DataColumn ColumnTransactionStatus;
        /// The header (eg, cashbook #) that the transaction is associated with. [NOT USED]
        public DataColumn ColumnHeaderNumber;
        /// The detail (within the header) that the transaction is associated with. [NOT USED]
        public DataColumn ColumnDetailNumber;
        ///
        public DataColumn ColumnSubType;
        /// Indicates whether the ILT transaction has been transferred to transaction for ILT file.
        public DataColumn ColumnToIltFlag;
        /// To flag a transaction as having come from a source ledger and been processed in an ilt processing centre
        public DataColumn ColumnSourceFlag;
        /// Reference number/code for the transaction
        public DataColumn ColumnReference;
        /// Transaction key which initiated an ILT transaction
        public DataColumn ColumnSourceReference;
        /// Was this transaction generated automatically by the system?
        public DataColumn ColumnSystemGenerated;
        /// The transaction amount in the second base currency.
        public DataColumn ColumnAmountInIntlCurrency;
        /// identifes the ICH process number
        public DataColumn ColumnIchNumber;
        /// Key ministry to which this transaction applies (just for fund transfers)
        public DataColumn ColumnKeyMinistryKey;
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
        ///
        public DataColumn ColumnDateEntered;
        ///
        public DataColumn ColumnAnalysisAttributes;

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5500;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "GLBatchTDSATransaction", "GLBatchTDSATransaction",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "JournalNumber", "a_journal_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "TransactionNumber", "a_transaction_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "AccountCode", "a_account_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "PrimaryAccountCode", "a_primary_account_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "CostCentreCode", "a_cost_centre_code_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(7, "PrimaryCostCentreCode", "a_primary_cost_centre_code_c", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(8, "TransactionDate", "a_transaction_date_d", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(9, "TransactionAmount", "a_transaction_amount_n", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(10, "AmountInBaseCurrency", "a_amount_in_base_currency_n", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(11, "AnalysisIndicator", "a_analysis_indicator_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(12, "ReconciledStatus", "a_reconciled_status_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(13, "Narrative", "a_narrative_c", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(14, "DebitCreditIndicator", "a_debit_credit_indicator_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(15, "TransactionStatus", "a_transaction_status_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(16, "HeaderNumber", "a_header_number_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(17, "DetailNumber", "a_detail_number_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(18, "SubType", "a_sub_type_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(19, "ToIltFlag", "a_to_ilt_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(20, "SourceFlag", "a_source_flag_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(21, "Reference", "a_reference_c", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(22, "SourceReference", "a_source_reference_c", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(23, "SystemGenerated", "a_system_generated_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(24, "AmountInIntlCurrency", "a_amount_in_intl_currency_n", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(25, "IchNumber", "a_ich_number_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(26, "KeyMinistryKey", "a_key_ministry_key_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(27, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(28, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(29, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(30, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(31, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false),
                    new TTypedColumnInfo(32, "DateEntered", "DateEntered", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(33, "AnalysisAttributes", "AnalysisAttributes", OdbcType.Int, -1, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public GLBatchTDSATransactionTable() :
                base("GLBatchTDSATransaction")
        {
        }

        /// constructor
        public GLBatchTDSATransactionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public GLBatchTDSATransactionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_journal_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_primary_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_primary_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_in_base_currency_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_analysis_indicator_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_reconciled_status_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_narrative_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_debit_credit_indicator_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_status_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_header_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_sub_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_to_ilt_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_source_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_source_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_system_generated_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_amount_in_intl_currency_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_ich_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_key_ministry_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DateEntered", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("AnalysisAttributes", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnJournalNumber = this.Columns["a_journal_number_i"];
            this.ColumnTransactionNumber = this.Columns["a_transaction_number_i"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnPrimaryAccountCode = this.Columns["a_primary_account_code_c"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnPrimaryCostCentreCode = this.Columns["a_primary_cost_centre_code_c"];
            this.ColumnTransactionDate = this.Columns["a_transaction_date_d"];
            this.ColumnTransactionAmount = this.Columns["a_transaction_amount_n"];
            this.ColumnAmountInBaseCurrency = this.Columns["a_amount_in_base_currency_n"];
            this.ColumnAnalysisIndicator = this.Columns["a_analysis_indicator_l"];
            this.ColumnReconciledStatus = this.Columns["a_reconciled_status_l"];
            this.ColumnNarrative = this.Columns["a_narrative_c"];
            this.ColumnDebitCreditIndicator = this.Columns["a_debit_credit_indicator_l"];
            this.ColumnTransactionStatus = this.Columns["a_transaction_status_l"];
            this.ColumnHeaderNumber = this.Columns["a_header_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnSubType = this.Columns["a_sub_type_c"];
            this.ColumnToIltFlag = this.Columns["a_to_ilt_flag_l"];
            this.ColumnSourceFlag = this.Columns["a_source_flag_l"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnSourceReference = this.Columns["a_source_reference_c"];
            this.ColumnSystemGenerated = this.Columns["a_system_generated_l"];
            this.ColumnAmountInIntlCurrency = this.Columns["a_amount_in_intl_currency_n"];
            this.ColumnIchNumber = this.Columns["a_ich_number_i"];
            this.ColumnKeyMinistryKey = this.Columns["a_key_ministry_key_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnDateEntered = this.Columns["DateEntered"];
            this.ColumnAnalysisAttributes = this.Columns["AnalysisAttributes"];
        }

        /// Access a typed row by index
        public GLBatchTDSATransactionRow this[int i]
        {
            get
            {
                return ((GLBatchTDSATransactionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public GLBatchTDSATransactionRow NewRowTyped(bool AWithDefaultValues)
        {
            GLBatchTDSATransactionRow ret = ((GLBatchTDSATransactionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public GLBatchTDSATransactionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new GLBatchTDSATransactionRow(builder);
        }

        /// get typed set of changes
        public GLBatchTDSATransactionTable GetChangesTyped()
        {
            return ((GLBatchTDSATransactionTable)(base.GetChangesTypedInternal()));
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
        public static string GetJournalNumberDBName()
        {
            return "a_journal_number_i";
        }

        /// get character length for column
        public static short GetJournalNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTransactionNumberDBName()
        {
            return "a_transaction_number_i";
        }

        /// get character length for column
        public static short GetTransactionNumberLength()
        {
            return -1;
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
        public static string GetPrimaryAccountCodeDBName()
        {
            return "a_primary_account_code_c";
        }

        /// get character length for column
        public static short GetPrimaryAccountCodeLength()
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
        public static string GetPrimaryCostCentreCodeDBName()
        {
            return "a_primary_cost_centre_code_c";
        }

        /// get character length for column
        public static short GetPrimaryCostCentreCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetTransactionDateDBName()
        {
            return "a_transaction_date_d";
        }

        /// get character length for column
        public static short GetTransactionDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTransactionAmountDBName()
        {
            return "a_transaction_amount_n";
        }

        /// get character length for column
        public static short GetTransactionAmountLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountInBaseCurrencyDBName()
        {
            return "a_amount_in_base_currency_n";
        }

        /// get character length for column
        public static short GetAmountInBaseCurrencyLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetAnalysisIndicatorDBName()
        {
            return "a_analysis_indicator_l";
        }

        /// get character length for column
        public static short GetAnalysisIndicatorLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReconciledStatusDBName()
        {
            return "a_reconciled_status_l";
        }

        /// get character length for column
        public static short GetReconciledStatusLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNarrativeDBName()
        {
            return "a_narrative_c";
        }

        /// get character length for column
        public static short GetNarrativeLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetDebitCreditIndicatorDBName()
        {
            return "a_debit_credit_indicator_l";
        }

        /// get character length for column
        public static short GetDebitCreditIndicatorLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTransactionStatusDBName()
        {
            return "a_transaction_status_l";
        }

        /// get character length for column
        public static short GetTransactionStatusLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHeaderNumberDBName()
        {
            return "a_header_number_i";
        }

        /// get character length for column
        public static short GetHeaderNumberLength()
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
        public static string GetSubTypeDBName()
        {
            return "a_sub_type_c";
        }

        /// get character length for column
        public static short GetSubTypeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetToIltFlagDBName()
        {
            return "a_to_ilt_flag_l";
        }

        /// get character length for column
        public static short GetToIltFlagLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetSourceFlagDBName()
        {
            return "a_source_flag_l";
        }

        /// get character length for column
        public static short GetSourceFlagLength()
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
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetSourceReferenceDBName()
        {
            return "a_source_reference_c";
        }

        /// get character length for column
        public static short GetSourceReferenceLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetSystemGeneratedDBName()
        {
            return "a_system_generated_l";
        }

        /// get character length for column
        public static short GetSystemGeneratedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAmountInIntlCurrencyDBName()
        {
            return "a_amount_in_intl_currency_n";
        }

        /// get character length for column
        public static short GetAmountInIntlCurrencyLength()
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
        public static string GetKeyMinistryKeyDBName()
        {
            return "a_key_ministry_key_n";
        }

        /// get character length for column
        public static short GetKeyMinistryKeyLength()
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

        /// get the name of the field in the database for this column
        public static string GetDateEnteredDBName()
        {
            return "DateEntered";
        }

        /// get character length for column
        public static short GetDateEnteredLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnalysisAttributesDBName()
        {
            return "AnalysisAttributes";
        }

        /// get character length for column
        public static short GetAnalysisAttributesLength()
        {
            return -1;
        }

    }

    /// Detailed information for each debit and credit in a general ledger journal.
    [Serializable()]
    public class GLBatchTDSATransactionRow : System.Data.DataRow
    {
        private GLBatchTDSATransactionTable myTable;

        /// Constructor
        public GLBatchTDSATransactionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((GLBatchTDSATransactionTable)(this.Table));
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

        ///
        public Int32 JournalNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnJournalNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnJournalNumber)
                            || (((Int32)(this[this.myTable.ColumnJournalNumber])) != value)))
                {
                    this[this.myTable.ColumnJournalNumber] = value;
                }
            }
        }

        /// Identifies a transaction within a journal within a batch within a ledger
        public Int32 TransactionNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTransactionNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTransactionNumber)
                            || (((Int32)(this[this.myTable.ColumnTransactionNumber])) != value)))
                {
                    this[this.myTable.ColumnTransactionNumber] = value;
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

        /// This identifies the account the financial transaction must be stored against [NOT USED]
        public String PrimaryAccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPrimaryAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPrimaryAccountCode)
                            || (((String)(this[this.myTable.ColumnPrimaryAccountCode])) != value)))
                {
                    this[this.myTable.ColumnPrimaryAccountCode] = value;
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

        /// This identifies which cost centre an account is applied to [NOT USED]
        public String PrimaryCostCentreCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPrimaryCostCentreCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPrimaryCostCentreCode)
                            || (((String)(this[this.myTable.ColumnPrimaryCostCentreCode])) != value)))
                {
                    this[this.myTable.ColumnPrimaryCostCentreCode] = value;
                }
            }
        }

        /// Date the transaction took place
        public System.DateTime TransactionDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTransactionDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTransactionDate)
                            || (((System.DateTime)(this[this.myTable.ColumnTransactionDate])) != value)))
                {
                    this[this.myTable.ColumnTransactionDate] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double TransactionAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTransactionAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTransactionAmount)
                            || (((Double)(this[this.myTable.ColumnTransactionAmount])) != value)))
                {
                    this[this.myTable.ColumnTransactionAmount] = value;
                }
            }
        }

        /// This is a number of currency units
        public Double AmountInBaseCurrency
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAmountInBaseCurrency.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAmountInBaseCurrency)
                            || (((Double)(this[this.myTable.ColumnAmountInBaseCurrency])) != value)))
                {
                    this[this.myTable.ColumnAmountInBaseCurrency] = value;
                }
            }
        }

        /// Used to get a yes no response from the user
        public Boolean AnalysisIndicator
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnalysisIndicator.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnalysisIndicator)
                            || (((Boolean)(this[this.myTable.ColumnAnalysisIndicator])) != value)))
                {
                    this[this.myTable.ColumnAnalysisIndicator] = value;
                }
            }
        }

        /// shows if the transaction has been reconciled or not
        public Boolean ReconciledStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReconciledStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReconciledStatus)
                            || (((Boolean)(this[this.myTable.ColumnReconciledStatus])) != value)))
                {
                    this[this.myTable.ColumnReconciledStatus] = value;
                }
            }
        }

        ///
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

        ///
        public Boolean DebitCreditIndicator
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDebitCreditIndicator.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDebitCreditIndicator)
                            || (((Boolean)(this[this.myTable.ColumnDebitCreditIndicator])) != value)))
                {
                    this[this.myTable.ColumnDebitCreditIndicator] = value;
                }
            }
        }

        /// Has a transaction been posted yet
        public Boolean TransactionStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTransactionStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTransactionStatus)
                            || (((Boolean)(this[this.myTable.ColumnTransactionStatus])) != value)))
                {
                    this[this.myTable.ColumnTransactionStatus] = value;
                }
            }
        }

        /// The header (eg, cashbook #) that the transaction is associated with. [NOT USED]
        public Int32 HeaderNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHeaderNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHeaderNumber)
                            || (((Int32)(this[this.myTable.ColumnHeaderNumber])) != value)))
                {
                    this[this.myTable.ColumnHeaderNumber] = value;
                }
            }
        }

        /// The detail (within the header) that the transaction is associated with. [NOT USED]
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
        public String SubType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSubType.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSubType)
                            || (((String)(this[this.myTable.ColumnSubType])) != value)))
                {
                    this[this.myTable.ColumnSubType] = value;
                }
            }
        }

        /// Indicates whether the ILT transaction has been transferred to transaction for ILT file.
        public Boolean ToIltFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnToIltFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnToIltFlag)
                            || (((Boolean)(this[this.myTable.ColumnToIltFlag])) != value)))
                {
                    this[this.myTable.ColumnToIltFlag] = value;
                }
            }
        }

        /// To flag a transaction as having come from a source ledger and been processed in an ilt processing centre
        public Boolean SourceFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSourceFlag.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSourceFlag)
                            || (((Boolean)(this[this.myTable.ColumnSourceFlag])) != value)))
                {
                    this[this.myTable.ColumnSourceFlag] = value;
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

        /// Transaction key which initiated an ILT transaction
        public String SourceReference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSourceReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSourceReference)
                            || (((String)(this[this.myTable.ColumnSourceReference])) != value)))
                {
                    this[this.myTable.ColumnSourceReference] = value;
                }
            }
        }

        /// Was this transaction generated automatically by the system?
        public Boolean SystemGenerated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSystemGenerated.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSystemGenerated)
                            || (((Boolean)(this[this.myTable.ColumnSystemGenerated])) != value)))
                {
                    this[this.myTable.ColumnSystemGenerated] = value;
                }
            }
        }

        /// The transaction amount in the second base currency.
        public Double AmountInIntlCurrency
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAmountInIntlCurrency.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAmountInIntlCurrency)
                            || (((Double)(this[this.myTable.ColumnAmountInIntlCurrency])) != value)))
                {
                    this[this.myTable.ColumnAmountInIntlCurrency] = value;
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

        /// Key ministry to which this transaction applies (just for fund transfers)
        public Int64 KeyMinistryKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnKeyMinistryKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnKeyMinistryKey)
                            || (((Int64)(this[this.myTable.ColumnKeyMinistryKey])) != value)))
                {
                    this[this.myTable.ColumnKeyMinistryKey] = value;
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

        ///
        public DateTime DateEntered
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
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateEntered)
                            || (((DateTime)(this[this.myTable.ColumnDateEntered])) != value)))
                {
                    this[this.myTable.ColumnDateEntered] = value;
                }
            }
        }

        ///
        public string AnalysisAttributes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnalysisAttributes.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnalysisAttributes)
                            || (((string)(this[this.myTable.ColumnAnalysisAttributes])) != value)))
                {
                    this[this.myTable.ColumnAnalysisAttributes] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnJournalNumber.Ordinal] = 0;
            this[this.myTable.ColumnTransactionNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnPrimaryAccountCode);
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this.SetNull(this.myTable.ColumnPrimaryCostCentreCode);
            this[this.myTable.ColumnTransactionDate.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnTransactionAmount.Ordinal] = 0;
            this[this.myTable.ColumnAmountInBaseCurrency.Ordinal] = 0;
            this[this.myTable.ColumnAnalysisIndicator.Ordinal] = false;
            this[this.myTable.ColumnReconciledStatus.Ordinal] = false;
            this.SetNull(this.myTable.ColumnNarrative);
            this[this.myTable.ColumnDebitCreditIndicator.Ordinal] = true;
            this[this.myTable.ColumnTransactionStatus.Ordinal] = false;
            this[this.myTable.ColumnHeaderNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnSubType);
            this[this.myTable.ColumnToIltFlag.Ordinal] = false;
            this[this.myTable.ColumnSourceFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnReference);
            this.SetNull(this.myTable.ColumnSourceReference);
            this[this.myTable.ColumnSystemGenerated.Ordinal] = false;
            this[this.myTable.ColumnAmountInIntlCurrency.Ordinal] = 0;
            this[this.myTable.ColumnIchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnKeyMinistryKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnDateEntered);
            this.SetNull(this.myTable.ColumnAnalysisAttributes);
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
        public bool IsJournalNumberNull()
        {
            return this.IsNull(this.myTable.ColumnJournalNumber);
        }

        /// assign NULL value
        public void SetJournalNumberNull()
        {
            this.SetNull(this.myTable.ColumnJournalNumber);
        }

        /// test for NULL value
        public bool IsTransactionNumberNull()
        {
            return this.IsNull(this.myTable.ColumnTransactionNumber);
        }

        /// assign NULL value
        public void SetTransactionNumberNull()
        {
            this.SetNull(this.myTable.ColumnTransactionNumber);
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
        public bool IsPrimaryAccountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPrimaryAccountCode);
        }

        /// assign NULL value
        public void SetPrimaryAccountCodeNull()
        {
            this.SetNull(this.myTable.ColumnPrimaryAccountCode);
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
        public bool IsPrimaryCostCentreCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPrimaryCostCentreCode);
        }

        /// assign NULL value
        public void SetPrimaryCostCentreCodeNull()
        {
            this.SetNull(this.myTable.ColumnPrimaryCostCentreCode);
        }

        /// test for NULL value
        public bool IsTransactionDateNull()
        {
            return this.IsNull(this.myTable.ColumnTransactionDate);
        }

        /// assign NULL value
        public void SetTransactionDateNull()
        {
            this.SetNull(this.myTable.ColumnTransactionDate);
        }

        /// test for NULL value
        public bool IsTransactionAmountNull()
        {
            return this.IsNull(this.myTable.ColumnTransactionAmount);
        }

        /// assign NULL value
        public void SetTransactionAmountNull()
        {
            this.SetNull(this.myTable.ColumnTransactionAmount);
        }

        /// test for NULL value
        public bool IsAmountInBaseCurrencyNull()
        {
            return this.IsNull(this.myTable.ColumnAmountInBaseCurrency);
        }

        /// assign NULL value
        public void SetAmountInBaseCurrencyNull()
        {
            this.SetNull(this.myTable.ColumnAmountInBaseCurrency);
        }

        /// test for NULL value
        public bool IsAnalysisIndicatorNull()
        {
            return this.IsNull(this.myTable.ColumnAnalysisIndicator);
        }

        /// assign NULL value
        public void SetAnalysisIndicatorNull()
        {
            this.SetNull(this.myTable.ColumnAnalysisIndicator);
        }

        /// test for NULL value
        public bool IsReconciledStatusNull()
        {
            return this.IsNull(this.myTable.ColumnReconciledStatus);
        }

        /// assign NULL value
        public void SetReconciledStatusNull()
        {
            this.SetNull(this.myTable.ColumnReconciledStatus);
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
        public bool IsDebitCreditIndicatorNull()
        {
            return this.IsNull(this.myTable.ColumnDebitCreditIndicator);
        }

        /// assign NULL value
        public void SetDebitCreditIndicatorNull()
        {
            this.SetNull(this.myTable.ColumnDebitCreditIndicator);
        }

        /// test for NULL value
        public bool IsTransactionStatusNull()
        {
            return this.IsNull(this.myTable.ColumnTransactionStatus);
        }

        /// assign NULL value
        public void SetTransactionStatusNull()
        {
            this.SetNull(this.myTable.ColumnTransactionStatus);
        }

        /// test for NULL value
        public bool IsHeaderNumberNull()
        {
            return this.IsNull(this.myTable.ColumnHeaderNumber);
        }

        /// assign NULL value
        public void SetHeaderNumberNull()
        {
            this.SetNull(this.myTable.ColumnHeaderNumber);
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
        public bool IsSubTypeNull()
        {
            return this.IsNull(this.myTable.ColumnSubType);
        }

        /// assign NULL value
        public void SetSubTypeNull()
        {
            this.SetNull(this.myTable.ColumnSubType);
        }

        /// test for NULL value
        public bool IsToIltFlagNull()
        {
            return this.IsNull(this.myTable.ColumnToIltFlag);
        }

        /// assign NULL value
        public void SetToIltFlagNull()
        {
            this.SetNull(this.myTable.ColumnToIltFlag);
        }

        /// test for NULL value
        public bool IsSourceFlagNull()
        {
            return this.IsNull(this.myTable.ColumnSourceFlag);
        }

        /// assign NULL value
        public void SetSourceFlagNull()
        {
            this.SetNull(this.myTable.ColumnSourceFlag);
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
        public bool IsSourceReferenceNull()
        {
            return this.IsNull(this.myTable.ColumnSourceReference);
        }

        /// assign NULL value
        public void SetSourceReferenceNull()
        {
            this.SetNull(this.myTable.ColumnSourceReference);
        }

        /// test for NULL value
        public bool IsSystemGeneratedNull()
        {
            return this.IsNull(this.myTable.ColumnSystemGenerated);
        }

        /// assign NULL value
        public void SetSystemGeneratedNull()
        {
            this.SetNull(this.myTable.ColumnSystemGenerated);
        }

        /// test for NULL value
        public bool IsAmountInIntlCurrencyNull()
        {
            return this.IsNull(this.myTable.ColumnAmountInIntlCurrency);
        }

        /// assign NULL value
        public void SetAmountInIntlCurrencyNull()
        {
            this.SetNull(this.myTable.ColumnAmountInIntlCurrency);
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
        public bool IsKeyMinistryKeyNull()
        {
            return this.IsNull(this.myTable.ColumnKeyMinistryKey);
        }

        /// assign NULL value
        public void SetKeyMinistryKeyNull()
        {
            this.SetNull(this.myTable.ColumnKeyMinistryKey);
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
        public bool IsAnalysisAttributesNull()
        {
            return this.IsNull(this.myTable.ColumnAnalysisAttributes);
        }

        /// assign NULL value
        public void SetAnalysisAttributesNull()
        {
            this.SetNull(this.myTable.ColumnAnalysisAttributes);
        }
    }
}