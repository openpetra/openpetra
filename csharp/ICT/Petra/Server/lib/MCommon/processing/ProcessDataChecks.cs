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
using System.IO;
using System.Data;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Petra.Server.MSysMan.Cacheable.WebConnectors;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.MCommon.Processing
{
    /// <summary>
    /// run some data checks against the database and tell the users how to fix consistency issues
    /// </summary>
    public class TProcessDataChecks
    {
        private const string PROCESSDATACHECK_LAST_RUN = "PROCESSDATACHECK_LAST_RUN";
        private const float SENDREPORTFORDAYS_TOUSERS = 14.0f;
        private static DateTime Errors_SinceDate;

        /// <summary>
        /// Gets called in regular intervals from a Timer in Class TTimedProcessing.
        /// </summary>
        /// <param name="ADBAccessObj">Instantiated DB Access object with opened DB connection.</param>
        /// <param name="ARunManually">this is true if the process was called manually from the server admin console</param>
        public static void Process(TDataBase ADBAccessObj, bool ARunManually)
        {
            // only check once a day (or as specified in config file), if not manually called
            if (!ARunManually)
            {
                DateTime LastRun =
                    TVariant.DecodeFromString(


                        TSystemDefaults.GetSystemDefault(
                            PROCESSDATACHECK_LAST_RUN,
                            new TVariant(DateTime.MinValue).EncodeToString())).ToDate();

                if (LastRun.AddDays(TAppSettingsManager.GetInt16("DataChecks.RunEveryXDays", 1)) > DateTime.Now)
                {
                    // do not run the data check more than once a day or a week (depending on configuration setting), too many emails
                    TLogging.LogAtLevel(1, "TProcessDataChecks.Process: not running, since last run was at " + LastRun.ToString());
                    return;
                }
            }

            Errors_SinceDate = DateTime.Today.AddDays(-1 * SENDREPORTFORDAYS_TOUSERS);

            TLogging.LogAtLevel(1, "TProcessDataChecks.Process: Checking Modules");
            CheckModule(ADBAccessObj, "DataCheck.MPartner.");

            TSystemDefaults.SetSystemDefault(PROCESSDATACHECK_LAST_RUN, new TVariant(DateTime.Now).EncodeToString());
        }

        private static void CheckModule(TDataBase ADBAccessObj, string AModule)
        {
            // get all sql files starting with module
            string[] sqlfiles = Directory.GetFiles(Path.GetFullPath(TAppSettingsManager.GetValue("SqlFiles.Path", ".")),
                AModule + "*.sql");

            DataTable errors = new DataTable(AModule + "Errors");

            foreach (string sqlfile in sqlfiles)
            {
                string sql = TDataBase.ReadSqlFile(Path.GetFileName(sqlfile));

                // extend the sql to load the s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c
                // only for the first table in the FROM clause
                string firstTableAlias = sql.Substring(sql.ToUpper().IndexOf("FROM ") + "FROM ".Length);
                firstTableAlias = firstTableAlias.Substring(0, firstTableAlias.ToUpper().IndexOf("WHERE"));
                int indexOfAs = firstTableAlias.ToUpper().IndexOf(" AS ");

                if (indexOfAs > -1)
                {
                    firstTableAlias = firstTableAlias.Substring(indexOfAs + " AS ".Length).Trim();

                    if (firstTableAlias.Contains(","))
                    {
                        firstTableAlias = firstTableAlias.Substring(0, firstTableAlias.IndexOf(",")).Trim();
                    }
                }

                sql = sql.Replace("FROM ", ", " + firstTableAlias + ".s_date_created_d AS DateCreated, " +
                    firstTableAlias + ".s_created_by_c AS CreatedBy, " +
                    firstTableAlias + ".s_date_modified_d AS DateModified, " +
                    firstTableAlias + ".s_modified_by_c AS ModifiedBy FROM ");

                errors.Merge(ADBAccessObj.SelectDT(sql, "temp", null));
            }

            if (errors.Rows.Count > 0)
            {
                SendEmailToAdmin(errors);
                SendEmailsPerUser(errors);
            }
        }

        private static void SendEmailToAdmin(DataTable AErrors)
        {
            // Create excel output of the errors table
            string excelfile = TAppSettingsManager.GetValue("DataChecks.TempPath") + "/errors.xlsx";

            try
            {
                using (StreamWriter sw = new StreamWriter(excelfile))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        if (!TCsv2Xml.DataTable2ExcelStream(AErrors, m))
                        {
                            return;
                        }

                        m.WriteTo(sw.BaseStream);
                        m.Close();
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log("Problems writing to file " + excelfile);
                TLogging.Log(e.ToString());
                return;
            }

            if (TAppSettingsManager.HasValue("DataChecks.Email.Recipient"))
            {
                new TSmtpSender().SendEmail("<" + TAppSettingsManager.GetValue("DataChecks.Email.Sender") + ">",
                    "OpenPetra DataCheck Robot",
                    TAppSettingsManager.GetValue("DataChecks.Email.Recipient"),
                    "Data Check",
                    "there are " + AErrors.Rows.Count.ToString() + " errors. Please see attachment!",
                    excelfile);
            }
            else
            {
                TLogging.Log("there is no email sent because DataChecks.Email.Recipient is not defined in the config file");
            }
        }

        private static void SendEmailForUser(string AUserId, DataTable AErrors)
        {
            // get the email address of the user
            SUserRow userrow = SUserAccess.LoadByPrimaryKey(AUserId, null)[0];

            string excelfile = TAppSettingsManager.GetValue("DataChecks.TempPath") + "/errors" + AUserId + ".xlsx";

            DataView v = new DataView(AErrors,
                "(CreatedBy='" + AUserId + "' AND ModifiedBy IS NULL AND DateCreated > #" + Errors_SinceDate.ToString("MM/dd/yyyy") + "#) " +
                "OR (ModifiedBy='" + AUserId + "' AND DateModified > #" + Errors_SinceDate.ToString("MM/dd/yyyy") + "#)",
                string.Empty, DataViewRowState.CurrentRows);

            try
            {
                using (StreamWriter sw = new StreamWriter(excelfile))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        if (!TCsv2Xml.DataTable2ExcelStream(v.ToTable(), m))
                        {
                            return;
                        }

                        m.WriteTo(sw.BaseStream);
                        m.Close();
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log("Problems writing to file " + excelfile);
                TLogging.Log(e.ToString());
                return;
            }

            string recipientEmail = string.Empty;

            if (!userrow.IsEmailAddressNull())
            {
                recipientEmail = userrow.EmailAddress;
            }
            else if (TAppSettingsManager.HasValue("DataChecks.Email.Recipient.UserDomain"))
            {
                recipientEmail = userrow.FirstName + "." + userrow.LastName + "@" + TAppSettingsManager.GetValue(
                    "DataChecks.Email.Recipient.UserDomain");
            }
            else if (TAppSettingsManager.HasValue("DataChecks.Email.Recipient"))
            {
                recipientEmail = TAppSettingsManager.GetValue("DataChecks.Email.Recipient");
            }

            if (recipientEmail.Length > 0)
            {
                new TSmtpSender().SendEmail("<" + TAppSettingsManager.GetValue("DataChecks.Email.Sender") + ">",
                    "OpenPetra DataCheck Robot",
                    recipientEmail,
                    "Data Check for " + AUserId,
                    "there are " + v.Count.ToString() + " errors. Please see attachment!",
                    excelfile);
            }
            else
            {
                TLogging.Log("no email can be sent to " + AUserId);
            }
        }

        private static void SendEmailsPerUser(DataTable AErrors)
        {
            // get all users that have created or modified the records in the past week(s)
            List <String>Users = new List <string>();

            foreach (DataRow r in AErrors.Rows)
            {
                string lastUser = string.Empty;

                if (!r.IsNull("DateModified") && (Convert.ToDateTime(r["DateModified"]) > Errors_SinceDate))
                {
                    lastUser = r["ModifiedBy"].ToString();
                }
                else if (!r.IsNull("DateCreated") && (Convert.ToDateTime(r["DateCreated"]) > Errors_SinceDate))
                {
                    lastUser = r["CreatedBy"].ToString();
                }

                if ((lastUser.Trim().Length > 0) && !Users.Contains(lastUser))
                {
                    Users.Add(lastUser);

                    SendEmailForUser(lastUser, AErrors);
                }
            }
        }
    }
}