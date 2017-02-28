//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    /// <summary>
    /// manual code for TFrmPrintPartner class
    /// </summary>
    public partial class TFrmPrintPartner
    {
        private void InitializeManualCode()
        {
            if (!UserInfo.GUserInfo.IsInModule("FINANCE-1"))
            {
                chkFinanceDetails.Checked = false;
                chkFinanceDetails.Enabled = false;
            }

            ucoPartnerSelection.ShowAllStaffOption(false);
            ucoPartnerSelection.ShowCurrentStaffOption(false);

            // correct tab order (two columns of checkboxes for data sections, we want to go down first column and then down second column)
            chkPartnerClassData.TabIndex = 0;
            chkSubscriptions.TabIndex = 10;
            chkRelationships.TabIndex = 20;
            chkLocations.TabIndex = 30;
            chkContactDetails.TabIndex = 40;
            chkFinanceDetails.TabIndex = 50;
            chkInterests.TabIndex = 60;
            chkContacts.TabIndex = 70;
            chkReminders.TabIndex = 80;
            chkSpecialTypes.TabIndex = 90;
            btnSelectAll.TabIndex = 100;
            btnDeselectAll.TabIndex = 110;
            chkHideEmpty.TabIndex = 120;
            chkPaginate.TabIndex = 130;
        }

        private void SelectAll(Object sender, EventArgs e)
        {
            chkPartnerClassData.Checked = true;
            chkInterests.Checked = true;
            chkSubscriptions.Checked = true;
            chkContacts.Checked = true;
            chkRelationships.Checked = true;
            chkReminders.Checked = true;
            chkLocations.Checked = true;
            chkContactDetails.Checked = true;
            chkSpecialTypes.Checked = true;

            if (UserInfo.GUserInfo.IsInModule("FINANCE-1"))
            {
                chkFinanceDetails.Checked = true;
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && (!paramsDictionary.ContainsKey(p.name)))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataSet ReportSet = TRemote.MReporting.WebConnectors.GetReportDataSet("PrintPartner", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportSet == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Partners"], "Partners");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassPerson"], "ClassPerson");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassFamily"], "ClassFamily");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassOrganisation"], "ClassOrganisation");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassBank"], "ClassBank");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassChurch"], "ClassChurch");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassUnit"], "ClassUnit");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ClassVenue"], "ClassVenue");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Subscriptions"], "Subscriptions");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Relationships"], "Relationships");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Locations"], "Locations");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ContactDetails"], "ContactDetails");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["FinanceDetails"], "FinanceDetails");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["PartnerInterests"], "PartnerInterests");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["PartnerContacts"], "PartnerContacts");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Reminders"], "Reminders");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["SpecialTypes"], "SpecialTypes");

            return true;
        }

        private void DeselectAll(Object sender, EventArgs e)
        {
            chkPartnerClassData.Checked = false;
            chkInterests.Checked = false;
            chkSubscriptions.Checked = false;
            chkContacts.Checked = false;
            chkRelationships.Checked = false;
            chkReminders.Checked = false;
            chkLocations.Checked = false;
            chkFinanceDetails.Checked = false;
            chkContactDetails.Checked = false;
            chkSpecialTypes.Checked = false;
        }

        /// <summary>
        /// Used for passing a Partner's partner key to the screen before the screen is actually shown.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner to print.</param>
        public void SetParameters(Int64 APartnerKey)
        {
            TParameterList Parameters = new TParameterList();

            Parameters.Add("param_selection", "one partner");
            Parameters.Add("param_partnerkey", APartnerKey);
            this.ucoPartnerSelection.SetControls(Parameters);
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
    /// </summary>
    public static class TPrintPartnerModal
    {
        /// <summary>
        /// Opens a Modal instance of the Print Partner screen.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner to print.</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if a Partner was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(Int64 APartnerKey, Form AParentForm)
        {
            TFrmPrintPartner PrintPartnerForm;
            DialogResult dlgResult;

            PrintPartnerForm = new TFrmPrintPartner(AParentForm);
            PrintPartnerForm.SetParameters(APartnerKey);

            dlgResult = PrintPartnerForm.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}