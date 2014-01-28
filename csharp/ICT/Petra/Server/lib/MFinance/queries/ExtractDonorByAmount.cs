//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Extracts;

namespace Ict.Petra.Server.MFinance.queries
{
    /// <summary>
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryDonorByAmount : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all donors that have given to particular fields (ledgers)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            // Sql statements will be initialized later on in special treatment
            string SqlStmt = "";

            // create a new object of this class and control extract calculation from base class
            QueryDonorByAmount ExtractQuery = new QueryDonorByAmount();

            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public QueryDonorByAmount() : base()
        {
            // This extract involves post processing of a queries
            FSpecialTreatment = true;
        }

        /// <summary>
        /// This method needs to be implemented by extracts that can't follow the default processing with just
        /// one query.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        protected override bool RunSpecialTreatment(TParameterList AParameters, TDBTransaction ATransaction, out int AExtractId)
        {
            Boolean ReturnValue = false;
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out NewTransaction);
            DataTable giftdetails;

            string SqlStmt = TDataBase.ReadSqlFile("Gift.Queries.ExtractDonorByAmount.sql");

            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            bool AddressFilterAdded;
            DataTable partnerkeys = new DataTable();

            AExtractId = -1;

            // call to derived class to retrieve parameters specific for extract
            RetrieveParameters(AParameters, ref SqlStmt, ref SqlParameterList);

            // add address filter information to sql statement and parameter list
            AddressFilterAdded = AddAddressFilter(AParameters, ref SqlStmt, ref SqlParameterList);

