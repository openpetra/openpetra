//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timh, timop
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;

namespace Ict.Petra.Server.MPartner.Processing
{
    /// <summary>
    /// Processes the Partner Reminders.
    /// </summary>
    public class TProcessPartnerReminders
    {
        const string SYSTEMDEFAULT_LAST_REMINDER_DATE = "LastReminderDate";
        const string SYSTEMDEFAULT_LAST_REMINDER_DATE_DESC = "Date the Partner Reminder process last ran";
        const string StrRemindersProcessing = " - Partner Reminders Processing";

        /// <summary>
        /// Gets called in regular intervals from a Timer in Class TTimedProcessing.
        /// </summary>
        /// <param name="ADBAccessObj">Already instatiated DB Access object with opened DB connection.</param>
        /// <param name="ARunManually">this is true if the process was called manually from the server admin console</param>
        public static void Process(TDataBase ADBAccessObj, bool ARunManually)
        {
            TDBTransaction ReadWriteTransaction;
            bool NewTransaction;
            bool LastReminderDateAcquired;
            DateTime LastReminderDate;
            DataSet ReminderResultsDS;
            SSystemDefaultsRow SystemDefaultsDR;
            PPartnerReminderTable PartnerReminderDT;
            int ReminderFreqency;
            TDataBase DBAccessObj;

            if (TLogging.DebugLevel >= 6)
            {
                TLogging.Log("Entering TProcessPartnerReminders.Process...");
            }

            // TODO: it is quite ipossible at the moment to use ADBAccessObj instead of DBAccess.GDBAccessObj due to SubmitChanges etc
            //DBAccessObj = ADBAccessObj;
            DBAccessObj = DBAccess.GDBAccessObj;

            // SubmitChanges references a user
            TPetraIdentity PetraIdentity = new TPetraIdentity(
                "SYSADMIN", "", "", "", "", DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, 0, -1, -1, false,
                false);

            UserInfo.GUserInfo = new TPetraPrincipal(PetraIdentity, null);

            ReadWriteTransaction = DBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                out NewTransaction);

