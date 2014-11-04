//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Linq;
// generateNamespaceMap-Link-Extra-DLL System.Data.DataSetExtensions;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Verification.Exceptions;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Server.MConference.Data.Access;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// general methods for use in partner module
    /// </summary>
    public class TPartnerWebConnector
    {
        /// <summary>
        /// adds partner to list of recently used partners for given use scenario
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ANewPartner"></param>
        /// <param name="ALastPartnerUse"></param>
        /// <returns>true if action was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool AddRecentlyUsedPartner(Int64 APartnerKey, TPartnerClass APartnerClass,
            Boolean ANewPartner, TLastPartnerUse ALastPartnerUse)
        {
            bool ResultValue = false;

            ResultValue = Server.MPartner.Partner.TRecentPartnersHandling.AddRecentlyUsedPartner
                              (APartnerKey, APartnerClass, ANewPartner, ALastPartnerUse);

            return ResultValue;
        }

        /// <summary>
        /// return the location key and site key for the best address for that partner
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static TLocationPK DetermineBestAddress(Int64 APartnerKey)
        {
            return ServerCalculations.DetermineBestAddress(APartnerKey);
        }

        /// <summary>
        /// performs database changes to move person from current (old) family to new family record
        /// </summary>
        /// <param name="APersonKey"></param>
        /// <param name="AOldFamilyKey"></param>
        /// <param name="ANewFamilyKey"></param>
        /// <param name="AProblemMessage"></param>
        /// <returns>true if change of family completed successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool ChangeFamily(Int64 APersonKey, Int64 AOldFamilyKey, Int64 ANewFamilyKey,
            out String AProblemMessage)
        {
            bool ResultValue = false;

            ResultValue = Server.MPartner.Partner.TFamilyHandling.ChangeFamily(APersonKey,
                AOldFamilyKey,
                ANewFamilyKey,
                out AProblemMessage);

            return ResultValue;
        }

        /// <summary>
        /// get the correct bank partner, according to the sortcode/branchcode.
        /// if it does not exist yet, create a new bank partner with empty location
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 GetBankBySortCode(string ABranchCode)
        {
            TDBTransaction ReadTransaction = null;
            string sqlFindBankBySortCode =
                String.Format("SELECT * FROM PUB_{0} WHERE {1}=?",
                    PBankTable.GetTableDBName(),
                    PBankTable.GetBranchCodeDBName());

            OdbcParameter param = new OdbcParameter("branchcode", OdbcType.VarChar);

            param.Value = ABranchCode;
            PBankTable bank = new PBankTable();

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    DBAccess.GDBAccessObj.SelectDT(bank, sqlFindBankBySortCode, ReadTransaction, new OdbcParameter[] {
                            param
                        }, -1, -1);
                });

            if (bank.Count > 0)
            {
                return bank[0].PartnerKey;
            }
            else
            {
                // create new bank partner, with empty location
                PartnerEditTDS MainDS = new PartnerEditTDS();

                PPartnerRow newPartner = MainDS.PPartner.NewRowTyped();
                Int64 BankPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, BankPartnerKey, ref BankPartnerKey);
                newPartner.PartnerKey = BankPartnerKey;
                newPartner.PartnerShortName = "Bank " + ABranchCode;
                newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_BANK;
                MainDS.PPartner.Rows.Add(newPartner);

                PBankRow newBank = MainDS.PBank.NewRowTyped(true);
                newBank.PartnerKey = newPartner.PartnerKey;
                newBank.BranchCode = ABranchCode;
                newBank.BranchName = newPartner.PartnerShortName;
                MainDS.PBank.Rows.Add(newBank);

                PPartnerLocationRow partnerlocation = MainDS.PPartnerLocation.NewRowTyped(true);
                partnerlocation.SiteKey = DomainManager.GSiteKey;
                partnerlocation.PartnerKey = newPartner.PartnerKey;
                partnerlocation.DateEffective = DateTime.Now;
                partnerlocation.LocationType = "HOME";
                partnerlocation.SendMail = false;
                MainDS.PPartnerLocation.Rows.Add(partnerlocation);

                PartnerEditTDSAccess.SubmitChanges(MainDS);

                return newPartner.PartnerKey;
            }
        }

        /// <summary>
        /// check if partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CanPartnerBeDeleted(Int64 APartnerKey, out String ADisplayMessage)
        {
            TDBTransaction Transaction;
            Boolean NewTransaction;
            string ShortName;
            TPartnerClass PartnerClass;
            TStdPartnerStatusCode PartnerStatusCode;
            bool ResultValue = true;

            ADisplayMessage = "";

            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // check if the partner does exist in the database at all
            if (!MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, Transaction))
            {
                ResultValue = false;
                ADisplayMessage = String.Format(Catalog.GetString("Partner {0} does not exist."), APartnerKey.ToString());
            }

            // check if the partner is linked to an OpenPetra user
            if (ResultValue
                && (SUserAccess.CountViaPPartner(APartnerKey, Transaction) > 0))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete an OpenPetra user.");
            }

            // make sure we don't delete an active ledger
            if (ResultValue
                && (Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                            "SELECT COUNT(*) FROM PUB_" + ALedgerTable.GetTableDBName() +
                            " WHERE " + ALedgerTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString(),
                            Transaction)) > 0))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete an active ledger.");
            }

            // check if the partner is a gift donor or recipient
            if (ResultValue
                && ((AGiftAccess.CountViaPPartner(APartnerKey, Transaction) > 0)
                    || (AGiftDetailAccess.CountViaPPartnerRecipientKey(APartnerKey, Transaction) > 0)))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete a gift donor or recipient.");
            }

            // make sure the partner is not currently linked to a cost centre
            if (ResultValue
                && (AValidLedgerNumberAccess.CountViaPPartnerPartnerKey(APartnerKey, Transaction) > 0))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete a partner currently linked to a cost centre.");
            }

            // make sure the partner is not a supplier in the AP system
            if (ResultValue
                && (AApSupplierAccess.CountViaPPartner(APartnerKey, Transaction) > 0))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete a supplier from the AP system.");
            }

            // make sure the partner is not signed up to gift a subscription
            if (ResultValue
                && (PSubscriptionAccess.CountViaPPartnerGiftFromKey(APartnerKey, Transaction) > 0))
            {
                ResultValue = false;
                ADisplayMessage = Catalog.GetString("Unable to delete a Partner who has gifted a Subscription.");
            }

            // now check partner class specific information
            if (ResultValue)
            {
                switch (PartnerClass)
                {
                    case TPartnerClass.FAMILY:
                        ResultValue = CanFamilyPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.PERSON:
                        ResultValue = CanPersonPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.UNIT:
                        ResultValue = CanUnitPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.ORGANISATION:
                        ResultValue = CanOrganisationPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.CHURCH:
                        ResultValue = CanChurchPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.BANK:
                        ResultValue = CanBankPartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    case TPartnerClass.VENUE:
                        ResultValue = CanVenuePartnerBeDeleted(APartnerKey, Transaction, out ADisplayMessage);
                        break;

                    default:
                        break;
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// collect information to be displayed so user can confirm if this partner record should be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner was found</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool GetPartnerStatisticsForDeletion(Int64 APartnerKey, out String APartnerShortName, out String ADisplayMessage)
        {
            TDBTransaction Transaction;
            Boolean NewTransaction;
            TPartnerClass PartnerClass;
            TStdPartnerStatusCode PartnerStatusCode;
            string Linebreak = "\r\n";
            int Count;
            bool ResultValue = true;

            ADisplayMessage = "";

            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            if (MCommonMain.RetrievePartnerShortName(APartnerKey, out APartnerShortName, out PartnerClass, out PartnerStatusCode, Transaction))
            {
                ADisplayMessage = String.Format(Catalog.GetString("Are you sure you want to delete {0} {1} ?"),
                    SharedTypes.PartnerClassEnumToString(PartnerClass), APartnerShortName);
                ADisplayMessage += Linebreak + Linebreak;

                // count active subscription records
                Count = Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                        "SELECT COUNT(*) FROM PUB_" + PSubscriptionTable.GetTableDBName() +
                        " WHERE " + PSubscriptionTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() +
                        " AND " + PSubscriptionTable.GetSubscriptionStatusDBName() + " NOT IN ('CANCELLED','EXPIRED')",
                        Transaction));
                ADisplayMessage += String.Format(Catalog.GetString("{0} active Subscriptions"), Count.ToString()) + Linebreak;

                // count contact records
                Count = PPartnerContactAccess.CountViaPPartner(APartnerKey, Transaction);
                ADisplayMessage += String.Format(Catalog.GetString("{0} Contacts"), Count.ToString()) + Linebreak;

                // count relationships (consider both possible partner key fields in relationship table)
                Count = 0;
                Count = PPartnerRelationshipAccess.CountViaPPartnerPartnerKey(APartnerKey, Transaction);
                Count += PPartnerRelationshipAccess.CountViaPPartnerRelationKey(APartnerKey, Transaction);
                ADisplayMessage += String.Format(Catalog.GetString("{0} Relationships"), Count.ToString()) + Linebreak;

                if (PartnerClass == TPartnerClass.PERSON)
                {
                    // count application records
                    Count = PmGeneralApplicationAccess.CountViaPPersonPartnerKey(APartnerKey, Transaction);
                    ADisplayMessage += String.Format(Catalog.GetString("{0} Applications"), Count.ToString()) + Linebreak;

                    // count commitment records
                    Count = PmStaffDataAccess.CountViaPPerson(APartnerKey, Transaction);
                    ADisplayMessage += String.Format(Catalog.GetString("{0} Commitments"), Count.ToString()) + Linebreak;

                    // count past experience records
                    Count = PmPastExperienceAccess.CountViaPPerson(APartnerKey, Transaction);
                    ADisplayMessage += String.Format(Catalog.GetString("{0} Past Experience"), Count.ToString()) + Linebreak;
                }
            }
            else
            {
                // Partner not found
                APartnerShortName = "";
                ResultValue = false;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// delete partner record and related database entries
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if deletion was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeletePartner(Int64 APartnerKey, out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction Transaction;
            Boolean NewTransaction;
            string ShortName;
            TPartnerClass PartnerClass;
            TStdPartnerStatusCode PartnerStatusCode;
            bool ResultValue = false;

            AVerificationResult = null;
            ResultValue = true;

            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ResultValue = MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, Transaction);

                /* s_user - delete not allowed by CanPartnerBeDeleted */
                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PRecentPartnersTable.GetTableDBName(),
                        PRecentPartnersTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerGraphicTable.GetTableDBName(),
                        PPartnerGraphicTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                /* Delete extract entries before possibly attempting to delete a  */
                /* location record referenced in the extract.  Decrease key count */
                /* in m_extract_master.                                           */
                if (ResultValue)
                {
                    // first make sure that extract master tables are up to date
                    // (decrease key count by number of rows to be deleted)
                    MExtractMasterTable ExtractMasterTable;
                    MExtractMasterRow ExtractMasterRow;
                    MExtractTable ExtractTable = MExtractAccess.LoadViaPPartner(APartnerKey, Transaction);

                    foreach (DataRow Row in ExtractTable.Rows)
                    {
                        ExtractMasterTable = MExtractMasterAccess.LoadByPrimaryKey(((MExtractRow)Row).ExtractId, Transaction);
                        ExtractMasterRow = (MExtractMasterRow)ExtractMasterTable.Rows[0];
                        ExtractMasterRow.KeyCount = ExtractMasterRow.KeyCount - 1;
                        MExtractMasterAccess.SubmitChanges(ExtractMasterTable, Transaction);
                    }

                    // now delete the actual extract entries
                    ResultValue = DeleteEntries(MExtractTable.GetTableDBName(),
                        MExtractTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                // Delete Partner Location. If locations were only used by this partner then also delete location record.
                if (ResultValue)
                {
                    PPartnerLocationTable PartnerLocationTable = PPartnerLocationAccess.LoadViaPPartner(APartnerKey, Transaction);
                    PPartnerLocationTable OtherPartnerLocationTable;
                    PPartnerLocationRow PartnerLocationRow;
                    PLocationRow LocationRow;
                    PLocationTable LocationTableToDelete = new PLocationTable();

                    foreach (DataRow Row in PartnerLocationTable.Rows)
                    {
                        PartnerLocationRow = (PPartnerLocationRow)Row;
                        OtherPartnerLocationTable = PPartnerLocationAccess.LoadViaPLocation(PartnerLocationRow.SiteKey,
                            PartnerLocationRow.LocationKey,
                            Transaction);

                        // if there is only one partner left using this location (which must be this one) then delete location
                        if ((OtherPartnerLocationTable.Count == 1)
                            && (PartnerLocationRow.LocationKey != 0))
                        {
                            LocationRow = LocationTableToDelete.NewRowTyped();
                            LocationRow.SiteKey = PartnerLocationRow.SiteKey;
                            LocationRow.LocationKey = PartnerLocationRow.LocationKey;
                            LocationTableToDelete.Rows.Add(LocationRow);
                        }
                    }

                    // now first delete the partner locations
                    ResultValue = DeleteEntries(PPartnerLocationTable.GetTableDBName(),
                        PPartnerLocationTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);

                    // and now locations if they don't refer to any partners any longer
                    foreach (DataRow RowToDelete in LocationTableToDelete.Rows)
                    {
                        LocationRow = (PLocationRow)RowToDelete;
                        PLocationAccess.DeleteByPrimaryKey(LocationRow.SiteKey, LocationRow.LocationKey, Transaction);
                    }
                }

                if (ResultValue)
                {
                    TSearchCriteria[] PartnerAttributeAccessSC = new TSearchCriteria[] {
                        new TSearchCriteria(PPartnerAttributeTable.GetPartnerKeyDBName(),
                            APartnerKey)
                    };

                    if (PPartnerAttributeAccess.CountUsingTemplate(PartnerAttributeAccessSC, Transaction) > 0)
                    {
                        PPartnerAttributeAccess.DeleteUsingTemplate(PartnerAttributeAccessSC, Transaction);
                    }
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PChurchTable.GetTableDBName(),
                        PChurchTable.GetContactPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(POrganisationTable.GetTableDBName(),
                        POrganisationTable.GetContactPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PBankTable.GetTableDBName(),
                        PBankTable.GetContactPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PVenueTable.GetTableDBName(),
                        PVenueTable.GetContactPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                // Delete Partner Banking Details.
                // If Banking Details were only used by this partner then also delete Banking Details record.
                if (ResultValue)
                {
                    PPartnerBankingDetailsTable PartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPPartner(APartnerKey, Transaction);
                    PPartnerBankingDetailsTable OtherPartnerBankingDetailsTable;
                    PPartnerBankingDetailsRow PartnerBankingDetailsRow;
                    PBankingDetailsRow BankingDetailsRow;
                    PBankingDetailsTable BankingDetailsTableToDelete = new PBankingDetailsTable();

                    foreach (DataRow Row in PartnerBankingDetailsTable.Rows)
                    {
                        PartnerBankingDetailsRow = (PPartnerBankingDetailsRow)Row;
                        OtherPartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPBankingDetails(
                            PartnerBankingDetailsRow.BankingDetailsKey,
                            Transaction);

                        // if there is only one partner left using this banking details record (which must be this one) then delete banking details
                        if (OtherPartnerBankingDetailsTable.Count == 1)
                        {
                            BankingDetailsRow = BankingDetailsTableToDelete.NewRowTyped();
                            BankingDetailsRow.BankingDetailsKey = PartnerBankingDetailsRow.BankingDetailsKey;
                            BankingDetailsTableToDelete.Rows.Add(BankingDetailsRow);
                        }
                    }

                    // now first delete the partner banking details
                    ResultValue = DeleteEntries(PPartnerBankingDetailsTable.GetTableDBName(),
                        PPartnerBankingDetailsTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);

                    // and now banking details if they don't refer to any partners any longer
                    foreach (DataRow RowToDelete in BankingDetailsTableToDelete.Rows)
                    {
                        BankingDetailsRow = (PBankingDetailsRow)RowToDelete;
                        PBankingDetailsAccess.DeleteByPrimaryKey(BankingDetailsRow.BankingDetailsKey, Transaction);
                    }
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerTypeTable.GetTableDBName(),
                        PPartnerTypeTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerRelationshipTable.GetTableDBName(),
                        PPartnerRelationshipTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerRelationshipTable.GetTableDBName(),
                        PPartnerRelationshipTable.GetRelationKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PCustomisedGreetingTable.GetTableDBName(),
                        PCustomisedGreetingTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PSubscriptionTable.GetTableDBName(),
                        PSubscriptionTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                // delete reminders before contacts
                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerReminderTable.GetTableDBName(),
                        PPartnerReminderTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                var partnerContacts = MPartner.Partner.WebConnectors.TContactsWebConnector.GetPartnerContacts(APartnerKey)
                                      .AsEnumerable().Select(r => {
                        var id = r.ItemArray[PPartnerContactTable.ColumnContactLogIdId];
                        return new { ContactLogId = id, deleteThis = !TContactsWebConnector.IsContactLogAssociatedWithMoreThanOnePartner((long)id) };
                    });;

                // Delete contact attributes before deleting contacts
                if (ResultValue)
                {
                    String SqlStmt;

                    foreach (var row in partnerContacts)
                    {
                        if (row.deleteThis)
                        {
                            try
                            {
                                // build sql statement for deletion
                                SqlStmt = "DELETE FROM " + PPartnerContactAttributeTable.GetTableDBName() +
                                          " WHERE " + PPartnerContactAttributeTable.GetContactIdDBName() +
                                          " IN (SELECT " + PPartnerContactTable.GetContactLogIdDBName() +
                                          " FROM " + PPartnerContactTable.GetTableDBName() +
                                          " WHERE " + PPartnerContactTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() +
                                          " AND " + PPartnerContactTable.GetContactLogIdDBName() + " = " + row.ContactLogId + ")";

                                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, Transaction);
                            }
                            catch (Exception Exc)
                            {
                                TLogging.Log(
                                    "An Exception occured during the deletion of " + PPartnerContactAttributeTable.GetTableDBName() +
                                    " while deleting a partner: " + Environment.NewLine + Exc.ToString());

                                throw;
                            }
                        }
                    }
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerContactTable.GetTableDBName(),
                        PPartnerContactTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                // Delete any would-be orphaned ContactLog records
                if (ResultValue)
                {
                    foreach (var r in partnerContacts.Where(p => p.deleteThis))
                    {
                        PContactLogAccess.DeleteByPrimaryKey((long)r.ContactLogId, Transaction);
                    }
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(AEmailDestinationTable.GetTableDBName(),
                        AEmailDestinationTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(AMotivationDetailTable.GetTableDBName(),
                        AMotivationDetailTable.GetRecipientKeyDBName(),
                        APartnerKey, Transaction);
                }

                /* a_recurring_gift - delete not allowed by CanPartnerBeDeleted */
                /* a_recurring_gift_detail - delete not allowed by CanPartnerBeDeleted */
                /* a_gift - delete not allowed by CanPartnerBeDeleted */
                /* a_gift_detail - delete not allowed by CanPartnerBeDeleted */
                /* a_ap_supplier - delete not allowed by CanPartnerBeDeleted */

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PmDocumentTable.GetTableDBName(),
                        PmDocumentTable.GetContactPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PDataLabelValuePartnerTable.GetTableDBName(),
                        PDataLabelValuePartnerTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PDataLabelValuePartnerTable.GetTableDBName(),
                        PDataLabelValuePartnerTable.GetValuePartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PDataLabelValueApplicationTable.GetTableDBName(),
                        PDataLabelValueApplicationTable.GetValuePartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PmJobAssignmentTable.GetTableDBName(),
                        PmJobAssignmentTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PTaxTable.GetTableDBName(),
                        PTaxTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerInterestTable.GetTableDBName(),
                        PPartnerInterestTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerMergeTable.GetTableDBName(),
                        PPartnerMergeTable.GetMergeFromDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerMergeTable.GetTableDBName(),
                        PPartnerMergeTable.GetMergeToDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerGiftDestinationTable.GetTableDBName(),
                        PPartnerGiftDestinationTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerCommentTable.GetTableDBName(),
                        PPartnerCommentTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PFoundationTable.GetTableDBName(),
                        PFoundationTable.GetContactPartnerDBName(),
                        APartnerKey, Transaction);
                }

                if (ResultValue)
                {
                    ResultValue = ZeroEntries(PFoundationProposalTable.GetTableDBName(),
                        PFoundationProposalTable.GetPartnerSubmittedByDBName(),
                        APartnerKey, Transaction);
                }

                // now delete partner class specific information
                if (ResultValue)
                {
                    switch (PartnerClass)
                    {
                        case TPartnerClass.FAMILY:
                            ResultValue = DeleteFamily(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.PERSON:
                            ResultValue = DeletePerson(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.UNIT:
                            ResultValue = DeleteUnit(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.ORGANISATION:
                            ResultValue = DeleteOrganisation(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.CHURCH:
                            ResultValue = DeleteChurch(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.BANK:
                            ResultValue = DeleteBank(APartnerKey, Transaction);
                            break;

                        case TPartnerClass.VENUE:
                            ResultValue = DeleteVenue(APartnerKey, Transaction);
                            break;

                        default:
                            break;
                    }
                }

                // finally delete p_partner record itself
                if (ResultValue)
                {
                    ResultValue = DeleteEntries(PPartnerTable.GetTableDBName(),
                        PPartnerTable.GetPartnerKeyDBName(),
                        APartnerKey, Transaction);
                }

                if (NewTransaction)
                {
                    if (ResultValue)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the deletion of a Partner:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }

            return ResultValue;
        }

        /// <summary>
        /// Cancel all subscriptions that have a past expiry date and that are not cancelled yet
        /// </summary>
        /// <returns>true if cancellation was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CancelExpiredSubscriptions()
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum,
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    try
                    {
                        DBAccess.GDBAccessObj.ExecuteNonQuery("UPDATE " + PSubscriptionTable.GetTableDBName() +
                            " SET " + PSubscriptionTable.GetDateCancelledDBName() + " = DATE(NOW()), " +
                            PSubscriptionTable.GetSubscriptionStatusDBName() + " = 'EXPIRED', " +
                            PSubscriptionTable.GetReasonSubsCancelledCodeDBName() + " = 'COMPLETE' " +
                            "WHERE " + PSubscriptionTable.GetExpiryDateDBName() + " < DATE(NOW()) " +
                            "AND " + PSubscriptionTable.GetDateCancelledDBName() + " IS NULL", Transaction);

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage =
                            Catalog.GetString("Unknown error while cancelling expired subscriptions:" +
                                Environment.NewLine + Environment.NewLine + ex.ToString());
                        ErrorType = TResultSeverity.Resv_Critical;
                        VerificationResult = new TVerificationResultCollection();
                        VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                        throw new EVerificationResultsException(ErrorMessage, VerificationResult, ex.InnerException);
                    }
                });

            return SubmissionOK;
        }

        /// <summary>
        /// check if family partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanFamilyPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // make sure the family does not have any members assigned to it
            if (PPersonAccess.CountViaPFamily(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a family that still has members.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if person partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PERSONNEL")]
        private static bool CanPersonPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // cannot delete a person with a commitment
            if (PmStaffDataAccess.CountViaPPerson(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person with a commitment.");
                return false;
            }

            // cannot delete a person with documents
            if (PmDocumentAccess.CountViaPPerson(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person who still has documents set up.");
                return false;
            }

            // cannot delete a person with commitment status history (table not in use yet)
            if (PmPersonCommitmentStatusAccess.CountViaPPerson(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person with commitment status history.");
                return false;
            }

            // cannot delete a conference attendee
            if (PcAttendeeAccess.CountViaPPerson(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a conference attendee.");
                return false;
            }

            // cannot delete person if he/she is owner of a foundation
            if ((PFoundationAccess.CountViaPPartnerOwner1Key(APartnerKey, ATransaction) > 0)
                || (PFoundationAccess.CountViaPPartnerOwner2Key(APartnerKey, ATransaction) > 0))
            {
                ADisplayMessage = Catalog.GetString("Unable to delete this person because he/she is owner of a foundation.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if unit partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanUnitPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // make sure we don't delete a key ministry
            if (Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                        "SELECT COUNT(*) FROM PUB_" + PUnitTable.GetTableDBName() +
                        " WHERE " + PUnitTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString() +
                        " AND " + PUnitTable.GetUnitTypeCodeDBName() + " = 'KEY-MIN'",
                        ATransaction)) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a Key Ministry.");
                return false;
            }

            // cannot delete a unit that is primary office of a field
            if (PUnitAccess.CountViaPPartnerPrimaryOffice(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit that is the primary office of a field.");
                return false;
            }

            // cannot delete unit if it is "parent" to other partners
            if (UmUnitStructureAccess.CountViaPUnitParentUnitKey(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a parent unit.");
                return false;
            }

            // cannot delete unit if it is in the list of available sites
            if (PPartnerLedgerAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit that is in the Available Sites list.");
                return false;
            }

            // cannot delete unit if it is used as registration office
            if (PmGeneralApplicationAccess.CountViaPUnitRegistrationOffice(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete the registration office of an application.");
                return false;
            }

            // cannot delete unit if it is used as possible field of an application
            if (PmGeneralApplicationAccess.CountViaPUnitGenAppPossSrvUnitKey(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit that is the possible field of an application.");
                return false;
            }

            // cannot delete unit if it is used as confirmed event for an application
            if (PmShortTermApplicationAccess.CountViaPUnitStConfirmedOption(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a confirmed application event.");
                return false;
            }

            // cannot delete unit if it is used as current field of an event application
            if (PmShortTermApplicationAccess.CountViaPUnitStCurrentField(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit that is the current field of an event application.");
                return false;
            }

            // cannot delete unit if it is used as charged field of an event application
            if (PmShortTermApplicationAccess.CountViaPUnitStFieldCharged(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete the charged field of ane event application.");
                return false;
            }

            // cannot delete unit if it is used in a commitment as recruiting office
            if (PmStaffDataAccess.CountViaPUnitOfficeRecruitedBy(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person's recruiting field.");
                return false;
            }

            // cannot delete unit if it is used in a commitment as home office
            if (PmStaffDataAccess.CountViaPUnitHomeOffice(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person's sending field.");
                return false;
            }

            // cannot delete unit if it is used in a commitment as receiving field
            if (PmStaffDataAccess.CountViaPUnitReceivingField(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person's receiving field.");
                return false;
            }

            // cannot delete unit if it is used in a commitment as receiving field office
            if (PmStaffDataAccess.CountViaPUnitReceivingFieldOffice(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a person's receiving field office.");
                return false;
            }

            // cannot delete unit if it has jobs defined for it
            if (UmJobAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit that has jobs defined.");
                return false;
            }

            // cannot delete unit if it is being used as a conference
            if (PcConferenceAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a conference.");
                return false;
            }

            // cannot delete unit if it is being used as sending field for a conference attendee
            if (PcAttendeeAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a conference attendee's sending field.");
                return false;
            }

            // cannot delete unit if it is being used as cost authorising field for conference costs
            if (PcExtraCostAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a cost-authorising field.");
                return false;
            }

            // cannot delete unit if it a partner is interested in it
            if (PPartnerInterestAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a partner's interest field.");
                return false;
            }

            // cannot delete unit if it is a personnel's gift destination
            if (PPartnerGiftDestinationAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a partner's gift destination.");
                return false;
            }

            // cannot delete unit if it is used in partner sets
            if (PPartnerSetAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit with partner sets.");
                return false;
            }

            // cannot delete unit if it is used in partner sets
            if (PPartnerSetAccess.CountViaPUnit(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit with partner sets.");
                return false;
            }

            // cannot delete unit if it is used for foundation proposals
            if ((PFoundationProposalDetailAccess.CountViaPUnitKeyMinistryKey(APartnerKey, ATransaction) > 0)
                || (PFoundationProposalDetailAccess.CountViaPUnitAreaPartnerKey(APartnerKey, ATransaction) > 0)
                || (PFoundationProposalDetailAccess.CountViaPUnitFieldPartnerKey(APartnerKey, ATransaction) > 0))
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a unit with foundation proposals.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if organisation partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanOrganisationPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            // nothing needs to be checked for organisation at the moment
            ADisplayMessage = "";
            return true;
        }

        /// <summary>
        /// check if church partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanChurchPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // make sure this is not a supporting church
            PPartnerRelationshipTable PartnerRelationshipTemplateTable = new PPartnerRelationshipTable();
            PPartnerRelationshipRow PartnerRelationshipTemplateRow;
            StringCollection TemplateOperators;

            PartnerRelationshipTemplateRow = (PPartnerRelationshipRow)PartnerRelationshipTemplateTable.NewRow();
            PartnerRelationshipTemplateRow.PartnerKey = APartnerKey;
            PartnerRelationshipTemplateRow.RelationName = "SUPPCHURCH";
            TemplateOperators = new StringCollection();
            TemplateOperators.Add("=");

            if (PPartnerRelationshipAccess.CountUsingTemplate(PartnerRelationshipTemplateRow, TemplateOperators, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a supporting church.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if bank partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanBankPartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // make sure there are no accounts set up for this bank
            if (PBankingDetailsAccess.CountViaPBank(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a bank that has accounts set up.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if venue partner is allowed to be deleted
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADisplayMessage"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool CanVenuePartnerBeDeleted(Int64 APartnerKey, TDBTransaction ATransaction, out String ADisplayMessage)
        {
            ADisplayMessage = "";

            // make sure the venue to delete does not have buildings set up for it
            if (PcBuildingAccess.CountViaPVenue(APartnerKey, ATransaction) > 0)
            {
                ADisplayMessage = Catalog.GetString("Unable to delete a venue that has buildings set up.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// perform deletion of record with given partner key column in given table
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="APartnerKeyColumnName"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteEntries(String ATableName, String APartnerKeyColumnName, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            string SqlStmt;

            try
            {
                // build sql statement for deletion
                SqlStmt = "DELETE FROM pub_" + ATableName +
                          " WHERE " + APartnerKeyColumnName + " = " + APartnerKey.ToString();

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, ATransaction);
            }
            catch (Exception Exc)
            {
                TLogging.Log(
                    "Problem during deletion of " + ATableName + "." + APartnerKeyColumnName + " while deleting a partner: " + Environment.NewLine +
                    Exc.ToString());

                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// set entries to zero in records with given partner key column in given table
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="APartnerKeyColumnName"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if partner can be deleted</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool ZeroEntries(String ATableName, String APartnerKeyColumnName, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            string SqlStmt;

            try
            {
                // build sql statement for deletion
                SqlStmt = "UPDATE pub_" + ATableName +
                          " SET " + APartnerKeyColumnName + " = 0" +
                          " WHERE " + APartnerKeyColumnName + " = " + APartnerKey.ToString();

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, ATransaction);
            }
            catch (Exception Exc)
            {
                TLogging.Log(
                    "Problem during set to 0 of " + ATableName + "." + APartnerKeyColumnName + " while deleting a partner: " + Environment.NewLine +
                    Exc.ToString());

                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// delete family specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteFamily(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            /* p_person - delete not allowed by CanFamilyPartnerBeDeleted (using p_family_members_l) */

            ResultValue = DeleteEntries(PFamilyTable.GetTableDBName(),
                PFamilyTable.GetPartnerKeyDBName(),
                APartnerKey, ATransaction);

            return ResultValue;
        }

        /// <summary>
        /// delete person specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PERSONNEL")]
        private static bool DeletePerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            if (ResultValue)
            {
                ResultValue = ZeroEntries(PmGeneralApplicationTable.GetTableDBName(),
                    PmGeneralApplicationTable.GetPlacementPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PDataLabelValueApplicationTable.GetTableDBName(),
                    PDataLabelValueApplicationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmApplicationStatusHistoryTable.GetTableDBName(),
                    PmApplicationStatusHistoryTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmYearProgramApplicationTable.GetTableDBName(),
                    PmYearProgramApplicationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmShortTermApplicationTable.GetTableDBName(),
                    PmShortTermApplicationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmGeneralApplicationTable.GetTableDBName(),
                    PmGeneralApplicationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            /* pm_document - delete prevented by CanPersonPartnerBeDeleted */

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPassportDetailsTable.GetTableDBName(),
                    PmPassportDetailsTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPersonLanguageTable.GetTableDBName(),
                    PmPersonLanguageTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPastExperienceTable.GetTableDBName(),
                    PmPastExperienceTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPersonAbilityTable.GetTableDBName(),
                    PmPersonAbilityTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPersonQualificationTable.GetTableDBName(),
                    PmPersonQualificationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPersonalDataTable.GetTableDBName(),
                    PmPersonalDataTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmPersonEvaluationTable.GetTableDBName(),
                    PmPersonEvaluationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PmSpecialNeedTable.GetTableDBName(),
                    PmSpecialNeedTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            /* pm_staff_data: referenced by p_partner_field_of_service */
            /* pm_person_commitment_status - delete prevented by CanPersonPartnerBeDeleted */
            /* pc_attendee - delete prevented by CanPersonPartnerBeDeleted */
            /* p_foundation - delete prevented by CanPersonPartnerBeDeleted */

            if (ResultValue)
            {
                PPersonTable PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                PPersonRow PersonRow = (PPersonRow)PersonTable.Rows[0];

                // reset family members flag to false if this person is the only member of the family
                if (!PersonRow.IsFamilyKeyNull() && (PPersonAccess.CountViaPFamily(PersonRow.FamilyKey, ATransaction) == 1))
                {
                    PFamilyTable FamilyTable = PFamilyAccess.LoadByPrimaryKey(PersonRow.FamilyKey, ATransaction);
                    PFamilyRow FamilyRow = (PFamilyRow)FamilyTable.Rows[0];

                    FamilyRow.FamilyMembers = false;
                    PFamilyAccess.SubmitChanges(FamilyTable, ATransaction);
                }
            }

            // finally delete the actual p_person record
            if (ResultValue)
            {
                ResultValue = DeleteEntries(PPersonTable.GetTableDBName(),
                    PPersonTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// delete unit specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            if (ResultValue)
            {
                ResultValue = DeleteEntries(UmUnitStructureTable.GetTableDBName(),
                    UmUnitStructureTable.GetChildUnitKeyDBName(),
                    APartnerKey, ATransaction);
            }

            /* p_family - delete prevented by CanUnitPartnerBeDeleted */
            /* p_person - delete prevented by CanUnitPartnerBeDeleted */
            /* p_partner_ledger - delete prevented by CanUnitPartnerBeDeleted */
            /* pm_general_application - delete prevented by CanUnitPartnerBeDeleted */
            /* pm_short_term_application - delete prevented by CanUnitPartnerBeDeleted */
            /* pm_staff_data - delete prevented by CanUnitPartnerBeDeleted */
            /* um_job - delete prevented by CanUnitPartnerBeDeleted */

            if (ResultValue)
            {
                ResultValue = DeleteEntries(UmUnitAbilityTable.GetTableDBName(),
                    UmUnitAbilityTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(UmUnitLanguageTable.GetTableDBName(),
                    UmUnitLanguageTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(UmUnitCostTable.GetTableDBName(),
                    UmUnitCostTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(UmUnitEvaluationTable.GetTableDBName(),
                    UmUnitEvaluationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PPartnerSetPartnerTable.GetTableDBName(),
                    PPartnerSetPartnerTable.GetPartnerSetUnitKeyDBName(),
                    APartnerKey, ATransaction);
            }

            /* pc_conference  - delete prevented by CanUnitPartnerBeDeleted */
            /* pc_attendee - delete prevented by CanUnitPartnerBeDeleted */
            /* pc_extra_cost - delete prevented by CanUnitPartnerBeDeleted */
            /* p_partner_interest - delete prevented by CanUnitPartnerBeDeleted */
            /* p_partner_field_of_service - delete prevented by CanUnitPartnerBeDeleted */
            /* p_partner_set - delete prevented by CanUnitPartnerBeDeleted */
            /* p_foundation_proposal_detail - delete prevented by CanUnitPartnerBeDeleted */

            // finally delete p_unit record itself
            if (ResultValue)
            {
                ResultValue = DeleteEntries(PUnitTable.GetTableDBName(),
                    PUnitTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// delete organisation specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteOrganisation(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            // delete foundation related records: deadline, proposal detail, proposal, foundation
            if (ResultValue)
            {
                ResultValue = DeleteEntries(PFoundationDeadlineTable.GetTableDBName(),
                    PFoundationDeadlineTable.GetFoundationPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PFoundationProposalDetailTable.GetTableDBName(),
                    PFoundationProposalDetailTable.GetFoundationPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PFoundationProposalTable.GetTableDBName(),
                    PFoundationProposalTable.GetFoundationPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PFoundationTable.GetTableDBName(),
                    PFoundationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            // finally delete p_organisation record itself
            if (ResultValue)
            {
                ResultValue = DeleteEntries(POrganisationTable.GetTableDBName(),
                    POrganisationTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// delete church specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteChurch(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PChurchTable.GetTableDBName(),
                    PChurchTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// delete bank specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteBank(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            /* p_banking_details - delete not allowed by CanBankPartnerBeDeleted */

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PBankTable.GetTableDBName(),
                    PBankTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// delete venue specific information
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if deletion failed</returns>
        [RequireModulePermission("PTNRUSER")]
        private static bool DeleteVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            Boolean ResultValue = true;

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PcConferenceVenueTable.GetTableDBName(),
                    PcConferenceVenueTable.GetVenueKeyDBName(),
                    APartnerKey, ATransaction);
            }

            if (ResultValue)
            {
                ResultValue = DeleteEntries(PVenueTable.GetTableDBName(),
                    PVenueTable.GetPartnerKeyDBName(),
                    APartnerKey, ATransaction);
            }

            return ResultValue;
        }
    }
}