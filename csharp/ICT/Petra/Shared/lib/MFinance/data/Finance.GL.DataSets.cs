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

using Ict.Common;
using Ict.Common.Data;
using System;
using System.Data;
using System.Data.Odbc;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MFinance.GL.Data
{

     /// auto generated
    [Serializable()]
    public class GLBatchTDS : TTypedDataSet
    {

        private ALedgerTable TableALedger;
        private ABatchTable TableABatch;
        private AJournalTable TableAJournal;
        private GLBatchTDSATransactionTable TableATransaction;
        private AAccountTable TableAAccount;
        private ACostCentreTable TableACostCentre;
        private AAccountHierarchyDetailTable TableAAccountHierarchyDetail;
        private AGeneralLedgerMasterTable TableAGeneralLedgerMaster;
        private AGeneralLedgerMasterPeriodTable TableAGeneralLedgerMasterPeriod;

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
        public AAccountTable AAccount
        {
            get
            {
                return this.TableAAccount;
            }
        }

        /// auto generated
        public ACostCentreTable ACostCentre
        {
            get
            {
                return this.TableACostCentre;
            }
        }

        /// auto generated
        public AAccountHierarchyDetailTable AAccountHierarchyDetail
        {
            get
            {
                return this.TableAAccountHierarchyDetail;
            }
        }

        /// auto generated
        public AGeneralLedgerMasterTable AGeneralLedgerMaster
        {
            get
            {
                return this.TableAGeneralLedgerMaster;
            }
        }

        /// auto generated
        public AGeneralLedgerMasterPeriodTable AGeneralLedgerMasterPeriod
        {
            get
            {
                return this.TableAGeneralLedgerMasterPeriod;
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
            this.Tables.Add(new AAccountTable("AAccount"));
            this.Tables.Add(new ACostCentreTable("ACostCentre"));
            this.Tables.Add(new AAccountHierarchyDetailTable("AAccountHierarchyDetail"));
            this.Tables.Add(new AGeneralLedgerMasterTable("AGeneralLedgerMaster"));
            this.Tables.Add(new AGeneralLedgerMasterPeriodTable("AGeneralLedgerMasterPeriod"));
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
            if ((ds.Tables.IndexOf("AAccount") != -1))
            {
                this.Tables.Add(new AAccountTable("AAccount"));
            }
            if ((ds.Tables.IndexOf("ACostCentre") != -1))
            {
                this.Tables.Add(new ACostCentreTable("ACostCentre"));
            }
            if ((ds.Tables.IndexOf("AAccountHierarchyDetail") != -1))
            {
                this.Tables.Add(new AAccountHierarchyDetailTable("AAccountHierarchyDetail"));
            }
            if ((ds.Tables.IndexOf("AGeneralLedgerMaster") != -1))
            {
                this.Tables.Add(new AGeneralLedgerMasterTable("AGeneralLedgerMaster"));
            }
            if ((ds.Tables.IndexOf("AGeneralLedgerMasterPeriod") != -1))
            {
                this.Tables.Add(new AGeneralLedgerMasterPeriodTable("AGeneralLedgerMasterPeriod"));
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
            if ((this.TableAAccount != null))
            {
                this.TableAAccount.InitVars();
            }
            if ((this.TableACostCentre != null))
            {
                this.TableACostCentre.InitVars();
            }
            if ((this.TableAAccountHierarchyDetail != null))
            {
                this.TableAAccountHierarchyDetail.InitVars();
            }
            if ((this.TableAGeneralLedgerMaster != null))
            {
                this.TableAGeneralLedgerMaster.InitVars();
            }
            if ((this.TableAGeneralLedgerMasterPeriod != null))
            {
                this.TableAGeneralLedgerMasterPeriod.InitVars();
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
            this.TableAAccount = ((AAccountTable)(this.Tables["AAccount"]));
            this.TableACostCentre = ((ACostCentreTable)(this.Tables["ACostCentre"]));
            this.TableAAccountHierarchyDetail = ((AAccountHierarchyDetailTable)(this.Tables["AAccountHierarchyDetail"]));
            this.TableAGeneralLedgerMaster = ((AGeneralLedgerMasterTable)(this.Tables["AGeneralLedgerMaster"]));
            this.TableAGeneralLedgerMasterPeriod = ((AGeneralLedgerMasterPeriodTable)(this.Tables["AGeneralLedgerMasterPeriod"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableALedger != null)
                        && (this.TableAAccount != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccount1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AAccount", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountHierarchyDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchyDetail2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountHierarchyDetail", new string[] {
                                "a_ledger_number_i", "a_reporting_account_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountHierarchyDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchyDetail3", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountHierarchyDetail", new string[] {
                                "a_ledger_number_i", "a_account_code_to_report_to_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableABatch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBatch1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ABatch", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableACostCentre != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKCostCentre1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ACostCentre", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAGeneralLedgerMaster != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralLedgerMaster1", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AGeneralLedgerMaster", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAGeneralLedgerMaster != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralLedgerMaster2", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AGeneralLedgerMaster", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TableAGeneralLedgerMaster != null)
                        && (this.TableAGeneralLedgerMasterPeriod != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGlmPeriod1", "AGeneralLedgerMaster", new string[] {
                                "a_glm_sequence_i"}, "AGeneralLedgerMasterPeriod", new string[] {
                                "a_glm_sequence_i"}));
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
            if (((this.TableAAccount != null)
                        && (this.TableATransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransaction2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "ATransaction", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableATransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransaction3", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "ATransaction", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
        }
    }

    /// Detailed information for each debit and credit in a general ledger journal.
    [Serializable()]
    public class GLBatchTDSATransactionTable : ATransactionTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 168;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEnteredId = 32;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnalysisAttributesId = 33;

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

     /// auto generated
    [Serializable()]
    public class GLSetupTDS : TTypedDataSet
    {

        private PPartnerTable TablePPartner;
        private ALedgerTable TableALedger;
        private ALedgerInitFlagTable TableALedgerInitFlag;
        private AAccountingSystemParameterTable TableAAccountingSystemParameter;
        private AAccountingPeriodTable TableAAccountingPeriod;
        private ASystemInterfaceTable TableASystemInterface;
        private ATransactionTypeTable TableATransactionType;
        private AValidLedgerNumberTable TableAValidLedgerNumber;
        private GLSetupTDSAAccountTable TableAAccount;
        private AAccountHierarchyTable TableAAccountHierarchy;
        private AAccountHierarchyDetailTable TableAAccountHierarchyDetail;
        private AAccountPropertyTable TableAAccountProperty;
        private AAccountPropertyCodeTable TableAAccountPropertyCode;
        private AAnalysisAttributeTable TableAAnalysisAttribute;
        private AAnalysisStoreTableTable TableAAnalysisStoreTable;
        private AAnalysisTypeTable TableAAnalysisType;
        private AFreeformAnalysisTable TableAFreeformAnalysis;
        private ABudgetTable TableABudget;
        private ABudgetPeriodTable TableABudgetPeriod;
        private ABudgetRevisionTable TableABudgetRevision;
        private ABudgetTypeTable TableABudgetType;
        private ACostCentreTypesTable TableACostCentreTypes;
        private ACostCentreTable TableACostCentre;
        private AGeneralLedgerMasterTable TableAGeneralLedgerMaster;
        private AGeneralLedgerMasterPeriodTable TableAGeneralLedgerMasterPeriod;
        private AMotivationGroupTable TableAMotivationGroup;
        private AMotivationDetailTable TableAMotivationDetail;
        private SModuleTable TableSModule;
        private SSystemDefaultsTable TableSSystemDefaults;
        private PUnitTable TablePUnit;
        private PLocationTable TablePLocation;
        private PPartnerTypeTable TablePPartnerType;
        private PPartnerLocationTable TablePPartnerLocation;
        private PPartnerLedgerTable TablePPartnerLedger;

        /// auto generated
        public GLSetupTDS() :
                base("GLSetupTDS")
        {
        }

        /// auto generated for serialization
        public GLSetupTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public GLSetupTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public PPartnerTable PPartner
        {
            get
            {
                return this.TablePPartner;
            }
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
        public ALedgerInitFlagTable ALedgerInitFlag
        {
            get
            {
                return this.TableALedgerInitFlag;
            }
        }

        /// auto generated
        public AAccountingSystemParameterTable AAccountingSystemParameter
        {
            get
            {
                return this.TableAAccountingSystemParameter;
            }
        }

        /// auto generated
        public AAccountingPeriodTable AAccountingPeriod
        {
            get
            {
                return this.TableAAccountingPeriod;
            }
        }

        /// auto generated
        public ASystemInterfaceTable ASystemInterface
        {
            get
            {
                return this.TableASystemInterface;
            }
        }

        /// auto generated
        public ATransactionTypeTable ATransactionType
        {
            get
            {
                return this.TableATransactionType;
            }
        }

        /// auto generated
        public AValidLedgerNumberTable AValidLedgerNumber
        {
            get
            {
                return this.TableAValidLedgerNumber;
            }
        }

        /// auto generated
        public GLSetupTDSAAccountTable AAccount
        {
            get
            {
                return this.TableAAccount;
            }
        }

        /// auto generated
        public AAccountHierarchyTable AAccountHierarchy
        {
            get
            {
                return this.TableAAccountHierarchy;
            }
        }

        /// auto generated
        public AAccountHierarchyDetailTable AAccountHierarchyDetail
        {
            get
            {
                return this.TableAAccountHierarchyDetail;
            }
        }

        /// auto generated
        public AAccountPropertyTable AAccountProperty
        {
            get
            {
                return this.TableAAccountProperty;
            }
        }

        /// auto generated
        public AAccountPropertyCodeTable AAccountPropertyCode
        {
            get
            {
                return this.TableAAccountPropertyCode;
            }
        }

        /// auto generated
        public AAnalysisAttributeTable AAnalysisAttribute
        {
            get
            {
                return this.TableAAnalysisAttribute;
            }
        }

        /// auto generated
        public AAnalysisStoreTableTable AAnalysisStoreTable
        {
            get
            {
                return this.TableAAnalysisStoreTable;
            }
        }

        /// auto generated
        public AAnalysisTypeTable AAnalysisType
        {
            get
            {
                return this.TableAAnalysisType;
            }
        }

        /// auto generated
        public AFreeformAnalysisTable AFreeformAnalysis
        {
            get
            {
                return this.TableAFreeformAnalysis;
            }
        }

        /// auto generated
        public ABudgetTable ABudget
        {
            get
            {
                return this.TableABudget;
            }
        }

        /// auto generated
        public ABudgetPeriodTable ABudgetPeriod
        {
            get
            {
                return this.TableABudgetPeriod;
            }
        }

        /// auto generated
        public ABudgetRevisionTable ABudgetRevision
        {
            get
            {
                return this.TableABudgetRevision;
            }
        }

        /// auto generated
        public ABudgetTypeTable ABudgetType
        {
            get
            {
                return this.TableABudgetType;
            }
        }

        /// auto generated
        public ACostCentreTypesTable ACostCentreTypes
        {
            get
            {
                return this.TableACostCentreTypes;
            }
        }

        /// auto generated
        public ACostCentreTable ACostCentre
        {
            get
            {
                return this.TableACostCentre;
            }
        }

        /// auto generated
        public AGeneralLedgerMasterTable AGeneralLedgerMaster
        {
            get
            {
                return this.TableAGeneralLedgerMaster;
            }
        }

        /// auto generated
        public AGeneralLedgerMasterPeriodTable AGeneralLedgerMasterPeriod
        {
            get
            {
                return this.TableAGeneralLedgerMasterPeriod;
            }
        }

        /// auto generated
        public AMotivationGroupTable AMotivationGroup
        {
            get
            {
                return this.TableAMotivationGroup;
            }
        }

        /// auto generated
        public AMotivationDetailTable AMotivationDetail
        {
            get
            {
                return this.TableAMotivationDetail;
            }
        }

        /// auto generated
        public SModuleTable SModule
        {
            get
            {
                return this.TableSModule;
            }
        }

        /// auto generated
        public SSystemDefaultsTable SSystemDefaults
        {
            get
            {
                return this.TableSSystemDefaults;
            }
        }

        /// auto generated
        public PUnitTable PUnit
        {
            get
            {
                return this.TablePUnit;
            }
        }

        /// auto generated
        public PLocationTable PLocation
        {
            get
            {
                return this.TablePLocation;
            }
        }

        /// auto generated
        public PPartnerTypeTable PPartnerType
        {
            get
            {
                return this.TablePPartnerType;
            }
        }

        /// auto generated
        public PPartnerLocationTable PPartnerLocation
        {
            get
            {
                return this.TablePPartnerLocation;
            }
        }

        /// auto generated
        public PPartnerLedgerTable PPartnerLedger
        {
            get
            {
                return this.TablePPartnerLedger;
            }
        }

        /// auto generated
        public new virtual GLSetupTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((GLSetupTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new ALedgerTable("ALedger"));
            this.Tables.Add(new ALedgerInitFlagTable("ALedgerInitFlag"));
            this.Tables.Add(new AAccountingSystemParameterTable("AAccountingSystemParameter"));
            this.Tables.Add(new AAccountingPeriodTable("AAccountingPeriod"));
            this.Tables.Add(new ASystemInterfaceTable("ASystemInterface"));
            this.Tables.Add(new ATransactionTypeTable("ATransactionType"));
            this.Tables.Add(new AValidLedgerNumberTable("AValidLedgerNumber"));
            this.Tables.Add(new GLSetupTDSAAccountTable("AAccount"));
            this.Tables.Add(new AAccountHierarchyTable("AAccountHierarchy"));
            this.Tables.Add(new AAccountHierarchyDetailTable("AAccountHierarchyDetail"));
            this.Tables.Add(new AAccountPropertyTable("AAccountProperty"));
            this.Tables.Add(new AAccountPropertyCodeTable("AAccountPropertyCode"));
            this.Tables.Add(new AAnalysisAttributeTable("AAnalysisAttribute"));
            this.Tables.Add(new AAnalysisStoreTableTable("AAnalysisStoreTable"));
            this.Tables.Add(new AAnalysisTypeTable("AAnalysisType"));
            this.Tables.Add(new AFreeformAnalysisTable("AFreeformAnalysis"));
            this.Tables.Add(new ABudgetTable("ABudget"));
            this.Tables.Add(new ABudgetPeriodTable("ABudgetPeriod"));
            this.Tables.Add(new ABudgetRevisionTable("ABudgetRevision"));
            this.Tables.Add(new ABudgetTypeTable("ABudgetType"));
            this.Tables.Add(new ACostCentreTypesTable("ACostCentreTypes"));
            this.Tables.Add(new ACostCentreTable("ACostCentre"));
            this.Tables.Add(new AGeneralLedgerMasterTable("AGeneralLedgerMaster"));
            this.Tables.Add(new AGeneralLedgerMasterPeriodTable("AGeneralLedgerMasterPeriod"));
            this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
            this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
            this.Tables.Add(new SModuleTable("SModule"));
            this.Tables.Add(new SSystemDefaultsTable("SSystemDefaults"));
            this.Tables.Add(new PUnitTable("PUnit"));
            this.Tables.Add(new PLocationTable("PLocation"));
            this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            this.Tables.Add(new PPartnerLedgerTable("PPartnerLedger"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("ALedger") != -1))
            {
                this.Tables.Add(new ALedgerTable("ALedger"));
            }
            if ((ds.Tables.IndexOf("ALedgerInitFlag") != -1))
            {
                this.Tables.Add(new ALedgerInitFlagTable("ALedgerInitFlag"));
            }
            if ((ds.Tables.IndexOf("AAccountingSystemParameter") != -1))
            {
                this.Tables.Add(new AAccountingSystemParameterTable("AAccountingSystemParameter"));
            }
            if ((ds.Tables.IndexOf("AAccountingPeriod") != -1))
            {
                this.Tables.Add(new AAccountingPeriodTable("AAccountingPeriod"));
            }
            if ((ds.Tables.IndexOf("ASystemInterface") != -1))
            {
                this.Tables.Add(new ASystemInterfaceTable("ASystemInterface"));
            }
            if ((ds.Tables.IndexOf("ATransactionType") != -1))
            {
                this.Tables.Add(new ATransactionTypeTable("ATransactionType"));
            }
            if ((ds.Tables.IndexOf("AValidLedgerNumber") != -1))
            {
                this.Tables.Add(new AValidLedgerNumberTable("AValidLedgerNumber"));
            }
            if ((ds.Tables.IndexOf("AAccount") != -1))
            {
                this.Tables.Add(new GLSetupTDSAAccountTable("AAccount"));
            }
            if ((ds.Tables.IndexOf("AAccountHierarchy") != -1))
            {
                this.Tables.Add(new AAccountHierarchyTable("AAccountHierarchy"));
            }
            if ((ds.Tables.IndexOf("AAccountHierarchyDetail") != -1))
            {
                this.Tables.Add(new AAccountHierarchyDetailTable("AAccountHierarchyDetail"));
            }
            if ((ds.Tables.IndexOf("AAccountProperty") != -1))
            {
                this.Tables.Add(new AAccountPropertyTable("AAccountProperty"));
            }
            if ((ds.Tables.IndexOf("AAccountPropertyCode") != -1))
            {
                this.Tables.Add(new AAccountPropertyCodeTable("AAccountPropertyCode"));
            }
            if ((ds.Tables.IndexOf("AAnalysisAttribute") != -1))
            {
                this.Tables.Add(new AAnalysisAttributeTable("AAnalysisAttribute"));
            }
            if ((ds.Tables.IndexOf("AAnalysisStoreTable") != -1))
            {
                this.Tables.Add(new AAnalysisStoreTableTable("AAnalysisStoreTable"));
            }
            if ((ds.Tables.IndexOf("AAnalysisType") != -1))
            {
                this.Tables.Add(new AAnalysisTypeTable("AAnalysisType"));
            }
            if ((ds.Tables.IndexOf("AFreeformAnalysis") != -1))
            {
                this.Tables.Add(new AFreeformAnalysisTable("AFreeformAnalysis"));
            }
            if ((ds.Tables.IndexOf("ABudget") != -1))
            {
                this.Tables.Add(new ABudgetTable("ABudget"));
            }
            if ((ds.Tables.IndexOf("ABudgetPeriod") != -1))
            {
                this.Tables.Add(new ABudgetPeriodTable("ABudgetPeriod"));
            }
            if ((ds.Tables.IndexOf("ABudgetRevision") != -1))
            {
                this.Tables.Add(new ABudgetRevisionTable("ABudgetRevision"));
            }
            if ((ds.Tables.IndexOf("ABudgetType") != -1))
            {
                this.Tables.Add(new ABudgetTypeTable("ABudgetType"));
            }
            if ((ds.Tables.IndexOf("ACostCentreTypes") != -1))
            {
                this.Tables.Add(new ACostCentreTypesTable("ACostCentreTypes"));
            }
            if ((ds.Tables.IndexOf("ACostCentre") != -1))
            {
                this.Tables.Add(new ACostCentreTable("ACostCentre"));
            }
            if ((ds.Tables.IndexOf("AGeneralLedgerMaster") != -1))
            {
                this.Tables.Add(new AGeneralLedgerMasterTable("AGeneralLedgerMaster"));
            }
            if ((ds.Tables.IndexOf("AGeneralLedgerMasterPeriod") != -1))
            {
                this.Tables.Add(new AGeneralLedgerMasterPeriodTable("AGeneralLedgerMasterPeriod"));
            }
            if ((ds.Tables.IndexOf("AMotivationGroup") != -1))
            {
                this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
            }
            if ((ds.Tables.IndexOf("AMotivationDetail") != -1))
            {
                this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
            }
            if ((ds.Tables.IndexOf("SModule") != -1))
            {
                this.Tables.Add(new SModuleTable("SModule"));
            }
            if ((ds.Tables.IndexOf("SSystemDefaults") != -1))
            {
                this.Tables.Add(new SSystemDefaultsTable("SSystemDefaults"));
            }
            if ((ds.Tables.IndexOf("PUnit") != -1))
            {
                this.Tables.Add(new PUnitTable("PUnit"));
            }
            if ((ds.Tables.IndexOf("PLocation") != -1))
            {
                this.Tables.Add(new PLocationTable("PLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerType") != -1))
            {
                this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            }
            if ((ds.Tables.IndexOf("PPartnerLocation") != -1))
            {
                this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerLedger") != -1))
            {
                this.Tables.Add(new PPartnerLedgerTable("PPartnerLedger"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePPartner != null))
            {
                this.TablePPartner.InitVars();
            }
            if ((this.TableALedger != null))
            {
                this.TableALedger.InitVars();
            }
            if ((this.TableALedgerInitFlag != null))
            {
                this.TableALedgerInitFlag.InitVars();
            }
            if ((this.TableAAccountingSystemParameter != null))
            {
                this.TableAAccountingSystemParameter.InitVars();
            }
            if ((this.TableAAccountingPeriod != null))
            {
                this.TableAAccountingPeriod.InitVars();
            }
            if ((this.TableASystemInterface != null))
            {
                this.TableASystemInterface.InitVars();
            }
            if ((this.TableATransactionType != null))
            {
                this.TableATransactionType.InitVars();
            }
            if ((this.TableAValidLedgerNumber != null))
            {
                this.TableAValidLedgerNumber.InitVars();
            }
            if ((this.TableAAccount != null))
            {
                this.TableAAccount.InitVars();
            }
            if ((this.TableAAccountHierarchy != null))
            {
                this.TableAAccountHierarchy.InitVars();
            }
            if ((this.TableAAccountHierarchyDetail != null))
            {
                this.TableAAccountHierarchyDetail.InitVars();
            }
            if ((this.TableAAccountProperty != null))
            {
                this.TableAAccountProperty.InitVars();
            }
            if ((this.TableAAccountPropertyCode != null))
            {
                this.TableAAccountPropertyCode.InitVars();
            }
            if ((this.TableAAnalysisAttribute != null))
            {
                this.TableAAnalysisAttribute.InitVars();
            }
            if ((this.TableAAnalysisStoreTable != null))
            {
                this.TableAAnalysisStoreTable.InitVars();
            }
            if ((this.TableAAnalysisType != null))
            {
                this.TableAAnalysisType.InitVars();
            }
            if ((this.TableAFreeformAnalysis != null))
            {
                this.TableAFreeformAnalysis.InitVars();
            }
            if ((this.TableABudget != null))
            {
                this.TableABudget.InitVars();
            }
            if ((this.TableABudgetPeriod != null))
            {
                this.TableABudgetPeriod.InitVars();
            }
            if ((this.TableABudgetRevision != null))
            {
                this.TableABudgetRevision.InitVars();
            }
            if ((this.TableABudgetType != null))
            {
                this.TableABudgetType.InitVars();
            }
            if ((this.TableACostCentreTypes != null))
            {
                this.TableACostCentreTypes.InitVars();
            }
            if ((this.TableACostCentre != null))
            {
                this.TableACostCentre.InitVars();
            }
            if ((this.TableAGeneralLedgerMaster != null))
            {
                this.TableAGeneralLedgerMaster.InitVars();
            }
            if ((this.TableAGeneralLedgerMasterPeriod != null))
            {
                this.TableAGeneralLedgerMasterPeriod.InitVars();
            }
            if ((this.TableAMotivationGroup != null))
            {
                this.TableAMotivationGroup.InitVars();
            }
            if ((this.TableAMotivationDetail != null))
            {
                this.TableAMotivationDetail.InitVars();
            }
            if ((this.TableSModule != null))
            {
                this.TableSModule.InitVars();
            }
            if ((this.TableSSystemDefaults != null))
            {
                this.TableSSystemDefaults.InitVars();
            }
            if ((this.TablePUnit != null))
            {
                this.TablePUnit.InitVars();
            }
            if ((this.TablePLocation != null))
            {
                this.TablePLocation.InitVars();
            }
            if ((this.TablePPartnerType != null))
            {
                this.TablePPartnerType.InitVars();
            }
            if ((this.TablePPartnerLocation != null))
            {
                this.TablePPartnerLocation.InitVars();
            }
            if ((this.TablePPartnerLedger != null))
            {
                this.TablePPartnerLedger.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "GLSetupTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TableALedger = ((ALedgerTable)(this.Tables["ALedger"]));
            this.TableALedgerInitFlag = ((ALedgerInitFlagTable)(this.Tables["ALedgerInitFlag"]));
            this.TableAAccountingSystemParameter = ((AAccountingSystemParameterTable)(this.Tables["AAccountingSystemParameter"]));
            this.TableAAccountingPeriod = ((AAccountingPeriodTable)(this.Tables["AAccountingPeriod"]));
            this.TableASystemInterface = ((ASystemInterfaceTable)(this.Tables["ASystemInterface"]));
            this.TableATransactionType = ((ATransactionTypeTable)(this.Tables["ATransactionType"]));
            this.TableAValidLedgerNumber = ((AValidLedgerNumberTable)(this.Tables["AValidLedgerNumber"]));
            this.TableAAccount = ((GLSetupTDSAAccountTable)(this.Tables["AAccount"]));
            this.TableAAccountHierarchy = ((AAccountHierarchyTable)(this.Tables["AAccountHierarchy"]));
            this.TableAAccountHierarchyDetail = ((AAccountHierarchyDetailTable)(this.Tables["AAccountHierarchyDetail"]));
            this.TableAAccountProperty = ((AAccountPropertyTable)(this.Tables["AAccountProperty"]));
            this.TableAAccountPropertyCode = ((AAccountPropertyCodeTable)(this.Tables["AAccountPropertyCode"]));
            this.TableAAnalysisAttribute = ((AAnalysisAttributeTable)(this.Tables["AAnalysisAttribute"]));
            this.TableAAnalysisStoreTable = ((AAnalysisStoreTableTable)(this.Tables["AAnalysisStoreTable"]));
            this.TableAAnalysisType = ((AAnalysisTypeTable)(this.Tables["AAnalysisType"]));
            this.TableAFreeformAnalysis = ((AFreeformAnalysisTable)(this.Tables["AFreeformAnalysis"]));
            this.TableABudget = ((ABudgetTable)(this.Tables["ABudget"]));
            this.TableABudgetPeriod = ((ABudgetPeriodTable)(this.Tables["ABudgetPeriod"]));
            this.TableABudgetRevision = ((ABudgetRevisionTable)(this.Tables["ABudgetRevision"]));
            this.TableABudgetType = ((ABudgetTypeTable)(this.Tables["ABudgetType"]));
            this.TableACostCentreTypes = ((ACostCentreTypesTable)(this.Tables["ACostCentreTypes"]));
            this.TableACostCentre = ((ACostCentreTable)(this.Tables["ACostCentre"]));
            this.TableAGeneralLedgerMaster = ((AGeneralLedgerMasterTable)(this.Tables["AGeneralLedgerMaster"]));
            this.TableAGeneralLedgerMasterPeriod = ((AGeneralLedgerMasterPeriodTable)(this.Tables["AGeneralLedgerMasterPeriod"]));
            this.TableAMotivationGroup = ((AMotivationGroupTable)(this.Tables["AMotivationGroup"]));
            this.TableAMotivationDetail = ((AMotivationDetailTable)(this.Tables["AMotivationDetail"]));
            this.TableSModule = ((SModuleTable)(this.Tables["SModule"]));
            this.TableSSystemDefaults = ((SSystemDefaultsTable)(this.Tables["SSystemDefaults"]));
            this.TablePUnit = ((PUnitTable)(this.Tables["PUnit"]));
            this.TablePLocation = ((PLocationTable)(this.Tables["PLocation"]));
            this.TablePPartnerType = ((PPartnerTypeTable)(this.Tables["PPartnerType"]));
            this.TablePPartnerLocation = ((PPartnerLocationTable)(this.Tables["PPartnerLocation"]));
            this.TablePPartnerLedger = ((PPartnerLedgerTable)(this.Tables["PPartnerLedger"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableALedger != null)
                        && (this.TableAAccount != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccount1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AAccount", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableABudgetType != null)
                        && (this.TableAAccount != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccount2", "ABudgetType", new string[] {
                                "a_budget_type_code_c"}, "AAccount", new string[] {
                                "a_budget_type_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountHierarchy != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchy1", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountHierarchy", new string[] {
                                "a_ledger_number_i", "a_root_account_code_c"}));
            }
            if (((this.TableAAccountHierarchy != null)
                        && (this.TableAAccountHierarchyDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchyDetail1", "AAccountHierarchy", new string[] {
                                "a_ledger_number_i", "a_account_hierarchy_code_c"}, "AAccountHierarchyDetail", new string[] {
                                "a_ledger_number_i", "a_account_hierarchy_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountHierarchyDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchyDetail2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountHierarchyDetail", new string[] {
                                "a_ledger_number_i", "a_reporting_account_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountHierarchyDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountHierarchyDetail3", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountHierarchyDetail", new string[] {
                                "a_ledger_number_i", "a_account_code_to_report_to_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAAccountProperty != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountProperty1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AAccountProperty", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAccountProperty != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountProperty2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAccountProperty", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableAAccountPropertyCode != null)
                        && (this.TableAAccountProperty != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountProperty3", "AAccountPropertyCode", new string[] {
                                "a_property_code_c"}, "AAccountProperty", new string[] {
                                "a_property_code_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAAccountingPeriod != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountingPeriod1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AAccountingPeriod", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAAccountingSystemParameter != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAccountingSystemParam1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AAccountingSystemParameter", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAAnalysisAttribute != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAnalysisAttribute1", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AAnalysisAttribute", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableAAnalysisType != null)
                        && (this.TableAAnalysisAttribute != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAnalysisAttribute2", "AAnalysisType", new string[] {
                                "a_analysis_type_code_c"}, "AAnalysisAttribute", new string[] {
                                "a_analysis_type_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAAnalysisAttribute != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKAnalysisAttribute3", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AAnalysisAttribute", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableABudget != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBudget1", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "ABudget", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableABudget != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBudget2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "ABudget", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableABudgetType != null)
                        && (this.TableABudget != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBudget3", "ABudgetType", new string[] {
                                "a_budget_type_code_c"}, "ABudget", new string[] {
                                "a_budget_type_code_c"}));
            }
            if (((this.TableABudgetRevision != null)
                        && (this.TableABudget != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBudget4", "ABudgetRevision", new string[] {
                                "a_ledger_number_i", "a_year_i", "a_revision_i"}, "ABudget", new string[] {
                                "a_ledger_number_i", "a_year_i", "a_revision_i"}));
            }
            if (((this.TableABudget != null)
                        && (this.TableABudgetPeriod != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBudgetPeriod1", "ABudget", new string[] {
                                "a_budget_sequence_i"}, "ABudgetPeriod", new string[] {
                                "a_budget_sequence_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableACostCentre != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKCostCentre1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ACostCentre", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableACostCentreTypes != null)
                        && (this.TableACostCentre != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKCostCentre2", "ACostCentreTypes", new string[] {
                                "a_ledger_number_i", "a_cost_centre_type_c"}, "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_type_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableACostCentreTypes != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKCostCentreTypes1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ACostCentreTypes", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAFreeformAnalysis != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFreeformAnalysis1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AFreeformAnalysis", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAnalysisType != null)
                        && (this.TableAFreeformAnalysis != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFreeformAnalysis2", "AAnalysisType", new string[] {
                                "a_analysis_type_code_c"}, "AFreeformAnalysis", new string[] {
                                "a_analysis_type_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAGeneralLedgerMaster != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralLedgerMaster1", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AGeneralLedgerMaster", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAGeneralLedgerMaster != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralLedgerMaster2", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AGeneralLedgerMaster", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TableAGeneralLedgerMaster != null)
                        && (this.TableAGeneralLedgerMasterPeriod != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGlmPeriod1", "AGeneralLedgerMaster", new string[] {
                                "a_glm_sequence_i"}, "AGeneralLedgerMasterPeriod", new string[] {
                                "a_glm_sequence_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableALedgerInitFlag != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKLedgerInitFlag1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ALedgerInitFlag", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAMotivationGroup != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail1", "AMotivationGroup", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c"}, "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail3", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail4", "PPartner", new string[] {
                                "p_partner_key_n"}, "AMotivationDetail", new string[] {
                                "p_recipient_key_n"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAMotivationGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationGroup1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AMotivationGroup", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableASystemInterface != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKSystemInterface1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "ASystemInterface", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableATransactionType != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransactionType1", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "ATransactionType", new string[] {
                                "a_ledger_number_i", "a_debit_account_code_c"}));
            }
            if (((this.TableAAccount != null)
                        && (this.TableATransactionType != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransactionType2", "AAccount", new string[] {
                                "a_ledger_number_i", "a_account_code_c"}, "ATransactionType", new string[] {
                                "a_ledger_number_i", "a_credit_account_code_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAValidLedgerNumber != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKValidLedgerNumber1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AValidLedgerNumber", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TableAValidLedgerNumber != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKValidLedgerNumber2", "PPartner", new string[] {
                                "p_partner_key_n"}, "AValidLedgerNumber", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TableAValidLedgerNumber != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKValidLedgerNumber3", "PPartner", new string[] {
                                "p_partner_key_n"}, "AValidLedgerNumber", new string[] {
                                "a_ilt_processing_centre_n"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAValidLedgerNumber != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKValidLedgerNumber4", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AValidLedgerNumber", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePPartnerLedger != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLedger1", "PUnit", new string[] {
                                "p_partner_key_n"}, "PPartnerLedger", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePPartnerLocation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLocation1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerLocation", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePLocation != null)
                        && (this.TablePPartnerLocation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLocation2", "PLocation", new string[] {
                                "p_site_key_n", "p_location_key_i"}, "PPartnerLocation", new string[] {
                                "p_site_key_n", "p_location_key_i"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePPartnerType != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerType3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerType", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePUnit != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnit1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PUnit", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePUnit != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnit7", "PPartner", new string[] {
                                "p_partner_key_n"}, "PUnit", new string[] {
                                "p_primary_office_n"}));
            }
        }
    }

    /// Details about each account code within a ledger. Also holds information on the summary account structure for reporting.
    [Serializable()]
    public class GLSetupTDSAAccountTable : AAccountTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 111;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountFlagId = 26;

        /// constructor
        public GLSetupTDSAAccountTable() :
                base("AAccount")
        {
        }

        /// constructor
        public GLSetupTDSAAccountTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public GLSetupTDSAAccountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnBankAccountFlag;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_long_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_short_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_eng_account_code_short_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_eng_account_code_long_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_debit_credit_indicator_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_account_active_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_analysis_attribute_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_standard_account_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_consolidation_account_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_intercompany_account_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_budget_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_posting_status_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_system_account_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_budget_control_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_valid_cc_combo_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_foreign_currency_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_foreign_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_banking_details_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("BankAccountFlag", typeof(bool)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnAccountType = this.Columns["a_account_type_c"];
            this.ColumnAccountCodeLongDesc = this.Columns["a_account_code_long_desc_c"];
            this.ColumnAccountCodeShortDesc = this.Columns["a_account_code_short_desc_c"];
            this.ColumnEngAccountCodeShortDesc = this.Columns["a_eng_account_code_short_desc_c"];
            this.ColumnEngAccountCodeLongDesc = this.Columns["a_eng_account_code_long_desc_c"];
            this.ColumnDebitCreditIndicator = this.Columns["a_debit_credit_indicator_l"];
            this.ColumnAccountActiveFlag = this.Columns["a_account_active_flag_l"];
            this.ColumnAnalysisAttributeFlag = this.Columns["a_analysis_attribute_flag_l"];
            this.ColumnStandardAccountFlag = this.Columns["a_standard_account_flag_l"];
            this.ColumnConsolidationAccountFlag = this.Columns["a_consolidation_account_flag_l"];
            this.ColumnIntercompanyAccountFlag = this.Columns["a_intercompany_account_flag_l"];
            this.ColumnBudgetTypeCode = this.Columns["a_budget_type_code_c"];
            this.ColumnPostingStatus = this.Columns["a_posting_status_l"];
            this.ColumnSystemAccountFlag = this.Columns["a_system_account_flag_l"];
            this.ColumnBudgetControlFlag = this.Columns["a_budget_control_flag_l"];
            this.ColumnValidCcCombo = this.Columns["a_valid_cc_combo_c"];
            this.ColumnForeignCurrencyFlag = this.Columns["a_foreign_currency_flag_l"];
            this.ColumnForeignCurrencyCode = this.Columns["a_foreign_currency_code_c"];
            this.ColumnBankingDetailsKey = this.Columns["p_banking_details_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnBankAccountFlag = this.Columns["BankAccountFlag"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnLedgerNumber,ColumnAccountCode};
        }

        /// Access a typed row by index
        public new GLSetupTDSAAccountRow this[int i]
        {
            get
            {
                return ((GLSetupTDSAAccountRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new GLSetupTDSAAccountRow NewRowTyped(bool AWithDefaultValues)
        {
            GLSetupTDSAAccountRow ret = ((GLSetupTDSAAccountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new GLSetupTDSAAccountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new GLSetupTDSAAccountRow(builder);
        }

        /// get typed set of changes
        public new GLSetupTDSAAccountTable GetChangesTyped()
        {
            return ((GLSetupTDSAAccountTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AAccount";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_account";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountFlagDBName()
        {
            return "BankAccountFlag";
        }

        /// get character length for column
        public static short GetBankAccountFlagLength()
        {
            return -1;
        }

    }

    /// Details about each account code within a ledger. Also holds information on the summary account structure for reporting.
    [Serializable()]
    public class GLSetupTDSAAccountRow : AAccountRow
    {
        private GLSetupTDSAAccountTable myTable;

        /// Constructor
        public GLSetupTDSAAccountRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((GLSetupTDSAAccountTable)(this.Table));
        }

        ///
        public bool BankAccountFlag
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccountFlag.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccountFlag)
                            || (((bool)(this[this.myTable.ColumnBankAccountFlag])) != value)))
                {
                    this[this.myTable.ColumnBankAccountFlag] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnAccountType);
            this.SetNull(this.myTable.ColumnAccountCodeLongDesc);
            this.SetNull(this.myTable.ColumnAccountCodeShortDesc);
            this.SetNull(this.myTable.ColumnEngAccountCodeShortDesc);
            this.SetNull(this.myTable.ColumnEngAccountCodeLongDesc);
            this[this.myTable.ColumnDebitCreditIndicator.Ordinal] = true;
            this[this.myTable.ColumnAccountActiveFlag.Ordinal] = true;
            this[this.myTable.ColumnAnalysisAttributeFlag.Ordinal] = false;
            this[this.myTable.ColumnStandardAccountFlag.Ordinal] = false;
            this[this.myTable.ColumnConsolidationAccountFlag.Ordinal] = false;
            this[this.myTable.ColumnIntercompanyAccountFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnBudgetTypeCode);
            this[this.myTable.ColumnPostingStatus.Ordinal] = true;
            this[this.myTable.ColumnSystemAccountFlag.Ordinal] = false;
            this[this.myTable.ColumnBudgetControlFlag.Ordinal] = false;
            this[this.myTable.ColumnValidCcCombo.Ordinal] = "All";
            this[this.myTable.ColumnForeignCurrencyFlag.Ordinal] = false;
            this.SetNull(this.myTable.ColumnForeignCurrencyCode);
            this.SetNull(this.myTable.ColumnBankingDetailsKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnBankAccountFlag);
        }

        /// test for NULL value
        public bool IsBankAccountFlagNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccountFlag);
        }

        /// assign NULL value
        public void SetBankAccountFlagNull()
        {
            this.SetNull(this.myTable.ColumnBankAccountFlag);
        }
    }
}
