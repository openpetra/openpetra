//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.App.Core.Security;
using System.IO;
using Ict.Common;
using Ict.Petra.Server.MCommon;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Manages lists of templates for the various report types.
    /// </summary>
    public class TReportTemplateWebConnector
    {
        private static String TemplateBackupFilename(String AType, String ATemplateId)
        {
            return TAppSettingsManager.GetValue("Reporting.PathStandardReports") + "/Backup_" +
                   ATemplateId + "_" + AType + ".sql";
        }

        /// <summary>
        /// For Development only, templates are also kept in a disc file.
        /// This means that Bazaar does the internal update management for us.
        /// </summary>
        private static void LoadTemplatesFromBackupFile(String AType, TDataBase dbConnection)
        {
            String BackupFilename = TemplateBackupFilename(AType, "*");

            String[] BackupFiles = Directory.GetFiles(Path.GetDirectoryName(BackupFilename), Path.GetFileName(BackupFilename));
            TDBTransaction Transaction = null;
            Boolean submissionOk = false;

            dbConnection.BeginAutoTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                ref submissionOk,
                "LoadTemplatesFromBackupFile",
                delegate
                {
                    foreach (String fname in BackupFiles)
                    {
                        if (File.Exists(fname))
                        {
                            String Query = File.ReadAllText(fname);
                            Transaction.DataBaseObj.ExecuteNonQuery(Query, Transaction);
                            submissionOk = true;
                        }
                    }
                });
        }

        //
        // This "Escape string for SQL" method is written only for Postgres,
        // Since that's what we're using in development, and the backup-to-file
        // function is only for development:
        private static String escape(Object AField)
        {
            String Txt = AField.ToString();

            Txt = Txt.Replace("'", "''");
            return Txt;
        }

        /// <summary>
        /// For Development only, templates are also kept in disc files.
        /// This means that Bazaar will do the internal update management for us.
        ///
        /// For the backup to work, the XmlReports\FastReportsBackup.sql file must be present,
        /// but it doesn't need to contain anything specifically.
        /// </summary>
        private static void SaveTemplateToBackupFile(SReportTemplateRow Row)
        {
            if (File.Exists(TAppSettingsManager.GetValue("Reporting.PathStandardReports") + "\\FastReportsBackup.sql")
                && (Row.RowState != DataRowState.Deleted))
            {
                String BackupFilename = TemplateBackupFilename(Row.ReportType, Row.TemplateId.ToString("D2"));

                String sqlQuery = "DELETE FROM s_report_template WHERE s_template_id_i=" + Row.TemplateId + ";\r\n" +
                                  "INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)\r\nVALUES("
                                  +
                                  Convert.ToInt32(Row["s_template_id_i"]) + ",'" +
                                  escape(Row["s_report_type_c"]) + "','" +
                                  escape(Row["s_report_variant_c"]) + "','" +
                                  escape(Row["s_author_c"]) + "'," +
                                  Row["s_default_l"] + "," +
                                  Row["s_readonly_l"] + "," +
                                  Row["s_private_l"] + "," +
                                  Row["s_private_default_l"] + ",\r\n'" +
                                  escape(Row["s_xml_text_c"]) + "');\r\n";

                sqlQuery += "\r\nSELECT TRUE;";
                File.WriteAllText(BackupFilename, sqlQuery);
            }
        }

        /// <summary>
        /// Get a list of templates for this Report Type.
        /// The list will contain:
        ///   * all "Public" templates and
        ///   * all non-Public templates by this Author.
        ///
        /// If DefaultOnly is given, the table contains
        ///   * a single row marked with PrivateDefault, if one is present, or
        ///   * a single row marked Default - there should be only one Default for this ReportType.
        ///   * a single row with neither flag, since it's better to return something than nothing!
        /// </summary>
        /// <param name="AReportType"></param>
        /// <param name="AAuthor"></param>
        /// <param name="ADefaultOnly"></param>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static SReportTemplateTable GetTemplateVariants(String AReportType, String AAuthor, Boolean ADefaultOnly = false)
        {
            SReportTemplateTable Tbl = new SReportTemplateTable();
            SReportTemplateTable Ret = new SReportTemplateTable();
            TDBTransaction Transaction = null;
            TDataBase dbConnection = new TDataBase();

            try
            {
                dbConnection = TReportingDbAdapter.EstablishDBConnection(true, "GetTemplateVariants");
                LoadTemplatesFromBackupFile(AReportType, dbConnection);

                dbConnection.BeginAutoReadTransaction(
                    ref Transaction,
                    delegate
                    {
                        SReportTemplateRow TemplateRow = Tbl.NewRowTyped(false);
                        TemplateRow.ReportType = AReportType;

                        Tbl = SReportTemplateAccess.LoadUsingTemplate(TemplateRow, Transaction);
                    });

                String filter = String.Format("(s_author_c ='{0}' OR s_private_l=false)", AAuthor);

                if (ADefaultOnly)
                {
                    filter += " AND (s_default_l=true OR s_private_default_l=true)";
                }

                Tbl.DefaultView.RowFilter = filter;

                if (Tbl.DefaultView.Count > 0)
                {
                    Tbl.DefaultView.Sort =
                        (ADefaultOnly) ? "s_private_default_l DESC, s_default_l DESC" :
                        "s_readonly_l DESC, s_default_l DESC, s_private_default_l DESC";
                }
                else // Something went wrong, but I'll try not to return empty-handed.
                {
                    Tbl.DefaultView.RowFilter = "";
                }

                Ret.Merge(Tbl.DefaultView.ToTable());
                Ret.AcceptChanges();
            }
            finally
            {
                dbConnection.CloseDBConnection();
            }
            return Ret;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="editedTemplates"></param>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static SReportTemplateTable SaveTemplates(SReportTemplateTable editedTemplates)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            SReportTemplateTable ChangedTemplates = editedTemplates.GetChangesTyped();

            if ((ChangedTemplates != null) && (ChangedTemplates.Rows.Count > 0))
            {
                SReportTemplateAccess.SubmitChanges(ChangedTemplates, Transaction);
                DBAccess.GDBAccessObj.CommitTransaction();

                foreach (SReportTemplateRow Row in ChangedTemplates.Rows)
                {
                    if (Row.RowState == DataRowState.Deleted)
                    {
                        // The template was deleted - I'll attempt to delete the backup.
                        Int32 templateId = Convert.ToInt32(Row["s_template_id_i", DataRowVersion.Original]);
                        String reportType = Row["s_report_type_c", DataRowVersion.Original].ToString();
                        String deletedBackupFilename = TemplateBackupFilename(reportType, templateId.ToString("D2"));

                        try
                        {
                            File.Delete(deletedBackupFilename);
                        }
                        catch (Exception) // I'm not interested in knowing why this didn't work.
                        {
                        }
                    }
                    else
                    {
                        SaveTemplateToBackupFile(Row);
                    }
                }
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ChangedTemplates;
        }
    }
}