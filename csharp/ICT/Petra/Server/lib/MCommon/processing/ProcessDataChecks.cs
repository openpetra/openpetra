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
using Ict.Common.IO;
using Ict.Petra.Server.MSysMan.Cacheable.WebConnectors;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;

namespace Ict.Petra.Server.MCommon.Processing
{
    /// <summary>
    /// run some data checks against the database and tell the users how to fix consistency issues
    /// </summary>
    public class TProcessDataChecks
    {
        private const string PROCESSDATACHECK_LAST_RUN = "PROCESSDATACHECK_LAST_RUN";
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

            // Create excel output of the errors table
            string excelfile = TAppSettingsManager.GetValue("DataChecks.TempPath") + "/errors.xlsx";
            StreamWriter sw = new StreamWriter(excelfile);
            MemoryStream m = new MemoryStream();
            TCsv2Xml.DataTable2ExcelStream(errors, m);
            m.WriteTo(sw.BaseStream);
            m.Close();
            sw.Close();

            if (TAppSettingsManager.HasValue("DataChecks.Email.Recipient"))
            {
                new TSmtpSender().SendEmail("<" + TAppSettingsManager.GetValue("DataChecks.Email.Sender") + ">",
                    "OpenPetra DataCheck Robot",
                    TAppSettingsManager.GetValue("DataChecks.Email.Recipient"),
                    "Data Check",
                    "there are " + errors.Rows.Count.ToString() + " errors. Please see attachment!",
                    excelfile);
            }
            else
            {
                TLogging.Log("there is no email sent because DataChecks.Email.Recipient is not defined in the config file");
            }
        }
    }
}