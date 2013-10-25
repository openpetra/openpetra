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
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
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