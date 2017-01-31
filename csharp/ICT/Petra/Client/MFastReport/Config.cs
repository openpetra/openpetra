//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Moray
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

using FastReport.Utils;
using FastReport.Design;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.MSysMan.Data;
using System;
using System.Data;
using System.Windows.Forms;
using FastReport;

namespace Ict.Petra.Client.MFastReport
{
    /// <summary>
    /// Static singleton FastReport configuration settings for OpenPetra.
    /// </summary>
    public static class IctConfig
    {
        static private bool FInitialized = false;

        /// <summary>
        /// Initialize the configuration once.
        /// </summary>
        public static void InitIctConfig()
        {
            if (FInitialized)
            {
                return;
            }

            //TODO:
            //  1. Modify all FastReport Backup.sql files and templates to remove OmDate, DateTimeNow and PartnerKey from their Code sections.
            //  2. Remove those functions from Utils.cs
            //  3. Uncomment the following lines to register those functions within FastReport, making them available to the reports
            //     and visually under Functions in Report Designer
            //RegisteredObjects.AddFunctionCategory("OMFuncs", "OM Functions");
            //Type OMFunctionType = typeof(OMFunctions);
            //RegisteredObjects.AddFunction(OMFunctionType.GetMethod("OMDate"), "OMFuncs");
            //RegisteredObjects.AddFunction(OMFunctionType.GetMethod("DateTimeNow"), "OMFuncs");
            //RegisteredObjects.AddFunction(OMFunctionType.GetMethod("PartnerKey"), "OMFuncs");

            Config.DesignerSettings.ShowInTaskbar = true;

            Config.DesignerSettings.CustomSaveDialog += new OpenSaveDialogEventHandler(DesignerSettings_CustomSaveDialog);
            Config.DesignerSettings.CustomSaveReport += new OpenSaveReportEventHandler(DesignerSettings_CustomSaveReport);

            FInitialized = true;
        }

        /// <summary>
        /// OM International date format.
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string OMDate(DateTime Date)
        {
            return Date.ToString("dd-MMM-yyyy");
        }

        /// <summary>
        /// Current date and time in OM International format.
        /// </summary>
        /// <returns></returns>
        public static String DateTimeNow()
        {
            return DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
        }

        /// <summary>Partner keys always have 10 digits.</summary>
        public static String PartnerKey(Int64 PartnerKey)
        {
            return PartnerKey.ToString("0000000000");
        }

        /// <summary>
        /// Called during "Save As". We don't want to present the user with the standard file selection dialog because it doesn't save to a file,
        /// but unfortunately we can't disable Save As without disabling Save too. So make this effectively the same as Save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DesignerSettings_CustomSaveDialog(object sender, OpenSaveDialogEventArgs e)
        {
            var TemplateId = Convert.ToInt32(e.Designer.ActiveReport.GetParameterValue("param_design_template_id"));

            var TemplateTable = TRemote.MReporting.WebConnectors.GetTemplateById(TemplateId);
            var SelectedTemplate = (SReportTemplateRow)TemplateTable.Rows[0];

            if (MessageBox.Show(String.Format("{0} {1}?\n{2}", Catalog.GetString("Save changes to"), SelectedTemplate.ReportType,
                        SelectedTemplate.ReportVariant), Catalog.GetString("Save Report"), MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Save the report template to the database and Backup.sql file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DesignerSettings_CustomSaveReport(object sender, OpenSaveReportEventArgs e)
        {
            int TemplateId = Convert.ToInt32(e.Report.GetParameterValue("param_design_template_id"));

            SaveReport(e.Report, TemplateId);
        }

        private static void SaveReport(Report Report, int TemplateId)
        {
            SReportTemplateTable TemplateTable;
            SReportTemplateRow SelectedTemplate;

            try
            {
                TemplateTable = TRemote.MReporting.WebConnectors.GetTemplateById(TemplateId);
                SelectedTemplate = (SReportTemplateRow)TemplateTable.Rows[0];
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex);
                throw;
            }

            if (SelectedTemplate.Readonly)
            {
                if (MessageBox.Show(
                        String.Format("{0} {1}", SelectedTemplate.ReportVariant, Catalog.GetString("cannot be ovewritten.\nMake a copy instead?")),
                        Catalog.GetString("Design Template"),
                        MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                SReportTemplateRow CopyRow = TemplateTable.NewRowTyped();
                DataUtilities.CopyAllColumnValues(SelectedTemplate, CopyRow);
                String currentUser = UserInfo.GUserInfo.UserID;
                CopyRow.TemplateId = -1; // The value will come from the sequence
                CopyRow.ReportVariant = String.Format("{0} {1} {2}", currentUser, Catalog.GetString("copy of"), SelectedTemplate.ReportVariant);
                CopyRow.Author = currentUser;
                CopyRow.XmlText = Report.SaveToString();
                CopyRow.Readonly = false;
                CopyRow.Default = false;
                CopyRow.PrivateDefault = false;
                TemplateTable.Rows.Add(CopyRow);
            }
            else
            {
                SelectedTemplate.XmlText = Report.SaveToString();
            }

            SReportTemplateTable Tbl = TRemote.MReporting.WebConnectors.SaveTemplates(TemplateTable);

            Report.SetParameterValue("param_design_template_id", Tbl[0].TemplateId);
        }
    }
}