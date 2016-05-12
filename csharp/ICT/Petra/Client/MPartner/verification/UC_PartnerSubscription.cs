//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MPartner.Verification
{
    /// <summary>
    /// Contains verification logic for the UC_PartnerSubscription UserControl.
    /// </summary>
    public class TPartnerSubscriptionVerification
    {
        #region Resourcestrings

        private static readonly string StrGiftGivenByMandatory = Catalog.GetString(
            "You need to specify the Partner who has made the gift for this Subscription!" + "\r\n" +
            "Either choose the 'Gift Given By' button to select the Partner or enter\r\n" +
            "the Partners' Partner Key in the Field next to it.\r\n\r\n" +
            "Note: If the price for the Subscription is no longer paid by another Partner, you need to select\r\n" +
            "a different Subscription Status than 'GIFT'.");

        private static readonly string StrGiftGivenByMandatoryTitle = Catalog.GetString("Gift Given By Mandatory");

        #endregion

        #region TPartnerSubscriptionVerification

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="FDataColumnComparedTo"></param>
        /// <returns></returns>
        public static Boolean VerifySubscriptionData(DataColumnChangeEventArgs e,
            TVerificationResultCollection AVerificationResultCollection,
            out TVerificationResult AVerificationResult,
            out DataColumn FDataColumnComparedTo)
        {
            Boolean ReturnValue;

            AVerificationResult = null;
            DataColumn FDataColumnComparedTo2 = null;

            if ((e.Column.ColumnName == PSubscriptionTable.GetDateCancelledDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetExpiryDateDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetDateNoticeSentDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetStartDateDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetSubscriptionRenewalDateDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetFirstIssueDBName()) || (e.Column.ColumnName == PSubscriptionTable.GetLastIssueDBName()))
            {
                VerifySubscriptionDates(e, AVerificationResultCollection, out AVerificationResult, out FDataColumnComparedTo2);
            }

            FDataColumnComparedTo = FDataColumnComparedTo2;

            // if (e.Column.Ordinal = (e.Column.Table as PPartnerLocationTable).ColumnEmailAddress.Ordinal) then
            // begin
            // VerifyEMailAddress(e, AVerificationResult);
            // end;
            if ((e.Column.ColumnName == PSubscriptionTable.GetPublicationCopiesDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetNumberIssuesReceivedDBName())
                || (e.Column.ColumnName == PSubscriptionTable.GetNumberComplimentaryDBName()))
            {
                VerifyInteger(e, out AVerificationResult);
            }

            // any verification errors?
            if (AVerificationResult == null)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="VerificationResult"></param>
        /// <param name="AErroneousDC"></param>
        /// <param name="FtmpPartnerKeyValid"></param>
        /// <returns></returns>
        public static Boolean VerifySubscriptionDataFinal(PSubscriptionRow Row,
            out TVerificationResultCollection AVerificationResultCollection,
            out TVerificationResult VerificationResult,
            out DataColumn AErroneousDC,
            Boolean FtmpPartnerKeyValid)
        {
            Boolean ReturnValue;
            Boolean Completed;

            //Boolean NoErrors;
            TVerificationResultCollection TmpCollection = null;

            //TVerificationResult TmpError;
            DataColumn TmpDC;
            DataColumn NilDataColumn;
            String mVerifiedString;
            bool mPartnerExists;
            TPartnerClass mPartnerClass;
            TStdPartnerStatusCode partnerStatus;

            TPartnerClass[] mPartnerClassSet;
            Completed = false;
            ReturnValue = false;
            VerificationResult = null;
            AVerificationResultCollection = null;
            AErroneousDC = null;

            while (!Completed)
            {
                TmpDC = Row.Table.Columns[PSubscriptionTable.GetStartDateDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetStartDateDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetExpiryDateDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetExpiryDateDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetSubscriptionRenewalDateDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetDateNoticeSentDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetDateNoticeSentDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetDateCancelledDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetDateCancelledDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetFirstIssueDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetFirstIssueDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetLastIssueDBName()];
                VerifySubscriptionDates(new DataColumnChangeEventArgs(Row, TmpDC,
                        Row[PSubscriptionTable.GetLastIssueDBName()]), TmpCollection, out VerificationResult, out NilDataColumn);

                if (VerificationResult != null)
                {
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                // if subscription status is cancelled, is there a partner that gives the gift.
                TmpDC = Row.Table.Columns[PSubscriptionTable.GetGiftFromKeyDBName()];

                if (Row.IsSubscriptionStatusNull())
                {
                    // we go no further:
                    VerificationResult = new TVerificationResult("",
                        "Please select a valid subscription status",
                        "Subscription Status Mandatory",
                        "X_0041",
                        TResultSeverity.Resv_Critical);
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;

                    // escape before we crash
                    return false;
                }

                if (Row.SubscriptionStatus == "GIFT")
                {
                    // validate partner key again, to be sure!
                    mPartnerClassSet = new TPartnerClass[0];
                    FtmpPartnerKeyValid = TServerLookup.TMPartner.VerifyPartner(Row.GiftFromKey,
                        mPartnerClassSet,
                        out mPartnerExists,
                        out mVerifiedString,
                        out mPartnerClass,
                        out partnerStatus);
                }

                if ((Row.SubscriptionStatus == "GIFT") && ((!FtmpPartnerKeyValid) || (Row.GiftFromKey == 00000000)))
                {
                    // MessageBox.Show('FtmpPartnerKeyValid: ' + FtmpPartnerKeyValid.toString);
                    VerificationResult = new TVerificationResult("",
                        StrGiftGivenByMandatory,
                        StrGiftGivenByMandatoryTitle,
                        "",
                        TResultSeverity.Resv_Critical);
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetReasonSubsCancelledCodeDBName()];

                if ((Row.SubscriptionStatus == "CANCELLED") && (Row.IsReasonSubsCancelledCodeNull()))
                {
                    VerificationResult = new TVerificationResult("",
                        "Please select reason for the cancellation",
                        "Reason for Cancellation Mandatory",
                        "X_0041",
                        TResultSeverity.Resv_Critical);
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }
                else
                {
                    if ((Row.SubscriptionStatus == "CANCELLED") && (Row.ReasonSubsCancelledCode.Length < 2))
                    {
                        VerificationResult = new TVerificationResult("",
                            "Please select reason for the cancellation",
                            "Reason for Cancellation Mandatory",
                            "X_0041",
                            TResultSeverity.Resv_Critical);
                        Completed = true;
                        ReturnValue = false;
                        AErroneousDC = TmpDC;
                        break;
                    }
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetReasonSubsCancelledCodeDBName()];

                if ((Row.SubscriptionStatus == "EXPIRED") && (Row.IsReasonSubsCancelledCodeNull()))
                {
                    VerificationResult = new TVerificationResult("",
                        "Please select reason for the cancellation",
                        "Reason for Cancellation Mandatory",
                        "X_0041",
                        TResultSeverity.Resv_Critical);
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }
                else
                {
                    if ((Row.SubscriptionStatus == "EXPIRED") && (Row.ReasonSubsCancelledCode.Length < 2))
                    {
                        VerificationResult = new TVerificationResult("",
                            "Please select reason for the cancellation",
                            "Reason for Cancellation Mandatory",
                            "X_0041",
                            TResultSeverity.Resv_Critical);
                        Completed = true;
                        ReturnValue = false;
                        AErroneousDC = TmpDC;
                        break;
                    }
                }

                TmpDC = Row.Table.Columns[PSubscriptionTable.GetReasonSubsGivenCodeDBName()];
                try
                {
                    if (Row.ReasonSubsGivenCode.Length < 2)
                    {
                        VerificationResult = new TVerificationResult("",
                            "Please select reason for the Gift",
                            "Reason for Gift Mandatory",
                            "X_0041",
                            TResultSeverity.Resv_Critical);
                        Completed = true;
                        ReturnValue = false;
                        AErroneousDC = TmpDC;
                        break;
                    }
                }
                catch (Exception)
                {
                    VerificationResult = new TVerificationResult("",
                        "Please select reason why the publication is given",
                        "Reason for Gift Mandatory",
                        "X_0041",
                        TResultSeverity.Resv_Critical);
                    Completed = true;
                    ReturnValue = false;
                    AErroneousDC = TmpDC;
                    break;
                }
                Completed = true;
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="FDataColumnComparedTo"></param>
        public static void VerifySubscriptionDates(DataColumnChangeEventArgs e,
            TVerificationResultCollection AVerificationResultCollection,
            out TVerificationResult AVerificationResult,
            out DataColumn FDataColumnComparedTo)
        {
            FDataColumnComparedTo = null;
            AVerificationResult = null;
            DateTime DateStarted = new DateTime();
            DateTime DateExpired = new DateTime();
            DateTime DateRenewed = new DateTime();
            DateTime DateNoticeSent = new DateTime();
            DateTime DateEnded = new DateTime();
            DateTime DateFirstSent = new DateTime();
            DateTime DateLastSent = new DateTime();
            Boolean Completed;
            Completed = false;

            if (e.Column.ColumnName == PSubscriptionTable.GetStartDateDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetExpiryDateDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetSubscriptionRenewalDateDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetDateNoticeSentDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetDateCancelledDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetFirstIssueDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.ProposedValue);
                DateLastSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetLastIssueDBName()]);
            }

            if (e.Column.ColumnName == PSubscriptionTable.GetLastIssueDBName())
            {
                DateStarted = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetStartDateDBName()]);
                DateExpired = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetExpiryDateDBName()]);
                DateRenewed = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetSubscriptionRenewalDateDBName()]);
                DateNoticeSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateNoticeSentDBName()]);
                DateEnded = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetDateCancelledDBName()]);
                DateFirstSent = TSaveConvert.ObjectToDate(e.Row[PSubscriptionTable.GetFirstIssueDBName()]);
                DateLastSent = TSaveConvert.ObjectToDate(e.ProposedValue);
            }

            while (!Completed)
            {
                // when the StartDate has changed, do this:
                if (e.Column.ColumnName == PSubscriptionTable.GetStartDateDBName())
                {
                    FDataColumnComparedTo = e.Column;
                    TPartnerSubscriptionVerification.VerifyDatesAgainstStartDate(DateStarted,
                        DateExpired,
                        DateRenewed,
                        DateNoticeSent,
                        DateEnded,
                        DateFirstSent,
                        DateLastSent,
                        out AVerificationResult,
                        out Completed);

                    if (Completed == true)
                    {
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetExpiryDateDBName())
                {
                    FDataColumnComparedTo = e.Column;
                    TPartnerSubscriptionVerification.VerifyDatesAgainstStartDate(DateStarted,
                        DateExpired,
                        DateRenewed,
                        DateNoticeSent,
                        DateEnded,
                        DateFirstSent,
                        DateLastSent,
                        out AVerificationResult,
                        out Completed);

                    if (Completed == true)
                    {
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetSubscriptionRenewalDateDBName())
                {
                    FDataColumnComparedTo = e.Column;
                    TPartnerSubscriptionVerification.VerifyDatesAgainstStartDate(DateStarted,
                        DateExpired,
                        DateRenewed,
                        DateNoticeSent,
                        DateEnded,
                        DateFirstSent,
                        DateLastSent,
                        out AVerificationResult,
                        out Completed);

                    if (Completed == true)
                    {
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed, DateTime.Today, "Date Renewed", "Today") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed, DateTime.Today, "DateRenewed", "Today");
                        Completed = true;
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed, DateExpired, "Date Renewed", "Date Expired") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed, DateExpired, "Date Renewed", "Date Expired");
                        Completed = true;
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed, DateNoticeSent, "Date Renewed", "Date Notice Sent") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateRenewed,
                            DateNoticeSent,
                            "Date Renewed",
                            "Date Notice Sent");
                        Completed = true;
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetDateNoticeSentDBName())
                {
                    FDataColumnComparedTo = e.Column;
                    TPartnerSubscriptionVerification.VerifyDatesAgainstStartDate(DateStarted,
                        DateExpired,
                        DateRenewed,
                        DateNoticeSent,
                        DateEnded,
                        DateFirstSent,
                        DateLastSent,
                        out AVerificationResult,
                        out Completed);

                    if (Completed == true)
                    {
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetDateCancelledDBName())
                {
                    FDataColumnComparedTo = e.Column;
                    TPartnerSubscriptionVerification.VerifyDatesAgainstStartDate(DateStarted,
                        DateExpired,
                        DateRenewed,
                        DateNoticeSent,
                        DateEnded,
                        DateFirstSent,
                        DateLastSent,
                        out AVerificationResult,
                        out Completed);

                    if (Completed == true)
                    {
                        break;
                    }

                    if (TDateChecks.FirstLesserThanSecondDate(DateTime.Today, DateEnded, "Today", "Cancelled") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Today, DateEnded, "Today", "Cancelled");
                        Completed = true;
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetFirstIssueDBName())
                {
                    FDataColumnComparedTo = e.Column;

                    if (TDateChecks.FirstGreaterOrEqualThanSecondDate(DateLastSent, DateFirstSent, "Last Sent", "First Sent") != null)
                    {
                        AVerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateLastSent, DateFirstSent, "Last Sent", "First Sent");
                        Completed = true;
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateFirstSent, "Date Started", "First Sent") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateFirstSent, "Date Started", "First Sent");
                        Completed = true;
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateFirstSent, DateTime.Today, "First Sent", "today") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateFirstSent, DateTime.Today, "First Sent", "today");
                        Completed = true;
                        break;
                    }
                }

                if (e.Column.ColumnName == PSubscriptionTable.GetLastIssueDBName())
                {
                    FDataColumnComparedTo = e.Column;

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateFirstSent, DateLastSent, "First Sent", "Last Sent") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateFirstSent, DateLastSent, "First Sent", "Last Sent");
                        Completed = true;
                        break;
                    }

                    if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateLastSent, DateTime.Today, "Last Sent", "today") != null)
                    {
                        AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateLastSent, DateTime.Today, "Last Sent", "today");
                        Completed = true;
                        break;
                    }
                }

                Completed = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="DateStarted"></param>
        /// <param name="DateExpired"></param>
        /// <param name="DateRenewed"></param>
        /// <param name="DateNoticeSent"></param>
        /// <param name="DateEnded"></param>
        /// <param name="DateFirstSent"></param>
        /// <param name="DateLastSent"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="Completed"></param>
        public static void VerifyDatesAgainstStartDate(DateTime DateStarted,
            DateTime DateExpired,
            DateTime DateRenewed,
            DateTime DateNoticeSent,
            DateTime DateEnded,
            DateTime DateFirstSent,
            DateTime DateLastSent,
            out TVerificationResult AVerificationResult,
            out Boolean Completed)
        {
            AVerificationResult = null;
            Completed = false;

            while (!Completed)
            {
                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateExpired, "Start Date", "Expiry Date") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateExpired, "Start Date", "Expiry Date");
                    Completed = true;
                    break;
                }

                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateRenewed, "Start Date", "Renewal Date") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateRenewed, "Start Date", "Renewal Date");
                    Completed = true;
                    break;
                }

                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateEnded, "Start Date", "End Date") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateEnded, "Start Date", "End Date");
                    Completed = true;
                    break;
                }

                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateNoticeSent, "Start Date", "Notice sent") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateNoticeSent, "Start Date", "Notice sent");
                    Completed = true;
                    break;
                }

                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateFirstSent, "Start Date", "First sent") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateFirstSent, "Start Date", "First sent");
                    Completed = true;
                    break;
                }

                if (TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateLastSent, "Start Date", "Last sent") != null)
                {
                    AVerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateStarted, DateLastSent, "Start Date", "Last sent");
                    Completed = true;
                    break;
                }

                Completed = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyInteger(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            AVerificationResult = (TVerificationResult)TNumericalChecks.IsValidInteger(e.ProposedValue.ToString(), "");
        }

        #endregion
    }
}