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
    public class GLBatchTDSATransactionTable : ATransactionTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 5500;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEnteredId = 32;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnalysisAttributesId = 33;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ATransaction", "a_transaction",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "JournalNumber", "a_journal_number_i", "Journal Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "TransactionNumber", "a_transaction_number_i", "Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "AccountCode", "a_account_code_c", "Account Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(5, "PrimaryAccountCode", "a_primary_account_code_c", "Primary Account Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre Code", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(7, "PrimaryCostCentreCode", "a_primary_cost_centre_code_c", "Primary Cost Centre Code", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(8, "TransactionDate", "a_transaction_date_d", "Transaction Date", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(9, "TransactionAmount", "a_transaction_amount_n", "Transaction Amount", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(10, "AmountInBaseCurrency", "a_amount_in_base_currency_n", "Amount in Base Currency", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(11, "AnalysisIndicator", "a_analysis_indicator_l", "Analysis Indicator", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(12, "ReconciledStatus", "a_reconciled_status_l", "a_reconciled_status_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(13, "Narrative", "a_narrative_c", "Narrative", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(14, "DebitCreditIndicator", "a_debit_credit_indicator_l", "Debit/Credit Indicator", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(15, "TransactionStatus", "a_transaction_status_l", "Transaction Posted Status", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(16, "HeaderNumber", "a_header_number_i", "Header Number", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(17, "DetailNumber", "a_detail_number_i", "Detail Number", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(18, "SubType", "a_sub_type_c", "a_sub_type_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(19, "ToIltFlag", "a_to_ilt_flag_l", "Transferred to ILT Ledger", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(20, "SourceFlag", "a_source_flag_l", "Source Transaction", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(21, "Reference", "a_reference_c", "Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(22, "SourceReference", "a_source_reference_c", "Source Reference", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(23, "SystemGenerated", "a_system_generated_l", "System Generated", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(24, "AmountInIntlCurrency", "a_amount_in_intl_currency_n", "Amount in International Currency", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(25, "IchNumber", "a_ich_number_i", "ICH Process Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(26, "KeyMinistryKey", "a_key_ministry_key_n", "Key Ministry", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(27, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(28, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(29, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(30, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(31, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false),
                    new TTypedColumnInfo(32, "DateEntered", "DateEntered", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(33, "AnalysisAttributes", "AnalysisAttributes", "", OdbcType.Int, -1, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public GLBatchTDSATransactionTable() :
                base("ATransaction")
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

        ///
        public DataColumn ColumnDateEntered;
        ///
        public DataColumn ColumnAnalysisAttributes;

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
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnJournalNumber,ColumnTransactionNumber};
        }

        /// Access a typed row by index
        public new GLBatchTDSATransactionRow this[int i]
        {
            get
            {
                return ((GLBatchTDSATransactionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new GLBatchTDSATransactionRow NewRowTyped(bool AWithDefaultValues)
        {
            GLBatchTDSATransactionRow ret = ((GLBatchTDSATransactionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new GLBatchTDSATransactionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new GLBatchTDSATransactionRow(builder);
        }

        /// get typed set of changes
        public new GLBatchTDSATransactionTable GetChangesTyped()
        {
            return ((GLBatchTDSATransactionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "ATransaction";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_transaction";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
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
    public class GLBatchTDSATransactionRow : ATransactionRow
    {
        private GLBatchTDSATransactionTable myTable;

        /// Constructor
        public GLBatchTDSATransactionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((GLBatchTDSATransactionTable)(this.Table));
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
                    return DateTime.MinValue;
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
                    return String.Empty;
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
        public override void InitValues()
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