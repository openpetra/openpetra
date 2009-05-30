/* Auto generated with nant generateORM
 * based on PartnerTypedDataSets.xml
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MPartner.Partner.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MPartner.Mailroom.Data;
    using Ict.Petra.Shared.MPersonnel.Personnel.Data;
    
    
    /// auto generated table derived from PPartnerLocation
    [Serializable()]
    public class PartnerEditTDSPPartnerLocationTable : PPartnerLocationTable
    {
        
        /// 
        public DataColumn ColumnBestAddress;
        
        /// 
        public DataColumn ColumnIcon;
        
        /// constructor
        public PartnerEditTDSPPartnerLocationTable() : 
                base("PPartnerLocation")
        {
        }
        
        /// constructor
        public PartnerEditTDSPPartnerLocationTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSPPartnerLocationTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public new PartnerEditTDSPPartnerLocationRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPPartnerLocationRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetBestAddressDBName()
        {
            return "BestAddress";
        }
        
        /// get help text for column
        public static string GetBestAddressHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetBestAddressLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetIconDBName()
        {
            return "Icon";
        }
        
        /// get help text for column
        public static string GetIconHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetIconLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public new static string GetTableName()
        {
            return "PPartnerLocation";
        }
        
        /// original name of table in the database
        public new static string GetTableDBName()
        {
            return "PPartnerLocation";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            base.InitVars();
            this.ColumnBestAddress = this.Columns["BestAddress"];
            this.ColumnIcon = this.Columns["Icon"];
        }
        
        /// create a new typed row
        public new PartnerEditTDSPPartnerLocationRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSPPartnerLocationRow ret = ((PartnerEditTDSPPartnerLocationRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPPartnerLocationRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            base.InitClass();
            this.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnBestAddress))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnIcon))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return base.CreateOdbcParameter(ACol);
        }
    }
    
    /// DerivedRow from PPartnerLocationRow
    [Serializable()]
    public class PartnerEditTDSPPartnerLocationRow : PPartnerLocationRow
    {
        
        private PartnerEditTDSPPartnerLocationTable myTable;
        
        /// Constructor
        public PartnerEditTDSPPartnerLocationRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSPPartnerLocationTable)(this.Table));
        }
        
        /// 
        public Boolean BestAddress
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBestAddress.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBestAddress) 
                            || (((Boolean)(this[this.myTable.ColumnBestAddress])) != value)))
                {
                    this[this.myTable.ColumnBestAddress] = value;
                }
            }
        }
        
        /// 
        public Int32 Icon
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnIcon.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnIcon) 
                            || (((Int32)(this[this.myTable.ColumnIcon])) != value)))
                {
                    this[this.myTable.ColumnIcon] = value;
                }
            }
        }
        
        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnBestAddress);
            this.SetNull(this.myTable.ColumnIcon);
        }
        
        /// test for NULL value
        public bool IsBestAddressNull()
        {
            return this.IsNull(this.myTable.ColumnBestAddress);
        }
        
        /// assign NULL value
        public void SetBestAddressNull()
        {
            this.SetNull(this.myTable.ColumnBestAddress);
        }
        
        /// test for NULL value
        public bool IsIconNull()
        {
            return this.IsNull(this.myTable.ColumnIcon);
        }
        
        /// assign NULL value
        public void SetIconNull()
        {
            this.SetNull(this.myTable.ColumnIcon);
        }
    }
    
    /// auto generated table derived from PPerson
    [Serializable()]
    public class PartnerEditTDSPPersonTable : PPersonTable
    {
        
        /// 
        public DataColumn ColumnUnitName;
        
        /// constructor
        public PartnerEditTDSPPersonTable() : 
                base("PPerson")
        {
        }
        
        /// constructor
        public PartnerEditTDSPPersonTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSPPersonTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public new PartnerEditTDSPPersonRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPPersonRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetUnitNameDBName()
        {
            return "p_unit_name_c";
        }
        
        /// get help text for column
        public static string GetUnitNameHelp()
        {
            return "Enter the name of the unit";
        }
        
        /// get label of column
        public static string GetUnitNameLabel()
        {
            return "Unit Name";
        }
        
        /// CamelCase version of the tablename
        public new static string GetTableName()
        {
            return "PPerson";
        }
        
        /// original name of table in the database
        public new static string GetTableDBName()
        {
            return "PPerson";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            base.InitVars();
            this.ColumnUnitName = this.Columns["p_unit_name_c"];
        }
        
        /// create a new typed row
        public new PartnerEditTDSPPersonRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSPPersonRow ret = ((PartnerEditTDSPPersonRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPPersonRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            base.InitClass();
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnUnitName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            return base.CreateOdbcParameter(ACol);
        }
    }
    
    /// DerivedRow from PPersonRow
    [Serializable()]
    public class PartnerEditTDSPPersonRow : PPersonRow
    {
        
        private PartnerEditTDSPPersonTable myTable;
        
        /// Constructor
        public PartnerEditTDSPPersonRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSPPersonTable)(this.Table));
        }
        
        /// 
        public String UnitName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitName) 
                            || (((String)(this[this.myTable.ColumnUnitName])) != value)))
                {
                    this[this.myTable.ColumnUnitName] = value;
                }
            }
        }
        
        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnUnitName);
        }
        
        /// test for NULL value
        public bool IsUnitNameNull()
        {
            return this.IsNull(this.myTable.ColumnUnitName);
        }
        
        /// assign NULL value
        public void SetUnitNameNull()
        {
            this.SetNull(this.myTable.ColumnUnitName);
        }
    }
    
    /// auto generated table derived from PFamily
    [Serializable()]
    public class PartnerEditTDSPFamilyTable : PFamilyTable
    {
        
        /// 
        public DataColumn ColumnUnitName;
        
        /// constructor
        public PartnerEditTDSPFamilyTable() : 
                base("PFamily")
        {
        }
        
        /// constructor
        public PartnerEditTDSPFamilyTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSPFamilyTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public new PartnerEditTDSPFamilyRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPFamilyRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetUnitNameDBName()
        {
            return "p_unit_name_c";
        }
        
        /// get help text for column
        public static string GetUnitNameHelp()
        {
            return "Enter the name of the unit";
        }
        
        /// get label of column
        public static string GetUnitNameLabel()
        {
            return "Unit Name";
        }
        
        /// CamelCase version of the tablename
        public new static string GetTableName()
        {
            return "PFamily";
        }
        
        /// original name of table in the database
        public new static string GetTableDBName()
        {
            return "PFamily";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            base.InitVars();
            this.ColumnUnitName = this.Columns["p_unit_name_c"];
        }
        
        /// create a new typed row
        public new PartnerEditTDSPFamilyRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSPFamilyRow ret = ((PartnerEditTDSPFamilyRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPFamilyRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            base.InitClass();
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnUnitName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            return base.CreateOdbcParameter(ACol);
        }
    }
    
    /// DerivedRow from PFamilyRow
    [Serializable()]
    public class PartnerEditTDSPFamilyRow : PFamilyRow
    {
        
        private PartnerEditTDSPFamilyTable myTable;
        
        /// Constructor
        public PartnerEditTDSPFamilyRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSPFamilyTable)(this.Table));
        }
        
        /// 
        public String UnitName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUnitName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUnitName) 
                            || (((String)(this[this.myTable.ColumnUnitName])) != value)))
                {
                    this[this.myTable.ColumnUnitName] = value;
                }
            }
        }
        
        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnUnitName);
        }
        
        /// test for NULL value
        public bool IsUnitNameNull()
        {
            return this.IsNull(this.myTable.ColumnUnitName);
        }
        
        /// assign NULL value
        public void SetUnitNameNull()
        {
            this.SetNull(this.myTable.ColumnUnitName);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerEditTDSMiscellaneousDataTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// This is the key that tell what site created the linked location
        public DataColumn ColumnSelectedSiteKey;
        
        /// 
        public DataColumn ColumnSelectedLocationKey;
        
        /// 
        public DataColumn ColumnLastGiftDate;
        
        /// 
        public DataColumn ColumnLastGiftInfo;
        
        /// 
        public DataColumn ColumnLastContactDate;
        
        /// 
        public DataColumn ColumnItemsCountAddresses;
        
        /// 
        public DataColumn ColumnItemsCountAddressesActive;
        
        /// 
        public DataColumn ColumnItemsCountSubscriptions;
        
        /// 
        public DataColumn ColumnItemsCountSubscriptionsActive;
        
        /// 
        public DataColumn ColumnItemsCountPartnerTypes;
        
        /// 
        public DataColumn ColumnItemsCountFamilyMembers;
        
        /// 
        public DataColumn ColumnItemsCountInterests;
        
        /// 
        public DataColumn ColumnItemsCountReminders;
        
        /// 
        public DataColumn ColumnItemsCountRelationships;
        
        /// 
        public DataColumn ColumnItemsCountContacts;
        
        /// 
        public DataColumn ColumnOfficeSpecificDataLabelsAvailable;
        
        /// 
        public DataColumn ColumnFoundationOwner1Key;
        
        /// 
        public DataColumn ColumnFoundationOwner2Key;
        
        /// 
        public DataColumn ColumnHasEXWORKERPartnerType;
        
        /// auto generated
        public DataColumn[] FKPartnerLocation2;
        
        /// constructor
        public PartnerEditTDSMiscellaneousDataTable() : 
                base("MiscellaneousData")
        {
        }
        
        /// constructor
        public PartnerEditTDSMiscellaneousDataTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSMiscellaneousDataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerEditTDSMiscellaneousDataRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSMiscellaneousDataRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSelectedSiteKeyDBName()
        {
            return "p_site_key_n";
        }
        
        /// get help text for column
        public static string GetSelectedSiteKeyHelp()
        {
            return "Enter the site key";
        }
        
        /// get label of column
        public static string GetSelectedSiteKeyLabel()
        {
            return "Site Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSelectedLocationKeyDBName()
        {
            return "p_location_key_i";
        }
        
        /// get help text for column
        public static string GetSelectedLocationKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetSelectedLocationKeyLabel()
        {
            return "Location Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastGiftDateDBName()
        {
            return "LastGiftDate";
        }
        
        /// get help text for column
        public static string GetLastGiftDateHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLastGiftDateLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastGiftInfoDBName()
        {
            return "LastGiftInfo";
        }
        
        /// get help text for column
        public static string GetLastGiftInfoHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLastGiftInfoLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastContactDateDBName()
        {
            return "LastContactDate";
        }
        
        /// get help text for column
        public static string GetLastContactDateHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLastContactDateLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountAddressesDBName()
        {
            return "ItemsCountAddresses";
        }
        
        /// get help text for column
        public static string GetItemsCountAddressesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountAddressesLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountAddressesActiveDBName()
        {
            return "ItemsCountAddressesActive";
        }
        
        /// get help text for column
        public static string GetItemsCountAddressesActiveHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountAddressesActiveLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountSubscriptionsDBName()
        {
            return "ItemsCountSubscriptions";
        }
        
        /// get help text for column
        public static string GetItemsCountSubscriptionsHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountSubscriptionsLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountSubscriptionsActiveDBName()
        {
            return "ItemsCountSubscriptionsActive";
        }
        
        /// get help text for column
        public static string GetItemsCountSubscriptionsActiveHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountSubscriptionsActiveLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountPartnerTypesDBName()
        {
            return "ItemsCountPartnerTypes";
        }
        
        /// get help text for column
        public static string GetItemsCountPartnerTypesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountPartnerTypesLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountFamilyMembersDBName()
        {
            return "ItemsCountFamilyMembers";
        }
        
        /// get help text for column
        public static string GetItemsCountFamilyMembersHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountFamilyMembersLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountInterestsDBName()
        {
            return "ItemsCountInterests";
        }
        
        /// get help text for column
        public static string GetItemsCountInterestsHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountInterestsLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountRemindersDBName()
        {
            return "ItemsCountReminders";
        }
        
        /// get help text for column
        public static string GetItemsCountRemindersHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountRemindersLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountRelationshipsDBName()
        {
            return "ItemsCountRelationships";
        }
        
        /// get help text for column
        public static string GetItemsCountRelationshipsHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountRelationshipsLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetItemsCountContactsDBName()
        {
            return "ItemsCountContacts";
        }
        
        /// get help text for column
        public static string GetItemsCountContactsHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetItemsCountContactsLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetOfficeSpecificDataLabelsAvailableDBName()
        {
            return "OfficeSpecificDataLabelsAvailable";
        }
        
        /// get help text for column
        public static string GetOfficeSpecificDataLabelsAvailableHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetOfficeSpecificDataLabelsAvailableLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFoundationOwner1KeyDBName()
        {
            return "FoundationOwner1Key";
        }
        
        /// get help text for column
        public static string GetFoundationOwner1KeyHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetFoundationOwner1KeyLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFoundationOwner2KeyDBName()
        {
            return "FoundationOwner2Key";
        }
        
        /// get help text for column
        public static string GetFoundationOwner2KeyHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetFoundationOwner2KeyLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetHasEXWORKERPartnerTypeDBName()
        {
            return "HasEXWORKERPartnerType";
        }
        
        /// get help text for column
        public static string GetHasEXWORKERPartnerTypeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetHasEXWORKERPartnerTypeLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "MiscellaneousData";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "MiscellaneousData";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnSelectedSiteKey = this.Columns["p_site_key_n"];
            this.ColumnSelectedLocationKey = this.Columns["p_location_key_i"];
            this.ColumnLastGiftDate = this.Columns["LastGiftDate"];
            this.ColumnLastGiftInfo = this.Columns["LastGiftInfo"];
            this.ColumnLastContactDate = this.Columns["LastContactDate"];
            this.ColumnItemsCountAddresses = this.Columns["ItemsCountAddresses"];
            this.ColumnItemsCountAddressesActive = this.Columns["ItemsCountAddressesActive"];
            this.ColumnItemsCountSubscriptions = this.Columns["ItemsCountSubscriptions"];
            this.ColumnItemsCountSubscriptionsActive = this.Columns["ItemsCountSubscriptionsActive"];
            this.ColumnItemsCountPartnerTypes = this.Columns["ItemsCountPartnerTypes"];
            this.ColumnItemsCountFamilyMembers = this.Columns["ItemsCountFamilyMembers"];
            this.ColumnItemsCountInterests = this.Columns["ItemsCountInterests"];
            this.ColumnItemsCountReminders = this.Columns["ItemsCountReminders"];
            this.ColumnItemsCountRelationships = this.Columns["ItemsCountRelationships"];
            this.ColumnItemsCountContacts = this.Columns["ItemsCountContacts"];
            this.ColumnOfficeSpecificDataLabelsAvailable = this.Columns["OfficeSpecificDataLabelsAvailable"];
            this.ColumnFoundationOwner1Key = this.Columns["FoundationOwner1Key"];
            this.ColumnFoundationOwner2Key = this.Columns["FoundationOwner2Key"];
            this.ColumnHasEXWORKERPartnerType = this.Columns["HasEXWORKERPartnerType"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey};
            this.FKPartnerLocation2 = new System.Data.DataColumn[] {
                    this.ColumnSelectedSiteKey,
                    this.ColumnSelectedLocationKey};
        }
        
        /// create a new typed row
        public PartnerEditTDSMiscellaneousDataRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSMiscellaneousDataRow ret = ((PartnerEditTDSMiscellaneousDataRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSMiscellaneousDataRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("LastGiftDate", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("LastGiftInfo", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("LastContactDate", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountAddresses", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountAddressesActive", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountSubscriptions", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountSubscriptionsActive", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountPartnerTypes", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountFamilyMembers", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountInterests", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountReminders", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountRelationships", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ItemsCountContacts", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("OfficeSpecificDataLabelsAvailable", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("FoundationOwner1Key", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("FoundationOwner2Key", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("HasEXWORKERPartnerType", typeof(Boolean)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnSelectedSiteKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnSelectedLocationKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLastGiftDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLastGiftInfo))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLastContactDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountAddresses))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountAddressesActive))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountSubscriptions))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountSubscriptionsActive))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountPartnerTypes))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountFamilyMembers))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountInterests))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountReminders))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountRelationships))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnItemsCountContacts))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnOfficeSpecificDataLabelsAvailable))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnFoundationOwner1Key))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnFoundationOwner2Key))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnHasEXWORKERPartnerType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerEditTDSMiscellaneousDataRow : System.Data.DataRow
    {
        
        private PartnerEditTDSMiscellaneousDataTable myTable;
        
        /// Constructor
        public PartnerEditTDSMiscellaneousDataRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSMiscellaneousDataTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// This is the key that tell what site created the linked location
        public Int64 SelectedSiteKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSelectedSiteKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSelectedSiteKey) 
                            || (((Int64)(this[this.myTable.ColumnSelectedSiteKey])) != value)))
                {
                    this[this.myTable.ColumnSelectedSiteKey] = value;
                }
            }
        }
        
        /// 
        public Int32 SelectedLocationKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSelectedLocationKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSelectedLocationKey) 
                            || (((Int32)(this[this.myTable.ColumnSelectedLocationKey])) != value)))
                {
                    this[this.myTable.ColumnSelectedLocationKey] = value;
                }
            }
        }
        
        /// 
        public DateTime LastGiftDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastGiftDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastGiftDate) 
                            || (((DateTime)(this[this.myTable.ColumnLastGiftDate])) != value)))
                {
                    this[this.myTable.ColumnLastGiftDate] = value;
                }
            }
        }
        
        /// 
        public String LastGiftInfo
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastGiftInfo.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastGiftInfo) 
                            || (((String)(this[this.myTable.ColumnLastGiftInfo])) != value)))
                {
                    this[this.myTable.ColumnLastGiftInfo] = value;
                }
            }
        }
        
        /// 
        public DateTime LastContactDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastContactDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastContactDate) 
                            || (((DateTime)(this[this.myTable.ColumnLastContactDate])) != value)))
                {
                    this[this.myTable.ColumnLastContactDate] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountAddresses
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountAddresses.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountAddresses) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountAddresses])) != value)))
                {
                    this[this.myTable.ColumnItemsCountAddresses] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountAddressesActive
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountAddressesActive.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountAddressesActive) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountAddressesActive])) != value)))
                {
                    this[this.myTable.ColumnItemsCountAddressesActive] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountSubscriptions
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountSubscriptions.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountSubscriptions) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountSubscriptions])) != value)))
                {
                    this[this.myTable.ColumnItemsCountSubscriptions] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountSubscriptionsActive
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountSubscriptionsActive.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountSubscriptionsActive) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountSubscriptionsActive])) != value)))
                {
                    this[this.myTable.ColumnItemsCountSubscriptionsActive] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountPartnerTypes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountPartnerTypes.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountPartnerTypes) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountPartnerTypes])) != value)))
                {
                    this[this.myTable.ColumnItemsCountPartnerTypes] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountFamilyMembers
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountFamilyMembers.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountFamilyMembers) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountFamilyMembers])) != value)))
                {
                    this[this.myTable.ColumnItemsCountFamilyMembers] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountInterests
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountInterests.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountInterests) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountInterests])) != value)))
                {
                    this[this.myTable.ColumnItemsCountInterests] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountReminders
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountReminders.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountReminders) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountReminders])) != value)))
                {
                    this[this.myTable.ColumnItemsCountReminders] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountRelationships
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountRelationships.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountRelationships) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountRelationships])) != value)))
                {
                    this[this.myTable.ColumnItemsCountRelationships] = value;
                }
            }
        }
        
        /// 
        public Int32 ItemsCountContacts
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnItemsCountContacts.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnItemsCountContacts) 
                            || (((Int32)(this[this.myTable.ColumnItemsCountContacts])) != value)))
                {
                    this[this.myTable.ColumnItemsCountContacts] = value;
                }
            }
        }
        
        /// 
        public Boolean OfficeSpecificDataLabelsAvailable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOfficeSpecificDataLabelsAvailable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOfficeSpecificDataLabelsAvailable) 
                            || (((Boolean)(this[this.myTable.ColumnOfficeSpecificDataLabelsAvailable])) != value)))
                {
                    this[this.myTable.ColumnOfficeSpecificDataLabelsAvailable] = value;
                }
            }
        }
        
        /// 
        public Int64 FoundationOwner1Key
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFoundationOwner1Key.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFoundationOwner1Key) 
                            || (((Int64)(this[this.myTable.ColumnFoundationOwner1Key])) != value)))
                {
                    this[this.myTable.ColumnFoundationOwner1Key] = value;
                }
            }
        }
        
        /// 
        public Int64 FoundationOwner2Key
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFoundationOwner2Key.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFoundationOwner2Key) 
                            || (((Int64)(this[this.myTable.ColumnFoundationOwner2Key])) != value)))
                {
                    this[this.myTable.ColumnFoundationOwner2Key] = value;
                }
            }
        }
        
        /// 
        public Boolean HasEXWORKERPartnerType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHasEXWORKERPartnerType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHasEXWORKERPartnerType) 
                            || (((Boolean)(this[this.myTable.ColumnHasEXWORKERPartnerType])) != value)))
                {
                    this[this.myTable.ColumnHasEXWORKERPartnerType] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnSelectedSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnSelectedLocationKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnLastGiftDate);
            this.SetNull(this.myTable.ColumnLastGiftInfo);
            this.SetNull(this.myTable.ColumnLastContactDate);
            this.SetNull(this.myTable.ColumnItemsCountAddresses);
            this.SetNull(this.myTable.ColumnItemsCountAddressesActive);
            this.SetNull(this.myTable.ColumnItemsCountSubscriptions);
            this.SetNull(this.myTable.ColumnItemsCountSubscriptionsActive);
            this.SetNull(this.myTable.ColumnItemsCountPartnerTypes);
            this.SetNull(this.myTable.ColumnItemsCountFamilyMembers);
            this.SetNull(this.myTable.ColumnItemsCountInterests);
            this.SetNull(this.myTable.ColumnItemsCountReminders);
            this.SetNull(this.myTable.ColumnItemsCountRelationships);
            this.SetNull(this.myTable.ColumnItemsCountContacts);
            this.SetNull(this.myTable.ColumnOfficeSpecificDataLabelsAvailable);
            this.SetNull(this.myTable.ColumnFoundationOwner1Key);
            this.SetNull(this.myTable.ColumnFoundationOwner2Key);
            this.SetNull(this.myTable.ColumnHasEXWORKERPartnerType);
        }
        
        /// test for NULL value
        public bool IsLastGiftDateNull()
        {
            return this.IsNull(this.myTable.ColumnLastGiftDate);
        }
        
        /// assign NULL value
        public void SetLastGiftDateNull()
        {
            this.SetNull(this.myTable.ColumnLastGiftDate);
        }
        
        /// test for NULL value
        public bool IsLastGiftInfoNull()
        {
            return this.IsNull(this.myTable.ColumnLastGiftInfo);
        }
        
        /// assign NULL value
        public void SetLastGiftInfoNull()
        {
            this.SetNull(this.myTable.ColumnLastGiftInfo);
        }
        
        /// test for NULL value
        public bool IsLastContactDateNull()
        {
            return this.IsNull(this.myTable.ColumnLastContactDate);
        }
        
        /// assign NULL value
        public void SetLastContactDateNull()
        {
            this.SetNull(this.myTable.ColumnLastContactDate);
        }
        
        /// test for NULL value
        public bool IsItemsCountAddressesNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountAddresses);
        }
        
        /// assign NULL value
        public void SetItemsCountAddressesNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountAddresses);
        }
        
        /// test for NULL value
        public bool IsItemsCountAddressesActiveNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountAddressesActive);
        }
        
        /// assign NULL value
        public void SetItemsCountAddressesActiveNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountAddressesActive);
        }
        
        /// test for NULL value
        public bool IsItemsCountSubscriptionsNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountSubscriptions);
        }
        
        /// assign NULL value
        public void SetItemsCountSubscriptionsNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountSubscriptions);
        }
        
        /// test for NULL value
        public bool IsItemsCountSubscriptionsActiveNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountSubscriptionsActive);
        }
        
        /// assign NULL value
        public void SetItemsCountSubscriptionsActiveNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountSubscriptionsActive);
        }
        
        /// test for NULL value
        public bool IsItemsCountPartnerTypesNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountPartnerTypes);
        }
        
        /// assign NULL value
        public void SetItemsCountPartnerTypesNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountPartnerTypes);
        }
        
        /// test for NULL value
        public bool IsItemsCountFamilyMembersNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountFamilyMembers);
        }
        
        /// assign NULL value
        public void SetItemsCountFamilyMembersNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountFamilyMembers);
        }
        
        /// test for NULL value
        public bool IsItemsCountInterestsNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountInterests);
        }
        
        /// assign NULL value
        public void SetItemsCountInterestsNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountInterests);
        }
        
        /// test for NULL value
        public bool IsItemsCountRemindersNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountReminders);
        }
        
        /// assign NULL value
        public void SetItemsCountRemindersNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountReminders);
        }
        
        /// test for NULL value
        public bool IsItemsCountRelationshipsNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountRelationships);
        }
        
        /// assign NULL value
        public void SetItemsCountRelationshipsNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountRelationships);
        }
        
        /// test for NULL value
        public bool IsItemsCountContactsNull()
        {
            return this.IsNull(this.myTable.ColumnItemsCountContacts);
        }
        
        /// assign NULL value
        public void SetItemsCountContactsNull()
        {
            this.SetNull(this.myTable.ColumnItemsCountContacts);
        }
        
        /// test for NULL value
        public bool IsOfficeSpecificDataLabelsAvailableNull()
        {
            return this.IsNull(this.myTable.ColumnOfficeSpecificDataLabelsAvailable);
        }
        
        /// assign NULL value
        public void SetOfficeSpecificDataLabelsAvailableNull()
        {
            this.SetNull(this.myTable.ColumnOfficeSpecificDataLabelsAvailable);
        }
        
        /// test for NULL value
        public bool IsFoundationOwner1KeyNull()
        {
            return this.IsNull(this.myTable.ColumnFoundationOwner1Key);
        }
        
        /// assign NULL value
        public void SetFoundationOwner1KeyNull()
        {
            this.SetNull(this.myTable.ColumnFoundationOwner1Key);
        }
        
        /// test for NULL value
        public bool IsFoundationOwner2KeyNull()
        {
            return this.IsNull(this.myTable.ColumnFoundationOwner2Key);
        }
        
        /// assign NULL value
        public void SetFoundationOwner2KeyNull()
        {
            this.SetNull(this.myTable.ColumnFoundationOwner2Key);
        }
        
        /// test for NULL value
        public bool IsHasEXWORKERPartnerTypeNull()
        {
            return this.IsNull(this.myTable.ColumnHasEXWORKERPartnerType);
        }
        
        /// assign NULL value
        public void SetHasEXWORKERPartnerTypeNull()
        {
            this.SetNull(this.myTable.ColumnHasEXWORKERPartnerType);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerEditTDSFamilyMembersTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        
        /// This field indicates the family id of the individual.
        ///ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
        public DataColumn ColumnFamilyId;
        
        /// 
        public DataColumn ColumnGender;
        
        /// This is the date the rthe person was born
        public DataColumn ColumnDateOfBirth;
        
        /// 
        public DataColumn ColumnTypeCodeModify;
        
        /// 
        public DataColumn ColumnTypeCodePresent;
        
        /// 
        public DataColumn ColumnOtherTypeCodes;
        
        /// constructor
        public PartnerEditTDSFamilyMembersTable() : 
                base("FamilyMembers")
        {
        }
        
        /// constructor
        public PartnerEditTDSFamilyMembersTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSFamilyMembersTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerEditTDSFamilyMembersRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSFamilyMembersRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }
        
        /// get help text for column
        public static string GetPartnerShortNameHelp()
        {
            return "Enter a short name for this partner";
        }
        
        /// get label of column
        public static string GetPartnerShortNameLabel()
        {
            return "Short Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFamilyIdDBName()
        {
            return "p_family_id_i";
        }
        
        /// get help text for column
        public static string GetFamilyIdHelp()
        {
            return "This field indicates the family id of the individual.";
        }
        
        /// get label of column
        public static string GetFamilyIdLabel()
        {
            return "Family ID";
        }
        
        /// get the name of the field in the database for this column
        public static string GetGenderDBName()
        {
            return "p_gender_c";
        }
        
        /// get help text for column
        public static string GetGenderHelp()
        {
            return "Select the gender of the Person";
        }
        
        /// get label of column
        public static string GetGenderLabel()
        {
            return "Gender";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateOfBirthDBName()
        {
            return "p_date_of_birth_d";
        }
        
        /// get help text for column
        public static string GetDateOfBirthHelp()
        {
            return "Enter the date the person was born";
        }
        
        /// get label of column
        public static string GetDateOfBirthLabel()
        {
            return "Date of Birth";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTypeCodeModifyDBName()
        {
            return "TypeCodeModify";
        }
        
        /// get help text for column
        public static string GetTypeCodeModifyHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetTypeCodeModifyLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTypeCodePresentDBName()
        {
            return "TypeCodePresent";
        }
        
        /// get help text for column
        public static string GetTypeCodePresentHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetTypeCodePresentLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetOtherTypeCodesDBName()
        {
            return "OtherTypeCodes";
        }
        
        /// get help text for column
        public static string GetOtherTypeCodesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetOtherTypeCodesLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "FamilyMembers";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "FamilyMembers";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnFamilyId = this.Columns["p_family_id_i"];
            this.ColumnGender = this.Columns["p_gender_c"];
            this.ColumnDateOfBirth = this.Columns["p_date_of_birth_d"];
            this.ColumnTypeCodeModify = this.Columns["TypeCodeModify"];
            this.ColumnTypeCodePresent = this.Columns["TypeCodePresent"];
            this.ColumnOtherTypeCodes = this.Columns["OtherTypeCodes"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey};
        }
        
        /// create a new typed row
        public PartnerEditTDSFamilyMembersRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSFamilyMembersRow ret = ((PartnerEditTDSFamilyMembersRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSFamilyMembersRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_gender_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_date_of_birth_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("TypeCodeModify", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("TypeCodePresent", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("OtherTypeCodes", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPartnerShortName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnFamilyId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnGender))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDateOfBirth))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnTypeCodeModify))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTypeCodePresent))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnOtherTypeCodes))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerEditTDSFamilyMembersRow : System.Data.DataRow
    {
        
        private PartnerEditTDSFamilyMembersTable myTable;
        
        /// Constructor
        public PartnerEditTDSFamilyMembersRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSFamilyMembersTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public String PartnerShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerShortName) 
                            || (((String)(this[this.myTable.ColumnPartnerShortName])) != value)))
                {
                    this[this.myTable.ColumnPartnerShortName] = value;
                }
            }
        }
        
        /// This field indicates the family id of the individual.
        ///ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
        public Int32 FamilyId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFamilyId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFamilyId) 
                            || (((Int32)(this[this.myTable.ColumnFamilyId])) != value)))
                {
                    this[this.myTable.ColumnFamilyId] = value;
                }
            }
        }
        
        /// 
        public String Gender
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGender.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGender) 
                            || (((String)(this[this.myTable.ColumnGender])) != value)))
                {
                    this[this.myTable.ColumnGender] = value;
                }
            }
        }
        
        /// This is the date the rthe person was born
        public System.DateTime DateOfBirth
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfBirth.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateOfBirth) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateOfBirth])) != value)))
                {
                    this[this.myTable.ColumnDateOfBirth] = value;
                }
            }
        }
        
        /// 
        public Boolean TypeCodeModify
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTypeCodeModify.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTypeCodeModify) 
                            || (((Boolean)(this[this.myTable.ColumnTypeCodeModify])) != value)))
                {
                    this[this.myTable.ColumnTypeCodeModify] = value;
                }
            }
        }
        
        /// 
        public Boolean TypeCodePresent
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTypeCodePresent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTypeCodePresent) 
                            || (((Boolean)(this[this.myTable.ColumnTypeCodePresent])) != value)))
                {
                    this[this.myTable.ColumnTypeCodePresent] = value;
                }
            }
        }
        
        /// 
        public String OtherTypeCodes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOtherTypeCodes.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOtherTypeCodes) 
                            || (((String)(this[this.myTable.ColumnOtherTypeCodes])) != value)))
                {
                    this[this.myTable.ColumnOtherTypeCodes] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPartnerShortName);
            this[this.myTable.ColumnFamilyId.Ordinal] = 0;
            this[this.myTable.ColumnGender.Ordinal] = "Unknown";
            this.SetNull(this.myTable.ColumnDateOfBirth);
            this.SetNull(this.myTable.ColumnTypeCodeModify);
            this.SetNull(this.myTable.ColumnTypeCodePresent);
            this.SetNull(this.myTable.ColumnOtherTypeCodes);
        }
        
        /// test for NULL value
        public bool IsPartnerShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// assign NULL value
        public void SetPartnerShortNameNull()
        {
            this.SetNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// test for NULL value
        public bool IsFamilyIdNull()
        {
            return this.IsNull(this.myTable.ColumnFamilyId);
        }
        
        /// assign NULL value
        public void SetFamilyIdNull()
        {
            this.SetNull(this.myTable.ColumnFamilyId);
        }
        
        /// test for NULL value
        public bool IsGenderNull()
        {
            return this.IsNull(this.myTable.ColumnGender);
        }
        
        /// assign NULL value
        public void SetGenderNull()
        {
            this.SetNull(this.myTable.ColumnGender);
        }
        
        /// test for NULL value
        public bool IsDateOfBirthNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfBirth);
        }
        
        /// assign NULL value
        public void SetDateOfBirthNull()
        {
            this.SetNull(this.myTable.ColumnDateOfBirth);
        }
        
        /// test for NULL value
        public bool IsTypeCodeModifyNull()
        {
            return this.IsNull(this.myTable.ColumnTypeCodeModify);
        }
        
        /// assign NULL value
        public void SetTypeCodeModifyNull()
        {
            this.SetNull(this.myTable.ColumnTypeCodeModify);
        }
        
        /// test for NULL value
        public bool IsTypeCodePresentNull()
        {
            return this.IsNull(this.myTable.ColumnTypeCodePresent);
        }
        
        /// assign NULL value
        public void SetTypeCodePresentNull()
        {
            this.SetNull(this.myTable.ColumnTypeCodePresent);
        }
        
        /// test for NULL value
        public bool IsOtherTypeCodesNull()
        {
            return this.IsNull(this.myTable.ColumnOtherTypeCodes);
        }
        
        /// assign NULL value
        public void SetOtherTypeCodesNull()
        {
            this.SetNull(this.myTable.ColumnOtherTypeCodes);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerEditTDSFamilyMembersInfoForStatusChangeTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// constructor
        public PartnerEditTDSFamilyMembersInfoForStatusChangeTable() : 
                base("FamilyMembersInfoForStatusChange")
        {
        }
        
        /// constructor
        public PartnerEditTDSFamilyMembersInfoForStatusChangeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSFamilyMembersInfoForStatusChangeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerEditTDSFamilyMembersInfoForStatusChangeRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSFamilyMembersInfoForStatusChangeRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "FamilyMembersInfoForStatusChange";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "FamilyMembersInfoForStatusChange";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
        }
        
        /// create a new typed row
        public PartnerEditTDSFamilyMembersInfoForStatusChangeRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSFamilyMembersInfoForStatusChangeRow ret = ((PartnerEditTDSFamilyMembersInfoForStatusChangeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSFamilyMembersInfoForStatusChangeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerEditTDSFamilyMembersInfoForStatusChangeRow : System.Data.DataRow
    {
        
        private PartnerEditTDSFamilyMembersInfoForStatusChangeTable myTable;
        
        /// Constructor
        public PartnerEditTDSFamilyMembersInfoForStatusChangeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSFamilyMembersInfoForStatusChangeTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// 
        public DataColumn ColumnTypeCode;
        
        /// 
        public DataColumn ColumnAddTypeCode;
        
        /// 
        public DataColumn ColumnRemoveTypeCode;
        
        /// constructor
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable() : 
                base("PartnerTypeChangeFamilyMembersPromotion")
        {
        }
        
        /// constructor
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTypeCodeDBName()
        {
            return "TypeCode";
        }
        
        /// get help text for column
        public static string GetTypeCodeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetTypeCodeLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddTypeCodeDBName()
        {
            return "AddTypeCode";
        }
        
        /// get help text for column
        public static string GetAddTypeCodeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAddTypeCodeLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetRemoveTypeCodeDBName()
        {
            return "RemoveTypeCode";
        }
        
        /// get help text for column
        public static string GetRemoveTypeCodeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetRemoveTypeCodeLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "PartnerTypeChangeFamilyMembersPromotion";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "PartnerTypeChangeFamilyMembersPromotion";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnTypeCode = this.Columns["TypeCode"];
            this.ColumnAddTypeCode = this.Columns["AddTypeCode"];
            this.ColumnRemoveTypeCode = this.Columns["RemoveTypeCode"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey,
                    this.ColumnTypeCode,
                    this.ColumnAddTypeCode};
        }
        
        /// create a new typed row
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow ret = ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("TypeCode", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("AddTypeCode", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("RemoveTypeCode", typeof(Boolean)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAddTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnRemoveTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow : System.Data.DataRow
    {
        
        private PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable myTable;
        
        /// Constructor
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// 
        public String TypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTypeCode) 
                            || (((String)(this[this.myTable.ColumnTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTypeCode] = value;
                }
            }
        }
        
        /// 
        public Boolean AddTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddTypeCode) 
                            || (((Boolean)(this[this.myTable.ColumnAddTypeCode])) != value)))
                {
                    this[this.myTable.ColumnAddTypeCode] = value;
                }
            }
        }
        
        /// 
        public Boolean RemoveTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRemoveTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRemoveTypeCode) 
                            || (((Boolean)(this[this.myTable.ColumnRemoveTypeCode])) != value)))
                {
                    this[this.myTable.ColumnRemoveTypeCode] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTypeCode);
            this.SetNull(this.myTable.ColumnAddTypeCode);
            this.SetNull(this.myTable.ColumnRemoveTypeCode);
        }
        
        /// test for NULL value
        public bool IsTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnTypeCode);
        }
        
        /// assign NULL value
        public void SetTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnTypeCode);
        }
        
        /// test for NULL value
        public bool IsAddTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAddTypeCode);
        }
        
        /// assign NULL value
        public void SetAddTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnAddTypeCode);
        }
        
        /// test for NULL value
        public bool IsRemoveTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnRemoveTypeCode);
        }
        
        /// assign NULL value
        public void SetRemoveTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnRemoveTypeCode);
        }
    }
    
    /// auto generated
    [Serializable()]
    public class PartnerEditTDS : TTypedDataSet
    {
        
        private PPartnerTable TablePPartner;
        
        private PPartnerTypeTable TablePPartnerType;
        
        private PSubscriptionTable TablePSubscription;
        
        private PartnerEditTDSPPartnerLocationTable TablePPartnerLocation;
        
        private PLocationTable TablePLocation;
        
        private PartnerEditTDSPPersonTable TablePPerson;
        
        private PartnerEditTDSPFamilyTable TablePFamily;
        
        private PUnitTable TablePUnit;
        
        private POrganisationTable TablePOrganisation;
        
        private PChurchTable TablePChurch;
        
        private PBankTable TablePBank;
        
        private PBankingDetailsTable TablePBankingDetails;
        
        private PPartnerBankingDetailsTable TablePPartnerBankingDetails;
        
        private PVenueTable TablePVenue;
        
        private PFoundationTable TablePFoundation;
        
        private PFoundationDeadlineTable TablePFoundationDeadline;
        
        private PFoundationProposalTable TablePFoundationProposal;
        
        private PFoundationProposalDetailTable TablePFoundationProposalDetail;
        
        private PPartnerInterestTable TablePPartnerInterest;
        
        private PInterestTable TablePInterest;
        
        private PPartnerReminderTable TablePPartnerReminder;
        
        private PPartnerRelationshipTable TablePPartnerRelationship;
        
        private PPartnerContactTable TablePPartnerContact;
        
        private PDataLabelValueApplicationTable TablePDataLabelValueApplication;
        
        private PDataLabelValuePartnerTable TablePDataLabelValuePartner;
        
        private PartnerEditTDSMiscellaneousDataTable TableMiscellaneousData;
        
        private PartnerEditTDSFamilyMembersTable TableFamilyMembers;
        
        private PartnerEditTDSFamilyMembersInfoForStatusChangeTable TableFamilyMembersInfoForStatusChange;
        
        private PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable TablePartnerTypeChangeFamilyMembersPromotion;
        
        /// auto generated
        public PartnerEditTDS() : 
                base("PartnerEditTDS")
        {
        }
        
        /// auto generated for serialization
        public PartnerEditTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public PartnerEditTDS(string ADatasetName) : 
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
        public PPartnerTypeTable PPartnerType
        {
            get
            {
                return this.TablePPartnerType;
            }
        }
        
        /// auto generated
        public PSubscriptionTable PSubscription
        {
            get
            {
                return this.TablePSubscription;
            }
        }
        
        /// auto generated
        public PartnerEditTDSPPartnerLocationTable PPartnerLocation
        {
            get
            {
                return this.TablePPartnerLocation;
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
        public PartnerEditTDSPPersonTable PPerson
        {
            get
            {
                return this.TablePPerson;
            }
        }
        
        /// auto generated
        public PartnerEditTDSPFamilyTable PFamily
        {
            get
            {
                return this.TablePFamily;
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
        public POrganisationTable POrganisation
        {
            get
            {
                return this.TablePOrganisation;
            }
        }
        
        /// auto generated
        public PChurchTable PChurch
        {
            get
            {
                return this.TablePChurch;
            }
        }
        
        /// auto generated
        public PBankTable PBank
        {
            get
            {
                return this.TablePBank;
            }
        }
        
        /// auto generated
        public PBankingDetailsTable PBankingDetails
        {
            get
            {
                return this.TablePBankingDetails;
            }
        }
        
        /// auto generated
        public PPartnerBankingDetailsTable PPartnerBankingDetails
        {
            get
            {
                return this.TablePPartnerBankingDetails;
            }
        }
        
        /// auto generated
        public PVenueTable PVenue
        {
            get
            {
                return this.TablePVenue;
            }
        }
        
        /// auto generated
        public PFoundationTable PFoundation
        {
            get
            {
                return this.TablePFoundation;
            }
        }
        
        /// auto generated
        public PFoundationDeadlineTable PFoundationDeadline
        {
            get
            {
                return this.TablePFoundationDeadline;
            }
        }
        
        /// auto generated
        public PFoundationProposalTable PFoundationProposal
        {
            get
            {
                return this.TablePFoundationProposal;
            }
        }
        
        /// auto generated
        public PFoundationProposalDetailTable PFoundationProposalDetail
        {
            get
            {
                return this.TablePFoundationProposalDetail;
            }
        }
        
        /// auto generated
        public PPartnerInterestTable PPartnerInterest
        {
            get
            {
                return this.TablePPartnerInterest;
            }
        }
        
        /// auto generated
        public PInterestTable PInterest
        {
            get
            {
                return this.TablePInterest;
            }
        }
        
        /// auto generated
        public PPartnerReminderTable PPartnerReminder
        {
            get
            {
                return this.TablePPartnerReminder;
            }
        }
        
        /// auto generated
        public PPartnerRelationshipTable PPartnerRelationship
        {
            get
            {
                return this.TablePPartnerRelationship;
            }
        }
        
        /// auto generated
        public PPartnerContactTable PPartnerContact
        {
            get
            {
                return this.TablePPartnerContact;
            }
        }
        
        /// auto generated
        public PDataLabelValueApplicationTable PDataLabelValueApplication
        {
            get
            {
                return this.TablePDataLabelValueApplication;
            }
        }
        
        /// auto generated
        public PDataLabelValuePartnerTable PDataLabelValuePartner
        {
            get
            {
                return this.TablePDataLabelValuePartner;
            }
        }
        
        /// auto generated
        public PartnerEditTDSMiscellaneousDataTable MiscellaneousData
        {
            get
            {
                return this.TableMiscellaneousData;
            }
        }
        
        /// auto generated
        public PartnerEditTDSFamilyMembersTable FamilyMembers
        {
            get
            {
                return this.TableFamilyMembers;
            }
        }
        
        /// auto generated
        public PartnerEditTDSFamilyMembersInfoForStatusChangeTable FamilyMembersInfoForStatusChange
        {
            get
            {
                return this.TableFamilyMembersInfoForStatusChange;
            }
        }
        
        /// auto generated
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable PartnerTypeChangeFamilyMembersPromotion
        {
            get
            {
                return this.TablePartnerTypeChangeFamilyMembersPromotion;
            }
        }
        
        /// auto generated
        public new virtual PartnerEditTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerEditTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            this.Tables.Add(new PSubscriptionTable("PSubscription"));
            this.Tables.Add(new PartnerEditTDSPPartnerLocationTable("PPartnerLocation"));
            this.Tables.Add(new PLocationTable("PLocation"));
            this.Tables.Add(new PartnerEditTDSPPersonTable("PPerson"));
            this.Tables.Add(new PartnerEditTDSPFamilyTable("PFamily"));
            this.Tables.Add(new PUnitTable("PUnit"));
            this.Tables.Add(new POrganisationTable("POrganisation"));
            this.Tables.Add(new PChurchTable("PChurch"));
            this.Tables.Add(new PBankTable("PBank"));
            this.Tables.Add(new PBankingDetailsTable("PBankingDetails"));
            this.Tables.Add(new PPartnerBankingDetailsTable("PPartnerBankingDetails"));
            this.Tables.Add(new PVenueTable("PVenue"));
            this.Tables.Add(new PFoundationTable("PFoundation"));
            this.Tables.Add(new PFoundationDeadlineTable("PFoundationDeadline"));
            this.Tables.Add(new PFoundationProposalTable("PFoundationProposal"));
            this.Tables.Add(new PFoundationProposalDetailTable("PFoundationProposalDetail"));
            this.Tables.Add(new PPartnerInterestTable("PPartnerInterest"));
            this.Tables.Add(new PInterestTable("PInterest"));
            this.Tables.Add(new PPartnerReminderTable("PPartnerReminder"));
            this.Tables.Add(new PPartnerRelationshipTable("PPartnerRelationship"));
            this.Tables.Add(new PPartnerContactTable("PPartnerContact"));
            this.Tables.Add(new PDataLabelValueApplicationTable("PDataLabelValueApplication"));
            this.Tables.Add(new PDataLabelValuePartnerTable("PDataLabelValuePartner"));
            this.Tables.Add(new PartnerEditTDSMiscellaneousDataTable("MiscellaneousData"));
            this.Tables.Add(new PartnerEditTDSFamilyMembersTable("FamilyMembers"));
            this.Tables.Add(new PartnerEditTDSFamilyMembersInfoForStatusChangeTable("FamilyMembersInfoForStatusChange"));
            this.Tables.Add(new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable("PartnerTypeChangeFamilyMembersPromotion"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("PPartnerType") != -1))
            {
                this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            }
            if ((ds.Tables.IndexOf("PSubscription") != -1))
            {
                this.Tables.Add(new PSubscriptionTable("PSubscription"));
            }
            if ((ds.Tables.IndexOf("PPartnerLocation") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPPartnerLocationTable("PPartnerLocation"));
            }
            if ((ds.Tables.IndexOf("PLocation") != -1))
            {
                this.Tables.Add(new PLocationTable("PLocation"));
            }
            if ((ds.Tables.IndexOf("PPerson") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPPersonTable("PPerson"));
            }
            if ((ds.Tables.IndexOf("PFamily") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPFamilyTable("PFamily"));
            }
            if ((ds.Tables.IndexOf("PUnit") != -1))
            {
                this.Tables.Add(new PUnitTable("PUnit"));
            }
            if ((ds.Tables.IndexOf("POrganisation") != -1))
            {
                this.Tables.Add(new POrganisationTable("POrganisation"));
            }
            if ((ds.Tables.IndexOf("PChurch") != -1))
            {
                this.Tables.Add(new PChurchTable("PChurch"));
            }
            if ((ds.Tables.IndexOf("PBank") != -1))
            {
                this.Tables.Add(new PBankTable("PBank"));
            }
            if ((ds.Tables.IndexOf("PBankingDetails") != -1))
            {
                this.Tables.Add(new PBankingDetailsTable("PBankingDetails"));
            }
            if ((ds.Tables.IndexOf("PPartnerBankingDetails") != -1))
            {
                this.Tables.Add(new PPartnerBankingDetailsTable("PPartnerBankingDetails"));
            }
            if ((ds.Tables.IndexOf("PVenue") != -1))
            {
                this.Tables.Add(new PVenueTable("PVenue"));
            }
            if ((ds.Tables.IndexOf("PFoundation") != -1))
            {
                this.Tables.Add(new PFoundationTable("PFoundation"));
            }
            if ((ds.Tables.IndexOf("PFoundationDeadline") != -1))
            {
                this.Tables.Add(new PFoundationDeadlineTable("PFoundationDeadline"));
            }
            if ((ds.Tables.IndexOf("PFoundationProposal") != -1))
            {
                this.Tables.Add(new PFoundationProposalTable("PFoundationProposal"));
            }
            if ((ds.Tables.IndexOf("PFoundationProposalDetail") != -1))
            {
                this.Tables.Add(new PFoundationProposalDetailTable("PFoundationProposalDetail"));
            }
            if ((ds.Tables.IndexOf("PPartnerInterest") != -1))
            {
                this.Tables.Add(new PPartnerInterestTable("PPartnerInterest"));
            }
            if ((ds.Tables.IndexOf("PInterest") != -1))
            {
                this.Tables.Add(new PInterestTable("PInterest"));
            }
            if ((ds.Tables.IndexOf("PPartnerReminder") != -1))
            {
                this.Tables.Add(new PPartnerReminderTable("PPartnerReminder"));
            }
            if ((ds.Tables.IndexOf("PPartnerRelationship") != -1))
            {
                this.Tables.Add(new PPartnerRelationshipTable("PPartnerRelationship"));
            }
            if ((ds.Tables.IndexOf("PPartnerContact") != -1))
            {
                this.Tables.Add(new PPartnerContactTable("PPartnerContact"));
            }
            if ((ds.Tables.IndexOf("PDataLabelValueApplication") != -1))
            {
                this.Tables.Add(new PDataLabelValueApplicationTable("PDataLabelValueApplication"));
            }
            if ((ds.Tables.IndexOf("PDataLabelValuePartner") != -1))
            {
                this.Tables.Add(new PDataLabelValuePartnerTable("PDataLabelValuePartner"));
            }
            if ((ds.Tables.IndexOf("MiscellaneousData") != -1))
            {
                this.Tables.Add(new PartnerEditTDSMiscellaneousDataTable("MiscellaneousData"));
            }
            if ((ds.Tables.IndexOf("FamilyMembers") != -1))
            {
                this.Tables.Add(new PartnerEditTDSFamilyMembersTable("FamilyMembers"));
            }
            if ((ds.Tables.IndexOf("FamilyMembersInfoForStatusChange") != -1))
            {
                this.Tables.Add(new PartnerEditTDSFamilyMembersInfoForStatusChangeTable("FamilyMembersInfoForStatusChange"));
            }
            if ((ds.Tables.IndexOf("PartnerTypeChangeFamilyMembersPromotion") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable("PartnerTypeChangeFamilyMembersPromotion"));
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
            if ((this.TablePPartnerType != null))
            {
                this.TablePPartnerType.InitVars();
            }
            if ((this.TablePSubscription != null))
            {
                this.TablePSubscription.InitVars();
            }
            if ((this.TablePPartnerLocation != null))
            {
                this.TablePPartnerLocation.InitVars();
            }
            if ((this.TablePLocation != null))
            {
                this.TablePLocation.InitVars();
            }
            if ((this.TablePPerson != null))
            {
                this.TablePPerson.InitVars();
            }
            if ((this.TablePFamily != null))
            {
                this.TablePFamily.InitVars();
            }
            if ((this.TablePUnit != null))
            {
                this.TablePUnit.InitVars();
            }
            if ((this.TablePOrganisation != null))
            {
                this.TablePOrganisation.InitVars();
            }
            if ((this.TablePChurch != null))
            {
                this.TablePChurch.InitVars();
            }
            if ((this.TablePBank != null))
            {
                this.TablePBank.InitVars();
            }
            if ((this.TablePBankingDetails != null))
            {
                this.TablePBankingDetails.InitVars();
            }
            if ((this.TablePPartnerBankingDetails != null))
            {
                this.TablePPartnerBankingDetails.InitVars();
            }
            if ((this.TablePVenue != null))
            {
                this.TablePVenue.InitVars();
            }
            if ((this.TablePFoundation != null))
            {
                this.TablePFoundation.InitVars();
            }
            if ((this.TablePFoundationDeadline != null))
            {
                this.TablePFoundationDeadline.InitVars();
            }
            if ((this.TablePFoundationProposal != null))
            {
                this.TablePFoundationProposal.InitVars();
            }
            if ((this.TablePFoundationProposalDetail != null))
            {
                this.TablePFoundationProposalDetail.InitVars();
            }
            if ((this.TablePPartnerInterest != null))
            {
                this.TablePPartnerInterest.InitVars();
            }
            if ((this.TablePInterest != null))
            {
                this.TablePInterest.InitVars();
            }
            if ((this.TablePPartnerReminder != null))
            {
                this.TablePPartnerReminder.InitVars();
            }
            if ((this.TablePPartnerRelationship != null))
            {
                this.TablePPartnerRelationship.InitVars();
            }
            if ((this.TablePPartnerContact != null))
            {
                this.TablePPartnerContact.InitVars();
            }
            if ((this.TablePDataLabelValueApplication != null))
            {
                this.TablePDataLabelValueApplication.InitVars();
            }
            if ((this.TablePDataLabelValuePartner != null))
            {
                this.TablePDataLabelValuePartner.InitVars();
            }
            if ((this.TableMiscellaneousData != null))
            {
                this.TableMiscellaneousData.InitVars();
            }
            if ((this.TableFamilyMembers != null))
            {
                this.TableFamilyMembers.InitVars();
            }
            if ((this.TableFamilyMembersInfoForStatusChange != null))
            {
                this.TableFamilyMembersInfoForStatusChange.InitVars();
            }
            if ((this.TablePartnerTypeChangeFamilyMembersPromotion != null))
            {
                this.TablePartnerTypeChangeFamilyMembersPromotion.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerEditTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TablePPartnerType = ((PPartnerTypeTable)(this.Tables["PPartnerType"]));
            this.TablePSubscription = ((PSubscriptionTable)(this.Tables["PSubscription"]));
            this.TablePPartnerLocation = ((PartnerEditTDSPPartnerLocationTable)(this.Tables["PPartnerLocation"]));
            this.TablePLocation = ((PLocationTable)(this.Tables["PLocation"]));
            this.TablePPerson = ((PartnerEditTDSPPersonTable)(this.Tables["PPerson"]));
            this.TablePFamily = ((PartnerEditTDSPFamilyTable)(this.Tables["PFamily"]));
            this.TablePUnit = ((PUnitTable)(this.Tables["PUnit"]));
            this.TablePOrganisation = ((POrganisationTable)(this.Tables["POrganisation"]));
            this.TablePChurch = ((PChurchTable)(this.Tables["PChurch"]));
            this.TablePBank = ((PBankTable)(this.Tables["PBank"]));
            this.TablePBankingDetails = ((PBankingDetailsTable)(this.Tables["PBankingDetails"]));
            this.TablePPartnerBankingDetails = ((PPartnerBankingDetailsTable)(this.Tables["PPartnerBankingDetails"]));
            this.TablePVenue = ((PVenueTable)(this.Tables["PVenue"]));
            this.TablePFoundation = ((PFoundationTable)(this.Tables["PFoundation"]));
            this.TablePFoundationDeadline = ((PFoundationDeadlineTable)(this.Tables["PFoundationDeadline"]));
            this.TablePFoundationProposal = ((PFoundationProposalTable)(this.Tables["PFoundationProposal"]));
            this.TablePFoundationProposalDetail = ((PFoundationProposalDetailTable)(this.Tables["PFoundationProposalDetail"]));
            this.TablePPartnerInterest = ((PPartnerInterestTable)(this.Tables["PPartnerInterest"]));
            this.TablePInterest = ((PInterestTable)(this.Tables["PInterest"]));
            this.TablePPartnerReminder = ((PPartnerReminderTable)(this.Tables["PPartnerReminder"]));
            this.TablePPartnerRelationship = ((PPartnerRelationshipTable)(this.Tables["PPartnerRelationship"]));
            this.TablePPartnerContact = ((PPartnerContactTable)(this.Tables["PPartnerContact"]));
            this.TablePDataLabelValueApplication = ((PDataLabelValueApplicationTable)(this.Tables["PDataLabelValueApplication"]));
            this.TablePDataLabelValuePartner = ((PDataLabelValuePartnerTable)(this.Tables["PDataLabelValuePartner"]));
            this.TableMiscellaneousData = ((PartnerEditTDSMiscellaneousDataTable)(this.Tables["MiscellaneousData"]));
            this.TableFamilyMembers = ((PartnerEditTDSFamilyMembersTable)(this.Tables["FamilyMembers"]));
            this.TableFamilyMembersInfoForStatusChange = ((PartnerEditTDSFamilyMembersInfoForStatusChangeTable)(this.Tables["FamilyMembersInfoForStatusChange"]));
            this.TablePartnerTypeChangeFamilyMembersPromotion = ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable)(this.Tables["PartnerTypeChangeFamilyMembersPromotion"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerType != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerType3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerType", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePSubscription != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKSubscription2", "PPartner", new string[] {
                                "p_partner_key_n"}, "PSubscription", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePSubscription != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKSubscription3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PSubscription", new string[] {
                                "p_gift_from_key_n"}));
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
                                "p_site_key_n",
                                "p_location_key_i"}, "PPartnerLocation", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePFamily != null) 
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson2", "PFamily", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_family_key_n"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson4", "PUnit", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_field_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePFamily != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFamily1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PFamily", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePFamily != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFamily2", "PUnit", new string[] {
                                "p_partner_key_n"}, "PFamily", new string[] {
                                "p_field_key_n"}));
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
            if (((this.TablePPartner != null) 
                        && (this.TablePOrganisation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKOrganisation1", "PPartner", new string[] {
                                "p_partner_key_n"}, "POrganisation", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePOrganisation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKOrganisation3", "PPartner", new string[] {
                                "p_partner_key_n"}, "POrganisation", new string[] {
                                "p_contact_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePChurch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKChurch1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PChurch", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePChurch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKChurch3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PChurch", new string[] {
                                "p_contact_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePBank != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBank1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PBank", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePBank != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBank2", "PPartner", new string[] {
                                "p_partner_key_n"}, "PBank", new string[] {
                                "p_contact_partner_key_n"}));
            }
            if (((this.TablePBank != null) 
                        && (this.TablePBankingDetails != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBankingDetails4", "PBank", new string[] {
                                "p_partner_key_n"}, "PBankingDetails", new string[] {
                                "p_bank_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerBankingDetails != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerBankingLink1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerBankingDetails", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePBankingDetails != null) 
                        && (this.TablePPartnerBankingDetails != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerBankingLink2", "PBankingDetails", new string[] {
                                "p_banking_details_key_i"}, "PPartnerBankingDetails", new string[] {
                                "p_banking_details_key_i"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePVenue != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKVenue1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PVenue", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePVenue != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKVenue3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PVenue", new string[] {
                                "p_contact_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePFoundation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFoundationContact1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PFoundation", new string[] {
                                "p_contact_partner_n"}));
            }
            if (((this.TablePOrganisation != null) 
                        && (this.TablePFoundation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFoundation1", "POrganisation", new string[] {
                                "p_partner_key_n"}, "PFoundation", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePFoundation != null) 
                        && (this.TablePFoundationDeadline != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFoundationDeadline1", "PFoundation", new string[] {
                                "p_partner_key_n"}, "PFoundationDeadline", new string[] {
                                "p_foundation_partner_key_n"}));
            }
            if (((this.TablePFoundation != null) 
                        && (this.TablePFoundationProposal != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKProposalStatus1", "PFoundation", new string[] {
                                "p_partner_key_n"}, "PFoundationProposal", new string[] {
                                "p_foundation_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePFoundationProposal != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKProposalSubmitted3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PFoundationProposal", new string[] {
                                "p_partner_submitted_by_n"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePFoundationProposalDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKArea1", "PUnit", new string[] {
                                "p_partner_key_n"}, "PFoundationProposalDetail", new string[] {
                                "p_area_partner_key_n"}));
            }
            if (((this.TablePFoundationProposal != null) 
                        && (this.TablePFoundationProposalDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDetailProposal1", "PFoundationProposal", new string[] {
                                "p_foundation_partner_key_n",
                                "p_foundation_proposal_key_i"}, "PFoundationProposalDetail", new string[] {
                                "p_foundation_partner_key_n",
                                "p_foundation_proposal_key_i"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePFoundationProposalDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKField1", "PUnit", new string[] {
                                "p_partner_key_n"}, "PFoundationProposalDetail", new string[] {
                                "p_field_partner_key_n"}));
            }
            if (((this.TablePFoundation != null) 
                        && (this.TablePFoundationProposalDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDetailProposal2", "PFoundation", new string[] {
                                "p_partner_key_n"}, "PFoundationProposalDetail", new string[] {
                                "p_foundation_partner_key_n"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePFoundationProposalDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKProposalMinistry1", "PUnit", new string[] {
                                "p_partner_key_n"}, "PFoundationProposalDetail", new string[] {
                                "p_key_ministry_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerInterest != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerInterest1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerInterest", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null) 
                        && (this.TablePPartnerInterest != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerInterest2", "PUnit", new string[] {
                                "p_partner_key_n"}, "PPartnerInterest", new string[] {
                                "p_field_key_n"}));
            }
            if (((this.TablePInterest != null) 
                        && (this.TablePPartnerInterest != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerInterest4", "PInterest", new string[] {
                                "p_interest_c"}, "PPartnerInterest", new string[] {
                                "p_interest_c"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerReminder != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerReminder1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerReminder", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartnerContact != null) 
                        && (this.TablePPartnerReminder != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerReminder2", "PPartnerContact", new string[] {
                                "p_contact_id_i"}, "PPartnerReminder", new string[] {
                                "p_contact_id_i"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerRelationship != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerRelationship1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerRelationship", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerRelationship != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerRelationship2", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerRelationship", new string[] {
                                "p_relation_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerContact != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerContact1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerContact", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePDataLabelValueApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelValueApplication3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PDataLabelValueApplication", new string[] {
                                "p_value_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePDataLabelValuePartner != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelValuePartner1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PDataLabelValuePartner", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePDataLabelValuePartner != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelValuePartner3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PDataLabelValuePartner", new string[] {
                                "p_value_partner_key_n"}));
            }
            if (((this.TablePLocation != null) 
                        && (this.TableMiscellaneousData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLocation2", "PLocation", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}, "MiscellaneousData", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}));
            }
            this.FRelations.Add(new TTypedRelation("Address", "PPartnerLocation", new string[] {
                            "p_site_key_n",
                            "p_location_key_i"}, "PLocation", new string[] {
                            "p_site_key_n",
                            "p_location_key_i"}, false));
            this.FRelations.Add(new TTypedRelation("PartnerInterestCategory", "PInterest", new string[] {
                            "p_interest_c"}, "PPartnerInterest", new string[] {
                            "p_interest_c"}, false));
        }
    }
    
    /// auto generated table derived from PLocation
    [Serializable()]
    public class PartnerAddressAggregateTDSSimilarLocationParametersTable : PLocationTable
    {
        
        /// 
        public DataColumn ColumnSiteKeyOfSimilarLocation;
        
        /// 
        public DataColumn ColumnLocationKeyOfSimilarLocation;
        
        /// 
        public DataColumn ColumnUsedByNOtherPartners;
        
        /// 
        public DataColumn ColumnAnswerReuse;
        
        /// 
        public DataColumn ColumnAnswerProcessedClientSide;
        
        /// 
        public DataColumn ColumnAnswerProcessedServerSide;
        
        /// constructor
        public PartnerAddressAggregateTDSSimilarLocationParametersTable() : 
                base("SimilarLocationParameters")
        {
        }
        
        /// constructor
        public PartnerAddressAggregateTDSSimilarLocationParametersTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerAddressAggregateTDSSimilarLocationParametersTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public new PartnerAddressAggregateTDSSimilarLocationParametersRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSSimilarLocationParametersRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetSiteKeyOfSimilarLocationDBName()
        {
            return "SiteKeyOfSimilarLocation";
        }
        
        /// get help text for column
        public static string GetSiteKeyOfSimilarLocationHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetSiteKeyOfSimilarLocationLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationKeyOfSimilarLocationDBName()
        {
            return "LocationKeyOfSimilarLocation";
        }
        
        /// get help text for column
        public static string GetLocationKeyOfSimilarLocationHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLocationKeyOfSimilarLocationLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetUsedByNOtherPartnersDBName()
        {
            return "UsedByNOtherPartners";
        }
        
        /// get help text for column
        public static string GetUsedByNOtherPartnersHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetUsedByNOtherPartnersLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnswerReuseDBName()
        {
            return "AnswerReuse";
        }
        
        /// get help text for column
        public static string GetAnswerReuseHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnswerReuseLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedClientSideDBName()
        {
            return "AnswerProcessedClientSide";
        }
        
        /// get help text for column
        public static string GetAnswerProcessedClientSideHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnswerProcessedClientSideLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedServerSideDBName()
        {
            return "AnswerProcessedServerSide";
        }
        
        /// get help text for column
        public static string GetAnswerProcessedServerSideHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnswerProcessedServerSideLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public new static string GetTableName()
        {
            return "SimilarLocationParameters";
        }
        
        /// original name of table in the database
        public new static string GetTableDBName()
        {
            return "SimilarLocationParameters";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            base.InitVars();
            this.ColumnSiteKeyOfSimilarLocation = this.Columns["SiteKeyOfSimilarLocation"];
            this.ColumnLocationKeyOfSimilarLocation = this.Columns["LocationKeyOfSimilarLocation"];
            this.ColumnUsedByNOtherPartners = this.Columns["UsedByNOtherPartners"];
            this.ColumnAnswerReuse = this.Columns["AnswerReuse"];
            this.ColumnAnswerProcessedClientSide = this.Columns["AnswerProcessedClientSide"];
            this.ColumnAnswerProcessedServerSide = this.Columns["AnswerProcessedServerSide"];
        }
        
        /// create a new typed row
        public new PartnerAddressAggregateTDSSimilarLocationParametersRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerAddressAggregateTDSSimilarLocationParametersRow ret = ((PartnerAddressAggregateTDSSimilarLocationParametersRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSSimilarLocationParametersRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            base.InitClass();
            this.Columns.Add(new System.Data.DataColumn("SiteKeyOfSimilarLocation", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("LocationKeyOfSimilarLocation", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("UsedByNOtherPartners", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("AnswerReuse", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedClientSide", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedServerSide", typeof(Boolean)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnSiteKeyOfSimilarLocation))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLocationKeyOfSimilarLocation))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnUsedByNOtherPartners))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnswerReuse))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnswerProcessedClientSide))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnswerProcessedServerSide))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return base.CreateOdbcParameter(ACol);
        }
    }
    
    /// DerivedRow from PLocationRow
    [Serializable()]
    public class PartnerAddressAggregateTDSSimilarLocationParametersRow : PLocationRow
    {
        
        private PartnerAddressAggregateTDSSimilarLocationParametersTable myTable;
        
        /// Constructor
        public PartnerAddressAggregateTDSSimilarLocationParametersRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerAddressAggregateTDSSimilarLocationParametersTable)(this.Table));
        }
        
        /// 
        public Int64 SiteKeyOfSimilarLocation
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSiteKeyOfSimilarLocation.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSiteKeyOfSimilarLocation) 
                            || (((Int64)(this[this.myTable.ColumnSiteKeyOfSimilarLocation])) != value)))
                {
                    this[this.myTable.ColumnSiteKeyOfSimilarLocation] = value;
                }
            }
        }
        
        /// 
        public Int32 LocationKeyOfSimilarLocation
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKeyOfSimilarLocation.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKeyOfSimilarLocation) 
                            || (((Int32)(this[this.myTable.ColumnLocationKeyOfSimilarLocation])) != value)))
                {
                    this[this.myTable.ColumnLocationKeyOfSimilarLocation] = value;
                }
            }
        }
        
        /// 
        public Int32 UsedByNOtherPartners
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUsedByNOtherPartners.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUsedByNOtherPartners) 
                            || (((Int32)(this[this.myTable.ColumnUsedByNOtherPartners])) != value)))
                {
                    this[this.myTable.ColumnUsedByNOtherPartners] = value;
                }
            }
        }
        
        /// 
        public Boolean AnswerReuse
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnswerReuse.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnswerReuse) 
                            || (((Boolean)(this[this.myTable.ColumnAnswerReuse])) != value)))
                {
                    this[this.myTable.ColumnAnswerReuse] = value;
                }
            }
        }
        
        /// 
        public Boolean AnswerProcessedClientSide
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnswerProcessedClientSide.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedClientSide) 
                            || (((Boolean)(this[this.myTable.ColumnAnswerProcessedClientSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedClientSide] = value;
                }
            }
        }
        
        /// 
        public Boolean AnswerProcessedServerSide
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnswerProcessedServerSide.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedServerSide) 
                            || (((Boolean)(this[this.myTable.ColumnAnswerProcessedServerSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedServerSide] = value;
                }
            }
        }
        
        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnSiteKeyOfSimilarLocation);
            this.SetNull(this.myTable.ColumnLocationKeyOfSimilarLocation);
            this.SetNull(this.myTable.ColumnUsedByNOtherPartners);
            this.SetNull(this.myTable.ColumnAnswerReuse);
            this.SetNull(this.myTable.ColumnAnswerProcessedClientSide);
            this.SetNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
        
        /// test for NULL value
        public bool IsSiteKeyOfSimilarLocationNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKeyOfSimilarLocation);
        }
        
        /// assign NULL value
        public void SetSiteKeyOfSimilarLocationNull()
        {
            this.SetNull(this.myTable.ColumnSiteKeyOfSimilarLocation);
        }
        
        /// test for NULL value
        public bool IsLocationKeyOfSimilarLocationNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKeyOfSimilarLocation);
        }
        
        /// assign NULL value
        public void SetLocationKeyOfSimilarLocationNull()
        {
            this.SetNull(this.myTable.ColumnLocationKeyOfSimilarLocation);
        }
        
        /// test for NULL value
        public bool IsUsedByNOtherPartnersNull()
        {
            return this.IsNull(this.myTable.ColumnUsedByNOtherPartners);
        }
        
        /// assign NULL value
        public void SetUsedByNOtherPartnersNull()
        {
            this.SetNull(this.myTable.ColumnUsedByNOtherPartners);
        }
        
        /// test for NULL value
        public bool IsAnswerReuseNull()
        {
            return this.IsNull(this.myTable.ColumnAnswerReuse);
        }
        
        /// assign NULL value
        public void SetAnswerReuseNull()
        {
            this.SetNull(this.myTable.ColumnAnswerReuse);
        }
        
        /// test for NULL value
        public bool IsAnswerProcessedClientSideNull()
        {
            return this.IsNull(this.myTable.ColumnAnswerProcessedClientSide);
        }
        
        /// assign NULL value
        public void SetAnswerProcessedClientSideNull()
        {
            this.SetNull(this.myTable.ColumnAnswerProcessedClientSide);
        }
        
        /// test for NULL value
        public bool IsAnswerProcessedServerSideNull()
        {
            return this.IsNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
        
        /// assign NULL value
        public void SetAnswerProcessedServerSideNull()
        {
            this.SetNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerAddressAggregateTDSChangePromotionParametersTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// This is the key that tell what site created the linked location
        public DataColumn ColumnSiteKey;
        
        /// 
        public DataColumn ColumnLocationKey;
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        
        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public DataColumn ColumnPartnerClass;
        
        /// 
        public DataColumn ColumnTelephoneNumber;
        
        /// 
        public DataColumn ColumnExtension;
        
        /// 
        public DataColumn ColumnFaxNumber;
        
        /// 
        public DataColumn ColumnFaxExtension;
        
        /// 
        public DataColumn ColumnAlternateTelephone;
        
        /// 
        public DataColumn ColumnMobileNumber;
        
        /// 
        public DataColumn ColumnEmailAddress;
        
        /// 
        public DataColumn ColumnUrl;
        
        /// 
        public DataColumn ColumnSendMail;
        
        /// 
        public DataColumn ColumnDateEffective;
        
        /// 
        public DataColumn ColumnDateGoodUntil;
        
        /// 
        public DataColumn ColumnLocationType;
        
        /// 
        public DataColumn ColumnSiteKeyOfEditedRecord;
        
        /// 
        public DataColumn ColumnLocationKeyOfEditedRecord;
        
        /// auto generated
        public DataColumn[] FKPartnerLocation1;
        
        /// auto generated
        public DataColumn[] FKPartnerLocation2;
        
        /// auto generated
        public DataColumn[] FKPartner7;
        
        /// auto generated
        public DataColumn[] FKPartnerLocation3;
        
        /// constructor
        public PartnerAddressAggregateTDSChangePromotionParametersTable() : 
                base("ChangePromotionParameters")
        {
        }
        
        /// constructor
        public PartnerAddressAggregateTDSChangePromotionParametersTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerAddressAggregateTDSChangePromotionParametersTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerAddressAggregateTDSChangePromotionParametersRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSChangePromotionParametersRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }
        
        /// get help text for column
        public static string GetSiteKeyHelp()
        {
            return "Enter the site key";
        }
        
        /// get label of column
        public static string GetSiteKeyLabel()
        {
            return "Site Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }
        
        /// get help text for column
        public static string GetLocationKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetLocationKeyLabel()
        {
            return "Location Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }
        
        /// get help text for column
        public static string GetPartnerShortNameHelp()
        {
            return "Enter a short name for this partner";
        }
        
        /// get label of column
        public static string GetPartnerShortNameLabel()
        {
            return "Short Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "p_partner_class_c";
        }
        
        /// get help text for column
        public static string GetPartnerClassHelp()
        {
            return "Select a partner class";
        }
        
        /// get label of column
        public static string GetPartnerClassLabel()
        {
            return "Partner Class";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTelephoneNumberDBName()
        {
            return "p_telephone_number_c";
        }
        
        /// get help text for column
        public static string GetTelephoneNumberHelp()
        {
            return "Enter a Telephone number for this address (if available)";
        }
        
        /// get label of column
        public static string GetTelephoneNumberLabel()
        {
            return "Phone";
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtensionDBName()
        {
            return "p_extension_i";
        }
        
        /// get help text for column
        public static string GetExtensionHelp()
        {
            return "Enter the Telephone Extension (if available)";
        }
        
        /// get label of column
        public static string GetExtensionLabel()
        {
            return "Phone Extension";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFaxNumberDBName()
        {
            return "p_fax_number_c";
        }
        
        /// get help text for column
        public static string GetFaxNumberHelp()
        {
            return "Enter a Fax number for this address location (if available)";
        }
        
        /// get label of column
        public static string GetFaxNumberLabel()
        {
            return "Fax";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFaxExtensionDBName()
        {
            return "p_fax_extension_i";
        }
        
        /// get help text for column
        public static string GetFaxExtensionHelp()
        {
            return "Enter the Fax Extension (if available)";
        }
        
        /// get label of column
        public static string GetFaxExtensionLabel()
        {
            return "Fax Extension";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAlternateTelephoneDBName()
        {
            return "p_alternate_telephone_c";
        }
        
        /// get help text for column
        public static string GetAlternateTelephoneHelp()
        {
            return "Enter an Alternative phone number (if available)";
        }
        
        /// get label of column
        public static string GetAlternateTelephoneLabel()
        {
            return "Alternate";
        }
        
        /// get the name of the field in the database for this column
        public static string GetMobileNumberDBName()
        {
            return "p_mobile_number_c";
        }
        
        /// get help text for column
        public static string GetMobileNumberHelp()
        {
            return "Enter a Mobile phone number for this address (if available)";
        }
        
        /// get label of column
        public static string GetMobileNumberLabel()
        {
            return "Mobile";
        }
        
        /// get the name of the field in the database for this column
        public static string GetEmailAddressDBName()
        {
            return "p_email_address_c";
        }
        
        /// get help text for column
        public static string GetEmailAddressHelp()
        {
            return "Enter an Email address (if available)";
        }
        
        /// get label of column
        public static string GetEmailAddressLabel()
        {
            return "Email";
        }
        
        /// get the name of the field in the database for this column
        public static string GetUrlDBName()
        {
            return "p_url_c";
        }
        
        /// get help text for column
        public static string GetUrlHelp()
        {
            return "URL for this Partner\'s Web Home Page";
        }
        
        /// get label of column
        public static string GetUrlLabel()
        {
            return "Website";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSendMailDBName()
        {
            return "p_send_mail_l";
        }
        
        /// get help text for column
        public static string GetSendMailHelp()
        {
            return "Can or should, postal mail be sent to this address?";
        }
        
        /// get label of column
        public static string GetSendMailLabel()
        {
            return "Mailing Address";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateEffectiveDBName()
        {
            return "p_date_effective_d";
        }
        
        /// get help text for column
        public static string GetDateEffectiveHelp()
        {
            return "Enter the date this address is effective";
        }
        
        /// get label of column
        public static string GetDateEffectiveLabel()
        {
            return "Valid From";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateGoodUntilDBName()
        {
            return "p_date_good_until_d";
        }
        
        /// get help text for column
        public static string GetDateGoodUntilHelp()
        {
            return "Enter the date this address expires on";
        }
        
        /// get label of column
        public static string GetDateGoodUntilLabel()
        {
            return "Valid To";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationTypeDBName()
        {
            return "p_location_type_c";
        }
        
        /// get help text for column
        public static string GetLocationTypeHelp()
        {
            return "Select the address location type";
        }
        
        /// get label of column
        public static string GetLocationTypeLabel()
        {
            return "Location Type";
        }
        
        /// get the name of the field in the database for this column
        public static string GetSiteKeyOfEditedRecordDBName()
        {
            return "SiteKeyOfEditedRecord";
        }
        
        /// get help text for column
        public static string GetSiteKeyOfEditedRecordHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetSiteKeyOfEditedRecordLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationKeyOfEditedRecordDBName()
        {
            return "LocationKeyOfEditedRecord";
        }
        
        /// get help text for column
        public static string GetLocationKeyOfEditedRecordHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLocationKeyOfEditedRecordLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "ChangePromotionParameters";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "ChangePromotionParameters";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnPartnerClass = this.Columns["p_partner_class_c"];
            this.ColumnTelephoneNumber = this.Columns["p_telephone_number_c"];
            this.ColumnExtension = this.Columns["p_extension_i"];
            this.ColumnFaxNumber = this.Columns["p_fax_number_c"];
            this.ColumnFaxExtension = this.Columns["p_fax_extension_i"];
            this.ColumnAlternateTelephone = this.Columns["p_alternate_telephone_c"];
            this.ColumnMobileNumber = this.Columns["p_mobile_number_c"];
            this.ColumnEmailAddress = this.Columns["p_email_address_c"];
            this.ColumnUrl = this.Columns["p_url_c"];
            this.ColumnSendMail = this.Columns["p_send_mail_l"];
            this.ColumnDateEffective = this.Columns["p_date_effective_d"];
            this.ColumnDateGoodUntil = this.Columns["p_date_good_until_d"];
            this.ColumnLocationType = this.Columns["p_location_type_c"];
            this.ColumnSiteKeyOfEditedRecord = this.Columns["SiteKeyOfEditedRecord"];
            this.ColumnLocationKeyOfEditedRecord = this.Columns["LocationKeyOfEditedRecord"];
            this.FKPartnerLocation1 = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey};
            this.FKPartnerLocation2 = new System.Data.DataColumn[] {
                    this.ColumnSiteKey,
                    this.ColumnLocationKey};
            this.FKPartner7 = new System.Data.DataColumn[] {
                    this.ColumnPartnerClass};
            this.FKPartnerLocation3 = new System.Data.DataColumn[] {
                    this.ColumnLocationType};
        }
        
        /// create a new typed row
        public PartnerAddressAggregateTDSChangePromotionParametersRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerAddressAggregateTDSChangePromotionParametersRow ret = ((PartnerAddressAggregateTDSChangePromotionParametersRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSChangePromotionParametersRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_class_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_telephone_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_extension_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_fax_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_fax_extension_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_alternate_telephone_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mobile_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_url_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_send_mail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_date_effective_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_date_good_until_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_location_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("SiteKeyOfEditedRecord", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("LocationKeyOfEditedRecord", typeof(Int32)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnSiteKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnLocationKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerShortName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnPartnerClass))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnTelephoneNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnExtension))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnFaxNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnFaxExtension))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAlternateTelephone))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnMobileNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnEmailAddress))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 120);
            }
            if ((ACol == ColumnUrl))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 128);
            }
            if ((ACol == ColumnSendMail))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDateEffective))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateGoodUntil))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnLocationType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnSiteKeyOfEditedRecord))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLocationKeyOfEditedRecord))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerAddressAggregateTDSChangePromotionParametersRow : System.Data.DataRow
    {
        
        private PartnerAddressAggregateTDSChangePromotionParametersTable myTable;
        
        /// Constructor
        public PartnerAddressAggregateTDSChangePromotionParametersRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerAddressAggregateTDSChangePromotionParametersTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// This is the key that tell what site created the linked location
        public Int64 SiteKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSiteKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSiteKey) 
                            || (((Int64)(this[this.myTable.ColumnSiteKey])) != value)))
                {
                    this[this.myTable.ColumnSiteKey] = value;
                }
            }
        }
        
        /// 
        public Int32 LocationKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKey) 
                            || (((Int32)(this[this.myTable.ColumnLocationKey])) != value)))
                {
                    this[this.myTable.ColumnLocationKey] = value;
                }
            }
        }
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public String PartnerShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerShortName) 
                            || (((String)(this[this.myTable.ColumnPartnerShortName])) != value)))
                {
                    this[this.myTable.ColumnPartnerShortName] = value;
                }
            }
        }
        
        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public String PartnerClass
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerClass.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerClass) 
                            || (((String)(this[this.myTable.ColumnPartnerClass])) != value)))
                {
                    this[this.myTable.ColumnPartnerClass] = value;
                }
            }
        }
        
        /// 
        public String TelephoneNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTelephoneNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTelephoneNumber) 
                            || (((String)(this[this.myTable.ColumnTelephoneNumber])) != value)))
                {
                    this[this.myTable.ColumnTelephoneNumber] = value;
                }
            }
        }
        
        /// 
        public Int32 Extension
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtension.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtension) 
                            || (((Int32)(this[this.myTable.ColumnExtension])) != value)))
                {
                    this[this.myTable.ColumnExtension] = value;
                }
            }
        }
        
        /// 
        public String FaxNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFaxNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFaxNumber) 
                            || (((String)(this[this.myTable.ColumnFaxNumber])) != value)))
                {
                    this[this.myTable.ColumnFaxNumber] = value;
                }
            }
        }
        
        /// 
        public Int32 FaxExtension
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFaxExtension.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFaxExtension) 
                            || (((Int32)(this[this.myTable.ColumnFaxExtension])) != value)))
                {
                    this[this.myTable.ColumnFaxExtension] = value;
                }
            }
        }
        
        /// 
        public String AlternateTelephone
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAlternateTelephone.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAlternateTelephone) 
                            || (((String)(this[this.myTable.ColumnAlternateTelephone])) != value)))
                {
                    this[this.myTable.ColumnAlternateTelephone] = value;
                }
            }
        }
        
        /// 
        public String MobileNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMobileNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMobileNumber) 
                            || (((String)(this[this.myTable.ColumnMobileNumber])) != value)))
                {
                    this[this.myTable.ColumnMobileNumber] = value;
                }
            }
        }
        
        /// 
        public String EmailAddress
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEmailAddress.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEmailAddress) 
                            || (((String)(this[this.myTable.ColumnEmailAddress])) != value)))
                {
                    this[this.myTable.ColumnEmailAddress] = value;
                }
            }
        }
        
        /// 
        public String Url
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUrl.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUrl) 
                            || (((String)(this[this.myTable.ColumnUrl])) != value)))
                {
                    this[this.myTable.ColumnUrl] = value;
                }
            }
        }
        
        /// 
        public Boolean SendMail
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSendMail.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSendMail) 
                            || (((Boolean)(this[this.myTable.ColumnSendMail])) != value)))
                {
                    this[this.myTable.ColumnSendMail] = value;
                }
            }
        }
        
        /// 
        public System.DateTime DateEffective
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateEffective.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateEffective) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateEffective])) != value)))
                {
                    this[this.myTable.ColumnDateEffective] = value;
                }
            }
        }
        
        /// 
        public System.DateTime DateGoodUntil
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateGoodUntil.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateGoodUntil) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateGoodUntil])) != value)))
                {
                    this[this.myTable.ColumnDateGoodUntil] = value;
                }
            }
        }
        
        /// 
        public String LocationType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationType) 
                            || (((String)(this[this.myTable.ColumnLocationType])) != value)))
                {
                    this[this.myTable.ColumnLocationType] = value;
                }
            }
        }
        
        /// 
        public Int64 SiteKeyOfEditedRecord
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSiteKeyOfEditedRecord.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSiteKeyOfEditedRecord) 
                            || (((Int64)(this[this.myTable.ColumnSiteKeyOfEditedRecord])) != value)))
                {
                    this[this.myTable.ColumnSiteKeyOfEditedRecord] = value;
                }
            }
        }
        
        /// 
        public Int32 LocationKeyOfEditedRecord
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKeyOfEditedRecord.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKeyOfEditedRecord) 
                            || (((Int32)(this[this.myTable.ColumnLocationKeyOfEditedRecord])) != value)))
                {
                    this[this.myTable.ColumnLocationKeyOfEditedRecord] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPartnerShortName);
            this.SetNull(this.myTable.ColumnPartnerClass);
            this.SetNull(this.myTable.ColumnTelephoneNumber);
            this[this.myTable.ColumnExtension.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnFaxNumber);
            this[this.myTable.ColumnFaxExtension.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAlternateTelephone);
            this.SetNull(this.myTable.ColumnMobileNumber);
            this.SetNull(this.myTable.ColumnEmailAddress);
            this.SetNull(this.myTable.ColumnUrl);
            this[this.myTable.ColumnSendMail.Ordinal] = false;
            this.SetNull(this.myTable.ColumnDateEffective);
            this.SetNull(this.myTable.ColumnDateGoodUntil);
            this.SetNull(this.myTable.ColumnLocationType);
            this.SetNull(this.myTable.ColumnSiteKeyOfEditedRecord);
            this.SetNull(this.myTable.ColumnLocationKeyOfEditedRecord);
        }
        
        /// test for NULL value
        public bool IsPartnerShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// assign NULL value
        public void SetPartnerShortNameNull()
        {
            this.SetNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// test for NULL value
        public bool IsPartnerClassNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerClass);
        }
        
        /// assign NULL value
        public void SetPartnerClassNull()
        {
            this.SetNull(this.myTable.ColumnPartnerClass);
        }
        
        /// test for NULL value
        public bool IsTelephoneNumberNull()
        {
            return this.IsNull(this.myTable.ColumnTelephoneNumber);
        }
        
        /// assign NULL value
        public void SetTelephoneNumberNull()
        {
            this.SetNull(this.myTable.ColumnTelephoneNumber);
        }
        
        /// test for NULL value
        public bool IsExtensionNull()
        {
            return this.IsNull(this.myTable.ColumnExtension);
        }
        
        /// assign NULL value
        public void SetExtensionNull()
        {
            this.SetNull(this.myTable.ColumnExtension);
        }
        
        /// test for NULL value
        public bool IsFaxNumberNull()
        {
            return this.IsNull(this.myTable.ColumnFaxNumber);
        }
        
        /// assign NULL value
        public void SetFaxNumberNull()
        {
            this.SetNull(this.myTable.ColumnFaxNumber);
        }
        
        /// test for NULL value
        public bool IsFaxExtensionNull()
        {
            return this.IsNull(this.myTable.ColumnFaxExtension);
        }
        
        /// assign NULL value
        public void SetFaxExtensionNull()
        {
            this.SetNull(this.myTable.ColumnFaxExtension);
        }
        
        /// test for NULL value
        public bool IsAlternateTelephoneNull()
        {
            return this.IsNull(this.myTable.ColumnAlternateTelephone);
        }
        
        /// assign NULL value
        public void SetAlternateTelephoneNull()
        {
            this.SetNull(this.myTable.ColumnAlternateTelephone);
        }
        
        /// test for NULL value
        public bool IsMobileNumberNull()
        {
            return this.IsNull(this.myTable.ColumnMobileNumber);
        }
        
        /// assign NULL value
        public void SetMobileNumberNull()
        {
            this.SetNull(this.myTable.ColumnMobileNumber);
        }
        
        /// test for NULL value
        public bool IsEmailAddressNull()
        {
            return this.IsNull(this.myTable.ColumnEmailAddress);
        }
        
        /// assign NULL value
        public void SetEmailAddressNull()
        {
            this.SetNull(this.myTable.ColumnEmailAddress);
        }
        
        /// test for NULL value
        public bool IsUrlNull()
        {
            return this.IsNull(this.myTable.ColumnUrl);
        }
        
        /// assign NULL value
        public void SetUrlNull()
        {
            this.SetNull(this.myTable.ColumnUrl);
        }
        
        /// test for NULL value
        public bool IsSendMailNull()
        {
            return this.IsNull(this.myTable.ColumnSendMail);
        }
        
        /// assign NULL value
        public void SetSendMailNull()
        {
            this.SetNull(this.myTable.ColumnSendMail);
        }
        
        /// test for NULL value
        public bool IsDateEffectiveNull()
        {
            return this.IsNull(this.myTable.ColumnDateEffective);
        }
        
        /// assign NULL value
        public void SetDateEffectiveNull()
        {
            this.SetNull(this.myTable.ColumnDateEffective);
        }
        
        /// test for NULL value
        public bool IsDateGoodUntilNull()
        {
            return this.IsNull(this.myTable.ColumnDateGoodUntil);
        }
        
        /// assign NULL value
        public void SetDateGoodUntilNull()
        {
            this.SetNull(this.myTable.ColumnDateGoodUntil);
        }
        
        /// test for NULL value
        public bool IsLocationTypeNull()
        {
            return this.IsNull(this.myTable.ColumnLocationType);
        }
        
        /// assign NULL value
        public void SetLocationTypeNull()
        {
            this.SetNull(this.myTable.ColumnLocationType);
        }
        
        /// test for NULL value
        public bool IsSiteKeyOfEditedRecordNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKeyOfEditedRecord);
        }
        
        /// assign NULL value
        public void SetSiteKeyOfEditedRecordNull()
        {
            this.SetNull(this.myTable.ColumnSiteKeyOfEditedRecord);
        }
        
        /// test for NULL value
        public bool IsLocationKeyOfEditedRecordNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKeyOfEditedRecord);
        }
        
        /// assign NULL value
        public void SetLocationKeyOfEditedRecordNull()
        {
            this.SetNull(this.myTable.ColumnLocationKeyOfEditedRecord);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable : TTypedDataTable
    {
        
        /// This is the key that tell what site created this location, it will help to merge addresses when doing imports
        public DataColumn ColumnSiteKey;
        
        /// 
        public DataColumn ColumnLocationKey;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// 
        public DataColumn ColumnLocationChange;
        
        /// 
        public DataColumn ColumnPartnerLocationChange;
        
        /// 
        public DataColumn ColumnLocationAdded;
        
        /// 
        public DataColumn ColumnChangedFields;
        
        /// 
        public DataColumn ColumnUserAnswer;
        
        /// 
        public DataColumn ColumnAnswerProcessedClientSide;
        
        /// 
        public DataColumn ColumnAnswerProcessedServerSide;
        
        /// constructor
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable() : 
                base("AddressAddedOrChangedPromotion")
        {
        }
        
        /// constructor
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }
        
        /// get help text for column
        public static string GetSiteKeyHelp()
        {
            return "Enter the site key";
        }
        
        /// get label of column
        public static string GetSiteKeyLabel()
        {
            return "Site Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }
        
        /// get help text for column
        public static string GetLocationKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetLocationKeyLabel()
        {
            return "Location Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationChangeDBName()
        {
            return "LocationChange";
        }
        
        /// get help text for column
        public static string GetLocationChangeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLocationChangeLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerLocationChangeDBName()
        {
            return "PartnerLocationChange";
        }
        
        /// get help text for column
        public static string GetPartnerLocationChangeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetPartnerLocationChangeLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationAddedDBName()
        {
            return "LocationAdded";
        }
        
        /// get help text for column
        public static string GetLocationAddedHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLocationAddedLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetChangedFieldsDBName()
        {
            return "ChangedFields";
        }
        
        /// get help text for column
        public static string GetChangedFieldsHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetChangedFieldsLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetUserAnswerDBName()
        {
            return "UserAnswer";
        }
        
        /// get help text for column
        public static string GetUserAnswerHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetUserAnswerLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedClientSideDBName()
        {
            return "AnswerProcessedClientSide";
        }
        
        /// get help text for column
        public static string GetAnswerProcessedClientSideHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnswerProcessedClientSideLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedServerSideDBName()
        {
            return "AnswerProcessedServerSide";
        }
        
        /// get help text for column
        public static string GetAnswerProcessedServerSideHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAnswerProcessedServerSideLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AddressAddedOrChangedPromotion";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "AddressAddedOrChangedPromotion";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnLocationChange = this.Columns["LocationChange"];
            this.ColumnPartnerLocationChange = this.Columns["PartnerLocationChange"];
            this.ColumnLocationAdded = this.Columns["LocationAdded"];
            this.ColumnChangedFields = this.Columns["ChangedFields"];
            this.ColumnUserAnswer = this.Columns["UserAnswer"];
            this.ColumnAnswerProcessedClientSide = this.Columns["AnswerProcessedClientSide"];
            this.ColumnAnswerProcessedServerSide = this.Columns["AnswerProcessedServerSide"];
        }
        
        /// create a new typed row
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow ret = ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("LocationChange", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("PartnerLocationChange", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("LocationAdded", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("ChangedFields", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("UserAnswer", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedClientSide", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedServerSide", typeof(Boolean)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnSiteKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnLocationKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnLocationChange))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerLocationChange))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLocationAdded))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnChangedFields))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnUserAnswer))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnswerProcessedClientSide))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAnswerProcessedServerSide))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow : System.Data.DataRow
    {
        
        private PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable myTable;
        
        /// Constructor
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)(this.Table));
        }
        
        /// This is the key that tell what site created this location, it will help to merge addresses when doing imports
        public Int64 SiteKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSiteKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSiteKey) 
                            || (((Int64)(this[this.myTable.ColumnSiteKey])) != value)))
                {
                    this[this.myTable.ColumnSiteKey] = value;
                }
            }
        }
        
        /// 
        public Int32 LocationKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKey) 
                            || (((Int32)(this[this.myTable.ColumnLocationKey])) != value)))
                {
                    this[this.myTable.ColumnLocationKey] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// 
        public Boolean LocationChange
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationChange.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationChange) 
                            || (((Boolean)(this[this.myTable.ColumnLocationChange])) != value)))
                {
                    this[this.myTable.ColumnLocationChange] = value;
                }
            }
        }
        
        /// 
        public Boolean PartnerLocationChange
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerLocationChange.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerLocationChange) 
                            || (((Boolean)(this[this.myTable.ColumnPartnerLocationChange])) != value)))
                {
                    this[this.myTable.ColumnPartnerLocationChange] = value;
                }
            }
        }
        
        /// 
        public Boolean LocationAdded
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationAdded.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationAdded) 
                            || (((Boolean)(this[this.myTable.ColumnLocationAdded])) != value)))
                {
                    this[this.myTable.ColumnLocationAdded] = value;
                }
            }
        }
        
        /// 
        public String ChangedFields
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChangedFields.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChangedFields) 
                            || (((String)(this[this.myTable.ColumnChangedFields])) != value)))
                {
                    this[this.myTable.ColumnChangedFields] = value;
                }
            }
        }
        
        /// 
        public String UserAnswer
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUserAnswer.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUserAnswer) 
                            || (((String)(this[this.myTable.ColumnUserAnswer])) != value)))
                {
                    this[this.myTable.ColumnUserAnswer] = value;
                }
            }
        }
        
        /// 
        public Boolean AnswerProcessedClientSide
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnswerProcessedClientSide.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedClientSide) 
                            || (((Boolean)(this[this.myTable.ColumnAnswerProcessedClientSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedClientSide] = value;
                }
            }
        }
        
        /// 
        public Boolean AnswerProcessedServerSide
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAnswerProcessedServerSide.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedServerSide) 
                            || (((Boolean)(this[this.myTable.ColumnAnswerProcessedServerSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedServerSide] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnLocationChange);
            this.SetNull(this.myTable.ColumnPartnerLocationChange);
            this.SetNull(this.myTable.ColumnLocationAdded);
            this.SetNull(this.myTable.ColumnChangedFields);
            this.SetNull(this.myTable.ColumnUserAnswer);
            this.SetNull(this.myTable.ColumnAnswerProcessedClientSide);
            this.SetNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
        
        /// test for NULL value
        public bool IsLocationChangeNull()
        {
            return this.IsNull(this.myTable.ColumnLocationChange);
        }
        
        /// assign NULL value
        public void SetLocationChangeNull()
        {
            this.SetNull(this.myTable.ColumnLocationChange);
        }
        
        /// test for NULL value
        public bool IsPartnerLocationChangeNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerLocationChange);
        }
        
        /// assign NULL value
        public void SetPartnerLocationChangeNull()
        {
            this.SetNull(this.myTable.ColumnPartnerLocationChange);
        }
        
        /// test for NULL value
        public bool IsLocationAddedNull()
        {
            return this.IsNull(this.myTable.ColumnLocationAdded);
        }
        
        /// assign NULL value
        public void SetLocationAddedNull()
        {
            this.SetNull(this.myTable.ColumnLocationAdded);
        }
        
        /// test for NULL value
        public bool IsChangedFieldsNull()
        {
            return this.IsNull(this.myTable.ColumnChangedFields);
        }
        
        /// assign NULL value
        public void SetChangedFieldsNull()
        {
            this.SetNull(this.myTable.ColumnChangedFields);
        }
        
        /// test for NULL value
        public bool IsUserAnswerNull()
        {
            return this.IsNull(this.myTable.ColumnUserAnswer);
        }
        
        /// assign NULL value
        public void SetUserAnswerNull()
        {
            this.SetNull(this.myTable.ColumnUserAnswer);
        }
        
        /// test for NULL value
        public bool IsAnswerProcessedClientSideNull()
        {
            return this.IsNull(this.myTable.ColumnAnswerProcessedClientSide);
        }
        
        /// assign NULL value
        public void SetAnswerProcessedClientSideNull()
        {
            this.SetNull(this.myTable.ColumnAnswerProcessedClientSide);
        }
        
        /// test for NULL value
        public bool IsAnswerProcessedServerSideNull()
        {
            return this.IsNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
        
        /// assign NULL value
        public void SetAnswerProcessedServerSideNull()
        {
            this.SetNull(this.myTable.ColumnAnswerProcessedServerSide);
        }
    }
    
    /// auto generated
    [Serializable()]
    public class PartnerAddressAggregateTDS : TTypedDataSet
    {
        
        private PartnerAddressAggregateTDSSimilarLocationParametersTable TableSimilarLocationParameters;
        
        private PartnerAddressAggregateTDSChangePromotionParametersTable TableChangePromotionParameters;
        
        private PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable TableAddressAddedOrChangedPromotion;
        
        /// auto generated
        public PartnerAddressAggregateTDS() : 
                base("PartnerAddressAggregateTDS")
        {
        }
        
        /// auto generated for serialization
        public PartnerAddressAggregateTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public PartnerAddressAggregateTDS(string ADatasetName) : 
                base(ADatasetName)
        {
        }
        
        /// auto generated
        public PartnerAddressAggregateTDSSimilarLocationParametersTable SimilarLocationParameters
        {
            get
            {
                return this.TableSimilarLocationParameters;
            }
        }
        
        /// auto generated
        public PartnerAddressAggregateTDSChangePromotionParametersTable ChangePromotionParameters
        {
            get
            {
                return this.TableChangePromotionParameters;
            }
        }
        
        /// auto generated
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AddressAddedOrChangedPromotion
        {
            get
            {
                return this.TableAddressAddedOrChangedPromotion;
            }
        }
        
        /// auto generated
        public new virtual PartnerAddressAggregateTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerAddressAggregateTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PartnerAddressAggregateTDSSimilarLocationParametersTable("SimilarLocationParameters"));
            this.Tables.Add(new PartnerAddressAggregateTDSChangePromotionParametersTable("ChangePromotionParameters"));
            this.Tables.Add(new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable("AddressAddedOrChangedPromotion"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("SimilarLocationParameters") != -1))
            {
                this.Tables.Add(new PartnerAddressAggregateTDSSimilarLocationParametersTable("SimilarLocationParameters"));
            }
            if ((ds.Tables.IndexOf("ChangePromotionParameters") != -1))
            {
                this.Tables.Add(new PartnerAddressAggregateTDSChangePromotionParametersTable("ChangePromotionParameters"));
            }
            if ((ds.Tables.IndexOf("AddressAddedOrChangedPromotion") != -1))
            {
                this.Tables.Add(new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable("AddressAddedOrChangedPromotion"));
            }
        }
        
        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableSimilarLocationParameters != null))
            {
                this.TableSimilarLocationParameters.InitVars();
            }
            if ((this.TableChangePromotionParameters != null))
            {
                this.TableChangePromotionParameters.InitVars();
            }
            if ((this.TableAddressAddedOrChangedPromotion != null))
            {
                this.TableAddressAddedOrChangedPromotion.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerAddressAggregateTDS";
            this.TableSimilarLocationParameters = ((PartnerAddressAggregateTDSSimilarLocationParametersTable)(this.Tables["SimilarLocationParameters"]));
            this.TableChangePromotionParameters = ((PartnerAddressAggregateTDSChangePromotionParametersTable)(this.Tables["ChangePromotionParameters"]));
            this.TableAddressAddedOrChangedPromotion = ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)(this.Tables["AddressAddedOrChangedPromotion"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TableSimilarLocationParameters != null) 
                        && (this.TableChangePromotionParameters != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLocation2", "SimilarLocationParameters", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}, "ChangePromotionParameters", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}));
            }
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerInfoTDSPartnerHeadInfoTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        
        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public DataColumn ColumnPartnerClass;
        
        /// This code describes the status of a partner.
        ///Eg,  Active, Deceased etc
        public DataColumn ColumnStatusCode;
        
        /// This code identifies the method of aquisition.
        public DataColumn ColumnAcquisitionCode;
        
        /// The Petra user that the partner record is restricted to if p_restricted_i is 2.
        public DataColumn ColumnPrivatePartnerOwner;
        
        /// auto generated
        public DataColumn[] FKPartner7;
        
        /// auto generated
        public DataColumn[] FKPartner3;
        
        /// auto generated
        public DataColumn[] FKPartner1;
        
        /// constructor
        public PartnerInfoTDSPartnerHeadInfoTable() : 
                base("PartnerHeadInfo")
        {
        }
        
        /// constructor
        public PartnerInfoTDSPartnerHeadInfoTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerInfoTDSPartnerHeadInfoTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerInfoTDSPartnerHeadInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSPartnerHeadInfoRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }
        
        /// get help text for column
        public static string GetPartnerShortNameHelp()
        {
            return "Enter a short name for this partner";
        }
        
        /// get label of column
        public static string GetPartnerShortNameLabel()
        {
            return "Short Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "p_partner_class_c";
        }
        
        /// get help text for column
        public static string GetPartnerClassHelp()
        {
            return "Select a partner class";
        }
        
        /// get label of column
        public static string GetPartnerClassLabel()
        {
            return "Partner Class";
        }
        
        /// get the name of the field in the database for this column
        public static string GetStatusCodeDBName()
        {
            return "p_status_code_c";
        }
        
        /// get help text for column
        public static string GetStatusCodeHelp()
        {
            return "Select a partner status";
        }
        
        /// get label of column
        public static string GetStatusCodeLabel()
        {
            return "Partner Status";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAcquisitionCodeDBName()
        {
            return "p_acquisition_code_c";
        }
        
        /// get help text for column
        public static string GetAcquisitionCodeHelp()
        {
            return "Select a method-of-acquisition code";
        }
        
        /// get label of column
        public static string GetAcquisitionCodeLabel()
        {
            return "Acquisition Code";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPrivatePartnerOwnerDBName()
        {
            return "p_user_id_c";
        }
        
        /// get help text for column
        public static string GetPrivatePartnerOwnerHelp()
        {
            return "Select a corresponding User Restriction";
        }
        
        /// get label of column
        public static string GetPrivatePartnerOwnerLabel()
        {
            return "User ID";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "PartnerHeadInfo";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "PartnerHeadInfo";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnPartnerClass = this.Columns["p_partner_class_c"];
            this.ColumnStatusCode = this.Columns["p_status_code_c"];
            this.ColumnAcquisitionCode = this.Columns["p_acquisition_code_c"];
            this.ColumnPrivatePartnerOwner = this.Columns["p_user_id_c"];
            this.FKPartner7 = new System.Data.DataColumn[] {
                    this.ColumnPartnerClass};
            this.FKPartner3 = new System.Data.DataColumn[] {
                    this.ColumnStatusCode};
            this.FKPartner1 = new System.Data.DataColumn[] {
                    this.ColumnAcquisitionCode};
        }
        
        /// create a new typed row
        public PartnerInfoTDSPartnerHeadInfoRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerInfoTDSPartnerHeadInfoRow ret = ((PartnerInfoTDSPartnerHeadInfoRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSPartnerHeadInfoRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_class_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_status_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_acquisition_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_user_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPartnerShortName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnPartnerClass))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnStatusCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnAcquisitionCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnPrivatePartnerOwner))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerInfoTDSPartnerHeadInfoRow : System.Data.DataRow
    {
        
        private PartnerInfoTDSPartnerHeadInfoTable myTable;
        
        /// Constructor
        public PartnerInfoTDSPartnerHeadInfoRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerInfoTDSPartnerHeadInfoTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public String PartnerShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerShortName) 
                            || (((String)(this[this.myTable.ColumnPartnerShortName])) != value)))
                {
                    this[this.myTable.ColumnPartnerShortName] = value;
                }
            }
        }
        
        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public String PartnerClass
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerClass.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerClass) 
                            || (((String)(this[this.myTable.ColumnPartnerClass])) != value)))
                {
                    this[this.myTable.ColumnPartnerClass] = value;
                }
            }
        }
        
        /// This code describes the status of a partner.
        ///Eg,  Active, Deceased etc
        public String StatusCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStatusCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStatusCode) 
                            || (((String)(this[this.myTable.ColumnStatusCode])) != value)))
                {
                    this[this.myTable.ColumnStatusCode] = value;
                }
            }
        }
        
        /// This code identifies the method of aquisition.
        public String AcquisitionCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAcquisitionCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAcquisitionCode) 
                            || (((String)(this[this.myTable.ColumnAcquisitionCode])) != value)))
                {
                    this[this.myTable.ColumnAcquisitionCode] = value;
                }
            }
        }
        
        /// The Petra user that the partner record is restricted to if p_restricted_i is 2.
        public String PrivatePartnerOwner
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPrivatePartnerOwner.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPrivatePartnerOwner) 
                            || (((String)(this[this.myTable.ColumnPrivatePartnerOwner])) != value)))
                {
                    this[this.myTable.ColumnPrivatePartnerOwner] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPartnerShortName);
            this.SetNull(this.myTable.ColumnPartnerClass);
            this.SetNull(this.myTable.ColumnStatusCode);
            this.SetNull(this.myTable.ColumnAcquisitionCode);
            this.SetNull(this.myTable.ColumnPrivatePartnerOwner);
        }
        
        /// test for NULL value
        public bool IsPartnerShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// assign NULL value
        public void SetPartnerShortNameNull()
        {
            this.SetNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// test for NULL value
        public bool IsPartnerClassNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerClass);
        }
        
        /// assign NULL value
        public void SetPartnerClassNull()
        {
            this.SetNull(this.myTable.ColumnPartnerClass);
        }
        
        /// test for NULL value
        public bool IsStatusCodeNull()
        {
            return this.IsNull(this.myTable.ColumnStatusCode);
        }
        
        /// assign NULL value
        public void SetStatusCodeNull()
        {
            this.SetNull(this.myTable.ColumnStatusCode);
        }
        
        /// test for NULL value
        public bool IsAcquisitionCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAcquisitionCode);
        }
        
        /// assign NULL value
        public void SetAcquisitionCodeNull()
        {
            this.SetNull(this.myTable.ColumnAcquisitionCode);
        }
        
        /// test for NULL value
        public bool IsPrivatePartnerOwnerNull()
        {
            return this.IsNull(this.myTable.ColumnPrivatePartnerOwner);
        }
        
        /// assign NULL value
        public void SetPrivatePartnerOwnerNull()
        {
            this.SetNull(this.myTable.ColumnPrivatePartnerOwner);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerInfoTDSPartnerAdditionalInfoTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnMainLanguages;
        
        /// 
        public DataColumn ColumnAdditionalLanguages;
        
        /// Date of contact
        public DataColumn ColumnLastContact;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// This is the date the rthe person was born
        public DataColumn ColumnDateOfBirth;
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnFamily;
        
        /// A cross reference to the family record of this person.
        ///It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
        public DataColumn ColumnFamilyKey;
        
        /// 
        public DataColumn ColumnPreviousName;
        
        /// 
        public DataColumn ColumnField;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnFieldKey;
        
        /// Additional information about the partner that is important to store in the database.
        public DataColumn ColumnNotes;
        
        /// auto generated
        public DataColumn[] FKPerson2;
        
        /// auto generated
        public DataColumn[] FKUnit1;
        
        /// constructor
        public PartnerInfoTDSPartnerAdditionalInfoTable() : 
                base("PartnerAdditionalInfo")
        {
        }
        
        /// constructor
        public PartnerInfoTDSPartnerAdditionalInfoTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerInfoTDSPartnerAdditionalInfoTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerInfoTDSPartnerAdditionalInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSPartnerAdditionalInfoRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetMainLanguagesDBName()
        {
            return "MainLanguages";
        }
        
        /// get help text for column
        public static string GetMainLanguagesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetMainLanguagesLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetAdditionalLanguagesDBName()
        {
            return "AdditionalLanguages";
        }
        
        /// get help text for column
        public static string GetAdditionalLanguagesHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetAdditionalLanguagesLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastContactDBName()
        {
            return "s_contact_date_d";
        }
        
        /// get help text for column
        public static string GetLastContactHelp()
        {
            return "Enter the date the contact was made";
        }
        
        /// get label of column
        public static string GetLastContactLabel()
        {
            return "Contact Date";
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
        
        /// get the name of the field in the database for this column
        public static string GetDateOfBirthDBName()
        {
            return "p_date_of_birth_d";
        }
        
        /// get help text for column
        public static string GetDateOfBirthHelp()
        {
            return "Enter the date the person was born";
        }
        
        /// get label of column
        public static string GetDateOfBirthLabel()
        {
            return "Date of Birth";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFamilyDBName()
        {
            return "p_partner_short_name_c";
        }
        
        /// get help text for column
        public static string GetFamilyHelp()
        {
            return "Enter a short name for this partner";
        }
        
        /// get label of column
        public static string GetFamilyLabel()
        {
            return "Short Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFamilyKeyDBName()
        {
            return "p_family_key_n";
        }
        
        /// get help text for column
        public static string GetFamilyKeyHelp()
        {
            return "Enter the partner key of the Family record";
        }
        
        /// get label of column
        public static string GetFamilyKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPreviousNameDBName()
        {
            return "p_previous_name_c";
        }
        
        /// get help text for column
        public static string GetPreviousNameHelp()
        {
            return "Enter the previously used Surname (eg before marriage)";
        }
        
        /// get label of column
        public static string GetPreviousNameLabel()
        {
            return "Previous Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFieldDBName()
        {
            return "p_unit_name_c";
        }
        
        /// get help text for column
        public static string GetFieldHelp()
        {
            return "Enter the name of the unit";
        }
        
        /// get label of column
        public static string GetFieldLabel()
        {
            return "Unit Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFieldKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetFieldKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetFieldKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNotesDBName()
        {
            return "p_comment_c";
        }
        
        /// get help text for column
        public static string GetNotesHelp()
        {
            return "Enter any additional comments (if full, use notebook)";
        }
        
        /// get label of column
        public static string GetNotesLabel()
        {
            return "Comments";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "PartnerAdditionalInfo";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "PartnerAdditionalInfo";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMainLanguages = this.Columns["MainLanguages"];
            this.ColumnAdditionalLanguages = this.Columns["AdditionalLanguages"];
            this.ColumnLastContact = this.Columns["s_contact_date_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnDateOfBirth = this.Columns["p_date_of_birth_d"];
            this.ColumnFamily = this.Columns["p_partner_short_name_c"];
            this.ColumnFamilyKey = this.Columns["p_family_key_n"];
            this.ColumnPreviousName = this.Columns["p_previous_name_c"];
            this.ColumnField = this.Columns["p_unit_name_c"];
            this.ColumnFieldKey = this.Columns["p_partner_key_n"];
            this.ColumnNotes = this.Columns["p_comment_c"];
            this.FKPerson2 = new System.Data.DataColumn[] {
                    this.ColumnFamilyKey};
            this.FKUnit1 = new System.Data.DataColumn[] {
                    this.ColumnFieldKey};
        }
        
        /// create a new typed row
        public PartnerInfoTDSPartnerAdditionalInfoRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerInfoTDSPartnerAdditionalInfoRow ret = ((PartnerInfoTDSPartnerAdditionalInfoRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSPartnerAdditionalInfoRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("MainLanguages", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("AdditionalLanguages", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_contact_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_date_of_birth_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_previous_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_comment_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnMainLanguages))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAdditionalLanguages))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLastContact))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateOfBirth))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnFamily))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnFamilyKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPreviousName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 512);
            }
            if ((ACol == ColumnField))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnFieldKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnNotes))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 10000);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerInfoTDSPartnerAdditionalInfoRow : System.Data.DataRow
    {
        
        private PartnerInfoTDSPartnerAdditionalInfoTable myTable;
        
        /// Constructor
        public PartnerInfoTDSPartnerAdditionalInfoRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerInfoTDSPartnerAdditionalInfoTable)(this.Table));
        }
        
        /// 
        public String MainLanguages
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMainLanguages.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMainLanguages) 
                            || (((String)(this[this.myTable.ColumnMainLanguages])) != value)))
                {
                    this[this.myTable.ColumnMainLanguages] = value;
                }
            }
        }
        
        /// 
        public String AdditionalLanguages
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAdditionalLanguages.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAdditionalLanguages) 
                            || (((String)(this[this.myTable.ColumnAdditionalLanguages])) != value)))
                {
                    this[this.myTable.ColumnAdditionalLanguages] = value;
                }
            }
        }
        
        /// Date of contact
        public System.DateTime LastContact
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastContact.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastContact) 
                            || (((System.DateTime)(this[this.myTable.ColumnLastContact])) != value)))
                {
                    this[this.myTable.ColumnLastContact] = value;
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
        
        /// This is the date the rthe person was born
        public System.DateTime DateOfBirth
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfBirth.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateOfBirth) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateOfBirth])) != value)))
                {
                    this[this.myTable.ColumnDateOfBirth] = value;
                }
            }
        }
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public String Family
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFamily.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFamily) 
                            || (((String)(this[this.myTable.ColumnFamily])) != value)))
                {
                    this[this.myTable.ColumnFamily] = value;
                }
            }
        }
        
        /// A cross reference to the family record of this person.
        ///It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
        public Int64 FamilyKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFamilyKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFamilyKey) 
                            || (((Int64)(this[this.myTable.ColumnFamilyKey])) != value)))
                {
                    this[this.myTable.ColumnFamilyKey] = value;
                }
            }
        }
        
        /// 
        public String PreviousName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPreviousName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPreviousName) 
                            || (((String)(this[this.myTable.ColumnPreviousName])) != value)))
                {
                    this[this.myTable.ColumnPreviousName] = value;
                }
            }
        }
        
        /// 
        public String Field
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnField.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnField) 
                            || (((String)(this[this.myTable.ColumnField])) != value)))
                {
                    this[this.myTable.ColumnField] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 FieldKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFieldKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFieldKey) 
                            || (((Int64)(this[this.myTable.ColumnFieldKey])) != value)))
                {
                    this[this.myTable.ColumnFieldKey] = value;
                }
            }
        }
        
        /// Additional information about the partner that is important to store in the database.
        public String Notes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNotes.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNotes) 
                            || (((String)(this[this.myTable.ColumnNotes])) != value)))
                {
                    this[this.myTable.ColumnNotes] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnMainLanguages);
            this.SetNull(this.myTable.ColumnAdditionalLanguages);
            this.SetNull(this.myTable.ColumnLastContact);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnDateOfBirth);
            this.SetNull(this.myTable.ColumnFamily);
            this[this.myTable.ColumnFamilyKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPreviousName);
            this.SetNull(this.myTable.ColumnField);
            this[this.myTable.ColumnFieldKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnNotes);
        }
        
        /// test for NULL value
        public bool IsMainLanguagesNull()
        {
            return this.IsNull(this.myTable.ColumnMainLanguages);
        }
        
        /// assign NULL value
        public void SetMainLanguagesNull()
        {
            this.SetNull(this.myTable.ColumnMainLanguages);
        }
        
        /// test for NULL value
        public bool IsAdditionalLanguagesNull()
        {
            return this.IsNull(this.myTable.ColumnAdditionalLanguages);
        }
        
        /// assign NULL value
        public void SetAdditionalLanguagesNull()
        {
            this.SetNull(this.myTable.ColumnAdditionalLanguages);
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
        public bool IsDateOfBirthNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfBirth);
        }
        
        /// assign NULL value
        public void SetDateOfBirthNull()
        {
            this.SetNull(this.myTable.ColumnDateOfBirth);
        }
        
        /// test for NULL value
        public bool IsFamilyNull()
        {
            return this.IsNull(this.myTable.ColumnFamily);
        }
        
        /// assign NULL value
        public void SetFamilyNull()
        {
            this.SetNull(this.myTable.ColumnFamily);
        }
        
        /// test for NULL value
        public bool IsFamilyKeyNull()
        {
            return this.IsNull(this.myTable.ColumnFamilyKey);
        }
        
        /// assign NULL value
        public void SetFamilyKeyNull()
        {
            this.SetNull(this.myTable.ColumnFamilyKey);
        }
        
        /// test for NULL value
        public bool IsPreviousNameNull()
        {
            return this.IsNull(this.myTable.ColumnPreviousName);
        }
        
        /// assign NULL value
        public void SetPreviousNameNull()
        {
            this.SetNull(this.myTable.ColumnPreviousName);
        }
        
        /// test for NULL value
        public bool IsFieldNull()
        {
            return this.IsNull(this.myTable.ColumnField);
        }
        
        /// assign NULL value
        public void SetFieldNull()
        {
            this.SetNull(this.myTable.ColumnField);
        }
        
        /// test for NULL value
        public bool IsNotesNull()
        {
            return this.IsNull(this.myTable.ColumnNotes);
        }
        
        /// assign NULL value
        public void SetNotesNull()
        {
            this.SetNull(this.myTable.ColumnNotes);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerInfoTDSUnitInfoTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnParentUnitKey;
        
        /// 
        public DataColumn ColumnParentUnitName;
        
        /// auto generated
        public DataColumn[] FKUnit1;
        
        /// constructor
        public PartnerInfoTDSUnitInfoTable() : 
                base("UnitInfo")
        {
        }
        
        /// constructor
        public PartnerInfoTDSUnitInfoTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerInfoTDSUnitInfoTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerInfoTDSUnitInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSUnitInfoRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetParentUnitKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetParentUnitKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetParentUnitKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetParentUnitNameDBName()
        {
            return "p_unit_name_c";
        }
        
        /// get help text for column
        public static string GetParentUnitNameHelp()
        {
            return "Enter the name of the unit";
        }
        
        /// get label of column
        public static string GetParentUnitNameLabel()
        {
            return "Unit Name";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "UnitInfo";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "UnitInfo";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnParentUnitKey = this.Columns["p_partner_key_n"];
            this.ColumnParentUnitName = this.Columns["p_unit_name_c"];
            this.FKUnit1 = new System.Data.DataColumn[] {
                    this.ColumnParentUnitKey};
        }
        
        /// create a new typed row
        public PartnerInfoTDSUnitInfoRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerInfoTDSUnitInfoRow ret = ((PartnerInfoTDSUnitInfoRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSUnitInfoRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnParentUnitKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnParentUnitName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerInfoTDSUnitInfoRow : System.Data.DataRow
    {
        
        private PartnerInfoTDSUnitInfoTable myTable;
        
        /// Constructor
        public PartnerInfoTDSUnitInfoRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerInfoTDSUnitInfoTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public Int64 ParentUnitKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnParentUnitKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnParentUnitKey) 
                            || (((Int64)(this[this.myTable.ColumnParentUnitKey])) != value)))
                {
                    this[this.myTable.ColumnParentUnitKey] = value;
                }
            }
        }
        
        /// 
        public String ParentUnitName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnParentUnitName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnParentUnitName) 
                            || (((String)(this[this.myTable.ColumnParentUnitName])) != value)))
                {
                    this[this.myTable.ColumnParentUnitName] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnParentUnitKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnParentUnitName);
        }
        
        /// test for NULL value
        public bool IsParentUnitNameNull()
        {
            return this.IsNull(this.myTable.ColumnParentUnitName);
        }
        
        /// assign NULL value
        public void SetParentUnitNameNull()
        {
            this.SetNull(this.myTable.ColumnParentUnitName);
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class PartnerInfoTDSFamilyMembersTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        
        /// This field indicates the family id of the individual.
        ///ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
        public DataColumn ColumnFamilyId;
        
        /// constructor
        public PartnerInfoTDSFamilyMembersTable() : 
                base("FamilyMembers")
        {
        }
        
        /// constructor
        public PartnerInfoTDSFamilyMembersTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PartnerInfoTDSFamilyMembersTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PartnerInfoTDSFamilyMembersRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSFamilyMembersRow)(this.Rows[i]));
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
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }
        
        /// get help text for column
        public static string GetPartnerShortNameHelp()
        {
            return "Enter a short name for this partner";
        }
        
        /// get label of column
        public static string GetPartnerShortNameLabel()
        {
            return "Short Name";
        }
        
        /// get the name of the field in the database for this column
        public static string GetFamilyIdDBName()
        {
            return "p_family_id_i";
        }
        
        /// get help text for column
        public static string GetFamilyIdHelp()
        {
            return "This field indicates the family id of the individual.";
        }
        
        /// get label of column
        public static string GetFamilyIdLabel()
        {
            return "Family ID";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "FamilyMembers";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "FamilyMembers";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnFamilyId = this.Columns["p_family_id_i"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey};
        }
        
        /// create a new typed row
        public PartnerInfoTDSFamilyMembersRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerInfoTDSFamilyMembersRow ret = ((PartnerInfoTDSFamilyMembersRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSFamilyMembersRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_id_i", typeof(Int32)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPartnerShortName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnFamilyId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class PartnerInfoTDSFamilyMembersRow : System.Data.DataRow
    {
        
        private PartnerInfoTDSFamilyMembersTable myTable;
        
        /// Constructor
        public PartnerInfoTDSFamilyMembersRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PartnerInfoTDSFamilyMembersTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public String PartnerShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerShortName) 
                            || (((String)(this[this.myTable.ColumnPartnerShortName])) != value)))
                {
                    this[this.myTable.ColumnPartnerShortName] = value;
                }
            }
        }
        
        /// This field indicates the family id of the individual.
        ///ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
        public Int32 FamilyId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFamilyId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFamilyId) 
                            || (((Int32)(this[this.myTable.ColumnFamilyId])) != value)))
                {
                    this[this.myTable.ColumnFamilyId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPartnerShortName);
            this[this.myTable.ColumnFamilyId.Ordinal] = 0;
        }
        
        /// test for NULL value
        public bool IsPartnerShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// assign NULL value
        public void SetPartnerShortNameNull()
        {
            this.SetNull(this.myTable.ColumnPartnerShortName);
        }
        
        /// test for NULL value
        public bool IsFamilyIdNull()
        {
            return this.IsNull(this.myTable.ColumnFamilyId);
        }
        
        /// assign NULL value
        public void SetFamilyIdNull()
        {
            this.SetNull(this.myTable.ColumnFamilyId);
        }
    }
    
    /// auto generated
    [Serializable()]
    public class PartnerInfoTDS : TTypedDataSet
    {
        
        private PartnerInfoTDSPartnerHeadInfoTable TablePartnerHeadInfo;
        
        private PartnerInfoTDSPartnerAdditionalInfoTable TablePartnerAdditionalInfo;
        
        private PartnerInfoTDSUnitInfoTable TableUnitInfo;
        
        private PLocationTable TablePLocation;
        
        private PPartnerLocationTable TablePPartnerLocation;
        
        private PPartnerTypeTable TablePPartnerType;
        
        private PSubscriptionTable TablePSubscription;
        
        private PartnerInfoTDSFamilyMembersTable TableFamilyMembers;
        
        /// auto generated
        public PartnerInfoTDS() : 
                base("PartnerInfoTDS")
        {
        }
        
        /// auto generated for serialization
        public PartnerInfoTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public PartnerInfoTDS(string ADatasetName) : 
                base(ADatasetName)
        {
        }
        
        /// auto generated
        public PartnerInfoTDSPartnerHeadInfoTable PartnerHeadInfo
        {
            get
            {
                return this.TablePartnerHeadInfo;
            }
        }
        
        /// auto generated
        public PartnerInfoTDSPartnerAdditionalInfoTable PartnerAdditionalInfo
        {
            get
            {
                return this.TablePartnerAdditionalInfo;
            }
        }
        
        /// auto generated
        public PartnerInfoTDSUnitInfoTable UnitInfo
        {
            get
            {
                return this.TableUnitInfo;
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
        public PPartnerLocationTable PPartnerLocation
        {
            get
            {
                return this.TablePPartnerLocation;
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
        public PSubscriptionTable PSubscription
        {
            get
            {
                return this.TablePSubscription;
            }
        }
        
        /// auto generated
        public PartnerInfoTDSFamilyMembersTable FamilyMembers
        {
            get
            {
                return this.TableFamilyMembers;
            }
        }
        
        /// auto generated
        public new virtual PartnerInfoTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerInfoTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PartnerInfoTDSPartnerHeadInfoTable("PartnerHeadInfo"));
            this.Tables.Add(new PartnerInfoTDSPartnerAdditionalInfoTable("PartnerAdditionalInfo"));
            this.Tables.Add(new PartnerInfoTDSUnitInfoTable("UnitInfo"));
            this.Tables.Add(new PLocationTable("PLocation"));
            this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            this.Tables.Add(new PSubscriptionTable("PSubscription"));
            this.Tables.Add(new PartnerInfoTDSFamilyMembersTable("FamilyMembers"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PartnerHeadInfo") != -1))
            {
                this.Tables.Add(new PartnerInfoTDSPartnerHeadInfoTable("PartnerHeadInfo"));
            }
            if ((ds.Tables.IndexOf("PartnerAdditionalInfo") != -1))
            {
                this.Tables.Add(new PartnerInfoTDSPartnerAdditionalInfoTable("PartnerAdditionalInfo"));
            }
            if ((ds.Tables.IndexOf("UnitInfo") != -1))
            {
                this.Tables.Add(new PartnerInfoTDSUnitInfoTable("UnitInfo"));
            }
            if ((ds.Tables.IndexOf("PLocation") != -1))
            {
                this.Tables.Add(new PLocationTable("PLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerLocation") != -1))
            {
                this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerType") != -1))
            {
                this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            }
            if ((ds.Tables.IndexOf("PSubscription") != -1))
            {
                this.Tables.Add(new PSubscriptionTable("PSubscription"));
            }
            if ((ds.Tables.IndexOf("FamilyMembers") != -1))
            {
                this.Tables.Add(new PartnerInfoTDSFamilyMembersTable("FamilyMembers"));
            }
        }
        
        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePartnerHeadInfo != null))
            {
                this.TablePartnerHeadInfo.InitVars();
            }
            if ((this.TablePartnerAdditionalInfo != null))
            {
                this.TablePartnerAdditionalInfo.InitVars();
            }
            if ((this.TableUnitInfo != null))
            {
                this.TableUnitInfo.InitVars();
            }
            if ((this.TablePLocation != null))
            {
                this.TablePLocation.InitVars();
            }
            if ((this.TablePPartnerLocation != null))
            {
                this.TablePPartnerLocation.InitVars();
            }
            if ((this.TablePPartnerType != null))
            {
                this.TablePPartnerType.InitVars();
            }
            if ((this.TablePSubscription != null))
            {
                this.TablePSubscription.InitVars();
            }
            if ((this.TableFamilyMembers != null))
            {
                this.TableFamilyMembers.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerInfoTDS";
            this.TablePartnerHeadInfo = ((PartnerInfoTDSPartnerHeadInfoTable)(this.Tables["PartnerHeadInfo"]));
            this.TablePartnerAdditionalInfo = ((PartnerInfoTDSPartnerAdditionalInfoTable)(this.Tables["PartnerAdditionalInfo"]));
            this.TableUnitInfo = ((PartnerInfoTDSUnitInfoTable)(this.Tables["UnitInfo"]));
            this.TablePLocation = ((PLocationTable)(this.Tables["PLocation"]));
            this.TablePPartnerLocation = ((PPartnerLocationTable)(this.Tables["PPartnerLocation"]));
            this.TablePPartnerType = ((PPartnerTypeTable)(this.Tables["PPartnerType"]));
            this.TablePSubscription = ((PSubscriptionTable)(this.Tables["PSubscription"]));
            this.TableFamilyMembers = ((PartnerInfoTDSFamilyMembersTable)(this.Tables["FamilyMembers"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePLocation != null) 
                        && (this.TablePPartnerLocation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerLocation2", "PLocation", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}, "PPartnerLocation", new string[] {
                                "p_site_key_n",
                                "p_location_key_i"}));
            }
        }
    }
}