            // Now run the database query. This time it is returning gift detail records.
            TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);

            try
            {
                giftdetails = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "giftdetails", Transaction,
                    SqlParameterList.ToArray());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
            // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
            if (AParameters.Get("CancelReportCalculation").ToBool() == true)
            {
                return false;
            }

            TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

            // With the result of the original query process the data and identify the partner keys for
            // the extract.
            partnerkeys.Columns.Add("0", typeof(Int64));
            partnerkeys.Columns.Add("1", typeof(string));
            partnerkeys.Columns.Add("2", typeof(Int64));
            partnerkeys.Columns.Add("3", typeof(Int32));
            ProcessGiftDetailRecords(giftdetails, AddressFilterAdded, AParameters, ref partnerkeys);

            // create an extract with the given name in the parameters
            ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                AParameters.Get("param_extract_name").ToString(),
                AParameters.Get("param_extract_description").ToString(),
                out AExtractId,
                partnerkeys,
                0,
                AddressFilterAdded,
                true);

            return ReturnValue;
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASQLParameterList"></param>
        protected override void RetrieveParameters(TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASQLParameterList)
        {
            ASQLParameterList.Add(new OdbcParameter("param_date_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_from").ToDate()
                });

            ASQLParameterList.Add(new OdbcParameter("param_date_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_to").ToDate()
                });

            ASQLParameterList.Add(new OdbcParameter("param_new_donors_only", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_new_donors_only").ToBool()
                });

            ASQLParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("param_families_only", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_families_only").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });
        }

        /// <summary>
        /// Post processing of db query that retrieved gift detail records to create a list of partner keys
        /// </summary>
        /// <param name="AGiftDetails"></param>
        /// <param name="AAddressFilterAdded"></param>
        /// <param name="AParameters"></param>
        /// <param name="APartnerKeys"></param>
        private void ProcessGiftDetailRecords(DataTable AGiftDetails, bool AAddressFilterAdded,
            TParameterList AParameters, ref DataTable APartnerKeys)
        {
            int PartnerKeyColumn = 0;
            int LedgerNumberColumn = 2;
            int BatchNumberColumn = 3;
            int GiftTransactionNumberColumn = 4;
            int FirstTimeGiftColumn = 5;
            int GiftDetailNumberColumn = 6;
            int GiftAmountColumn = 7;
            int GiftAmountInternationalColumn = 8;
            int SiteKeyColumn = -1;
            int LocationKeyColumn = -1;

            Int32 LedgerNumber = -1;
            Int32 BatchNumber = -1;
            Int32 GiftTransactionNumber = -1;
            Int32 GiftDetailNumber = -1;
            Int64 PartnerKey = -1;
            Int64 SiteKey = 0;
            Int32 LocationKey = 0;

            Int32 PreviousLedgerNumber = -1;
            Int32 PreviousBatchNumber = -1;
            Int32 PreviousGiftTransactionNumber = -1;
            Int32 PreviousGiftDetailNumber = -1;
            Int64 PreviousPartnerKey = -1;
            Int64 PreviousSiteKey = 0;
            Int32 PreviousLocationKey = 0;

            int CountGifts = 0;
            decimal GiftAmountTotal = 0;
            decimal GiftAmount;
            decimal GiftAmountInternationalTotal = 0;
            decimal GiftAmountInternational;
            bool ReversedGift = false;

            Decimal MinAmount = 0;
            Decimal MaxAmount = 999999999.99M;
            bool AmountPerSingleGift = false;
            Int32 MinNumberOfGifts = 0;
            Int32 MaxNumberOfGifts = 999999;
            bool BaseCurrency = true;
            bool NewDonor = false;
            bool NewDonorsOnly = false;

            bool NextGift = false;
            bool NextPartner = false;

            // initialize parameters needed from parameter list
            if (!AParameters.Get("param_new_donors_only").IsNil())
            {
                NewDonorsOnly = AParameters.Get("param_new_donors_only").ToBool();
            }

            if (!AParameters.Get("param_min_gift_amount").IsNil())
            {
                MinAmount = AParameters.Get("param_min_gift_amount").ToDecimal();
            }

            if (!AParameters.Get("param_max_gift_amount").IsNil())
            {
                MaxAmount = AParameters.Get("param_max_gift_amount").ToDecimal();
            }

            if (!AParameters.Get("param_amount_per_single_gift").IsNil())
            {
                AmountPerSingleGift = AParameters.Get("param_amount_per_single_gift").ToBool();
            }

            if (!AParameters.Get("param_min_number_gifts").IsNil())
            {
                MinNumberOfGifts = AParameters.Get("param_min_number_gifts").ToInt32();
            }

            if (!AParameters.Get("param_max_number_gifts").IsNil())
            {
                MaxNumberOfGifts = AParameters.Get("param_max_number_gifts").ToInt32();
            }

            if (!AParameters.Get("param_currency").IsNil()
                && (AParameters.Get("param_currency").ToString() == "InternationalCurrency"))
            {
                BaseCurrency = false;
            }

            // only set columns for address related information if address filter was added
            if (AAddressFilterAdded)
            {
                SiteKeyColumn = 9;
                LocationKeyColumn = 10;
            }

            // now start processing rows retrieved from database to filter according to criteria
            foreach (DataRow giftDetailRow in AGiftDetails.Rows)
            {
                LedgerNumber = Convert.ToInt32(giftDetailRow[LedgerNumberColumn]);
                BatchNumber = Convert.ToInt32(giftDetailRow[BatchNumberColumn]);
                GiftTransactionNumber = Convert.ToInt32(giftDetailRow[GiftTransactionNumberColumn]);
                GiftDetailNumber = Convert.ToInt32(giftDetailRow[GiftDetailNumberColumn]);
                PartnerKey = Convert.ToInt64(giftDetailRow[PartnerKeyColumn]);

                if (AAddressFilterAdded)
                {
                    SiteKey = Convert.ToInt64(giftDetailRow[SiteKeyColumn]);
                    LocationKey = Convert.ToInt32(giftDetailRow[LocationKeyColumn]);

                    // if key field values have not changed then this record is the same with just
                    // a different location key --> skip this record as otherwise gift amounts and
                    // numbers of gifts would be wrongly multiplied.
                    if ((LedgerNumber == PreviousLedgerNumber)
                        && (BatchNumber == PreviousBatchNumber)
                        && (GiftTransactionNumber == PreviousGiftTransactionNumber)
                        && (GiftDetailNumber == PreviousGiftDetailNumber)
                        && (PartnerKey == PreviousPartnerKey))
                    {
                        continue;
                    }
                }

                // check for new partner record (unless this is the first record)
                if (PreviousPartnerKey != -1)
                {
                    if (PartnerKey != PreviousPartnerKey)
                    {
                        NextPartner = true;
                    }
                }

                // check for new gift record (unless this is the first record)
                if (PreviousGiftTransactionNumber != -1)
                {
                    if (!((LedgerNumber == PreviousLedgerNumber)
                          && (BatchNumber == PreviousBatchNumber)
                          && (GiftTransactionNumber == PreviousGiftTransactionNumber)))
                    {
                        NextGift = true;
                    }
                }

                // new gift: check for criteria and possibly increase gift counter
                if (NextGift)
                {
                    if (AmountPerSingleGift)
                    {
                        if ((BaseCurrency
                             && (Math.Abs(GiftAmountTotal) >= MinAmount)
                             && (Math.Abs(GiftAmountTotal) <= MaxAmount))
                            || (!BaseCurrency
                                && (Math.Abs(GiftAmountInternationalTotal) >= MinAmount)
                                && (Math.Abs(GiftAmountInternationalTotal) <= MaxAmount)))
                        {
                            if (ReversedGift)
                            {
                                CountGifts = CountGifts - 1;
                            }
                            else
                            {
                                CountGifts = CountGifts + 1;
                            }
                        }

                        GiftAmountTotal = 0;
                        GiftAmountInternationalTotal = 0;
                    }
                    else
                    {
                        if (ReversedGift)
                        {
                            CountGifts = CountGifts - 1;
                        }
                        else
                        {
                            CountGifts = CountGifts + 1;
                        }
                    }

                    // reset variable for reversed gift since a new gift record is starting
                    ReversedGift = false;
                }

                if (NextPartner)
                {
                    // different partner than the record before: check if partner meets criteria for extract
                    // (in case of single gift amounts don't check amounts again)
                    if ((!AmountPerSingleGift
                         && ((BaseCurrency
                              && ((Math.Abs(GiftAmountTotal) < MinAmount)
                                  || (Math.Abs(GiftAmountTotal) > MaxAmount)))
                             || (!BaseCurrency
                                 && ((Math.Abs(GiftAmountInternationalTotal) < MinAmount)
                                     || (Math.Abs(GiftAmountInternationalTotal) > MaxAmount)))))
                        || (CountGifts < MinNumberOfGifts)
                        || (CountGifts > MaxNumberOfGifts)
                        || !NewDonor)
                    {
                        // skip partner as criteria are not fulfilled
                    }
                    else
                    {
                        // add partner to extract
                        APartnerKeys.Rows.Add(PreviousPartnerKey, "", PreviousSiteKey, PreviousLocationKey);
                    }
                }

                if (NextPartner)
                {
                    // reset variables needed for calculation
                    NewDonor = false;
                    CountGifts = 0;
                    GiftAmountTotal = 0;
                    GiftAmountInternationalTotal = 0;
                }

                if (Convert.ToBoolean(giftDetailRow[FirstTimeGiftColumn])
                    || !NewDonorsOnly)
                {
                    NewDonor = true;
                }

                GiftAmount = Convert.ToInt32(giftDetailRow[GiftAmountColumn]);
                GiftAmountTotal = GiftAmountTotal + GiftAmount;
                GiftAmountInternational = Convert.ToInt32(giftDetailRow[GiftAmountInternationalColumn]);
                GiftAmountInternationalTotal = GiftAmountInternationalTotal + GiftAmountInternational;

                if (GiftAmount < 0)
                {
                    ReversedGift = true;
                }

                // prepare for next round of loop
                PreviousLedgerNumber = LedgerNumber;
                PreviousBatchNumber = BatchNumber;
                PreviousGiftTransactionNumber = GiftTransactionNumber;
                PreviousGiftDetailNumber = GiftDetailNumber;
                PreviousPartnerKey = PartnerKey;
                PreviousSiteKey = SiteKey;
                PreviousLocationKey = LocationKey;

                NextPartner = false;
                NextGift = false;
            }

            // process last record after loop through all records has finished
            // (in case of single gift amounts don't check amounts again)
            if ((!AmountPerSingleGift
                 && ((BaseCurrency
                      && ((Math.Abs(GiftAmountTotal) < MinAmount)
                          || (Math.Abs(GiftAmountTotal) > MaxAmount)))
                     || (!BaseCurrency
                         && ((Math.Abs(GiftAmountInternationalTotal) < MinAmount)
                             || (Math.Abs(GiftAmountInternationalTotal) > MaxAmount)))))
                || (CountGifts < MinNumberOfGifts)
                || (CountGifts > MaxNumberOfGifts)
                || !NewDonor)
            {
                // skip partner as criteria are not fulfilled
            }
            else
            {
                // add partner to extract
                APartnerKeys.Rows.Add(PreviousPartnerKey, "", PreviousSiteKey, PreviousLocationKey);
            }
        }
    }
}