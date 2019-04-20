//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using System.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.AP.WebConnectors;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Tests.MPartner.shared.CreateTestPartnerData
{
    /// This will create data to be used in Partner tests
    public class TCreateTestPartnerData
    {
        /// create a new partner
        public static PPartnerRow CreateNewPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            // get a new partner key
            Int64 newPartnerKey = -1;

            do
            {
                newPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey, ADataBase);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, newPartnerKey, ref newPartnerKey, ADataBase);
                PartnerRow.PartnerKey = newPartnerKey;
            } while (newPartnerKey == -1);

            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            AMainDS.PPartner.Rows.Add(PartnerRow);

            TLogging.Log("Creating new partner: " + PartnerRow.PartnerKey.ToString());

            return PartnerRow;
        }

        /// create a new family
        public static PPartnerRow CreateNewFamilyPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestPartner, Mr";

            PFamilyRow FamilyRow = AMainDS.PFamily.NewRowTyped();
            FamilyRow.PartnerKey = PartnerRow.PartnerKey;
            FamilyRow.FamilyName = PartnerRow.PartnerKey.ToString();
            FamilyRow.FirstName = "TestPartner";
            FamilyRow.Title = "Mr";
            AMainDS.PFamily.Rows.Add(FamilyRow);

            return PartnerRow;
        }

        /// create a new person
        public static PPersonRow CreateNewPerson(PartnerEditTDS AMainDS,
            Int64 AFamilyKey,
            Int32 ALocationKey,
            string AFirstName,
            string ATitle,
            int AFamilyID,
            TDataBase ADataBase)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            PartnerRow.PartnerShortName = AFamilyKey.ToString() + ", " + AFirstName + ", " + ATitle;

            PPersonRow PersonRow = AMainDS.PPerson.NewRowTyped();
            PersonRow.PartnerKey = PartnerRow.PartnerKey;
            PersonRow.FamilyKey = AFamilyKey;
            PersonRow.FamilyName = AFamilyKey.ToString();
            PersonRow.FirstName = AFirstName;
            PersonRow.FamilyId = AFamilyID;
            PersonRow.Title = ATitle;
            AMainDS.PPerson.Rows.Add(PersonRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = ALocationKey;
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            return PersonRow;
        }

        /// create a new unit
        public static PPartnerRow CreateNewUnitPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestUnit";

            PUnitRow UnitRow = AMainDS.PUnit.NewRowTyped();
            UnitRow.PartnerKey = PartnerRow.PartnerKey;
            UnitRow.UnitName = "TestUnit";
            AMainDS.PUnit.Rows.Add(UnitRow);

            return PartnerRow;
        }

        /// create a new unit
        public static PPartnerRow CreateNewUnitPartnerWithTypeCode(PartnerEditTDS AMainDS, string AUnitType, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestUnit";

            PUnitRow UnitRow = AMainDS.PUnit.NewRowTyped();
            UnitRow.PartnerKey = PartnerRow.PartnerKey;
            UnitRow.UnitName = "TestUnit";
            UnitRow.UnitTypeCode = AUnitType;
            AMainDS.PUnit.Rows.Add(UnitRow);

            return PartnerRow;
        }

        /// create a new organisation
        public static PPartnerRow CreateNewOrganisationPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_ORGANISATION;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestOrganisation";

            POrganisationRow OrganisationRow = AMainDS.POrganisation.NewRowTyped();
            OrganisationRow.PartnerKey = PartnerRow.PartnerKey;
            OrganisationRow.OrganisationName = "TestOrganisation";
            AMainDS.POrganisation.Rows.Add(OrganisationRow);

            return PartnerRow;
        }

        /// create a new church
        public static PPartnerRow CreateNewChurchPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);
            TDataBase db = DBAccess.Connect("CreateNewChurchPartner", ADataBase);
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable);

            // make sure denomination "UNKNOWN" exists as this is the default value
            if (!PDenominationAccess.Exists("UNKNOWN", Transaction))
            {
                PDenominationTable DenominationTable = new PDenominationTable();
                PDenominationRow DenominationRow = DenominationTable.NewRowTyped();
                DenominationRow.DenominationCode = "UNKNOWN";
                DenominationRow.DenominationName = "Unknown";
                DenominationTable.Rows.Add(DenominationRow);
                PDenominationAccess.SubmitChanges(DenominationTable, Transaction);
                Transaction.Commit();
            }
            else
            {
                Transaction.Rollback();
            }

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_CHURCH;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestChurch";

            PChurchRow ChurchRow = AMainDS.PChurch.NewRowTyped();
            ChurchRow.PartnerKey = PartnerRow.PartnerKey;
            ChurchRow.ChurchName = "TestChurch";
            ChurchRow.DenominationCode = "UNKNOWN";
            AMainDS.PChurch.Rows.Add(ChurchRow);

            return PartnerRow;
        }

        /// create a new bank
        public static PPartnerRow CreateNewBankPartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_BANK;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestBank";

            PBankRow BankRow = AMainDS.PBank.NewRowTyped();
            BankRow.PartnerKey = PartnerRow.PartnerKey;
            BankRow.BranchName = "TestBank";
            AMainDS.PBank.Rows.Add(BankRow);

            return PartnerRow;
        }

        /// create a new BankingDetails record and a new PartnerBankingDetails record
        public static PartnerEditTDSPBankingDetailsRow CreateNewBankingRecords(long APartnerKey, PartnerEditTDS AMainDS)
        {
            PartnerEditTDSPBankingDetailsRow BankingDetailsRow = AMainDS.PBankingDetails.NewRowTyped();

            BankingDetailsRow.BankingDetailsKey = Convert.ToInt32(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_bank_details));
            BankingDetailsRow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            BankingDetailsRow.MainAccount = true;
            AMainDS.PBankingDetails.Rows.Add(BankingDetailsRow);

            PPartnerBankingDetailsRow PartnerBankingDetailsRow = AMainDS.PPartnerBankingDetails.NewRowTyped();
            PartnerBankingDetailsRow.PartnerKey = APartnerKey;
            PartnerBankingDetailsRow.BankingDetailsKey = BankingDetailsRow.BankingDetailsKey;
            AMainDS.PPartnerBankingDetails.Rows.Add(PartnerBankingDetailsRow);

            PBankingDetailsUsageRow BankingDetailsUsageRow = AMainDS.PBankingDetailsUsage.NewRowTyped();
            BankingDetailsUsageRow.PartnerKey = APartnerKey;
            BankingDetailsUsageRow.BankingDetailsKey = BankingDetailsRow.BankingDetailsKey;
            BankingDetailsUsageRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;
            AMainDS.PBankingDetailsUsage.Rows.Add(BankingDetailsUsageRow);

            return BankingDetailsRow;
        }

        /// create a new venue
        public static PPartnerRow CreateNewVenuePartner(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_VENUE;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestVenue";

            PVenueRow VenueRow = AMainDS.PVenue.NewRowTyped();
            VenueRow.PartnerKey = PartnerRow.PartnerKey;
            VenueRow.VenueCode = "TEST" + PartnerRow.PartnerKey.ToString();
            VenueRow.VenueName = "TestVenue" + PartnerRow.PartnerKey.ToString();
            AMainDS.PVenue.Rows.Add(VenueRow);

            return PartnerRow;
        }

        /// create a new family with one person
        public static void CreateFamilyWithOnePersonRecord(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewFamilyPartner(AMainDS, ADataBase);

            CreateNewLocation(PartnerRow.PartnerKey, AMainDS);

            CreateNewPerson(AMainDS,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Adam",
                "Mr",
                0,
                ADataBase);
        }

        /// create a new family with two persons
        public static void CreateFamilyWithTwoPersonRecords(PartnerEditTDS AMainDS, TDataBase ADataBase = null)
        {
            PPartnerRow PartnerRow = CreateNewFamilyPartner(AMainDS, ADataBase);

            CreateNewLocation(PartnerRow.PartnerKey, AMainDS);

            CreateNewPerson(AMainDS,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Adam",
                "Mr",
                0,
                ADataBase);
            CreateNewPerson(AMainDS,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Eve",
                "Mrs",
                1,
                ADataBase);
        }

        /// create a new location
        public static void CreateNewLocation(Int64 APartnerKey, PartnerEditTDS AMainDS)
        {
            // avoid duplicate addresses: StreetName contains the partner key
            PLocationRow LocationRow = AMainDS.PLocation.NewRowTyped();

            LocationRow.SiteKey = DomainManager.GSiteKey;
            LocationRow.LocationKey = -1;
            LocationRow.StreetName = APartnerKey.ToString() + " Nowhere Lane";
            LocationRow.PostalCode = "LO2 2CX";
            LocationRow.City = "London";
            LocationRow.CountryCode = "99";
            AMainDS.PLocation.Rows.Add(LocationRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.PartnerKey = APartnerKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        /// create new gift info
        public static AGiftBatchRow CreateNewGiftInfo(Int64 APartnerKey, ref GiftBatchTDS AGiftDS, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("CreateNewGiftInfo", ADataBase);
            bool NewTransaction;
            TDBTransaction Transaction = db.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            ALedgerAccess.LoadAll(AGiftDS, Transaction);

            AGiftDS = TGiftTransactionWebConnector.CreateAGiftBatch(AGiftDS.ALedger[0].LedgerNumber, DateTime.Today, "Test batch", ADataBase);

            // Create a new GiftBatch
            AGiftBatchRow Batch = AGiftDS.AGiftBatch[0];
            Batch.BankAccountCode = "6000";
            Batch.BatchYear = 1;
            Batch.BatchPeriod = 1;
            Batch.CurrencyCode = "EUR";
            Batch.BankCostCentre = Batch.LedgerNumber.ToString() + "00";
            Batch.LastGiftNumber = 1;
            Batch.ExchangeRateToBase = 0.5M;

            // Create a new Gift record
            AGiftRow Gift = AGiftDS.AGift.NewRowTyped();
            Gift.LedgerNumber = Batch.LedgerNumber;
            Gift.BatchNumber = Batch.BatchNumber;
            Gift.GiftTransactionNumber = 1;
            Gift.DonorKey = APartnerKey;
            AGiftDS.AGift.Rows.Add(Gift);

            // Create a new GiftDetail record
            AGiftDetailRow GiftDetail = AGiftDS.AGiftDetail.NewRowTyped();
            GiftDetail.LedgerNumber = Gift.LedgerNumber;
            GiftDetail.BatchNumber = Gift.BatchNumber;
            GiftDetail.GiftTransactionNumber = Gift.GiftTransactionNumber;
            GiftDetail.DetailNumber = 1;
            GiftDetail.MotivationGroupCode = "GIFT";
            GiftDetail.MotivationDetailCode = "SUPPORT";
            // this won't work with RecipientKey 0 anymore. see https://github.com/openpetra/openpetra/issues/183
            GiftDetail.RecipientKey = 43000000;
            GiftDetail.RecipientLedgerNumber = APartnerKey;
            GiftDetail.GiftTransactionAmount = 10;
            AGiftDS.AGiftDetail.Rows.Add(GiftDetail);

            if (NewTransaction)
            {
                  Transaction.Rollback();
            }

            return Batch;
        }

        /// create new recurring gift info
        public static ARecurringGiftBatchRow CreateNewRecurringGiftInfo(Int64 APartnerKey, ref GiftBatchTDS AGiftDS, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("CreateNewRecurringGiftInfo", ADataBase);
            bool NewTransaction;
            TDBTransaction Transaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            ALedgerAccess.LoadAll(AGiftDS, Transaction);

            AGiftDS = TGiftTransactionWebConnector.CreateARecurringGiftBatch(AGiftDS.ALedger[0].LedgerNumber, db);

            if (NewTransaction)
            {
                Transaction.Rollback();
            }

            // Create a new RecurringGiftBatch
            ARecurringGiftBatchRow Batch = AGiftDS.ARecurringGiftBatch[0];
            Batch.BankAccountCode = "6000";
            Batch.CurrencyCode = "EUR";

            // Create a new RecurringGift record
            ARecurringGiftRow RecurringGift = AGiftDS.ARecurringGift.NewRowTyped();
            RecurringGift.LedgerNumber = Batch.LedgerNumber;
            RecurringGift.BatchNumber = Batch.BatchNumber;
            RecurringGift.GiftTransactionNumber = 1;
            RecurringGift.DonorKey = APartnerKey;
            AGiftDS.ARecurringGift.Rows.Add(RecurringGift);

            // Create a new RecurringGiftDetail record
            ARecurringGiftDetailRow RecurringGiftDetail = AGiftDS.ARecurringGiftDetail.NewRowTyped();
            RecurringGiftDetail.LedgerNumber = Batch.LedgerNumber;
            RecurringGiftDetail.BatchNumber = Batch.BatchNumber;
            RecurringGiftDetail.GiftTransactionNumber = 1;
            RecurringGiftDetail.MotivationGroupCode = "GIFT";
            RecurringGiftDetail.MotivationDetailCode = "SUPPORT";
            RecurringGiftDetail.RecipientKey = 43000000;
            RecurringGiftDetail.RecipientLedgerNumber = APartnerKey;
            RecurringGiftDetail.GiftAmount = 10;
            AGiftDS.ARecurringGiftDetail.Rows.Add(RecurringGiftDetail);

            return Batch;
        }

        /// create new AP info
        public static AApDocumentRow CreateNewAPInfo(Int64 APartnerKey, ref AccountsPayableTDS AMainDS, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("CreateNewAPInfo", ADataBase);
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadAll(Transaction);

            AMainDS = TAPTransactionWebConnector.CreateAApDocument(((ALedgerRow)LedgerTable.Rows[0]).LedgerNumber, APartnerKey, true, db);

            // Create a new RecurringGiftBatch
            AApDocumentRow Document = AMainDS.AApDocument[0];
            Document.DocumentCode = "TEST";
            Document.CreditNoteFlag = false;
            Document.DateIssued = DateTime.Today;
            Document.DateEntered = DateTime.Today;
            Document.TotalAmount = 0;
            Document.CurrencyCode = "EUR";
            Document.LastDetailNumber = 0;

            // Create a new RecurringGift record
            AApSupplierRow ApSupplierRow = AMainDS.AApSupplier.NewRowTyped();
            ApSupplierRow.PartnerKey = APartnerKey;
            ApSupplierRow.CurrencyCode = "EUR";
            AMainDS.AApSupplier.Rows.Add(ApSupplierRow);

            Transaction.Commit();

            return Document;
        }

        /// create new PM data
        public static PDataLabelTable CreateNewPMData(long AFromPartnerKey, long AToPartnerKey, IndividualDataTDS AMainDS, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("CreateNewPMData", ADataBase);
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            // Create a new DataLabel record
            PDataLabelTable AllDataLabelTable = PDataLabelAccess.LoadAll(Transaction);
            PDataLabelTable DataLabelTable = new PDataLabelTable();
            PDataLabelRow DataLabelRow = DataLabelTable.NewRowTyped();

            // Get the first available key, which is our unique primary key field
            Int32 Key = 1;

            while (AllDataLabelTable.Rows.Find(new object[] { Key }) != null)
            {
                Key++;
            }

            DataLabelRow.Key = Key;
            DataLabelRow.DataType = "char";
            DataLabelTable.Rows.Add(DataLabelRow);

            // Create a new DataLabelValuePartner record
            PDataLabelValuePartnerRow DataLabelValuePartner = AMainDS.PDataLabelValuePartner.NewRowTyped();
            DataLabelValuePartner.PartnerKey = AFromPartnerKey;
            DataLabelValuePartner.DataLabelKey = DataLabelRow.Key;
            AMainDS.PDataLabelValuePartner.Rows.Add(DataLabelValuePartner);

            // Create a new PassportDetails record
            IndividualDataTDSPmPassportDetailsRow PassportDetails = AMainDS.PmPassportDetails.NewRowTyped();
            PassportDetails.PartnerKey = AFromPartnerKey;
            PassportDetails.PassportNumber = "0";
            PassportDetails.PassportNationalityName = "IRELAND";
            AMainDS.PmPassportDetails.Rows.Add(PassportDetails);

            // Create two new PersonalData records
            PmPersonalDataRow FromPersonalData = AMainDS.PmPersonalData.NewRowTyped();
            FromPersonalData.PartnerKey = AFromPartnerKey;
            FromPersonalData.HeightCm = 175;
            FromPersonalData.WeightKg = 80;
            AMainDS.PmPersonalData.Rows.Add(FromPersonalData);

            PmPersonalDataRow ToPersonalData = AMainDS.PmPersonalData.NewRowTyped();
            ToPersonalData.PartnerKey = AToPartnerKey;
            ToPersonalData.WeightKg = 95;
            AMainDS.PmPersonalData.Rows.Add(ToPersonalData);

            return DataLabelTable;
        }
    }
}
