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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Shared.MPartner.Partner.Data
{
     /// auto generated
    [Serializable()]
    public class PartnerEditTDS : TTypedDataSet
    {

        private PPartnerTable TablePPartner;
        private PPartnerTypeTable TablePPartnerType;
        private PSubscriptionTable TablePSubscription;
        private PLocationTable TablePLocation;
        private PartnerEditTDSPPartnerLocationTable TablePPartnerLocation;
        private PartnerEditTDSPFamilyTable TablePFamily;
        private PartnerEditTDSPPersonTable TablePPerson;
        private PUnitTable TablePUnit;
        private UmUnitStructureTable TableUmUnitStructure;
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
        public PLocationTable PLocation
        {
            get
            {
                return this.TablePLocation;
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
        public PartnerEditTDSPFamilyTable PFamily
        {
            get
            {
                return this.TablePFamily;
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
        public PUnitTable PUnit
        {
            get
            {
                return this.TablePUnit;
            }
        }

        /// auto generated
        public UmUnitStructureTable UmUnitStructure
        {
            get
            {
                return this.TableUmUnitStructure;
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
            this.Tables.Add(new PLocationTable("PLocation"));
            this.Tables.Add(new PartnerEditTDSPPartnerLocationTable("PPartnerLocation"));
            this.Tables.Add(new PartnerEditTDSPFamilyTable("PFamily"));
            this.Tables.Add(new PartnerEditTDSPPersonTable("PPerson"));
            this.Tables.Add(new PUnitTable("PUnit"));
            this.Tables.Add(new UmUnitStructureTable("UmUnitStructure"));
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
            if ((ds.Tables.IndexOf("PLocation") != -1))
            {
                this.Tables.Add(new PLocationTable("PLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerLocation") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPPartnerLocationTable("PPartnerLocation"));
            }
            if ((ds.Tables.IndexOf("PFamily") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPFamilyTable("PFamily"));
            }
            if ((ds.Tables.IndexOf("PPerson") != -1))
            {
                this.Tables.Add(new PartnerEditTDSPPersonTable("PPerson"));
            }
            if ((ds.Tables.IndexOf("PUnit") != -1))
            {
                this.Tables.Add(new PUnitTable("PUnit"));
            }
            if ((ds.Tables.IndexOf("UmUnitStructure") != -1))
            {
                this.Tables.Add(new UmUnitStructureTable("UmUnitStructure"));
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
            if ((this.TablePLocation != null))
            {
                this.TablePLocation.InitVars();
            }
            if ((this.TablePPartnerLocation != null))
            {
                this.TablePPartnerLocation.InitVars();
            }
            if ((this.TablePFamily != null))
            {
                this.TablePFamily.InitVars();
            }
            if ((this.TablePPerson != null))
            {
                this.TablePPerson.InitVars();
            }
            if ((this.TablePUnit != null))
            {
                this.TablePUnit.InitVars();
            }
            if ((this.TableUmUnitStructure != null))
            {
                this.TableUmUnitStructure.InitVars();
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
            this.TablePLocation = ((PLocationTable)(this.Tables["PLocation"]));
            this.TablePPartnerLocation = ((PartnerEditTDSPPartnerLocationTable)(this.Tables["PPartnerLocation"]));
            this.TablePFamily = ((PartnerEditTDSPFamilyTable)(this.Tables["PFamily"]));
            this.TablePPerson = ((PartnerEditTDSPPersonTable)(this.Tables["PPerson"]));
            this.TablePUnit = ((PUnitTable)(this.Tables["PUnit"]));
            this.TableUmUnitStructure = ((UmUnitStructureTable)(this.Tables["UmUnitStructure"]));
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
                                "p_foundation_partner_key_n", "p_foundation_proposal_key_i"}, "PFoundationProposalDetail", new string[] {
                                "p_foundation_partner_key_n", "p_foundation_proposal_key_i"}));
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
                        && (this.TablePPartnerContact != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerContact1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerContact", new string[] {
                                "p_partner_key_n"}));
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
                        && (this.TablePPartnerType != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerType3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerType", new string[] {
                                "p_partner_key_n"}));
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
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitStructure != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitStructure1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitStructure", new string[] {
                                "um_parent_unit_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitStructure != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitStructure2", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitStructure", new string[] {
                                "um_child_unit_key_n"}));
            }
            this.FRelations.Add(new TTypedRelation("Address", "PPartnerLocation", new string[] {
                            "p_site_key_n", "p_location_key_i"}, "PLocation", new string[] {
                            "p_site_key_n", "p_location_key_i"}, false));
            this.FRelations.Add(new TTypedRelation("PartnerInterestCategory", "PInterest", new string[] {
                            "p_interest_c"}, "PPartnerInterest", new string[] {
                            "p_interest_c"}, false));
        }
    }

    /// Links partners with locations (addresses) and has specific info about the link (e.g. phone number)
    [Serializable()]
    public class PartnerEditTDSPPartnerLocationTable : PPartnerLocationTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 42;
        /// used for generic TTypedDataTable functions
        public static short ColumnBestAddressId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnIconId = 24;

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

        ///
        public DataColumn ColumnBestAddress;
        ///
        public DataColumn ColumnIcon;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_date_effective_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_date_good_until_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_location_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_send_mail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_fax_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_telex_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_telephone_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_extension_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_location_detail_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_fax_extension_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_mobile_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_alternate_telephone_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_url_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnDateEffective = this.Columns["p_date_effective_d"];
            this.ColumnDateGoodUntil = this.Columns["p_date_good_until_d"];
            this.ColumnLocationType = this.Columns["p_location_type_c"];
            this.ColumnSendMail = this.Columns["p_send_mail_l"];
            this.ColumnFaxNumber = this.Columns["p_fax_number_c"];
            this.ColumnTelex = this.Columns["p_telex_i"];
            this.ColumnTelephoneNumber = this.Columns["p_telephone_number_c"];
            this.ColumnExtension = this.Columns["p_extension_i"];
            this.ColumnEmailAddress = this.Columns["p_email_address_c"];
            this.ColumnLocationDetailComment = this.Columns["p_location_detail_comment_c"];
            this.ColumnFaxExtension = this.Columns["p_fax_extension_i"];
            this.ColumnMobileNumber = this.Columns["p_mobile_number_c"];
            this.ColumnAlternateTelephone = this.Columns["p_alternate_telephone_c"];
            this.ColumnUrl = this.Columns["p_url_c"];
            this.ColumnRestricted = this.Columns["p_restricted_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnBestAddress = this.Columns["BestAddress"];
            this.ColumnIcon = this.Columns["Icon"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnPartnerKey,ColumnSiteKey,ColumnLocationKey};
        }

        /// Access a typed row by index
        public new PartnerEditTDSPPartnerLocationRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPPartnerLocationRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public new PartnerEditTDSPPartnerLocationRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPPartnerLocationRow(builder);
        }

        /// get typed set of changes
        public new PartnerEditTDSPPartnerLocationTable GetChangesTyped()
        {
            return ((PartnerEditTDSPPartnerLocationTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "PPartnerLocation";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_partner_location";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetBestAddressDBName()
        {
            return "BestAddress";
        }

        /// get character length for column
        public static short GetBestAddressLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetIconDBName()
        {
            return "Icon";
        }

        /// get character length for column
        public static short GetIconLength()
        {
            return -1;
        }

    }

    /// Links partners with locations (addresses) and has specific info about the link (e.g. phone number)
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
        public bool BestAddress
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBestAddress)
                            || (((bool)(this[this.myTable.ColumnBestAddress])) != value)))
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnDateEffective);
            this.SetNull(this.myTable.ColumnDateGoodUntil);
            this.SetNull(this.myTable.ColumnLocationType);
            this[this.myTable.ColumnSendMail.Ordinal] = false;
            this.SetNull(this.myTable.ColumnFaxNumber);
            this[this.myTable.ColumnTelex.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTelephoneNumber);
            this[this.myTable.ColumnExtension.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnEmailAddress);
            this.SetNull(this.myTable.ColumnLocationDetailComment);
            this[this.myTable.ColumnFaxExtension.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMobileNumber);
            this.SetNull(this.myTable.ColumnAlternateTelephone);
            this.SetNull(this.myTable.ColumnUrl);
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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

    /// Contains details about a family in Partnership with us.  May have P_Person records linked to it.
    [Serializable()]
    public class PartnerEditTDSPFamilyTable : PFamilyTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 48;
        /// used for generic TTypedDataTable functions
        public static short ColumnUnitNameId = 15;

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

        ///
        public DataColumn ColumnUnitName;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_family_members_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_different_surnames_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_field_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_since_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnFamilyMembers = this.Columns["p_family_members_l"];
            this.ColumnTitle = this.Columns["p_title_c"];
            this.ColumnFirstName = this.Columns["p_first_name_c"];
            this.ColumnFamilyName = this.Columns["p_family_name_c"];
            this.ColumnDifferentSurnames = this.Columns["p_different_surnames_l"];
            this.ColumnFieldKey = this.Columns["p_field_key_n"];
            this.ColumnMaritalStatus = this.Columns["p_marital_status_c"];
            this.ColumnMaritalStatusSince = this.Columns["p_marital_status_since_d"];
            this.ColumnMaritalStatusComment = this.Columns["p_marital_status_comment_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnUnitName = this.Columns["p_unit_name_c"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public new PartnerEditTDSPFamilyRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPFamilyRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public new PartnerEditTDSPFamilyRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPFamilyRow(builder);
        }

        /// get typed set of changes
        public new PartnerEditTDSPFamilyTable GetChangesTyped()
        {
            return ((PartnerEditTDSPFamilyTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "PFamily";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_family";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitNameDBName()
        {
            return "p_unit_name_c";
        }

        /// get character length for column
        public static short GetUnitNameLength()
        {
            return 160;
        }

    }

    /// Contains details about a family in Partnership with us.  May have P_Person records linked to it.
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnFamilyMembers.Ordinal] = false;
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnFamilyName);
            this[this.myTable.ColumnDifferentSurnames.Ordinal] = false;
            this.SetNull(this.myTable.ColumnFieldKey);
            this[this.myTable.ColumnMaritalStatus.Ordinal] = "U";
            this.SetNull(this.myTable.ColumnMaritalStatusSince);
            this.SetNull(this.myTable.ColumnMaritalStatusComment);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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

    /// Details of a person.  A person must also have a related FAMILY class p_partner record.
    [Serializable()]
    public class PartnerEditTDSPPersonTable : PPersonTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 51;
        /// used for generic TTypedDataTable functions
        public static short ColumnUnitNameId = 26;

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

        ///
        public DataColumn ColumnUnitName;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_prefered_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_middle_name_1_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_middle_name_2_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_middle_name_3_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_decorations_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_date_of_birth_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_gender_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_occupation_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_believer_since_year_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_believer_since_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_family_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_field_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_academic_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_since_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_marital_status_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnTitle = this.Columns["p_title_c"];
            this.ColumnFirstName = this.Columns["p_first_name_c"];
            this.ColumnPreferedName = this.Columns["p_prefered_name_c"];
            this.ColumnMiddleName1 = this.Columns["p_middle_name_1_c"];
            this.ColumnMiddleName2 = this.Columns["p_middle_name_2_c"];
            this.ColumnMiddleName3 = this.Columns["p_middle_name_3_c"];
            this.ColumnFamilyName = this.Columns["p_family_name_c"];
            this.ColumnDecorations = this.Columns["p_decorations_c"];
            this.ColumnDateOfBirth = this.Columns["p_date_of_birth_d"];
            this.ColumnGender = this.Columns["p_gender_c"];
            this.ColumnMaritalStatus = this.Columns["p_marital_status_c"];
            this.ColumnOccupationCode = this.Columns["p_occupation_code_c"];
            this.ColumnBelieverSinceYear = this.Columns["p_believer_since_year_i"];
            this.ColumnBelieverSinceComment = this.Columns["p_believer_since_comment_c"];
            this.ColumnFamilyKey = this.Columns["p_family_key_n"];
            this.ColumnFamilyId = this.Columns["p_family_id_i"];
            this.ColumnFieldKey = this.Columns["p_field_key_n"];
            this.ColumnAcademicTitle = this.Columns["p_academic_title_c"];
            this.ColumnMaritalStatusSince = this.Columns["p_marital_status_since_d"];
            this.ColumnMaritalStatusComment = this.Columns["p_marital_status_comment_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnUnitName = this.Columns["p_unit_name_c"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public new PartnerEditTDSPPersonRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPPersonRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public new PartnerEditTDSPPersonRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPPersonRow(builder);
        }

        /// get typed set of changes
        public new PartnerEditTDSPPersonTable GetChangesTyped()
        {
            return ((PartnerEditTDSPPersonTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "PPerson";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_person";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetUnitNameDBName()
        {
            return "p_unit_name_c";
        }

        /// get character length for column
        public static short GetUnitNameLength()
        {
            return 160;
        }

    }

    /// Details of a person.  A person must also have a related FAMILY class p_partner record.
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnPreferedName);
            this.SetNull(this.myTable.ColumnMiddleName1);
            this.SetNull(this.myTable.ColumnMiddleName2);
            this.SetNull(this.myTable.ColumnMiddleName3);
            this.SetNull(this.myTable.ColumnFamilyName);
            this.SetNull(this.myTable.ColumnDecorations);
            this.SetNull(this.myTable.ColumnDateOfBirth);
            this[this.myTable.ColumnGender.Ordinal] = "Unknown";
            this[this.myTable.ColumnMaritalStatus.Ordinal] = "U";
            this.SetNull(this.myTable.ColumnOccupationCode);
            this.SetNull(this.myTable.ColumnBelieverSinceYear);
            this.SetNull(this.myTable.ColumnBelieverSinceComment);
            this[this.myTable.ColumnFamilyKey.Ordinal] = 0;
            this[this.myTable.ColumnFamilyId.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnFieldKey);
            this.SetNull(this.myTable.ColumnAcademicTitle);
            this.SetNull(this.myTable.ColumnMaritalStatusSince);
            this.SetNull(this.myTable.ColumnMaritalStatusComment);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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

    ///
    [Serializable()]
    public class PartnerEditTDSMiscellaneousDataTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5103;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnSelectedSiteKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnSelectedLocationKeyId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastGiftDateId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastGiftInfoId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastContactDateId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountAddressesId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountAddressesActiveId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountSubscriptionsId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountSubscriptionsActiveId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountPartnerTypesId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountFamilyMembersId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountInterestsId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountRemindersId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountRelationshipsId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnItemsCountContactsId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnOfficeSpecificDataLabelsAvailableId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnFoundationOwner1KeyId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnFoundationOwner2KeyId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnHasEXWORKERPartnerTypeId = 19;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "MiscellaneousData", "PartnerEditTDSMiscellaneousData",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "SelectedSiteKey", "p_site_key_n", "Site Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "SelectedLocationKey", "p_location_key_i", "Location Key", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "LastGiftDate", "LastGiftDate", "", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "LastGiftInfo", "LastGiftInfo", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(5, "LastContactDate", "LastContactDate", "", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ItemsCountAddresses", "ItemsCountAddresses", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "ItemsCountAddressesActive", "ItemsCountAddressesActive", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(8, "ItemsCountSubscriptions", "ItemsCountSubscriptions", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "ItemsCountSubscriptionsActive", "ItemsCountSubscriptionsActive", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(10, "ItemsCountPartnerTypes", "ItemsCountPartnerTypes", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "ItemsCountFamilyMembers", "ItemsCountFamilyMembers", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(12, "ItemsCountInterests", "ItemsCountInterests", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(13, "ItemsCountReminders", "ItemsCountReminders", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(14, "ItemsCountRelationships", "ItemsCountRelationships", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(15, "ItemsCountContacts", "ItemsCountContacts", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(16, "OfficeSpecificDataLabelsAvailable", "OfficeSpecificDataLabelsAvailable", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(17, "FoundationOwner1Key", "FoundationOwner1Key", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(18, "FoundationOwner2Key", "FoundationOwner2Key", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(19, "HasEXWORKERPartnerType", "HasEXWORKERPartnerType", "", OdbcType.Bit, -1, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("LastGiftDate", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("LastGiftInfo", typeof(string)));
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
            this.Columns.Add(new System.Data.DataColumn("OfficeSpecificDataLabelsAvailable", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("FoundationOwner1Key", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("FoundationOwner2Key", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("HasEXWORKERPartnerType", typeof(bool)));
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
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public PartnerEditTDSMiscellaneousDataRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSMiscellaneousDataRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerEditTDSMiscellaneousDataRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSMiscellaneousDataRow(builder);
        }

        /// get typed set of changes
        public PartnerEditTDSMiscellaneousDataTable GetChangesTyped()
        {
            return ((PartnerEditTDSMiscellaneousDataTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "MiscellaneousData";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerEditTDSMiscellaneousData";
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
        public static string GetSelectedSiteKeyDBName()
        {
            return "p_site_key_n";
        }

        /// get character length for column
        public static short GetSelectedSiteKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetSelectedLocationKeyDBName()
        {
            return "p_location_key_i";
        }

        /// get character length for column
        public static short GetSelectedLocationKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastGiftDateDBName()
        {
            return "LastGiftDate";
        }

        /// get character length for column
        public static short GetLastGiftDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastGiftInfoDBName()
        {
            return "LastGiftInfo";
        }

        /// get character length for column
        public static short GetLastGiftInfoLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastContactDateDBName()
        {
            return "LastContactDate";
        }

        /// get character length for column
        public static short GetLastContactDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountAddressesDBName()
        {
            return "ItemsCountAddresses";
        }

        /// get character length for column
        public static short GetItemsCountAddressesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountAddressesActiveDBName()
        {
            return "ItemsCountAddressesActive";
        }

        /// get character length for column
        public static short GetItemsCountAddressesActiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountSubscriptionsDBName()
        {
            return "ItemsCountSubscriptions";
        }

        /// get character length for column
        public static short GetItemsCountSubscriptionsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountSubscriptionsActiveDBName()
        {
            return "ItemsCountSubscriptionsActive";
        }

        /// get character length for column
        public static short GetItemsCountSubscriptionsActiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountPartnerTypesDBName()
        {
            return "ItemsCountPartnerTypes";
        }

        /// get character length for column
        public static short GetItemsCountPartnerTypesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountFamilyMembersDBName()
        {
            return "ItemsCountFamilyMembers";
        }

        /// get character length for column
        public static short GetItemsCountFamilyMembersLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountInterestsDBName()
        {
            return "ItemsCountInterests";
        }

        /// get character length for column
        public static short GetItemsCountInterestsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountRemindersDBName()
        {
            return "ItemsCountReminders";
        }

        /// get character length for column
        public static short GetItemsCountRemindersLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountRelationshipsDBName()
        {
            return "ItemsCountRelationships";
        }

        /// get character length for column
        public static short GetItemsCountRelationshipsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetItemsCountContactsDBName()
        {
            return "ItemsCountContacts";
        }

        /// get character length for column
        public static short GetItemsCountContactsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetOfficeSpecificDataLabelsAvailableDBName()
        {
            return "OfficeSpecificDataLabelsAvailable";
        }

        /// get character length for column
        public static short GetOfficeSpecificDataLabelsAvailableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetFoundationOwner1KeyDBName()
        {
            return "FoundationOwner1Key";
        }

        /// get character length for column
        public static short GetFoundationOwner1KeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetFoundationOwner2KeyDBName()
        {
            return "FoundationOwner2Key";
        }

        /// get character length for column
        public static short GetFoundationOwner2KeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetHasEXWORKERPartnerTypeDBName()
        {
            return "HasEXWORKERPartnerType";
        }

        /// get character length for column
        public static short GetHasEXWORKERPartnerTypeLength()
        {
            return -1;
        }

    }

    ///
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
                    return DateTime.MinValue;
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
        public string LastGiftInfo
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastGiftInfo.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastGiftInfo)
                            || (((string)(this[this.myTable.ColumnLastGiftInfo])) != value)))
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
                    return DateTime.MinValue;
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
        public bool OfficeSpecificDataLabelsAvailable
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnOfficeSpecificDataLabelsAvailable)
                            || (((bool)(this[this.myTable.ColumnOfficeSpecificDataLabelsAvailable])) != value)))
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
        public bool HasEXWORKERPartnerType
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnHasEXWORKERPartnerType)
                            || (((bool)(this[this.myTable.ColumnHasEXWORKERPartnerType])) != value)))
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
        public bool IsSelectedSiteKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSelectedSiteKey);
        }

        /// assign NULL value
        public void SetSelectedSiteKeyNull()
        {
            this.SetNull(this.myTable.ColumnSelectedSiteKey);
        }

        /// test for NULL value
        public bool IsSelectedLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSelectedLocationKey);
        }

        /// assign NULL value
        public void SetSelectedLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnSelectedLocationKey);
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

    ///
    [Serializable()]
    public class PartnerEditTDSFamilyMembersTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5104;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyIdId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnGenderId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfBirthId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnTypeCodeModifyId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnTypeCodePresentId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnOtherTypeCodesId = 7;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "FamilyMembers", "PartnerEditTDSFamilyMembers",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerShortName", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "FamilyId", "p_family_id_i", "Family ID", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "Gender", "p_gender_c", "Gender", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "DateOfBirth", "p_date_of_birth_d", "Date of Birth", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "TypeCodeModify", "TypeCodeModify", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "TypeCodePresent", "TypeCodePresent", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(7, "OtherTypeCodes", "OtherTypeCodes", "", OdbcType.VarChar, -1, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        /// This field indicates the family id of the individual.
        /// ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_gender_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_date_of_birth_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("TypeCodeModify", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("TypeCodePresent", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("OtherTypeCodes", typeof(string)));
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
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public PartnerEditTDSFamilyMembersRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSFamilyMembersRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerEditTDSFamilyMembersRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSFamilyMembersRow(builder);
        }

        /// get typed set of changes
        public PartnerEditTDSFamilyMembersTable GetChangesTyped()
        {
            return ((PartnerEditTDSFamilyMembersTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "FamilyMembers";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerEditTDSFamilyMembers";
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
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetPartnerShortNameLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetFamilyIdDBName()
        {
            return "p_family_id_i";
        }

        /// get character length for column
        public static short GetFamilyIdLength()
        {
            return -1;
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
        public static string GetTypeCodeModifyDBName()
        {
            return "TypeCodeModify";
        }

        /// get character length for column
        public static short GetTypeCodeModifyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetTypeCodePresentDBName()
        {
            return "TypeCodePresent";
        }

        /// get character length for column
        public static short GetTypeCodePresentLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetOtherTypeCodesDBName()
        {
            return "OtherTypeCodes";
        }

        /// get character length for column
        public static short GetOtherTypeCodesLength()
        {
            return -1;
        }

    }

    ///
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
        /// ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
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

        ///
        public bool TypeCodeModify
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTypeCodeModify)
                            || (((bool)(this[this.myTable.ColumnTypeCodeModify])) != value)))
                {
                    this[this.myTable.ColumnTypeCodeModify] = value;
                }
            }
        }

        ///
        public bool TypeCodePresent
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTypeCodePresent)
                            || (((bool)(this[this.myTable.ColumnTypeCodePresent])) != value)))
                {
                    this[this.myTable.ColumnTypeCodePresent] = value;
                }
            }
        }

        ///
        public string OtherTypeCodes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOtherTypeCodes.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOtherTypeCodes)
                            || (((string)(this[this.myTable.ColumnOtherTypeCodes])) != value)))
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

    ///
    [Serializable()]
    public class PartnerEditTDSFamilyMembersInfoForStatusChangeTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5105;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "FamilyMembersInfoForStatusChange", "PartnerEditTDSFamilyMembersInfoForStatusChange",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true)
                },
                new int[] {
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
        }

        /// Access a typed row by index
        public PartnerEditTDSFamilyMembersInfoForStatusChangeRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSFamilyMembersInfoForStatusChangeRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerEditTDSFamilyMembersInfoForStatusChangeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSFamilyMembersInfoForStatusChangeRow(builder);
        }

        /// get typed set of changes
        public PartnerEditTDSFamilyMembersInfoForStatusChangeTable GetChangesTyped()
        {
            return ((PartnerEditTDSFamilyMembersInfoForStatusChangeTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "FamilyMembersInfoForStatusChange";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerEditTDSFamilyMembersInfoForStatusChange";
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

    }

    ///
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
    }

    ///
    [Serializable()]
    public class PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5106;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnTypeCodeId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddTypeCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnRemoveTypeCodeId = 3;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PartnerTypeChangeFamilyMembersPromotion", "PartnerEditTDSPartnerTypeChangeFamilyMembersPromotion",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "TypeCode", "TypeCode", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(2, "AddTypeCode", "AddTypeCode", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "RemoveTypeCode", "RemoveTypeCode", "", OdbcType.Bit, -1, false)
                },
                new int[] {
                    0, 1, 2
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnTypeCode;
        ///
        public DataColumn ColumnAddTypeCode;
        ///
        public DataColumn ColumnRemoveTypeCode;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("TypeCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("AddTypeCode", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("RemoveTypeCode", typeof(bool)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnTypeCode = this.Columns["TypeCode"];
            this.ColumnAddTypeCode = this.Columns["AddTypeCode"];
            this.ColumnRemoveTypeCode = this.Columns["RemoveTypeCode"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnPartnerKey,ColumnTypeCode,ColumnAddTypeCode};
        }

        /// Access a typed row by index
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow this[int i]
        {
            get
            {
                return ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionRow(builder);
        }

        /// get typed set of changes
        public PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable GetChangesTyped()
        {
            return ((PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PartnerTypeChangeFamilyMembersPromotion";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerEditTDSPartnerTypeChangeFamilyMembersPromotion";
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
        public static string GetTypeCodeDBName()
        {
            return "TypeCode";
        }

        /// get character length for column
        public static short GetTypeCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddTypeCodeDBName()
        {
            return "AddTypeCode";
        }

        /// get character length for column
        public static short GetAddTypeCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRemoveTypeCodeDBName()
        {
            return "RemoveTypeCode";
        }

        /// get character length for column
        public static short GetRemoveTypeCodeLength()
        {
            return -1;
        }

    }

    ///
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
        public string TypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTypeCode)
                            || (((string)(this[this.myTable.ColumnTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTypeCode] = value;
                }
            }
        }

        ///
        public bool AddTypeCode
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAddTypeCode)
                            || (((bool)(this[this.myTable.ColumnAddTypeCode])) != value)))
                {
                    this[this.myTable.ColumnAddTypeCode] = value;
                }
            }
        }

        ///
        public bool RemoveTypeCode
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnRemoveTypeCode)
                            || (((bool)(this[this.myTable.ColumnRemoveTypeCode])) != value)))
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
        }
    }

    /// Address and other data related to that address.
    [Serializable()]
    public class PartnerAddressAggregateTDSSimilarLocationParametersTable : PLocationTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 40;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyOfSimilarLocationId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyOfSimilarLocationId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnUsedByNOtherPartnersId = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnswerReuseId = 26;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnswerProcessedClientSideId = 27;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnswerProcessedServerSideId = 28;

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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_building_1_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_building_2_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_street_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_locality_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_suburb_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_city_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_county_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_postal_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_3_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_latitude_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_longitude_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_km_x_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_km_y_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_accuracy_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("SiteKeyOfSimilarLocation", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("LocationKeyOfSimilarLocation", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("UsedByNOtherPartners", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("AnswerReuse", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedClientSide", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedServerSide", typeof(bool)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnBuilding1 = this.Columns["p_building_1_c"];
            this.ColumnBuilding2 = this.Columns["p_building_2_c"];
            this.ColumnStreetName = this.Columns["p_street_name_c"];
            this.ColumnLocality = this.Columns["p_locality_c"];
            this.ColumnSuburb = this.Columns["p_suburb_c"];
            this.ColumnCity = this.Columns["p_city_c"];
            this.ColumnCounty = this.Columns["p_county_c"];
            this.ColumnPostalCode = this.Columns["p_postal_code_c"];
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnAddress3 = this.Columns["p_address_3_c"];
            this.ColumnGeoLatitude = this.Columns["p_geo_latitude_n"];
            this.ColumnGeoLongitude = this.Columns["p_geo_longitude_n"];
            this.ColumnGeoKmX = this.Columns["p_geo_km_x_i"];
            this.ColumnGeoKmY = this.Columns["p_geo_km_y_i"];
            this.ColumnGeoAccuracy = this.Columns["p_geo_accuracy_i"];
            this.ColumnRestricted = this.Columns["p_restricted_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnSiteKeyOfSimilarLocation = this.Columns["SiteKeyOfSimilarLocation"];
            this.ColumnLocationKeyOfSimilarLocation = this.Columns["LocationKeyOfSimilarLocation"];
            this.ColumnUsedByNOtherPartners = this.Columns["UsedByNOtherPartners"];
            this.ColumnAnswerReuse = this.Columns["AnswerReuse"];
            this.ColumnAnswerProcessedClientSide = this.Columns["AnswerProcessedClientSide"];
            this.ColumnAnswerProcessedServerSide = this.Columns["AnswerProcessedServerSide"];
            this.PrimaryKey = new System.Data.DataColumn[2] {
                    ColumnSiteKey,ColumnLocationKey};
        }

        /// Access a typed row by index
        public new PartnerAddressAggregateTDSSimilarLocationParametersRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSSimilarLocationParametersRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public new PartnerAddressAggregateTDSSimilarLocationParametersRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSSimilarLocationParametersRow(builder);
        }

        /// get typed set of changes
        public new PartnerAddressAggregateTDSSimilarLocationParametersTable GetChangesTyped()
        {
            return ((PartnerAddressAggregateTDSSimilarLocationParametersTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "SimilarLocationParameters";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_location";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetSiteKeyOfSimilarLocationDBName()
        {
            return "SiteKeyOfSimilarLocation";
        }

        /// get character length for column
        public static short GetSiteKeyOfSimilarLocationLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyOfSimilarLocationDBName()
        {
            return "LocationKeyOfSimilarLocation";
        }

        /// get character length for column
        public static short GetLocationKeyOfSimilarLocationLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUsedByNOtherPartnersDBName()
        {
            return "UsedByNOtherPartners";
        }

        /// get character length for column
        public static short GetUsedByNOtherPartnersLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnswerReuseDBName()
        {
            return "AnswerReuse";
        }

        /// get character length for column
        public static short GetAnswerReuseLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedClientSideDBName()
        {
            return "AnswerProcessedClientSide";
        }

        /// get character length for column
        public static short GetAnswerProcessedClientSideLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedServerSideDBName()
        {
            return "AnswerProcessedServerSide";
        }

        /// get character length for column
        public static short GetAnswerProcessedServerSideLength()
        {
            return -1;
        }

    }

    /// Address and other data related to that address.
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
        public bool AnswerReuse
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnswerReuse)
                            || (((bool)(this[this.myTable.ColumnAnswerReuse])) != value)))
                {
                    this[this.myTable.ColumnAnswerReuse] = value;
                }
            }
        }

        ///
        public bool AnswerProcessedClientSide
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedClientSide)
                            || (((bool)(this[this.myTable.ColumnAnswerProcessedClientSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedClientSide] = value;
                }
            }
        }

        ///
        public bool AnswerProcessedServerSide
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedServerSide)
                            || (((bool)(this[this.myTable.ColumnAnswerProcessedServerSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedServerSide] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBuilding1);
            this.SetNull(this.myTable.ColumnBuilding2);
            this.SetNull(this.myTable.ColumnStreetName);
            this.SetNull(this.myTable.ColumnLocality);
            this.SetNull(this.myTable.ColumnSuburb);
            this.SetNull(this.myTable.ColumnCity);
            this.SetNull(this.myTable.ColumnCounty);
            this.SetNull(this.myTable.ColumnPostalCode);
            this.SetNull(this.myTable.ColumnCountryCode);
            this.SetNull(this.myTable.ColumnAddress3);
            this.SetNull(this.myTable.ColumnGeoLatitude);
            this.SetNull(this.myTable.ColumnGeoLongitude);
            this.SetNull(this.myTable.ColumnGeoKmX);
            this.SetNull(this.myTable.ColumnGeoKmY);
            this[this.myTable.ColumnGeoAccuracy.Ordinal] = -1;
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
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

    ///
    [Serializable()]
    public class PartnerAddressAggregateTDSChangePromotionParametersTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5108;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerShortNameId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerClassId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnTelephoneNumberId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnExtensionId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnFaxNumberId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnFaxExtensionId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnAlternateTelephoneId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnMobileNumberId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailAddressId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnUrlId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnSendMailId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEffectiveId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateGoodUntilId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationTypeId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyOfEditedRecordId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyOfEditedRecordId = 18;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ChangePromotionParameters", "PartnerAddressAggregateTDSChangePromotionParameters",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "SiteKey", "p_site_key_n", "Site Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(2, "LocationKey", "p_location_key_i", "Location Key", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "PartnerShortName", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(4, "PartnerClass", "p_partner_class_c", "Partner Class", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(5, "TelephoneNumber", "p_telephone_number_c", "Phone", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(6, "Extension", "p_extension_i", "Phone Extension", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "FaxNumber", "p_fax_number_c", "Fax", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(8, "FaxExtension", "p_fax_extension_i", "Fax Extension", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(9, "AlternateTelephone", "p_alternate_telephone_c", "Alternate", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(10, "MobileNumber", "p_mobile_number_c", "Mobile", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(11, "EmailAddress", "p_email_address_c", "Email", OdbcType.VarChar, 120, false),
                    new TTypedColumnInfo(12, "Url", "p_url_c", "Website", OdbcType.VarChar, 128, false),
                    new TTypedColumnInfo(13, "SendMail", "p_send_mail_l", "Mailing Address", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "DateEffective", "p_date_effective_d", "Valid From", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "DateGoodUntil", "p_date_good_until_d", "Valid To", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(16, "LocationType", "p_location_type_c", "Location Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(17, "SiteKeyOfEditedRecord", "SiteKeyOfEditedRecord", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(18, "LocationKeyOfEditedRecord", "LocationKeyOfEditedRecord", "", OdbcType.Int, -1, false)
                },
                new int[] {
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PartnerAddressAggregateTDSChangePromotionParametersRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSChangePromotionParametersRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerAddressAggregateTDSChangePromotionParametersRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSChangePromotionParametersRow(builder);
        }

        /// get typed set of changes
        public PartnerAddressAggregateTDSChangePromotionParametersTable GetChangesTyped()
        {
            return ((PartnerAddressAggregateTDSChangePromotionParametersTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "ChangePromotionParameters";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerAddressAggregateTDSChangePromotionParameters";
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
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }

        /// get character length for column
        public static short GetSiteKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }

        /// get character length for column
        public static short GetLocationKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetPartnerShortNameLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "p_partner_class_c";
        }

        /// get character length for column
        public static short GetPartnerClassLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetTelephoneNumberDBName()
        {
            return "p_telephone_number_c";
        }

        /// get character length for column
        public static short GetTelephoneNumberLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetExtensionDBName()
        {
            return "p_extension_i";
        }

        /// get character length for column
        public static short GetExtensionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetFaxNumberDBName()
        {
            return "p_fax_number_c";
        }

        /// get character length for column
        public static short GetFaxNumberLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetFaxExtensionDBName()
        {
            return "p_fax_extension_i";
        }

        /// get character length for column
        public static short GetFaxExtensionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAlternateTelephoneDBName()
        {
            return "p_alternate_telephone_c";
        }

        /// get character length for column
        public static short GetAlternateTelephoneLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetMobileNumberDBName()
        {
            return "p_mobile_number_c";
        }

        /// get character length for column
        public static short GetMobileNumberLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetEmailAddressDBName()
        {
            return "p_email_address_c";
        }

        /// get character length for column
        public static short GetEmailAddressLength()
        {
            return 120;
        }

        /// get the name of the field in the database for this column
        public static string GetUrlDBName()
        {
            return "p_url_c";
        }

        /// get character length for column
        public static short GetUrlLength()
        {
            return 128;
        }

        /// get the name of the field in the database for this column
        public static string GetSendMailDBName()
        {
            return "p_send_mail_l";
        }

        /// get character length for column
        public static short GetSendMailLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateEffectiveDBName()
        {
            return "p_date_effective_d";
        }

        /// get character length for column
        public static short GetDateEffectiveLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateGoodUntilDBName()
        {
            return "p_date_good_until_d";
        }

        /// get character length for column
        public static short GetDateGoodUntilLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationTypeDBName()
        {
            return "p_location_type_c";
        }

        /// get character length for column
        public static short GetLocationTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetSiteKeyOfEditedRecordDBName()
        {
            return "SiteKeyOfEditedRecord";
        }

        /// get character length for column
        public static short GetSiteKeyOfEditedRecordLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyOfEditedRecordDBName()
        {
            return "LocationKeyOfEditedRecord";
        }

        /// get character length for column
        public static short GetLocationKeyOfEditedRecordLength()
        {
            return -1;
        }

    }

    ///
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
        public System.DateTime? DateEffective
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateEffective.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateEffective)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateEffective])) != value)))
                {
                    this[this.myTable.ColumnDateEffective] = value;
                }
            }
        }

        ///
        public System.DateTime? DateGoodUntil
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateGoodUntil.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateGoodUntil)
                            || (((System.DateTime?)(this[this.myTable.ColumnDateGoodUntil])) != value)))
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
        public bool IsSiteKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKey);
        }

        /// assign NULL value
        public void SetSiteKeyNull()
        {
            this.SetNull(this.myTable.ColumnSiteKey);
        }

        /// test for NULL value
        public bool IsLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKey);
        }

        /// assign NULL value
        public void SetLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnLocationKey);
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

    ///
    [Serializable()]
    public class PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5109;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationChangeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerLocationChangeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationAddedId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnChangedFieldsId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnUserAnswerId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnswerProcessedClientSideId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnAnswerProcessedServerSideId = 9;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AddressAddedOrChangedPromotion", "PartnerAddressAggregateTDSAddressAddedOrChangedPromotion",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "SiteKey", "p_site_key_n", "Site Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "LocationKey", "p_location_key_i", "Location Key", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(3, "LocationChange", "LocationChange", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "PartnerLocationChange", "PartnerLocationChange", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(5, "LocationAdded", "LocationAdded", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "ChangedFields", "ChangedFields", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(7, "UserAnswer", "UserAnswer", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(8, "AnswerProcessedClientSide", "AnswerProcessedClientSide", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(9, "AnswerProcessedServerSide", "AnswerProcessedServerSide", "", OdbcType.Bit, -1, false)
                },
                new int[] {
                }));
            return true;
        }

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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("LocationChange", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("PartnerLocationChange", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("LocationAdded", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("ChangedFields", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("UserAnswer", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedClientSide", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("AnswerProcessedServerSide", typeof(bool)));
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

        /// Access a typed row by index
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow this[int i]
        {
            get
            {
                return ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow(builder);
        }

        /// get typed set of changes
        public PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable GetChangesTyped()
        {
            return ((PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "AddressAddedOrChangedPromotion";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerAddressAggregateTDSAddressAddedOrChangedPromotion";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }

        /// get character length for column
        public static short GetSiteKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }

        /// get character length for column
        public static short GetLocationKeyLength()
        {
            return -1;
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
        public static string GetLocationChangeDBName()
        {
            return "LocationChange";
        }

        /// get character length for column
        public static short GetLocationChangeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerLocationChangeDBName()
        {
            return "PartnerLocationChange";
        }

        /// get character length for column
        public static short GetPartnerLocationChangeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationAddedDBName()
        {
            return "LocationAdded";
        }

        /// get character length for column
        public static short GetLocationAddedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetChangedFieldsDBName()
        {
            return "ChangedFields";
        }

        /// get character length for column
        public static short GetChangedFieldsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetUserAnswerDBName()
        {
            return "UserAnswer";
        }

        /// get character length for column
        public static short GetUserAnswerLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedClientSideDBName()
        {
            return "AnswerProcessedClientSide";
        }

        /// get character length for column
        public static short GetAnswerProcessedClientSideLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAnswerProcessedServerSideDBName()
        {
            return "AnswerProcessedServerSide";
        }

        /// get character length for column
        public static short GetAnswerProcessedServerSideLength()
        {
            return -1;
        }

    }

    ///
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
        public bool LocationChange
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLocationChange)
                            || (((bool)(this[this.myTable.ColumnLocationChange])) != value)))
                {
                    this[this.myTable.ColumnLocationChange] = value;
                }
            }
        }

        ///
        public bool PartnerLocationChange
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPartnerLocationChange)
                            || (((bool)(this[this.myTable.ColumnPartnerLocationChange])) != value)))
                {
                    this[this.myTable.ColumnPartnerLocationChange] = value;
                }
            }
        }

        ///
        public bool LocationAdded
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLocationAdded)
                            || (((bool)(this[this.myTable.ColumnLocationAdded])) != value)))
                {
                    this[this.myTable.ColumnLocationAdded] = value;
                }
            }
        }

        ///
        public string ChangedFields
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChangedFields.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnChangedFields)
                            || (((string)(this[this.myTable.ColumnChangedFields])) != value)))
                {
                    this[this.myTable.ColumnChangedFields] = value;
                }
            }
        }

        ///
        public string UserAnswer
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUserAnswer.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUserAnswer)
                            || (((string)(this[this.myTable.ColumnUserAnswer])) != value)))
                {
                    this[this.myTable.ColumnUserAnswer] = value;
                }
            }
        }

        ///
        public bool AnswerProcessedClientSide
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedClientSide)
                            || (((bool)(this[this.myTable.ColumnAnswerProcessedClientSide])) != value)))
                {
                    this[this.myTable.ColumnAnswerProcessedClientSide] = value;
                }
            }
        }

        ///
        public bool AnswerProcessedServerSide
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAnswerProcessedServerSide)
                            || (((bool)(this[this.myTable.ColumnAnswerProcessedServerSide])) != value)))
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
        public bool IsSiteKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKey);
        }

        /// assign NULL value
        public void SetSiteKeyNull()
        {
            this.SetNull(this.myTable.ColumnSiteKey);
        }

        /// test for NULL value
        public bool IsLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKey);
        }

        /// assign NULL value
        public void SetLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnLocationKey);
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
                                "p_site_key_n", "p_location_key_i"}, "PPartnerLocation", new string[] {
                                "p_site_key_n", "p_location_key_i"}));
            }
        }
    }

    ///
    [Serializable()]
    public class PartnerInfoTDSPartnerHeadInfoTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5110;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerClassId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnStatusCodeId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnAcquisitionCodeId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnPrivatePartnerOwnerId = 5;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PartnerHeadInfo", "PartnerInfoTDSPartnerHeadInfo",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerShortName", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "PartnerClass", "p_partner_class_c", "Partner Class", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(3, "StatusCode", "p_status_code_c", "Partner Status", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(4, "AcquisitionCode", "p_acquisition_code_c", "Acquisition Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(5, "PrivatePartnerOwner", "p_user_id_c", "User ID", OdbcType.VarChar, 40, false)
                },
                new int[] {
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public DataColumn ColumnPartnerClass;
        /// This code describes the status of a partner.
        /// Eg,  Active, Deceased etc
        public DataColumn ColumnStatusCode;
        /// This code identifies the method of aquisition.
        public DataColumn ColumnAcquisitionCode;
        /// The Petra user that the partner record is restricted to if p_restricted_i is 2.
        public DataColumn ColumnPrivatePartnerOwner;

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

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnPartnerClass = this.Columns["p_partner_class_c"];
            this.ColumnStatusCode = this.Columns["p_status_code_c"];
            this.ColumnAcquisitionCode = this.Columns["p_acquisition_code_c"];
            this.ColumnPrivatePartnerOwner = this.Columns["p_user_id_c"];
        }

        /// Access a typed row by index
        public PartnerInfoTDSPartnerHeadInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSPartnerHeadInfoRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerInfoTDSPartnerHeadInfoRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSPartnerHeadInfoRow(builder);
        }

        /// get typed set of changes
        public PartnerInfoTDSPartnerHeadInfoTable GetChangesTyped()
        {
            return ((PartnerInfoTDSPartnerHeadInfoTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PartnerHeadInfo";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerInfoTDSPartnerHeadInfo";
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
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetPartnerShortNameLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "p_partner_class_c";
        }

        /// get character length for column
        public static short GetPartnerClassLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetStatusCodeDBName()
        {
            return "p_status_code_c";
        }

        /// get character length for column
        public static short GetStatusCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetAcquisitionCodeDBName()
        {
            return "p_acquisition_code_c";
        }

        /// get character length for column
        public static short GetAcquisitionCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetPrivatePartnerOwnerDBName()
        {
            return "p_user_id_c";
        }

        /// get character length for column
        public static short GetPrivatePartnerOwnerLength()
        {
            return 40;
        }

    }

    ///
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
        /// Eg,  Active, Deceased etc
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

    ///
    [Serializable()]
    public class PartnerInfoTDSPartnerAdditionalInfoTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5111;
        /// used for generic TTypedDataTable functions
        public static short ColumnMainLanguagesId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnAdditionalLanguagesId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnLastContactId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateCreatedId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateModifiedId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfBirthId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyKeyId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnPreviousNameId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnFieldId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnFieldKeyId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnNotesId = 11;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PartnerAdditionalInfo", "PartnerInfoTDSPartnerAdditionalInfo",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "MainLanguages", "MainLanguages", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(1, "AdditionalLanguages", "AdditionalLanguages", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(2, "LastContact", "s_contact_date_d", "Contact Date", OdbcType.Date, -1, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "DateOfBirth", "p_date_of_birth_d", "Date of Birth", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "Family", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(7, "FamilyKey", "p_family_key_n", "Partner Key", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(8, "PreviousName", "p_previous_name_c", "Previous Name", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(9, "Field", "p_unit_name_c", "Unit Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(10, "FieldKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(11, "Notes", "p_comment_c", "Comments", OdbcType.VarChar, 10000, false)
                },
                new int[] {
                }));
            return true;
        }

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
        /// It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
        public DataColumn ColumnFamilyKey;
        ///
        public DataColumn ColumnPreviousName;
        ///
        public DataColumn ColumnField;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnFieldKey;
        /// Additional information about the partner that is important to store in the database.
        public DataColumn ColumnNotes;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("MainLanguages", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("AdditionalLanguages", typeof(string)));
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
        }

        /// Access a typed row by index
        public PartnerInfoTDSPartnerAdditionalInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSPartnerAdditionalInfoRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerInfoTDSPartnerAdditionalInfoRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSPartnerAdditionalInfoRow(builder);
        }

        /// get typed set of changes
        public PartnerInfoTDSPartnerAdditionalInfoTable GetChangesTyped()
        {
            return ((PartnerInfoTDSPartnerAdditionalInfoTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "PartnerAdditionalInfo";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerInfoTDSPartnerAdditionalInfo";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMainLanguagesDBName()
        {
            return "MainLanguages";
        }

        /// get character length for column
        public static short GetMainLanguagesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAdditionalLanguagesDBName()
        {
            return "AdditionalLanguages";
        }

        /// get character length for column
        public static short GetAdditionalLanguagesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLastContactDBName()
        {
            return "s_contact_date_d";
        }

        /// get character length for column
        public static short GetLastContactLength()
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
        public static string GetFamilyDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetFamilyLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetFamilyKeyDBName()
        {
            return "p_family_key_n";
        }

        /// get character length for column
        public static short GetFamilyKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetPreviousNameDBName()
        {
            return "p_previous_name_c";
        }

        /// get character length for column
        public static short GetPreviousNameLength()
        {
            return 512;
        }

        /// get the name of the field in the database for this column
        public static string GetFieldDBName()
        {
            return "p_unit_name_c";
        }

        /// get character length for column
        public static short GetFieldLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetFieldKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetFieldKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetNotesDBName()
        {
            return "p_comment_c";
        }

        /// get character length for column
        public static short GetNotesLength()
        {
            return 10000;
        }

    }

    ///
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
        public string MainLanguages
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMainLanguages.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMainLanguages)
                            || (((string)(this[this.myTable.ColumnMainLanguages])) != value)))
                {
                    this[this.myTable.ColumnMainLanguages] = value;
                }
            }
        }

        ///
        public string AdditionalLanguages
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAdditionalLanguages.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAdditionalLanguages)
                            || (((string)(this[this.myTable.ColumnAdditionalLanguages])) != value)))
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
                    return DateTime.MinValue;
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
        /// It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
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
            this.SetNull(this.myTable.ColumnFieldKey);
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
        public bool IsLastContactNull()
        {
            return this.IsNull(this.myTable.ColumnLastContact);
        }

        /// assign NULL value
        public void SetLastContactNull()
        {
            this.SetNull(this.myTable.ColumnLastContact);
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
        public bool IsFieldKeyNull()
        {
            return this.IsNull(this.myTable.ColumnFieldKey);
        }

        /// assign NULL value
        public void SetFieldKeyNull()
        {
            this.SetNull(this.myTable.ColumnFieldKey);
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

    ///
    [Serializable()]
    public class PartnerInfoTDSUnitInfoTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5112;
        /// used for generic TTypedDataTable functions
        public static short ColumnParentUnitKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnParentUnitNameId = 1;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "UnitInfo", "PartnerInfoTDSUnitInfo",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "ParentUnitKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "ParentUnitName", "p_unit_name_c", "Unit Name", OdbcType.VarChar, 160, false)
                },
                new int[] {
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnParentUnitKey;
        ///
        public DataColumn ColumnParentUnitName;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_unit_name_c", typeof(String)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnParentUnitKey = this.Columns["p_partner_key_n"];
            this.ColumnParentUnitName = this.Columns["p_unit_name_c"];
        }

        /// Access a typed row by index
        public PartnerInfoTDSUnitInfoRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSUnitInfoRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerInfoTDSUnitInfoRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSUnitInfoRow(builder);
        }

        /// get typed set of changes
        public PartnerInfoTDSUnitInfoTable GetChangesTyped()
        {
            return ((PartnerInfoTDSUnitInfoTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "UnitInfo";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerInfoTDSUnitInfo";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetParentUnitKeyDBName()
        {
            return "p_partner_key_n";
        }

        /// get character length for column
        public static short GetParentUnitKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetParentUnitNameDBName()
        {
            return "p_unit_name_c";
        }

        /// get character length for column
        public static short GetParentUnitNameLength()
        {
            return 160;
        }

    }

    ///
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
            this.SetNull(this.myTable.ColumnParentUnitKey);
            this.SetNull(this.myTable.ColumnParentUnitName);
        }

        /// test for NULL value
        public bool IsParentUnitKeyNull()
        {
            return this.IsNull(this.myTable.ColumnParentUnitKey);
        }

        /// assign NULL value
        public void SetParentUnitKeyNull()
        {
            this.SetNull(this.myTable.ColumnParentUnitKey);
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

    ///
    [Serializable()]
    public class PartnerInfoTDSFamilyMembersTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5113;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyIdId = 2;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "FamilyMembers", "PartnerInfoTDSFamilyMembers",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(1, "PartnerShortName", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "FamilyId", "p_family_id_i", "Family ID", OdbcType.Int, -1, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        /// This field indicates the family id of the individual.
        /// ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
        public DataColumn ColumnFamilyId;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_id_i", typeof(Int32)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnFamilyId = this.Columns["p_family_id_i"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public PartnerInfoTDSFamilyMembersRow this[int i]
        {
            get
            {
                return ((PartnerInfoTDSFamilyMembersRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public PartnerInfoTDSFamilyMembersRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerInfoTDSFamilyMembersRow(builder);
        }

        /// get typed set of changes
        public PartnerInfoTDSFamilyMembersTable GetChangesTyped()
        {
            return ((PartnerInfoTDSFamilyMembersTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "FamilyMembers";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerInfoTDSFamilyMembers";
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
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetPartnerShortNameLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetFamilyIdDBName()
        {
            return "p_family_id_i";
        }

        /// get character length for column
        public static short GetFamilyIdLength()
        {
            return -1;
        }

    }

    ///
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
        /// ID's 0 and 1 are used for parents; 2, 3, 4 ... 9 are used for children.
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
    public class PartnerFindTDS : TTypedDataSet
    {

        private PartnerFindTDSSearchCriteriaTable TableSearchCriteria;
        private PartnerFindTDSSearchResultTable TableSearchResult;

        /// auto generated
        public PartnerFindTDS() :
                base("PartnerFindTDS")
        {
        }

        /// auto generated for serialization
        public PartnerFindTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public PartnerFindTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public PartnerFindTDSSearchCriteriaTable SearchCriteria
        {
            get
            {
                return this.TableSearchCriteria;
            }
        }

        /// auto generated
        public PartnerFindTDSSearchResultTable SearchResult
        {
            get
            {
                return this.TableSearchResult;
            }
        }

        /// auto generated
        public new virtual PartnerFindTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerFindTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PartnerFindTDSSearchCriteriaTable("SearchCriteria"));
            this.Tables.Add(new PartnerFindTDSSearchResultTable("SearchResult"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("SearchCriteria") != -1))
            {
                this.Tables.Add(new PartnerFindTDSSearchCriteriaTable("SearchCriteria"));
            }
            if ((ds.Tables.IndexOf("SearchResult") != -1))
            {
                this.Tables.Add(new PartnerFindTDSSearchResultTable("SearchResult"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableSearchCriteria != null))
            {
                this.TableSearchCriteria.InitVars();
            }
            if ((this.TableSearchResult != null))
            {
                this.TableSearchResult.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerFindTDS";
            this.TableSearchCriteria = ((PartnerFindTDSSearchCriteriaTable)(this.Tables["SearchCriteria"]));
            this.TableSearchResult = ((PartnerFindTDSSearchResultTable)(this.Tables["SearchResult"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
        }
    }

    ///
    [Serializable()]
    public class PartnerFindTDSSearchCriteriaTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5114;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerNameId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerNameMatchId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnPersonalNameId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPersonalNameMatchId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPreviousNameId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnPreviousNameMatchId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress1Id = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress1MatchId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnCityId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnCityMatchId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnPostCodeId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnPostCodeMatchId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountyId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountyMatchId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnMailingAddressOnlyId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerClassId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerStatusId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailMatchId = 20;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress2Id = 21;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress2MatchId = 22;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress3Id = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress3MatchId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyId = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnWorkerFamOnlyId = 26;
        /// used for generic TTypedDataTable functions
        public static short ColumnExactPartnerKeyMatchId = 27;
        /// used for generic TTypedDataTable functions
        public static short ColumnPhoneNumberId = 28;
        /// used for generic TTypedDataTable functions
        public static short ColumnPhoneNumberMatchId = 29;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "SearchCriteria", "PartnerFindTDSSearchCriteria",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerName", "PartnerName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(1, "PartnerNameMatch", "PartnerNameMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(2, "PersonalName", "PersonalName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(3, "PersonalNameMatch", "PersonalNameMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(4, "PreviousName", "PreviousName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(5, "PreviousNameMatch", "PreviousNameMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(6, "Address1", "Address1", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(7, "Address1Match", "Address1Match", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(8, "City", "City", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(9, "CityMatch", "CityMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(10, "PostCode", "PostCode", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(11, "PostCodeMatch", "PostCodeMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(12, "County", "County", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(13, "CountyMatch", "CountyMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(14, "Country", "Country", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(15, "MailingAddressOnly", "MailingAddressOnly", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(16, "PartnerClass", "PartnerClass", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(17, "PartnerKey", "PartnerKey", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(18, "PartnerStatus", "PartnerStatus", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(19, "Email", "Email", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(20, "EmailMatch", "EmailMatch", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(21, "Address2", "Address2", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(22, "Address2Match", "Address2Match", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(23, "Address3", "Address3", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(24, "Address3Match", "Address3Match", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(25, "LocationKey", "LocationKey", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(26, "WorkerFamOnly", "WorkerFamOnly", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(27, "ExactPartnerKeyMatch", "ExactPartnerKeyMatch", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(28, "PhoneNumber", "PhoneNumber", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(29, "PhoneNumberMatch", "PhoneNumberMatch", "", OdbcType.VarChar, -1, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public PartnerFindTDSSearchCriteriaTable() :
                base("SearchCriteria")
        {
        }

        /// constructor
        public PartnerFindTDSSearchCriteriaTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PartnerFindTDSSearchCriteriaTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnPartnerName;
        ///
        public DataColumn ColumnPartnerNameMatch;
        ///
        public DataColumn ColumnPersonalName;
        ///
        public DataColumn ColumnPersonalNameMatch;
        ///
        public DataColumn ColumnPreviousName;
        ///
        public DataColumn ColumnPreviousNameMatch;
        ///
        public DataColumn ColumnAddress1;
        ///
        public DataColumn ColumnAddress1Match;
        ///
        public DataColumn ColumnCity;
        ///
        public DataColumn ColumnCityMatch;
        ///
        public DataColumn ColumnPostCode;
        ///
        public DataColumn ColumnPostCodeMatch;
        ///
        public DataColumn ColumnCounty;
        ///
        public DataColumn ColumnCountyMatch;
        ///
        public DataColumn ColumnCountry;
        ///
        public DataColumn ColumnMailingAddressOnly;
        ///
        public DataColumn ColumnPartnerClass;
        ///
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnPartnerStatus;
        ///
        public DataColumn ColumnEmail;
        ///
        public DataColumn ColumnEmailMatch;
        ///
        public DataColumn ColumnAddress2;
        ///
        public DataColumn ColumnAddress2Match;
        ///
        public DataColumn ColumnAddress3;
        ///
        public DataColumn ColumnAddress3Match;
        ///
        public DataColumn ColumnLocationKey;
        ///
        public DataColumn ColumnWorkerFamOnly;
        ///
        public DataColumn ColumnExactPartnerKeyMatch;
        ///
        public DataColumn ColumnPhoneNumber;
        ///
        public DataColumn ColumnPhoneNumberMatch;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("PartnerName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PartnerNameMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PersonalName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PersonalNameMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PreviousName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PreviousNameMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address1", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address1Match", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("City", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("CityMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PostCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PostCodeMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("County", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("CountyMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Country", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("MailingAddressOnly", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("PartnerClass", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PartnerKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("PartnerStatus", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Email", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("EmailMatch", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address2", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address2Match", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address3", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address3Match", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("LocationKey", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("WorkerFamOnly", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("ExactPartnerKeyMatch", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("PhoneNumber", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PhoneNumberMatch", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerName = this.Columns["PartnerName"];
            this.ColumnPartnerNameMatch = this.Columns["PartnerNameMatch"];
            this.ColumnPersonalName = this.Columns["PersonalName"];
            this.ColumnPersonalNameMatch = this.Columns["PersonalNameMatch"];
            this.ColumnPreviousName = this.Columns["PreviousName"];
            this.ColumnPreviousNameMatch = this.Columns["PreviousNameMatch"];
            this.ColumnAddress1 = this.Columns["Address1"];
            this.ColumnAddress1Match = this.Columns["Address1Match"];
            this.ColumnCity = this.Columns["City"];
            this.ColumnCityMatch = this.Columns["CityMatch"];
            this.ColumnPostCode = this.Columns["PostCode"];
            this.ColumnPostCodeMatch = this.Columns["PostCodeMatch"];
            this.ColumnCounty = this.Columns["County"];
            this.ColumnCountyMatch = this.Columns["CountyMatch"];
            this.ColumnCountry = this.Columns["Country"];
            this.ColumnMailingAddressOnly = this.Columns["MailingAddressOnly"];
            this.ColumnPartnerClass = this.Columns["PartnerClass"];
            this.ColumnPartnerKey = this.Columns["PartnerKey"];
            this.ColumnPartnerStatus = this.Columns["PartnerStatus"];
            this.ColumnEmail = this.Columns["Email"];
            this.ColumnEmailMatch = this.Columns["EmailMatch"];
            this.ColumnAddress2 = this.Columns["Address2"];
            this.ColumnAddress2Match = this.Columns["Address2Match"];
            this.ColumnAddress3 = this.Columns["Address3"];
            this.ColumnAddress3Match = this.Columns["Address3Match"];
            this.ColumnLocationKey = this.Columns["LocationKey"];
            this.ColumnWorkerFamOnly = this.Columns["WorkerFamOnly"];
            this.ColumnExactPartnerKeyMatch = this.Columns["ExactPartnerKeyMatch"];
            this.ColumnPhoneNumber = this.Columns["PhoneNumber"];
            this.ColumnPhoneNumberMatch = this.Columns["PhoneNumberMatch"];
        }

        /// Access a typed row by index
        public PartnerFindTDSSearchCriteriaRow this[int i]
        {
            get
            {
                return ((PartnerFindTDSSearchCriteriaRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PartnerFindTDSSearchCriteriaRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerFindTDSSearchCriteriaRow ret = ((PartnerFindTDSSearchCriteriaRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PartnerFindTDSSearchCriteriaRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerFindTDSSearchCriteriaRow(builder);
        }

        /// get typed set of changes
        public PartnerFindTDSSearchCriteriaTable GetChangesTyped()
        {
            return ((PartnerFindTDSSearchCriteriaTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "SearchCriteria";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerFindTDSSearchCriteria";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerNameDBName()
        {
            return "PartnerName";
        }

        /// get character length for column
        public static short GetPartnerNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerNameMatchDBName()
        {
            return "PartnerNameMatch";
        }

        /// get character length for column
        public static short GetPartnerNameMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPersonalNameDBName()
        {
            return "PersonalName";
        }

        /// get character length for column
        public static short GetPersonalNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPersonalNameMatchDBName()
        {
            return "PersonalNameMatch";
        }

        /// get character length for column
        public static short GetPersonalNameMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPreviousNameDBName()
        {
            return "PreviousName";
        }

        /// get character length for column
        public static short GetPreviousNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPreviousNameMatchDBName()
        {
            return "PreviousNameMatch";
        }

        /// get character length for column
        public static short GetPreviousNameMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress1DBName()
        {
            return "Address1";
        }

        /// get character length for column
        public static short GetAddress1Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress1MatchDBName()
        {
            return "Address1Match";
        }

        /// get character length for column
        public static short GetAddress1MatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCityDBName()
        {
            return "City";
        }

        /// get character length for column
        public static short GetCityLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCityMatchDBName()
        {
            return "CityMatch";
        }

        /// get character length for column
        public static short GetCityMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPostCodeDBName()
        {
            return "PostCode";
        }

        /// get character length for column
        public static short GetPostCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPostCodeMatchDBName()
        {
            return "PostCodeMatch";
        }

        /// get character length for column
        public static short GetPostCodeMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountyDBName()
        {
            return "County";
        }

        /// get character length for column
        public static short GetCountyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountyMatchDBName()
        {
            return "CountyMatch";
        }

        /// get character length for column
        public static short GetCountyMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryDBName()
        {
            return "Country";
        }

        /// get character length for column
        public static short GetCountryLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMailingAddressOnlyDBName()
        {
            return "MailingAddressOnly";
        }

        /// get character length for column
        public static short GetMailingAddressOnlyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "PartnerClass";
        }

        /// get character length for column
        public static short GetPartnerClassLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "PartnerKey";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerStatusDBName()
        {
            return "PartnerStatus";
        }

        /// get character length for column
        public static short GetPartnerStatusLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEmailDBName()
        {
            return "Email";
        }

        /// get character length for column
        public static short GetEmailLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEmailMatchDBName()
        {
            return "EmailMatch";
        }

        /// get character length for column
        public static short GetEmailMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress2DBName()
        {
            return "Address2";
        }

        /// get character length for column
        public static short GetAddress2Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress2MatchDBName()
        {
            return "Address2Match";
        }

        /// get character length for column
        public static short GetAddress2MatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress3DBName()
        {
            return "Address3";
        }

        /// get character length for column
        public static short GetAddress3Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress3MatchDBName()
        {
            return "Address3Match";
        }

        /// get character length for column
        public static short GetAddress3MatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "LocationKey";
        }

        /// get character length for column
        public static short GetLocationKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetWorkerFamOnlyDBName()
        {
            return "WorkerFamOnly";
        }

        /// get character length for column
        public static short GetWorkerFamOnlyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetExactPartnerKeyMatchDBName()
        {
            return "ExactPartnerKeyMatch";
        }

        /// get character length for column
        public static short GetExactPartnerKeyMatchLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPhoneNumberDBName()
        {
            return "PhoneNumber";
        }

        /// get character length for column
        public static short GetPhoneNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPhoneNumberMatchDBName()
        {
            return "PhoneNumberMatch";
        }

        /// get character length for column
        public static short GetPhoneNumberMatchLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class PartnerFindTDSSearchCriteriaRow : System.Data.DataRow
    {
        private PartnerFindTDSSearchCriteriaTable myTable;

        /// Constructor
        public PartnerFindTDSSearchCriteriaRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PartnerFindTDSSearchCriteriaTable)(this.Table));
        }

        ///
        public string PartnerName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerName)
                            || (((string)(this[this.myTable.ColumnPartnerName])) != value)))
                {
                    this[this.myTable.ColumnPartnerName] = value;
                }
            }
        }

        ///
        public string PartnerNameMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerNameMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerNameMatch)
                            || (((string)(this[this.myTable.ColumnPartnerNameMatch])) != value)))
                {
                    this[this.myTable.ColumnPartnerNameMatch] = value;
                }
            }
        }

        ///
        public string PersonalName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPersonalName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPersonalName)
                            || (((string)(this[this.myTable.ColumnPersonalName])) != value)))
                {
                    this[this.myTable.ColumnPersonalName] = value;
                }
            }
        }

        ///
        public string PersonalNameMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPersonalNameMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPersonalNameMatch)
                            || (((string)(this[this.myTable.ColumnPersonalNameMatch])) != value)))
                {
                    this[this.myTable.ColumnPersonalNameMatch] = value;
                }
            }
        }

        ///
        public string PreviousName
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPreviousName)
                            || (((string)(this[this.myTable.ColumnPreviousName])) != value)))
                {
                    this[this.myTable.ColumnPreviousName] = value;
                }
            }
        }

        ///
        public string PreviousNameMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPreviousNameMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPreviousNameMatch)
                            || (((string)(this[this.myTable.ColumnPreviousNameMatch])) != value)))
                {
                    this[this.myTable.ColumnPreviousNameMatch] = value;
                }
            }
        }

        ///
        public string Address1
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress1.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress1)
                            || (((string)(this[this.myTable.ColumnAddress1])) != value)))
                {
                    this[this.myTable.ColumnAddress1] = value;
                }
            }
        }

        ///
        public string Address1Match
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress1Match.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress1Match)
                            || (((string)(this[this.myTable.ColumnAddress1Match])) != value)))
                {
                    this[this.myTable.ColumnAddress1Match] = value;
                }
            }
        }

        ///
        public string City
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCity.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCity)
                            || (((string)(this[this.myTable.ColumnCity])) != value)))
                {
                    this[this.myTable.ColumnCity] = value;
                }
            }
        }

        ///
        public string CityMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCityMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCityMatch)
                            || (((string)(this[this.myTable.ColumnCityMatch])) != value)))
                {
                    this[this.myTable.ColumnCityMatch] = value;
                }
            }
        }

        ///
        public string PostCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPostCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPostCode)
                            || (((string)(this[this.myTable.ColumnPostCode])) != value)))
                {
                    this[this.myTable.ColumnPostCode] = value;
                }
            }
        }

        ///
        public string PostCodeMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPostCodeMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPostCodeMatch)
                            || (((string)(this[this.myTable.ColumnPostCodeMatch])) != value)))
                {
                    this[this.myTable.ColumnPostCodeMatch] = value;
                }
            }
        }

        ///
        public string County
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCounty.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCounty)
                            || (((string)(this[this.myTable.ColumnCounty])) != value)))
                {
                    this[this.myTable.ColumnCounty] = value;
                }
            }
        }

        ///
        public string CountyMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountyMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountyMatch)
                            || (((string)(this[this.myTable.ColumnCountyMatch])) != value)))
                {
                    this[this.myTable.ColumnCountyMatch] = value;
                }
            }
        }

        ///
        public string Country
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountry.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountry)
                            || (((string)(this[this.myTable.ColumnCountry])) != value)))
                {
                    this[this.myTable.ColumnCountry] = value;
                }
            }
        }

        ///
        public bool MailingAddressOnly
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingAddressOnly.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingAddressOnly)
                            || (((bool)(this[this.myTable.ColumnMailingAddressOnly])) != value)))
                {
                    this[this.myTable.ColumnMailingAddressOnly] = value;
                }
            }
        }

        ///
        public string PartnerClass
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPartnerClass)
                            || (((string)(this[this.myTable.ColumnPartnerClass])) != value)))
                {
                    this[this.myTable.ColumnPartnerClass] = value;
                }
            }
        }

        ///
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
        public string PartnerStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerStatus)
                            || (((string)(this[this.myTable.ColumnPartnerStatus])) != value)))
                {
                    this[this.myTable.ColumnPartnerStatus] = value;
                }
            }
        }

        ///
        public string Email
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEmail.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEmail)
                            || (((string)(this[this.myTable.ColumnEmail])) != value)))
                {
                    this[this.myTable.ColumnEmail] = value;
                }
            }
        }

        ///
        public string EmailMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEmailMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEmailMatch)
                            || (((string)(this[this.myTable.ColumnEmailMatch])) != value)))
                {
                    this[this.myTable.ColumnEmailMatch] = value;
                }
            }
        }

        ///
        public string Address2
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress2.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress2)
                            || (((string)(this[this.myTable.ColumnAddress2])) != value)))
                {
                    this[this.myTable.ColumnAddress2] = value;
                }
            }
        }

        ///
        public string Address2Match
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress2Match.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress2Match)
                            || (((string)(this[this.myTable.ColumnAddress2Match])) != value)))
                {
                    this[this.myTable.ColumnAddress2Match] = value;
                }
            }
        }

        ///
        public string Address3
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress3.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress3)
                            || (((string)(this[this.myTable.ColumnAddress3])) != value)))
                {
                    this[this.myTable.ColumnAddress3] = value;
                }
            }
        }

        ///
        public string Address3Match
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress3Match.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress3Match)
                            || (((string)(this[this.myTable.ColumnAddress3Match])) != value)))
                {
                    this[this.myTable.ColumnAddress3Match] = value;
                }
            }
        }

        ///
        public string LocationKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKey)
                            || (((string)(this[this.myTable.ColumnLocationKey])) != value)))
                {
                    this[this.myTable.ColumnLocationKey] = value;
                }
            }
        }

        ///
        public bool WorkerFamOnly
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnWorkerFamOnly.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnWorkerFamOnly)
                            || (((bool)(this[this.myTable.ColumnWorkerFamOnly])) != value)))
                {
                    this[this.myTable.ColumnWorkerFamOnly] = value;
                }
            }
        }

        ///
        public bool ExactPartnerKeyMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExactPartnerKeyMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExactPartnerKeyMatch)
                            || (((bool)(this[this.myTable.ColumnExactPartnerKeyMatch])) != value)))
                {
                    this[this.myTable.ColumnExactPartnerKeyMatch] = value;
                }
            }
        }

        ///
        public string PhoneNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPhoneNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPhoneNumber)
                            || (((string)(this[this.myTable.ColumnPhoneNumber])) != value)))
                {
                    this[this.myTable.ColumnPhoneNumber] = value;
                }
            }
        }

        ///
        public string PhoneNumberMatch
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPhoneNumberMatch.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPhoneNumberMatch)
                            || (((string)(this[this.myTable.ColumnPhoneNumberMatch])) != value)))
                {
                    this[this.myTable.ColumnPhoneNumberMatch] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnPartnerName);
            this[this.myTable.ColumnPartnerNameMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnPersonalName);
            this[this.myTable.ColumnPersonalNameMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnPreviousName);
            this[this.myTable.ColumnPreviousNameMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnAddress1);
            this[this.myTable.ColumnAddress1Match.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnCity);
            this[this.myTable.ColumnCityMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnPostCode);
            this[this.myTable.ColumnPostCodeMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnCounty);
            this[this.myTable.ColumnCountyMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnCountry);
            this[this.myTable.ColumnMailingAddressOnly.Ordinal] = false;
            this.SetNull(this.myTable.ColumnPartnerClass);
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnPartnerStatus.Ordinal] = "ACTIVE";
            this.SetNull(this.myTable.ColumnEmail);
            this[this.myTable.ColumnEmailMatch.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnAddress2);
            this[this.myTable.ColumnAddress2Match.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnAddress3);
            this[this.myTable.ColumnAddress3Match.Ordinal] = "BEGINS";
            this.SetNull(this.myTable.ColumnLocationKey);
            this[this.myTable.ColumnWorkerFamOnly.Ordinal] = false;
            this[this.myTable.ColumnExactPartnerKeyMatch.Ordinal] = false;
            this.SetNull(this.myTable.ColumnPhoneNumber);
            this[this.myTable.ColumnPhoneNumberMatch.Ordinal] = "BEGINS";
        }

        /// test for NULL value
        public bool IsPartnerNameNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerName);
        }

        /// assign NULL value
        public void SetPartnerNameNull()
        {
            this.SetNull(this.myTable.ColumnPartnerName);
        }

        /// test for NULL value
        public bool IsPartnerNameMatchNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerNameMatch);
        }

        /// assign NULL value
        public void SetPartnerNameMatchNull()
        {
            this.SetNull(this.myTable.ColumnPartnerNameMatch);
        }

        /// test for NULL value
        public bool IsPersonalNameNull()
        {
            return this.IsNull(this.myTable.ColumnPersonalName);
        }

        /// assign NULL value
        public void SetPersonalNameNull()
        {
            this.SetNull(this.myTable.ColumnPersonalName);
        }

        /// test for NULL value
        public bool IsPersonalNameMatchNull()
        {
            return this.IsNull(this.myTable.ColumnPersonalNameMatch);
        }

        /// assign NULL value
        public void SetPersonalNameMatchNull()
        {
            this.SetNull(this.myTable.ColumnPersonalNameMatch);
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
        public bool IsPreviousNameMatchNull()
        {
            return this.IsNull(this.myTable.ColumnPreviousNameMatch);
        }

        /// assign NULL value
        public void SetPreviousNameMatchNull()
        {
            this.SetNull(this.myTable.ColumnPreviousNameMatch);
        }

        /// test for NULL value
        public bool IsAddress1Null()
        {
            return this.IsNull(this.myTable.ColumnAddress1);
        }

        /// assign NULL value
        public void SetAddress1Null()
        {
            this.SetNull(this.myTable.ColumnAddress1);
        }

        /// test for NULL value
        public bool IsAddress1MatchNull()
        {
            return this.IsNull(this.myTable.ColumnAddress1Match);
        }

        /// assign NULL value
        public void SetAddress1MatchNull()
        {
            this.SetNull(this.myTable.ColumnAddress1Match);
        }

        /// test for NULL value
        public bool IsCityNull()
        {
            return this.IsNull(this.myTable.ColumnCity);
        }

        /// assign NULL value
        public void SetCityNull()
        {
            this.SetNull(this.myTable.ColumnCity);
        }

        /// test for NULL value
        public bool IsCityMatchNull()
        {
            return this.IsNull(this.myTable.ColumnCityMatch);
        }

        /// assign NULL value
        public void SetCityMatchNull()
        {
            this.SetNull(this.myTable.ColumnCityMatch);
        }

        /// test for NULL value
        public bool IsPostCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPostCode);
        }

        /// assign NULL value
        public void SetPostCodeNull()
        {
            this.SetNull(this.myTable.ColumnPostCode);
        }

        /// test for NULL value
        public bool IsPostCodeMatchNull()
        {
            return this.IsNull(this.myTable.ColumnPostCodeMatch);
        }

        /// assign NULL value
        public void SetPostCodeMatchNull()
        {
            this.SetNull(this.myTable.ColumnPostCodeMatch);
        }

        /// test for NULL value
        public bool IsCountyNull()
        {
            return this.IsNull(this.myTable.ColumnCounty);
        }

        /// assign NULL value
        public void SetCountyNull()
        {
            this.SetNull(this.myTable.ColumnCounty);
        }

        /// test for NULL value
        public bool IsCountyMatchNull()
        {
            return this.IsNull(this.myTable.ColumnCountyMatch);
        }

        /// assign NULL value
        public void SetCountyMatchNull()
        {
            this.SetNull(this.myTable.ColumnCountyMatch);
        }

        /// test for NULL value
        public bool IsCountryNull()
        {
            return this.IsNull(this.myTable.ColumnCountry);
        }

        /// assign NULL value
        public void SetCountryNull()
        {
            this.SetNull(this.myTable.ColumnCountry);
        }

        /// test for NULL value
        public bool IsMailingAddressOnlyNull()
        {
            return this.IsNull(this.myTable.ColumnMailingAddressOnly);
        }

        /// assign NULL value
        public void SetMailingAddressOnlyNull()
        {
            this.SetNull(this.myTable.ColumnMailingAddressOnly);
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
        public bool IsPartnerStatusNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerStatus);
        }

        /// assign NULL value
        public void SetPartnerStatusNull()
        {
            this.SetNull(this.myTable.ColumnPartnerStatus);
        }

        /// test for NULL value
        public bool IsEmailNull()
        {
            return this.IsNull(this.myTable.ColumnEmail);
        }

        /// assign NULL value
        public void SetEmailNull()
        {
            this.SetNull(this.myTable.ColumnEmail);
        }

        /// test for NULL value
        public bool IsEmailMatchNull()
        {
            return this.IsNull(this.myTable.ColumnEmailMatch);
        }

        /// assign NULL value
        public void SetEmailMatchNull()
        {
            this.SetNull(this.myTable.ColumnEmailMatch);
        }

        /// test for NULL value
        public bool IsAddress2Null()
        {
            return this.IsNull(this.myTable.ColumnAddress2);
        }

        /// assign NULL value
        public void SetAddress2Null()
        {
            this.SetNull(this.myTable.ColumnAddress2);
        }

        /// test for NULL value
        public bool IsAddress2MatchNull()
        {
            return this.IsNull(this.myTable.ColumnAddress2Match);
        }

        /// assign NULL value
        public void SetAddress2MatchNull()
        {
            this.SetNull(this.myTable.ColumnAddress2Match);
        }

        /// test for NULL value
        public bool IsAddress3Null()
        {
            return this.IsNull(this.myTable.ColumnAddress3);
        }

        /// assign NULL value
        public void SetAddress3Null()
        {
            this.SetNull(this.myTable.ColumnAddress3);
        }

        /// test for NULL value
        public bool IsAddress3MatchNull()
        {
            return this.IsNull(this.myTable.ColumnAddress3Match);
        }

        /// assign NULL value
        public void SetAddress3MatchNull()
        {
            this.SetNull(this.myTable.ColumnAddress3Match);
        }

        /// test for NULL value
        public bool IsLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKey);
        }

        /// assign NULL value
        public void SetLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnLocationKey);
        }

        /// test for NULL value
        public bool IsWorkerFamOnlyNull()
        {
            return this.IsNull(this.myTable.ColumnWorkerFamOnly);
        }

        /// assign NULL value
        public void SetWorkerFamOnlyNull()
        {
            this.SetNull(this.myTable.ColumnWorkerFamOnly);
        }

        /// test for NULL value
        public bool IsExactPartnerKeyMatchNull()
        {
            return this.IsNull(this.myTable.ColumnExactPartnerKeyMatch);
        }

        /// assign NULL value
        public void SetExactPartnerKeyMatchNull()
        {
            this.SetNull(this.myTable.ColumnExactPartnerKeyMatch);
        }

        /// test for NULL value
        public bool IsPhoneNumberNull()
        {
            return this.IsNull(this.myTable.ColumnPhoneNumber);
        }

        /// assign NULL value
        public void SetPhoneNumberNull()
        {
            this.SetNull(this.myTable.ColumnPhoneNumber);
        }

        /// test for NULL value
        public bool IsPhoneNumberMatchNull()
        {
            return this.IsNull(this.myTable.ColumnPhoneNumberMatch);
        }

        /// assign NULL value
        public void SetPhoneNumberMatchNull()
        {
            this.SetNull(this.myTable.ColumnPhoneNumberMatch);
        }
    }

    ///
    [Serializable()]
    public class PartnerFindTDSSearchResultTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5115;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerClassId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnStatusCodeId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnPreviousNameId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnAcquisitionCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnFirstNameId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyNameId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnFamilyKeyId = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnTelephoneNumberId = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationTypeId = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailAddressId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnSiteKeyId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnCityId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnPostalCodeId = 14;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocalityId = 15;
        /// used for generic TTypedDataTable functions
        public static short ColumnStreetNameId = 16;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress3Id = 17;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountyId = 18;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryCodeId = 19;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocationKeyId = 20;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "SearchResult", "PartnerFindTDSSearchResult",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "PartnerClass", "p_partner_class_c", "Partner Class", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(1, "PartnerShortName", "p_partner_short_name_c", "Short Name", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(2, "StatusCode", "p_status_code_c", "Partner Status", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(3, "PartnerKey", "p_partner_key_n", "Partner Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(4, "PreviousName", "p_previous_name_c", "Previous Name", OdbcType.VarChar, 512, false),
                    new TTypedColumnInfo(5, "AcquisitionCode", "p_acquisition_code_c", "Acquisition Code", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "FirstName", "p_first_name_c", "First Name", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(7, "FamilyName", "p_family_name_c", "Family Name", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(8, "FamilyKey", "p_family_key_n", "Partner Key", OdbcType.Decimal, 10, false),
                    new TTypedColumnInfo(9, "TelephoneNumber", "p_telephone_number_c", "Phone", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(10, "LocationType", "p_location_type_c", "Location Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(11, "EmailAddress", "p_email_address_c", "Email", OdbcType.VarChar, 120, false),
                    new TTypedColumnInfo(12, "SiteKey", "p_site_key_n", "Site Key", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(13, "City", "p_city_c", "City", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(14, "PostalCode", "p_postal_code_c", "Post Code", OdbcType.VarChar, 40, false),
                    new TTypedColumnInfo(15, "Locality", "p_locality_c", "Addr1", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(16, "StreetName", "p_street_name_c", "Addr2", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(17, "Address3", "p_address_3_c", "Addr3", OdbcType.VarChar, 100, false),
                    new TTypedColumnInfo(18, "County", "p_county_c", "Province", OdbcType.VarChar, 64, false),
                    new TTypedColumnInfo(19, "CountryCode", "p_country_code_c", "Country Code", OdbcType.VarChar, 8, false),
                    new TTypedColumnInfo(20, "LocationKey", "p_location_key_i", "Location Key", OdbcType.Int, -1, true)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public PartnerFindTDSSearchResultTable() :
                base("SearchResult")
        {
        }

        /// constructor
        public PartnerFindTDSSearchResultTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public PartnerFindTDSSearchResultTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This defines what type of partner this is. The classes that may be assigned are PERSON, FAMILY, CHURCH, ORGANISATION, UNIT, VENUE and BANK.
        public DataColumn ColumnPartnerClass;
        /// Name of the person or organisation.  If a person, more name info is stored in p_person.
        public DataColumn ColumnPartnerShortName;
        /// This code describes the status of a partner.
        /// Eg,  Active, Deceased etc
        public DataColumn ColumnStatusCode;
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnPreviousName;
        /// This code identifies the method of aquisition.
        public DataColumn ColumnAcquisitionCode;
        ///
        public DataColumn ColumnFirstName;
        ///
        public DataColumn ColumnFamilyName;
        /// A cross reference to the family record of this person.
        /// It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
        public DataColumn ColumnFamilyKey;
        ///
        public DataColumn ColumnTelephoneNumber;
        ///
        public DataColumn ColumnLocationType;
        ///
        public DataColumn ColumnEmailAddress;
        /// This is the key that tell what site created the linked location
        public DataColumn ColumnSiteKey;
        /// This can be a post town or city
        public DataColumn ColumnCity;
        /// This is the national post code
        public DataColumn ColumnPostalCode;
        /// This is the first element of an address
        public DataColumn ColumnLocality;
        /// The name of the street that the house is located on
        public DataColumn ColumnStreetName;
        /// This is the third element of an address (if required)
        public DataColumn ColumnAddress3;
        /// This can be a county (UK), a state (US), province (CDN), canton (CH) etc.
        public DataColumn ColumnCounty;
        /// This is a code which identifies a country.
        /// It is taken from the ISO 3166-1-alpha-2 code elements.
        public DataColumn ColumnCountryCode;
        ///
        public DataColumn ColumnLocationKey;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_class_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_status_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_previous_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_acquisition_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_family_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_telephone_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_location_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_city_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_postal_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_locality_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_street_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_3_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_county_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerClass = this.Columns["p_partner_class_c"];
            this.ColumnPartnerShortName = this.Columns["p_partner_short_name_c"];
            this.ColumnStatusCode = this.Columns["p_status_code_c"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPreviousName = this.Columns["p_previous_name_c"];
            this.ColumnAcquisitionCode = this.Columns["p_acquisition_code_c"];
            this.ColumnFirstName = this.Columns["p_first_name_c"];
            this.ColumnFamilyName = this.Columns["p_family_name_c"];
            this.ColumnFamilyKey = this.Columns["p_family_key_n"];
            this.ColumnTelephoneNumber = this.Columns["p_telephone_number_c"];
            this.ColumnLocationType = this.Columns["p_location_type_c"];
            this.ColumnEmailAddress = this.Columns["p_email_address_c"];
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnCity = this.Columns["p_city_c"];
            this.ColumnPostalCode = this.Columns["p_postal_code_c"];
            this.ColumnLocality = this.Columns["p_locality_c"];
            this.ColumnStreetName = this.Columns["p_street_name_c"];
            this.ColumnAddress3 = this.Columns["p_address_3_c"];
            this.ColumnCounty = this.Columns["p_county_c"];
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
        }

        /// Access a typed row by index
        public PartnerFindTDSSearchResultRow this[int i]
        {
            get
            {
                return ((PartnerFindTDSSearchResultRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public PartnerFindTDSSearchResultRow NewRowTyped(bool AWithDefaultValues)
        {
            PartnerFindTDSSearchResultRow ret = ((PartnerFindTDSSearchResultRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public PartnerFindTDSSearchResultRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PartnerFindTDSSearchResultRow(builder);
        }

        /// get typed set of changes
        public PartnerFindTDSSearchResultTable GetChangesTyped()
        {
            return ((PartnerFindTDSSearchResultTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "SearchResult";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "PartnerFindTDSSearchResult";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerClassDBName()
        {
            return "p_partner_class_c";
        }

        /// get character length for column
        public static short GetPartnerClassLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerShortNameDBName()
        {
            return "p_partner_short_name_c";
        }

        /// get character length for column
        public static short GetPartnerShortNameLength()
        {
            return 160;
        }

        /// get the name of the field in the database for this column
        public static string GetStatusCodeDBName()
        {
            return "p_status_code_c";
        }

        /// get character length for column
        public static short GetStatusCodeLength()
        {
            return 16;
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
        public static string GetPreviousNameDBName()
        {
            return "p_previous_name_c";
        }

        /// get character length for column
        public static short GetPreviousNameLength()
        {
            return 512;
        }

        /// get the name of the field in the database for this column
        public static string GetAcquisitionCodeDBName()
        {
            return "p_acquisition_code_c";
        }

        /// get character length for column
        public static short GetAcquisitionCodeLength()
        {
            return 16;
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
        public static string GetFamilyKeyDBName()
        {
            return "p_family_key_n";
        }

        /// get character length for column
        public static short GetFamilyKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetTelephoneNumberDBName()
        {
            return "p_telephone_number_c";
        }

        /// get character length for column
        public static short GetTelephoneNumberLength()
        {
            return 50;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationTypeDBName()
        {
            return "p_location_type_c";
        }

        /// get character length for column
        public static short GetLocationTypeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetEmailAddressDBName()
        {
            return "p_email_address_c";
        }

        /// get character length for column
        public static short GetEmailAddressLength()
        {
            return 120;
        }

        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }

        /// get character length for column
        public static short GetSiteKeyLength()
        {
            return 10;
        }

        /// get the name of the field in the database for this column
        public static string GetCityDBName()
        {
            return "p_city_c";
        }

        /// get character length for column
        public static short GetCityLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetPostalCodeDBName()
        {
            return "p_postal_code_c";
        }

        /// get character length for column
        public static short GetPostalCodeLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetLocalityDBName()
        {
            return "p_locality_c";
        }

        /// get character length for column
        public static short GetLocalityLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetStreetNameDBName()
        {
            return "p_street_name_c";
        }

        /// get character length for column
        public static short GetStreetNameLength()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress3DBName()
        {
            return "p_address_3_c";
        }

        /// get character length for column
        public static short GetAddress3Length()
        {
            return 100;
        }

        /// get the name of the field in the database for this column
        public static string GetCountyDBName()
        {
            return "p_county_c";
        }

        /// get character length for column
        public static short GetCountyLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }

        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }

        /// get character length for column
        public static short GetLocationKeyLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class PartnerFindTDSSearchResultRow : System.Data.DataRow
    {
        private PartnerFindTDSSearchResultTable myTable;

        /// Constructor
        public PartnerFindTDSSearchResultRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((PartnerFindTDSSearchResultTable)(this.Table));
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

        /// This code describes the status of a partner.
        /// Eg,  Active, Deceased etc
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

        /// A cross reference to the family record of this person.
        /// It should be set to ? (not 0 because such a record does not exist!) when there is no family record.
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

        /// This can be a post town or city
        public String City
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCity.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCity)
                            || (((String)(this[this.myTable.ColumnCity])) != value)))
                {
                    this[this.myTable.ColumnCity] = value;
                }
            }
        }

        /// This is the national post code
        public String PostalCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPostalCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPostalCode)
                            || (((String)(this[this.myTable.ColumnPostalCode])) != value)))
                {
                    this[this.myTable.ColumnPostalCode] = value;
                }
            }
        }

        /// This is the first element of an address
        public String Locality
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocality.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocality)
                            || (((String)(this[this.myTable.ColumnLocality])) != value)))
                {
                    this[this.myTable.ColumnLocality] = value;
                }
            }
        }

        /// The name of the street that the house is located on
        public String StreetName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStreetName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStreetName)
                            || (((String)(this[this.myTable.ColumnStreetName])) != value)))
                {
                    this[this.myTable.ColumnStreetName] = value;
                }
            }
        }

        /// This is the third element of an address (if required)
        public String Address3
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress3.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress3)
                            || (((String)(this[this.myTable.ColumnAddress3])) != value)))
                {
                    this[this.myTable.ColumnAddress3] = value;
                }
            }
        }

        /// This can be a county (UK), a state (US), province (CDN), canton (CH) etc.
        public String County
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCounty.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCounty)
                            || (((String)(this[this.myTable.ColumnCounty])) != value)))
                {
                    this[this.myTable.ColumnCounty] = value;
                }
            }
        }

        /// This is a code which identifies a country.
        /// It is taken from the ISO 3166-1-alpha-2 code elements.
        public String CountryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryCode)
                            || (((String)(this[this.myTable.ColumnCountryCode])) != value)))
                {
                    this[this.myTable.ColumnCountryCode] = value;
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

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnPartnerClass);
            this.SetNull(this.myTable.ColumnPartnerShortName);
            this.SetNull(this.myTable.ColumnStatusCode);
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnPreviousName);
            this.SetNull(this.myTable.ColumnAcquisitionCode);
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnFamilyName);
            this[this.myTable.ColumnFamilyKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTelephoneNumber);
            this.SetNull(this.myTable.ColumnLocationType);
            this.SetNull(this.myTable.ColumnEmailAddress);
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnCity);
            this.SetNull(this.myTable.ColumnPostalCode);
            this.SetNull(this.myTable.ColumnLocality);
            this.SetNull(this.myTable.ColumnStreetName);
            this.SetNull(this.myTable.ColumnAddress3);
            this.SetNull(this.myTable.ColumnCounty);
            this.SetNull(this.myTable.ColumnCountryCode);
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
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
        public bool IsSiteKeyNull()
        {
            return this.IsNull(this.myTable.ColumnSiteKey);
        }

        /// assign NULL value
        public void SetSiteKeyNull()
        {
            this.SetNull(this.myTable.ColumnSiteKey);
        }

        /// test for NULL value
        public bool IsCityNull()
        {
            return this.IsNull(this.myTable.ColumnCity);
        }

        /// assign NULL value
        public void SetCityNull()
        {
            this.SetNull(this.myTable.ColumnCity);
        }

        /// test for NULL value
        public bool IsPostalCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPostalCode);
        }

        /// assign NULL value
        public void SetPostalCodeNull()
        {
            this.SetNull(this.myTable.ColumnPostalCode);
        }

        /// test for NULL value
        public bool IsLocalityNull()
        {
            return this.IsNull(this.myTable.ColumnLocality);
        }

        /// assign NULL value
        public void SetLocalityNull()
        {
            this.SetNull(this.myTable.ColumnLocality);
        }

        /// test for NULL value
        public bool IsStreetNameNull()
        {
            return this.IsNull(this.myTable.ColumnStreetName);
        }

        /// assign NULL value
        public void SetStreetNameNull()
        {
            this.SetNull(this.myTable.ColumnStreetName);
        }

        /// test for NULL value
        public bool IsAddress3Null()
        {
            return this.IsNull(this.myTable.ColumnAddress3);
        }

        /// assign NULL value
        public void SetAddress3Null()
        {
            this.SetNull(this.myTable.ColumnAddress3);
        }

        /// test for NULL value
        public bool IsCountyNull()
        {
            return this.IsNull(this.myTable.ColumnCounty);
        }

        /// assign NULL value
        public void SetCountyNull()
        {
            this.SetNull(this.myTable.ColumnCounty);
        }

        /// test for NULL value
        public bool IsCountryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCountryCode);
        }

        /// assign NULL value
        public void SetCountryCodeNull()
        {
            this.SetNull(this.myTable.ColumnCountryCode);
        }

        /// test for NULL value
        public bool IsLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKey);
        }

        /// assign NULL value
        public void SetLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnLocationKey);
        }
    }

     /// auto generated
    [Serializable()]
    public class BestAddressTDS : TTypedDataSet
    {

        private BestAddressTDSLocationTable TableLocation;

        /// auto generated
        public BestAddressTDS() :
                base("BestAddressTDS")
        {
        }

        /// auto generated for serialization
        public BestAddressTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public BestAddressTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public BestAddressTDSLocationTable Location
        {
            get
            {
                return this.TableLocation;
            }
        }

        /// auto generated
        public new virtual BestAddressTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((BestAddressTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new BestAddressTDSLocationTable("Location"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("Location") != -1))
            {
                this.Tables.Add(new BestAddressTDSLocationTable("Location"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableLocation != null))
            {
                this.TableLocation.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "BestAddressTDS";
            this.TableLocation = ((BestAddressTDSLocationTable)(this.Tables["Location"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
        }
    }

    /// Address and other data related to that address.
    [Serializable()]
    public class BestAddressTDSLocationTable : PLocationTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 40;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 23;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailAddressId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnValidAddressId = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryNameId = 26;

        /// constructor
        public BestAddressTDSLocationTable() :
                base("Location")
        {
        }

        /// constructor
        public BestAddressTDSLocationTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public BestAddressTDSLocationTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        ///
        public DataColumn ColumnEmailAddress;
        ///
        public DataColumn ColumnValidAddress;
        ///
        public DataColumn ColumnCountryName;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_building_1_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_building_2_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_street_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_locality_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_suburb_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_city_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_county_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_postal_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_3_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_latitude_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_longitude_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_km_x_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_km_y_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_geo_accuracy_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_email_address_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("ValidAddress", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("CountryName", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnBuilding1 = this.Columns["p_building_1_c"];
            this.ColumnBuilding2 = this.Columns["p_building_2_c"];
            this.ColumnStreetName = this.Columns["p_street_name_c"];
            this.ColumnLocality = this.Columns["p_locality_c"];
            this.ColumnSuburb = this.Columns["p_suburb_c"];
            this.ColumnCity = this.Columns["p_city_c"];
            this.ColumnCounty = this.Columns["p_county_c"];
            this.ColumnPostalCode = this.Columns["p_postal_code_c"];
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnAddress3 = this.Columns["p_address_3_c"];
            this.ColumnGeoLatitude = this.Columns["p_geo_latitude_n"];
            this.ColumnGeoLongitude = this.Columns["p_geo_longitude_n"];
            this.ColumnGeoKmX = this.Columns["p_geo_km_x_i"];
            this.ColumnGeoKmY = this.Columns["p_geo_km_y_i"];
            this.ColumnGeoAccuracy = this.Columns["p_geo_accuracy_i"];
            this.ColumnRestricted = this.Columns["p_restricted_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnEmailAddress = this.Columns["p_email_address_c"];
            this.ColumnValidAddress = this.Columns["ValidAddress"];
            this.ColumnCountryName = this.Columns["CountryName"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnPartnerKey};
        }

        /// Access a typed row by index
        public new BestAddressTDSLocationRow this[int i]
        {
            get
            {
                return ((BestAddressTDSLocationRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new BestAddressTDSLocationRow NewRowTyped(bool AWithDefaultValues)
        {
            BestAddressTDSLocationRow ret = ((BestAddressTDSLocationRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new BestAddressTDSLocationRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new BestAddressTDSLocationRow(builder);
        }

        /// get typed set of changes
        public new BestAddressTDSLocationTable GetChangesTyped()
        {
            return ((BestAddressTDSLocationTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "Location";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_location";
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
        public static string GetEmailAddressDBName()
        {
            return "p_email_address_c";
        }

        /// get character length for column
        public static short GetEmailAddressLength()
        {
            return 120;
        }

        /// get the name of the field in the database for this column
        public static string GetValidAddressDBName()
        {
            return "ValidAddress";
        }

        /// get character length for column
        public static short GetValidAddressLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryNameDBName()
        {
            return "CountryName";
        }

        /// get character length for column
        public static short GetCountryNameLength()
        {
            return -1;
        }

    }

    /// Address and other data related to that address.
    [Serializable()]
    public class BestAddressTDSLocationRow : PLocationRow
    {
        private BestAddressTDSLocationTable myTable;

        /// Constructor
        public BestAddressTDSLocationRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((BestAddressTDSLocationTable)(this.Table));
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
        public Boolean ValidAddress
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidAddress.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValidAddress)
                            || (((Boolean)(this[this.myTable.ColumnValidAddress])) != value)))
                {
                    this[this.myTable.ColumnValidAddress] = value;
                }
            }
        }

        ///
        public string CountryName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryName)
                            || (((string)(this[this.myTable.ColumnCountryName])) != value)))
                {
                    this[this.myTable.ColumnCountryName] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBuilding1);
            this.SetNull(this.myTable.ColumnBuilding2);
            this.SetNull(this.myTable.ColumnStreetName);
            this.SetNull(this.myTable.ColumnLocality);
            this.SetNull(this.myTable.ColumnSuburb);
            this.SetNull(this.myTable.ColumnCity);
            this.SetNull(this.myTable.ColumnCounty);
            this.SetNull(this.myTable.ColumnPostalCode);
            this.SetNull(this.myTable.ColumnCountryCode);
            this.SetNull(this.myTable.ColumnAddress3);
            this.SetNull(this.myTable.ColumnGeoLatitude);
            this.SetNull(this.myTable.ColumnGeoLongitude);
            this.SetNull(this.myTable.ColumnGeoKmX);
            this.SetNull(this.myTable.ColumnGeoKmY);
            this[this.myTable.ColumnGeoAccuracy.Ordinal] = -1;
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnEmailAddress);
            this.SetNull(this.myTable.ColumnValidAddress);
            this.SetNull(this.myTable.ColumnCountryName);
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
        public bool IsValidAddressNull()
        {
            return this.IsNull(this.myTable.ColumnValidAddress);
        }

        /// assign NULL value
        public void SetValidAddressNull()
        {
            this.SetNull(this.myTable.ColumnValidAddress);
        }

        /// test for NULL value
        public bool IsCountryNameNull()
        {
            return this.IsNull(this.myTable.ColumnCountryName);
        }

        /// assign NULL value
        public void SetCountryNameNull()
        {
            this.SetNull(this.myTable.ColumnCountryName);
        }
    }

     /// auto generated
    [Serializable()]
    public class PartnerSetupTDS : TTypedDataSet
    {

        private PCountryTable TablePCountry;
        private PLanguageTable TablePLanguage;
        private PPartnerStatusTable TablePPartnerStatus;
        private PAcquisitionTable TablePAcquisition;
        private PTypeTable TablePType;
        private PEmailTable TablePEmail;
        private PDenominationTable TablePDenomination;
        private POccupationTable TablePOccupation;
        private PtMaritalStatusTable TablePtMaritalStatus;
        private AFrequencyTable TableAFrequency;
        private PRelationTable TablePRelation;
        private PRelationCategoryTable TablePRelationCategory;
        private PInterestTable TablePInterest;
        private PInterestCategoryTable TablePInterestCategory;
        private PMethodOfContactTable TablePMethodOfContact;
        private PContactAttributeTable TablePContactAttribute;
        private PPublicationTable TablePPublication;
        private PPublicationCostTable TablePPublicationCost;
        private PReasonSubscriptionGivenTable TablePReasonSubscriptionGiven;
        private PReasonSubscriptionCancelledTable TablePReasonSubscriptionCancelled;

        /// auto generated
        public PartnerSetupTDS() :
                base("PartnerSetupTDS")
        {
        }

        /// auto generated for serialization
        public PartnerSetupTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public PartnerSetupTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public PCountryTable PCountry
        {
            get
            {
                return this.TablePCountry;
            }
        }

        /// auto generated
        public PLanguageTable PLanguage
        {
            get
            {
                return this.TablePLanguage;
            }
        }

        /// auto generated
        public PPartnerStatusTable PPartnerStatus
        {
            get
            {
                return this.TablePPartnerStatus;
            }
        }

        /// auto generated
        public PAcquisitionTable PAcquisition
        {
            get
            {
                return this.TablePAcquisition;
            }
        }

        /// auto generated
        public PTypeTable PType
        {
            get
            {
                return this.TablePType;
            }
        }

        /// auto generated
        public PEmailTable PEmail
        {
            get
            {
                return this.TablePEmail;
            }
        }

        /// auto generated
        public PDenominationTable PDenomination
        {
            get
            {
                return this.TablePDenomination;
            }
        }

        /// auto generated
        public POccupationTable POccupation
        {
            get
            {
                return this.TablePOccupation;
            }
        }

        /// auto generated
        public PtMaritalStatusTable PtMaritalStatus
        {
            get
            {
                return this.TablePtMaritalStatus;
            }
        }

        /// auto generated
        public AFrequencyTable AFrequency
        {
            get
            {
                return this.TableAFrequency;
            }
        }

        /// auto generated
        public PRelationTable PRelation
        {
            get
            {
                return this.TablePRelation;
            }
        }

        /// auto generated
        public PRelationCategoryTable PRelationCategory
        {
            get
            {
                return this.TablePRelationCategory;
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
        public PInterestCategoryTable PInterestCategory
        {
            get
            {
                return this.TablePInterestCategory;
            }
        }

        /// auto generated
        public PMethodOfContactTable PMethodOfContact
        {
            get
            {
                return this.TablePMethodOfContact;
            }
        }

        /// auto generated
        public PContactAttributeTable PContactAttribute
        {
            get
            {
                return this.TablePContactAttribute;
            }
        }

        /// auto generated
        public PPublicationTable PPublication
        {
            get
            {
                return this.TablePPublication;
            }
        }

        /// auto generated
        public PPublicationCostTable PPublicationCost
        {
            get
            {
                return this.TablePPublicationCost;
            }
        }

        /// auto generated
        public PReasonSubscriptionGivenTable PReasonSubscriptionGiven
        {
            get
            {
                return this.TablePReasonSubscriptionGiven;
            }
        }

        /// auto generated
        public PReasonSubscriptionCancelledTable PReasonSubscriptionCancelled
        {
            get
            {
                return this.TablePReasonSubscriptionCancelled;
            }
        }

        /// auto generated
        public new virtual PartnerSetupTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerSetupTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PCountryTable("PCountry"));
            this.Tables.Add(new PLanguageTable("PLanguage"));
            this.Tables.Add(new PPartnerStatusTable("PPartnerStatus"));
            this.Tables.Add(new PAcquisitionTable("PAcquisition"));
            this.Tables.Add(new PTypeTable("PType"));
            this.Tables.Add(new PEmailTable("PEmail"));
            this.Tables.Add(new PDenominationTable("PDenomination"));
            this.Tables.Add(new POccupationTable("POccupation"));
            this.Tables.Add(new PtMaritalStatusTable("PtMaritalStatus"));
            this.Tables.Add(new AFrequencyTable("AFrequency"));
            this.Tables.Add(new PRelationTable("PRelation"));
            this.Tables.Add(new PRelationCategoryTable("PRelationCategory"));
            this.Tables.Add(new PInterestTable("PInterest"));
            this.Tables.Add(new PInterestCategoryTable("PInterestCategory"));
            this.Tables.Add(new PMethodOfContactTable("PMethodOfContact"));
            this.Tables.Add(new PContactAttributeTable("PContactAttribute"));
            this.Tables.Add(new PPublicationTable("PPublication"));
            this.Tables.Add(new PPublicationCostTable("PPublicationCost"));
            this.Tables.Add(new PReasonSubscriptionGivenTable("PReasonSubscriptionGiven"));
            this.Tables.Add(new PReasonSubscriptionCancelledTable("PReasonSubscriptionCancelled"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PCountry") != -1))
            {
                this.Tables.Add(new PCountryTable("PCountry"));
            }
            if ((ds.Tables.IndexOf("PLanguage") != -1))
            {
                this.Tables.Add(new PLanguageTable("PLanguage"));
            }
            if ((ds.Tables.IndexOf("PPartnerStatus") != -1))
            {
                this.Tables.Add(new PPartnerStatusTable("PPartnerStatus"));
            }
            if ((ds.Tables.IndexOf("PAcquisition") != -1))
            {
                this.Tables.Add(new PAcquisitionTable("PAcquisition"));
            }
            if ((ds.Tables.IndexOf("PType") != -1))
            {
                this.Tables.Add(new PTypeTable("PType"));
            }
            if ((ds.Tables.IndexOf("PEmail") != -1))
            {
                this.Tables.Add(new PEmailTable("PEmail"));
            }
            if ((ds.Tables.IndexOf("PDenomination") != -1))
            {
                this.Tables.Add(new PDenominationTable("PDenomination"));
            }
            if ((ds.Tables.IndexOf("POccupation") != -1))
            {
                this.Tables.Add(new POccupationTable("POccupation"));
            }
            if ((ds.Tables.IndexOf("PtMaritalStatus") != -1))
            {
                this.Tables.Add(new PtMaritalStatusTable("PtMaritalStatus"));
            }
            if ((ds.Tables.IndexOf("AFrequency") != -1))
            {
                this.Tables.Add(new AFrequencyTable("AFrequency"));
            }
            if ((ds.Tables.IndexOf("PRelation") != -1))
            {
                this.Tables.Add(new PRelationTable("PRelation"));
            }
            if ((ds.Tables.IndexOf("PRelationCategory") != -1))
            {
                this.Tables.Add(new PRelationCategoryTable("PRelationCategory"));
            }
            if ((ds.Tables.IndexOf("PInterest") != -1))
            {
                this.Tables.Add(new PInterestTable("PInterest"));
            }
            if ((ds.Tables.IndexOf("PInterestCategory") != -1))
            {
                this.Tables.Add(new PInterestCategoryTable("PInterestCategory"));
            }
            if ((ds.Tables.IndexOf("PMethodOfContact") != -1))
            {
                this.Tables.Add(new PMethodOfContactTable("PMethodOfContact"));
            }
            if ((ds.Tables.IndexOf("PContactAttribute") != -1))
            {
                this.Tables.Add(new PContactAttributeTable("PContactAttribute"));
            }
            if ((ds.Tables.IndexOf("PPublication") != -1))
            {
                this.Tables.Add(new PPublicationTable("PPublication"));
            }
            if ((ds.Tables.IndexOf("PPublicationCost") != -1))
            {
                this.Tables.Add(new PPublicationCostTable("PPublicationCost"));
            }
            if ((ds.Tables.IndexOf("PReasonSubscriptionGiven") != -1))
            {
                this.Tables.Add(new PReasonSubscriptionGivenTable("PReasonSubscriptionGiven"));
            }
            if ((ds.Tables.IndexOf("PReasonSubscriptionCancelled") != -1))
            {
                this.Tables.Add(new PReasonSubscriptionCancelledTable("PReasonSubscriptionCancelled"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePCountry != null))
            {
                this.TablePCountry.InitVars();
            }
            if ((this.TablePLanguage != null))
            {
                this.TablePLanguage.InitVars();
            }
            if ((this.TablePPartnerStatus != null))
            {
                this.TablePPartnerStatus.InitVars();
            }
            if ((this.TablePAcquisition != null))
            {
                this.TablePAcquisition.InitVars();
            }
            if ((this.TablePType != null))
            {
                this.TablePType.InitVars();
            }
            if ((this.TablePEmail != null))
            {
                this.TablePEmail.InitVars();
            }
            if ((this.TablePDenomination != null))
            {
                this.TablePDenomination.InitVars();
            }
            if ((this.TablePOccupation != null))
            {
                this.TablePOccupation.InitVars();
            }
            if ((this.TablePtMaritalStatus != null))
            {
                this.TablePtMaritalStatus.InitVars();
            }
            if ((this.TableAFrequency != null))
            {
                this.TableAFrequency.InitVars();
            }
            if ((this.TablePRelation != null))
            {
                this.TablePRelation.InitVars();
            }
            if ((this.TablePRelationCategory != null))
            {
                this.TablePRelationCategory.InitVars();
            }
            if ((this.TablePInterest != null))
            {
                this.TablePInterest.InitVars();
            }
            if ((this.TablePInterestCategory != null))
            {
                this.TablePInterestCategory.InitVars();
            }
            if ((this.TablePMethodOfContact != null))
            {
                this.TablePMethodOfContact.InitVars();
            }
            if ((this.TablePContactAttribute != null))
            {
                this.TablePContactAttribute.InitVars();
            }
            if ((this.TablePPublication != null))
            {
                this.TablePPublication.InitVars();
            }
            if ((this.TablePPublicationCost != null))
            {
                this.TablePPublicationCost.InitVars();
            }
            if ((this.TablePReasonSubscriptionGiven != null))
            {
                this.TablePReasonSubscriptionGiven.InitVars();
            }
            if ((this.TablePReasonSubscriptionCancelled != null))
            {
                this.TablePReasonSubscriptionCancelled.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerSetupTDS";
            this.TablePCountry = ((PCountryTable)(this.Tables["PCountry"]));
            this.TablePLanguage = ((PLanguageTable)(this.Tables["PLanguage"]));
            this.TablePPartnerStatus = ((PPartnerStatusTable)(this.Tables["PPartnerStatus"]));
            this.TablePAcquisition = ((PAcquisitionTable)(this.Tables["PAcquisition"]));
            this.TablePType = ((PTypeTable)(this.Tables["PType"]));
            this.TablePEmail = ((PEmailTable)(this.Tables["PEmail"]));
            this.TablePDenomination = ((PDenominationTable)(this.Tables["PDenomination"]));
            this.TablePOccupation = ((POccupationTable)(this.Tables["POccupation"]));
            this.TablePtMaritalStatus = ((PtMaritalStatusTable)(this.Tables["PtMaritalStatus"]));
            this.TableAFrequency = ((AFrequencyTable)(this.Tables["AFrequency"]));
            this.TablePRelation = ((PRelationTable)(this.Tables["PRelation"]));
            this.TablePRelationCategory = ((PRelationCategoryTable)(this.Tables["PRelationCategory"]));
            this.TablePInterest = ((PInterestTable)(this.Tables["PInterest"]));
            this.TablePInterestCategory = ((PInterestCategoryTable)(this.Tables["PInterestCategory"]));
            this.TablePMethodOfContact = ((PMethodOfContactTable)(this.Tables["PMethodOfContact"]));
            this.TablePContactAttribute = ((PContactAttributeTable)(this.Tables["PContactAttribute"]));
            this.TablePPublication = ((PPublicationTable)(this.Tables["PPublication"]));
            this.TablePPublicationCost = ((PPublicationCostTable)(this.Tables["PPublicationCost"]));
            this.TablePReasonSubscriptionGiven = ((PReasonSubscriptionGivenTable)(this.Tables["PReasonSubscriptionGiven"]));
            this.TablePReasonSubscriptionCancelled = ((PReasonSubscriptionCancelledTable)(this.Tables["PReasonSubscriptionCancelled"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePInterestCategory != null)
                        && (this.TablePInterest != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKInterest1", "PInterestCategory", new string[] {
                                "p_category_c"}, "PInterest", new string[] {
                                "p_category_c"}));
            }
            if (((this.TableAFrequency != null)
                        && (this.TablePPublication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPublication1", "AFrequency", new string[] {
                                "a_frequency_code_c"}, "PPublication", new string[] {
                                "a_frequency_code_c"}));
            }
            if (((this.TablePLanguage != null)
                        && (this.TablePPublication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPublication2", "PLanguage", new string[] {
                                "p_language_code_c"}, "PPublication", new string[] {
                                "p_publication_language_c"}));
            }
            if (((this.TablePPublication != null)
                        && (this.TablePPublicationCost != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPublicationCost1", "PPublication", new string[] {
                                "p_publication_code_c"}, "PPublicationCost", new string[] {
                                "p_publication_code_c"}));
            }
            if (((this.TablePRelationCategory != null)
                        && (this.TablePRelation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRelation1", "PRelationCategory", new string[] {
                                "p_code_c"}, "PRelation", new string[] {
                                "p_relation_category_c"}));
            }
        }
    }
     /// auto generated
    [Serializable()]
    public class PartnerImportExportTDS : TTypedDataSet
    {

        private PPartnerTable TablePPartner;
        private PLocationTable TablePLocation;
        private PPartnerLocationTable TablePPartnerLocation;
        private PPersonTable TablePPerson;
        private PFamilyTable TablePFamily;
        private PUnitTable TablePUnit;
        private POrganisationTable TablePOrganisation;
        private PChurchTable TablePChurch;
        private PBankTable TablePBank;
        private PVenueTable TablePVenue;
        private PPartnerCommentTable TablePPartnerComment;
        private PPartnerTypeTable TablePPartnerType;
        private PPartnerInterestTable TablePPartnerInterest;
        private PInterestTable TablePInterest;
        private PPublicationTable TablePPublication;
        private PSubscriptionTable TablePSubscription;
        private PmPersonalDataTable TablePmPersonalData;
        private PmPassportDetailsTable TablePmPassportDetails;
        private PmDocumentTable TablePmDocument;
        private PmDocumentTypeTable TablePmDocumentType;
        private PmPersonQualificationTable TablePmPersonQualification;
        private PmSpecialNeedTable TablePmSpecialNeed;
        private PmPastExperienceTable TablePmPastExperience;
        private PmPersonLanguageTable TablePmPersonLanguage;
        private PmPersonAbilityTable TablePmPersonAbility;
        private PmPersonVisionTable TablePmPersonVision;
        private PmStaffDataTable TablePmStaffData;
        private PmJobAssignmentTable TablePmJobAssignment;
        private PmPersonEvaluationTable TablePmPersonEvaluation;
        private PmGeneralApplicationTable TablePmGeneralApplication;
        private PtApplicationTypeTable TablePtApplicationType;
        private PmShortTermApplicationTable TablePmShortTermApplication;
        private PmYearProgramApplicationTable TablePmYearProgramApplication;
        private PmApplicationFormsTable TablePmApplicationForms;
        private UmUnitStructureTable TableUmUnitStructure;
        private UmUnitAbilityTable TableUmUnitAbility;
        private UmUnitLanguageTable TableUmUnitLanguage;
        private UmUnitVisionTable TableUmUnitVision;
        private UmUnitCostTable TableUmUnitCost;
        private UmJobTable TableUmJob;
        private UmJobRequirementTable TableUmJobRequirement;
        private UmJobLanguageTable TableUmJobLanguage;
        private UmJobQualificationTable TableUmJobQualification;
        private UmJobVisionTable TableUmJobVision;
        private PcBuildingTable TablePcBuilding;
        private PcRoomTable TablePcRoom;

        /// auto generated
        public PartnerImportExportTDS() :
                base("PartnerImportExportTDS")
        {
        }

        /// auto generated for serialization
        public PartnerImportExportTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public PartnerImportExportTDS(string ADatasetName) :
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
        public PPersonTable PPerson
        {
            get
            {
                return this.TablePPerson;
            }
        }

        /// auto generated
        public PFamilyTable PFamily
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
        public PVenueTable PVenue
        {
            get
            {
                return this.TablePVenue;
            }
        }

        /// auto generated
        public PPartnerCommentTable PPartnerComment
        {
            get
            {
                return this.TablePPartnerComment;
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
        public PPublicationTable PPublication
        {
            get
            {
                return this.TablePPublication;
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
        public PmPersonalDataTable PmPersonalData
        {
            get
            {
                return this.TablePmPersonalData;
            }
        }

        /// auto generated
        public PmPassportDetailsTable PmPassportDetails
        {
            get
            {
                return this.TablePmPassportDetails;
            }
        }

        /// auto generated
        public PmDocumentTable PmDocument
        {
            get
            {
                return this.TablePmDocument;
            }
        }

        /// auto generated
        public PmDocumentTypeTable PmDocumentType
        {
            get
            {
                return this.TablePmDocumentType;
            }
        }

        /// auto generated
        public PmPersonQualificationTable PmPersonQualification
        {
            get
            {
                return this.TablePmPersonQualification;
            }
        }

        /// auto generated
        public PmSpecialNeedTable PmSpecialNeed
        {
            get
            {
                return this.TablePmSpecialNeed;
            }
        }

        /// auto generated
        public PmPastExperienceTable PmPastExperience
        {
            get
            {
                return this.TablePmPastExperience;
            }
        }

        /// auto generated
        public PmPersonLanguageTable PmPersonLanguage
        {
            get
            {
                return this.TablePmPersonLanguage;
            }
        }

        /// auto generated
        public PmPersonAbilityTable PmPersonAbility
        {
            get
            {
                return this.TablePmPersonAbility;
            }
        }

        /// auto generated
        public PmPersonVisionTable PmPersonVision
        {
            get
            {
                return this.TablePmPersonVision;
            }
        }

        /// auto generated
        public PmStaffDataTable PmStaffData
        {
            get
            {
                return this.TablePmStaffData;
            }
        }

        /// auto generated
        public PmJobAssignmentTable PmJobAssignment
        {
            get
            {
                return this.TablePmJobAssignment;
            }
        }

        /// auto generated
        public PmPersonEvaluationTable PmPersonEvaluation
        {
            get
            {
                return this.TablePmPersonEvaluation;
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
        public PtApplicationTypeTable PtApplicationType
        {
            get
            {
                return this.TablePtApplicationType;
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
        public PmYearProgramApplicationTable PmYearProgramApplication
        {
            get
            {
                return this.TablePmYearProgramApplication;
            }
        }

        /// auto generated
        public PmApplicationFormsTable PmApplicationForms
        {
            get
            {
                return this.TablePmApplicationForms;
            }
        }

        /// auto generated
        public UmUnitStructureTable UmUnitStructure
        {
            get
            {
                return this.TableUmUnitStructure;
            }
        }

        /// auto generated
        public UmUnitAbilityTable UmUnitAbility
        {
            get
            {
                return this.TableUmUnitAbility;
            }
        }

        /// auto generated
        public UmUnitLanguageTable UmUnitLanguage
        {
            get
            {
                return this.TableUmUnitLanguage;
            }
        }

        /// auto generated
        public UmUnitVisionTable UmUnitVision
        {
            get
            {
                return this.TableUmUnitVision;
            }
        }

        /// auto generated
        public UmUnitCostTable UmUnitCost
        {
            get
            {
                return this.TableUmUnitCost;
            }
        }

        /// auto generated
        public UmJobTable UmJob
        {
            get
            {
                return this.TableUmJob;
            }
        }

        /// auto generated
        public UmJobRequirementTable UmJobRequirement
        {
            get
            {
                return this.TableUmJobRequirement;
            }
        }

        /// auto generated
        public UmJobLanguageTable UmJobLanguage
        {
            get
            {
                return this.TableUmJobLanguage;
            }
        }

        /// auto generated
        public UmJobQualificationTable UmJobQualification
        {
            get
            {
                return this.TableUmJobQualification;
            }
        }

        /// auto generated
        public UmJobVisionTable UmJobVision
        {
            get
            {
                return this.TableUmJobVision;
            }
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
        public new virtual PartnerImportExportTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((PartnerImportExportTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new PLocationTable("PLocation"));
            this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            this.Tables.Add(new PPersonTable("PPerson"));
            this.Tables.Add(new PFamilyTable("PFamily"));
            this.Tables.Add(new PUnitTable("PUnit"));
            this.Tables.Add(new POrganisationTable("POrganisation"));
            this.Tables.Add(new PChurchTable("PChurch"));
            this.Tables.Add(new PBankTable("PBank"));
            this.Tables.Add(new PVenueTable("PVenue"));
            this.Tables.Add(new PPartnerCommentTable("PPartnerComment"));
            this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            this.Tables.Add(new PPartnerInterestTable("PPartnerInterest"));
            this.Tables.Add(new PInterestTable("PInterest"));
            this.Tables.Add(new PPublicationTable("PPublication"));
            this.Tables.Add(new PSubscriptionTable("PSubscription"));
            this.Tables.Add(new PmPersonalDataTable("PmPersonalData"));
            this.Tables.Add(new PmPassportDetailsTable("PmPassportDetails"));
            this.Tables.Add(new PmDocumentTable("PmDocument"));
            this.Tables.Add(new PmDocumentTypeTable("PmDocumentType"));
            this.Tables.Add(new PmPersonQualificationTable("PmPersonQualification"));
            this.Tables.Add(new PmSpecialNeedTable("PmSpecialNeed"));
            this.Tables.Add(new PmPastExperienceTable("PmPastExperience"));
            this.Tables.Add(new PmPersonLanguageTable("PmPersonLanguage"));
            this.Tables.Add(new PmPersonAbilityTable("PmPersonAbility"));
            this.Tables.Add(new PmPersonVisionTable("PmPersonVision"));
            this.Tables.Add(new PmStaffDataTable("PmStaffData"));
            this.Tables.Add(new PmJobAssignmentTable("PmJobAssignment"));
            this.Tables.Add(new PmPersonEvaluationTable("PmPersonEvaluation"));
            this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            this.Tables.Add(new PtApplicationTypeTable("PtApplicationType"));
            this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
            this.Tables.Add(new PmYearProgramApplicationTable("PmYearProgramApplication"));
            this.Tables.Add(new PmApplicationFormsTable("PmApplicationForms"));
            this.Tables.Add(new UmUnitStructureTable("UmUnitStructure"));
            this.Tables.Add(new UmUnitAbilityTable("UmUnitAbility"));
            this.Tables.Add(new UmUnitLanguageTable("UmUnitLanguage"));
            this.Tables.Add(new UmUnitVisionTable("UmUnitVision"));
            this.Tables.Add(new UmUnitCostTable("UmUnitCost"));
            this.Tables.Add(new UmJobTable("UmJob"));
            this.Tables.Add(new UmJobRequirementTable("UmJobRequirement"));
            this.Tables.Add(new UmJobLanguageTable("UmJobLanguage"));
            this.Tables.Add(new UmJobQualificationTable("UmJobQualification"));
            this.Tables.Add(new UmJobVisionTable("UmJobVision"));
            this.Tables.Add(new PcBuildingTable("PcBuilding"));
            this.Tables.Add(new PcRoomTable("PcRoom"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("PLocation") != -1))
            {
                this.Tables.Add(new PLocationTable("PLocation"));
            }
            if ((ds.Tables.IndexOf("PPartnerLocation") != -1))
            {
                this.Tables.Add(new PPartnerLocationTable("PPartnerLocation"));
            }
            if ((ds.Tables.IndexOf("PPerson") != -1))
            {
                this.Tables.Add(new PPersonTable("PPerson"));
            }
            if ((ds.Tables.IndexOf("PFamily") != -1))
            {
                this.Tables.Add(new PFamilyTable("PFamily"));
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
            if ((ds.Tables.IndexOf("PVenue") != -1))
            {
                this.Tables.Add(new PVenueTable("PVenue"));
            }
            if ((ds.Tables.IndexOf("PPartnerComment") != -1))
            {
                this.Tables.Add(new PPartnerCommentTable("PPartnerComment"));
            }
            if ((ds.Tables.IndexOf("PPartnerType") != -1))
            {
                this.Tables.Add(new PPartnerTypeTable("PPartnerType"));
            }
            if ((ds.Tables.IndexOf("PPartnerInterest") != -1))
            {
                this.Tables.Add(new PPartnerInterestTable("PPartnerInterest"));
            }
            if ((ds.Tables.IndexOf("PInterest") != -1))
            {
                this.Tables.Add(new PInterestTable("PInterest"));
            }
            if ((ds.Tables.IndexOf("PPublication") != -1))
            {
                this.Tables.Add(new PPublicationTable("PPublication"));
            }
            if ((ds.Tables.IndexOf("PSubscription") != -1))
            {
                this.Tables.Add(new PSubscriptionTable("PSubscription"));
            }
            if ((ds.Tables.IndexOf("PmPersonalData") != -1))
            {
                this.Tables.Add(new PmPersonalDataTable("PmPersonalData"));
            }
            if ((ds.Tables.IndexOf("PmPassportDetails") != -1))
            {
                this.Tables.Add(new PmPassportDetailsTable("PmPassportDetails"));
            }
            if ((ds.Tables.IndexOf("PmDocument") != -1))
            {
                this.Tables.Add(new PmDocumentTable("PmDocument"));
            }
            if ((ds.Tables.IndexOf("PmDocumentType") != -1))
            {
                this.Tables.Add(new PmDocumentTypeTable("PmDocumentType"));
            }
            if ((ds.Tables.IndexOf("PmPersonQualification") != -1))
            {
                this.Tables.Add(new PmPersonQualificationTable("PmPersonQualification"));
            }
            if ((ds.Tables.IndexOf("PmSpecialNeed") != -1))
            {
                this.Tables.Add(new PmSpecialNeedTable("PmSpecialNeed"));
            }
            if ((ds.Tables.IndexOf("PmPastExperience") != -1))
            {
                this.Tables.Add(new PmPastExperienceTable("PmPastExperience"));
            }
            if ((ds.Tables.IndexOf("PmPersonLanguage") != -1))
            {
                this.Tables.Add(new PmPersonLanguageTable("PmPersonLanguage"));
            }
            if ((ds.Tables.IndexOf("PmPersonAbility") != -1))
            {
                this.Tables.Add(new PmPersonAbilityTable("PmPersonAbility"));
            }
            if ((ds.Tables.IndexOf("PmPersonVision") != -1))
            {
                this.Tables.Add(new PmPersonVisionTable("PmPersonVision"));
            }
            if ((ds.Tables.IndexOf("PmStaffData") != -1))
            {
                this.Tables.Add(new PmStaffDataTable("PmStaffData"));
            }
            if ((ds.Tables.IndexOf("PmJobAssignment") != -1))
            {
                this.Tables.Add(new PmJobAssignmentTable("PmJobAssignment"));
            }
            if ((ds.Tables.IndexOf("PmPersonEvaluation") != -1))
            {
                this.Tables.Add(new PmPersonEvaluationTable("PmPersonEvaluation"));
            }
            if ((ds.Tables.IndexOf("PmGeneralApplication") != -1))
            {
                this.Tables.Add(new PmGeneralApplicationTable("PmGeneralApplication"));
            }
            if ((ds.Tables.IndexOf("PtApplicationType") != -1))
            {
                this.Tables.Add(new PtApplicationTypeTable("PtApplicationType"));
            }
            if ((ds.Tables.IndexOf("PmShortTermApplication") != -1))
            {
                this.Tables.Add(new PmShortTermApplicationTable("PmShortTermApplication"));
            }
            if ((ds.Tables.IndexOf("PmYearProgramApplication") != -1))
            {
                this.Tables.Add(new PmYearProgramApplicationTable("PmYearProgramApplication"));
            }
            if ((ds.Tables.IndexOf("PmApplicationForms") != -1))
            {
                this.Tables.Add(new PmApplicationFormsTable("PmApplicationForms"));
            }
            if ((ds.Tables.IndexOf("UmUnitStructure") != -1))
            {
                this.Tables.Add(new UmUnitStructureTable("UmUnitStructure"));
            }
            if ((ds.Tables.IndexOf("UmUnitAbility") != -1))
            {
                this.Tables.Add(new UmUnitAbilityTable("UmUnitAbility"));
            }
            if ((ds.Tables.IndexOf("UmUnitLanguage") != -1))
            {
                this.Tables.Add(new UmUnitLanguageTable("UmUnitLanguage"));
            }
            if ((ds.Tables.IndexOf("UmUnitVision") != -1))
            {
                this.Tables.Add(new UmUnitVisionTable("UmUnitVision"));
            }
            if ((ds.Tables.IndexOf("UmUnitCost") != -1))
            {
                this.Tables.Add(new UmUnitCostTable("UmUnitCost"));
            }
            if ((ds.Tables.IndexOf("UmJob") != -1))
            {
                this.Tables.Add(new UmJobTable("UmJob"));
            }
            if ((ds.Tables.IndexOf("UmJobRequirement") != -1))
            {
                this.Tables.Add(new UmJobRequirementTable("UmJobRequirement"));
            }
            if ((ds.Tables.IndexOf("UmJobLanguage") != -1))
            {
                this.Tables.Add(new UmJobLanguageTable("UmJobLanguage"));
            }
            if ((ds.Tables.IndexOf("UmJobQualification") != -1))
            {
                this.Tables.Add(new UmJobQualificationTable("UmJobQualification"));
            }
            if ((ds.Tables.IndexOf("UmJobVision") != -1))
            {
                this.Tables.Add(new UmJobVisionTable("UmJobVision"));
            }
            if ((ds.Tables.IndexOf("PcBuilding") != -1))
            {
                this.Tables.Add(new PcBuildingTable("PcBuilding"));
            }
            if ((ds.Tables.IndexOf("PcRoom") != -1))
            {
                this.Tables.Add(new PcRoomTable("PcRoom"));
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
            if ((this.TablePLocation != null))
            {
                this.TablePLocation.InitVars();
            }
            if ((this.TablePPartnerLocation != null))
            {
                this.TablePPartnerLocation.InitVars();
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
            if ((this.TablePVenue != null))
            {
                this.TablePVenue.InitVars();
            }
            if ((this.TablePPartnerComment != null))
            {
                this.TablePPartnerComment.InitVars();
            }
            if ((this.TablePPartnerType != null))
            {
                this.TablePPartnerType.InitVars();
            }
            if ((this.TablePPartnerInterest != null))
            {
                this.TablePPartnerInterest.InitVars();
            }
            if ((this.TablePInterest != null))
            {
                this.TablePInterest.InitVars();
            }
            if ((this.TablePPublication != null))
            {
                this.TablePPublication.InitVars();
            }
            if ((this.TablePSubscription != null))
            {
                this.TablePSubscription.InitVars();
            }
            if ((this.TablePmPersonalData != null))
            {
                this.TablePmPersonalData.InitVars();
            }
            if ((this.TablePmPassportDetails != null))
            {
                this.TablePmPassportDetails.InitVars();
            }
            if ((this.TablePmDocument != null))
            {
                this.TablePmDocument.InitVars();
            }
            if ((this.TablePmDocumentType != null))
            {
                this.TablePmDocumentType.InitVars();
            }
            if ((this.TablePmPersonQualification != null))
            {
                this.TablePmPersonQualification.InitVars();
            }
            if ((this.TablePmSpecialNeed != null))
            {
                this.TablePmSpecialNeed.InitVars();
            }
            if ((this.TablePmPastExperience != null))
            {
                this.TablePmPastExperience.InitVars();
            }
            if ((this.TablePmPersonLanguage != null))
            {
                this.TablePmPersonLanguage.InitVars();
            }
            if ((this.TablePmPersonAbility != null))
            {
                this.TablePmPersonAbility.InitVars();
            }
            if ((this.TablePmPersonVision != null))
            {
                this.TablePmPersonVision.InitVars();
            }
            if ((this.TablePmStaffData != null))
            {
                this.TablePmStaffData.InitVars();
            }
            if ((this.TablePmJobAssignment != null))
            {
                this.TablePmJobAssignment.InitVars();
            }
            if ((this.TablePmPersonEvaluation != null))
            {
                this.TablePmPersonEvaluation.InitVars();
            }
            if ((this.TablePmGeneralApplication != null))
            {
                this.TablePmGeneralApplication.InitVars();
            }
            if ((this.TablePtApplicationType != null))
            {
                this.TablePtApplicationType.InitVars();
            }
            if ((this.TablePmShortTermApplication != null))
            {
                this.TablePmShortTermApplication.InitVars();
            }
            if ((this.TablePmYearProgramApplication != null))
            {
                this.TablePmYearProgramApplication.InitVars();
            }
            if ((this.TablePmApplicationForms != null))
            {
                this.TablePmApplicationForms.InitVars();
            }
            if ((this.TableUmUnitStructure != null))
            {
                this.TableUmUnitStructure.InitVars();
            }
            if ((this.TableUmUnitAbility != null))
            {
                this.TableUmUnitAbility.InitVars();
            }
            if ((this.TableUmUnitLanguage != null))
            {
                this.TableUmUnitLanguage.InitVars();
            }
            if ((this.TableUmUnitVision != null))
            {
                this.TableUmUnitVision.InitVars();
            }
            if ((this.TableUmUnitCost != null))
            {
                this.TableUmUnitCost.InitVars();
            }
            if ((this.TableUmJob != null))
            {
                this.TableUmJob.InitVars();
            }
            if ((this.TableUmJobRequirement != null))
            {
                this.TableUmJobRequirement.InitVars();
            }
            if ((this.TableUmJobLanguage != null))
            {
                this.TableUmJobLanguage.InitVars();
            }
            if ((this.TableUmJobQualification != null))
            {
                this.TableUmJobQualification.InitVars();
            }
            if ((this.TableUmJobVision != null))
            {
                this.TableUmJobVision.InitVars();
            }
            if ((this.TablePcBuilding != null))
            {
                this.TablePcBuilding.InitVars();
            }
            if ((this.TablePcRoom != null))
            {
                this.TablePcRoom.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "PartnerImportExportTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TablePLocation = ((PLocationTable)(this.Tables["PLocation"]));
            this.TablePPartnerLocation = ((PPartnerLocationTable)(this.Tables["PPartnerLocation"]));
            this.TablePPerson = ((PPersonTable)(this.Tables["PPerson"]));
            this.TablePFamily = ((PFamilyTable)(this.Tables["PFamily"]));
            this.TablePUnit = ((PUnitTable)(this.Tables["PUnit"]));
            this.TablePOrganisation = ((POrganisationTable)(this.Tables["POrganisation"]));
            this.TablePChurch = ((PChurchTable)(this.Tables["PChurch"]));
            this.TablePBank = ((PBankTable)(this.Tables["PBank"]));
            this.TablePVenue = ((PVenueTable)(this.Tables["PVenue"]));
            this.TablePPartnerComment = ((PPartnerCommentTable)(this.Tables["PPartnerComment"]));
            this.TablePPartnerType = ((PPartnerTypeTable)(this.Tables["PPartnerType"]));
            this.TablePPartnerInterest = ((PPartnerInterestTable)(this.Tables["PPartnerInterest"]));
            this.TablePInterest = ((PInterestTable)(this.Tables["PInterest"]));
            this.TablePPublication = ((PPublicationTable)(this.Tables["PPublication"]));
            this.TablePSubscription = ((PSubscriptionTable)(this.Tables["PSubscription"]));
            this.TablePmPersonalData = ((PmPersonalDataTable)(this.Tables["PmPersonalData"]));
            this.TablePmPassportDetails = ((PmPassportDetailsTable)(this.Tables["PmPassportDetails"]));
            this.TablePmDocument = ((PmDocumentTable)(this.Tables["PmDocument"]));
            this.TablePmDocumentType = ((PmDocumentTypeTable)(this.Tables["PmDocumentType"]));
            this.TablePmPersonQualification = ((PmPersonQualificationTable)(this.Tables["PmPersonQualification"]));
            this.TablePmSpecialNeed = ((PmSpecialNeedTable)(this.Tables["PmSpecialNeed"]));
            this.TablePmPastExperience = ((PmPastExperienceTable)(this.Tables["PmPastExperience"]));
            this.TablePmPersonLanguage = ((PmPersonLanguageTable)(this.Tables["PmPersonLanguage"]));
            this.TablePmPersonAbility = ((PmPersonAbilityTable)(this.Tables["PmPersonAbility"]));
            this.TablePmPersonVision = ((PmPersonVisionTable)(this.Tables["PmPersonVision"]));
            this.TablePmStaffData = ((PmStaffDataTable)(this.Tables["PmStaffData"]));
            this.TablePmJobAssignment = ((PmJobAssignmentTable)(this.Tables["PmJobAssignment"]));
            this.TablePmPersonEvaluation = ((PmPersonEvaluationTable)(this.Tables["PmPersonEvaluation"]));
            this.TablePmGeneralApplication = ((PmGeneralApplicationTable)(this.Tables["PmGeneralApplication"]));
            this.TablePtApplicationType = ((PtApplicationTypeTable)(this.Tables["PtApplicationType"]));
            this.TablePmShortTermApplication = ((PmShortTermApplicationTable)(this.Tables["PmShortTermApplication"]));
            this.TablePmYearProgramApplication = ((PmYearProgramApplicationTable)(this.Tables["PmYearProgramApplication"]));
            this.TablePmApplicationForms = ((PmApplicationFormsTable)(this.Tables["PmApplicationForms"]));
            this.TableUmUnitStructure = ((UmUnitStructureTable)(this.Tables["UmUnitStructure"]));
            this.TableUmUnitAbility = ((UmUnitAbilityTable)(this.Tables["UmUnitAbility"]));
            this.TableUmUnitLanguage = ((UmUnitLanguageTable)(this.Tables["UmUnitLanguage"]));
            this.TableUmUnitVision = ((UmUnitVisionTable)(this.Tables["UmUnitVision"]));
            this.TableUmUnitCost = ((UmUnitCostTable)(this.Tables["UmUnitCost"]));
            this.TableUmJob = ((UmJobTable)(this.Tables["UmJob"]));
            this.TableUmJobRequirement = ((UmJobRequirementTable)(this.Tables["UmJobRequirement"]));
            this.TableUmJobLanguage = ((UmJobLanguageTable)(this.Tables["UmJobLanguage"]));
            this.TableUmJobQualification = ((UmJobQualificationTable)(this.Tables["UmJobQualification"]));
            this.TableUmJobVision = ((UmJobVisionTable)(this.Tables["UmJobVision"]));
            this.TablePcBuilding = ((PcBuildingTable)(this.Tables["PcBuilding"]));
            this.TablePcRoom = ((PcRoomTable)(this.Tables["PcRoom"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
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
                        && (this.TablePPartnerComment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerComment1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerComment", new string[] {
                                "p_partner_key_n"}));
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
            if (((this.TablePPublication != null)
                        && (this.TablePSubscription != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKSubscription1", "PPublication", new string[] {
                                "p_publication_code_c"}, "PSubscription", new string[] {
                                "p_publication_code_c"}));
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
            if (((this.TablePVenue != null)
                        && (this.TablePcBuilding != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKBuilding1", "PVenue", new string[] {
                                "p_partner_key_n"}, "PcBuilding", new string[] {
                                "p_venue_key_n"}));
            }
            if (((this.TablePcBuilding != null)
                        && (this.TablePcRoom != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKRoom1", "PcBuilding", new string[] {
                                "p_venue_key_n", "pc_building_code_c"}, "PcRoom", new string[] {
                                "p_venue_key_n", "pc_building_code_c"}));
            }
            if (((this.TablePmGeneralApplication != null)
                        && (this.TablePmApplicationForms != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApplicationForms1", "PmGeneralApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}, "PmApplicationForms", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePmApplicationForms != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApplicationForms3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PmApplicationForms", new string[] {
                                "pm_reference_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDocument1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmDocument", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePmDocumentType != null)
                        && (this.TablePmDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDocument2", "PmDocumentType", new string[] {
                                "pm_doc_code_c"}, "PmDocument", new string[] {
                                "pm_doc_code_c"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePmDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDocument3", "PPartner", new string[] {
                                "p_partner_key_n"}, "PmDocument", new string[] {
                                "pm_contact_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication2", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "pm_registration_office_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication3", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "pm_gen_app_poss_srv_unit_key_n"}));
            }
            if (((this.TablePtApplicationType != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication5", "PtApplicationType", new string[] {
                                "pt_app_type_name_c"}, "PmGeneralApplication", new string[] {
                                "pt_app_type_name_c"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmGeneralApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGeneralApplication9", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmGeneralApplication", new string[] {
                                "pm_placement_partner_key_n"}));
            }
            if (((this.TablePPartner != null)
                        && (this.TablePmJobAssignment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobAssignment1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PmJobAssignment", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TableUmJob != null)
                        && (this.TablePmJobAssignment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobAssignment2", "UmJob", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}, "PmJobAssignment", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPassportDetails != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPassportDetails1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPassportDetails", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPastExperience != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPastExperience1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPastExperience", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonAbility != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonAbility1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonAbility", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonEvaluation != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonEvaluation1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonEvaluation", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonLanguage != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonLanguage1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonLanguage", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonQualification != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonQualification1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonQualification", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonVision != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonVision1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonVision", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmPersonalData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPersonalData1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmPersonalData", new string[] {
                                "p_partner_key_n"}));
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
            if (((this.TablePUnit != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication16", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_confirmed_option_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication17", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_option1_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication18", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_option2_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication19", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_current_field_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmShortTermApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKShortTermApplication20", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmShortTermApplication", new string[] {
                                "pm_st_field_charged_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmSpecialNeed != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKSpecialNeed1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmSpecialNeed", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null)
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData2", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "pm_office_recruited_by_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData3", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "pm_home_office_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData4", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "pm_receiving_field_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData5", "PUnit", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "pm_receiving_field_office_n"}));
            }
            if (((this.TablePmGeneralApplication != null)
                        && (this.TablePmYearProgramApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKYearProgramApplication1", "PmGeneralApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}, "PmYearProgramApplication", new string[] {
                                "p_partner_key_n", "pm_application_key_i", "pm_registration_office_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmJob != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJob1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmJob", new string[] {
                                "pm_unit_key_n"}));
            }
            if (((this.TableUmJob != null)
                        && (this.TableUmJobLanguage != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobLanguage1", "UmJob", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}, "UmJobLanguage", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}));
            }
            if (((this.TableUmJob != null)
                        && (this.TableUmJobQualification != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobQualification1", "UmJob", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}, "UmJobQualification", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}));
            }
            if (((this.TableUmJob != null)
                        && (this.TableUmJobRequirement != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobRequirement1", "UmJob", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}, "UmJobRequirement", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}));
            }
            if (((this.TableUmJob != null)
                        && (this.TableUmJobVision != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKJobVision1", "UmJob", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}, "UmJobVision", new string[] {
                                "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitAbility != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitAbility1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitAbility", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitCost != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitCost1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitCost", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitLanguage != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitLanguage1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitLanguage", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitStructure != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitStructure1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitStructure", new string[] {
                                "um_parent_unit_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitStructure != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitStructure2", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitStructure", new string[] {
                                "um_child_unit_key_n"}));
            }
            if (((this.TablePUnit != null)
                        && (this.TableUmUnitVision != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKUnitVision1", "PUnit", new string[] {
                                "p_partner_key_n"}, "UmUnitVision", new string[] {
                                "p_partner_key_n"}));
            }
        }
    }
}
