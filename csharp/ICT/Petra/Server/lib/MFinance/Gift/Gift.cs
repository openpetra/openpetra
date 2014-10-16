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

                if (TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(SharedConstants.SYSDEFAULT_DISPLAYGIFTAMOUNT) == "true")
                {
                    if ((UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE1))
                        || (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE2))
                        || (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_FINANCE3))
                        || (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_DEVUSER)))
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
            Boolean ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
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

            if ((!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapINQUIRE, AGiftTable.GetTableDBName())))
            {
                // User hasn't got access to a_gift Table in the DB
                ReturnValue = false;
                return ReturnValue;
            }

            // Set up temp DataSet
            LastGiftDS = new DataSet("LastGiftDetails");
            LastGiftDS.Tables.Add(new AGiftTable());
            LastGiftDS.Tables.Add(new AGiftBatchTable());
            LastGiftDS.Tables.Add(new AGiftDetailTable());
            LastGiftDS.Tables.Add(new ACurrencyTable());
            LastGiftDS.Tables.Add(new PPartnerTable());
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    AGiftAccess.LoadViaPPartner(LastGiftDS, APartnerKey, null, ReadTransaction,
                        StringHelper.InitStrArr(new String[] { "ORDER BY", AGiftTable.GetDateEnteredDBName() + " DESC" }), 0, 1);
                }
                catch (ESecurityDBTableAccessDeniedException)
                {
                    // User hasn't got access to a_gift Table in the DB
                    ReturnValue = false;
                    return ReturnValue;
                }
                catch (Exception)
                {
                    throw;
                }

                if (LastGiftDS.Tables[AGiftTable.GetTableName()].Rows.Count == 0)
                {
                    // Partner hasn't given any Gift so far
                    ReturnValue = false;
                    return ReturnValue;
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
                        ReadTransaction);
                    UserGroupDT = SUserGroupAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, ReadTransaction);

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
                    ALastGiftDate = GiftDR.DateEntered;

                    // Console.WriteLine('GiftDR.LedgerNumber: ' + GiftDR.LedgerNumber.ToString + '; ' +
                    // 'GiftDR.BatchNumber:  ' + GiftDR.BatchNumber.ToString);
                    // Load Gift Batch
                    AGiftBatchAccess.LoadByPrimaryKey(LastGiftDS, GiftDR.LedgerNumber, GiftDR.BatchNumber,
                        StringHelper.InitStrArr(new String[] { AGiftBatchTable.GetCurrencyCodeDBName() }), ReadTransaction, null, 0, 0);

                    if (LastGiftDS.Tables[AGiftBatchTable.GetTableName()].Rows.Count != 0)
                    {
                        GiftBatchDR = ((AGiftBatchRow)LastGiftDS.Tables[AGiftBatchTable.GetTableName()].Rows[0]);
                        ALastGiftCurrencyCode = GiftBatchDR.CurrencyCode;

                        // Get Currency
                        ACurrencyAccess.LoadByPrimaryKey(LastGiftDS, GiftBatchDR.CurrencyCode, ReadTransaction);

                        if (LastGiftDS.Tables[ACurrencyTable.GetTableName()].Rows.Count != 0)
                        {
                            CurrencyDR = (ACurrencyRow)(LastGiftDS.Tables[ACurrencyTable.GetTableName()].Rows[0]);
                            ALastGiftCurrencyCode = CurrencyDR.CurrencyCode;
                            ALastGiftDisplayFormat = CurrencyDR.DisplayFormat;
                        }
                        else
                        {
                            ALastGiftCurrencyCode = "";
                            ALastGiftDisplayFormat = "";
                        }
                    }
                    else
                    {
                        // missing Currency
                        ALastGiftCurrencyCode = "";
                        ALastGiftDisplayFormat = "";
                    }

                    // Load Gift Detail
                    AGiftDetailAccess.LoadViaAGift(LastGiftDS,
                        GiftDR.LedgerNumber,
                        GiftDR.BatchNumber,
                        GiftDR.GiftTransactionNumber,
                        StringHelper.InitStrArr(new String[] { AGiftDetailTable.GetGiftTransactionAmountDBName(),
                                                               AGiftDetailTable.GetRecipientKeyDBName(),
                                                               AGiftDetailTable.
                                                               GetRecipientLedgerNumberDBName(), AGiftDetailTable.GetConfidentialGiftFlagDBName() }),
                        ReadTransaction,
                        null,
                        0,
                        0);
                    GiftDetailDT = (AGiftDetailTable)LastGiftDS.Tables[AGiftDetailTable.GetTableName()];

                    if (GiftDetailDT.Rows.Count != 0)
                    {
                        if (GiftDR.LastDetailNumber > 1)
                        {
                            // Gift is a Split Gift
                            ALastGiftAmount = 0;

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
                                        ARestrictedOrConfidentialGiftAccessDenied = true;
                                        ALastGiftAmount = 0;
                                        ReturnValue = false;
                                        return ReturnValue;
                                    }
                                }

                                ALastGiftAmount = ALastGiftAmount + GiftDetailDR.GiftTransactionAmount;
                            }

                            ALastGiftGivenToShortName = "";
                            ALastGiftRecipientLedgerShortName = "";
                            ALastGiftGivenToPartnerKey = -1;
                            ALastGiftRecipientLedger = -1;
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
                                    ARestrictedOrConfidentialGiftAccessDenied = true;
                                    ReturnValue = false;
                                    return ReturnValue;
                                }
                            }

                            ALastGiftAmount = GiftDetailDR.GiftTransactionAmount;
                            ALastGiftGivenToPartnerKey = GiftDetailDR.RecipientKey;

                            // Get Partner ShortName
                            PPartnerAccess.LoadByPrimaryKey(LastGiftDS, GiftDetailDR.RecipientKey,
                                StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), ReadTransaction, null, 0, 0);

                            if (LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Count != 0)
                            {
                                ALastGiftGivenToShortName = ((PPartnerRow)(LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows[0])).PartnerShortName;
                            }
                            else
                            {
                                // missing Partner
                                ALastGiftGivenToShortName = "";
                            }

                            // Get rid of last record because we are about to select again into the same DataTable...
                            LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Clear();

                            // Get Recipient Ledger
                            PPartnerAccess.LoadByPrimaryKey(LastGiftDS, GiftDetailDR.RecipientLedgerNumber,
                                StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), ReadTransaction, null, 0, 0);

                            if (LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows.Count != 0)
                            {
                                ALastGiftRecipientLedgerShortName =
                                    ((PPartnerRow)(LastGiftDS.Tables[PPartnerTable.GetTableName()].Rows[0])).PartnerShortName;
                            }
                            else
                            {
                                // missing Ledger
                                ALastGiftRecipientLedgerShortName = "";
                            }
                        }
                    }
                    else
                    {
                        // missing Gift Detail
                        ALastGiftAmount = 0;
                        ALastGiftGivenToShortName = "";
                        ALastGiftRecipientLedgerShortName = "";
                        ALastGiftGivenToPartnerKey = -1;
                        ALastGiftRecipientLedger = -1;
                    }
                }
                else
                {
                    // Gift is a restriced Gift and the current user isn't allowed to see it
                    ARestrictedOrConfidentialGiftAccessDenied = true;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TGift.GetLastGiftDetails: committed own transaction.");
                }
            }

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
        /// <param name="GiftDetailRow">Calculated amounts are added to this row</param>
        /// <param name="ADateEntered"></param>
        /// <param name="ATransaction"></param>
        public static void SetDefaultTaxDeductibilityData(
        	ref GiftBatchTDSAGiftDetailRow GiftDetailRow, DateTime ADateEntered, TDBTransaction ATransaction)
        {
        	bool FoundTaxDeductiblePct = false;
                                    		
    		// if the gift it tax deductible
        	if (GiftDetailRow.TaxDeductible)
        	{
        		AMotivationDetailRow MotivationDetailRow = AMotivationDetailAccess.LoadByPrimaryKey(
        			GiftDetailRow.LedgerNumber, GiftDetailRow.MotivationGroupCode, GiftDetailRow.MotivationDetailCode, ATransaction)[0];
        		
        		// if the gift's motivation detail has a tax-deductible account
        		if (!string.IsNullOrEmpty(MotivationDetailRow.TaxDeductibleAccount))
        		{
        			// default pct is 100
        			GiftDetailRow.TaxDeductiblePct = 100;
					FoundTaxDeductiblePct = true;
        					
        			PPartnerTaxDeductiblePctTable PartnerTaxDeductiblePctTable = 
        				PPartnerTaxDeductiblePctAccess.LoadViaPPartner(GiftDetailRow.RecipientKey, ATransaction);
        			
        			// search for tax deductible pct for recipient
        			foreach (PPartnerTaxDeductiblePctRow Row in PartnerTaxDeductiblePctTable.Rows)
        			{
        				if (Row.DateValidFrom <= ADateEntered)
        				{
        					GiftDetailRow.TaxDeductiblePct = Row.PercentageTaxDeductible;
        					break;
        				}
        			}
        		}
        	}
        	
        	// if a tax deductible pct is set for the recipient
        	if (FoundTaxDeductiblePct)
        	{
        		// calculate TaxDeductibleAmount and NonDeductibleAmount for all three currencies
        		TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref GiftDetailRow);
        	}
        	
        	// if gift is not tax deductible or motivation detail does not hace a tax deductible account
        	if (!GiftDetailRow.TaxDeductible || !FoundTaxDeductiblePct)
        	{
            	GiftDetailRow.TaxDeductiblePct = 0;
            	GiftDetailRow.NonDeductibleAmount = GiftDetailRow.GiftTransactionAmount;
            	GiftDetailRow.NonDeductibleAmountBase = GiftDetailRow.GiftAmount;
            	GiftDetailRow.NonDeductibleAmountIntl = GiftDetailRow.GiftAmountIntl;
        	}
        }
    }
}