            /*
             * This whole process must either succeed or fail, therefore the whole thing is in a try-catch.
             */
            try
            {
                /*
                 * Obtain date when PartnerReminders last ran. This is stored in a SystemDefault. If it doesn't exist already,
                 * a new SystemDefault with an ancient date is created for us.
                 */
                LastReminderDateAcquired = GetLastReminderDate(out LastReminderDate, out SystemDefaultsDR, ReadWriteTransaction);

                if (!LastReminderDateAcquired)
                {
                    TLogging.Log(
                        TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                        ": Could not send Partner Reminders because Petra couldn't create the required SystemDefault setting for the Last Reminder Date!");

                    DBAccessObj.RollbackTransaction();

                    return;
                }

                // Retrieve all PartnerReminders we need to process.
                ReminderResultsDS = GetRemindersToProcess(LastReminderDate, out PartnerReminderDT,
                    DBAccessObj, ReadWriteTransaction);

                /*
                 * We now have a Typed DataTable with the PartnerReminders that we need to process.
                 * Iterate through the PartnerReminders, update data, and send an email for each PartnerReminder.
                 */
                if (TLogging.DebugLevel >= 6)
                {
                    TLogging.Log("_---------------------------------_");
                    TLogging.Log("PartnerReminders data before we start processing all PartnerReminders....");
                    TLogging.Log(ReminderResultsDS.GetXml().ToString());
                }

                foreach (PPartnerReminderRow PartnerReminderDR in PartnerReminderDT.Rows)
                {
                    if (TLogging.DebugLevel >= 4)
                    {
                        TLogging.Log(String.Format(TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                                ": Processing Reminder ID {0} for Partner {1}.", PartnerReminderDR.ReminderId, PartnerReminderDR.PartnerKey));
                    }

                    ReminderFreqency = (PartnerReminderDR.IsReminderFrequencyNull()) ? 0 : PartnerReminderDR.ReminderFrequency;

                    PartnerReminderDR.BeginEdit();
                    PartnerReminderDR.LastReminderSent = DateTime.Now.Date;
                    PartnerReminderDR.NextReminderDate = DateTime.Now.Date.AddDays(ReminderFreqency);

                    if (!PartnerReminderDR.IsEventDateNull())   // Reminder has an Event Date
                    {
                        if (PartnerReminderDR.NextReminderDate > PartnerReminderDR.EventDate)
                        {
                            if (TLogging.DebugLevel >= 5)
                            {
                                TLogging.Log(String.Format(TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                                        ": Deactivating Reminder ID {0} for Partner {1} as its Event Date is in the past.",
                                        PartnerReminderDR.ReminderId, PartnerReminderDR.PartnerKey));
                            }

                            PartnerReminderDR.ReminderActive = false;
                        }
                    }

                    if (TLogging.DebugLevel >= 4)
                    {
                        TLogging.Log(String.Format(TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                                ": Sending email for Reminder ID {0} for Partner {1}.", PartnerReminderDR.ReminderId, PartnerReminderDR.PartnerKey));
                    }

                    if (SendReminderEmail(PartnerReminderDR, ReadWriteTransaction))
                    {
                        // Accept the edit
                        if (TLogging.DebugLevel >= 4)
                        {
                            TLogging.Log(String.Format(TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                                    ": Reminder ID {0} for Partner {1} accepted by SMTP server.", PartnerReminderDR.ReminderId,
                                    PartnerReminderDR.PartnerKey));
                        }

                        PartnerReminderDR.EndEdit();
                    }
                    else
                    {
                        // Cancel the edit
                        TLogging.Log(String.Format(TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing +
                                ": Reminder ID {0} for Partner {1} REJECTED by SMTP server.", PartnerReminderDR.ReminderId,
                                PartnerReminderDR.PartnerKey));

                        PartnerReminderDR.CancelEdit();
                    }
                }

                if (TLogging.DebugLevel >= 6)
                {
                    TLogging.Log("_---------------------------------_");
                    TLogging.Log("PartnerReminders data after processing all PartnerReminders, before writing it to DB....");
                    TLogging.Log(PartnerReminderDT.DataSet.GetXml().ToString());
                }

                // Update all the changed PartnerReminder Rows
                PPartnerReminderAccess.SubmitChanges(PartnerReminderDT, ReadWriteTransaction);

                if (TLogging.DebugLevel >= 6)
                {
                    TLogging.Log("_---------------------------------_");
                }

                /*
                 * Update the SystemDefault that keeps track of when Partner Reminders last ran.
                 * (SystemDefaultsDR will point to the row we loaded earlier on, OR the row we added earlier on
                 * if there wasn't already a SystemDefault row.)
                 */
                UpdateLastReminderDate(SystemDefaultsDR, ReadWriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                TLogging.LogAtLevel(1, TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing + " ran succesfully.");
            }
            catch (Exception Exc)
            {
                TLogging.Log(
                    TTimedProcessing.StrAutomaticProcessing + StrRemindersProcessing + " encountered an Exception:" + Environment.NewLine +
                    Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        #region Helper Methods


        /// <summary>
        /// Determines the 'Last Reminder Date', that is the date when PartnerReminders last ran.
        /// <para>
        /// This is done by reading a certain SystemDefault. If PartnerReminders was never run before,
        /// this SystemDefault is created.
        /// </para>
        /// </summary>
        /// <param name="ALastReminderDate">Date when PartnerReminders last ran. Will be January 1st, 1980
        /// if PartnerReminders never ran before.</param>
        /// <param name="ASystemDefaultsDR">SystemDefaults DataRow containing the date. This is used later for updating
        /// the date.</param>
        /// <param name="AReadWriteTransaction">Already instantiated DB Transaction.</param>
        /// <returns>True if the 'Last Reminder Date' could be read/created. False if PartnerReminders was never run before
        /// AND creation of the new SystemDefault record failed for some reason.</returns>
        private static bool GetLastReminderDate(out DateTime ALastReminderDate,
            out SSystemDefaultsRow ASystemDefaultsDR,
            TDBTransaction AReadWriteTransaction)
        {
            const string UNDEFINED_SYSTEMDEFAULT_LAST_REMINDER_DATE = "1980,1,1";   // Double check order!

            SSystemDefaultsTable SystemDefaultsDT = new SSystemDefaultsTable();
            string LastReminderDateStr;

            string[] DateParts;
            bool ReturnValue = true;


            ASystemDefaultsDR = null;

            // Check if there is already a SystemDefault for the Last Reminder Date (most likely there is!)
            if (SSystemDefaultsAccess.Exists(SYSTEMDEFAULT_LAST_REMINDER_DATE, AReadWriteTransaction))
            {
                if (TLogging.DebugLevel >= 6)
                {
                    TLogging.Log("GetLastReminderDate: System Default for the Last Reminder Date exists: use it.");
                }

                // There is already a SystemDefault for the Last Reminder Date: read its value
                SystemDefaultsDT = SSystemDefaultsAccess.LoadByPrimaryKey(SYSTEMDEFAULT_LAST_REMINDER_DATE, AReadWriteTransaction);

                // Used later to update the row
                ASystemDefaultsDR = SystemDefaultsDT[0];
            }
            else
            {
                // System Default for the Last Reminder Date doesn't exist: add a new SystemDefault for future use
                if (TLogging.DebugLevel >= 6)
                {
                    TLogging.Log("GetLastReminderDate: System Default for the Last Reminder Date doesn't exist yet: creating it.");
                }

                ASystemDefaultsDR = SystemDefaultsDT.NewRowTyped();
                ASystemDefaultsDR.DefaultCode = SYSTEMDEFAULT_LAST_REMINDER_DATE;
                ASystemDefaultsDR.DefaultDescription = SYSTEMDEFAULT_LAST_REMINDER_DATE_DESC;
                ASystemDefaultsDR.DefaultValue = UNDEFINED_SYSTEMDEFAULT_LAST_REMINDER_DATE;

                try
                {
                    SSystemDefaultsAccess.SubmitChanges(SystemDefaultsDT, AReadWriteTransaction);
                }
                catch (Exception Exc)
                {
                    TLogging.Log("TProcessPartnerReminders.GetLastReminderDate: An Exception occured:" + Environment.NewLine + Exc.ToString());

                    throw;
                }
            }

            LastReminderDateStr = ASystemDefaultsDR.DefaultValue;

            // Last Reminder Date is stored as YEAR,MONTH,DAY
            DateParts = LastReminderDateStr.Split(',');
            ALastReminderDate = new DateTime(
                Convert.ToInt32(DateParts[0]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[2]),
                0, 0, 1);   // One second past midnight

            if (TLogging.DebugLevel >= 6)
            {
                TLogging.Log(String.Format("GetLastReminderDate: DB Field value: {0}; Parsed date: {1}", LastReminderDateStr, ALastReminderDate));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Updates the 'Last Reminder Date', that is the date when PartnerReminders last ran.
        /// <para>
        /// This is done by updating a certain SystemDefault.
        /// </para>
        /// </summary>
        /// <param name="ASystemDefaultsDR">SystemDefaults DataRow containing the date.</param>
        /// <param name="AReadWriteTransaction">Already instantiated DB Transaction.</param>
        private static void UpdateLastReminderDate(SSystemDefaultsRow ASystemDefaultsDR, TDBTransaction AReadWriteTransaction)
        {
            // Set SystemDefault value to today's date (mind the Format!)
            ASystemDefaultsDR.DefaultValue = String.Format("{0},{1},{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            try
            {
                SSystemDefaultsAccess.SubmitChanges((SSystemDefaultsTable)ASystemDefaultsDR.Table, AReadWriteTransaction);
            }
            catch (Exception Exc)
            {
                TLogging.Log("TProcessPartnerReminders.UpdateLastReminderDate: An Exception occured:" + Environment.NewLine + Exc.ToString());

                throw;
            }
        }

        /// <summary>
        /// Retrieves all the PartnerReminders that need to be processed.
        /// </summary>
        /// <param name="ALastReminderDate">Date when PartnerReminders processing last ran.</param>
        /// <param name="APartnerReminderDT">PartnerReminders DataRows that need to be processed.</param>
        /// <param name="ADBAccessObj">Already instatiated DB Access object with opened DB connection.</param>
        /// <param name="AReadTransaction">Already instantiated DB Transaction.</param>
        /// <returns>DataSet containing the <paramref name="APartnerReminderDT" /> DataTable.</returns>
        private static DataSet GetRemindersToProcess(DateTime ALastReminderDate, out PPartnerReminderTable APartnerReminderDT,
            TDataBase ADBAccessObj, TDBTransaction AReadTransaction)
        {
            List <OdbcParameter>OdbcParams;
            string SQLCommand;

            // Add Typed DataTable to a DataSet so we get the Partner Reminders in a Typed DataTable
            APartnerReminderDT = new PPartnerReminderTable();
            DataSet ReminderResultsDS = new DataSet();
            ReminderResultsDS.Tables.Add(APartnerReminderDT);

            SQLCommand = "SELECT * FROM PUB_p_partner_reminder WHERE ";
            SQLCommand += " ( p_next_reminder_date_d > ? OR p_last_reminder_sent_d IS NULL) AND ";
            SQLCommand += " p_next_reminder_date_d <= ? AND ";
            SQLCommand += " p_reminder_active_l = TRUE AND ";
            SQLCommand += " p_email_address_c <> ''";

            OdbcParams = new List <OdbcParameter>();

            // Parameter 1 = LastReminderDate from SystemDefaults
            OdbcParams.Add(new OdbcParameter("LastDate", OdbcType.Date));
            OdbcParams[0].Value = ALastReminderDate.Date;

            // Parameter 2 = Today's date
            OdbcParams.Add(new OdbcParameter("Now", OdbcType.Date));
            OdbcParams[1].Value = DateTime.Now.Date;

            DBAccess.GDBAccessObj.Select(ReminderResultsDS, SQLCommand, APartnerReminderDT.TableName, AReadTransaction, OdbcParams.ToArray());

            // Mark the data as being 'unchanged' (rather than 'new', which it is by default)
            ReminderResultsDS.AcceptChanges();

            return ReminderResultsDS;
        }

        /// <summary>
        /// Sends a Reminder Email to a Partner.
        /// </summary>
        /// <param name="APartnerReminderDR">DataRow containing the Reminder data.</param>
        /// <param name="AReadWriteTransaction">Already instantiated DB Transaction.</param>
        /// <returns>True if the sending of the Reminder Email succeeded, otherwise false.</returns>
        private static bool SendReminderEmail(PPartnerReminderRow APartnerReminderDR, TDBTransaction AReadWriteTransaction)
        {
            string Subject = "";
            string Body = "";
            string Destination = APartnerReminderDR.EmailAddress;
            string PartnerShortName;
            char LF = Convert.ToChar(10);


            // Retrieve ShortName of the Partner to which the Reminder Email should be sent
            PartnerShortName = GetPartnerShortName(APartnerReminderDR.PartnerKey);

            // Format Email Recipient Address as per email RFC's
            if (!Destination.Trim().StartsWith("<"))
            {
                Destination = "<" + Destination;
            }

            if (!Destination.Trim().EndsWith(">"))
            {
                Destination = Destination + ">";
            }

            // Compose Email Subject
            Subject = String.Format("OpenPetra Reminder about {0}", PartnerShortName);

            /*
             * Compose Email Body
             */
            Body = string.Format("Partner: {0}   [{1:0000000000}]{2}", PartnerShortName, APartnerReminderDR.PartnerKey, LF);

            if (APartnerReminderDR.ContactId != 0)
            {
                Body += GetContactDetails(APartnerReminderDR.ContactId, AReadWriteTransaction);
            }

            Body += String.Format("Reason: {0}{1}", APartnerReminderDR.ReminderReason, LF);

            if (!APartnerReminderDR.IsEventDateNull())
            {
                Body += String.Format("Event Date: {0}{1}", StringHelper.DateToLocalizedString(APartnerReminderDR.EventDate), LF);
            }

            Body += String.Format("Comment: {0}{1}", APartnerReminderDR.Comment, LF);
            Body += String.Format("{0}Next Reminder: {1}{2}", LF, StringHelper.DateToLocalizedString(APartnerReminderDR.NextReminderDate), LF);

            if (APartnerReminderDR.ReminderActive == false)
            {
                Body += String.Format("This Reminder has now been disabled.{0}", LF);
            }

            // Send Email (this picks up the SMTPServer AppSetting from the Server Config File)
            return new TSmtpSender().SendEmail(Destination, "OpenPetra Server", Destination, Subject, Body);
        }

        /// <summary>
        /// Retrieves the ShortName of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <returns>ShortName of the specified Partner.</returns>
        private static string GetPartnerShortName(long APartnerKey)
        {
            string ShortName = "";

            Ict.Petra.Shared.TPartnerClass PartnerClass;

            TPartnerServerLookups.GetPartnerShortName(APartnerKey, out ShortName, out PartnerClass);

            return ShortName;
        }

        /// <summary>
        /// Return the specified contact details, formatted for use in an email.
        /// </summary>
        /// <param name="AContactID">The Contact ID to find.</param>
        /// <param name="AReadTransaction">Already instantiated DB Transaction.</param>
        /// <returns>Specified contact details.</returns>
        private static string GetContactDetails(int AContactID, TDBTransaction AReadTransaction)
        {
            PPartnerContactTable PartnerContactDT;
            PPartnerContactRow PartnerContactDR;
            DateTime ContactTime;
            char LF = Convert.ToChar(10);
            string ReturnValue = "";

            try
            {
                if (!PPartnerContactAccess.Exists(AContactID, AReadTransaction))
                {
                    return String.Format("Contact ID {0} not found{1}", AContactID, LF);
                }

                PartnerContactDT = PPartnerContactAccess.LoadByPrimaryKey(AContactID, AReadTransaction);
            }
            catch (Exception Exp)
            {
                TLogging.Log("TProcessPartnerReminders.GetContactDetails encountered an Exception: " + Exp.ToString());

                throw;
            }

            PartnerContactDR = PartnerContactDT[0];

            ContactTime = DateTime.Now.AddSeconds(PartnerContactDR.ContactTime);

            ReturnValue = String.Format("Contact: {0} {1} {2:HH}:{2:mm} {3} {4}",
                PartnerContactDR.Contactor,
                PartnerContactDR.ContactDate.Date,
                ContactTime,
                PartnerContactDR.ContactCode,
                Environment.NewLine);

            return ReturnValue;
        }

        #endregion
    }
}