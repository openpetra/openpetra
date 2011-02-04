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

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.Xml;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Petra.Shared.MHospitality.Data
{

    /// Details of building used for accomodation at a conference
    [Serializable()]
    public class PcBuildingTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 277;
        /// used for generic TTypedDataTable functions
        public static short ColumnVenueKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuildingCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuildingDescId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcBuilding", "pc_building",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "VenueKey", "p_venue_key_n", "Venue Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "BuildingCode", "pc_building_code_c", "Building Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "BuildingDesc", "pc_building_desc_c", "Description", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnVenueKey,ColumnBuildingCode};
        }

        /// Access a typed row by index
        public PcBuildingRow this[int i]
        {
            get
            {
                return ((PcBuildingRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PcBuildingTable GetChangesTyped()
        {
            return ((PcBuildingTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PcBuilding";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "pc_building";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }

        /// get character length for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }

        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetBuildingDescDBName()
        {
            return "pc_building_desc_c";
        }

        /// get character length for column
        public static short GetBuildingDescLength()
        {
            return 160;
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        public bool IsVenueKeyNull()
        {
            return this.IsNull(this.myTable.ColumnVenueKey);
        }

        /// assign NULL value
        public void SetVenueKeyNull()
        {
            this.SetNull(this.myTable.ColumnVenueKey);
        }

        /// test for NULL value
        public bool IsBuildingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBuildingCode);
        }

        /// assign NULL value
        public void SetBuildingCodeNull()
        {
            this.SetNull(this.myTable.ColumnBuildingCode);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 278;
        /// used for generic TTypedDataTable functions
        public static short ColumnVenueKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuildingCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomNameId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnBedsId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnMaxOccupancyId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnBedChargeId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnBedCostId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnUsageId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenderPreferenceId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnLayoutXposId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnLayoutYposId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnLayoutWidthId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnLayoutHeightId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 18;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcRoom", "pc_room",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "VenueKey", "p_venue_key_n", "Venue Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "BuildingCode", "pc_building_code_c", "Building Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "RoomNumber", "pc_room_number_c", "Room Number", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "RoomName", "pc_room_name_c", "Room Name", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(4, "Beds", "pc_beds_i", "Number of Beds", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "MaxOccupancy", "pc_max_occupancy_i", "Maximum Occupancy", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "BedCharge", "pc_bed_charge_n", "Charge per night", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(7, "BedCost", "pc_bed_cost_n", "Cost per Night", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(8, "Usage", "pc_usage_c", "Room Usage", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(9, "GenderPreference", "pc_gender_preference_c", "pc_gender_preference_c", OdbcType.VarChar, 6, false),
                    new TTypedColumnInfo(10, "LayoutXpos", "pc_layout_xpos_i", "pc_layout_xpos_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "LayoutYpos", "pc_layout_ypos_i", "pc_layout_ypos_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(12, "LayoutWidth", "pc_layout_width_i", "pc_layout_width_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "LayoutHeight", "pc_layout_height_i", "pc_layout_height_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(14, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_venue_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("pc_building_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_room_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pc_beds_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_max_occupancy_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("pc_bed_charge_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("pc_bed_cost_n", typeof(Decimal)));
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
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnVenueKey,ColumnBuildingCode,ColumnRoomNumber};
        }

        /// Access a typed row by index
        public PcRoomRow this[int i]
        {
            get
            {
                return ((PcRoomRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PcRoomTable GetChangesTyped()
        {
            return ((PcRoomTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PcRoom";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "pc_room";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }

        /// get character length for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }

        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }

        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomNameDBName()
        {
            return "pc_room_name_c";
        }

        /// get character length for column
        public static short GetRoomNameLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetBedsDBName()
        {
            return "pc_beds_i";
        }

        /// get character length for column
        public static short GetBedsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMaxOccupancyDBName()
        {
            return "pc_max_occupancy_i";
        }

        /// get character length for column
        public static short GetMaxOccupancyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBedChargeDBName()
        {
            return "pc_bed_charge_n";
        }

        /// get character length for column
        public static short GetBedChargeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetBedCostDBName()
        {
            return "pc_bed_cost_n";
        }

        /// get character length for column
        public static short GetBedCostLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetUsageDBName()
        {
            return "pc_usage_c";
        }

        /// get character length for column
        public static short GetUsageLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetGenderPreferenceDBName()
        {
            return "pc_gender_preference_c";
        }

        /// get character length for column
        public static short GetGenderPreferenceLength()
        {
            return 6;
        }

        /// get the name of the field in the database for this column
        public static string GetLayoutXposDBName()
        {
            return "pc_layout_xpos_i";
        }

        /// get character length for column
        public static short GetLayoutXposLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLayoutYposDBName()
        {
            return "pc_layout_ypos_i";
        }

        /// get character length for column
        public static short GetLayoutYposLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLayoutWidthDBName()
        {
            return "pc_layout_width_i";
        }

        /// get character length for column
        public static short GetLayoutWidthLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLayoutHeightDBName()
        {
            return "pc_layout_height_i";
        }

        /// get character length for column
        public static short GetLayoutHeightLength()
        {
            return -1;
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
        public Decimal BedCharge
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
                    return ((Decimal)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBedCharge)
                            || (((Decimal)(this[this.myTable.ColumnBedCharge])) != value)))
                {
                    this[this.myTable.ColumnBedCharge] = value;
                }
            }
        }

        ///
        public Decimal BedCost
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
                    return ((Decimal)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBedCost)
                            || (((Decimal)(this[this.myTable.ColumnBedCost])) != value)))
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        public bool IsVenueKeyNull()
        {
            return this.IsNull(this.myTable.ColumnVenueKey);
        }

        /// assign NULL value
        public void SetVenueKeyNull()
        {
            this.SetNull(this.myTable.ColumnVenueKey);
        }

        /// test for NULL value
        public bool IsBuildingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBuildingCode);
        }

        /// assign NULL value
        public void SetBuildingCodeNull()
        {
            this.SetNull(this.myTable.ColumnBuildingCode);
        }

        /// test for NULL value
        public bool IsRoomNumberNull()
        {
            return this.IsNull(this.myTable.ColumnRoomNumber);
        }

        /// assign NULL value
        public void SetRoomNumberNull()
        {
            this.SetNull(this.myTable.ColumnRoomNumber);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 279;
        /// used for generic TTypedDataTable functions
        public static short ColumnKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnVenueKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuildingCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomNumberId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnConferenceKeyId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnBookWholeRoomId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfBedsId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfOverflowBedsId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenderId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnInId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnOutId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 16;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcRoomAlloc", "pc_room_alloc",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Key", "pc_key_i", "pc_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "VenueKey", "p_venue_key_n", "Venue Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "BuildingCode", "pc_building_code_c", "Building Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "RoomNumber", "pc_room_number_c", "Room Number", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(4, "ConferenceKey", "pc_conference_key_n", "pc_conference_key_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(5, "PartnerKey", "p_partner_key_n", "p_partner_key_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(6, "BookWholeRoom", "ph_book_whole_room_l", "ph_book_whole_room_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(7, "NumberOfBeds", "ph_number_of_beds_i", "ph_number_of_beds_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "NumberOfOverflowBeds", "ph_number_of_overflow_beds_i", "ph_number_of_overflow_beds_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "Gender", "ph_gender_c", "ph_gender_c", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(10, "In", "pc_in_d", "Date In", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(11, "Out", "pc_out_d", "Date Out", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(12, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnKey};
        }

        /// Access a typed row by index
        public PcRoomAllocRow this[int i]
        {
            get
            {
                return ((PcRoomAllocRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PcRoomAllocTable GetChangesTyped()
        {
            return ((PcRoomAllocTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PcRoomAlloc";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "pc_room_alloc";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetKeyDBName()
        {
            return "pc_key_i";
        }

        /// get character length for column
        public static short GetKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }

        /// get character length for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }

        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }

        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetConferenceKeyDBName()
        {
            return "pc_conference_key_n";
        }

        /// get character length for column
        public static short GetConferenceKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetBookWholeRoomDBName()
        {
            return "ph_book_whole_room_l";
        }

        /// get character length for column
        public static short GetBookWholeRoomLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfBedsDBName()
        {
            return "ph_number_of_beds_i";
        }

        /// get character length for column
        public static short GetNumberOfBedsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfOverflowBedsDBName()
        {
            return "ph_number_of_overflow_beds_i";
        }

        /// get character length for column
        public static short GetNumberOfOverflowBedsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGenderDBName()
        {
            return "ph_gender_c";
        }

        /// get character length for column
        public static short GetGenderLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetInDBName()
        {
            return "pc_in_d";
        }

        /// get character length for column
        public static short GetInLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetOutDBName()
        {
            return "pc_out_d";
        }

        /// get character length for column
        public static short GetOutLength()
        {
            return -1;
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
                    return DateTime.MinValue;
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

        ///
        public System.DateTime? Out
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOut.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnOut)
                            || (((System.DateTime?)(this[this.myTable.ColumnOut])) != value)))
                {
                    this[this.myTable.ColumnOut] = value;
                }
            }
        }

        /// The date the record was created.
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
            this.SetNull(this.myTable.ColumnConferenceKey);
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
        public bool IsKeyNull()
        {
            return this.IsNull(this.myTable.ColumnKey);
        }

        /// assign NULL value
        public void SetKeyNull()
        {
            this.SetNull(this.myTable.ColumnKey);
        }

        /// test for NULL value
        public bool IsVenueKeyNull()
        {
            return this.IsNull(this.myTable.ColumnVenueKey);
        }

        /// assign NULL value
        public void SetVenueKeyNull()
        {
            this.SetNull(this.myTable.ColumnVenueKey);
        }

        /// test for NULL value
        public bool IsBuildingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBuildingCode);
        }

        /// assign NULL value
        public void SetBuildingCodeNull()
        {
            this.SetNull(this.myTable.ColumnBuildingCode);
        }

        /// test for NULL value
        public bool IsRoomNumberNull()
        {
            return this.IsNull(this.myTable.ColumnRoomNumber);
        }

        /// assign NULL value
        public void SetRoomNumberNull()
        {
            this.SetNull(this.myTable.ColumnRoomNumber);
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
        public bool IsBookWholeRoomNull()
        {
            return this.IsNull(this.myTable.ColumnBookWholeRoom);
        }

        /// assign NULL value
        public void SetBookWholeRoomNull()
        {
            this.SetNull(this.myTable.ColumnBookWholeRoom);
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
        public bool IsInNull()
        {
            return this.IsNull(this.myTable.ColumnIn);
        }

        /// assign NULL value
        public void SetInNull()
        {
            this.SetNull(this.myTable.ColumnIn);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 280;
        /// used for generic TTypedDataTable functions
        public static short ColumnCodeId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDescId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnValidId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDeletableId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcRoomAttributeType", "pc_room_attribute_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Code", "pc_code_c", "Room Attribute Type", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(1, "Desc", "pc_desc_c", "Description", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(2, "Valid", "pc_valid_l", "Valid Type", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "Deletable", "pc_deletable_l", "Deletable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnCode};
        }

        /// Access a typed row by index
        public PcRoomAttributeTypeRow this[int i]
        {
            get
            {
                return ((PcRoomAttributeTypeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PcRoomAttributeTypeTable GetChangesTyped()
        {
            return ((PcRoomAttributeTypeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PcRoomAttributeType";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "pc_room_attribute_type";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "pc_code_c";
        }

        /// get character length for column
        public static short GetCodeLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetDescDBName()
        {
            return "pc_desc_c";
        }

        /// get character length for column
        public static short GetDescLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetValidDBName()
        {
            return "pc_valid_l";
        }

        /// get character length for column
        public static short GetValidLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "pc_deletable_l";
        }

        /// get character length for column
        public static short GetDeletableLength()
        {
            return -1;
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 281;
        /// used for generic TTypedDataTable functions
        public static short ColumnVenueKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuildingCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomNumberId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomAttrTypeCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 8;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PcRoomAttribute", "pc_room_attribute",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "VenueKey", "p_venue_key_n", "Venue Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "BuildingCode", "pc_building_code_c", "Building Code", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(2, "RoomNumber", "pc_room_number_c", "Room Number", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(3, "RoomAttrTypeCode", "pc_room_attr_type_code_c", "Room Attribute", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnVenueKey,ColumnBuildingCode,ColumnRoomNumber,ColumnRoomAttrTypeCode};
        }

        /// Access a typed row by index
        public PcRoomAttributeRow this[int i]
        {
            get
            {
                return ((PcRoomAttributeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PcRoomAttributeTable GetChangesTyped()
        {
            return ((PcRoomAttributeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PcRoomAttribute";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "pc_room_attribute";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetVenueKeyDBName()
        {
            return "p_venue_key_n";
        }

        /// get character length for column
        public static short GetVenueKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetBuildingCodeDBName()
        {
            return "pc_building_code_c";
        }

        /// get character length for column
        public static short GetBuildingCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomNumberDBName()
        {
            return "pc_room_number_c";
        }

        /// get character length for column
        public static short GetRoomNumberLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomAttrTypeCodeDBName()
        {
            return "pc_room_attr_type_code_c";
        }

        /// get character length for column
        public static short GetRoomAttrTypeCodeLength()
        {
            return 40;
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        public bool IsVenueKeyNull()
        {
            return this.IsNull(this.myTable.ColumnVenueKey);
        }

        /// assign NULL value
        public void SetVenueKeyNull()
        {
            this.SetNull(this.myTable.ColumnVenueKey);
        }

        /// test for NULL value
        public bool IsBuildingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnBuildingCode);
        }

        /// assign NULL value
        public void SetBuildingCodeNull()
        {
            this.SetNull(this.myTable.ColumnBuildingCode);
        }

        /// test for NULL value
        public bool IsRoomNumberNull()
        {
            return this.IsNull(this.myTable.ColumnRoomNumber);
        }

        /// assign NULL value
        public void SetRoomNumberNull()
        {
            this.SetNull(this.myTable.ColumnRoomNumber);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 283;
        /// used for generic TTypedDataTable functions
        public static short ColumnBookingKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnRoomAllocKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PhRoomBooking", "ph_room_booking",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "BookingKey", "ph_booking_key_i", "ph_booking_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "RoomAllocKey", "ph_room_alloc_key_i", "ph_room_alloc_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(3, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0, 1
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnBookingKey,ColumnRoomAllocKey};
        }

        /// Access a typed row by index
        public PhRoomBookingRow this[int i]
        {
            get
            {
                return ((PhRoomBookingRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PhRoomBookingTable GetChangesTyped()
        {
            return ((PhRoomBookingTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PhRoomBooking";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "ph_room_booking";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetBookingKeyDBName()
        {
            return "ph_booking_key_i";
        }

        /// get character length for column
        public static short GetBookingKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRoomAllocKeyDBName()
        {
            return "ph_room_alloc_key_i";
        }

        /// get character length for column
        public static short GetRoomAllocKeyLength()
        {
            return -1;
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        public bool IsBookingKeyNull()
        {
            return this.IsNull(this.myTable.ColumnBookingKey);
        }

        /// assign NULL value
        public void SetBookingKeyNull()
        {
            this.SetNull(this.myTable.ColumnBookingKey);
        }

        /// test for NULL value
        public bool IsRoomAllocKeyNull()
        {
            return this.IsNull(this.myTable.ColumnRoomAllocKey);
        }

        /// assign NULL value
        public void SetRoomAllocKeyNull()
        {
            this.SetNull(this.myTable.ColumnRoomAllocKey);
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
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 284;
        /// used for generic TTypedDataTable functions
        public static short ColumnKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnContactKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnLedgerNumberForInvoiceId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnArInvoiceKeyId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfAdultsId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfChildrenId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfBreakfastId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfLunchId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfSupperId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnNumberOfLinenNeededId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnConfirmedId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnInId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnOutId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnTimeArrivalId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnTimeDepartureId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnNotesId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnCreatedById = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnModifiedById = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnModificationIdId = 20;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PhBooking", "ph_booking",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "Key", "ph_key_i", "ph_key_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "ContactKey", "p_contact_key_n", "p_contact_key_n", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(2, "LedgerNumberForInvoice", "a_ledger_number_for_invoice_i", "Ledger Number for Invoice", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "ArInvoiceKey", "a_ar_invoice_key_i", "a_ar_invoice_key_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "NumberOfAdults", "ph_number_of_adults_i", "ph_number_of_adults_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "NumberOfChildren", "ph_number_of_children_i", "ph_number_of_children_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "NumberOfBreakfast", "ph_number_of_breakfast_i", "ph_number_of_breakfast_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "NumberOfLunch", "ph_number_of_lunch_i", "ph_number_of_lunch_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "NumberOfSupper", "ph_number_of_supper_i", "ph_number_of_supper_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "NumberOfLinenNeeded", "ph_number_of_linen_needed_i", "ph_number_of_linen_needed_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(10, "Confirmed", "ph_confirmed_d", "ph_confirmed_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(11, "In", "ph_in_d", "Date In", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(12, "Out", "ph_out_d", "Date Out", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "TimeArrival", "ph_time_arrival_i", "Time of Arrival", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(14, "TimeDeparture", "ph_time_departure_i", "Time of Departure", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(15, "Notes", "ph_notes_c", "ph_notes_c", OdbcType.VarChar, 1000, false),
                    new TTypedColumnInfo(16, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(17, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(18, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(19, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(20, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnKey};
        }

        /// Access a typed row by index
        public PhBookingRow this[int i]
        {
            get
            {
                return ((PhBookingRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PhBookingTable GetChangesTyped()
        {
            return ((PhBookingTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PhBooking";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "ph_booking";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetKeyDBName()
        {
            return "ph_key_i";
        }

        /// get character length for column
        public static short GetKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetContactKeyDBName()
        {
            return "p_contact_key_n";
        }

        /// get character length for column
        public static short GetContactKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetLedgerNumberForInvoiceDBName()
        {
            return "a_ledger_number_for_invoice_i";
        }

        /// get character length for column
        public static short GetLedgerNumberForInvoiceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetArInvoiceKeyDBName()
        {
            return "a_ar_invoice_key_i";
        }

        /// get character length for column
        public static short GetArInvoiceKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfAdultsDBName()
        {
            return "ph_number_of_adults_i";
        }

        /// get character length for column
        public static short GetNumberOfAdultsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfChildrenDBName()
        {
            return "ph_number_of_children_i";
        }

        /// get character length for column
        public static short GetNumberOfChildrenLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfBreakfastDBName()
        {
            return "ph_number_of_breakfast_i";
        }

        /// get character length for column
        public static short GetNumberOfBreakfastLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfLunchDBName()
        {
            return "ph_number_of_lunch_i";
        }

        /// get character length for column
        public static short GetNumberOfLunchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfSupperDBName()
        {
            return "ph_number_of_supper_i";
        }

        /// get character length for column
        public static short GetNumberOfSupperLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfLinenNeededDBName()
        {
            return "ph_number_of_linen_needed_i";
        }

        /// get character length for column
        public static short GetNumberOfLinenNeededLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetConfirmedDBName()
        {
            return "ph_confirmed_d";
        }

        /// get character length for column
        public static short GetConfirmedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetInDBName()
        {
            return "ph_in_d";
        }

        /// get character length for column
        public static short GetInLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetOutDBName()
        {
            return "ph_out_d";
        }

        /// get character length for column
        public static short GetOutLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTimeArrivalDBName()
        {
            return "ph_time_arrival_i";
        }

        /// get character length for column
        public static short GetTimeArrivalLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTimeDepartureDBName()
        {
            return "ph_time_departure_i";
        }

        /// get character length for column
        public static short GetTimeDepartureLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNotesDBName()
        {
            return "ph_notes_c";
        }

        /// get character length for column
        public static short GetNotesLength()
        {
            return 1000;
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
        public System.DateTime? Confirmed
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnConfirmed.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnConfirmed)
                            || (((System.DateTime?)(this[this.myTable.ColumnConfirmed])) != value)))
                {
                    this[this.myTable.ColumnConfirmed] = value;
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
                    return DateTime.MinValue;
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

        ///
        public System.DateTime? Out
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOut.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnOut)
                            || (((System.DateTime?)(this[this.myTable.ColumnOut])) != value)))
                {
                    this[this.myTable.ColumnOut] = value;
                }
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
        public System.DateTime? DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateCreated])) != value)))
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
        public System.DateTime? DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return null;
                }
                else
                {
                    return ((System.DateTime?)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateModified])) != value)))
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
        public bool IsKeyNull()
        {
            return this.IsNull(this.myTable.ColumnKey);
        }

        /// assign NULL value
        public void SetKeyNull()
        {
            this.SetNull(this.myTable.ColumnKey);
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
        public bool IsInNull()
        {
            return this.IsNull(this.myTable.ColumnIn);
        }

        /// assign NULL value
        public void SetInNull()
        {
            this.SetNull(this.myTable.ColumnIn);
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
