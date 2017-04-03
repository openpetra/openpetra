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

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmPersonalDocumentExpiryReport class
    /// </summary>
    public partial class TFrmPersonalDataReport
    {
        private void InitializeManualCode()
        {
            ucoPartnerSelection.SetRestrictedPartnerClasses("PERSON");
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddColumnLayout(0, 0, 0, 19);
            ACalc.SetMaxDisplayColumns(1);
            ACalc.AddColumnCalculation(0, "Personal Data");
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("PersonalDataReport",
                true,
                new string[]
                {
                    "Person", "LocalPartnerData", "LocalPersonnelData",
                    "JobAssignments", "Commitments", "Passports",
                    "PersonalDocuments", "SpecialNeeds", "PersonalBudget",
                    "Skills", "Languages", "PreviousExperience"
                },
                ACalc,
                this,
                true,
                false);
        }

        private void DeselectAll(Object sender, EventArgs e)
        {
            chkCommitments.Checked = false;
            chkJobAssignments.Checked = false;
            chkLanguages.Checked = false;
            chkLocalPartnerData.Checked = false;
            chkLocalPersonnelData.Checked = false;
            chkNewPage.Checked = false;
            chkPassport.Checked = false;
            chkPersonalDocuments.Checked = false;
            chkPreviousExperiences.Checked = false;
            chkSkills.Checked = false;
            chkSpecialNeeds.Checked = false;
        }

        private void SelectAll(Object sender, EventArgs e)
        {
            chkCommitments.Checked = true;
            chkJobAssignments.Checked = true;
            chkLanguages.Checked = true;
            chkLocalPartnerData.Checked = true;
            chkLocalPersonnelData.Checked = true;
            chkNewPage.Checked = true;
            chkPassport.Checked = true;
            chkPersonalDocuments.Checked = true;
            chkPreviousExperiences.Checked = true;
            chkSkills.Checked = true;
            chkSpecialNeeds.Checked = true;
        }
    }
}