//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       bernd
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel.ShortTerm
{
    /// <summary>
    /// Description of TFrmUC_ShortTermerAdditionalSetting
    /// </summary>
    public partial class TFrmUC_ShortTermerAdditionalSetting
    {
        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;
        }

        /// <summary>
        /// set the functions and column names that are available
        /// </summary>
        /// <param name="AAvailableFunctions"></param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// read the values from the controls and give them to the calculator
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            if (chkHideEmptyLines.Checked)
            {
                ACalculator.AddParameter("param_hide_empty_lines", "true");
            }
            else
            {
                ACalculator.AddParameter("param_hide_empty_lines", "false");
            }

            if (chkPrintTwoLines.Checked)
            {
                ACalculator.AddParameter("param_print_two_lines", "true");
                // we can't sort with two lines
                ACalculator.RemoveParameter("param_sortby_columns");
            }
            else
            {
                ACalculator.AddParameter("param_print_two_lines", "false");
            }
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            chkHideEmptyLines.Checked = (AParameters.Get("param_hide_empty_lines").ToString() == "true");
            chkPrintTwoLines.Checked = (AParameters.Get("param_print_two_lines").ToString() == "true");
        }
    }
}