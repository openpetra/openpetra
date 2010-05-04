/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
	/// <summary>
	/// Description of UC_PartnerSelection.ManualCode.
	/// </summary>
	public partial class TFrmUC_PartnerSelection
	{
        /// <summary>
        /// Initialisation
        /// </summary>
		public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
		{
			FPetraUtilsObject = APetraUtilsObject;
			
			rbtPartner.Checked = true;
			txtExtract.Enabled = false;
			dtpCurrentStaff.Enabled = false;
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
        	if (rbtPartner.Checked)
        	{
        		ACalculator.AddParameter("param_selection", "one partner");
        	}
        	else if (rbtExtract.Checked)
        	{
	        	ACalculator.AddParameter("param_selection", "an extract");
        	}
        	else if (rbtCurrentStaff.Checked)
        	{
        		ACalculator.AddParameter("param_selection", "all current staff");
        	}
        	else if (rbtAllStaff.Checked)
        	{
        		ACalculator.AddParameter("param_selection", "all staff");
        	}
        	ACalculator.AddParameter("param_currentstaffdate", dtpCurrentStaff.Date);
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
        	rbtPartner.Checked = (AParameters.Get("param_selection").ToString() == "one partner");
        	rbtExtract.Checked = (AParameters.Get("param_selection").ToString() == "an extract");
        	rbtCurrentStaff.Checked = (AParameters.Get("param_selection").ToString() == "all current staff");
        	rbtAllStaff.Checked = (AParameters.Get("param_selection").ToString() == "all staff");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtSelectionChange(object sender, EventArgs e)
        {
        	RadioButton RBtn = sender as RadioButton;
        	
        	if (RBtn.Name == "rbtPartner")
        	{
	        	txtPartnerKey.Enabled = RBtn.Checked;
        	}
        	else if (RBtn.Name == "rbtExtract")
        	{
	        	txtExtract.Enabled = RBtn.Checked;
        	}
        	else if (RBtn.Name == "rbtCurrentStaff")
        	{
	        	dtpCurrentStaff.Enabled = RBtn.Checked;
        	}
        }
	}
}
