//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2014 by OM International
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

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Manages lists of templates for the various report types.
    /// </summary>
    public class TReportTemplateWebConnector
    {
        /// <summary>
        /// For Development only, all templates marked "read only" are also kept in a disc file.
        /// This means that Bazaar does the internal update management for us.
        /// </summary>
        private static void LoadTemplatesFromBackupFile()
        {
            String TemplateBackupFilename = TAppSettingsManager.GetValue("Reporting.PathStandardReports") + "/SystemTemplates.sql";

            if (File.Exists(TemplateBackupFilename))
            {
                String Query = File.ReadAllText(TemplateBackupFilename);
                DBAccess.GDBAccessObj.ExecuteScalar(Query, IsolationLevel.Serializable); // This objects if there's no return value, so a useless SELECT was added to the end of the SQL.
            }
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
        /// For Development only, all templates marked "read only" are also kept in a disc file.
        /// This means that Bazaar does the internal update management for us.
        /// </summary>
        private static void SaveTemplatesToBackupFile()
        {
            String TemplateBackupFilename = TAppSettingsManager.GetValue("Reporting.PathStandardReports") + "/SystemTemplates.sql";

            if (File.Exists(TemplateBackupFilename))
            {
                String Query = "SELECT * FROM s_report_template WHERE s_readonly_l=TRUE;";

                TDBTransaction ReadTrans = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
                DataTable resultTable = DBAccess.GDBAccessObj.SelectDT(Query, "Templates", ReadTrans);
                DBAccess.GDBAccessObj.RollbackTransaction();

                String FinalQuery = "DELETE FROM s_report_template WHERE s_readonly_l=TRUE;\r\n";
                for (Int32 Idx = 0; Idx < resultTable.Rows.Count; Idx++)
                {
                    DataRow Row = resultTable.Rows[Idx];
                    FinalQuery += ("INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)\r\nVALUES("
                        + Convert.ToInt32(Row["s_template_id_i"]) + ",'"
                        + escape(Row["s_report_type_c"]) + "','"
                        + escape(Row["s_report_variant_c"]) + "','"
                        + escape(Row["s_author_c"]) + "',"
                        + Row["s_default_l"] + ","
                        + Row["s_readonly_l"] + ",\r\n'"
                        + escape(Row["s_xml_text_c"]) + "');\r\n");
                }
                FinalQuery += "\r\nSELECT TRUE;";
                File.WriteAllText(TemplateBackupFilename, FinalQuery);
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
        ///   * a single row marked Default - there will be only one Default for this ReportType.
        /// </summary>
        /// <param name="AReportType"></param>
        /// <param name="AAuthor"></param>
        /// <param name="ADefaultOnly"></param>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static SReportTemplateTable GetTemplateVariants(String AReportType, String AAuthor, Boolean ADefaultOnly = false)
        {
            LoadTemplatesFromBackupFile();
            SReportTemplateTable Tbl = new SReportTemplateTable();
            SReportTemplateRow TemplateRow = Tbl.NewRowTyped(false);
            TemplateRow.ReportType = AReportType;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            Tbl = SReportTemplateAccess.LoadUsingTemplate(TemplateRow, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();

            String filter = String.Format("s_author_c ='{0}' OR s_private_l=false", AAuthor);

            if (ADefaultOnly)
            {
                filter += " AND (s_default_l=true OR s_private_default_l=true)";
            }

            Tbl.DefaultView.RowFilter = filter;
            Tbl.DefaultView.Sort = (ADefaultOnly)? "s_private_default_l DESC, s_default_l DESC" : "s_readonly_l DESC, s_default_l DESC, s_private_default_l DESC";

            SReportTemplateTable Ret = new SReportTemplateTable();
            Ret.Merge(Tbl.DefaultView.ToTable());
            Ret.AcceptChanges();
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
            SReportTemplateAccess.SubmitChanges(editedTemplates, Transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
            SaveTemplatesToBackupFile();
            return editedTemplates;
        }
    }
}