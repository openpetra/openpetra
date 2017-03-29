//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.Gift
{
    /// <summary>
    /// Business functions for Gift sub system of Finance
    ///
    /// These Business Objects handle the retrieval, verification and saving of data.
    ///
    /// @Comment These Business Objects can be instantiated by other Server Objects
    ///          (usually UIConnectors).
    /// </summary>
    public class TGift
    {
        /// <summary>
        /// constant for split gifts
        /// </summary>
        private static readonly string StrSplitGift = Catalog.GetString("Split Gift");

        /// <summary>
        /// get the details of the last gift of the partner
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ALastGiftDate"></param>
        /// <param name="AGiftInfo"></param>
        public static void GetLastGiftDetails(Int64 APartnerKey, out DateTime ALastGiftDate, out String AGiftInfo)
        {
            Boolean LastGiftAvailable;
            DateTime LastGiftDate;
            decimal LastGiftAmount;
            Int64 LastGiftGivenToPartnerKey;
            Int64 LastGiftRecipientLedger;
            String LastGiftCurrencyCode;
            String LastGiftDisplayFormat;
            String LastGiftGivenToShortName;
            String LastGiftRecipientLedgerShortName;
            Boolean RestrictedGiftAccessDenied;

            AGiftInfo = "";
            ALastGiftDate = DateTime.MinValue;

            LastGiftAvailable = GetLastGiftDetails(APartnerKey,
                out LastGiftDate,
                out LastGiftAmount,
                out LastGiftGivenToPartnerKey,
                out LastGiftRecipientLedger,
                out LastGiftCurrencyCode,
                out LastGiftDisplayFormat,
                out LastGiftGivenToShortName,
                out LastGiftRecipientLedgerShortName,
                out RestrictedGiftAccessDenied);

            if (LastGiftAvailable)
            {
                ALastGiftDate = LastGiftDate;

                if (TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTAMOUNT).ToLower() == "true")
                {
                    // Check OpenPetra Module access to FINANCE-1 or Financial Development
                    if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)
                        || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER))
                    {
                        if (LastGiftCurrencyCode != "")
                        {
                            AGiftInfo = LastGiftCurrencyCode + ' ' + StringHelper.FormatCurrency(LastGiftAmount, LastGiftDisplayFormat) + "  ";
                        }
                        else
                        {
                            AGiftInfo = LastGiftAmount.ToString() + "  ";
                        }
                    }
                }

                if (LastGiftGivenToPartnerKey == -1)
                {
                    // Split Gift
                    if ((TSystemDefaultsCache.GSystemDefaultsCache.GetBooleanDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTRECIPIENT))
                        || (TSystemDefaultsCache.GSystemDefaultsCache.GetBooleanDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTFIELD)))
                    {
                        AGiftInfo = AGiftInfo + StrSplitGift;
                    }
                }
                else
                {
                    // Not a Split Gift
                    if (TSystemDefaultsCache.GSystemDefaultsCache.GetBooleanDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTRECIPIENT))
                    {
                        if (LastGiftGivenToPartnerKey != -1)
                        {
                            AGiftInfo = AGiftInfo + LastGiftGivenToShortName;
                        }
                        else
                        {
                            AGiftInfo = AGiftInfo + LastGiftGivenToPartnerKey.ToString();
                        }
                    }

                    if (TSystemDefaultsCache.GSystemDefaultsCache.GetBooleanDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTFIELD))
                    {
                        if (LastGiftRecipientLedger != -1)
                        {
                            AGiftInfo = AGiftInfo + " (" + LastGiftRecipientLedgerShortName + ')';
                        }
                        else
                        {
                            AGiftInfo = AGiftInfo + " (" + LastGiftRecipientLedger.ToString() + ')';
                        }
                    }
                }
            }
            else
            {
                if (!RestrictedGiftAccessDenied)
                {
                    ALastGiftDate = TSaveConvert.ObjectToDate(LastGiftDate);
                    AGiftInfo = "";
                }
                else
                {
                    AGiftInfo = Catalog.GetString("** confidential **");
                }
            }
        }

        /// <summary>
        /// get more details of the last gift of the partner
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ALastGiftDate"></param>
        /// <param name="ALastGiftAmount"></param>
        /// <param name="ALastGiftGivenToPartnerKey"></param>
        /// <param name="ALastGiftRecipientLedger"></param>
        /// <param name="ALastGiftCurrencyCode"></param>
        /// <param name="ALastGiftDisplayFormat"></param>
        /// <param name="ALastGiftGivenToShortName"></param>
        /// <param name="ALastGiftRecipientLedgerShortName"></param>
        /// <param name="ARestrictedOrConfidentialGiftAccessDenied"></param>
        /// <returns></returns>
        public static Boolean GetLastGiftDetails(Int64 APartnerKey,
            out DateTime ALastGiftDate,
            out decimal ALastGiftAmount,
            out Int64 ALastGiftGivenToPartnerKey,
            out Int64 ALastGiftRecipientLedger,
            out String ALastGiftCurrencyCode,
            out String ALastGiftDisplayFormat,
            out String ALastGiftGivenToShortName,
            out String ALastGiftRecipientLedgerShortName,
            out Boolean ARestrictedOrConfidentialGiftAccessDenied)
        {
            DataSet LastGiftDS;
            AGiftDetailTable GiftDetailDT;
            SGroupGiftTable GroupGiftDT;
            SUserGroupTable UserGroupDT;
            AGiftRow GiftDR;
            AGiftBatchRow GiftBatchDR;
            AGiftDetailRow GiftDetailDR;
            ACurrencyRow CurrencyDR;
            Int16 Counter;
            Boolean AccessToGift = false;

            DataRow[] FoundUserGroups;

            ALastGiftAmount = 0;
            ALastGiftCurrencyCode = "";
            ALastGiftDisplayFormat = "";
            ALastGiftDate = DateTime.MinValue;
            ALastGiftGivenToPartnerKey = 0;
            ALastGiftGivenToShortName = "";
            ALastGiftRecipientLedger = 0;
            ALastGiftRecipientLedgerShortName = "";
            ARestrictedOrConfidentialGiftAccessDenied = false;

            DateTime tmpLastGiftDate = ALastGiftDate;
            decimal tmpLastGiftAmount = ALastGiftAmount;
            Int64 tmpLastGiftGivenToPartnerKey = ALastGiftGivenToPartnerKey;
            Int64 tmpLastGiftRecipientLedger = ALastGiftRecipientLedger;
            String tmpLastGiftCurrencyCode = ALastGiftCurrencyCode;
            String tmpLastGiftDisplayFormat = ALastGiftDisplayFormat;
            String tmpLastGiftGivenToShortName = ALastGiftGivenToShortName;
            String tmpLastGiftRecipientLedgerShortName = ALastGiftRecipientLedgerShortName;
            Boolean tmpRestrictedOrConfidentialGiftAccessDenied = ARestrictedOrConfidentialGiftAccessDenied;

            if ((UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)
                 || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER)) == false)
            {
                // User hasn't got access to FINANCE-1 module or Financial Development module
                return false;
            }

            // Set up temp DataSet
            LastGiftDS = new DataSet("LastGiftDetails");
            LastGiftDS.Tables.Add(new AGiftTable());
            LastGiftDS.Tables.Add(new AGiftBatchTable());
            LastGiftDS.Tables.Add(new AGiftDetailTable());
            LastGiftDS.Tables.Add(new ACurrencyTable());
            LastGiftDS.Tables.Add(new PPartnerTable());

            TDBTransaction Transaction = null;
            bool SubmissionOK = true;

            // Important: The IsolationLevel here needs to correspond with the IsolationLevel in the
            // Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector.LoadData Method
            // as otherwise the attempt of taking-out of a DB Transaction here will lead to Bug #4167!
            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref Transaction, ref SubmissionOK,
                delegate
                {
                    try
                    {
                        try
                        {
                            AGiftAccess.LoadViaPPartner(LastGiftDS, APartnerKey, null, Transaction,
                                StringHelper.InitStrArr(new String[] { "ORDER BY", AGiftTable.GetDateEnteredDBName() + " DESC" }), 0, 1);
                        }
                        catch (ESecurityDBTableAccessDeniedException)
                        {
                            // User hasn't got access to a_gift Table in the DB
                            return;
                        }
                        catch (Exception ex)
                        {
                            TLogging.LogException(ex, Utilities.GetMethodSignature());
                            throw;
                        }

                        if (LastGiftDS.Tables[AGiftTable.GetTableName()].Rows.Count == 0)
                        {
                            // Partner hasn't given any Gift so far
                            return;
                        }

                        // Get the last gift
                        GiftDR = ((AGiftTable)LastGiftDS.Tables[AGiftTable.GetTableName()])[0];

                        if (GiftDR.Restricted)
                        {
                            AccessToGift = false;
                            GroupGiftDT = SGroupGiftAccess.LoadViaAGift(
                                GiftDR.LedgerNumber,
                                GiftDR.BatchNumber,
                                GiftDR.GiftTransactionNumber,
                                Transaction);
                            UserGroupDT = SUserGroupAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, Transaction);

                            // Loop over all rows of GroupGiftDT
                            for (Counter = 0; Counter <= GroupGiftDT.Rows.Count - 1; Counter += 1)
                            {
                                // To be able to view a Gift, ReadAccess must be granted
                                if (GroupGiftDT[Counter].ReadAccess)
                                {
                                    // Find out whether the user has a row in s_user_group with the
                                    // GroupID of the GroupGift row
                                    FoundUserGroups =
                                        UserGroupDT.Select(SUserGroupTable.GetGroupIdDBName() + " = '" + GroupGiftDT[Counter].GroupId + "'");

                                    if (FoundUserGroups.Length != 0)
                                    {
                                        // Access to gift can be granted
                                        AccessToGift = true;
                                        continue;

                                        // don't evaluate further GroupGiftDT rows
                                    }
                                }
                            }
                        }
                        else
                        {
                            AccessToGift = true;
                        }

                        if (AccessToGift)
                        {
                            tmpLastGiftDate = GiftDR.DateEntered;

                            // Console.WriteLine('GiftDR.LedgerNumber: ' + GiftDR.LedgerNumber.ToString + '; ' +
                            // 'GiftDR.BatchNumber:  ' + GiftDR.BatchNumber.ToString);
                            // Load Gift Batch
                            AGiftBatchAccess.LoadByPrimaryKey(LastGiftDS, GiftDR.LedgerNumber, GiftDR.BatchNumber,
                                StringHelper.InitStrArr(new String[] { AGiftBatchTable.GetCurrencyCodeDBName() }), Transaction, null, 0, 0);

                            if (LastGiftDS.Tables[AGiftBatchTable.GetTableName()].Rows.Count != 0)
                            {
                                GiftBatchDR = ((AGiftBatchRow)LastGiftDS.Tables[AGiftBatchTable.GetTableName()].Rows[0]);
                                tmpLastGiftCurrencyCode = GiftBatchDR.CurrencyCode;

                                // Get Currency
                                ACurrencyAccess.LoadByPrimaryKey(LastGiftDS, GiftBatchDR.CurrencyCode, Transaction);

                                if (LastGiftDS.Tables[ACurrencyTable.GetTableName()].Rows.Count != 0)
                                {
                                    CurrencyDR = (ACurrencyRow)(LastGiftDS.Tables[ACurrencyTable.GetTableName()].Rows[0]);
                                    tmpLastGiftCurrencyCode = CurrencyDR.CurrencyCode;
                                    tmpLastGiftDisplayFormat = CurrencyDR.DisplayFormat;
                                }
                                else
                                {
                                    tmpLastGiftCurrencyCode = "";
                                    tmpLastGiftDisplayFormat = "";
                                }
                            }
                            else
                            {
                                // missing Currency
                                tmpLastGiftCurrencyCode = "";
                                tmpLastGiftDisplayFormat = "";
                            }

                            // Load Gift Detail
                            AGiftDetailAccess.LoadViaAGift(LastGiftDS,
                                GiftDR.LedgerNumber,
                                GiftDR.BatchNumber,
                                GiftDR.GiftTransactionNumber,
                                StringHelper.InitStrArr(new String[] { AGiftDetailTable.GetGiftTransactionAmountDBName(),
                                                                       AGiftDetailTable.GetRecipientKeyDBName(),
                                                                       AGiftDetailTable.
                                                                       GetRecipientLedgerNumberDBName(),
                                                                       AGiftDetailTable.GetConfidentialGiftFlagDBName() }),
                                Transaction,
                                null,
                                0,
                                0);
                            GiftDetailDT = (AGiftDetailTable)LastGiftDS.Tables[AGiftDetailTable.GetTableName()];

                            if (GiftDetailDT.Rows.Count != 0)
                            {
                                if (GiftDR.LastDetailNumber > 1)
                                {
                                    // Gift is a Split Gift
                                    tmpLastGiftAmount = 0;

                                    for (Counter = 0; Counter <= GiftDetailDT.Rows.Count - 1; Counter += 1)
                                    {
                                        GiftDetailDR = (AGiftDetailRow)GiftDetailDT.Rows[Counter];

                                        // Check for confidential gift and whether the current user is allowed to see it
                                        if (GiftDetailDR.ConfidentialGiftFlag)
                                        {
                                            if (!((UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE2))
                                                  || (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE3))))
                                            {
                                                // User isn't allowed to see the gift
                                                tmpRestrictedOrConfidentialGiftAccessDenied = true;
                                                tmpLastGiftAmount = 0;
                                                return;
                                            }
                                        }

                                        tmpLastGiftAmount = tmpLastGiftAmount + GiftDetailDR.GiftTransactionAmount;
                                    }

                                    tmpLastGiftGivenToShortName = "";
                                    tmpLastGiftRecipientLedgerShortName = "";
                                    tmpLastGiftGivenToPartnerKey = -1;
                                    tmpLastGiftRecipientLedger = -1;
                                }
                                else
                                {
                                    // Gift isn't a Split Gift
                                    GiftDetailDR = (AGiftDetailRow)GiftDetailDT.Rows[0];

                                    // Check for confidential gift and whether the current user is allowed to see it
                                    if (GiftDetailDR.ConfidentialGiftFlag)
                                    {
                                        if (!((UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE2))
                                              || (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE3))))
                                        {
                                            // User isn't allowed to see the gift
                                            tmpRestrictedOrConfidentialGiftAccessDenied = true;
                                            return;
                                        }
                                    }

                                    tmpLastGiftAmount = GiftDetailDR.GiftTransactionAmount;
                                    tmpLastGiftGivenToPartnerKey = GiftDetailDR.RecipientKey;

                                    // Get Partner ShortName
                                    PPartnerAccess.LoadByPrimaryKey(LastGiftDS, GiftDetailDR.RecipientKey,
                                        StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), Transaction, null, 0, 0);

                                    if (LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Count != 0)
                                    {
                                        tmpLastGiftGivenToShortName =
                                            ((PPartnerRow)(LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows[0])).PartnerShortName;
                                    }
                                    else
                                    {
                                        // missing Partner
                                        tmpLastGiftGivenToShortName = "";
                                    }

                                    // Get rid of last record because we are about to select again into the same DataTable...
                                    LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Clear();

                                    // Get Recipient Ledger
                                    PPartnerAccess.LoadByPrimaryKey(LastGiftDS, GiftDetailDR.RecipientLedgerNumber,
                                        StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), Transaction, null, 0, 0);

                                    if (LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Count != 0)
                                    {
                                        tmpLastGiftRecipientLedgerShortName =
                                            ((PPartnerRow)(LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows[0])).PartnerShortName;
                                    }
                                    else
                                    {
                                        // missing Ledger
                                        tmpLastGiftRecipientLedgerShortName = "";
                                    }
                                }
                            }
                            else
                            {
                                // missing Gift Detail
                                tmpLastGiftAmount = 0;
                                tmpLastGiftGivenToShortName = "";
                                tmpLastGiftRecipientLedgerShortName = "";
                                tmpLastGiftGivenToPartnerKey = -1;
                                tmpLastGiftRecipientLedger = -1;
                            }
                        }
                        else
                        {
                            // Gift is a restriced Gift and the current user isn't allowed to see it
                            tmpRestrictedOrConfidentialGiftAccessDenied = true;
                        }
                    }
                    finally
                    {
                        TLogging.LogAtLevel(7, "TGift.GetLastGiftDetails: committed own transaction.");
                    }
                });

            ALastGiftDate = tmpLastGiftDate;
            ALastGiftAmount = tmpLastGiftAmount;
            ALastGiftGivenToPartnerKey = tmpLastGiftGivenToPartnerKey;
            ALastGiftRecipientLedger = tmpLastGiftRecipientLedger;
            ALastGiftCurrencyCode = tmpLastGiftCurrencyCode;
            ALastGiftDisplayFormat = tmpLastGiftDisplayFormat;
            ALastGiftGivenToShortName = tmpLastGiftGivenToShortName;
            ALastGiftRecipientLedgerShortName = tmpLastGiftRecipientLedgerShortName;
            ARestrictedOrConfidentialGiftAccessDenied = tmpRestrictedOrConfidentialGiftAccessDenied;

            return AccessToGift;
        }

        /// <summary>
        /// Check if a gift is really restricted or if the user belongs to the group that is allowed
        /// to access the gift
        /// </summary>
        /// <param name="gift">the gift we want to check for restriction</param>
        /// <param name="ATransaction">A TDBTransaction object for reuse</param>
        /// <returns>true if the user has no permission and the gift is restricted
        ///</returns>
        public static bool GiftRestricted(AGiftRow gift, TDBTransaction ATransaction)
        {
            SGroupGiftTable GroupGiftDT;
            SUserGroupTable UserGroupDT;
            Int16 Counter;

            DataRow[] FoundUserGroups;

            if (gift.Restricted)
            {
                GroupGiftDT = SGroupGiftAccess.LoadViaAGift(
                    gift.LedgerNumber,
                    gift.BatchNumber,
                    gift.GiftTransactionNumber,
                    ATransaction);
                UserGroupDT = SUserGroupAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, ATransaction);

                // Loop over all rows of GroupGiftDT
                for (Counter = 0; Counter <= GroupGiftDT.Rows.Count - 1; Counter += 1)
                {
                    // To be able to view a Gift, ReadAccess must be granted
                    if (GroupGiftDT[Counter].ReadAccess)
                    {
                        // Find out whether the user has a row in s_user_group with the
                        // GroupID of the GroupGift row
                        FoundUserGroups = UserGroupDT.Select(SUserGroupTable.GetGroupIdDBName() + " = '" + GroupGiftDT[Counter].GroupId + "'");

                        if (FoundUserGroups.Length != 0)
                        {
                            // The gift is not restricted because there is a read access for the group
                            return false;
                            // don't evaluate further GroupGiftDT rows
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets TaxDeductiblePct and uses it to calculate the tax deductibility amounts for a Gift Detail
        /// </summary>
        /// <param name="AGiftDetail">Calculated amounts are added to this row</param>
        /// <param name="ADateEntered"></param>
        /// <param name="ATransaction"></param>
        public static void SetDefaultTaxDeductibilityData(
            ref AGiftDetailRow AGiftDetail, DateTime ADateEntered, TDBTransaction ATransaction)
        {
            bool FoundTaxDeductiblePct = false;

            // if the gift it tax deductible
            if (AGiftDetail.TaxDeductible)
            {
                AMotivationDetailTable Tbl = AMotivationDetailAccess.LoadByPrimaryKey(
                    AGiftDetail.LedgerNumber, AGiftDetail.MotivationGroupCode, AGiftDetail.MotivationDetailCode, ATransaction);
                AMotivationDetailRow MotivationDetailRow;

                Boolean HasTaxDeductibleAccountCode = false;

                if (Tbl.Rows.Count > 0)
                {
                    MotivationDetailRow = Tbl[0];
                    HasTaxDeductibleAccountCode = !string.IsNullOrEmpty(MotivationDetailRow.TaxDeductibleAccountCode);
                }

                // if the gift's motivation detail has a tax-deductible account
                if (HasTaxDeductibleAccountCode)
                {
                    // default pct is 100
                    AGiftDetail.TaxDeductiblePct = 100;
                    FoundTaxDeductiblePct = true;

                    PPartnerTaxDeductiblePctTable PartnerTaxDeductiblePctTable =
                        PPartnerTaxDeductiblePctAccess.LoadViaPPartner(AGiftDetail.RecipientKey, ATransaction);

                    // search for tax deductible pct for recipient
                    foreach (PPartnerTaxDeductiblePctRow Row in PartnerTaxDeductiblePctTable.Rows)
                    {
                        if (Row.DateValidFrom <= ADateEntered)
                        {
                            AGiftDetail.TaxDeductiblePct = Row.PercentageTaxDeductible;
                            break;
                        }
                    }
                }
            }

            // if a tax deductible pct is set for the recipient
            if (FoundTaxDeductiblePct)
            {
                // calculate TaxDeductibleAmount and NonDeductibleAmount for all three currencies
                TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref AGiftDetail);
            }

            // if gift is not tax deductible or motivation detail does not hace a tax deductible account
            if (!AGiftDetail.TaxDeductible || !FoundTaxDeductiblePct)
            {
                AGiftDetail.TaxDeductiblePct = 0;
                AGiftDetail.NonDeductibleAmount = AGiftDetail.GiftTransactionAmount;
                AGiftDetail.NonDeductibleAmountBase = AGiftDetail.GiftAmount;
                AGiftDetail.NonDeductibleAmountIntl = AGiftDetail.GiftAmountIntl;
            }
        }
    }
}