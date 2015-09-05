//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters
//
// Copyright 2004-2012 by OM International
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
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for the anniversaries
    /// </summary>
    public partial class TFrmLengthOfCommitmentReport
    {
        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (AReportAction == TReportActionEnum.raGenerate)
            {
                // validate anniversaires (comma seperated integers)
                if (chkAnniversaries.Checked && (string.IsNullOrEmpty(txtAnniversaries.Text)
                                                 || !Regex.IsMatch(txtAnniversaries.Text, @"^([0-9]+,)*[0-9]+$")))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Length of Commitment Report"),
                        Catalog.GetString("Please enter a comma seperated list of anniversaires."),
                        TResultSeverity.Resv_Critical);

                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }
        }
    }
}