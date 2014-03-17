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

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Manages lists of templates for the various report types.
    /// </summary>
    public class TReportTemplateWebConnector
    {

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
            return editedTemplates;
        }
    }
}