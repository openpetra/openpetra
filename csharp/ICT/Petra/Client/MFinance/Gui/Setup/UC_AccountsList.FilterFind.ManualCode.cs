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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    /// <summary>
    /// A helper manual code file that contains the logic for Filter Find associated with the Accounts List
    /// </summary>
    public class TUC_AccountsListFilterFind
    {
        private TextBox FFilterTxtAccountCode = null;
        private TCmbAutoComplete FFilterCmbAccountType = null;
        private TextBox FFilterTxtDescrEnglish = null;
        private TextBox FFilterTxtDescrLocal = null;
        private CheckBox FFilterChkBankAccount = null;
        private CheckBox FFilterChkActive = null;
        private CheckBox FFilterChkSummary = null;
        private CheckBox FFilterChkForeign = null;

        private TextBox FFindTxtAccountCode = null;
        private TCmbAutoComplete FFindCmbAccountType = null;
        private TextBox FFindTxtDescrEnglish = null;
        private TextBox FFindTxtDescrLocal = null;
        private CheckBox FFindChkBankAccount = null;
        private CheckBox FFindChkActive = null;
        private CheckBox FFindChkSummary = null;
        private CheckBox FFindChkForeign = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AFilterFindPanelObject">The FilterFindPanel Object on the main form</param>
        public TUC_AccountsListFilterFind(TFilterAndFindPanel AFilterFindPanelObject)
        {
            FFilterTxtAccountCode = (TextBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("txtAccountCode");
            FFilterCmbAccountType = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbAccountType");
            FFilterTxtDescrEnglish = (TextBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("txtDescrEnglish");
            FFilterTxtDescrLocal = (TextBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("txtDescrLocal");
            FFilterChkBankAccount = (CheckBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("chkBankAccount");
            FFilterChkActive = (CheckBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("chkActive");
            FFilterChkSummary = (CheckBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("chkSummary");
            FFilterChkForeign = (CheckBox)AFilterFindPanelObject.FilterPanelControls.FindControlByName("chkForeign");

            FFindTxtAccountCode = (TextBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("txtAccountCode");
            FFindCmbAccountType = (TCmbAutoComplete)AFilterFindPanelObject.FindPanelControls.FindControlByName("cmbAccountType");
            FFindTxtDescrEnglish = (TextBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("txtDescrEnglish");
            FFindTxtDescrLocal = (TextBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("txtDescrLocal");
            FFindChkBankAccount = (CheckBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("chkBankAccount");
            FFindChkActive = (CheckBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("chkActive");
            FFindChkSummary = (CheckBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("chkSummary");
            FFindChkForeign = (CheckBox)AFilterFindPanelObject.FindPanelControls.FindControlByName("chkForeign");
        }

        /// <summary>
        /// Implementation of the logic for setting the filter string for the accounts list screen
        /// </summary>
        /// <param name="AFilterString">The desired filter string</param>
        /// <param name="AAccountTable">The table instance that is being used for the data</param>
        public void ApplyFilterManual(ref string AFilterString, GLSetupTDSAAccountTable AAccountTable)
        {
            string filter = String.Empty;

            if (FFilterTxtAccountCode.Text != String.Empty)
            {
                JoinAndAppend(ref filter, String.Format("({0} LIKE '%{1}%')", AAccountTable.ColumnAccountCode, FFilterTxtAccountCode.Text));
            }

            if (FFilterCmbAccountType.Text != String.Empty)
            {
                JoinAndAppend(ref filter, String.Format("({0} LIKE '{1}')", AAccountTable.ColumnAccountType, FFilterCmbAccountType.Text));
            }

            if (FFilterTxtDescrEnglish.Text != String.Empty)
            {
                JoinAndAppend(ref filter,
                    String.Format("({0} LIKE '%{1}%')", AAccountTable.ColumnEngAccountCodeLongDesc, FFilterTxtDescrEnglish.Text));
            }

            if (FFilterTxtDescrLocal.Text != String.Empty)
            {
                JoinAndAppend(ref filter, String.Format("({0} LIKE '%{1}%')", AAccountTable.ColumnAccountCodeLongDesc, FFilterTxtDescrLocal.Text));
            }

            if (FFilterChkBankAccount.CheckState != CheckState.Indeterminate)
            {
                JoinAndAppend(ref filter, String.Format("({0}={1})", AAccountTable.ColumnBankAccountFlag, FFilterChkBankAccount.Checked ? 1 : 0));
            }

            if (FFilterChkActive.CheckState != CheckState.Indeterminate)
            {
                JoinAndAppend(ref filter, String.Format("({0}={1})", AAccountTable.ColumnAccountActiveFlag, FFilterChkActive.Checked ? 1 : 0));
            }

            if (FFilterChkSummary.CheckState != CheckState.Indeterminate)
            {
                JoinAndAppend(ref filter, String.Format("({0}={1})", AAccountTable.ColumnPostingStatus, FFilterChkSummary.Checked ? 0 : 1));
            }

            if (FFilterChkForeign.CheckState != CheckState.Indeterminate)
            {
                JoinAndAppend(ref filter, String.Format("({0}={1})", AAccountTable.ColumnForeignCurrencyFlag, FFilterChkForeign.Checked ? 1 : 0));
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
            string strAccountCode = FFindTxtAccountCode.Text.ToLower();
            string strAccountType = FFindCmbAccountType.Text.ToLower();
            string strAccountDescrEnglish = FFindTxtDescrEnglish.Text.ToLower();
            string strAccountDescrLocal = FFindTxtDescrLocal.Text.ToLower();
            bool isBankAccount = FFindChkBankAccount.Checked;
            bool isActive = FFindChkActive.Checked;
            bool isSummary = FFindChkSummary.Checked;
            bool isForeign = FFindChkForeign.Checked;

            GLSetupTDSAAccountRow accountRow = (GLSetupTDSAAccountRow)ARow;

            if (strAccountCode != String.Empty)
            {
                if (!accountRow.AccountCode.ToLower().Contains(strAccountCode))
                {
                    return false;
                }
            }

            if (strAccountType != String.Empty)
            {
                if (!accountRow.AccountType.ToLower().Contains(strAccountType))
                {
                    return false;
                }
            }

            if (strAccountDescrEnglish != String.Empty)
            {
                if (!accountRow.EngAccountCodeLongDesc.ToLower().Contains(strAccountDescrEnglish))
                {
                    return false;
                }
            }

            if (strAccountDescrLocal != String.Empty)
            {
                if (!accountRow.AccountCodeLongDesc.ToLower().Contains(strAccountDescrLocal))
                {
                    return false;
                }
            }

            if (FFindChkBankAccount.CheckState != CheckState.Indeterminate)
            {
                if (accountRow.BankAccountFlag != isBankAccount)
                {
                    return false;
                }
            }

            if (FFindChkActive.CheckState != CheckState.Indeterminate)
            {
                if (accountRow.AccountActiveFlag != isActive)
                {
                    return false;
                }
            }

            if (FFindChkSummary.CheckState != CheckState.Indeterminate)
            {
                if (accountRow.PostingStatus == isSummary)
                {
                    return false;
                }
            }

            if (FFindChkForeign.CheckState != CheckState.Indeterminate)
            {
                if (accountRow.ForeignCurrencyFlag != isActive)
                {
                    return false;
                }
            }

            return true;
        }
    }
}