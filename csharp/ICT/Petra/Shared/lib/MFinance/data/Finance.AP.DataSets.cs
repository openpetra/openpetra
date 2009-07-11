/* Auto generated with nant generateORM
 * based on Finance.AP.TypedDataSets.xml
 * Do not modify this file manually!
 */
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
        
        private AApDocumentTable TableAApDocument;
        
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
        public AApDocumentTable AApDocument
        {
            get
            {
                return this.TableAApDocument;
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
            this.Tables.Add(new AApDocumentTable("AApDocument"));
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
                this.Tables.Add(new AApDocumentTable("AApDocument"));
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
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "AccountsPayableTDS";
            this.TableAApSupplier = ((AApSupplierTable)(this.Tables["AApSupplier"]));
            this.TableAApDocument = ((AApDocumentTable)(this.Tables["AApDocument"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TableAApSupplier != null) 
                        && (this.TableAApDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocument2", "AApSupplier", new string[] {
                                "p_partner_key_n"}, "AApDocument", new string[] {
                                "p_partner_key_n"}));
            }
        }
    }
}
