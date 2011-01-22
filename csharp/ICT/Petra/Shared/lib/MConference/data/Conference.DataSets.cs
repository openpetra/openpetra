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
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MConference.Data
{
     /// auto generated
    [Serializable()]
    public class SelectConferenceTDS : TTypedDataSet
    {

        private PPartnerTable TablePPartner;
        private PcConferenceTable TablePcConference;

        /// auto generated
        public SelectConferenceTDS() :
                base("SelectConferenceTDS")
        {
        }

        /// auto generated for serialization
        public SelectConferenceTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public SelectConferenceTDS(string ADatasetName) :
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
        public PcConferenceTable PcConference
        {
            get
            {
                return this.TablePcConference;
            }
        }

        /// auto generated
        public new virtual SelectConferenceTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((SelectConferenceTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new PcConferenceTable("PcConference"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("PcConference") != -1))
            {
                this.Tables.Add(new PcConferenceTable("PcConference"));
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
            if ((this.TablePcConference != null))
            {
                this.TablePcConference.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "SelectConferenceTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TablePcConference = ((PcConferenceTable)(this.Tables["PcConference"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
        }
    }
     /// auto generated
    [Serializable()]
    public class ConferenceApplicationTDS : TTypedDataSet
    {

        private PPartnerTable TablePPartner;
        private PPersonTable TablePPerson;
        private PmGeneralApplicationTable TablePmGeneralApplication;
        private PmShortTermApplicationTable TablePmShortTermApplication;
        private ConferenceApplicationTDSApplicationGridTable TableApplicationGrid;

        /// auto generated
        public ConferenceApplicationTDS() :
                base("ConferenceApplicationTDS")
        {
        }

        /// auto generated for serialization
        public ConferenceApplicationTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public ConferenceApplicationTDS(string ADatasetName) :
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
        public PPersonTable PPerson
        {
            get
            {
                return this.TablePPerson;
            }
        }

        /// auto generated
        public PmGeneralApplicationTable PmGeneralApplication
        {
            get
            {
                return this.TablePmGeneralApplication;
            }
        }

        /// auto generated
        public PmShortTermApplicationTable PmShortTermApplication
        {
            get
            {
                return this.TablePmShortTermApplication;
            }
        }

        /// auto generated
        public ConferenceApplicationTDSApplicationGridTable ApplicationGrid
        {
            get
            {
                return this.TableApplicationGrid;
            }
        }

        /// auto generated
        public new virtual ConferenceApplicationTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((ConferenceApplicationTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new PPersonTable("PPerson"));
            this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
            this.Tables.Add(new ConferenceApplicationTDSApplicationGridTable("ApplicationGrid"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("PPerson") != -1))
            {
                this.Tables.Add(new PPersonTable("PPerson"));
            }
            if ((ds.Tables.IndexOf("PmGeneralApplication") != -1))
            {
                this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            }
            if ((ds.Tables.IndexOf("PmShortTermApplication") != -1))
            {
                this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
            }
            if ((ds.Tables.IndexOf("ApplicationGrid") != -1))
            {
                this.Tables.Add(new ConferenceApplicationTDSApplicationGridTable("ApplicationGrid"));
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
            if ((this.TablePPerson != null))
            {
                this.TablePPerson.InitVars();
            }
            if ((this.TablePmGeneralApplication != null))
            {
                this.TablePmGeneralApplication.InitVars();
            }
            if ((this.TablePmShortTermApplication != null))
            {
                this.TablePmShortTermApplication.InitVars();
            }
            if ((this.TableApplicationGrid != null))
            {
                this.TableApplicationGrid.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "ConferenceApplicationTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TablePPerson = ((PPersonTable)(this.Tables["PPerson"]));
            this.TablePmGeneralApplication = ((PmGeneralApplicationTable)(this.Tables["PmGeneralApplication"]));
            this.TablePmShortTermApplication = ((PmShortTermApplicationTable)(this.Tables["PmShortTermApplication"]));
            this.TableApplicationGrid = ((ConferenceApplicationTDSApplicationGridTable)(this.Tables["ApplicationGrid"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePPartner != null)
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication9", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "pm_placement_partner_key_n"}));
            }
            if (((this.TablePmGeneralApplication != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication1", "PmGeneralApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}, "PmShortTermApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication6", "PPartner", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_party_contact_n"}));
            }
        }
    }

    ///
    [Serializable()]
    public class ConferenceApplicationTDSApplicationGridTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5800;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnFirstNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyNameId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenderId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfBirthId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenAppDateId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenApplicationStatusId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnStCongressCodeId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ApplicationGrid", "ConferenceApplicationTDSApplicationGrid",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "FirstName", "p_first_name_c", "First Name", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(2, "FamilyName", "p_family_name_c", "Family Name", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(3, "Gender", "p_gender_c", "Gender", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "DateOfBirth", "p_date_of_birth_d", "Date of Birth", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "GenAppDate", "pm_gen_app_date_d", "Application Date", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(6, "GenApplicationStatus", "pm_gen_application_status_c", "Application Status", OdbcType.VarChar, 32, false),
                    new TTypedColumnInfo(7, "StCongressCode", "pm_st_congress_code_c", "Conference Role", OdbcType.VarChar, 32, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public ConferenceApplicationTDSApplicationGridTable() :
                base("ApplicationGrid")
        {
        }

        /// constructor
        public ConferenceApplicationTDSApplicationGridTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public ConferenceApplicationTDSApplicationGridTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnFirstName;
        ///
        public DataColumn ColumnFamilyName;
        ///
        public DataColumn ColumnGender;
        /// This is the date the rthe person was born
        public DataColumn ColumnDateOfBirth;
        /// Date of application.
        public DataColumn ColumnGenAppDate;
        /// Indicates the status of the application.
        public DataColumn ColumnGenApplicationStatus;
        /// Indicates the role for the Congress.
        public DataColumn ColumnStCongressCode;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_gender_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_date_of_birth_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pm_gen_app_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("pm_gen_application_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("pm_st_congress_code_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnFirstName = this.Columns["p_first_name_c"];
            this.ColumnFamilyName = this.Columns["p_family_name_c"];
            this.ColumnGender = this.Columns["p_gender_c"];
            this.ColumnDateOfBirth = this.Columns["p_date_of_birth_d"];
            this.ColumnGenAppDate = this.Columns["pm_gen_app_date_d"];
            this.ColumnGenApplicationStatus = this.Columns["pm_gen_application_status_c"];
            this.ColumnStCongressCode = this.Columns["pm_st_congress_code_c"];
        }

        /// Access a typed row by index
        public ConferenceApplicationTDSApplicationGridRow this[int i]
        {
            get
            {
                return ((ConferenceApplicationTDSApplicationGridRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public ConferenceApplicationTDSApplicationGridRow NewRowTyped(bool AWithDefaultValues)
        {
            ConferenceApplicationTDSApplicationGridRow ret = ((ConferenceApplicationTDSApplicationGridRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public ConferenceApplicationTDSApplicationGridRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ConferenceApplicationTDSApplicationGridRow(builder);
        }

        /// get typed set of changes
        public ConferenceApplicationTDSApplicationGridTable GetChangesTyped()
        {
            return ((ConferenceApplicationTDSApplicationGridTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ApplicationGrid";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "ConferenceApplicationTDSApplicationGrid";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
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
        public static string GetFirstNameDBName()
        {
            return "p_first_name_c";
        }

        /// get character length for column
        public static short GetFirstNameLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetFamilyNameDBName()
        {
            return "p_family_name_c";
        }

        /// get character length for column
        public static short GetFamilyNameLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetGenderDBName()
        {
            return "p_gender_c";
        }

        /// get character length for column
        public static short GetGenderLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDateOfBirthDBName()
        {
            return "p_date_of_birth_d";
        }

        /// get character length for column
        public static short GetDateOfBirthLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGenAppDateDBName()
        {
            return "pm_gen_app_date_d";
        }

        /// get character length for column
        public static short GetGenAppDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGenApplicationStatusDBName()
        {
            return "pm_gen_application_status_c";
        }

        /// get character length for column
        public static short GetGenApplicationStatusLength()
        {
            return 32;
        }

        /// get the name of the field in the database for this column
        public static string GetStCongressCodeDBName()
        {
            return "pm_st_congress_code_c";
        }

        /// get character length for column
        public static short GetStCongressCodeLength()
        {
            return 32;
        }

    }

    ///
    [Serializable()]
    public class ConferenceApplicationTDSApplicationGridRow : System.Data.DataRow
    {
        private ConferenceApplicationTDSApplicationGridTable myTable;

        /// Constructor
        public ConferenceApplicationTDSApplicationGridRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((ConferenceApplicationTDSApplicationGridTable)(this.Table));
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
        public String FirstName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFirstName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFirstName)
                            || (((String)(this[this.myTable.ColumnFirstName])) != value)))
                {
                    this[this.myTable.ColumnFirstName] = value;
                }
            }
        }

        ///
        public String FamilyName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFamilyName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFamilyName)
                            || (((String)(this[this.myTable.ColumnFamilyName])) != value)))
                {
                    this[this.myTable.ColumnFamilyName] = value;
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
        public System.DateTime? DateOfBirth
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfBirth.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateOfBirth)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateOfBirth])) != value)))
                {
                    this[this.myTable.ColumnDateOfBirth] = value;
                }
            }
        }

        /// Date of application.
        public System.DateTime GenAppDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGenAppDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGenAppDate)
                            || (((System.DateTime)(this[this.myTable.ColumnGenAppDate])) != value)))
                {
                    this[this.myTable.ColumnGenAppDate] = value;
                }
            }
        }

        /// Indicates the status of the application.
        public String GenApplicationStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGenApplicationStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGenApplicationStatus)
                            || (((String)(this[this.myTable.ColumnGenApplicationStatus])) != value)))
                {
                    this[this.myTable.ColumnGenApplicationStatus] = value;
                }
            }
        }

        /// Indicates the role for the Congress.
        public String StCongressCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStCongressCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStCongressCode)
                            || (((String)(this[this.myTable.ColumnStCongressCode])) != value)))
                {
                    this[this.myTable.ColumnStCongressCode] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnFamilyName);
            this[this.myTable.ColumnGender.Ordinal] = "Unknown";
            this.SetNull(this.myTable.ColumnDateOfBirth);
            this.SetNull(this.myTable.ColumnGenAppDate);
            this.SetNull(this.myTable.ColumnGenApplicationStatus);
            this.SetNull(this.myTable.ColumnStCongressCode);
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
        public bool IsFirstNameNull()
        {
            return this.IsNull(this.myTable.ColumnFirstName);
        }

        /// assign NULL value
        public void SetFirstNameNull()
        {
            this.SetNull(this.myTable.ColumnFirstName);
        }

        /// test for NULL value
        public bool IsFamilyNameNull()
        {
            return this.IsNull(this.myTable.ColumnFamilyName);
        }

        /// assign NULL value
        public void SetFamilyNameNull()
        {
            this.SetNull(this.myTable.ColumnFamilyName);
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
        public bool IsGenAppDateNull()
        {
            return this.IsNull(this.myTable.ColumnGenAppDate);
        }

        /// assign NULL value
        public void SetGenAppDateNull()
        {
            this.SetNull(this.myTable.ColumnGenAppDate);
        }

        /// test for NULL value
        public bool IsGenApplicationStatusNull()
        {
            return this.IsNull(this.myTable.ColumnGenApplicationStatus);
        }

        /// assign NULL value
        public void SetGenApplicationStatusNull()
        {
            this.SetNull(this.myTable.ColumnGenApplicationStatus);
        }

        /// test for NULL value
        public bool IsStCongressCodeNull()
        {
            return this.IsNull(this.myTable.ColumnStCongressCode);
        }

        /// assign NULL value
        public void SetStCongressCodeNull()
        {
            this.SetNull(this.myTable.ColumnStCongressCode);
        }
    }
}
