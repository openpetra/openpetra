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
        public new virtual AccountsPayableTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((AccountsPayableTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new AApSupplierTable("AApSupplier"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AApSupplier") != -1))
            {
                this.Tables.Add(new AApSupplierTable("AApSupplier"));
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
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "AccountsPayableTDS";
            this.TableAApSupplier = ((AApSupplierTable)(this.Tables["AApSupplier"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
        }
    }
}
