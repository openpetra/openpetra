//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
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
//
using System;
using System.Text;
using System.Data;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    /// <summary>
    /// A helper manual code file that contains the logic for Filter Find associated with the Accounts List
    /// </summary>
    public class TUC_CostCentreListFilterFind
    {
        private TextBox FFilterTxtCostCentreCode = null;
        private TCmbAutoComplete FFilterCmbCostCentreType = null;
        private TextBox FFilterTxtCostCentreName = null;
        private CheckBox FFilterChkActive = null;

        private TextBox FFindTxtCostCentreCode = null;
        private TCmbAutoComplete FFindCmbCostCentreType = null;
        private TextBox FFindTxtCostCentreName = null;
        private CheckBox FFindChkActive = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AFilterFindPanelObject">The FilterFindPanel Object on the main form</param>
        public TUC_CostCentreListFilterFind(TFilterAndFindPanel AFilterFindPanelObject)
        {
            FFilterTxtCostCentreCode = (TextBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("txtCostCentreCode");
            FFilterCmbCostCentreType = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbCostCentreType");
            FFilterTxtCostCentreName = (TextBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("txtCostCentreName");
            FFilterChkActive = (CheckBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("chkActive");

            FFindTxtCostCentreCode = (TextBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("txtCostCentreCode");
            FFindCmbCostCentreType = (TCmbAutoComplete)AFilterFindPanelObject.FindPanelControls.FindControlByName("cmbCostCentreType");
            FFindTxtCostCentreName = (TextBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("txtCostCentreName");
            FFindChkActive = (CheckBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("chkActive");
        }

        /// <summary>
        /// Implementation of the logic for setting the filter string for the accounts list screen
        /// </summary>
        /// <param name="AFilterString">The desired filter string</param>
        /// <param name="AMainDataSet">The data set</param>
        public void ApplyFilterManual(ref string AFilterString, GLSetupTDS AMainDataSet)
        {
            string filter = String.Empty;

            if (FFilterTxtCostCentreCode.Text != String.Empty)
            {
                JoinAndAppend(ref filter,
                    String.Format("({0} LIKE '%{1}%')", AMainDataSet.ACostCentre.ColumnCostCentreCode, FFilterTxtCostCentreCode.Text));
            }

            if (FFilterCmbCostCentreType.Text != String.Empty)
            {
                JoinAndAppend(ref filter,
                    String.Format("({0} LIKE '{1}')", AMainDataSet.ACostCentre.ColumnCostCentreType, FFilterCmbCostCentreType.Text));
            }

            if (FFilterTxtCostCentreName.Text != String.Empty)
            {
                JoinAndAppend(ref filter,
                    String.Format("({0} LIKE '%{1}%')", AMainDataSet.ACostCentre.ColumnCostCentreName, FFilterTxtCostCentreName.Text));
            }

            if (FFilterChkActive.CheckState != CheckState.Indeterminate)
            {
                JoinAndAppend(ref filter,
                    String.Format("({0}={1})", AMainDataSet.ACostCentre.ColumnCostCentreActiveFlag, FFilterChkActive.Checked ? 1 : 0));
            }

            AFilterString = filter;
        }

        private void JoinAndAppend(ref string AStringToExtend, string AStringToAppend)
        {
            if (AStringToExtend.Length > 0)
            {
                AStringToExtend += " AND ";
            }

            AStringToExtend += AStringToAppend;
        }

        /// <summary>
        /// The implementation of the logic for testing for a matching row
        /// </summary>
        /// <param name="ARow">The Row to test</param>
        /// <returns>True for a match</returns>
        public bool IsMatchingRowManual(DataRow ARow)
        {
            string strCostCentreCode = FFindTxtCostCentreCode.Text.ToLower();
            string strCostCentreType = FFindCmbCostCentreType.Text.ToLower();
            string strCostCentreName = FFindTxtCostCentreName.Text.ToLower();
            bool isActive = FFindChkActive.Checked;

            ACostCentreRow costCentreRow = (ACostCentreRow)ARow;

            if (strCostCentreCode != String.Empty)
            {
                if (!costCentreRow.CostCentreCode.ToLower().Contains(strCostCentreCode))
                {
                    return false;
                }
            }

            if (strCostCentreType != String.Empty)
            {
                if (!costCentreRow.CostCentreType.ToLower().Contains(strCostCentreType))
                {
                    return false;
                }
            }

            if (strCostCentreName != String.Empty)
            {
                if (!costCentreRow.CostCentreName.ToLower().Contains(strCostCentreName))
                {
                    return false;
                }
            }

            if (FFindChkActive.CheckState != CheckState.Indeterminate)
            {
                if (costCentreRow.CostCentreActiveFlag != isActive)
                {
                    return false;
                }
            }

            return true;
        }
    }
}