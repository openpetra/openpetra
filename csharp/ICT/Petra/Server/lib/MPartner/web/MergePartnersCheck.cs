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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;


namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TMergePartnersCheckWebConnector
    {
        /// <summary>
        /// performs checks to determine if two partners can be merged
        /// </summary>
        /// <param name="AFromPartnerKey">From Partner's Partner Key</param>
        /// <param name="AToPartnerKey">To Partner's Partner Key</param>
        /// <param name="AFromPartnerClass">From Partner's Partner Class</param>
        /// <param name="AToPartnerClass">To Partner's Partner Class</param>
        /// <param name="AReasonForMerging">Combo box selection for reason for merging</param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if partners can be merged</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CheckPartnersCanBeMerged(long AFromPartnerKey,
            long AToPartnerKey,
            TPartnerClass AFromPartnerClass,
            TPartnerClass AToPartnerClass,
            string AReasonForMerging,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            if (AFromPartnerClass != AToPartnerClass)
            {
                // confirm that user wants to merge partners from different partner classes
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                        String.Format(Catalog.GetString("Do you really want to merge a Partner of class {0} into a Partner of class {1}?"),
                            AFromPartnerClass, AToPartnerClass),
                        TResultSeverity.Resv_Noncritical));

                // Family Partner cannot be merged into a different partner class if family members, donations or bank accounts exist for that partner
                if (AFromPartnerClass == TPartnerClass.FAMILY)
                {
                    int FamilyMergeResult = CanFamilyMergeIntoDifferentClass(AFromPartnerKey);

                    if (FamilyMergeResult != 0)
                    {
                        string ErrorMessage = "";

                        if (FamilyMergeResult == 1)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This Family record cannot be merged into a Partner with different class as Family members exist!");
                        }
                        else if (FamilyMergeResult == 2)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This Family record cannot be merged into a Partner with different class as donations were received for it!");
                        }
                        else if (FamilyMergeResult == 3)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This record cannot be merged into a Partner with different class as bank accounts exist for it!");
                        }

                        // critical error - only need to return this VerificationResult
                        AVerificationResult.Clear();
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                                ErrorMessage,
                                TResultSeverity.Resv_Critical));

                        return false;
                    }
                }
            }
            else // partner classes are the same
            {
                string FromPartnerSupplierCurrency;
                string ToPartnerSupplierCurrency;

                // if two partners are suppliers they must have the same currency
                if ((GetSupplierCurrency(AFromPartnerKey, out FromPartnerSupplierCurrency) == true)
                    && (GetSupplierCurrency(AToPartnerKey, out ToPartnerSupplierCurrency) == true))
                {
                    if (FromPartnerSupplierCurrency != ToPartnerSupplierCurrency)
                    {
                        // critical error - only need to return this VerificationResult
                        AVerificationResult.Clear();
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                                Catalog.GetString(
                                    "These Partners cannot be merged. Partners that are suppliers must have the same currency in order to merge."),
                                TResultSeverity.Resv_Critical));

                        return false;
                    }
                }

                if (AFromPartnerClass == TPartnerClass.VENUE)
                {
                    AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                            Catalog.GetString("You are about to merge VENUEs. This will imply merging of buildings, rooms and room " +
                                "allocations defined for these Venues in the Conference Module!") + "\n\n" + Catalog.GetString("Continue?"),
                            TResultSeverity.Resv_Noncritical));
                }

                if (AFromPartnerClass == TPartnerClass.BANK)
                {
                    AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                            Catalog.GetString("You are about to merge BANKSs. This will imply that all bank accounts that were with the " +
                                "From-Bank Partner will become bank accounts of the To-Bank Partner. For this reason you should merge Banks only when "
                                +
                                "both Bank Partners actually represented the same Bank, or if two different Banks have merged their operations!") +
                            "\n\n" + Catalog.GetString("Continue?"), Catalog.GetString("Merge Partners"),
                            TResultSeverity.Resv_Noncritical));
                }
            }

            if (AReasonForMerging == "Duplicate Record Exists")
            {
                if (AFromPartnerClass == TPartnerClass.FAMILY)
                {
                    // AFromPartnerClass and AToPartnerClass are the same
                    int CheckCommitmentsResult = CheckPartnerCommitments(AFromPartnerKey, AToPartnerKey, AFromPartnerClass);

                    // if the from family Partner contains a person with an ongoing commitment
                    if (CheckCommitmentsResult != 0)
                    {
                        string FromPartnerShortName;
                        string ToPartnerShortName;
                        TPartnerClass PartnerClass;

                        TPartnerServerLookups.GetPartnerShortName(AFromPartnerKey, out FromPartnerShortName, out PartnerClass);
                        TPartnerServerLookups.GetPartnerShortName(AFromPartnerKey, out ToPartnerShortName, out PartnerClass);

                        string ErrorMessage = string.Format(Catalog.GetString("WARNING: You are about to change the family of {0} ({1}).") + "\n\n" +
                            Catalog.GetString("Changing a person's family can affect the person's ability to see their support information in" +
                                " Caleb including any support that they may receive from other Fields."), FromPartnerShortName, AFromPartnerKey);

                        if (CheckCommitmentsResult == 1)
                        {
                            ErrorMessage += "\n\n" + string.Format(Catalog.GetString("It is STRONGLY recommended that you do not continue and " +
                                    "consider merging family {0} ({1}) into family {2} ({3})."),
                                ToPartnerShortName, AToPartnerKey, FromPartnerShortName, AFromPartnerKey);
                        }

                        ErrorMessage += "\n\n" + Catalog.GetString("Do you want to continue?");

                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                                ErrorMessage, Catalog.GetString("Merge Partners"),
                                TResultSeverity.Resv_Noncritical));
                    }
                }
                else if (AFromPartnerClass == TPartnerClass.PERSON)
                {
                    // AFromPartnerClass and AToPartnerClass are the same
                    int CheckCommitmentsResult = CheckPartnerCommitments(AFromPartnerKey, AToPartnerKey, AFromPartnerClass);

                    // if the from Partner has an ongoing commitment
                    if (CheckCommitmentsResult != 0)
                    {
                        string ErrorMessage = "";
                        string FromPartnerShortName;
                        TPartnerClass PartnerClass;

                        TPartnerServerLookups.GetPartnerShortName(AFromPartnerKey, out FromPartnerShortName, out PartnerClass);

                        if (CheckCommitmentsResult == 3)
                        {
                            ErrorMessage = string.Format(Catalog.GetString("WARNING: You are about to change the family of {0} ({1}).") + "\n\n" +
                                Catalog.GetString("Changing a person's family can affect the person's ability to see their support information in" +
                                    " Caleb including any support that they may receive from other Fields."), FromPartnerShortName, AFromPartnerKey);
                        }
                        else if (CheckCommitmentsResult == 2)
                        {
                            ErrorMessage = Catalog.GetString("WARNING: Both Persons have a current commitment. " +
                                "Be aware that merging these Persons may affect their usage of Caleb.") +
                                           "\n\n" + Catalog.GetString("Do you want to continue?");
                        }
                        else if (CheckCommitmentsResult == 1)
                        {
                            ErrorMessage = string.Format(Catalog.GetString("WARNING: Person {0} ({1}) has a current commitment. " +
                                    "We strongly recommend merging the other way around."), FromPartnerShortName, AFromPartnerKey) +
                                           "\n\n" + Catalog.GetString("Do you want to continue?");
                        }

                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                                ErrorMessage, Catalog.GetString("Merge Partners"),
                                TResultSeverity.Resv_Noncritical));
                    }
                }
            }

            // checks if one of the partners is a Foundation organisation.
            if ((AFromPartnerClass == TPartnerClass.ORGANISATION) || (AToPartnerClass == TPartnerClass.ORGANISATION))
            {
                PFoundationTable FromFoundationTable = null;
                PFoundationTable ToFoundationTable = null;

                string ErrorMessage = "";

                if (AFromPartnerClass == TPartnerClass.ORGANISATION)
                {
                    FromFoundationTable = GetOrganisationFoundation(AFromPartnerKey);
                }

                if (AToPartnerClass == TPartnerClass.ORGANISATION)
                {
                    ToFoundationTable = GetOrganisationFoundation(AToPartnerKey);
                }

                // if both partners are Foundation organisations check permissions
                if ((FromFoundationTable != null) && (ToFoundationTable != null))
                {
                    if (!TSecurity.CheckFoundationSecurity((PFoundationRow)FromFoundationTable.Rows[0]))
                    {
                        ErrorMessage = Catalog.GetString("The Partner that you are merging from is a Foundation, but you do not " +
                            "have access rights to view its data. Therefore you are not allowed to merge these Foundations!") + "\n\n" +
                                       Catalog.GetString("Access Denied");
                    }
                    else if (!TSecurity.CheckFoundationSecurity((PFoundationRow)ToFoundationTable.Rows[0]))
                    {
                        ErrorMessage = Catalog.GetString("The Partner that you are merging into is a Foundation, but you do not " +
                            "have access rights to view its data. Therefore you are not allowed to merge these Foundations!") + "\n\n" +
                                       Catalog.GetString("Access Denied");
                    }
                }
                // none or both partners must be Foundation organisations
                else if (FromFoundationTable != null)
                {
                    ErrorMessage = Catalog.GetString("The Partner that you are merging from is a Foundation, but the Partner that you " +
                        "are merging into is not a Foundation. This is not allowed!") + "\n\n" +
                                   Catalog.GetString("Both Merge Partners Need to be Foundations!");
                }
                else if (ToFoundationTable != null)
                {
                    ErrorMessage = Catalog.GetString("The Partner that you are merging from isn't a Foundation, but the Partner that you " +
                        "are merging into is a Foundation. This is not allowed!") + "\n\n" +
                                   Catalog.GetString("Both Merge Partners Need to be Foundations!");
                }

                if (ErrorMessage != "")
                {
                    // critical error - only need to return this VerificationResult
                    AVerificationResult.Clear();
                    AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Merge Partners"),
                            ErrorMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// returns the supplier currency for a partner if it is a supplier
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ACurrency"></param>
        /// <returns>true if partner is a supplier and a currency is found</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool GetSupplierCurrency(long APartnerKey, out string ACurrency)
        {
            ACurrency = "";
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                AApSupplierTable Table = AApSupplierAccess.LoadByPrimaryKey(APartnerKey, Transaction);

                if (Table.Rows.Count != 0)
                {
                    ACurrency = ((AApSupplierRow)Table.Rows[0]).CurrencyCode;
                    return true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.GetSupplierCurrency: rollback own transaction.");
            }

            return false;
        }

        /// <summary>
        /// this will validate two partners who have been selected to be merged
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns>0: there are no probelms, 1: family members exist, 2: donations exist, 3: bank accounts exist</returns>
        [RequireModulePermission("PTNRUSER")]
        public static int CanFamilyMergeIntoDifferentClass(long APartnerKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                PPersonTable PersonTable = PPersonAccess.LoadViaPFamily(APartnerKey, Transaction);

                if (PersonTable.Rows.Count > 0)
                {
                    return 1;
                }

                AGiftDetailTable GiftDetailTable = AGiftDetailAccess.LoadViaPPartnerRecipientKey(APartnerKey, Transaction);

                if (GiftDetailTable.Rows.Count > 0)
                {
                    return 2;
                }

                PBankingDetailsTable BankingDetailsTable = PBankingDetailsAccess.LoadViaPPartner(APartnerKey, Transaction);

                if (BankingDetailsTable.Rows.Count > 0)
                {
                    return 3;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.CanFamilyMergeIntoDifferentClass: rollback own transaction.");
            }

            return 0;
        }

        /// <summary>
        /// check if persons have ongoing commitments or if familys contain persons with ongoing commitments
        /// </summary>
        /// <param name="AFromPartnerKey">Merge from Partner</param>
        /// <param name="AToPartnerKey">Merge to Partner</param>
        /// <param name="APartnerClass">Partner Class for both Partners</param>
        /// <returns>0: no commitments, 1: from partner/family only has commitments, 2: from and to partner/family have commitments,
        /// 3: to or/and from commitments and both in same family (person class only)</returns>
        [RequireModulePermission("PTNRUSER")]
        public static int CheckPartnerCommitments(long AFromPartnerKey, long AToPartnerKey, TPartnerClass APartnerClass)
        {
            PPersonTable FromPersonTable = null;
            PPersonTable ToPersonTable = null;
            int ReturnValue = 0;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if (APartnerClass == TPartnerClass.FAMILY)
                {
                    FromPersonTable = PPersonAccess.LoadViaPFamily(AFromPartnerKey, Transaction);
                    ToPersonTable = PPersonAccess.LoadViaPFamily(AToPartnerKey, Transaction);
                }
                else if (APartnerClass == TPartnerClass.PERSON)
                {
                    FromPersonTable = PPersonAccess.LoadViaPPartner(AFromPartnerKey, Transaction);
                    ToPersonTable = PPersonAccess.LoadViaPPartner(AToPartnerKey, Transaction);
                }

                // check if from partner has commitments
                if (PersonHasCommitments(FromPersonTable, Transaction))
                {
                    ReturnValue = 1;

                    // check if two persons are in same family
                    if ((APartnerClass == TPartnerClass.PERSON)
                        && (((PPersonRow)FromPersonTable.Rows[0]).FamilyKey == ((PPersonRow)ToPersonTable.Rows[0]).FamilyKey))
                    {
                        ReturnValue = 3;
                    }
                    // check if to partner also has commitments
                    else if (PersonHasCommitments(ToPersonTable, Transaction))
                    {
                        ReturnValue = 2;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.CheckPartnerCommitments: rollback own transaction.");
            }

            return ReturnValue;
        }

        // check if a person has on going commitments
        private static bool PersonHasCommitments(PPersonTable ATable, TDBTransaction ATransaction)
        {
            foreach (PPersonRow Row in ATable.Rows)
            {
                PmStaffDataTable StaffDataTable = PmStaffDataAccess.LoadViaPPerson(Row.PartnerKey, ATransaction);

                foreach (PmStaffDataRow StaffRow in StaffDataTable.Rows)
                {
                    if (((StaffRow.EndOfCommitment >= DateTime.Now) || (StaffRow.EndOfCommitment == null))
                        && (StaffRow.StartOfCommitment <= DateTime.Now))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// get the PFoundation table for an organisation if it is a foundation
        /// </summary>
        /// <param name="AFromPartnerKey">Organisation's Partner Key</param>
        /// <returns>Returns PFoundation table containing an organisation's foundation records if they exist. Otherwise returns null.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static PFoundationTable GetOrganisationFoundation(long AFromPartnerKey)
        {
            PFoundationTable ReturnValue = null;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                PFoundationTable FoundationTable = PFoundationAccess.LoadViaPOrganisation(AFromPartnerKey, Transaction);

                if (FoundationTable.Rows.Count > 0)
                {
                    ReturnValue = FoundationTable;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.OrganisationIsFoundation: rollback own transaction.");
            }

            return ReturnValue;
        }

        /// <summary>
        /// determines if the user needs to select a main bank account for the merged record
        /// </summary>
        /// <param name="AFromPartnerKey">From Partner Key</param>
        /// <param name="AToPartnerKey">To Partner Key</param>
        /// <returns>Returns PFoundation table containing an organisation's foundation records if they exist. Otherwise returns null.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool NeedMainBankAccount(long AFromPartnerKey, long AToPartnerKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                PPartnerBankingDetailsTable FromBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPPartner(AFromPartnerKey, Transaction);
                PPartnerBankingDetailsTable ToBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPPartner(AToPartnerKey, Transaction);

                int MainBankAccounts = 0;
                int FromBankAccounts = FromBankingDetailsTable.Rows.Count;
                int ToBankAccounts = ToBankingDetailsTable.Rows.Count;

                if (FromBankAccounts > 0)
                {
                    foreach (DataRow Row in FromBankingDetailsTable.Rows)
                    {
                        PPartnerBankingDetailsRow FromRow = (PPartnerBankingDetailsRow)Row;

                        if (PBankingDetailsUsageAccess.Exists(AFromPartnerKey, FromRow.BankingDetailsKey, "MAIN", Transaction))
                        {
                            MainBankAccounts += 1;
                        }
                    }
                }

                if (ToBankAccounts > 0)
                {
                    foreach (DataRow Row in ToBankingDetailsTable.Rows)
                    {
                        PPartnerBankingDetailsRow FromRow = (PPartnerBankingDetailsRow)Row;

                        if (PBankingDetailsUsageAccess.Exists(AToPartnerKey, FromRow.BankingDetailsKey, "MAIN", Transaction))
                        {
                            MainBankAccounts += 1;
                        }
                    }
                }

                // if the merged Partner will have more than one bank account and either 0 or 2 of the bank accounts are 'Main' accounts
                // then a new 'Main' account needs to be selected
                if (((FromBankAccounts + ToBankAccounts) > 1) && (MainBankAccounts != 1))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.OrganisationIsFoundation: rollback own transaction.");
            }

            return false;
        }

        /// <summary>
        /// get the banking details of the From Partner and the To Partner in a single table
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PBankingDetailsTable GetPartnerBankingDetails(long AFromPartnerKey, long AToPartnerKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            PBankingDetailsTable ReturnTable = new PBankingDetailsTable();

            try
            {
                PBankingDetailsTable FromBankingDetailsTable = PBankingDetailsAccess.LoadViaPPartner(AFromPartnerKey, Transaction);
                PBankingDetailsTable ToBankingDetailsTable = PBankingDetailsAccess.LoadViaPPartner(AToPartnerKey, Transaction);

                // clone the data in each table and add them to a new table to combine the data

                foreach (DataRow Row in FromBankingDetailsTable.Rows)
                {
                    object[] RowArray = Row.ItemArray;
                    object[] RowArrayClone = (object[])RowArray.Clone();
                    DataRow RowClone = ReturnTable.NewRowTyped(false);
                    RowClone.ItemArray = RowArrayClone;
                    ReturnTable.Rows.Add(RowClone);
                }

                foreach (DataRow Row in ToBankingDetailsTable.Rows)
                {
                    object[] RowArray = Row.ItemArray;
                    object[] RowArrayClone = (object[])RowArray.Clone();
                    DataRow RowClone = ReturnTable.NewRowTyped(false);
                    RowClone.ItemArray = RowArrayClone;
                    ReturnTable.Rows.Add(RowClone);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TMergePartnersWebConnector.GetPartnerBankingDetails: rollback own transaction.");
            }

            return ReturnTable;
        }
    }
}