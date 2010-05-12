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
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MHospitality.Data;

namespace Ict.Petra.Shared.MHospitality.Data
{

     /// auto generated
    [Serializable()]
    public class HospitalityTDS : TTypedDataSet
    {

        private PcBuildingTable TablePcBuilding;
        private PcRoomTable TablePcRoom;
        private PcRoomAllocTable TablePcRoomAlloc;
        private PcRoomAttributeTable TablePcRoomAttribute;
        private PhBookingTable TablePhBooking;
        private PhRoomBookingTable TablePhRoomBooking;

        /// auto generated
        public HospitalityTDS() :
                base("HospitalityTDS")
        {
        }

        /// auto generated for serialization
        public HospitalityTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public HospitalityTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public PcBuildingTable PcBuilding
        {
            get
            {
                return this.TablePcBuilding;
            }
        }

        /// auto generated
        public PcRoomTable PcRoom
        {
            get
            {
                return this.TablePcRoom;
            }
        }

        /// auto generated
        public PcRoomAllocTable PcRoomAlloc
        {
            get
            {
                return this.TablePcRoomAlloc;
            }
        }

        /// auto generated
        public PcRoomAttributeTable PcRoomAttribute
        {
            get
            {
                return this.TablePcRoomAttribute;
            }
        }

        /// auto generated
        public PhBookingTable PhBooking
        {
            get
            {
                return this.TablePhBooking;
            }
        }

        /// auto generated
        public PhRoomBookingTable PhRoomBooking
        {
            get
            {
                return this.TablePhRoomBooking;
            }
        }

        /// auto generated
        public new virtual HospitalityTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((HospitalityTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PcBuildingTable("PcBuilding"));
            this.Tables.Add(new PcRoomTable("PcRoom"));
            this.Tables.Add(new PcRoomAllocTable("PcRoomAlloc"));
            this.Tables.Add(new PcRoomAttributeTable("PcRoomAttribute"));
            this.Tables.Add(new PhBookingTable("PhBooking"));
            this.Tables.Add(new PhRoomBookingTable("PhRoomBooking"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PcBuilding") != -1))
            {
                this.Tables.Add(new PcBuildingTable("PcBuilding"));
            }
            if ((ds.Tables.IndexOf("PcRoom") != -1))
            {
                this.Tables.Add(new PcRoomTable("PcRoom"));
            }
            if ((ds.Tables.IndexOf("PcRoomAlloc") != -1))
            {
                this.Tables.Add(new PcRoomAllocTable("PcRoomAlloc"));
            }
            if ((ds.Tables.IndexOf("PcRoomAttribute") != -1))
            {
                this.Tables.Add(new PcRoomAttributeTable("PcRoomAttribute"));
            }
            if ((ds.Tables.IndexOf("PhBooking") != -1))
            {
                this.Tables.Add(new PhBookingTable("PhBooking"));
            }
            if ((ds.Tables.IndexOf("PhRoomBooking") != -1))
            {
                this.Tables.Add(new PhRoomBookingTable("PhRoomBooking"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePcBuilding != null))
            {
                this.TablePcBuilding.InitVars();
            }
            if ((this.TablePcRoom != null))
            {
                this.TablePcRoom.InitVars();
            }
            if ((this.TablePcRoomAlloc != null))
            {
                this.TablePcRoomAlloc.InitVars();
            }
            if ((this.TablePcRoomAttribute != null))
            {
                this.TablePcRoomAttribute.InitVars();
            }
            if ((this.TablePhBooking != null))
            {
                this.TablePhBooking.InitVars();
            }
            if ((this.TablePhRoomBooking != null))
            {
                this.TablePhRoomBooking.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "HospitalityTDS";
            this.TablePcBuilding = ((PcBuildingTable)(this.Tables["PcBuilding"]));
            this.TablePcRoom = ((PcRoomTable)(this.Tables["PcRoom"]));
            this.TablePcRoomAlloc = ((PcRoomAllocTable)(this.Tables["PcRoomAlloc"]));
            this.TablePcRoomAttribute = ((PcRoomAttributeTable)(this.Tables["PcRoomAttribute"]));
            this.TablePhBooking = ((PhBookingTable)(this.Tables["PhBooking"]));
            this.TablePhRoomBooking = ((PhRoomBookingTable)(this.Tables["PhRoomBooking"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TablePcBuilding != null)
                        && (this.TablePcRoom != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoom1", "PcBuilding", new string[] {
                                "p_venue_key_n", "pc_building_code_c"}, "PcRoom", new string[] {
                                "p_venue_key_n", "pc_building_code_c"}));
            }
            if (((this.TablePcRoom != null)
                        && (this.TablePcRoomAlloc != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoomAlloc2", "PcRoom", new string[] {
                                "p_venue_key_n", "pc_building_code_c", "pc_room_number_c"}, "PcRoomAlloc", new string[] {
                                "p_venue_key_n", "pc_building_code_c", "pc_room_number_c"}));
            }
            if (((this.TablePcRoom != null)
                        && (this.TablePcRoomAttribute != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoomAttribute1", "PcRoom", new string[] {
                                "p_venue_key_n", "pc_building_code_c", "pc_room_number_c"}, "PcRoomAttribute", new string[] {
                                "p_venue_key_n", "pc_building_code_c", "pc_room_number_c"}));
            }
            if (((this.TablePhBooking != null)
                        && (this.TablePhRoomBooking != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoomBooking1", "PhBooking", new string[] {
                                "ph_key_i"}, "PhRoomBooking", new string[] {
                                "ph_booking_key_i"}));
            }
            if (((this.TablePcRoomAlloc != null)
                        && (this.TablePhRoomBooking != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoomBooking2", "PcRoomAlloc", new string[] {
                                "pc_key_i"}, "PhRoomBooking", new string[] {
                                "ph_room_alloc_key_i"}));
            }
        }
    }
}
