//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using SourceGrid;
using System.Collections.Specialized;
using System.Globalization;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// A UserControl that allows to select several combinations
    /// of Motivation Group and Detail entries
    ///
    /// This could be implemented with 2 comboboxes and a
    /// list box to its right that displays the selected Motivations, and 2 buttons to add and remove.
    /// For the moment it is just implemented as a Checkedlistbox
    ///
    /// This is not used anywhere, but could be used in future as a starting point
    ///
    /// @Comment This was copied and modified from UC_MultiMotivationDetailSelection.pas
    /// </summary>
    public partial class TUC_MultiMotivationDetailSelection : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TUC_MultiMotivationDetailSelection()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
            this.clbMotivations.BindingContext = this.BindingContext;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseUserControl(Int32 ALedgerNumber)
        {
            DataTable Table;
            String CheckedMember = "";
            String ValueMember = "";
            String DisplayMember = "";

            // load the motivation details of this ledger
            Table = GetMotivationDetails(ALedgerNumber,
                ref CheckedMember,
                ref DisplayMember,
                ref ValueMember, true);

            // AExcludeInactive
            // Columns for the CostCentre checked listbox (grid)
            this.clbMotivations.Columns.Clear();
            this.clbMotivations.AddCheckBoxColumn("", Table.Columns[CheckedMember], 17);
            this.clbMotivations.AddTextColumn("Test1", Table.Columns[ValueMember], 60);
            this.clbMotivations.AddTextColumn("Test2", Table.Columns[DisplayMember], 200);
            this.clbMotivations.DataBindGrid(Table, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CustomDisable()
        {
            CustomEnablingDisabling.DisableControl(this, clbMotivations);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CustomEnable()
        {
            CustomEnablingDisabling.EnableControl(this, clbMotivations);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALedgerNr"></param>
        /// <param name="ACheckedMember"></param>
        /// <param name="ADisplayMember"></param>
        /// <param name="AValueMember"></param>
        /// <param name="AExcludeInactive"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetMotivationDetails(System.Int32 ALedgerNr,
            ref String ACheckedMember,
            ref String ADisplayMember,
            ref String AValueMember,
            bool AExcludeInactive)
        {
            System.Data.DataTable ReturnValue;
            DataTable CachedDataTable;
            String whereClause;
            DataRow[] filteredRows;
            CachedDataTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, ALedgerNr);
            whereClause = AMotivationDetailTable.GetLedgerNumberDBName() + " = " + ALedgerNr.ToString();

            if (AExcludeInactive)
            {
                whereClause = whereClause + " AND " + AMotivationDetailTable.GetMotivationStatusDBName() + " = 1";
            }

            filteredRows = CachedDataTable.Select(whereClause,
                AMotivationDetailTable.GetMotivationGroupCodeDBName() + ',' + AMotivationDetailTable.GetMotivationDetailCodeDBName());
            ReturnValue = CachedDataTable.Clone();

            foreach (DataRow oldDr in filteredRows)
            {
                ReturnValue.ImportRow(oldDr);
            }

            ACheckedMember = "CHECKED";
            ADisplayMember = "DISPLAY";
            AValueMember = "VALUE";
            ReturnValue.Columns.Add(new DataColumn(ACheckedMember, typeof(bool)));
            ReturnValue.Columns.Add(new DataColumn(AValueMember, typeof(String)));
            ReturnValue.Columns.Add(new DataColumn(ADisplayMember, typeof(String)));

            foreach (DataRow oldDr in ReturnValue.Rows)
            {
                oldDr[AValueMember] =
                    (System.Object)(oldDr[AMotivationDetailTable.GetMotivationGroupCodeDBName()].ToString() + ", " +
                        oldDr[AMotivationDetailTable.GetMotivationDetailCodeDBName()].ToString());
                oldDr[ADisplayMember] =
                    (System.Object)(oldDr[AMotivationDetailTable.GetMotivationGroupCodeDBName()].ToString() + " - " +
                        oldDr[AMotivationDetailTable.GetMotivationDetailCodeDBName()].ToString());
            }

            return ReturnValue;
        }
    }
}