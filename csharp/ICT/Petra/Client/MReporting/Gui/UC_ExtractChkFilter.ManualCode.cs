//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Andrew Webster <arw7@students.calvin.edu>
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of TFrmUC_Extract
    /// </summary>
    public partial class TFrmUC_ExtractChkFilter
    {
        /// <summary>
        /// initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;
        }

        /// <summary>
        /// Reads the selected values from the controls, and stores them into the parameter system of FCalculator
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_active", this.chkActivePartners.Checked);
            ACalc.AddParameter("param_mailing_addresses_only", this.chkMailingAddressesOnly.Checked);
            ACalc.AddParameter("param_persons_only", this.chkPersonsOnly.Checked);
            ACalc.AddParameter("param_families_only", this.chkFamiliesOnly.Checked);
            ACalc.AddParameter("param_exclude_no_solicitations", this.chkExcludeNoSolicitations.Checked);
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            chkActivePartners.Checked = AParameters.Get("param_active").ToBool();
            chkMailingAddressesOnly.Checked = AParameters.Get("param_mailing_addresses_only").ToBool();
            chkPersonsOnly.Checked = AParameters.Get("param_persons_only").ToBool();
            chkFamiliesOnly.Checked = AParameters.Get("param_families_only").ToBool();
            chkExcludeNoSolicitations.Checked = AParameters.Get("param_exclude_no_solicitations").ToBool();
        }

        /// <summary>
        /// This will add functions to the list of available functions
        /// </summary>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// hide/show tick box for "Families Only"
        /// </summary>
        public void ShowFamiliesOnly(bool AShow)
        {
            chkFamiliesOnly.Visible = AShow;
        }

        /// <summary>
        /// hide/show tick box for "Families Only"
        /// </summary>
        public void ShowPersonsOnly(bool AShow)
        {
            chkPersonsOnly.Visible = AShow;

            if (AShow == false)
            {
                int adjustment = chkFamiliesOnly.Top - chkPersonsOnly.Top;
                chkFamiliesOnly.Top -= adjustment;
                chkExcludeNoSolicitations.Top -= adjustment;
            }
        }

        /// <summary>
        /// hide/show tick box for "Mailing Addresses Only"
        /// </summary>
        public void ShowMailingAddressesOnly(bool AShow)
        {
            chkMailingAddressesOnly.Visible = AShow;
        }

        private void OnChangePersons(object sender, EventArgs e)
        {
            chkFamiliesOnly.Enabled = chkPersonsOnly.Checked == false;
        }

        private void OnChangeFamilies(object sender, EventArgs e)
        {
            if (!chkPersonsOnly.Visible)
            {
                return;
            }

            chkPersonsOnly.Enabled = chkFamiliesOnly.Checked == false;
        }
    }
}