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

        private PmGeneralApplicationTable TablePmGeneralApplication;
        private PmShortTermApplicationTable TablePmShortTermApplication;

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
        public new virtual ConferenceApplicationTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((ConferenceApplicationTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PmGeneralApplication") != -1))
            {
                this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            }
            if ((ds.Tables.IndexOf("PmShortTermApplication") != -1))
            {
                this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePmGeneralApplication != null))
            {
                this.TablePmGeneralApplication.InitVars();
            }
            if ((this.TablePmShortTermApplication != null))
            {
                this.TablePmShortTermApplication.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "ConferenceApplicationTDS";
            this.TablePmGeneralApplication = ((PmGeneralApplicationTable)(this.Tables["PmGeneralApplication"]));
            this.TablePmShortTermApplication = ((PmShortTermApplicationTable)(this.Tables["PmShortTermApplication"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePmGeneralApplication != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication1", "PmGeneralApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}, "PmShortTermApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}));
            }
        }
    }
}
