//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Drawing;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;
using System.Collections.Generic;
using System.Collections;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmPersonalDocumentExpiryReport class
    /// </summary>
    public partial class TFrmEmergencyDataReport
    {
        private void InitializeManualCode()
        {
            ucoPartnerSelection.SetRestrictedPartnerClasses("PERSON");
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);

            // if FastReport, then ignore columns tab
            if ((FPetraUtilsObject.GetCallerForm() != null) && FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
            }
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

            DataSet ReportSet = TRemote.MReporting.WebConnectors.GetReportDataSet("EmergencyDataReport", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportSet == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["PersonnelData"], "PersonnelData");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Family"], "Family");
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["FamilyLink"], "FamilyLink");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Passports"], "Passports");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Skills"], "Skills");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Languages"], "Languages");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["PersonalDocuments"], "PersonalDocuments");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["PartnerAddress"], "PartnerAddress");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["EmergencyContacts"], "EmergengcyContacts");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ECAddresses"], "ECAddresses");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ECContactDetails"], "ECContactDetails");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["OtherEmergData"], "OtherEmergData");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["ProofOfLife"], "ProofOfLife");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["SpecialNeeds"], "SpecialNeeds");

            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddColumnLayout(0, 0, 0, 19);
            ACalc.SetMaxDisplayColumns(1);
            ACalc.AddColumnCalculation(0, "Emergency Data");
        }
    }
}