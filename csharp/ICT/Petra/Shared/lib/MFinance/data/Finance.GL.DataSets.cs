/* Auto generated with nant generateORM
 * based on Finance.GL.TypedDataSets.xml
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MFinance.GL.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MFinance.Account.Data;
    
    
    /// auto generated table derived from ATransaction
    [Serializable()]
    public class GLBatchTDSATransactionTable : ATransactionTable
    {
        
        /// 
        public DataColumn ColumnDateEntered;
        
        /// 
        public DataColumn ColumnAnalysisAttributes;
        
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
        
        /// Access a typed row by index
        public new GLBatchTDSATransactionRow this[int i]
        {
            get
            {
                return ((GLBatchTDSATransactionRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateEnteredDBName()
        {
            return "DateEntered";
        }
        
        /// get help text for column
        public static string GetDateEnteredHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetDateEnteredLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnalysisAttributesDBName()
        {
            return "AnalysisAttributes";
        }
        
        /// get help text for column
        public static string GetAnalysisAttributesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnalysisAttributesLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public new static string GetTableName()
        {
            return "ATransaction";
        }
        
        /// original name of table in the database
        public new static string GetTableDBName()
        {
            return "ATransaction";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            base.InitVars();
            this.ColumnDateEntered = this.Columns["DateEntered"];
            this.ColumnAnalysisAttributes = this.Columns["AnalysisAttributes"];
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
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new GLBatchTDSATransactionRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            base.InitClass();
            this.Columns.Add(new System.Data.DataColumn("DateEntered", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("AnalysisAttributes", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnDateEntered))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnAnalysisAttributes))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return base.CreateOdbcParameter(ACol);
        }
    }
    
    /// DerivedRow from ATransactionRow
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
        
        /// 
        public String AnalysisAttributes
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
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnalysisAttributes) 
                            || (((String)(this[this.myTable.ColumnAnalysisAttributes])) != value)))
                {
                    this[this.myTable.ColumnAnalysisAttributes] = value;
                }
            }
        }
        
        /// set default values
        public override void InitValues()
        {
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
                                "a_ledger_number_i",
                                "a_batch_number_i"}, "AJournal", new string[] {
                                "a_ledger_number_i",
                                "a_batch_number_i"}));
            }
            if (((this.TableAJournal != null) 
                        && (this.TableATransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKTransaction1", "AJournal", new string[] {
                                "a_ledger_number_i",
                                "a_batch_number_i",
                                "a_journal_number_i"}, "ATransaction", new string[] {
                                "a_ledger_number_i",
                                "a_batch_number_i",
                                "a_journal_number_i"}));
            }
        }
    }
}
