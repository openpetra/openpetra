/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MHospitality.Data
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
    
    
    /// Details of building used for accomodation at a conference
    [Serializable()]
    public class PcBuildingTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnVenueKey;
        
        /// 
        public DataColumn ColumnBuildingCode;
        
        /// This is a long description and is 80 characters long.
        public DataColumn ColumnBuildingDesc;
        
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
        public PcBuildingTable() : 
                base("PcBuilding")
        {
        }
        
        /// constructor
        public PcBuildingTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PcBuildingTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PcBuildingRow this[int i]
        {
            get
            {
                return ((PcBuildingRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }
        
        /// get help text for column
        public static string GetVenueKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetVenueKeyLabel()
        {
            return "Venue Key";
        }
        
        /// get display format for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }
        
        /// get help text for column
        public static string GetBuildingCodeHelp()
        {
            return "Code to identify this building";
        }
        
        /// get label of column
        public static string GetBuildingCodeLabel()
        {
            return "Building Code";
        }
        
        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBuildingDescDBName()
        {
            return "pc_building_desc_c";
        }
        
        /// get help text for column
        public static string GetBuildingDescHelp()
        {
            return "Enter a description for the buiding";
        }
        
        /// get label of column
        public static string GetBuildingDescLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetBuildingDescLength()
        {
            return 80;
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
            return "PcBuilding";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "pc_building";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Building Information";
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
                    "p_venue_key_n",
                    "pc_building_code_c",
                    "pc_building_desc_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnVenueKey = this.Columns["p_venue_key_n"];
            this.ColumnBuildingCode = this.Columns["pc_building_code_c"];
            this.ColumnBuildingDesc = this.Columns["pc_building_desc_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnVenueKey,
                    this.ColumnBuildingCode};
        }
        
        /// get typed set of changes
        public PcBuildingTable GetChangesTyped()
        {
            return ((PcBuildingTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PcBuildingRow NewRowTyped(bool AWithDefaultValues)
        {
            PcBuildingRow ret = ((PcBuildingRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PcBuildingRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcBuildingRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnVenueKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnBuildingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnBuildingDesc))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
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
    
    /// Details of building used for accomodation at a conference
    [Serializable()]
    public class PcBuildingRow : System.Data.DataRow
    {
        
        private PcBuildingTable myTable;
        
        /// Constructor
        public PcBuildingRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PcBuildingTable)(this.Table));
        }
        
        /// 
        public Int64 VenueKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVenueKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVenueKey) 
                            || (((Int64)(this[this.myTable.ColumnVenueKey])) != value)))
                {
                    this[this.myTable.ColumnVenueKey] = value;
                }
            }
        }
        
        /// 
        public String BuildingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuildingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuildingCode) 
                            || (((String)(this[this.myTable.ColumnBuildingCode])) != value)))
                {
                    this[this.myTable.ColumnBuildingCode] = value;
                }
            }
        }
        
        /// This is a long description and is 80 characters long.
        public String BuildingDesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuildingDesc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuildingDesc) 
                            || (((String)(this[this.myTable.ColumnBuildingDesc])) != value)))
                {
                    this[this.myTable.ColumnBuildingDesc] = value;
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
            this.SetNull(this.myTable.ColumnVenueKey);
            this.SetNull(this.myTable.ColumnBuildingCode);
            this.SetNull(this.myTable.ColumnBuildingDesc);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsBuildingDescNull()
        {
            return this.IsNull(this.myTable.ColumnBuildingDesc);
        }
        
        /// assign NULL value
        public void SetBuildingDescNull()
        {
            this.SetNull(this.myTable.ColumnBuildingDesc);
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
    
    /// Details of rooms used for accommodation at a conference
    [Serializable()]
    public class PcRoomTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnVenueKey;
        
        /// 
        public DataColumn ColumnBuildingCode;
        
        /// 
        public DataColumn ColumnRoomNumber;
        
        /// 
        public DataColumn ColumnRoomName;
        
        /// 
        public DataColumn ColumnBeds;
        
        /// 
        public DataColumn ColumnMaxOccupancy;
        
        /// 
        public DataColumn ColumnBedCharge;
        
        /// 
        public DataColumn ColumnBedCost;
        
        /// 
        public DataColumn ColumnUsage;
        
        /// Gender that is preferred to use that room
        public DataColumn ColumnGenderPreference;
        
        /// X Position for the room layout designer in pixels
        public DataColumn ColumnLayoutXpos;
        
        /// Y Position for the room layout designer in pixels
        public DataColumn ColumnLayoutYpos;
        
        /// Width for the room layout designer in pixels
        public DataColumn ColumnLayoutWidth;
        
        /// Height for the room layout designer in pixels
        public DataColumn ColumnLayoutHeight;
        
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
        public PcRoomTable() : 
                base("PcRoom")
        {
        }
        
        /// constructor
        public PcRoomTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PcRoomTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PcRoomRow this[int i]
        {
            get
            {
                return ((PcRoomRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }
        
        /// get help text for column
        public static string GetVenueKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetVenueKeyLabel()
        {
            return "Venue Key";
        }
        
        /// get display format for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }
        
        /// get help text for column
        public static string GetBuildingCodeHelp()
        {
            return "Code to identify this building";
        }
        
        /// get label of column
        public static string GetBuildingCodeLabel()
        {
            return "Building Code";
        }
        
        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }
        
        /// get help text for column
        public static string GetRoomNumberHelp()
        {
            return "Number of the room";
        }
        
        /// get label of column
        public static string GetRoomNumberLabel()
        {
            return "Room Number";
        }
        
        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomNameDBName()
        {
            return "pc_room_name_c";
        }
        
        /// get help text for column
        public static string GetRoomNameHelp()
        {
            return "Name of the room";
        }
        
        /// get label of column
        public static string GetRoomNameLabel()
        {
            return "Room Name";
        }
        
        /// get character length for column
        public static short GetRoomNameLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBedsDBName()
        {
            return "pc_beds_i";
        }
        
        /// get help text for column
        public static string GetBedsHelp()
        {
            return "How many beds (spaces) in this room";
        }
        
        /// get label of column
        public static string GetBedsLabel()
        {
            return "Number of Beds";
        }
        
        /// get display format for column
        public static short GetBedsLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMaxOccupancyDBName()
        {
            return "pc_max_occupancy_i";
        }
        
        /// get help text for column
        public static string GetMaxOccupancyHelp()
        {
            return "Maximum number of people possible for this room";
        }
        
        /// get label of column
        public static string GetMaxOccupancyLabel()
        {
            return "Maximum Occupancy";
        }
        
        /// get display format for column
        public static short GetMaxOccupancyLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBedChargeDBName()
        {
            return "pc_bed_charge_n";
        }
        
        /// get help text for column
        public static string GetBedChargeHelp()
        {
            return "Charge to attendee per night";
        }
        
        /// get label of column
        public static string GetBedChargeLabel()
        {
            return "Charge per night";
        }
        
        /// get display format for column
        public static short GetBedChargeLength()
        {
            return 14;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBedCostDBName()
        {
            return "pc_bed_cost_n";
        }
        
        /// get help text for column
        public static string GetBedCostHelp()
        {
            return "Cost to conference per night per bed";
        }
        
        /// get label of column
        public static string GetBedCostLabel()
        {
            return "Cost per Night";
        }
        
        /// get display format for column
        public static short GetBedCostLength()
        {
            return 14;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUsageDBName()
        {
            return "pc_usage_c";
        }
        
        /// get help text for column
        public static string GetUsageHelp()
        {
            return "Who can use this room (eg Male, Female, Volunteers)";
        }
        
        /// get label of column
        public static string GetUsageLabel()
        {
            return "Room Usage";
        }
        
        /// get character length for column
        public static short GetUsageLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetGenderPreferenceDBName()
        {
            return "pc_gender_preference_c";
        }
        
        /// get help text for column
        public static string GetGenderPreferenceHelp()
        {
            return "Gender that is preferred to use that room";
        }
        
        /// get label of column
        public static string GetGenderPreferenceLabel()
        {
            return "pc_gender_preference_c";
        }
        
        /// get character length for column
        public static short GetGenderPreferenceLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLayoutXposDBName()
        {
            return "pc_layout_xpos_i";
        }
        
        /// get help text for column
        public static string GetLayoutXposHelp()
        {
            return "X Position for the room layout designer in pixels";
        }
        
        /// get label of column
        public static string GetLayoutXposLabel()
        {
            return "pc_layout_xpos_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLayoutYposDBName()
        {
            return "pc_layout_ypos_i";
        }
        
        /// get help text for column
        public static string GetLayoutYposHelp()
        {
            return "Y Position for the room layout designer in pixels";
        }
        
        /// get label of column
        public static string GetLayoutYposLabel()
        {
            return "pc_layout_ypos_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLayoutWidthDBName()
        {
            return "pc_layout_width_i";
        }
        
        /// get help text for column
        public static string GetLayoutWidthHelp()
        {
            return "Width for the room layout designer in pixels";
        }
        
        /// get label of column
        public static string GetLayoutWidthLabel()
        {
            return "pc_layout_width_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLayoutHeightDBName()
        {
            return "pc_layout_height_i";
        }
        
        /// get help text for column
        public static string GetLayoutHeightHelp()
        {
            return "Height for the room layout designer in pixels";
        }
        
        /// get label of column
        public static string GetLayoutHeightLabel()
        {
            return "pc_layout_height_i";
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
            return "PcRoom";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "pc_room";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Rooms";
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
                    "p_venue_key_n",
                    "pc_building_code_c",
                    "pc_room_number_c",
                    "pc_room_name_c",
                    "pc_beds_i",
                    "pc_max_occupancy_i",
                    "pc_bed_charge_n",
                    "pc_bed_cost_n",
                    "pc_usage_c",
                    "pc_gender_preference_c",
                    "pc_layout_xpos_i",
                    "pc_layout_ypos_i",
                    "pc_layout_width_i",
                    "pc_layout_height_i",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnVenueKey = this.Columns["p_venue_key_n"];
            this.ColumnBuildingCode = this.Columns["pc_building_code_c"];
            this.ColumnRoomNumber = this.Columns["pc_room_number_c"];
            this.ColumnRoomName = this.Columns["pc_room_name_c"];
            this.ColumnBeds = this.Columns["pc_beds_i"];
            this.ColumnMaxOccupancy = this.Columns["pc_max_occupancy_i"];
            this.ColumnBedCharge = this.Columns["pc_bed_charge_n"];
            this.ColumnBedCost = this.Columns["pc_bed_cost_n"];
            this.ColumnUsage = this.Columns["pc_usage_c"];
            this.ColumnGenderPreference = this.Columns["pc_gender_preference_c"];
            this.ColumnLayoutXpos = this.Columns["pc_layout_xpos_i"];
            this.ColumnLayoutYpos = this.Columns["pc_layout_ypos_i"];
            this.ColumnLayoutWidth = this.Columns["pc_layout_width_i"];
            this.ColumnLayoutHeight = this.Columns["pc_layout_height_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnVenueKey,
                    this.ColumnBuildingCode,
                    this.ColumnRoomNumber};
        }
        
        /// get typed set of changes
        public PcRoomTable GetChangesTyped()
        {
            return ((PcRoomTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PcRoomRow NewRowTyped(bool AWithDefaultValues)
        {
            PcRoomRow ret = ((PcRoomRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PcRoomRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcRoomRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_beds_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_max_occupancy_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_bed_charge_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("pc_bed_cost_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("pc_usage_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_gender_preference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_layout_xpos_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_layout_ypos_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_layout_width_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_layout_height_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnVenueKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnBuildingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnRoomNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnRoomName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnBeds))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnMaxOccupancy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnBedCharge))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnBedCost))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnUsage))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnGenderPreference))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 6);
            }
            if ((ACol == ColumnLayoutXpos))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLayoutYpos))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLayoutWidth))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLayoutHeight))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
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
    
    /// Details of rooms used for accommodation at a conference
    [Serializable()]
    public class PcRoomRow : System.Data.DataRow
    {
        
        private PcRoomTable myTable;
        
        /// Constructor
        public PcRoomRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PcRoomTable)(this.Table));
        }
        
        /// 
        public Int64 VenueKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVenueKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVenueKey) 
                            || (((Int64)(this[this.myTable.ColumnVenueKey])) != value)))
                {
                    this[this.myTable.ColumnVenueKey] = value;
                }
            }
        }
        
        /// 
        public String BuildingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuildingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuildingCode) 
                            || (((String)(this[this.myTable.ColumnBuildingCode])) != value)))
                {
                    this[this.myTable.ColumnBuildingCode] = value;
                }
            }
        }
        
        /// 
        public String RoomNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomNumber) 
                            || (((String)(this[this.myTable.ColumnRoomNumber])) != value)))
                {
                    this[this.myTable.ColumnRoomNumber] = value;
                }
            }
        }
        
        /// 
        public String RoomName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomName) 
                            || (((String)(this[this.myTable.ColumnRoomName])) != value)))
                {
                    this[this.myTable.ColumnRoomName] = value;
                }
            }
        }
        
        /// 
        public Int32 Beds
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBeds.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBeds) 
                            || (((Int32)(this[this.myTable.ColumnBeds])) != value)))
                {
                    this[this.myTable.ColumnBeds] = value;
                }
            }
        }
        
        /// 
        public Int32 MaxOccupancy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMaxOccupancy.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMaxOccupancy) 
                            || (((Int32)(this[this.myTable.ColumnMaxOccupancy])) != value)))
                {
                    this[this.myTable.ColumnMaxOccupancy] = value;
                }
            }
        }
        
        /// 
        public Double BedCharge
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBedCharge.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBedCharge) 
                            || (((Double)(this[this.myTable.ColumnBedCharge])) != value)))
                {
                    this[this.myTable.ColumnBedCharge] = value;
                }
            }
        }
        
        /// 
        public Double BedCost
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBedCost.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBedCost) 
                            || (((Double)(this[this.myTable.ColumnBedCost])) != value)))
                {
                    this[this.myTable.ColumnBedCost] = value;
                }
            }
        }
        
        /// 
        public String Usage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUsage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUsage) 
                            || (((String)(this[this.myTable.ColumnUsage])) != value)))
                {
                    this[this.myTable.ColumnUsage] = value;
                }
            }
        }
        
        /// Gender that is preferred to use that room
        public String GenderPreference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGenderPreference.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGenderPreference) 
                            || (((String)(this[this.myTable.ColumnGenderPreference])) != value)))
                {
                    this[this.myTable.ColumnGenderPreference] = value;
                }
            }
        }
        
        /// X Position for the room layout designer in pixels
        public Int32 LayoutXpos
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLayoutXpos.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLayoutXpos) 
                            || (((Int32)(this[this.myTable.ColumnLayoutXpos])) != value)))
                {
                    this[this.myTable.ColumnLayoutXpos] = value;
                }
            }
        }
        
        /// Y Position for the room layout designer in pixels
        public Int32 LayoutYpos
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLayoutYpos.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLayoutYpos) 
                            || (((Int32)(this[this.myTable.ColumnLayoutYpos])) != value)))
                {
                    this[this.myTable.ColumnLayoutYpos] = value;
                }
            }
        }
        
        /// Width for the room layout designer in pixels
        public Int32 LayoutWidth
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLayoutWidth.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLayoutWidth) 
                            || (((Int32)(this[this.myTable.ColumnLayoutWidth])) != value)))
                {
                    this[this.myTable.ColumnLayoutWidth] = value;
                }
            }
        }
        
        /// Height for the room layout designer in pixels
        public Int32 LayoutHeight
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLayoutHeight.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLayoutHeight) 
                            || (((Int32)(this[this.myTable.ColumnLayoutHeight])) != value)))
                {
                    this[this.myTable.ColumnLayoutHeight] = value;
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
            this.SetNull(this.myTable.ColumnVenueKey);
            this.SetNull(this.myTable.ColumnBuildingCode);
            this.SetNull(this.myTable.ColumnRoomNumber);
            this.SetNull(this.myTable.ColumnRoomName);
            this[this.myTable.ColumnBeds.Ordinal] = 0;
            this[this.myTable.ColumnMaxOccupancy.Ordinal] = 0;
            this[this.myTable.ColumnBedCharge.Ordinal] = 0;
            this[this.myTable.ColumnBedCost.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnUsage);
            this.SetNull(this.myTable.ColumnGenderPreference);
            this.SetNull(this.myTable.ColumnLayoutXpos);
            this.SetNull(this.myTable.ColumnLayoutYpos);
            this.SetNull(this.myTable.ColumnLayoutWidth);
            this.SetNull(this.myTable.ColumnLayoutHeight);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsRoomNameNull()
        {
            return this.IsNull(this.myTable.ColumnRoomName);
        }
        
        /// assign NULL value
        public void SetRoomNameNull()
        {
            this.SetNull(this.myTable.ColumnRoomName);
        }
        
        /// test for NULL value
        public bool IsBedsNull()
        {
            return this.IsNull(this.myTable.ColumnBeds);
        }
        
        /// assign NULL value
        public void SetBedsNull()
        {
            this.SetNull(this.myTable.ColumnBeds);
        }
        
        /// test for NULL value
        public bool IsMaxOccupancyNull()
        {
            return this.IsNull(this.myTable.ColumnMaxOccupancy);
        }
        
        /// assign NULL value
        public void SetMaxOccupancyNull()
        {
            this.SetNull(this.myTable.ColumnMaxOccupancy);
        }
        
        /// test for NULL value
        public bool IsBedChargeNull()
        {
            return this.IsNull(this.myTable.ColumnBedCharge);
        }
        
        /// assign NULL value
        public void SetBedChargeNull()
        {
            this.SetNull(this.myTable.ColumnBedCharge);
        }
        
        /// test for NULL value
        public bool IsBedCostNull()
        {
            return this.IsNull(this.myTable.ColumnBedCost);
        }
        
        /// assign NULL value
        public void SetBedCostNull()
        {
            this.SetNull(this.myTable.ColumnBedCost);
        }
        
        /// test for NULL value
        public bool IsUsageNull()
        {
            return this.IsNull(this.myTable.ColumnUsage);
        }
        
        /// assign NULL value
        public void SetUsageNull()
        {
            this.SetNull(this.myTable.ColumnUsage);
        }
        
        /// test for NULL value
        public bool IsGenderPreferenceNull()
        {
            return this.IsNull(this.myTable.ColumnGenderPreference);
        }
        
        /// assign NULL value
        public void SetGenderPreferenceNull()
        {
            this.SetNull(this.myTable.ColumnGenderPreference);
        }
        
        /// test for NULL value
        public bool IsLayoutXposNull()
        {
            return this.IsNull(this.myTable.ColumnLayoutXpos);
        }
        
        /// assign NULL value
        public void SetLayoutXposNull()
        {
            this.SetNull(this.myTable.ColumnLayoutXpos);
        }
        
        /// test for NULL value
        public bool IsLayoutYposNull()
        {
            return this.IsNull(this.myTable.ColumnLayoutYpos);
        }
        
        /// assign NULL value
        public void SetLayoutYposNull()
        {
            this.SetNull(this.myTable.ColumnLayoutYpos);
        }
        
        /// test for NULL value
        public bool IsLayoutWidthNull()
        {
            return this.IsNull(this.myTable.ColumnLayoutWidth);
        }
        
        /// assign NULL value
        public void SetLayoutWidthNull()
        {
            this.SetNull(this.myTable.ColumnLayoutWidth);
        }
        
        /// test for NULL value
        public bool IsLayoutHeightNull()
        {
            return this.IsNull(this.myTable.ColumnLayoutHeight);
        }
        
        /// assign NULL value
        public void SetLayoutHeightNull()
        {
            this.SetNull(this.myTable.ColumnLayoutHeight);
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
    
    /// Links rooms to attendees of a conference or a booking in the hospitality module
    [Serializable()]
    public class PcRoomAllocTable : TTypedDataTable
    {
        
        /// Surrogate Primary Key; required because there can be several bookings per room, and not all guests might be linked to a partner
        public DataColumn ColumnKey;
        
        /// 
        public DataColumn ColumnVenueKey;
        
        /// 
        public DataColumn ColumnBuildingCode;
        
        /// 
        public DataColumn ColumnRoomNumber;
        
        /// The room can be reserved for a conference
        public DataColumn ColumnConferenceKey;
        
        /// The partner key of the guest, can be null if group booking (see ph_booking for p_charged_key_n to find who is booking the room)
        public DataColumn ColumnPartnerKey;
        
        /// This makes the room unavailable for other guests even if not all beds are used
        public DataColumn ColumnBookWholeRoom;
        
        /// number of beds required by this allocation
        public DataColumn ColumnNumberOfBeds;
        
        /// number of additional beds (e.g. mattrass, childrens cot, etc) required by this allocation
        public DataColumn ColumnNumberOfOverflowBeds;
        
        /// possible values: couple, family, male, female
        public DataColumn ColumnGender;
        
        /// 
        public DataColumn ColumnIn;
        
        /// 
        public DataColumn ColumnOut;
        
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
        public PcRoomAllocTable() : 
                base("PcRoomAlloc")
        {
        }
        
        /// constructor
        public PcRoomAllocTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PcRoomAllocTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PcRoomAllocRow this[int i]
        {
            get
            {
                return ((PcRoomAllocRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetKeyDBName()
        {
            return "pc_key_i";
        }
        
        /// get help text for column
        public static string GetKeyHelp()
        {
            return "Surrogate Primary Key; required because there can be several bookings per room, a" +
                "nd not all guests might be linked to a partner";
        }
        
        /// get label of column
        public static string GetKeyLabel()
        {
            return "pc_key_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }
        
        /// get help text for column
        public static string GetVenueKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetVenueKeyLabel()
        {
            return "Venue Key";
        }
        
        /// get display format for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }
        
        /// get help text for column
        public static string GetBuildingCodeHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetBuildingCodeLabel()
        {
            return "Building Code";
        }
        
        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }
        
        /// get help text for column
        public static string GetRoomNumberHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetRoomNumberLabel()
        {
            return "Room Number";
        }
        
        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }
        
        /// get help text for column
        public static string GetConferenceKeyHelp()
        {
            return "The room can be reserved for a conference";
        }
        
        /// get label of column
        public static string GetConferenceKeyLabel()
        {
            return "pc_conference_key_n";
        }
        
        /// get display format for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "The partner key of the guest, can be null if group booking (see ph_booking for p_" +
                "charged_key_n to find who is booking the room)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "p_partner_key_n";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBookWholeRoomDBName()
        {
            return "ph_book_whole_room_l";
        }
        
        /// get help text for column
        public static string GetBookWholeRoomHelp()
        {
            return "This makes the room unavailable for other guests even if not all beds are used";
        }
        
        /// get label of column
        public static string GetBookWholeRoomLabel()
        {
            return "ph_book_whole_room_l";
        }
        
        /// get display format for column
        public static short GetBookWholeRoomLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfBedsDBName()
        {
            return "ph_number_of_beds_i";
        }
        
        /// get help text for column
        public static string GetNumberOfBedsHelp()
        {
            return "number of beds required by this allocation";
        }
        
        /// get label of column
        public static string GetNumberOfBedsLabel()
        {
            return "ph_number_of_beds_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfOverflowBedsDBName()
        {
            return "ph_number_of_overflow_beds_i";
        }
        
        /// get help text for column
        public static string GetNumberOfOverflowBedsHelp()
        {
            return "number of additional beds (e.g. mattrass, childrens cot, etc) required by this al" +
                "location";
        }
        
        /// get label of column
        public static string GetNumberOfOverflowBedsLabel()
        {
            return "ph_number_of_overflow_beds_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetGenderDBName()
        {
            return "ph_gender_c";
        }
        
        /// get help text for column
        public static string GetGenderHelp()
        {
            return "possible values: couple, family, male, female";
        }
        
        /// get label of column
        public static string GetGenderLabel()
        {
            return "ph_gender_c";
        }
        
        /// get character length for column
        public static short GetGenderLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInDBName()
        {
            return "pc_in_d";
        }
        
        /// get help text for column
        public static string GetInHelp()
        {
            return "Date the person first occupied this room";
        }
        
        /// get label of column
        public static string GetInLabel()
        {
            return "Date In";
        }
        
        /// get display format for column
        public static short GetInLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetOutDBName()
        {
            return "pc_out_d";
        }
        
        /// get help text for column
        public static string GetOutHelp()
        {
            return "Date the person stopped occupying this room";
        }
        
        /// get label of column
        public static string GetOutLabel()
        {
            return "Date Out";
        }
        
        /// get display format for column
        public static short GetOutLength()
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
            return "PcRoomAlloc";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "pc_room_alloc";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Room Allocation";
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
                    "pc_key_i",
                    "p_venue_key_n",
                    "pc_building_code_c",
                    "pc_room_number_c",
                    "pc_conference_key_n",
                    "p_partner_key_n",
                    "ph_book_whole_room_l",
                    "ph_number_of_beds_i",
                    "ph_number_of_overflow_beds_i",
                    "ph_gender_c",
                    "pc_in_d",
                    "pc_out_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnKey = this.Columns["pc_key_i"];
            this.ColumnVenueKey = this.Columns["p_venue_key_n"];
            this.ColumnBuildingCode = this.Columns["pc_building_code_c"];
            this.ColumnRoomNumber = this.Columns["pc_room_number_c"];
            this.ColumnConferenceKey = this.Columns["pc_conference_key_n"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnBookWholeRoom = this.Columns["ph_book_whole_room_l"];
            this.ColumnNumberOfBeds = this.Columns["ph_number_of_beds_i"];
            this.ColumnNumberOfOverflowBeds = this.Columns["ph_number_of_overflow_beds_i"];
            this.ColumnGender = this.Columns["ph_gender_c"];
            this.ColumnIn = this.Columns["pc_in_d"];
            this.ColumnOut = this.Columns["pc_out_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnKey};
        }
        
        /// get typed set of changes
        public PcRoomAllocTable GetChangesTyped()
        {
            return ((PcRoomAllocTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PcRoomAllocRow NewRowTyped(bool AWithDefaultValues)
        {
            PcRoomAllocRow ret = ((PcRoomAllocRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PcRoomAllocRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcRoomAllocRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_conference_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("ph_book_whole_room_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_beds_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_overflow_beds_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_gender_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_in_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pc_out_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnVenueKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnBuildingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnRoomNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnConferenceKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnBookWholeRoom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnNumberOfBeds))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfOverflowBeds))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnGender))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnIn))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnOut))
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
    
    /// Links rooms to attendees of a conference or a booking in the hospitality module
    [Serializable()]
    public class PcRoomAllocRow : System.Data.DataRow
    {
        
        private PcRoomAllocTable myTable;
        
        /// Constructor
        public PcRoomAllocRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PcRoomAllocTable)(this.Table));
        }
        
        /// Surrogate Primary Key; required because there can be several bookings per room, and not all guests might be linked to a partner
        public Int32 Key
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnKey) 
                            || (((Int32)(this[this.myTable.ColumnKey])) != value)))
                {
                    this[this.myTable.ColumnKey] = value;
                }
            }
        }
        
        /// 
        public Int64 VenueKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVenueKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVenueKey) 
                            || (((Int64)(this[this.myTable.ColumnVenueKey])) != value)))
                {
                    this[this.myTable.ColumnVenueKey] = value;
                }
            }
        }
        
        /// 
        public String BuildingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuildingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuildingCode) 
                            || (((String)(this[this.myTable.ColumnBuildingCode])) != value)))
                {
                    this[this.myTable.ColumnBuildingCode] = value;
                }
            }
        }
        
        /// 
        public String RoomNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomNumber) 
                            || (((String)(this[this.myTable.ColumnRoomNumber])) != value)))
                {
                    this[this.myTable.ColumnRoomNumber] = value;
                }
            }
        }
        
        /// The room can be reserved for a conference
        public Int64 ConferenceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConferenceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConferenceKey) 
                            || (((Int64)(this[this.myTable.ColumnConferenceKey])) != value)))
                {
                    this[this.myTable.ColumnConferenceKey] = value;
                }
            }
        }
        
        /// The partner key of the guest, can be null if group booking (see ph_booking for p_charged_key_n to find who is booking the room)
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
        
        /// This makes the room unavailable for other guests even if not all beds are used
        public Boolean BookWholeRoom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBookWholeRoom.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBookWholeRoom) 
                            || (((Boolean)(this[this.myTable.ColumnBookWholeRoom])) != value)))
                {
                    this[this.myTable.ColumnBookWholeRoom] = value;
                }
            }
        }
        
        /// number of beds required by this allocation
        public Int32 NumberOfBeds
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfBeds.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfBeds) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfBeds])) != value)))
                {
                    this[this.myTable.ColumnNumberOfBeds] = value;
                }
            }
        }
        
        /// number of additional beds (e.g. mattrass, childrens cot, etc) required by this allocation
        public Int32 NumberOfOverflowBeds
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfOverflowBeds.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfOverflowBeds) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfOverflowBeds])) != value)))
                {
                    this[this.myTable.ColumnNumberOfOverflowBeds] = value;
                }
            }
        }
        
        /// possible values: couple, family, male, female
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
        
        /// 
        public System.DateTime In
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnIn.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnIn) 
                            || (((System.DateTime)(this[this.myTable.ColumnIn])) != value)))
                {
                    this[this.myTable.ColumnIn] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime InLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnIn], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime InHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnIn.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime Out
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOut.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOut) 
                            || (((System.DateTime)(this[this.myTable.ColumnOut])) != value)))
                {
                    this[this.myTable.ColumnOut] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime OutLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnOut], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime OutHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnOut.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
            this.SetNull(this.myTable.ColumnKey);
            this.SetNull(this.myTable.ColumnVenueKey);
            this.SetNull(this.myTable.ColumnBuildingCode);
            this.SetNull(this.myTable.ColumnRoomNumber);
            this[this.myTable.ColumnConferenceKey.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnBookWholeRoom.Ordinal] = true;
            this[this.myTable.ColumnNumberOfBeds.Ordinal] = 1;
            this[this.myTable.ColumnNumberOfOverflowBeds.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnGender);
            this.SetNull(this.myTable.ColumnIn);
            this.SetNull(this.myTable.ColumnOut);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsConferenceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnConferenceKey);
        }
        
        /// assign NULL value
        public void SetConferenceKeyNull()
        {
            this.SetNull(this.myTable.ColumnConferenceKey);
        }
        
        /// test for NULL value
        public bool IsPartnerKeyNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerKey);
        }
        
        /// assign NULL value
        public void SetPartnerKeyNull()
        {
            this.SetNull(this.myTable.ColumnPartnerKey);
        }
        
        /// test for NULL value
        public bool IsNumberOfBedsNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfBeds);
        }
        
        /// assign NULL value
        public void SetNumberOfBedsNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfBeds);
        }
        
        /// test for NULL value
        public bool IsNumberOfOverflowBedsNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfOverflowBeds);
        }
        
        /// assign NULL value
        public void SetNumberOfOverflowBedsNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfOverflowBeds);
        }
        
        /// test for NULL value
        public bool IsOutNull()
        {
            return this.IsNull(this.myTable.ColumnOut);
        }
        
        /// assign NULL value
        public void SetOutNull()
        {
            this.SetNull(this.myTable.ColumnOut);
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
    
    /// Contains type of attributes that can be assigned to a room
    [Serializable()]
    public class PcRoomAttributeTypeTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnCode;
        
        /// 
        public DataColumn ColumnDesc;
        
        /// 
        public DataColumn ColumnValid;
        
        /// 
        public DataColumn ColumnDeletable;
        
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
        public PcRoomAttributeTypeTable() : 
                base("PcRoomAttributeType")
        {
        }
        
        /// constructor
        public PcRoomAttributeTypeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PcRoomAttributeTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PcRoomAttributeTypeRow this[int i]
        {
            get
            {
                return ((PcRoomAttributeTypeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "pc_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Room Attribute Type";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescDBName()
        {
            return "pc_desc_c";
        }
        
        /// get help text for column
        public static string GetDescHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetDescLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetValidDBName()
        {
            return "pc_valid_l";
        }
        
        /// get help text for column
        public static string GetValidHelp()
        {
            return "Is this a valid room attribute type ?";
        }
        
        /// get label of column
        public static string GetValidLabel()
        {
            return "Valid Type";
        }
        
        /// get display format for column
        public static short GetValidLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "pc_deletable_l";
        }
        
        /// get help text for column
        public static string GetDeletableHelp()
        {
            return "Can this room attribute type be deleted ?";
        }
        
        /// get label of column
        public static string GetDeletableLabel()
        {
            return "Deletable";
        }
        
        /// get display format for column
        public static short GetDeletableLength()
        {
            return 6;
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
            return "PcRoomAttributeType";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "pc_room_attribute_type";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Room Attribute Types";
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
                    "pc_code_c",
                    "pc_desc_c",
                    "pc_valid_l",
                    "pc_deletable_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["pc_code_c"];
            this.ColumnDesc = this.Columns["pc_desc_c"];
            this.ColumnValid = this.Columns["pc_valid_l"];
            this.ColumnDeletable = this.Columns["pc_deletable_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public PcRoomAttributeTypeTable GetChangesTyped()
        {
            return ((PcRoomAttributeTypeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PcRoomAttributeTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            PcRoomAttributeTypeRow ret = ((PcRoomAttributeTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PcRoomAttributeTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcRoomAttributeTypeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("pc_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_valid_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("pc_deletable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnDesc))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnValid))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDeletable))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
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
    
    /// Contains type of attributes that can be assigned to a room
    [Serializable()]
    public class PcRoomAttributeTypeRow : System.Data.DataRow
    {
        
        private PcRoomAttributeTypeTable myTable;
        
        /// Constructor
        public PcRoomAttributeTypeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PcRoomAttributeTypeTable)(this.Table));
        }
        
        /// 
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// 
        public String Desc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDesc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDesc) 
                            || (((String)(this[this.myTable.ColumnDesc])) != value)))
                {
                    this[this.myTable.ColumnDesc] = value;
                }
            }
        }
        
        /// 
        public Boolean Valid
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValid.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValid) 
                            || (((Boolean)(this[this.myTable.ColumnValid])) != value)))
                {
                    this[this.myTable.ColumnValid] = value;
                }
            }
        }
        
        /// 
        public Boolean Deletable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletable) 
                            || (((Boolean)(this[this.myTable.ColumnDeletable])) != value)))
                {
                    this[this.myTable.ColumnDeletable] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDesc);
            this[this.myTable.ColumnValid.Ordinal] = true;
            this[this.myTable.ColumnDeletable.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCode);
        }
        
        /// assign NULL value
        public void SetCodeNull()
        {
            this.SetNull(this.myTable.ColumnCode);
        }
        
        /// test for NULL value
        public bool IsDescNull()
        {
            return this.IsNull(this.myTable.ColumnDesc);
        }
        
        /// assign NULL value
        public void SetDescNull()
        {
            this.SetNull(this.myTable.ColumnDesc);
        }
        
        /// test for NULL value
        public bool IsValidNull()
        {
            return this.IsNull(this.myTable.ColumnValid);
        }
        
        /// assign NULL value
        public void SetValidNull()
        {
            this.SetNull(this.myTable.ColumnValid);
        }
        
        /// test for NULL value
        public bool IsDeletableNull()
        {
            return this.IsNull(this.myTable.ColumnDeletable);
        }
        
        /// assign NULL value
        public void SetDeletableNull()
        {
            this.SetNull(this.myTable.ColumnDeletable);
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
    
    /// Attributes assigned to rooms used for accommodation at a conference
    [Serializable()]
    public class PcRoomAttributeTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnVenueKey;
        
        /// 
        public DataColumn ColumnBuildingCode;
        
        /// 
        public DataColumn ColumnRoomNumber;
        
        /// 
        public DataColumn ColumnRoomAttrTypeCode;
        
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
        public PcRoomAttributeTable() : 
                base("PcRoomAttribute")
        {
        }
        
        /// constructor
        public PcRoomAttributeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PcRoomAttributeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PcRoomAttributeRow this[int i]
        {
            get
            {
                return ((PcRoomAttributeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }
        
        /// get help text for column
        public static string GetVenueKeyHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetVenueKeyLabel()
        {
            return "Venue Key";
        }
        
        /// get display format for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }
        
        /// get help text for column
        public static string GetBuildingCodeHelp()
        {
            return "Code to identify this building";
        }
        
        /// get label of column
        public static string GetBuildingCodeLabel()
        {
            return "Building Code";
        }
        
        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }
        
        /// get help text for column
        public static string GetRoomNumberHelp()
        {
            return "Number of the room";
        }
        
        /// get label of column
        public static string GetRoomNumberLabel()
        {
            return "Room Number";
        }
        
        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomAttrTypeCodeDBName()
        {
            return "pc_room_attr_type_code_c";
        }
        
        /// get help text for column
        public static string GetRoomAttrTypeCodeHelp()
        {
            return "Attribute assigned to this room";
        }
        
        /// get label of column
        public static string GetRoomAttrTypeCodeLabel()
        {
            return "Room Attribute";
        }
        
        /// get character length for column
        public static short GetRoomAttrTypeCodeLength()
        {
            return 20;
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
            return "PcRoomAttribute";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "pc_room_attribute";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Room Attributes";
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
                    "p_venue_key_n",
                    "pc_building_code_c",
                    "pc_room_number_c",
                    "pc_room_attr_type_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnVenueKey = this.Columns["p_venue_key_n"];
            this.ColumnBuildingCode = this.Columns["pc_building_code_c"];
            this.ColumnRoomNumber = this.Columns["pc_room_number_c"];
            this.ColumnRoomAttrTypeCode = this.Columns["pc_room_attr_type_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnVenueKey,
                    this.ColumnBuildingCode,
                    this.ColumnRoomNumber,
                    this.ColumnRoomAttrTypeCode};
        }
        
        /// get typed set of changes
        public PcRoomAttributeTable GetChangesTyped()
        {
            return ((PcRoomAttributeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PcRoomAttributeRow NewRowTyped(bool AWithDefaultValues)
        {
            PcRoomAttributeRow ret = ((PcRoomAttributeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PcRoomAttributeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PcRoomAttributeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_attr_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnVenueKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnBuildingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnRoomNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnRoomAttrTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
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
    
    /// Attributes assigned to rooms used for accommodation at a conference
    [Serializable()]
    public class PcRoomAttributeRow : System.Data.DataRow
    {
        
        private PcRoomAttributeTable myTable;
        
        /// Constructor
        public PcRoomAttributeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PcRoomAttributeTable)(this.Table));
        }
        
        /// 
        public Int64 VenueKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnVenueKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnVenueKey) 
                            || (((Int64)(this[this.myTable.ColumnVenueKey])) != value)))
                {
                    this[this.myTable.ColumnVenueKey] = value;
                }
            }
        }
        
        /// 
        public String BuildingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuildingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuildingCode) 
                            || (((String)(this[this.myTable.ColumnBuildingCode])) != value)))
                {
                    this[this.myTable.ColumnBuildingCode] = value;
                }
            }
        }
        
        /// 
        public String RoomNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomNumber) 
                            || (((String)(this[this.myTable.ColumnRoomNumber])) != value)))
                {
                    this[this.myTable.ColumnRoomNumber] = value;
                }
            }
        }
        
        /// 
        public String RoomAttrTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomAttrTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomAttrTypeCode) 
                            || (((String)(this[this.myTable.ColumnRoomAttrTypeCode])) != value)))
                {
                    this[this.myTable.ColumnRoomAttrTypeCode] = value;
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
            this.SetNull(this.myTable.ColumnVenueKey);
            this.SetNull(this.myTable.ColumnBuildingCode);
            this.SetNull(this.myTable.ColumnRoomNumber);
            this.SetNull(this.myTable.ColumnRoomAttrTypeCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsRoomAttrTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnRoomAttrTypeCode);
        }
        
        /// assign NULL value
        public void SetRoomAttrTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnRoomAttrTypeCode);
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
    
    /// Links room allocations and a booking
    [Serializable()]
    public class PhRoomBookingTable : TTypedDataTable
    {
        
        /// details of the booking
        public DataColumn ColumnBookingKey;
        
        /// which room/beds are booked
        public DataColumn ColumnRoomAllocKey;
        
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
        public PhRoomBookingTable() : 
                base("PhRoomBooking")
        {
        }
        
        /// constructor
        public PhRoomBookingTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PhRoomBookingTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PhRoomBookingRow this[int i]
        {
            get
            {
                return ((PhRoomBookingRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetBookingKeyDBName()
        {
            return "ph_booking_key_i";
        }
        
        /// get help text for column
        public static string GetBookingKeyHelp()
        {
            return "details of the booking";
        }
        
        /// get label of column
        public static string GetBookingKeyLabel()
        {
            return "ph_booking_key_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetRoomAllocKeyDBName()
        {
            return "ph_room_alloc_key_i";
        }
        
        /// get help text for column
        public static string GetRoomAllocKeyHelp()
        {
            return "which room/beds are booked";
        }
        
        /// get label of column
        public static string GetRoomAllocKeyLabel()
        {
            return "ph_room_alloc_key_i";
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
            return "PhRoomBooking";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "ph_room_booking";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "RoomBooking";
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
                    "ph_booking_key_i",
                    "ph_room_alloc_key_i",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnBookingKey = this.Columns["ph_booking_key_i"];
            this.ColumnRoomAllocKey = this.Columns["ph_room_alloc_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnBookingKey,
                    this.ColumnRoomAllocKey};
        }
        
        /// get typed set of changes
        public PhRoomBookingTable GetChangesTyped()
        {
            return ((PhRoomBookingTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PhRoomBookingRow NewRowTyped(bool AWithDefaultValues)
        {
            PhRoomBookingRow ret = ((PhRoomBookingRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PhRoomBookingRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PhRoomBookingRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("ph_booking_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_room_alloc_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnBookingKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnRoomAllocKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
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
    
    /// Links room allocations and a booking
    [Serializable()]
    public class PhRoomBookingRow : System.Data.DataRow
    {
        
        private PhRoomBookingTable myTable;
        
        /// Constructor
        public PhRoomBookingRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PhRoomBookingTable)(this.Table));
        }
        
        /// details of the booking
        public Int32 BookingKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBookingKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBookingKey) 
                            || (((Int32)(this[this.myTable.ColumnBookingKey])) != value)))
                {
                    this[this.myTable.ColumnBookingKey] = value;
                }
            }
        }
        
        /// which room/beds are booked
        public Int32 RoomAllocKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRoomAllocKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRoomAllocKey) 
                            || (((Int32)(this[this.myTable.ColumnRoomAllocKey])) != value)))
                {
                    this[this.myTable.ColumnRoomAllocKey] = value;
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
            this.SetNull(this.myTable.ColumnBookingKey);
            this.SetNull(this.myTable.ColumnRoomAllocKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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
    
    /// make sure charging works for a group or an individual; this summarises all the hospitality services that have to be paid for; also useful for planning meals in the kitchen and room preparation
    [Serializable()]
    public class PhBookingTable : TTypedDataTable
    {
        
        /// Surrogate Primary Key; required because there can be several bookings per room and per group
        public DataColumn ColumnKey;
        
        /// the partner key of the visitor or the partner key of the organisation or group that is visiting; each room allocation can refer to the individual guest as well; this can be different from the partner that is charged in the invoice
        public DataColumn ColumnContactKey;
        
        /// The ledger number that is needed for the invoice
        public DataColumn ColumnLedgerNumberForInvoice;
        
        /// refers to an offer which will later be the invoice
        public DataColumn ColumnArInvoiceKey;
        
        /// This is a booking for n adults
        public DataColumn ColumnNumberOfAdults;
        
        /// This is a booking for n children
        public DataColumn ColumnNumberOfChildren;
        
        /// The people that are part of this booking had n breakfasts; also useful for the kitchen
        public DataColumn ColumnNumberOfBreakfast;
        
        /// The people that are part of this booking had n lunches
        public DataColumn ColumnNumberOfLunch;
        
        /// The people that are part of this booking had n suppers
        public DataColumn ColumnNumberOfSupper;
        
        /// The number of linen that have been provided for this booking
        public DataColumn ColumnNumberOfLinenNeeded;
        
        /// this should be set to the date when the booking has been confirmed; required for early booking discounts
        public DataColumn ColumnConfirmed;
        
        /// 
        public DataColumn ColumnIn;
        
        /// 
        public DataColumn ColumnOut;
        
        /// 
        public DataColumn ColumnTimeArrival;
        
        /// 
        public DataColumn ColumnTimeDeparture;
        
        /// Add notes about the stay or special requests by the guest
        public DataColumn ColumnNotes;
        
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
        public PhBookingTable() : 
                base("PhBooking")
        {
        }
        
        /// constructor
        public PhBookingTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PhBookingTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PhBookingRow this[int i]
        {
            get
            {
                return ((PhBookingRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetKeyDBName()
        {
            return "ph_key_i";
        }
        
        /// get help text for column
        public static string GetKeyHelp()
        {
            return "Surrogate Primary Key; required because there can be several bookings per room an" +
                "d per group";
        }
        
        /// get label of column
        public static string GetKeyLabel()
        {
            return "ph_key_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactKeyDBName()
        {
            return "p_contact_key_n";
        }
        
        /// get help text for column
        public static string GetContactKeyHelp()
        {
            return "the partner key of the visitor or the partner key of the organisation or group th" +
                "at is visiting; each room allocation can refer to the individual guest as well; " +
                "this can be different from the partner that is charged in the invoice";
        }
        
        /// get label of column
        public static string GetContactKeyLabel()
        {
            return "p_contact_key_n";
        }
        
        /// get display format for column
        public static short GetContactKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberForInvoiceDBName()
        {
            return "a_ledger_number_for_invoice_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberForInvoiceHelp()
        {
            return "The ledger number that is needed for the invoice";
        }
        
        /// get label of column
        public static string GetLedgerNumberForInvoiceLabel()
        {
            return "Ledger Number for Invoice";
        }
        
        /// get display format for column
        public static short GetLedgerNumberForInvoiceLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArInvoiceKeyDBName()
        {
            return "a_ar_invoice_key_i";
        }
        
        /// get help text for column
        public static string GetArInvoiceKeyHelp()
        {
            return "refers to an offer which will later be the invoice";
        }
        
        /// get label of column
        public static string GetArInvoiceKeyLabel()
        {
            return "a_ar_invoice_key_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfAdultsDBName()
        {
            return "ph_number_of_adults_i";
        }
        
        /// get help text for column
        public static string GetNumberOfAdultsHelp()
        {
            return "This is a booking for n adults";
        }
        
        /// get label of column
        public static string GetNumberOfAdultsLabel()
        {
            return "ph_number_of_adults_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfChildrenDBName()
        {
            return "ph_number_of_children_i";
        }
        
        /// get help text for column
        public static string GetNumberOfChildrenHelp()
        {
            return "This is a booking for n children";
        }
        
        /// get label of column
        public static string GetNumberOfChildrenLabel()
        {
            return "ph_number_of_children_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfBreakfastDBName()
        {
            return "ph_number_of_breakfast_i";
        }
        
        /// get help text for column
        public static string GetNumberOfBreakfastHelp()
        {
            return "The people that are part of this booking had n breakfasts; also useful for the ki" +
                "tchen";
        }
        
        /// get label of column
        public static string GetNumberOfBreakfastLabel()
        {
            return "ph_number_of_breakfast_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfLunchDBName()
        {
            return "ph_number_of_lunch_i";
        }
        
        /// get help text for column
        public static string GetNumberOfLunchHelp()
        {
            return "The people that are part of this booking had n lunches";
        }
        
        /// get label of column
        public static string GetNumberOfLunchLabel()
        {
            return "ph_number_of_lunch_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfSupperDBName()
        {
            return "ph_number_of_supper_i";
        }
        
        /// get help text for column
        public static string GetNumberOfSupperHelp()
        {
            return "The people that are part of this booking had n suppers";
        }
        
        /// get label of column
        public static string GetNumberOfSupperLabel()
        {
            return "ph_number_of_supper_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfLinenNeededDBName()
        {
            return "ph_number_of_linen_needed_i";
        }
        
        /// get help text for column
        public static string GetNumberOfLinenNeededHelp()
        {
            return "The number of linen that have been provided for this booking";
        }
        
        /// get label of column
        public static string GetNumberOfLinenNeededLabel()
        {
            return "ph_number_of_linen_needed_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetConfirmedDBName()
        {
            return "ph_confirmed_d";
        }
        
        /// get help text for column
        public static string GetConfirmedHelp()
        {
            return "this should be set to the date when the booking has been confirmed; required for " +
                "early booking discounts";
        }
        
        /// get label of column
        public static string GetConfirmedLabel()
        {
            return "ph_confirmed_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetInDBName()
        {
            return "ph_in_d";
        }
        
        /// get help text for column
        public static string GetInHelp()
        {
            return "Date the guest arrives";
        }
        
        /// get label of column
        public static string GetInLabel()
        {
            return "Date In";
        }
        
        /// get display format for column
        public static short GetInLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetOutDBName()
        {
            return "ph_out_d";
        }
        
        /// get help text for column
        public static string GetOutHelp()
        {
            return "Date the guest leaves";
        }
        
        /// get label of column
        public static string GetOutLabel()
        {
            return "Date Out";
        }
        
        /// get display format for column
        public static short GetOutLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTimeArrivalDBName()
        {
            return "ph_time_arrival_i";
        }
        
        /// get help text for column
        public static string GetTimeArrivalHelp()
        {
            return "Time when the guest is arriving";
        }
        
        /// get label of column
        public static string GetTimeArrivalLabel()
        {
            return "Time of Arrival";
        }
        
        /// get display format for column
        public static short GetTimeArrivalLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTimeDepartureDBName()
        {
            return "ph_time_departure_i";
        }
        
        /// get help text for column
        public static string GetTimeDepartureHelp()
        {
            return "Time when the guest is departing";
        }
        
        /// get label of column
        public static string GetTimeDepartureLabel()
        {
            return "Time of Departure";
        }
        
        /// get display format for column
        public static short GetTimeDepartureLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNotesDBName()
        {
            return "ph_notes_c";
        }
        
        /// get help text for column
        public static string GetNotesHelp()
        {
            return "Time when the guest is departing";
        }
        
        /// get label of column
        public static string GetNotesLabel()
        {
            return "ph_notes_c";
        }
        
        /// get character length for column
        public static short GetNotesLength()
        {
            return 500;
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
            return "PhBooking";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "ph_booking";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Booking";
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
                    "ph_key_i",
                    "p_contact_key_n",
                    "a_ledger_number_for_invoice_i",
                    "a_ar_invoice_key_i",
                    "ph_number_of_adults_i",
                    "ph_number_of_children_i",
                    "ph_number_of_breakfast_i",
                    "ph_number_of_lunch_i",
                    "ph_number_of_supper_i",
                    "ph_number_of_linen_needed_i",
                    "ph_confirmed_d",
                    "ph_in_d",
                    "ph_out_d",
                    "ph_time_arrival_i",
                    "ph_time_departure_i",
                    "ph_notes_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnKey = this.Columns["ph_key_i"];
            this.ColumnContactKey = this.Columns["p_contact_key_n"];
            this.ColumnLedgerNumberForInvoice = this.Columns["a_ledger_number_for_invoice_i"];
            this.ColumnArInvoiceKey = this.Columns["a_ar_invoice_key_i"];
            this.ColumnNumberOfAdults = this.Columns["ph_number_of_adults_i"];
            this.ColumnNumberOfChildren = this.Columns["ph_number_of_children_i"];
            this.ColumnNumberOfBreakfast = this.Columns["ph_number_of_breakfast_i"];
            this.ColumnNumberOfLunch = this.Columns["ph_number_of_lunch_i"];
            this.ColumnNumberOfSupper = this.Columns["ph_number_of_supper_i"];
            this.ColumnNumberOfLinenNeeded = this.Columns["ph_number_of_linen_needed_i"];
            this.ColumnConfirmed = this.Columns["ph_confirmed_d"];
            this.ColumnIn = this.Columns["ph_in_d"];
            this.ColumnOut = this.Columns["ph_out_d"];
            this.ColumnTimeArrival = this.Columns["ph_time_arrival_i"];
            this.ColumnTimeDeparture = this.Columns["ph_time_departure_i"];
            this.ColumnNotes = this.Columns["ph_notes_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnKey};
        }
        
        /// get typed set of changes
        public PhBookingTable GetChangesTyped()
        {
            return ((PhBookingTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PhBookingRow NewRowTyped(bool AWithDefaultValues)
        {
            PhBookingRow ret = ((PhBookingRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PhBookingRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PhBookingRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("ph_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_for_invoice_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_invoice_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_adults_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_children_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_breakfast_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_lunch_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_supper_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_number_of_linen_needed_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_confirmed_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("ph_in_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("ph_out_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("ph_time_arrival_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_time_departure_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ph_notes_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnContactKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnLedgerNumberForInvoice))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArInvoiceKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfAdults))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfChildren))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfBreakfast))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfLunch))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfSupper))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfLinenNeeded))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnConfirmed))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnIn))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnOut))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnTimeArrival))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTimeDeparture))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNotes))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 1000);
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
    
    /// make sure charging works for a group or an individual; this summarises all the hospitality services that have to be paid for; also useful for planning meals in the kitchen and room preparation
    [Serializable()]
    public class PhBookingRow : System.Data.DataRow
    {
        
        private PhBookingTable myTable;
        
        /// Constructor
        public PhBookingRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PhBookingTable)(this.Table));
        }
        
        /// Surrogate Primary Key; required because there can be several bookings per room and per group
        public Int32 Key
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnKey) 
                            || (((Int32)(this[this.myTable.ColumnKey])) != value)))
                {
                    this[this.myTable.ColumnKey] = value;
                }
            }
        }
        
        /// the partner key of the visitor or the partner key of the organisation or group that is visiting; each room allocation can refer to the individual guest as well; this can be different from the partner that is charged in the invoice
        public Int64 ContactKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactKey) 
                            || (((Int64)(this[this.myTable.ColumnContactKey])) != value)))
                {
                    this[this.myTable.ColumnContactKey] = value;
                }
            }
        }
        
        /// The ledger number that is needed for the invoice
        public Int32 LedgerNumberForInvoice
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumberForInvoice.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumberForInvoice) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumberForInvoice])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumberForInvoice] = value;
                }
            }
        }
        
        /// refers to an offer which will later be the invoice
        public Int32 ArInvoiceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArInvoiceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArInvoiceKey) 
                            || (((Int32)(this[this.myTable.ColumnArInvoiceKey])) != value)))
                {
                    this[this.myTable.ColumnArInvoiceKey] = value;
                }
            }
        }
        
        /// This is a booking for n adults
        public Int32 NumberOfAdults
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfAdults.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfAdults) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfAdults])) != value)))
                {
                    this[this.myTable.ColumnNumberOfAdults] = value;
                }
            }
        }
        
        /// This is a booking for n children
        public Int32 NumberOfChildren
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfChildren.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfChildren) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfChildren])) != value)))
                {
                    this[this.myTable.ColumnNumberOfChildren] = value;
                }
            }
        }
        
        /// The people that are part of this booking had n breakfasts; also useful for the kitchen
        public Int32 NumberOfBreakfast
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfBreakfast.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfBreakfast) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfBreakfast])) != value)))
                {
                    this[this.myTable.ColumnNumberOfBreakfast] = value;
                }
            }
        }
        
        /// The people that are part of this booking had n lunches
        public Int32 NumberOfLunch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfLunch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfLunch) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfLunch])) != value)))
                {
                    this[this.myTable.ColumnNumberOfLunch] = value;
                }
            }
        }
        
        /// The people that are part of this booking had n suppers
        public Int32 NumberOfSupper
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfSupper.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfSupper) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfSupper])) != value)))
                {
                    this[this.myTable.ColumnNumberOfSupper] = value;
                }
            }
        }
        
        /// The number of linen that have been provided for this booking
        public Int32 NumberOfLinenNeeded
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfLinenNeeded.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfLinenNeeded) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfLinenNeeded])) != value)))
                {
                    this[this.myTable.ColumnNumberOfLinenNeeded] = value;
                }
            }
        }
        
        /// this should be set to the date when the booking has been confirmed; required for early booking discounts
        public System.DateTime Confirmed
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConfirmed.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnConfirmed) 
                            || (((System.DateTime)(this[this.myTable.ColumnConfirmed])) != value)))
                {
                    this[this.myTable.ColumnConfirmed] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ConfirmedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnConfirmed], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ConfirmedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnConfirmed.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime In
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnIn.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnIn) 
                            || (((System.DateTime)(this[this.myTable.ColumnIn])) != value)))
                {
                    this[this.myTable.ColumnIn] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime InLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnIn], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime InHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnIn.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime Out
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOut.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOut) 
                            || (((System.DateTime)(this[this.myTable.ColumnOut])) != value)))
                {
                    this[this.myTable.ColumnOut] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime OutLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnOut], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime OutHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnOut.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public Int32 TimeArrival
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTimeArrival.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTimeArrival) 
                            || (((Int32)(this[this.myTable.ColumnTimeArrival])) != value)))
                {
                    this[this.myTable.ColumnTimeArrival] = value;
                }
            }
        }
        
        /// 
        public Int32 TimeDeparture
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTimeDeparture.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTimeDeparture) 
                            || (((Int32)(this[this.myTable.ColumnTimeDeparture])) != value)))
                {
                    this[this.myTable.ColumnTimeDeparture] = value;
                }
            }
        }
        
        /// Add notes about the stay or special requests by the guest
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
            this.SetNull(this.myTable.ColumnKey);
            this.SetNull(this.myTable.ColumnContactKey);
            this[this.myTable.ColumnLedgerNumberForInvoice.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnArInvoiceKey);
            this[this.myTable.ColumnNumberOfAdults.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfChildren.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfBreakfast.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfLunch.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfSupper.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfLinenNeeded.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnConfirmed);
            this.SetNull(this.myTable.ColumnIn);
            this.SetNull(this.myTable.ColumnOut);
            this.SetNull(this.myTable.ColumnTimeArrival);
            this.SetNull(this.myTable.ColumnTimeDeparture);
            this.SetNull(this.myTable.ColumnNotes);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsContactKeyNull()
        {
            return this.IsNull(this.myTable.ColumnContactKey);
        }
        
        /// assign NULL value
        public void SetContactKeyNull()
        {
            this.SetNull(this.myTable.ColumnContactKey);
        }
        
        /// test for NULL value
        public bool IsLedgerNumberForInvoiceNull()
        {
            return this.IsNull(this.myTable.ColumnLedgerNumberForInvoice);
        }
        
        /// assign NULL value
        public void SetLedgerNumberForInvoiceNull()
        {
            this.SetNull(this.myTable.ColumnLedgerNumberForInvoice);
        }
        
        /// test for NULL value
        public bool IsArInvoiceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnArInvoiceKey);
        }
        
        /// assign NULL value
        public void SetArInvoiceKeyNull()
        {
            this.SetNull(this.myTable.ColumnArInvoiceKey);
        }
        
        /// test for NULL value
        public bool IsNumberOfAdultsNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfAdults);
        }
        
        /// assign NULL value
        public void SetNumberOfAdultsNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfAdults);
        }
        
        /// test for NULL value
        public bool IsNumberOfChildrenNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfChildren);
        }
        
        /// assign NULL value
        public void SetNumberOfChildrenNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfChildren);
        }
        
        /// test for NULL value
        public bool IsNumberOfBreakfastNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfBreakfast);
        }
        
        /// assign NULL value
        public void SetNumberOfBreakfastNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfBreakfast);
        }
        
        /// test for NULL value
        public bool IsNumberOfLunchNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfLunch);
        }
        
        /// assign NULL value
        public void SetNumberOfLunchNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfLunch);
        }
        
        /// test for NULL value
        public bool IsNumberOfSupperNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfSupper);
        }
        
        /// assign NULL value
        public void SetNumberOfSupperNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfSupper);
        }
        
        /// test for NULL value
        public bool IsNumberOfLinenNeededNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfLinenNeeded);
        }
        
        /// assign NULL value
        public void SetNumberOfLinenNeededNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfLinenNeeded);
        }
        
        /// test for NULL value
        public bool IsConfirmedNull()
        {
            return this.IsNull(this.myTable.ColumnConfirmed);
        }
        
        /// assign NULL value
        public void SetConfirmedNull()
        {
            this.SetNull(this.myTable.ColumnConfirmed);
        }
        
        /// test for NULL value
        public bool IsOutNull()
        {
            return this.IsNull(this.myTable.ColumnOut);
        }
        
        /// assign NULL value
        public void SetOutNull()
        {
            this.SetNull(this.myTable.ColumnOut);
        }
        
        /// test for NULL value
        public bool IsTimeArrivalNull()
        {
            return this.IsNull(this.myTable.ColumnTimeArrival);
        }
        
        /// assign NULL value
        public void SetTimeArrivalNull()
        {
            this.SetNull(this.myTable.ColumnTimeArrival);
        }
        
        /// test for NULL value
        public bool IsTimeDepartureNull()
        {
            return this.IsNull(this.myTable.ColumnTimeDeparture);
        }
        
        /// assign NULL value
        public void SetTimeDepartureNull()
        {
            this.SetNull(this.myTable.ColumnTimeDeparture);
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
