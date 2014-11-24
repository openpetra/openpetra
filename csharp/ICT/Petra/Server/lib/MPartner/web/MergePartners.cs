//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MSysMan;


namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TMergePartnersWebConnector
    {
        /* TODO The following tables are not yet used in OpenPetra and are not currently included in the merge yet:
         *      PmFormalEducation,
         *      PmPersonAbsence.
         * TODO The following functionalities have not yet been fully implimented:
         *      Save original merged Partner data in an XML file,
         *      Let the user select which sections they would like to merge. */


        // TODO private static StreamWriter MyWriter;

        /// <summary>
        /// returns the supplier currency for a partner if it is a supplier
        /// </summary>
        /// <param name="AFromPartnerKey"></param>
        /// <param name="AToPartnerKey"></param>
        /// <param name="AFromPartnerClass"></param>
        /// <param name="AToPartnerClass"></param>
        /// <param name="ASiteKeys">SiteKeys for any address that the user would like to include in the merge</param>
        /// <param name="ALocationKeys">LocationKeys for any address that the user would like to include in the merge</param>
        /// <param name="AMainBankingDetailsKey">BankingDetailsKey for what should be the 'Main' bank account</param>
        /// <param name="ACategories">Array determines which sections will be merged</param>
        /// <param name="ADifferentFamilies">True if two persons have been merged from different families</param>
        /// <returns>true if partner is a supplier and a currency is found</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool MergeTwoPartners(long AFromPartnerKey,
            long AToPartnerKey,
            TPartnerClass AFromPartnerClass,
            TPartnerClass AToPartnerClass,
            long[] ASiteKeys,
            int[] ALocationKeys,
            int AMainBankingDetailsKey,
            bool[] ACategories,
            ref bool ADifferentFamilies)
        {
            decimal TrackerPercent;
            int NumberOfCategories = 0;

            foreach (bool Cat in ACategories)
            {
                if (Cat)
                {
                    NumberOfCategories++;
                }
            }

            bool TaxDeductiblePercentageEnabled = Convert.ToBoolean(
                TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));

            // calculates each step's (optional and non-optional) percentage for the progress tracker
            TrackerPercent = 100 / (NumberOfCategories + 3);
            int CurrentCategory = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Merging Partners"), 100);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            /* // TODO write original data to xml file
             * string path = "../../log/OriginalRecordsBeforeMerge.xml";
             * FileStream outStream = File.Create(Path.GetFullPath(path);
             * MyWriter = new StreamWriter(outStream, Encoding.UTF8);*/

            try
            {
                if (ACategories[0] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Gift Destination"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeGiftDestination(AFromPartnerKey, AToPartnerKey, AFromPartnerClass, Transaction);
                }

                if (ACategories[1] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Gift Info"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeGiftInfo(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[1] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: AP Info"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeAPInfo(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[2] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Motivations"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeMotivations(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[3] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Extracts"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeExtracts(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[4] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Greetings"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeGreetings(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[5] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Contacts and Reminders"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeContactsAndReminders(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[6] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Interests"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeInterests(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[7] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Partner Locations"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergePartnerLocations(AFromPartnerKey, AToPartnerKey, ASiteKeys, ALocationKeys, Transaction);
                }

                if (ACategories[8] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Types"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergePartnerTypes(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[9] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Subscriptions"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeSubscriptions(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[10] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Applications"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeApplications(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[11] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Personnel Data"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergePMData(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[12] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Jobs"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeJobs(AFromPartnerKey, AToPartnerKey, AFromPartnerClass, Transaction);
                }

                // merge Partner class records
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Partner") + " (" +
                    AFromPartnerClass.ToString() + ")", TrackerPercent * CurrentCategory);
                CurrentCategory++;

                if (AFromPartnerClass == TPartnerClass.UNIT)
                {
                    if (AToPartnerClass == TPartnerClass.UNIT)
                    {
                        MergeUnit(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.CHURCH)
                {
                    if (AToPartnerClass == TPartnerClass.ORGANISATION)
                    {
                        MergeChurchToOrganisation(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.FAMILY)
                    {
                        MergeChurchToFamily(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.CHURCH)
                    {
                        MergeChurch(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.VENUE)
                {
                    if (AToPartnerClass == TPartnerClass.VENUE)
                    {
                        MergeVenue(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.FAMILY)
                {
                    if (AToPartnerClass == TPartnerClass.ORGANISATION)
                    {
                        MergeFamilyToOrganisation(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.CHURCH)
                    {
                        MergeFamilyToChurch(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.FAMILY)
                    {
                        MergeFamily(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.PERSON)
                {
                    if (AToPartnerClass == TPartnerClass.PERSON)
                    {
                        MergePerson(AFromPartnerKey, AToPartnerKey, ref ADifferentFamilies, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.ORGANISATION)
                {
                    if (AToPartnerClass == TPartnerClass.CHURCH)
                    {
                        MergeOrganisationToChurch(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.FAMILY)
                    {
                        MergeOrganisationToFamily(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.BANK)
                    {
                        MergeOrganisationToBank(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.ORGANISATION)
                    {
                        MergeOrganisation(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.BANK)
                {
                    if (AToPartnerClass == TPartnerClass.ORGANISATION)
                    {
                        MergeBankToOrganisation(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else if (AToPartnerClass == TPartnerClass.BANK)
                    {
                        MergeBank(AFromPartnerKey, AToPartnerKey, Transaction);
                    }
                    else
                    {
                        throw new Exception("Selected Partner Classes cannot be merged!");
                    }
                }

                if (ACategories[13] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Relationships"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeRelationships(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                // merge PPartner records
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Partner (Common)"),
                    TrackerPercent * CurrentCategory);
                CurrentCategory++;

                MergePartner(AFromPartnerKey, AToPartnerKey, Transaction);

                if (ACategories[14] == true)
                {
                    if (AFromPartnerClass == TPartnerClass.VENUE)
                    {
                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Merging: Venue - Buildings, Rooms and Allocations"), TrackerPercent * CurrentCategory);

                        MergeBuildingsAndRooms(AFromPartnerKey, AToPartnerKey, Transaction);
                    }

                    CurrentCategory++;
                }

                if (ACategories[15] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Partner - Bank Accounts"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeBankAccounts(AFromPartnerKey, AToPartnerKey, AMainBankingDetailsKey, Transaction);
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Partner - Bank Accounts"),
                    TrackerPercent * CurrentCategory);
                CurrentCategory++;

                MergeRecentAndLastPartnerInfo(AFromPartnerKey, AToPartnerKey, Transaction);

                if (ACategories[16] == true)
                {
                    if (TaxDeductiblePercentageEnabled
                        && (((AFromPartnerClass == TPartnerClass.FAMILY) && (AToPartnerClass == TPartnerClass.FAMILY))
                            || ((AFromPartnerClass == TPartnerClass.UNIT) && (AToPartnerClass == TPartnerClass.UNIT))))
                    {
                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Merging: Tax Deductibility Percentage"),
                            TrackerPercent * CurrentCategory);

                        MergeTaxDeductibilityPercentage(AFromPartnerKey, AToPartnerKey, Transaction);
                    }

                    CurrentCategory++;
                }

                if (ACategories[17] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Link to Cost Centre"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeLinkToCostCentre(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (ACategories[18] == true)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Merging: Graphic"),
                        TrackerPercent * CurrentCategory);
                    CurrentCategory++;

                    MergeGraphic(AFromPartnerKey, AToPartnerKey, Transaction);
                }

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw new Exception("Partner merge was cancelled by the user");
                }

                DBAccess.GDBAccessObj.CommitTransaction();

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                //TODO MyWriter.Close();
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();
                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                // TODO MyWriter.Close();
                return false;
            }
            finally
            {
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.MergeTwoPartners: rollback own transaction.");
            }

            return true;
        }

        private static void MergeGiftInfo(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            AGiftTable GiftTable = AGiftAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (GiftTable.Rows.Count > 0)
            {
                //TODO GiftTable.WriteXml(MyWriter);

                foreach (DataRow Row in GiftTable.Rows)
                {
                    ((AGiftRow)Row).DonorKey = AToPartnerKey;
                }

                AGiftAccess.SubmitChanges(GiftTable, ATransaction);
            }

            AGiftDetailTable GiftDetailTable = AGiftDetailAccess.LoadViaPPartnerRecipientKey(AFromPartnerKey, ATransaction);

            if (GiftDetailTable.Rows.Count > 0)
            {
                foreach (DataRow Row in GiftDetailTable.Rows)
                {
                    ((AGiftDetailRow)Row).RecipientKey = AToPartnerKey;
                }

                AGiftDetailAccess.SubmitChanges(GiftDetailTable, ATransaction);
            }

            GiftDetailTable = AGiftDetailAccess.LoadViaPPartnerRecipientLedgerNumber(AFromPartnerKey, ATransaction);

            if (GiftDetailTable.Rows.Count > 0)
            {
                foreach (DataRow Row in GiftDetailTable.Rows)
                {
                    ((AGiftDetailRow)Row).RecipientLedgerNumber = AToPartnerKey;
                }

                AGiftDetailAccess.SubmitChanges(GiftDetailTable, ATransaction);
            }

            ARecurringGiftTable RecurringGiftTable = ARecurringGiftAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (RecurringGiftTable.Rows.Count > 0)
            {
                foreach (DataRow Row in RecurringGiftTable.Rows)
                {
                    ((ARecurringGiftRow)Row).DonorKey = AToPartnerKey;
                }

                ARecurringGiftAccess.SubmitChanges(RecurringGiftTable, ATransaction);
            }

            ARecurringGiftDetailTable RecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaPPartnerRecipientKey(AFromPartnerKey, ATransaction);

            if (RecurringGiftDetailTable.Rows.Count > 0)
            {
                foreach (DataRow Row in RecurringGiftDetailTable.Rows)
                {
                    ((ARecurringGiftDetailRow)Row).RecipientKey = AToPartnerKey;
                }

                ARecurringGiftDetailAccess.SubmitChanges(RecurringGiftDetailTable, ATransaction);
            }

            RecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaPPartnerRecipientLedgerNumber(AFromPartnerKey, ATransaction);

            if (RecurringGiftDetailTable.Rows.Count > 0)
            {
                foreach (DataRow Row in RecurringGiftDetailTable.Rows)
                {
                    ((ARecurringGiftDetailRow)Row).RecipientLedgerNumber = AToPartnerKey;
                }

                ARecurringGiftDetailAccess.SubmitChanges(RecurringGiftDetailTable, ATransaction);
            }
        }

        private static void MergeAPInfo(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            bool MoveSupplier = false;

            // If the To partner is not a supplier, but the from one is, we need to move the supplier record to the to-partner.
            if (AApSupplierAccess.Exists(AToPartnerKey, ATransaction) == false)
            {
                AApSupplierTable SupplierTable = AApSupplierAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

                if (SupplierTable.Rows.Count > 0)
                {
                    AApSupplierRow NewRow = SupplierTable.NewRowTyped(false);

                    // Create new record (by cloning the old record) and keep old record to avoid constraint errors.
                    // Old record will be deleted later.
                    object[] FromRowArray = SupplierTable.Rows[0].ItemArray;
                    object[] FromRowArrayClone = (object[])FromRowArray.Clone();
                    NewRow.ItemArray = FromRowArrayClone;
                    NewRow.PartnerKey = AToPartnerKey;

                    SupplierTable.Rows.Add(NewRow);
                    AApSupplierAccess.SubmitChanges(SupplierTable, ATransaction);

                    MoveSupplier = true;
                }
            }

            // Loop through all invoices for the from supplier, and move them to the To supplier.  If there is none nothing will happen
            // (this also applies if the from partner is not a supplier)
            AApDocumentTable ApDocumentTable = AApDocumentAccess.LoadViaAApSupplier(AFromPartnerKey, ATransaction);

            if (ApDocumentTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ApDocumentTable.Rows)
                {
                    // If the invoice code already exists for the new supplier, then make it something different. <InvNo>_merged maybe?
                    AApDocumentTable ApDocumentTable2 = AApDocumentAccess.LoadViaAApSupplier(AToPartnerKey, ATransaction);

                    if (ApDocumentTable2.Rows.Count > 0)
                    {
                        foreach (DataRow Row2 in ApDocumentTable2.Rows)
                        {
                            if (((AApDocumentRow)Row2).DocumentCode == ((AApDocumentRow)Row).DocumentCode)
                            {
                                ((AApDocumentRow)Row).DocumentCode = ((AApDocumentRow)Row).DocumentCode + "_merged";
                            }
                        }

                        AApDocumentAccess.SubmitChanges(ApDocumentTable2, ATransaction);
                    }

                    ((AApDocumentRow)Row).PartnerKey = AToPartnerKey;

                    AApDocumentAccess.SubmitChanges(ApDocumentTable, ATransaction);
                }
            }

            // delete From supplier record if it was moved to the To Partner
            if (MoveSupplier)
            {
                AApSupplierTable SupplierTable = AApSupplierAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

                if (SupplierTable.Rows.Count > 0)
                {
                    SupplierTable.Rows[0].Delete();

                    AApSupplierAccess.SubmitChanges(SupplierTable, ATransaction);
                }
            }
        }

        private static void MergeMotivations(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            AMotivationDetailTable MotivationDetailTable = AMotivationDetailAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (MotivationDetailTable.Rows.Count > 0)
            {
                foreach (DataRow Row in MotivationDetailTable.Rows)
                {
                    ((AMotivationDetailRow)Row).RecipientKey = AToPartnerKey;
                }

                AMotivationDetailAccess.SubmitChanges(MotivationDetailTable, ATransaction);
            }
        }

        private static void MergeExtracts(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            MExtractTable FromExtractTable = MExtractAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            MExtractTable ToExtractTable = MExtractAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            if (FromExtractTable.Rows.Count > 0)
            {
                foreach (DataRow Row in FromExtractTable.Rows)
                {
                    foreach (DataRow Row2 in ToExtractTable.Rows)
                    {
                        // if Partner already in this Extract
                        if (((MExtractRow)Row2).ExtractId == ((MExtractRow)Row).ExtractId)
                        {
                            MExtractMasterTable ExtractMasterTable = MExtractMasterAccess.LoadByPrimaryKey(((MExtractRow)Row2).ExtractId,
                                ATransaction);

                            if (ExtractMasterTable.Rows.Count > 0)
                            {
                                // Reduce the number of people in the Extract by 1
                                ((MExtractMasterRow)ExtractMasterTable.Rows[0]).KeyCount -= 1;
                                MExtractMasterAccess.SubmitChanges(ExtractMasterTable, ATransaction);
                            }

                            Row.Delete();
                        }
                        else
                        {
                            ((MExtractRow)Row).PartnerKey = AToPartnerKey;
                        }
                    }
                }

                MExtractAccess.SubmitChanges(FromExtractTable, ATransaction);
            }
        }

        private static void MergeGreetings(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PCustomisedGreetingTable FromCustomisedGreetingTable = PCustomisedGreetingAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            PCustomisedGreetingTable ToCustomisedGreetingTable = PCustomisedGreetingAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            if (FromCustomisedGreetingTable.Rows.Count > 0)
            {
                foreach (DataRow Row in FromCustomisedGreetingTable.Rows)
                {
                    // if Partner already has a customised greeting for this user
                    if (ToCustomisedGreetingTable.Rows.Find(new object[] { AToPartnerKey, ((PCustomisedGreetingRow)Row).UserId }) != null)
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((PCustomisedGreetingRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                PCustomisedGreetingAccess.SubmitChanges(FromCustomisedGreetingTable, ATransaction);
            }
        }

        private static void MergeContactsAndReminders(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerReminderTable ReminderTable = PPartnerReminderAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in ReminderTable.Rows)
            {
                ((PPartnerReminderRow)Row).PartnerKey = AToPartnerKey;
            }

            PPartnerContactTable ContactTable = PPartnerContactAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in ContactTable.Rows)
            {
                ((PPartnerContactRow)Row).PartnerKey = AToPartnerKey;
            }

            PPartnerReminderAccess.SubmitChanges(ReminderTable, ATransaction);
            PPartnerContactAccess.SubmitChanges(ContactTable, ATransaction);
        }

        private static void MergeInterests(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerInterestTable InterestTable = PPartnerInterestAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (InterestTable.Rows.Count > 0)
            {
                foreach (DataRow Row in InterestTable.Rows)
                {
                    ((PPartnerInterestRow)Row).PartnerKey = AToPartnerKey;
                }

                PPartnerInterestAccess.SubmitChanges(InterestTable, ATransaction);
            }
        }

        private static void MergePartnerLocations(long AFromPartnerKey,
            long AToPartnerKey,
            long[] ASiteKeys,
            int[] ALocationKeys,
            TDBTransaction ATransaction)
        {
            PPartnerLocationTable FromLocationTable = PPartnerLocationAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            PPartnerLocationTable ToLocationTable = PPartnerLocationAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            if (FromLocationTable.Rows.Count > 0)
            {
                for (int Key = 0; Key < ALocationKeys.Length; Key++)
                {
                    DataRow Row = FromLocationTable.Rows.Find(new object[] { AFromPartnerKey, ASiteKeys[Key], ALocationKeys[Key] });

                    if (Row != null)
                    {
                        PPartnerLocationRow ToRow = (PPartnerLocationRow)ToLocationTable.Rows.Find(new object[] { AToPartnerKey, ASiteKeys[Key],
                                                                                                                  ALocationKeys[Key] });

                        // if To Partner has the same location - combine locations and delete the From PartnerLocation
                        if (ToRow != null)
                        {
                            PPartnerLocationRow FromRow = (PPartnerLocationRow)Row;

                            if (ToRow.DateEffective == null)
                            {
                                ToRow.DateEffective = FromRow.DateEffective;
                            }

                            if (ToRow.DateGoodUntil == null)
                            {
                                ToRow.DateGoodUntil = FromRow.DateGoodUntil;
                            }

                            if (ToRow.EmailAddress == "")
                            {
                                ToRow.EmailAddress = FromRow.EmailAddress;
                            }

                            if (ToRow.Extension == 0)
                            {
                                ToRow.Extension = FromRow.Extension;
                            }

                            if (ToRow.FaxExtension == 0)
                            {
                                ToRow.FaxExtension = FromRow.FaxExtension;
                            }

                            if (ToRow.FaxNumber == "")
                            {
                                ToRow.FaxNumber = FromRow.FaxNumber;
                            }

                            if (ToRow.LocationDetailComment == "")
                            {
                                ToRow.LocationDetailComment = FromRow.LocationDetailComment;
                            }

                            if (ToRow.LocationType == "")
                            {
                                ToRow.LocationType = FromRow.LocationType;
                            }

                            if (ToRow.SendMail == false)
                            {
                                ToRow.SendMail = FromRow.SendMail;
                            }

                            if (ToRow.TelephoneNumber == "")
                            {
                                ToRow.TelephoneNumber = FromRow.TelephoneNumber;
                            }

                            if (ToRow.Telex == 0)
                            {
                                ToRow.Telex = FromRow.Telex;
                            }

                            if (ToRow.CreatedBy == "")
                            {
                                ToRow.CreatedBy = FromRow.CreatedBy;
                            }

                            if (ToRow.DateCreated == null)
                            {
                                ToRow.DateCreated = FromRow.DateCreated;
                            }

                            if (ToRow.DateModified == null)
                            {
                                ToRow.DateModified = FromRow.DateModified;
                            }

                            if (ToRow.ModifiedBy == "")
                            {
                                ToRow.ModifiedBy = FromRow.ModifiedBy;
                            }

                            if (ToRow.AlternateTelephone == "")
                            {
                                ToRow.AlternateTelephone = FromRow.AlternateTelephone;
                            }

                            if (ToRow.MobileNumber == "")
                            {
                                ToRow.MobileNumber = FromRow.MobileNumber;
                            }

                            if (ToRow.Url == "")
                            {
                                ToRow.Url = FromRow.Url;
                            }

                            Row.Delete();
                        }
                        else
                        {
                            ((PPartnerLocationRow)Row).PartnerKey = AToPartnerKey;
                        }
                    }
                }

                PLocationTable LocationTable = PLocationAccess.LoadAll(ATransaction);

                // delete the unselected PPartnerLocations and PLocations if not in use elsewhere
                foreach (DataRow Row in FromLocationTable.Rows)
                {
                    if ((Row.RowState != DataRowState.Modified) && (Row.RowState != DataRowState.Deleted))
                    {
                        bool DeleteLocationRecord = true;

                        PPartnerLocationTable PartnerLocationTable = PPartnerLocationAccess.LoadViaPLocation(((PPartnerLocationRow)Row).SiteKey,
                            ((PPartnerLocationRow)Row).LocationKey, ATransaction);

                        // keep PLocation if used by another partner
                        if (PartnerLocationTable.Rows.Count > 1)
                        {
                            DeleteLocationRecord = false;
                        }

                        if (DeleteLocationRecord)
                        {
                            // keep PLocation if used by an extract
                            MExtractTable ExtractTable =
                                MExtractAccess.LoadViaPLocation(((PPartnerLocationRow)Row).SiteKey,
                                    ((PPartnerLocationRow)Row).LocationKey,
                                    ATransaction);

                            if (ExtractTable.Rows.Count != 0)
                            {
                                DeleteLocationRecord = false;
                            }
                        }

                        // delete unused PLocation
                        if (DeleteLocationRecord)
                        {
                            DataRow ToDelete = LocationTable.Rows.Find(new object[] { ((PPartnerLocationRow)Row).SiteKey,
                                                                                      ((PPartnerLocationRow)Row).LocationKey });
                            ToDelete.Delete();
                        }

                        // delete unselected PPartnerLocation
                        Row.Delete();
                    }
                }

                PPartnerLocationAccess.SubmitChanges(FromLocationTable, ATransaction);
                PPartnerLocationAccess.SubmitChanges(ToLocationTable, ATransaction);
                LocationTable.ThrowAwayAfterSubmitChanges = true;
                PLocationAccess.SubmitChanges(LocationTable, ATransaction);
            }
        }

        private static void MergePartnerTypes(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerTypeTable FromTypeTable = PPartnerTypeAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (FromTypeTable.Rows.Count > 0)
            {
                foreach (DataRow Row in FromTypeTable.Rows)
                {
                    // if Partner already has this type - delete
                    if (PPartnerTypeAccess.Exists(AToPartnerKey, ((PPartnerTypeRow)Row).TypeCode, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((PPartnerTypeRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                PPartnerTypeAccess.SubmitChanges(FromTypeTable, ATransaction);
            }
        }

        private static void MergeSubscriptions(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PSubscriptionTable FromSubscriptionTable = PSubscriptionAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            PSubscriptionTable ToSubscriptionTable = PSubscriptionAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            if (FromSubscriptionTable.Rows.Count > 0)
            {
                foreach (DataRow Row in FromSubscriptionTable.Rows)
                {
                    PSubscriptionRow FromRow = (PSubscriptionRow)Row;
                    PSubscriptionRow ToRow = (PSubscriptionRow)ToSubscriptionTable.Rows.Find(new object[] { FromRow.PublicationCode, AToPartnerKey });

                    // if both Partners contain the same subscription
                    if (ToRow != null)
                    {
                        // if To Partner's subscription is current or From Partner's subscription is cancelled - delete From Partner's subscription
                        if ((ToRow.DateCancelled == null) || (FromRow.DateCancelled != null))
                        {
                            FromRow.Delete();
                        }
                        // if To Partner's subscription is cancelled or From Partner's subscription is current - delete To Partner's subscription
                        else
                        {
                            ToRow.Delete();
                            FromRow.PartnerKey = AToPartnerKey;
                        }
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PSubscriptionAccess.SubmitChanges(ToSubscriptionTable, ATransaction);
                PSubscriptionAccess.SubmitChanges(FromSubscriptionTable, ATransaction);
            }
        }

        private static void MergeApplications(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PmGeneralApplicationTable FromApplicationTable = PmGeneralApplicationAccess.LoadViaPPersonPartnerKey(AFromPartnerKey, ATransaction);
            PmGeneralApplicationTable NewApplicationTable = new PmGeneralApplicationTable();

            if (FromApplicationTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromApplicationTable.Rows)
                {
                    PmGeneralApplicationRow NewRow = NewApplicationTable.NewRowTyped();
                    PmGeneralApplicationRow FromApplicationRow = (PmGeneralApplicationRow)FromRow;
                    DateTime AppDate = FromApplicationRow.GenAppDate;

                    // Create new record (by cloning the old record) and keep old record to avoid constraint errors.
                    // Old record will be deleted later.
                    object[] FromRowArray = FromApplicationRow.ItemArray;
                    object[] FromRowArrayClone = (object[])FromRowArray.Clone();
                    NewRow.ItemArray = FromRowArrayClone;
                    NewRow.PartnerKey = AToPartnerKey;

                    /*  If a general app record already exists in the "merged into" partner that has the same unique index consisting of
                     *  p_partner_key_n, pm_gen_app_date_d, pt_app_type_name_c, pm_old_link_c then change the "app date" (go backwards) field
                     *  so that this unique index is not violated. This will rarely happen but is possible. It will be the case if the two
                     *  partners would have an application for the same event at the same day.  */
                    PmGeneralApplicationTable ToApplicationTable = PmGeneralApplicationAccess.LoadByUniqueKey(AToPartnerKey, AppDate,
                        FromApplicationRow.AppTypeName, FromApplicationRow.OldLink, ATransaction);

                    AppDate = AppDate.AddDays(-ToApplicationTable.Rows.Count);

                    //  the application date needs to be reset in the short term or long term application as well (depending on the type)
                    if (AppDate != FromApplicationRow.GenAppDate)
                    {
                        PmShortTermApplicationTable STAppTable = PmShortTermApplicationAccess.LoadByPrimaryKey(
                            AFromPartnerKey, FromApplicationRow.ApplicationKey, FromApplicationRow.RegistrationOffice, ATransaction);

                        if (STAppTable.Rows.Count != 0)
                        {
                            ((PmShortTermApplicationRow)STAppTable.Rows[0]).StAppDate = AppDate;
                            PmShortTermApplicationAccess.SubmitChanges(STAppTable, ATransaction);
                        }
                        else
                        {
                            PmYearProgramApplicationTable YPAppTable = PmYearProgramApplicationAccess.LoadByPrimaryKey(
                                AFromPartnerKey, FromApplicationRow.ApplicationKey, FromApplicationRow.RegistrationOffice, ATransaction);

                            if (YPAppTable.Rows.Count != 0)
                            {
                                ((PmYearProgramApplicationRow)YPAppTable.Rows[0]).YpAppDate = AppDate;
                                PmYearProgramApplicationAccess.SubmitChanges(YPAppTable, ATransaction);
                            }
                        }
                    }

                    NewRow.GenAppDate = AppDate;

                    NewApplicationTable.Rows.Add(NewRow);
                }

                // submit new records (will delete old records later)
                PmGeneralApplicationAccess.SubmitChanges(NewApplicationTable, ATransaction);
            }

            PmApplicationFileTable FileTable = PmApplicationFileAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in FileTable.Rows)
            {
                ((PmApplicationFileRow)Row).PartnerKey = AToPartnerKey;
            }

            PmApplicationFileAccess.SubmitChanges(FileTable, ATransaction);

            PmYearProgramApplicationTable YPApplicationTable = PmYearProgramApplicationAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in YPApplicationTable.Rows)
            {
                ((PmYearProgramApplicationRow)Row).PartnerKey = AToPartnerKey;
            }

            PmYearProgramApplicationAccess.SubmitChanges(YPApplicationTable, ATransaction);

            PmShortTermApplicationTable STApplicationTable = PmShortTermApplicationAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in STApplicationTable.Rows)
            {
                ((PmShortTermApplicationRow)Row).PartnerKey = AToPartnerKey;
            }

            PmShortTermApplicationAccess.SubmitChanges(STApplicationTable, ATransaction);

            // delete old records
            foreach (DataRow Row in FromApplicationTable.Rows)
            {
                Row.Delete();
            }

            PmGeneralApplicationAccess.SubmitChanges(FromApplicationTable, ATransaction);
        }

        private static void MergePMData(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            // *** OFFICE SPECIFIC DATA ***

            PDataLabelValuePartnerTable DataLabelValuePartnerTable = PDataLabelValuePartnerAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey,
                ATransaction);

            if (DataLabelValuePartnerTable.Rows.Count > 0)
            {
                foreach (DataRow Row in DataLabelValuePartnerTable.Rows)
                {
                    PDataLabelValuePartnerRow FromRow = (PDataLabelValuePartnerRow)Row;

                    //  If there is a data label for the To Partner already then do not overwrite it. It should only exist if it has a value
                    //  and then we don't want to overwrite it with the merge.
                    if (PDataLabelValuePartnerAccess.Exists(AToPartnerKey, FromRow.DataLabelKey, ATransaction) == false)
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PDataLabelValuePartnerAccess.SubmitChanges(DataLabelValuePartnerTable, ATransaction);
            }

            PDataLabelValueApplicationTable DataLabelValueApplicationTable = PDataLabelValueApplicationAccess.LoadViaPPartner(AFromPartnerKey,
                ATransaction);

            if (DataLabelValueApplicationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in DataLabelValueApplicationTable.Rows)
                {
                    PDataLabelValueApplicationRow FromRow = (PDataLabelValueApplicationRow)Row;

                    //  If there is a data label for the To Partner already then do not overwrite it. It should only exist if it has a value
                    //  and then we don't want to overwrite it with the merge.
                    if (PDataLabelValueApplicationAccess.Exists(AToPartnerKey, FromRow.ApplicationKey, FromRow.RegistrationOffice,
                            FromRow.DataLabelKey, ATransaction) == false)
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PDataLabelValueApplicationAccess.SubmitChanges(DataLabelValueApplicationTable, ATransaction);
            }

            // *** PASSPORT ***

            PmPassportDetailsTable PassportDetailsTable = PmPassportDetailsAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PassportDetailsTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PassportDetailsTable.Rows)
                {
                    if (PmPassportDetailsAccess.Exists(AToPartnerKey, ((PmPassportDetailsRow)Row).PassportNumber, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((PmPassportDetailsRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                PmPassportDetailsAccess.SubmitChanges(PassportDetailsTable, ATransaction);
            }

            // *** DOCUMENTS ***

            PmDocumentTable DocumentTable = PmDocumentAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (DocumentTable.Rows.Count > 0)
            {
                foreach (DataRow Row in DocumentTable.Rows)
                {
                    ((PmDocumentRow)Row).PartnerKey = AToPartnerKey;
                }

                PmDocumentAccess.SubmitChanges(DocumentTable, ATransaction);
            }

            // *** PAST EXPERIENCE ***

            PmPastExperienceTable PastExperienceTable = PmPastExperienceAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PastExperienceTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PastExperienceTable.Rows)
                {
                    PmPastExperienceRow FromRow = (PmPastExperienceRow)Row;

                    // avoids System.NullReferenceException for blank dates
                    DateTime StartDate = DateTime.MinValue;
                    DateTime EndDate = DateTime.MinValue;

                    if (FromRow.StartDate != null)
                    {
                        StartDate = (DateTime)FromRow.StartDate;
                    }

                    if (FromRow.EndDate != null)
                    {
                        EndDate = (DateTime)FromRow.EndDate;
                    }

                    if (PmPastExperienceAccess.ExistsUniqueKey(PmPastExperienceTable.TableId,
                            new object[] { AToPartnerKey, StartDate, EndDate, FromRow.PrevLocation },
                            ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PmPastExperienceAccess.SubmitChanges(PastExperienceTable, ATransaction);
            }

            // *** ABILITY ***

            PmPersonAbilityTable PersonAbilityTable = PmPersonAbilityAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PersonAbilityTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PersonAbilityTable.Rows)
                {
                    if (PmPersonAbilityAccess.Exists(AToPartnerKey, ((PmPersonAbilityRow)Row).AbilityAreaName, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((PmPersonAbilityRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                PmPersonAbilityAccess.SubmitChanges(PersonAbilityTable, ATransaction);
            }

            // *** PERSON EVALUATION ***

            PmPersonEvaluationTable PersonEvaluationTable = PmPersonEvaluationAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PersonEvaluationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PersonEvaluationTable.Rows)
                {
                    PmPersonEvaluationRow FromRow = (PmPersonEvaluationRow)Row;

                    if (PmPersonEvaluationAccess.Exists(AToPartnerKey, FromRow.EvaluationDate, FromRow.Evaluator, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PmPersonEvaluationAccess.SubmitChanges(PersonEvaluationTable, ATransaction);
            }

            // *** LANGUAGE ***

            PmPersonLanguageTable PersonLanguageTable = PmPersonLanguageAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PersonLanguageTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PersonLanguageTable.Rows)
                {
                    PmPersonLanguageRow FromRow = (PmPersonLanguageRow)Row;

                    if (PmPersonLanguageAccess.Exists(AToPartnerKey, FromRow.LanguageCode, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PmPersonLanguageAccess.SubmitChanges(PersonLanguageTable, ATransaction);
            }

            // *** QUALIFICATION ***

            PmPersonQualificationTable PersonQualificationTable = PmPersonQualificationAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (PersonQualificationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PersonQualificationTable.Rows)
                {
                    PmPersonQualificationRow FromRow = (PmPersonQualificationRow)Row;

                    if (PmPersonQualificationAccess.Exists(AToPartnerKey, FromRow.QualificationAreaName, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PmPersonQualificationAccess.SubmitChanges(PersonQualificationTable, ATransaction);
            }

            // *** PERSONAL DATA ***

            PmPersonalDataTable FromPersonalDataTable = PmPersonalDataAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (FromPersonalDataTable.Rows.Count > 0)
            {
                PmPersonalDataRow FromRow = (PmPersonalDataRow)FromPersonalDataTable.Rows[0];

                PmPersonalDataTable ToPersonalDataTable = PmPersonalDataAccess.LoadViaPPerson(AToPartnerKey, ATransaction);

                if (ToPersonalDataTable.Rows.Count > 0)
                {
                    PmPersonalDataRow ToRow = (PmPersonalDataRow)ToPersonalDataTable.Rows[0];

                    if ((ToRow.IsHeightCmNull() || (ToRow.HeightCm == 0)) && !FromRow.IsHeightCmNull())
                    {
                        ToRow.HeightCm = FromRow.HeightCm;
                    }

                    if ((ToRow.IsWeightKgNull() || (ToRow.WeightKg == 0)) && !FromRow.IsWeightKgNull())
                    {
                        ToRow.WeightKg = FromRow.WeightKg;
                    }

                    if (ToRow.EyeColour == "")
                    {
                        ToRow.EyeColour = FromRow.EyeColour;
                    }

                    if (ToRow.HairColour == "")
                    {
                        ToRow.HairColour = FromRow.HairColour;
                    }

                    if (ToRow.FacialHair == "")
                    {
                        ToRow.FacialHair = FromRow.FacialHair;
                    }

                    if (ToRow.PhysicalDesc == "")
                    {
                        ToRow.PhysicalDesc = FromRow.PhysicalDesc;
                    }

                    if (ToRow.BloodType == "")
                    {
                        ToRow.BloodType = FromRow.BloodType;
                    }

                    if (ToRow.EthnicOrigin == "")
                    {
                        ToRow.EthnicOrigin = FromRow.EthnicOrigin;
                    }

                    if (ToRow.LifeQuestion1 == "")
                    {
                        ToRow.LifeQuestion1 = FromRow.LifeQuestion1;
                    }

                    if (ToRow.LifeAnswer1 == "")
                    {
                        ToRow.LifeAnswer1 = FromRow.LifeAnswer1;
                    }

                    if (ToRow.LifeQuestion2 == "")
                    {
                        ToRow.LifeQuestion2 = FromRow.LifeQuestion2;
                    }

                    if (ToRow.LifeAnswer2 == "")
                    {
                        ToRow.LifeAnswer2 = FromRow.LifeAnswer2;
                    }

                    if (ToRow.LifeQuestion3 == "")
                    {
                        ToRow.LifeQuestion3 = FromRow.LifeQuestion3;
                    }

                    if (ToRow.LifeAnswer3 == "")
                    {
                        ToRow.LifeAnswer3 = FromRow.LifeAnswer3;
                    }

                    if (ToRow.LifeQuestion4 == "")
                    {
                        ToRow.LifeQuestion4 = FromRow.LifeQuestion4;
                    }

                    if (ToRow.LifeAnswer4 == "")
                    {
                        ToRow.LifeAnswer4 = FromRow.LifeAnswer4;
                    }

                    if ((ToRow.LanguageCode == "") || (ToRow.LanguageCode == "99"))
                    {
                        ToRow.LanguageCode = FromRow.LanguageCode;
                    }

                    if ((ToRow.IsBelieverSinceYearNull() || (ToRow.BelieverSinceYear == 0)) && !FromRow.IsBelieverSinceYearNull())
                    {
                        ToRow.BelieverSinceYear = FromRow.BelieverSinceYear;
                    }

                    ToRow.BelieverSinceComment = ToRow.BelieverSinceComment + " / " + FromRow.BelieverSinceComment;
                }
                else
                {
                    FromRow.PartnerKey = AToPartnerKey;
                }

                PmPersonalDataAccess.SubmitChanges(FromPersonalDataTable, ATransaction);
                PmPersonalDataAccess.SubmitChanges(ToPersonalDataTable, ATransaction);
            }

            // *** SPECIAL NEED ***

            PmSpecialNeedTable FromSpecialNeedTable = PmSpecialNeedAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (FromSpecialNeedTable.Rows.Count > 0)
            {
                PmSpecialNeedRow FromRow = (PmSpecialNeedRow)FromSpecialNeedTable.Rows[0];

                PmSpecialNeedTable ToSpecialNeedTable = PmSpecialNeedAccess.LoadViaPPerson(AToPartnerKey, ATransaction);

                if (ToSpecialNeedTable.Rows.Count > 0)
                {
                    PmSpecialNeedRow ToRow = (PmSpecialNeedRow)ToSpecialNeedTable.Rows[0];

                    if (FromRow.MedicalComment != ToRow.MedicalComment)
                    {
                        ToRow.MedicalComment = ToRow.MedicalComment + "  /  " + FromRow.MedicalComment;
                    }

                    if (FromRow.DietaryComment != ToRow.DietaryComment)
                    {
                        ToRow.DietaryComment = ToRow.DietaryComment + "  /  " + FromRow.DietaryComment;
                    }

                    if (FromRow.OtherSpecialNeed != ToRow.OtherSpecialNeed)
                    {
                        ToRow.OtherSpecialNeed = ToRow.OtherSpecialNeed + "  /  " + FromRow.OtherSpecialNeed;
                    }

                    if (FromRow.VegetarianFlag == false)
                    {
                        ToRow.VegetarianFlag = ToRow.VegetarianFlag;
                    }

                    FromRow.Delete();
                }
                else
                {
                    FromRow.PartnerKey = AToPartnerKey;
                }

                PmSpecialNeedAccess.SubmitChanges(FromSpecialNeedTable, ATransaction);
                PmSpecialNeedAccess.SubmitChanges(ToSpecialNeedTable, ATransaction);
            }

            // *** STAFF DATA ***

            PmStaffDataTable StaffDataTable = PmStaffDataAccess.LoadViaPPerson(AFromPartnerKey, ATransaction);

            if (StaffDataTable.Rows.Count > 0)
            {
                foreach (DataRow Row in StaffDataTable.Rows)
                {
                    ((PmStaffDataRow)Row).PartnerKey = AToPartnerKey;
                }

                PmStaffDataAccess.SubmitChanges(StaffDataTable, ATransaction);
            }
        }

        private static void MergeJobs(long AFromPartnerKey, long AToPartnerKey, TPartnerClass AFromPartnerClass, TDBTransaction ATransaction)
        {
            // *** JOB ASSIGNMENT (PERSON) ***

            PmJobAssignmentTable FromJobAssignmentTable = PmJobAssignmentAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            PmJobAssignmentTable ToJobAssignmentTable = PmJobAssignmentAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            if (FromJobAssignmentTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromJobAssignmentTable.Rows)
                {
                    PmJobAssignmentRow FromJobAssignmentRow = (PmJobAssignmentRow)FromRow;

                    bool ToJobAssignmentFound = false;

                    foreach (DataRow ToRow in ToJobAssignmentTable.Rows)
                    {
                        PmJobAssignmentRow ToJobAssignmentRow = (PmJobAssignmentRow)ToRow;

                        if ((ToJobAssignmentRow.FromDate == FromJobAssignmentRow.FromDate)
                            && (ToJobAssignmentRow.PositionName == FromJobAssignmentRow.PositionName)
                            && (ToJobAssignmentRow.PositionScope == FromJobAssignmentRow.PositionScope))
                        {
                            ToJobAssignmentFound = true;
                            break;
                        }
                    }

                    if (ToJobAssignmentFound)
                    {
                        FromRow.Delete();
                    }
                    else
                    {
                        FromJobAssignmentRow.PartnerKey = AToPartnerKey;
                    }
                }

                PmJobAssignmentAccess.SubmitChanges(FromJobAssignmentTable, ATransaction);
            }

            // The rest of this method only applies to Unit Partners so return if Partner is not a Unit
            if (AFromPartnerClass != TPartnerClass.UNIT)
            {
                return;
            }

            // *** JOB (UNIT) ***

            UmJobTable FromJobTable = UmJobAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);
            UmJobTable ToJobTable = UmJobAccess.LoadViaPUnit(AToPartnerKey, ATransaction);

            if (FromJobTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromJobTable.Rows)
                {
                    UmJobRow FromJobRow = (UmJobRow)FromRow;

                    bool ToJobFound = false;

                    foreach (DataRow ToRow in ToJobTable.Rows)
                    {
                        UmJobRow ToJobRow = (UmJobRow)ToRow;

                        if ((ToJobRow.PositionName == FromJobRow.PositionName)
                            && (ToJobRow.PositionScope == FromJobRow.PositionScope))
                        {
                            ToJobFound = true;
                            break;
                        }
                    }

                    if (ToJobFound)
                    {
                        FromRow.Delete();
                    }
                    else
                    {
                        FromJobRow.UnitKey = AToPartnerKey;
                    }
                }

                UmJobAccess.SubmitChanges(FromJobTable, ATransaction);
            }

            // *** JOB ABILITY (UNIT) ***

            UmJobRequirementTable FromJobRequirementTable = UmJobRequirementAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);
            UmJobRequirementTable ToJobRequirementTable = UmJobRequirementAccess.LoadViaPUnit(AToPartnerKey, ATransaction);

            if (FromJobRequirementTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromJobRequirementTable.Rows)
                {
                    UmJobRequirementRow FromJobRequirementRow = (UmJobRequirementRow)FromRow;

                    bool ToJobRequirementFound = false;

                    foreach (DataRow ToRow in ToJobRequirementTable.Rows)
                    {
                        UmJobRequirementRow ToJobRequirementRow = (UmJobRequirementRow)ToRow;

                        if ((ToJobRequirementRow.PositionName == FromJobRequirementRow.PositionName)
                            && (ToJobRequirementRow.PositionScope == FromJobRequirementRow.PositionScope)
                            && (ToJobRequirementRow.AbilityAreaName == FromJobRequirementRow.AbilityAreaName))
                        {
                            ToJobRequirementFound = true;
                            break;
                        }
                    }

                    if (ToJobRequirementFound)
                    {
                        FromRow.Delete();
                    }
                    else
                    {
                        FromJobRequirementRow.UnitKey = AToPartnerKey;
                    }
                }

                UmJobRequirementAccess.SubmitChanges(FromJobRequirementTable, ATransaction);
            }

            // *** JOB LANGUAGE (UNIT) ***

            UmJobLanguageTable FromJobLanguageTable = UmJobLanguageAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);
            UmJobLanguageTable ToJobLanguageTable = UmJobLanguageAccess.LoadViaPUnit(AToPartnerKey, ATransaction);

            if (FromJobLanguageTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromJobLanguageTable.Rows)
                {
                    UmJobLanguageRow FromJobLanguageRow = (UmJobLanguageRow)FromRow;

                    bool ToJobLanguageFound = false;

                    foreach (DataRow ToRow in ToJobLanguageTable.Rows)
                    {
                        UmJobLanguageRow ToJobLanguageRow = (UmJobLanguageRow)ToRow;

                        if ((ToJobLanguageRow.PositionName == FromJobLanguageRow.PositionName)
                            && (ToJobLanguageRow.PositionScope == FromJobLanguageRow.PositionScope)
                            && (ToJobLanguageRow.LanguageCode == FromJobLanguageRow.LanguageCode))
                        {
                            ToJobLanguageFound = true;
                            break;
                        }
                    }

                    if (ToJobLanguageFound)
                    {
                        FromRow.Delete();
                    }
                    else
                    {
                        FromJobLanguageRow.UnitKey = AToPartnerKey;
                    }
                }

                UmJobLanguageAccess.SubmitChanges(FromJobLanguageTable, ATransaction);
            }

            // *** JOB Qualification (UNIT) ***

            UmJobQualificationTable FromJobQualificationTable = UmJobQualificationAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);
            UmJobQualificationTable ToJobQualificationTable = UmJobQualificationAccess.LoadViaPUnit(AToPartnerKey, ATransaction);

            if (FromJobQualificationTable.Rows.Count > 0)
            {
                foreach (DataRow FromRow in FromJobQualificationTable.Rows)
                {
                    UmJobQualificationRow FromJobQualificationRow = (UmJobQualificationRow)FromRow;

                    bool ToJobQualificationFound = false;

                    foreach (DataRow ToRow in ToJobQualificationTable.Rows)
                    {
                        UmJobQualificationRow ToJobQualificationRow = (UmJobQualificationRow)ToRow;

                        if ((ToJobQualificationRow.PositionName == FromJobQualificationRow.PositionName)
                            && (ToJobQualificationRow.PositionScope == FromJobQualificationRow.PositionScope))
                        {
                            ToJobQualificationFound = true;
                            break;
                        }
                    }

                    if (ToJobQualificationFound)
                    {
                        FromRow.Delete();
                    }
                    else
                    {
                        FromJobQualificationRow.UnitKey = AToPartnerKey;
                    }
                }

                UmJobQualificationAccess.SubmitChanges(FromJobQualificationTable, ATransaction);
            }
        }

        private static void MergeUnit(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PUnitTable FromUnitTable = PUnitAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            PUnitTable ToUnitTable = PUnitAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            PUnitRow FromUnitRow = (PUnitRow)FromUnitTable.Rows[0];
            PUnitRow ToUnitRow = (PUnitRow)ToUnitTable.Rows[0];

            if (ToUnitRow.UnitName == "")
            {
                ToUnitRow.UnitName = FromUnitRow.UnitName;
            }

            ToUnitRow.Description = ToUnitRow.Description + " / " + FromUnitRow.Description;

            if (ToUnitRow.UnitTypeCode == "")
            {
                ToUnitRow.UnitTypeCode = FromUnitRow.UnitTypeCode;
            }

            ToUnitRow.Minimum = ToUnitRow.Minimum + FromUnitRow.Minimum;
            ToUnitRow.Maximum = ToUnitRow.Maximum + FromUnitRow.Maximum;
            ToUnitRow.Present = ToUnitRow.Present + FromUnitRow.Present;
            ToUnitRow.PartTimers = ToUnitRow.PartTimers + FromUnitRow.PartTimers;

            if (ToUnitRow.OutreachCode == "")
            {
                ToUnitRow.OutreachCode = FromUnitRow.OutreachCode;
            }

            if (ToUnitRow.OutreachCost == 0)
            {
                ToUnitRow.OutreachCost = FromUnitRow.OutreachCost;
                ToUnitRow.OutreachCostCurrencyCode = FromUnitRow.OutreachCostCurrencyCode;
            }

            if (ToUnitRow.OutreachCostCurrencyCode == "")
            {
                ToUnitRow.OutreachCostCurrencyCode = FromUnitRow.OutreachCostCurrencyCode;
            }

            if ((ToUnitRow.CountryCode == "") || (ToUnitRow.CountryCode == "99"))
            {
                ToUnitRow.CountryCode = FromUnitRow.CountryCode;
            }

            if (ToUnitRow.PrimaryOffice == 0)
            {
                ToUnitRow.PrimaryOffice = FromUnitRow.PrimaryOffice;
            }

            PUnitAccess.SubmitChanges(ToUnitTable, ATransaction);

            // ** deal with various references to p_unit

            PmStaffDataTable OfficeRecruitedTable = PmStaffDataAccess.LoadViaPUnitOfficeRecruitedBy(AFromPartnerKey, ATransaction);

            if (OfficeRecruitedTable.Rows.Count > 0)
            {
                foreach (DataRow Row in OfficeRecruitedTable.Rows)
                {
                    ((PmStaffDataRow)Row).OfficeRecruitedBy = AToPartnerKey;
                }

                PmStaffDataAccess.SubmitChanges(OfficeRecruitedTable, ATransaction);
            }

            PmStaffDataTable HomeOfficeTable = PmStaffDataAccess.LoadViaPUnitHomeOffice(AFromPartnerKey, ATransaction);

            if (HomeOfficeTable.Rows.Count > 0)
            {
                foreach (DataRow Row in HomeOfficeTable.Rows)
                {
                    ((PmStaffDataRow)Row).HomeOffice = AToPartnerKey;
                }

                PmStaffDataAccess.SubmitChanges(HomeOfficeTable, ATransaction);
            }

            PmStaffDataTable ReceivingFieldTable = PmStaffDataAccess.LoadViaPUnitReceivingField(AFromPartnerKey, ATransaction);

            if (ReceivingFieldTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ReceivingFieldTable.Rows)
                {
                    ((PmStaffDataRow)Row).ReceivingField = AToPartnerKey;
                }

                PmStaffDataAccess.SubmitChanges(ReceivingFieldTable, ATransaction);
            }

            PmStaffDataTable ReceivingFieldOfficeTable = PmStaffDataAccess.LoadViaPUnitReceivingFieldOffice(AFromPartnerKey, ATransaction);

            if (ReceivingFieldOfficeTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ReceivingFieldOfficeTable.Rows)
                {
                    ((PmStaffDataRow)Row).ReceivingFieldOffice = AToPartnerKey;
                }

                PmStaffDataAccess.SubmitChanges(ReceivingFieldOfficeTable, ATransaction);
            }

            PmJobAssignmentTable JobAssignmentTable = PmJobAssignmentAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (JobAssignmentTable.Rows.Count > 0)
            {
                foreach (DataRow Row in JobAssignmentTable.Rows)
                {
                    ((PmJobAssignmentRow)Row).UnitKey = AToPartnerKey;
                }

                PmJobAssignmentAccess.SubmitChanges(JobAssignmentTable, ATransaction);
            }

            PPartnerGiftDestinationTable GiftDestinationTable = PPartnerGiftDestinationAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (GiftDestinationTable.Rows.Count > 0)
            {
                foreach (PPartnerGiftDestinationRow Row in GiftDestinationTable.Rows)
                {
                    Row.FieldKey = AToPartnerKey;
                }

                PPartnerGiftDestinationAccess.SubmitChanges(GiftDestinationTable, ATransaction);
            }

            PmGeneralApplicationTable GeneralApplicationTable = PmGeneralApplicationAccess.LoadViaPUnitRegistrationOffice(AFromPartnerKey,
                ATransaction);

            if (GeneralApplicationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in GeneralApplicationTable.Rows)
                {
                    ((PmGeneralApplicationRow)Row).RegistrationOffice = AToPartnerKey;
                }

                PmGeneralApplicationAccess.SubmitChanges(GeneralApplicationTable, ATransaction);
            }

            PmGeneralApplicationTable GeneralApplication2Table = PmGeneralApplicationAccess.LoadViaPUnitGenAppPossSrvUnitKey(AFromPartnerKey,
                ATransaction);

            if (GeneralApplication2Table.Rows.Count > 0)
            {
                foreach (DataRow Row in GeneralApplication2Table.Rows)
                {
                    ((PmGeneralApplicationRow)Row).GenAppPossSrvUnitKey = AToPartnerKey;
                }

                PmGeneralApplicationAccess.SubmitChanges(GeneralApplication2Table, ATransaction);
            }

            PmShortTermApplicationTable ShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPUnitStConfirmedOption(AFromPartnerKey,
                ATransaction);

            if (ShortTermApplicationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ShortTermApplicationTable.Rows)
                {
                    ((PmShortTermApplicationRow)Row).StConfirmedOption = AToPartnerKey;
                }

                PmShortTermApplicationAccess.SubmitChanges(ShortTermApplicationTable, ATransaction);
            }

            PmShortTermApplicationTable ShortTermApplication2Table = PmShortTermApplicationAccess.LoadViaPUnitStCurrentField(AFromPartnerKey,
                ATransaction);

            if (ShortTermApplication2Table.Rows.Count > 0)
            {
                foreach (DataRow Row in ShortTermApplication2Table.Rows)
                {
                    ((PmShortTermApplicationRow)Row).StCurrentField = AToPartnerKey;
                }

                PmShortTermApplicationAccess.SubmitChanges(ShortTermApplication2Table, ATransaction);
            }

            PmShortTermApplicationTable ShortTermApplication3Table = PmShortTermApplicationAccess.LoadViaPUnitRegistrationOffice(AFromPartnerKey,
                ATransaction);

            if (ShortTermApplication3Table.Rows.Count > 0)
            {
                foreach (DataRow Row in ShortTermApplication3Table.Rows)
                {
                    ((PmShortTermApplicationRow)Row).RegistrationOffice = AToPartnerKey;
                }

                PmShortTermApplicationAccess.SubmitChanges(ShortTermApplication3Table, ATransaction);
            }

            PmShortTermApplicationTable ShortTermApplication4Table = PmShortTermApplicationAccess.LoadViaPUnitStFieldCharged(AFromPartnerKey,
                ATransaction);

            if (ShortTermApplication4Table.Rows.Count > 0)
            {
                foreach (DataRow Row in ShortTermApplication4Table.Rows)
                {
                    ((PmShortTermApplicationRow)Row).StFieldCharged = AToPartnerKey;
                }

                PmShortTermApplicationAccess.SubmitChanges(ShortTermApplication4Table, ATransaction);
            }

            PcExtraCostTable ExtraCostTable = PcExtraCostAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (ExtraCostTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ExtraCostTable.Rows)
                {
                    ((PcExtraCostRow)Row).AuthorisingField = AToPartnerKey;
                }

                PcExtraCostAccess.SubmitChanges(ExtraCostTable, ATransaction);
            }

            // *** UNIT ABILITY ***

            UmUnitAbilityTable UnitAbilityTable = UmUnitAbilityAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (UnitAbilityTable.Rows.Count > 0)
            {
                foreach (DataRow Row in UnitAbilityTable.Rows)
                {
                    if (UmUnitAbilityAccess.Exists(AToPartnerKey, ((UmUnitAbilityRow)Row).AbilityAreaName, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((UmUnitAbilityRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                UmUnitAbilityAccess.SubmitChanges(UnitAbilityTable, ATransaction);
            }

            // *** UNIT COSTS ***

            UmUnitCostTable UnitCostTable = UmUnitCostAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (UnitCostTable.Rows.Count > 0)
            {
                foreach (DataRow Row in UnitCostTable.Rows)
                {
                    if (UmUnitCostAccess.Exists(AToPartnerKey, ((UmUnitCostRow)Row).ValidFromDate, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        ((UmUnitCostRow)Row).PartnerKey = AToPartnerKey;
                    }
                }

                UmUnitCostAccess.SubmitChanges(UnitCostTable, ATransaction);
            }

            // *** UNIT EVALUATION ***

            UmUnitEvaluationTable UnitEvaluationTable = UmUnitEvaluationAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (UnitEvaluationTable.Rows.Count > 0)
            {
                foreach (DataRow Row in UnitEvaluationTable.Rows)
                {
                    UmUnitEvaluationRow FromRow = (UmUnitEvaluationRow)Row;

                    if (UmUnitEvaluationAccess.Exists(AToPartnerKey, FromRow.DateOfEvaluation, FromRow.EvaluationNumber, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                UmUnitEvaluationAccess.SubmitChanges(UnitEvaluationTable, ATransaction);
            }

            // *** UNIT LANGUAGE ***

            UmUnitLanguageTable UnitLanguageTable = UmUnitLanguageAccess.LoadViaPUnit(AFromPartnerKey, ATransaction);

            if (UnitLanguageTable.Rows.Count > 0)
            {
                foreach (DataRow Row in UnitLanguageTable.Rows)
                {
                    UmUnitLanguageRow FromRow = (UmUnitLanguageRow)Row;

                    if (UmUnitLanguageAccess.Exists(AToPartnerKey, FromRow.LanguageCode, FromRow.LanguageLevel, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                UmUnitLanguageAccess.SubmitChanges(UnitLanguageTable, ATransaction);
            }

            // *** UNIT Structure (Parent) ***
        }

        private static void MergeChurchToOrganisation(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PChurchTable ChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            POrganisationTable OrganisationTable = POrganisationAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            PChurchRow FromRow = (PChurchRow)ChurchTable.Rows[0];
            POrganisationRow ToRow = (POrganisationRow)OrganisationTable.Rows[0];

            if (ToRow.OrganisationName == "")
            {
                ToRow.OrganisationName = FromRow.ChurchName;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            POrganisationAccess.SubmitChanges(OrganisationTable, ATransaction);
        }

        private static void MergeChurchToFamily(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PChurchTable ChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            PFamilyTable FamilyTable = PFamilyAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            PChurchRow FromRow = (PChurchRow)ChurchTable.Rows[0];
            PFamilyRow ToRow = (PFamilyRow)FamilyTable.Rows[0];

            if (ToRow.FamilyName == "")
            {
                ToRow.FamilyName = FromRow.ChurchName;
            }

            PFamilyAccess.SubmitChanges(FamilyTable, ATransaction);
        }

        private static void MergeChurch(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PChurchTable FromChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            PChurchTable ToChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            PChurchRow FromRow = (PChurchRow)FromChurchTable.Rows[0];
            PChurchRow ToRow = (PChurchRow)ToChurchTable.Rows[0];

            if (ToRow.ChurchName == "")
            {
                ToRow.ChurchName = FromRow.ChurchName;
            }

            if (!ToRow.Accomodation && FromRow.Accomodation)
            {
                ToRow.Accomodation = true;
                ToRow.AccomodationSize = FromRow.AccomodationSize;
                ToRow.AccomodationType = FromRow.AccomodationType;
            }

            if (ToRow.ApproximateSize == 0)
            {
                ToRow.ApproximateSize = FromRow.ApproximateSize;
            }

            if ((ToRow.DenominationCode == "") || (ToRow.DenominationCode == "UNKNOWN"))
            {
                ToRow.DenominationCode = FromRow.DenominationCode;
            }

            if (ToRow.PrayerGroup == false)
            {
                ToRow.PrayerGroup = FromRow.PrayerGroup;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            PChurchAccess.SubmitChanges(ToChurchTable, ATransaction);
        }

        private static void MergeVenue(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PVenueTable FromVenueTable = PVenueAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            PVenueTable ToVenueTable = PVenueAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            PVenueRow FromRow = (PVenueRow)FromVenueTable.Rows[0];
            PVenueRow ToRow = (PVenueRow)ToVenueTable.Rows[0];

            if (ToRow.VenueName == "")
            {
                ToRow.VenueName = FromRow.VenueName;
            }

            if (ToRow.VenueCode == "")
            {
                ToRow.VenueCode = 'V' + AToPartnerKey.ToString().Substring(1);
            }

            if (ToRow.CurrencyCode == "")
            {
                ToRow.CurrencyCode = FromRow.CurrencyCode;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.PartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            PVenueAccess.SubmitChanges(ToVenueTable, ATransaction);
        }

        private static void MergeFamilyToOrganisation(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            POrganisationTable OrganisationTable = POrganisationAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            if (((POrganisationRow)OrganisationTable.Rows[0]).OrganisationName == "")
            {
                ((POrganisationRow)OrganisationTable.Rows[0]).OrganisationName = ((PPartnerRow)PartnerTable.Rows[0]).PartnerShortName;

                POrganisationAccess.SubmitChanges(OrganisationTable, ATransaction);
            }
        }

        private static void MergeFamilyToChurch(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PChurchTable ChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            if (((PChurchRow)ChurchTable.Rows[0]).ChurchName == "")
            {
                ((PChurchRow)ChurchTable.Rows[0]).ChurchName = ((PPartnerRow)PartnerTable.Rows[0]).PartnerShortName;

                PChurchAccess.SubmitChanges(ChurchTable, ATransaction);
            }
        }

        private static void MergeFamily(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PFamilyTable FromFamilyTable = PFamilyAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            PFamilyTable ToFamilyTable = PFamilyAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            PFamilyRow FromRow = (PFamilyRow)FromFamilyTable.Rows[0];
            PFamilyRow ToRow = (PFamilyRow)ToFamilyTable.Rows[0];

            if (ToRow.Title == "")
            {
                ToRow.Title = FromRow.Title;
            }

            if (ToRow.FamilyName == "")
            {
                ToRow.FamilyName = FromRow.FamilyName;
            }

            if (ToRow.FirstName == "")
            {
                ToRow.FirstName = FromRow.FirstName;
            }

            if (!ToRow.DifferentSurnames && FromRow.DifferentSurnames)
            {
                ToRow.DifferentSurnames = true;
            }

            if ((ToRow.MaritalStatus == "") || (ToRow.MaritalStatus == "U"))
            {
                ToRow.MaritalStatus = FromRow.MaritalStatus;
            }

            if (ToRow.MaritalStatusSince == null)
            {
                ToRow.MaritalStatusSince = FromRow.MaritalStatusSince;
            }

            ToRow.MaritalStatusComment = ToRow.MaritalStatusComment + " / " + FromRow.MaritalStatusComment;

            // Update family keys in all p_person records
            PPersonTable PersonTable = PPersonAccess.LoadViaPFamily(AFromPartnerKey, ATransaction);

            if (PersonTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PersonTable.Rows)
                {
                    PPersonRow PersonRow = (PPersonRow)Row;

                    // Get available FamilyID
                    int FamilyID;
                    string ProblemMessage;
                    TPartnerFamilyIDHandling FamilyIDHandling = new TPartnerFamilyIDHandling();
                    FamilyIDHandling.GetNewFamilyID(AToPartnerKey, out FamilyID, out ProblemMessage);

                    PersonRow.FamilyKey = AToPartnerKey;
                    PersonRow.FamilyId = FamilyID;

                    PPersonAccess.SubmitChanges(PersonTable, ATransaction);
                }

                /* The p_family_members_l flag was sometimes not set correctly in the past. For this reason we don't want to copy it over as before,
                 * but determine it here anew to make sure it correctly reflects the situation after merging the Families */
                ToRow.FamilyMembers = true;
            }
            else
            {
                ToRow.FamilyMembers = false;
            }

            FromRow.FamilyMembers = false;

            PFamilyAccess.SubmitChanges(ToFamilyTable, ATransaction);
            PFamilyAccess.SubmitChanges(FromFamilyTable, ATransaction);
        }

        private static void MergePerson(long AFromPartnerKey, long AToPartnerKey, ref bool ADifferentFamilies, TDBTransaction ATransaction)
        {
            PPersonTable FromPersonTable = PPersonAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PPersonTable ToPersonTable = PPersonAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction);

            PPersonRow FromRow = (PPersonRow)FromPersonTable.Rows[0];
            PPersonRow ToRow = (PPersonRow)ToPersonTable.Rows[0];

            if (ToRow.Title == "")
            {
                ToRow.Title = FromRow.Title;
            }

            if (ToRow.AcademicTitle == "")
            {
                ToRow.AcademicTitle = FromRow.AcademicTitle;
            }

            if (ToRow.Decorations == "")
            {
                ToRow.Decorations = FromRow.Decorations;
            }

            if (ToRow.FamilyName == "")
            {
                ToRow.FamilyName = FromRow.FamilyName;
            }

            if (ToRow.FirstName == "")
            {
                ToRow.FirstName = FromRow.FirstName;
            }

            if (ToRow.MiddleName1 == "")
            {
                ToRow.MiddleName1 = FromRow.MiddleName1;
            }

            if (ToRow.MiddleName2 == "")
            {
                ToRow.MiddleName2 = FromRow.MiddleName2;
            }

            if (ToRow.MiddleName3 == "")
            {
                ToRow.MiddleName3 = FromRow.MiddleName3;
            }

            if (ToRow.PreferedName == "")
            {
                ToRow.PreferedName = FromRow.PreferedName;
            }

            if (ToRow.Gender == "UNKNOWN")
            {
                ToRow.Gender = FromRow.Gender;
            }

            if (ToRow.DateOfBirth == null)
            {
                ToRow.DateOfBirth = FromRow.DateOfBirth;
            }

            if ((ToRow.MaritalStatus == "") || (ToRow.MaritalStatus == "U"))
            {
                ToRow.MaritalStatus = FromRow.MaritalStatus;
            }

            if ((ToRow.OccupationCode == "") || (ToRow.OccupationCode == "UNKNOWN"))
            {
                ToRow.OccupationCode = FromRow.OccupationCode;
            }

            if (ToRow.MaritalStatusSince == null)
            {
                ToRow.MaritalStatusSince = FromRow.MaritalStatusSince;
            }

            ToRow.MaritalStatusComment = ToRow.MaritalStatusComment + " / " + FromRow.MaritalStatusComment;

            PPersonAccess.SubmitChanges(ToPersonTable, ATransaction);

            // Handle family assignments and family relationships if From Partner and To Partner are not in the same family
            if (FromRow.FamilyKey != ToRow.FamilyKey)
            {
                ADifferentFamilies = true;

                // Look if there are no members of the family left, except for the merged person
                if (!PPersonAccess.ExistsUniqueKey(PPersonTable.TableId, new object[] { FromRow.FamilyKey, FromRow.FamilyId }, ATransaction))
                {
                    // Record that there are no people in this family
                    PFamilyTable FamilyTable = PFamilyAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);

                    ((PFamilyRow)FamilyTable.Rows[0]).FamilyMembers = false;

                    PFamilyAccess.SubmitChanges(FamilyTable, ATransaction);
                }

                // Delete FAMILY relationship records
                PPartnerRelationshipTable RelationshipTable = PPartnerRelationshipAccess.LoadByPrimaryKey(
                    FromRow.FamilyKey, "FAMILY", AFromPartnerKey, ATransaction);

                if (RelationshipTable.Rows.Count > 0)
                {
                    foreach (DataRow Row in RelationshipTable.Rows)
                    {
                        Row.Delete();
                    }

                    PPartnerRelationshipAccess.SubmitChanges(RelationshipTable, ATransaction);
                }
            }

            FromRow.SetFamilyKeyNull();
            FromRow.FamilyId = 0;

            PPersonAccess.SubmitChanges(FromPersonTable, ATransaction);
        }

        private static void MergeOrganisationToChurch(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            POrganisationTable OrganisationTable = POrganisationAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PChurchTable ChurchTable = PChurchAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            POrganisationRow FromRow = (POrganisationRow)OrganisationTable.Rows[0];
            PChurchRow ToRow = (PChurchRow)ChurchTable.Rows[0];

            if (ToRow.ChurchName == "")
            {
                ToRow.ChurchName = FromRow.OrganisationName;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            PChurchAccess.SubmitChanges(ChurchTable, ATransaction);
        }

        private static void MergeOrganisationToFamily(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            POrganisationTable OrganisationTable = POrganisationAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PFamilyTable FamilyTable = PFamilyAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction);

            POrganisationRow FromRow = (POrganisationRow)OrganisationTable.Rows[0];
            PFamilyRow ToRow = (PFamilyRow)FamilyTable.Rows[0];

            if (ToRow.FamilyName == "")
            {
                ToRow.FamilyName = FromRow.OrganisationName;
            }

            PFamilyAccess.SubmitChanges(FamilyTable, ATransaction);
        }

        private static void MergeOrganisationToBank(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            POrganisationTable OrganisationTable = POrganisationAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PBankTable BankTable = PBankAccess.LoadViaPPartnerPartnerKey(AToPartnerKey, ATransaction);

            POrganisationRow FromRow = (POrganisationRow)OrganisationTable.Rows[0];
            PBankRow ToRow = (PBankRow)BankTable.Rows[0];

            if (ToRow.BranchName == "")
            {
                ToRow.BranchName = FromRow.OrganisationName;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            PBankAccess.SubmitChanges(BankTable, ATransaction);
        }

        private static void MergeOrganisation(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            POrganisationTable FromOrganisationTable = POrganisationAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            POrganisationTable ToOrganisationTable = POrganisationAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);

            POrganisationRow FromRow = (POrganisationRow)FromOrganisationTable.Rows[0];
            POrganisationRow ToRow = (POrganisationRow)ToOrganisationTable.Rows[0];

            if ((ToRow.BusinessCode == "") || (ToRow.BusinessCode == "UNKNOWN"))
            {
                ToRow.BusinessCode = FromRow.BusinessCode;
            }

            if (ToRow.Religious == false)
            {
                ToRow.Religious = FromRow.Religious;
            }

            if (ToRow.OrganisationName == "")
            {
                ToRow.OrganisationName = FromRow.OrganisationName;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            POrganisationAccess.SubmitChanges(ToOrganisationTable, ATransaction);
        }

        private static void MergeBankToOrganisation(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PBankTable BankTable = PBankAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);
            POrganisationTable OrganisationTable = POrganisationAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction);

            PBankRow FromRow = (PBankRow)BankTable.Rows[0];
            POrganisationRow ToRow = (POrganisationRow)OrganisationTable.Rows[0];

            if (ToRow.OrganisationName == "")
            {
                ToRow.OrganisationName = FromRow.BranchName;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            POrganisationAccess.SubmitChanges(OrganisationTable, ATransaction);
        }

        private static void MergeBank(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PBankTable FromBankTable = PBankAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PBankTable ToBankTable = PBankAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction);

            PBankRow FromRow = (PBankRow)FromBankTable.Rows[0];
            PBankRow ToRow = (PBankRow)ToBankTable.Rows[0];

            if (ToRow.BranchName == "")
            {
                ToRow.BranchName = FromRow.BranchName;
            }

            if (ToRow.BranchCode == "")
            {
                ToRow.BranchCode = FromRow.BranchCode;
            }

            if (ToRow.Bic == "")
            {
                ToRow.Bic = FromRow.Bic;
            }

            if (ToRow.EpFormatFile == "")
            {
                ToRow.EpFormatFile = FromRow.EpFormatFile;
            }

            if ((ToRow.IsContactPartnerKeyNull() || (ToRow.ContactPartnerKey == 0)) && !FromRow.IsContactPartnerKeyNull())
            {
                ToRow.ContactPartnerKey = FromRow.ContactPartnerKey;
            }

            if (ToRow.CreditCard == false)
            {
                ToRow.CreditCard = FromRow.CreditCard;
            }

            PBankAccess.SubmitChanges(ToBankTable, ATransaction);

            // Move the banking details where the From Bank is used to the To Partner
            PBankingDetailsTable BankingDetailsTable = PBankingDetailsAccess.LoadViaPBank(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in BankingDetailsTable.Rows)
            {
                ((PBankingDetailsRow)Row).BankKey = AToPartnerKey;
            }

            PBankingDetailsAccess.SubmitChanges(BankingDetailsTable, ATransaction);
        }

        private static void MergeRelationships(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            // Iterate through all relations that the From Partner has got
            PPartnerRelationshipTable RelationshipTable1 = PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);

            if (RelationshipTable1.Rows.Count > 0)
            {
                foreach (DataRow Row in RelationshipTable1.Rows)
                {
                    PPartnerRelationshipRow FromRow = (PPartnerRelationshipRow)Row;

                    if (FromRow.RelationKey == AToPartnerKey)
                    {
                        // Delete relations of the From Partner that point to the To Partner
                        Row.Delete();
                    }
                    else if (PPartnerRelationshipAccess.Exists(AToPartnerKey, FromRow.RelationName, FromRow.RelationKey, ATransaction))
                    {
                        // Delete relations of the From Partner if the To Partner already has a relation of the same type to the same Partner
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                PPartnerRelationshipAccess.SubmitChanges(RelationshipTable1, ATransaction);
            }

            // Iterate through all relations that point to the From Partner
            PPartnerRelationshipTable RelationshipTable2 = PPartnerRelationshipAccess.LoadViaPPartnerRelationKey(AFromPartnerKey, ATransaction);

            if (RelationshipTable2.Rows.Count > 0)
            {
                foreach (DataRow Row in RelationshipTable2.Rows)
                {
                    PPartnerRelationshipRow FromRow = (PPartnerRelationshipRow)Row;

                    if (FromRow.PartnerKey == AToPartnerKey)
                    {
                        // Delete relations of the To Partner that point to the From Partner
                        Row.Delete();
                    }
                    else if (PPartnerRelationshipAccess.Exists(FromRow.PartnerKey, FromRow.RelationName, AToPartnerKey, ATransaction))
                    {
                        // Delete relations of any Partner that point to the From Partner if a relation
                        // of the same Partner and the same type already points to the To Partner
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.RelationKey = AToPartnerKey;
                    }
                }

                PPartnerRelationshipAccess.SubmitChanges(RelationshipTable2, ATransaction);

                // Please note that FAMILY relations are handled specially in merge_p_person!
            }
        }

        private static void MergePartner(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerTable FromPartnerTable = PPartnerAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction);
            PPartnerTable ToPartnerTable = PPartnerAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction);

            PPartnerRow FromRow = (PPartnerRow)FromPartnerTable.Rows[0];
            PPartnerRow ToRow = (PPartnerRow)ToPartnerTable.Rows[0];

            if ((ToRow.AddresseeTypeCode == "DEFAULT") || (ToRow.AddresseeTypeCode == ""))
            {
                ToRow.AddresseeTypeCode = FromRow.AddresseeTypeCode;
            }

            if (ToRow.PartnerShortName == "")
            {
                ToRow.PartnerShortName = FromRow.PartnerShortName;
            }

            if (ToRow.PartnerShortNameLoc == "")
            {
                ToRow.PartnerShortNameLoc = FromRow.PartnerShortNameLoc;
            }

            if (ToRow.PrintedName == "")
            {
                ToRow.PrintedName = FromRow.PrintedName;
            }

            if ((ToRow.LanguageCode == "") || (ToRow.LanguageCode == "99"))
            {
                ToRow.LanguageCode = FromRow.LanguageCode;
            }

            if (ToRow.KeyInformation == "")
            {
                ToRow.KeyInformation = FromRow.KeyInformation;
            }

            if ((ToRow.Comment != "") && (FromRow.Comment != ""))
            {
                ToRow.Comment = ToRow.Comment + " / " + FromRow.Comment + "\n\n";
            }
            else if (ToRow.Comment != "")
            {
                ToRow.Comment = ToRow.Comment + "\n\n";
            }
            else if (FromRow.Comment != "")
            {
                ToRow.Comment = FromRow.Comment + "\n\n";
            }

            ToRow.Comment = ToRow.Comment + Catalog.GetString("(Merged from : ") + AFromPartnerKey.ToString() + ")";

            if (FromRow.Comment != "")
            {
                FromRow.Comment = FromRow.Comment + "\n\n";
            }

            FromRow.Comment = FromRow.Comment + Catalog.GetString("(Merged into : ") + AToPartnerKey.ToString() + ")";

            if (ToRow.StatusCode != "ACTIVE")
            {
                ToRow.StatusCode = FromRow.StatusCode;
                ToRow.StatusChange = DateTime.Today;
            }

            FromRow.StatusCode = "MERGED";
            FromRow.StatusChange = DateTime.Today;

            FromRow.DeletedPartner = true;

            if (ToRow.FinanceComment == "")
            {
                ToRow.FinanceComment = FromRow.FinanceComment;
            }

            if (ToRow.ReceiptLetterFrequency == "")
            {
                ToRow.ReceiptLetterFrequency = FromRow.ReceiptLetterFrequency;
            }

            if (ToRow.ReceiptEachGift == false)
            {
                ToRow.ReceiptEachGift = FromRow.ReceiptEachGift;
            }

            if (ToRow.EmailGiftStatement == false)
            {
                ToRow.EmailGiftStatement = FromRow.EmailGiftStatement;
            }

            if (ToRow.AnonymousDonor == false)
            {
                ToRow.AnonymousDonor = FromRow.AnonymousDonor;
            }

            if (ToRow.NoSolicitations == false)
            {
                ToRow.NoSolicitations = FromRow.NoSolicitations;
            }

            if (ToRow.ChildIndicator == false)
            {
                ToRow.ChildIndicator = FromRow.ChildIndicator;
            }

            // TODO Restricted? and User and Group ID

            if (ToRow.PreviousName == "")
            {
                ToRow.PreviousName = FromRow.PreviousName;
            }

            if (ToRow.FirstContactCode == "")
            {
                ToRow.FirstContactCode = FromRow.FirstContactCode;
            }

            if (ToRow.IntranetId == "")
            {
                ToRow.IntranetId = FromRow.IntranetId;
            }

            ToRow.FirstContactFreeform = ToRow.FirstContactFreeform + " / " + FromRow.FirstContactFreeform;

            if (ToRow.Timezone == "")
            {
                ToRow.Timezone = FromRow.Timezone;
            }

            PPartnerAccess.SubmitChanges(FromPartnerTable, ATransaction);
            PPartnerAccess.SubmitChanges(ToPartnerTable, ATransaction);

            // create a new PPartnerMerge row for the merged partner
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadAll(ATransaction);
            PPartnerMergeRow MergeRow = null;

            if (PPartnerMergeAccess.Exists(AFromPartnerKey, ATransaction) == false)
            {
                MergeRow = MergeTable.NewRowTyped();
                MergeRow.MergeFrom = AFromPartnerKey;
                MergeTable.Rows.Add(MergeRow);
            }
            else
            {
                MergeRow = (PPartnerMergeRow)PPartnerMergeAccess.LoadViaPPartnerMergeFrom(AFromPartnerKey, ATransaction).Rows[0];
            }

            MergeRow.MergeTo = AToPartnerKey;
            MergeRow.MergedBy = UserInfo.GUserInfo.UserID;
            MergeRow.MergeDate = DateTime.Now;

            PPartnerMergeAccess.SubmitChanges(MergeTable, ATransaction);
        }

        private static void MergeBuildingsAndRooms(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            // change the links of that venue used as a site for conferences
            PcConferenceVenueTable ConferenceTable = PcConferenceVenueAccess.LoadViaPVenue(AFromPartnerKey, ATransaction);

            foreach (DataRow Row in ConferenceTable.Rows)
            {
                PcConferenceVenueRow FromRow = (PcConferenceVenueRow)Row;

                if (PcConferenceVenueAccess.Exists(FromRow.ConferenceKey, AToPartnerKey, ATransaction))
                {
                    Row.Delete();
                }
                else
                {
                    FromRow.VenueKey = AToPartnerKey;
                }
            }

            PcConferenceVenueAccess.SubmitChanges(ConferenceTable, ATransaction);

            // change links for buildings:
            // if the building code does not yet exist in the new venue, then create a new record,
            // if it does exist, then do nothing here and delete it at the end of this procedure
            PcBuildingTable FromBuildingTable = PcBuildingAccess.LoadViaPVenue(AFromPartnerKey, ATransaction);
            PcBuildingTable ToBuildingTable = PcBuildingAccess.LoadAll(ATransaction);

            foreach (DataRow Row in FromBuildingTable.Rows)
            {
                PcBuildingRow FromRow = (PcBuildingRow)Row;

                if (PcBuildingAccess.Exists(AToPartnerKey, FromRow.BuildingCode, ATransaction) == false)
                {
                    // Create new record and keep old record to avoid constraint errors. Old record will be deleted later.
                    PcBuildingRow ToRow = ToBuildingTable.NewRowTyped();
                    ToRow.BuildingCode = FromRow.BuildingCode;
                    ToRow.BuildingDesc = FromRow.BuildingDesc;
                    ToRow.VenueKey = AToPartnerKey;
                    ToBuildingTable.Rows.Add(ToRow);
                }
            }

            PcBuildingAccess.SubmitChanges(ToBuildingTable, ATransaction);

            // change links for rooms:

            PcRoomTable FromRoomTable = PcRoomAccess.LoadViaPVenue(AFromPartnerKey, ATransaction);
            PcRoomTable ToRoomTable = PcRoomAccess.LoadAll(ATransaction);

            // List of data for rooms that is needed later on
            List <string[]>RoomDataList = new List <string[]>();

            foreach (DataRow Row in FromRoomTable.Rows)
            {
                PcRoomRow FromRow = (PcRoomRow)Row;
                string[] RoomData = new string[3];

                bool RoomExists = false;

                if (PcRoomAccess.Exists(AToPartnerKey, FromRow.BuildingCode, FromRow.RoomNumber, ATransaction))
                {
                    RoomData[0] = FromRow.BuildingCode;
                    RoomData[1] = FromRow.RoomNumber;
                    //  If the room number does already exists for the To venue, then add an "#M" at the end so that we don't loose information.
                    RoomData[2] = FromRow.RoomNumber + "#M";
                    RoomDataList.Add(RoomData);

                    RoomExists = true;

                    // if this room number has already been merged and "#M" added, add another "#M"
                    while (PcRoomAccess.Exists(AToPartnerKey, FromRow.BuildingCode, RoomData[2], ATransaction))
                    {
                        RoomData[2] = RoomData[2] + "#M";
                    }
                }

                // Create new record (by cloning the old record) and keep old record to avoid constraint errors.
                // Old record will be deleted later.
                object[] FromRowArray = FromRow.ItemArray;
                object[] FromRowArrayClone = (object[])FromRowArray.Clone();
                PcRoomRow FromRowClone = ToRoomTable.NewRowTyped();
                FromRowClone.ItemArray = FromRowArrayClone;

                FromRowClone.VenueKey = AToPartnerKey;

                if (RoomExists)
                {
                    FromRowClone.RoomNumber = RoomData[2];
                }

                ToRoomTable.Rows.Add(FromRowClone);
            }

            PcRoomAccess.SubmitChanges(ToRoomTable, ATransaction);

            // change links for room allocations:
            // if the room number has changed (#M has been added and it will then be in the temporary table for new room numbers) then this new
            // room number has to be set, otherwise only the venue key has to be exchanged
            PcRoomAllocTable RoomAllocTable = PcRoomAllocAccess.LoadAll(ATransaction);

            foreach (DataRow Row in RoomAllocTable.Rows)
            {
                PcRoomAllocRow FromRow = (PcRoomAllocRow)Row;

                if (FromRow.VenueKey == AFromPartnerKey)
                {
                    foreach (string[] RoomData in RoomDataList)
                    {
                        if ((RoomData[0] == FromRow.BuildingCode) && (RoomData[1] == FromRow.RoomNumber))
                        {
                            FromRow.RoomNumber = RoomData[2];
                        }

                        FromRow.VenueKey = AToPartnerKey;
                    }
                }
            }

            PcRoomAllocAccess.SubmitChanges(RoomAllocTable, ATransaction);

            // delete old room records
            foreach (DataRow Row in FromRoomTable.Rows)
            {
                Row.Delete();
            }

            PcRoomAccess.SubmitChanges(FromRoomTable, ATransaction);

            // delete old building records
            foreach (DataRow Row in FromBuildingTable.Rows)
            {
                Row.Delete();
            }

            PcBuildingAccess.SubmitChanges(FromBuildingTable, ATransaction);
        }

        private static void MergeBankAccounts(long AFromPartnerKey, long AToPartnerKey, int AMainBankingDetailsKey, TDBTransaction ATransaction)
        {
            PPartnerBankingDetailsTable BankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            // Change all From p_partner_banking_details partner keys
            foreach (DataRow Row in BankingDetailsTable.Rows)
            {
                PPartnerBankingDetailsRow FromRow = (PPartnerBankingDetailsRow)Row;

                // Delete any 'Main' Account records to prevent constraint error
                PBankingDetailsUsageTable BankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadByPrimaryKey(
                    AFromPartnerKey, FromRow.BankingDetailsKey, "MAIN", ATransaction);

                if (BankingDetailsUsageTable.Rows.Count > 0)
                {
                    BankingDetailsUsageTable.Rows[0].Delete();
                    PBankingDetailsUsageAccess.SubmitChanges(BankingDetailsUsageTable, ATransaction);
                }

                // if To partner does NOT already have this banking details relation
                if (PPartnerBankingDetailsAccess.Exists(AToPartnerKey, FromRow.BankingDetailsKey, ATransaction) == false)
                {
                    FromRow.PartnerKey = AToPartnerKey;
                }
                // if To partner has got the same banking details relation as the From Partner
                else
                {
                    FromRow.Delete();
                }
            }

            PPartnerBankingDetailsAccess.SubmitChanges(BankingDetailsTable, ATransaction);

            // set the 'Main' Bank Account if needed (-1 if not needed)
            if (AMainBankingDetailsKey != -1)
            {
                PBankingDetailsUsageTable BankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadAll(ATransaction);
                bool NeedToAddNewRow = true;

                foreach (DataRow Row in BankingDetailsUsageTable.Rows)
                {
                    PBankingDetailsUsageRow BankingDetailsUsageRow = (PBankingDetailsUsageRow)Row;

                    if ((BankingDetailsUsageRow.PartnerKey == AToPartnerKey) && (BankingDetailsUsageRow.Type == "MAIN"))
                    {
                        if (BankingDetailsUsageRow.BankingDetailsKey == AMainBankingDetailsKey)
                        {
                            NeedToAddNewRow = false;
                        }
                        else
                        {
                            // delete old 'Main'
                            Row.Delete();
                        }
                    }
                }

                // add new 'Main' account record if it does not already exist
                if (NeedToAddNewRow)
                {
                    PBankingDetailsUsageRow NewRow = BankingDetailsUsageTable.NewRowTyped();
                    NewRow.PartnerKey = AToPartnerKey;
                    NewRow.BankingDetailsKey = AMainBankingDetailsKey;
                    NewRow.Type = "MAIN";
                    BankingDetailsUsageTable.Rows.Add(NewRow);
                }

                PBankingDetailsUsageAccess.SubmitChanges(BankingDetailsUsageTable, ATransaction);
            }
        }

        private static void MergeTaxDeductibilityPercentage(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerTaxDeductiblePctTable FromTable = PPartnerTaxDeductiblePctAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            PPartnerTaxDeductiblePctTable ToTable = PPartnerTaxDeductiblePctAccess.LoadViaPPartner(AToPartnerKey, ATransaction);

            // Merge tax deductibile percentage if To partner does not already have one set
            foreach (PPartnerTaxDeductiblePctRow Row in FromTable.Rows)
            {
                if ((ToTable == null) || (ToTable.Rows.Count == 0))
                {
                    Row.PartnerKey = AToPartnerKey;
                }
                else
                {
                    Row.Delete();
                }
            }

            PPartnerTaxDeductiblePctAccess.SubmitChanges(FromTable, ATransaction);
        }

        //  1) Update recent partner settings for the current user to refer to the To partner.
        //  2) Removes the From partner from the p_recent_partners table.
        private static void MergeRecentAndLastPartnerInfo(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            // set user user defaults
            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_LASTPARTNERMAILROOM, AToPartnerKey);
            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_LASTPERSONCONFERENCE, AToPartnerKey);
            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_LASTPERSONPERSONNEL, AToPartnerKey);

            // if Partner is a conference then change LastConferenceWorkedWith to 0
            if (PcConferenceAccess.Exists(AFromPartnerKey, ATransaction))
            {
                TUserDefaults.SetDefault(MSysManConstants.CONFERENCE_LASTCONFERENCEWORKEDWITH, 0);
            }

            PRecentPartnersTable RecentPartnersTable = PRecentPartnersAccess.LoadByPrimaryKey(
                Ict.Petra.Shared.UserInfo.GUserInfo.UserID, AFromPartnerKey, ATransaction);

            // if a RecentPartners record exists for From Partner... delete.
            if (RecentPartnersTable.Rows.Count > 0)
            {
                RecentPartnersTable.Rows[0].Delete();
                PRecentPartnersAccess.SubmitChanges(RecentPartnersTable, ATransaction);
            }
        }

        private static void MergeLinkToCostCentre(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            AValidLedgerNumberTable ValidLedgerNumberTable = AValidLedgerNumberAccess.LoadViaPPartnerPartnerKey(AFromPartnerKey, ATransaction);

            if (ValidLedgerNumberTable.Rows.Count > 0)
            {
                foreach (DataRow Row in ValidLedgerNumberTable.Rows)
                {
                    AValidLedgerNumberRow FromRow = (AValidLedgerNumberRow)Row;

                    if (AValidLedgerNumberAccess.Exists(FromRow.LedgerNumber, AToPartnerKey, ATransaction))
                    {
                        Row.Delete();
                    }
                    else
                    {
                        FromRow.PartnerKey = AToPartnerKey;
                    }
                }

                AValidLedgerNumberAccess.SubmitChanges(ValidLedgerNumberTable, ATransaction);
            }
        }

        private static void MergeGraphic(long AFromPartnerKey, long AToPartnerKey, TDBTransaction ATransaction)
        {
            PPartnerGraphicTable PartnerGraphicTable = PPartnerGraphicAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);

            if (PartnerGraphicTable.Rows.Count > 0)
            {
                foreach (DataRow Row in PartnerGraphicTable.Rows)
                {
                    PPartnerGraphicRow FromRow = (PPartnerGraphicRow)Row;

                    FromRow.PartnerKey = AToPartnerKey;
                }

                PPartnerGraphicAccess.SubmitChanges(PartnerGraphicTable, ATransaction);
            }
        }

        private static void MergeGiftDestination(long AFromPartnerKey, long AToPartnerKey, TPartnerClass APartnerClass, TDBTransaction ATransaction)
        {
            PPartnerGiftDestinationRow ActiveRow = null;
            PPartnerGiftDestinationRow FromGiftDestinationNeedsEnded = null;
            PPartnerGiftDestinationRow ToGiftDestinationNeedsEnded = null;

            // if partners are Person's then find their family keys
            if (APartnerClass == TPartnerClass.PERSON)
            {
                AFromPartnerKey = ((PPersonRow)PPersonAccess.LoadByPrimaryKey(AFromPartnerKey, ATransaction).Rows[0]).FamilyKey;
                AToPartnerKey = ((PPersonRow)PPersonAccess.LoadByPrimaryKey(AToPartnerKey, ATransaction).Rows[0]).FamilyKey;
            }

            // check for an active gift destination for the 'From' partner
            PPartnerGiftDestinationTable FromGiftDestinations = PPartnerGiftDestinationAccess.LoadViaPPartner(AFromPartnerKey, ATransaction);
            ActiveRow = TMergePartnersCheckWebConnector.GetActiveGiftDestination(FromGiftDestinations);

            // return if no active record has been found (this should never happen!)
            if (ActiveRow == null)
            {
                return;
            }

            // check for clash with the 'To' partner
            PPartnerGiftDestinationTable ToGiftDestinations = PPartnerGiftDestinationAccess.LoadViaPPartner(AToPartnerKey, ATransaction);
            TMergePartnersCheckWebConnector.CheckGiftDestinationClashes(
                ToGiftDestinations, ActiveRow, out FromGiftDestinationNeedsEnded, out ToGiftDestinationNeedsEnded);

            // edit expiry dates if needed (the user will have given permission to do this)
            if (FromGiftDestinationNeedsEnded != null)
            {
                ActiveRow.DateExpires = FromGiftDestinationNeedsEnded.DateEffective.AddDays(-1);
            }

            if (ToGiftDestinationNeedsEnded != null)
            {
                ToGiftDestinationNeedsEnded.DateExpires = ActiveRow.DateEffective.AddDays(-1);
            }

            // move Active Gift Destination to new family
            ActiveRow.PartnerKey = AToPartnerKey;

            // submit changes
            PPartnerGiftDestinationAccess.SubmitChanges(FromGiftDestinations, ATransaction);
            PPartnerGiftDestinationAccess.SubmitChanges(ToGiftDestinations, ATransaction);
        }
    }
}