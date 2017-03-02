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
using Ict.Common;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmPassportExpiryReport class
    /// </summary>
    public partial class TFrmPassportExpiryReport
    {
        private void InitializeManualCode()
        {
            ucoPartnerSelection.SetRestrictedPartnerClasses("PERSON");
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("PassportExpiryReport",
                false,
                new string[] { "PassportExpiryReport" },
                ACalc,
                this,
                true,
                false);
        }
    }
}