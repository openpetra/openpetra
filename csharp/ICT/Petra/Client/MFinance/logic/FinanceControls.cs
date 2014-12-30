//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Specialized;
using System.Data;
using SourceGrid;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// this provides some static functions that initialise
    /// comboboxes and other controls with static values or cached values for the finance module
    /// this helps to make similar controls look the same throughout the application
    /// </summary>
    public class TFinanceControls
    {
        /// <summary>
        /// Check if a given account is active
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="AAccountList"></param>
        /// <param name="AAccountExists"></param>
        /// <returns></returns>
        public static bool AccountIsActive(Int32 ALedgerNumber,
            string AAccountCode,
            AAccountTable AAccountList,
            out bool AAccountExists)
        {
            AAccountExists = false;
            bool RetVal = false;

            AAccountRow CurrentAccountRow = null;

            if (AAccountList != null)
            {
                CurrentAccountRow = (AAccountRow)AAccountList.Rows.Find(new object[] { ALedgerNumber, AAccountCode });

                if (CurrentAccountRow != null)
                {
                    AAccountExists = true;
                    RetVal = CurrentAccountRow.AccountActiveFlag;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Check if a given cost centre is active
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="ACostCentreList"></param>
        /// <param name="ACostCentreExists"></param>
        /// <returns></returns>
        public static bool CostCentreIsActive(Int32 ALedgerNumber,
            string ACostCentreCode,
            ACostCentreTable ACostCentreList,
            out bool ACostCentreExists)
        {
            ACostCentreExists = false;
            bool RetVal = false;

            ACostCentreRow CurrentCostCentreRow = null;

            if (ACostCentreList != null)
            {
                CurrentCostCentreRow = (ACostCentreRow)ACostCentreList.Rows.Find(new object[] { ALedgerNumber, ACostCentreCode });

                if (CurrentCostCentreRow != null)
                {
                    ACostCentreExists = true;
                    RetVal = CurrentCostCentreRow.CostCentreActiveFlag;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// returns a filter for cost centre cached table
        /// </summary>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ALocalOnly"></param>
        public static string PrepareCostCentreFilter(bool APostingOnly, bool AExcludePosting, bool AActiveOnly, bool ALocalOnly)
        {
            string Filter = ACostCentreTable.GetCostCentreCodeDBName() + " = '' OR (";

            Filter += "1=1";

            if (APostingOnly)
            {
                Filter += " AND " + ACostCentreTable.GetPostingCostCentreFlagDBName() + " = true";
            }
            else if (AExcludePosting)
            {
                Filter += " AND " + ACostCentreTable.GetPostingCostCentreFlagDBName() + " = false";
            }

            if (AActiveOnly)
            {
                Filter += " AND " + ACostCentreTable.GetCostCentreActiveFlagDBName() + " = true";
            }

            if (ALocalOnly)
            {
                Filter += " AND " + ACostCentreTable.GetCostCentreTypeDBName() + " = 'Local'";
            }

            Filter += ")";

            return Filter;
        }

        // Adapter for TClbVersatile-init ...
        private static string PrepareAccountFilter(bool APostingOnly, bool AExcludePosting,
            bool AActiveOnly, bool ABankAccountOnly)
        {
            return PrepareAccountFilter(APostingOnly, AExcludePosting, AActiveOnly, ABankAccountOnly, "");
        }

        /// <summary>
        /// returns a filter for accounts cached table
        /// </summary>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ABankAccountOnly"></param>
        /// <param name="AForeignCurrencyName"></param>
        /// <returns>The filter string which shall be used in the data view</returns>
        public static string PrepareAccountFilter(bool APostingOnly, bool AExcludePosting,
            bool AActiveOnly, bool ABankAccountOnly,
            string AForeignCurrencyName)
        {
            string Filter = AAccountTable.GetAccountCodeDBName() + " = '' OR (";

            Filter += "1=1";

            if (APostingOnly)
            {
                Filter += " AND " + AAccountTable.GetPostingStatusDBName() + " = true";
            }
            else if (AExcludePosting)
            {
                Filter += " AND " + AAccountTable.GetPostingStatusDBName() + " = false";
            }

            if (AActiveOnly)
            {
                Filter += " AND " + AAccountTable.GetAccountActiveFlagDBName() + " = true";
            }

            // GetCacheableFinanceTable returns a DataTable with a bank flag
            if (ABankAccountOnly)
            {
                Filter += " AND (" + GLSetupTDSAAccountTable.GetBankAccountFlagDBName() + " = true OR " +
                          GLSetupTDSAAccountTable.GetCashAccountFlagDBName() + " = true)";
            }

            // AForeignCurrencyName.Equals("") means use default or do nothing!
            if (!AForeignCurrencyName.Equals(""))
            {
                Filter += " AND (";  // Bracket 1
                Filter += AAccountTable.GetForeignCurrencyFlagDBName() + " = false";
                Filter += " OR (";   // Bracket 2
                Filter += AAccountTable.GetForeignCurrencyFlagDBName() + " = true";
                Filter += " AND ";
                Filter += AAccountTable.GetForeignCurrencyCodeDBName() +
                          " = '" + AForeignCurrencyName + "'";
                Filter += ")";       // Bracket 2
                Filter += ")";       // Bracket 1
            }

            Filter += ")";

            return Filter;
        }

        /// <summary>
        /// fill checkedlistbox values with cost centre list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ALocalOnly">Local Costcentres only; otherwise foreign costcentres (ie from other legal entities) are included)</param>
        /// <param name="AIndicateInactive">Determines wether or not to indicate an account code as inactive</param>
        public static void InitialiseCostCentreList(ref TClbVersatile AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool ALocalOnly,
            bool AIndicateInactive = false)
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = ACostCentreTable.GetCostCentreNameDBName();
            string ValueMember = ACostCentreTable.GetCostCentreCodeDBName();

            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, ALedgerNumber);
            DataView view = new DataView(Table);

            view.RowFilter = PrepareCostCentreFilter(APostingOnly, AExcludePosting, AActiveOnly, ALocalOnly);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            AControl.Columns.Clear();
            AControl.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            AControl.AddTextColumn(Catalog.GetString("Code"), NewTable.Columns[ValueMember], 90);
            AControl.AddTextColumn(Catalog.GetString("Cost Centre Description"), NewTable.Columns[DisplayMember], 232);
            AControl.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        /// <summary>
        /// fill checkedlistbox values with cost centre list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="AFieldOnly"></param>
        /// <param name="ADepartmentOnly"></param>
        /// <param name="APersonalOnly"></param>
        public static void InitialiseCostCentreList(ref TClbVersatile AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool AFieldOnly,
            bool ADepartmentOnly,
            bool APersonalOnly)
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = ACostCentreTable.GetCostCentreNameDBName();
            string ValueMember = ACostCentreTable.GetCostCentreCodeDBName();

            //ACostCentreTable CostCentreTable = (ACostCentreTable)
            ACostCentreTable CostCentreTable = (ACostCentreTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.CostCentreList,
                ALedgerNumber);

            Type TableType;
            DataTable LinkedCostCentres = TDataCache.TMFinance.GetBasedOnLedger(
                TCacheableFinanceTablesEnum.CostCentresLinkedToPartnerList,
                ALedgerTable.GetLedgerNumberDBName(),
                ALedgerNumber,
                out TableType);

            LinkedCostCentres.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();

            foreach (ACostCentreRow CostCentreRow in CostCentreTable.Rows)
            {
                // there can be several partners linked to a cost centre
                DataRowView[] LinkedCCs = LinkedCostCentres.DefaultView.FindRows(CostCentreRow[ACostCentreTable.GetCostCentreCodeDBName()].ToString());

                bool DoNotShow = true;

                if (AFieldOnly)
                {
                    DoNotShow = CostCentreRow.CostCentreType != "Foreign";
                }
                else
                {
                    foreach (DataRowView rv in LinkedCCs)
                    {
                        DataRow LinkedCC = rv.Row;

                        if (APersonalOnly)
                        {
                            // personal costcentres are linked to a partner of type FAMILY
                            if ((LinkedCC != null)
                                && (LinkedCC[PPartnerTable.GetPartnerClassDBName()].ToString() == MPartnerConstants.PARTNERCLASS_FAMILY))
                            {
                                DoNotShow = false;
                            }
                        }
                        else if (ADepartmentOnly)
                        {
                            // department costcentres are a local costcentre, linked to no partner, or are not a unit, or UnitType != F
                            if ((LinkedCC == null)
                                || ((LinkedCC[PUnitTable.GetUnitTypeCodeDBName()].ToString() != MPartnerConstants.UNIT_TYPE_FIELD)
                                    && (LinkedCC[PPartnerTable.GetPartnerClassDBName()].ToString() != MPartnerConstants.PARTNERCLASS_FAMILY)))
                            {
                                DoNotShow = false;
                            }
                        }
                    }
                }

                if (DoNotShow)
                {
                    CostCentreRow.CostCentreName = "DONOTSHOW";
                }
            }

            DataView view = new DataView(CostCentreTable);

            view.RowFilter = "(" + PrepareCostCentreFilter(APostingOnly, AExcludePosting, AActiveOnly, ADepartmentOnly) +
                             ") AND NOT " + ACostCentreTable.GetCostCentreNameDBName() + "='DONOTSHOW'";

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            AControl.Columns.Clear();
            AControl.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            AControl.AddTextColumn(Catalog.GetString("Code"), NewTable.Columns[ValueMember], 60);
            AControl.AddTextColumn(Catalog.GetString("Cost Centre Description"), NewTable.Columns[DisplayMember], 200);
            AControl.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        /// <summary>
        /// fill checkedlistbox values with account codes
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ABankAccountOnly"></param>
        /// <param name="AIndicateInactive">Determines whether or not to show inactive accounts as inactive</param>
        public static void InitialiseAccountList(ref TClbVersatile AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool ABankAccountOnly,
            bool AIndicateInactive = false)
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = AAccountTable.GetAccountCodeShortDescDBName();
            string ValueMember = AAccountTable.GetAccountCodeDBName();

            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, ALedgerNumber);
            DataView view = new DataView(Table);

            view.RowFilter = PrepareAccountFilter(APostingOnly, AExcludePosting, AActiveOnly, ABankAccountOnly);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            //Highlight inactive Accounts
            if (!AActiveOnly && AIndicateInactive)
            {
                foreach (DataRow rw in NewTable.Rows)
                {
                    if ((rw[AAccountTable.ColumnAccountActiveFlagId] != null) && (rw[AAccountTable.ColumnAccountActiveFlagId].ToString() == "False"))
                    {
                        rw[AAccountTable.ColumnAccountCodeShortDescId] = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " " +
                                                                         rw[AAccountTable.ColumnAccountCodeShortDescId];
                    }
                }
            }

            AControl.Columns.Clear();
            AControl.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            AControl.AddTextColumn(Catalog.GetString("Code"), NewTable.Columns[ValueMember], 90);
            AControl.AddTextColumn(Catalog.GetString("Account Description"), NewTable.Columns[DisplayMember], 232);
            AControl.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        /// <summary>
        /// fill combobox values with cost centre list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ALocalOnly">Local Costcentres only; otherwise foreign costcentres (ie from other legal entities) are included)</param>
        /// <param name="AIndicateInactive">Determines wether or not to indicate a cost centre as inactive</param>
        /// <param name="AACostCentreListDataSource">If a reference to the ACostCentreList is available, pass it here.  Otherwise the method
        /// will get it from the data cache.</param>
        public static void InitialiseCostCentreList(ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool ALocalOnly,
            bool AIndicateInactive = false,
            DataTable AACostCentreListDataSource = null)
        {
            DataTable Table;

            if (AACostCentreListDataSource == null)
            {
                Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, ALedgerNumber);
            }
            else
            {
                Table = AACostCentreListDataSource.Copy();
            }

            // add empty row so that SetSelectedString for invalid string will not result in undefined behaviour (selecting the first cost centre etc)
            DataRow emptyRow = Table.NewRow();

            emptyRow[ACostCentreTable.ColumnLedgerNumberId] = ALedgerNumber;
            emptyRow[ACostCentreTable.ColumnCostCentreCodeId] = string.Empty;
            emptyRow[ACostCentreTable.ColumnCostCentreNameId] = Catalog.GetString("Select a valid cost centre");

            Table.Rows.Add(emptyRow);

            //Highlight inactive Cost Centres
            if (!AActiveOnly && AIndicateInactive)
            {
                foreach (DataRow rw in Table.Rows)
                {
                    if ((rw[ACostCentreTable.ColumnCostCentreActiveFlagId] != null)
                        && (rw[ACostCentreTable.ColumnCostCentreActiveFlagId].ToString() == "False"))
                    {
                        rw[ACostCentreTable.ColumnCostCentreNameId] = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " " +
                                                                      rw[ACostCentreTable.ColumnCostCentreNameId];
                    }
                }
            }

            AControl.InitialiseUserControl(Table,
                ACostCentreTable.GetCostCentreCodeDBName(),
                ACostCentreTable.GetCostCentreNameDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 200 }, -1);

            AControl.Filter = PrepareCostCentreFilter(APostingOnly, AExcludePosting, AActiveOnly, ALocalOnly);
        }

        /// Adapter for the modules which have been developed before multi-currency support
        /// was required
        public static void InitialiseAccountList(ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool ABankAccountOnly,
            bool AIndicateInactive = false,
            DataTable AAAccountListDataSource = null)
        {
            InitialiseAccountList(
                ref AControl, ALedgerNumber, APostingOnly,
                AExcludePosting, AActiveOnly, ABankAccountOnly, "", AIndicateInactive, AAAccountListDataSource);
        }

        /// <summary>
        /// Fill combobox values with account codes
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingOnly"></param>
        /// <param name="AExcludePosting"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ABankAccountOnly"></param>
        /// <param name="AForeignCurrencyName">If a value is defined, only base curreny or the defined currency are filtered</param>
        /// <param name="AIndicateInactive">Determines wether or not to indicate an account code as inactive</param>
        /// <param name="AAAccountListDataSource">If a reference to the AAccountList is available, pass it here.  Otherwise the method
        /// will get it from the data cache.</param>
        public static void InitialiseAccountList(ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            bool APostingOnly,
            bool AExcludePosting,
            bool AActiveOnly,
            bool ABankAccountOnly,
            string AForeignCurrencyName,
            bool AIndicateInactive = false,
            DataTable AAAccountListDataSource = null)
        {
            DataTable Table;

            if (AAAccountListDataSource == null)
            {
                Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, ALedgerNumber);
            }
            else
            {
                Table = AAAccountListDataSource.Copy();
            }

            // add empty row so that SetSelectedString for invalid string will not result in undefined behaviour (selecting the first account etc)
            DataRow emptyRow = Table.NewRow();

            emptyRow[AAccountTable.ColumnLedgerNumberId] = ALedgerNumber;
            emptyRow[AAccountTable.ColumnAccountCodeId] = string.Empty;
            emptyRow[AAccountTable.ColumnAccountCodeShortDescId] = Catalog.GetString("Select a valid account");
            Table.Rows.Add(emptyRow);

            //Highlight inactive Accounts
            if (!AActiveOnly && AIndicateInactive)
            {
                foreach (DataRow rw in Table.Rows)
                {
                    if ((rw[AAccountTable.ColumnAccountActiveFlagId] != null) && (rw[AAccountTable.ColumnAccountActiveFlagId].ToString() == "False"))
                    {
                        rw[AAccountTable.ColumnAccountCodeShortDescId] = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " " +
                                                                         rw[AAccountTable.ColumnAccountCodeShortDescId];
                    }
                }
            }

            AControl.InitialiseUserControl(Table,
                AAccountTable.GetAccountCodeDBName(),
                AAccountTable.GetAccountCodeShortDescDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 200 }, -1);

            AControl.Filter = PrepareAccountFilter(APostingOnly, AExcludePosting, AActiveOnly,
                ABankAccountOnly, AForeignCurrencyName);
        }

        /// <summary>
        /// fill combobox values with list of transaction types
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ASubSystemCode"></param>
        public static void InitialiseTransactionTypeList(ref TCmbAutoPopulated AControl, Int32 ALedgerNumber, string ASubSystemCode)
        {
            // TODO: use cached table for transaction types? use filter to get only appropriate types for subsystem?
            TTypedDataTable Table;

            TRemote.MCommon.DataReader.WebConnectors.GetData(TTypedDataTable.GetTableNameSQL(ATransactionTypeTable.TableId),
                new TSearchCriteria[] {
                    new TSearchCriteria(TTypedDataTable.GetColumnNameSQL(ATransactionTypeTable.TableId,
                            ATransactionTypeTable.ColumnLedgerNumberId), ALedgerNumber),
                    new TSearchCriteria(TTypedDataTable.GetColumnNameSQL(ATransactionTypeTable.TableId,
                            ATransactionTypeTable.ColumnSubSystemCodeId), ASubSystemCode)
                },
                out Table);

            AControl.InitialiseUserControl(
                Table,
                ATransactionTypeTable.GetTransactionTypeCodeDBName(),
                ATransactionTypeTable.GetTransactionTypeDescriptionDBName(),
                null);

            AControl.AppearanceSetup(new int[] { -1, 150 }, -1);
        }

        /// <summary>
        /// fill combobox values with motivation group list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AActiveOnly"></param>
        public static void InitialiseMotivationGroupList(ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            bool AActiveOnly)
        {
            DataTable groupTable =
                (AMotivationGroupTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationGroupList, ALedgerNumber);

            AControl.InitialiseUserControl(groupTable,
                AMotivationGroupTable.GetMotivationGroupCodeDBName(),
                AMotivationGroupTable.GetMotivationGroupDescriptionDBName(),
                null);

            AControl.AppearanceSetup(new int[] { -1, 200 }, -1);
        }

        /// <summary>
        /// fill combobox values with motivation detail list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AActiveOnly"></param>
        public static void InitialiseMotivationDetailList(ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            bool AActiveOnly)
        {
            DataTable detailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, ALedgerNumber);

            AControl.InitialiseUserControl(detailTable,
                AMotivationDetailTable.GetMotivationDetailCodeDBName(),
                AMotivationDetailTable.GetMotivationDetailDescDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 200 }, -1);

            if (AActiveOnly)
            {
                AControl.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true";
            }
            else
            {
                AControl.Filter = string.Empty;
            }
        }

        /// <summary>
        /// change the filter of the motivation detail combobox when a different motivation group gets selected
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AMotivationGroup"></param>
        public static void ChangeFilterMotivationDetailList(ref TCmbAutoPopulated AControl, String AMotivationGroup)
        {
            string newFilter = String.Empty;

            if ((AControl.Filter != null) && AControl.Filter.StartsWith(AMotivationDetailTable.GetMotivationStatusDBName() + " = true"))
            {
                newFilter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true And ";
            }

            newFilter += AMotivationDetailTable.GetMotivationGroupCodeDBName() + " = '" + AMotivationGroup + "'";

            AControl.Filter = newFilter;
        }

        /// <summary>
        /// fill combobox values with method of giving list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AActiveOnly"></param>
        public static void InitialiseMethodOfGivingCodeList(ref TCmbAutoPopulated AControl,
            bool AActiveOnly)
        {
            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MethodOfGivingList);

            AControl.InitialiseUserControl(Table,
                AMethodOfGivingTable.GetMethodOfGivingCodeDBName(),
                AMethodOfGivingTable.GetMethodOfGivingDescDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 150 }, -1);

            if (AActiveOnly)
            {
                AControl.Filter = AMethodOfGivingTable.GetActiveDBName() + " = true";
            }
            else
            {
                AControl.Filter = "";
            }
        }

        /// <summary>
        /// fill combobox values with method of payment list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AActiveOnly"></param>
        public static void InitialiseMethodOfPaymentCodeList(ref TCmbAutoPopulated AControl,
            bool AActiveOnly)
        {
            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MethodOfPaymentList);

            AControl.InitialiseUserControl(Table,
                AMethodOfPaymentTable.GetMethodOfPaymentCodeDBName(),
                AMethodOfPaymentTable.GetMethodOfPaymentCodeDBName(),
                AMethodOfPaymentTable.GetMethodOfPaymentDescDBName(),
                null,
                AMethodOfPaymentTable.GetActiveDBName()
                );
            AControl.AppearanceSetup(new int[] { -1, 150 }, -1);

            if (AActiveOnly)
            {
                AControl.Filter = AMethodOfPaymentTable.GetActiveDBName() + " = true";
            }
            else
            {
                AControl.Filter = "";
            }
        }

        /// <summary>
        /// fill combobox values with the mailing codes
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AActiveOnly"></param>
        public static void InitialisePMailingList(ref TCmbAutoPopulated AControl,
            bool AActiveOnly)
        {
            DataTable Table = TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.MailingList);

            // We need to add a row to the table that has a NULL value
            // We don't want to change the 'real' data table so we make a copy first
            Table = Table.Copy();

            // Allow NULL for the code in this table
            Table.Columns[PMailingTable.ColumnMailingCodeId].AllowDBNull = true;

            // Now add the row
            DataRow Dr = Table.NewRow();
            Dr[PMailingTable.GetMailingCodeDBName()] = DBNull.Value;
            Dr[PMailingTable.GetMailingDescriptionDBName()] = String.Empty;

            Table.Rows.InsertAt(Dr, 0);

            // Now use this table for the ComboBox data source
            AControl.InitialiseUserControl(Table,
                PMailingTable.GetMailingCodeDBName(),
                PMailingTable.GetMailingDescriptionDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 150 }, -1);

            if (AActiveOnly)
            {
                AControl.Filter = String.Format("({0}=true) OR ({1} IS NULL)", PMailingTable.GetViewableDBName(), PMailingTable.GetMailingCodeDBName());
                //TODO Add viewable until and date comparison
            }
            else
            {
                AControl.Filter = "";
            }
        }

        /// <summary>
        /// This function fills the available account hierarchies of a given ledger into a combobox
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNr"></param>
        public static void InitialiseAccountHierarchyList(ref TCmbAutoPopulated AControl, System.Int32 ALedgerNr)
        {
            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountHierarchyList, ALedgerNr);

            AControl.InitialiseUserControl(Table,
                AAccountHierarchyTable.GetAccountHierarchyCodeDBName(),
                null,
                null);
            AControl.AppearanceSetup(new int[] { 150 }, -1);

            AControl.Filter = AAccountHierarchyTable.GetLedgerNumberDBName() + " = " + ALedgerNr.ToString();
        }

        static PUnitTable FKeyMinTable = null;
        static Int64 FFieldNumber = -1;

        /// <summary>
        /// Field number of key ministry gift
        /// </summary>
        public static long FieldNumber {
            get
            {
                return FFieldNumber;
            }
        }

        /// <summary>
        /// This function fills the combobox for the key ministry depending on the partnerkey
        /// </summary>
        /// <param name="ACmbMinistry"></param>
        /// <param name="ATxtField"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ARefreshData"></param>
        public static void GetRecipientData(ref TCmbAutoPopulated ACmbMinistry,
            ref TtxtAutoPopulatedButtonLabel ATxtField,
            System.Int64 APartnerKey,
            Boolean ARefreshData = false)
        {
            GetRecipientData(ref ACmbMinistry, APartnerKey, out FFieldNumber, ARefreshData);

            if (Convert.ToInt64(ATxtField.Text) != FFieldNumber)
            {
                ATxtField.Text = FFieldNumber.ToString();
            }
        }

        /// <summary>
        /// This function fills the combobox for the key ministry depending on the partnerkey
        /// </summary>
        /// <param name="cmbMinistry"></param>
        /// <param name="APartnerKey"></param>
        public static void GetRecipientData(ref TCmbAutoPopulated cmbMinistry, System.Int64 APartnerKey)
        {
            GetRecipientData(ref cmbMinistry, APartnerKey, out FFieldNumber);
        }

        /// <summary>
        /// This function fills the combobox for the key ministry depending on the partnerkey
        /// </summary>
        /// <param name="ACmbKeyMinistry"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AFieldNumber"></param>
        /// <param name="ARefreshData"></param>
        private static void GetRecipientData(ref TCmbAutoPopulated ACmbKeyMinistry,
            Int64 APartnerKey,
            out Int64 AFieldNumber,
            Boolean ARefreshData = false)
        {
            string CurrentRowFilter = string.Empty;

            AFieldNumber = 0;

            if ((FKeyMinTable != null) && !ARefreshData)
            {
                if (FindAndSelect(ref ACmbKeyMinistry, APartnerKey))
                {
                    return;
                }
            }
            else if ((FKeyMinTable != null) && ARefreshData)
            {
                FKeyMinTable.Clear();
                FKeyMinTable = null;
            }

            string DisplayMember = PUnitTable.GetUnitNameDBName();
            string ValueMember = PUnitTable.GetPartnerKeyDBName();

            try
            {
                FKeyMinTable = TRemote.MFinance.Gift.WebConnectors.LoadKeyMinistry(APartnerKey, out FFieldNumber);

                AFieldNumber = FFieldNumber;

                CurrentRowFilter = FKeyMinTable.DefaultView.RowFilter;
                FKeyMinTable.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PUnitTable.GetUnitTypeCodeDBName(),
                    MPartnerConstants.UNIT_TYPE_KEYMIN);

                FKeyMinTable.DefaultView.Sort = DisplayMember + " Desc";

                DataTable dt = FKeyMinTable.DefaultView.ToTable();

                ACmbKeyMinistry.InitialiseUserControl(dt,
                    ValueMember,
                    DisplayMember,
                    DisplayMember,
                    null);
                ACmbKeyMinistry.AppearanceSetup(new int[] { 250 }, -1);

                if (!FindAndSelect(ref ACmbKeyMinistry, APartnerKey))
                {
                    //Clear the combobox
                    ACmbKeyMinistry.SelectedIndex = -1;
                }
            }
            finally
            {
                FKeyMinTable.DefaultView.RowFilter = CurrentRowFilter;
            }
        }

        static bool FindAndSelect(ref TCmbAutoPopulated AControl, System.Int64 APartnerKey)
        {
            foreach (PUnitRow pr in FKeyMinTable.Rows)
            {
                if (pr.PartnerKey == APartnerKey)
                {
                    AControl.SetSelectedInt64(APartnerKey);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function fills the available financial years of a given ledger into a TCmbAutoPopulated combobox
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNr"></param>
        public static void InitialiseAvailableGiftYearsList(ref TCmbAutoPopulated AControl, System.Int32 ALedgerNr)
        {
            string DisplayMember;
            string ValueMember;
            DataTable Table = TRemote.MFinance.Gift.WebConnectors.GetAvailableGiftYears(ALedgerNr, out DisplayMember, out ValueMember);

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.InitialiseUserControl(Table,
                ValueMember,
                DisplayMember,
                null,
                null);

            AControl.SelectedIndex = 0;

            AControl.AppearanceSetup(new int[] { -1 }, -1);
        }

        /// <summary>
        /// This function fills the available financial years of a given ledger into a TCmbAutoComplete combobox
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNr"></param>
        public static void InitialiseAvailableGiftYearsList(ref TCmbAutoComplete AControl, System.Int32 ALedgerNr)
        {
            string DisplayMember;
            string ValueMember;

            DataTable Table = TRemote.MFinance.Gift.WebConnectors.GetAvailableGiftYears(ALedgerNr, out DisplayMember, out ValueMember);

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.DisplayMember = DisplayMember;
            AControl.ValueMember = ValueMember;
            AControl.DataSource = Table.DefaultView;

            if (Table.DefaultView.Count > 0)
            {
                AControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This function fills the available financial end-of-years of a given ledger into a TCmbAutoPopulated combobox
        /// </summary>
        public static void InitialiseAvailableEndOfYearsList(ref TCmbAutoPopulated AControl,
            System.Int32 ALedgerNr,
            bool AIncludeNextYear = false)
        {
            string DisplayMember;
            string ValueMember;
            DataTable Table = TRemote.MFinance.GL.WebConnectors.GetAvailableGLYears(ALedgerNr,
                0,
                AIncludeNextYear,
                out DisplayMember,
                out ValueMember);

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.InitialiseUserControl(Table,
                ValueMember,
                "YearEnd",
                null,
                null);

            AControl.SelectedIndex = 0;

            AControl.AppearanceSetup(new int[] { 120 }, -1);
        }

        /// <summary>
        /// This function fills the available financial years of a given ledger into a TCmbAutoPopulated combobox
        /// </summary>
        public static void InitialiseAvailableFinancialYearsList(ref TCmbAutoPopulated AControl,
            System.Int32 ALedgerNr,
            bool AIncludeNextYear = false)
        {
            string DisplayMember;
            string ValueMember;
            DataTable Table = TRemote.MFinance.GL.WebConnectors.GetAvailableGLYears(ALedgerNr,
                0,
                AIncludeNextYear,
                out DisplayMember,
                out ValueMember);

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.InitialiseUserControl(Table,
                ValueMember,
                DisplayMember,
                null,
                null);

            AControl.SelectedIndex = 0;

            AControl.AppearanceSetup(new int[] { -1 }, -1);
        }

        /// <summary>
        /// This function fills the available financial years of a given ledger into a TCmbAutoPopulated combobox
        /// </summary>
        public static void InitialiseAvailableFinancialYearsListHOSA(ref TCmbAutoPopulated AControl,
            System.Int32 ALedgerNr,
            bool AIncludeNextYear = false)
        {
            string DisplayMember;
            string ValueMember;
            string DescriptionMember;

            DataTable Table = TRemote.MFinance.GL.WebConnectors.GetAvailableGLYearsHOSA(ALedgerNr,
                out DisplayMember,
                out ValueMember,
                out DescriptionMember);

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.InitialiseUserControl(Table,
                ValueMember,
                DisplayMember,
                DescriptionMember,
                null);

            AControl.AppearanceSetup(new int[] { 100, 150 }, -1);

            if (Table.DefaultView.Count > 0)
            {
                AControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This function fills the available financial years of a given ledger into a TCmbAutoComplete combobox
        /// </summary>
        public static void InitialiseAvailableFinancialYearsList(ref TCmbAutoComplete AControl,
            System.Int32 ALedgerNr,
            bool AIncludeNextYear = false, bool AShowYearEndings = false)
        {
            string DisplayMember;
            string ValueMember;
            DataTable Table;

            if (AShowYearEndings)
            {
                Table = TRemote.MFinance.GL.WebConnectors.GetAvailableGLYearEnds(ALedgerNr,
                    0,
                    AIncludeNextYear,
                    out DisplayMember,
                    out ValueMember);
            }
            else
            {
                Table = TRemote.MFinance.GL.WebConnectors.GetAvailableGLYears(ALedgerNr,
                    0,
                    AIncludeNextYear,
                    out DisplayMember,
                    out ValueMember);
            }

            Table.DefaultView.Sort = ValueMember + " DESC";

            AControl.DisplayMember = DisplayMember;
            AControl.ValueMember = ValueMember;
            AControl.DataSource = Table.DefaultView;

            if (Table.DefaultView.Count > 0)
            {
                AControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This function fills the available financial periods of a given ledger and financial year into a TCmbAutoComplete combobox
        /// </summary>
        public static void InitialiseAvailableFinancialPeriodsList(
            ref TCmbAutoComplete AControl,
            System.Int32 ALedgerNr,
            System.Int32 AYear,
            System.Int32 AInitialSelectedIndex,
            Boolean AShowCurrentAndForwarding)
        {
            DataTable periods = InitialiseAvailableFinancialPeriodsList(ALedgerNr, AYear, AShowCurrentAndForwarding);

            AControl.DisplayMember = "display";
            AControl.ValueMember = "value";
            AControl.SelectedIndexOnDataSourceChange = AInitialSelectedIndex;
            AControl.DataSource = periods.DefaultView;
        }

        /// <summary>
        /// This function fills the available financial periods of a given ledger and financial year into a TCmbAutoPopulated combobox
        /// </summary>
        public static void InitialiseAvailableFinancialPeriodsList(
            ref TCmbAutoPopulated AControl,
            System.Int32 ALedgerNr,
            System.Int32 AYear,
            System.Int32 AInitialSelectedIndex,
            Boolean AShowCurrentAndForwarding,
            Boolean AShowZero = true)
        {
            DataTable periods = InitialiseAvailableFinancialPeriodsList(ALedgerNr, AYear, AShowCurrentAndForwarding, AShowZero);

            AControl.InitialiseUserControl(periods, "value", "display", "descr", null, null);

            if (AShowCurrentAndForwarding)
            {
                AControl.AppearanceSetup(new int[] { AControl.ComboBoxWidth }, -1);
            }
            else
            {
                AControl.AppearanceSetup(new int[] { -1, 200 }, -1);
            }

            if (AInitialSelectedIndex >= 0)
            {
                AControl.SelectedIndex = AInitialSelectedIndex;
            }
        }

        /// <summary>
        /// This function fills a DataTable with the available financial periods of a given ledger and financial year
        /// </summary>
        public static DataTable InitialiseAvailableFinancialPeriodsList(
            System.Int32 ALedgerNr,
            System.Int32 AYear,
            Boolean AShowCurrentAndForwarding,
            Boolean AShowZero = true)
        {
            DataTable AccountingPeriods = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList, ALedgerNr);

            AccountingPeriods.DefaultView.Sort = AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " ASC";

            ALedgerRow Ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNr))[0];

            string DisplayMember = "display";
            string ValueMember = "value";
            string DescrMember = "descr";
            DataTable periods = new DataTable();
            periods.Columns.Add(new DataColumn(ValueMember, typeof(Int32)));
            periods.Columns.Add(new DataColumn(DisplayMember, typeof(string)));
            periods.Columns.Add(new DataColumn(DescrMember, typeof(string)));

            DataRow period;

            if (AShowZero)
            {
                period = periods.NewRow();
                period[ValueMember] = 0;
                period[DisplayMember] = "All";
                period[DescrMember] = Catalog.GetString("(All periods)");
                periods.Rows.Add(period);
            }

            if (Ledger.CurrentFinancialYear == AYear)
            {
                if (AShowCurrentAndForwarding)
                {
                    period = periods.NewRow();
                    period[ValueMember] = -1;
                    period[DisplayMember] = "Current";
                    period[DescrMember] = Catalog.GetString("(Current and forwarding periods)");
                    periods.Rows.Add(period);
                }

                for (int periodCounter = 1; periodCounter <= Ledger.CurrentPeriod + Ledger.NumberFwdPostingPeriods; periodCounter++)
                {
                    period = periods.NewRow();
                    period[ValueMember] = periodCounter;
                    period[DisplayMember] = periodCounter.ToString("00");
                    period[DescrMember] = ((AAccountingPeriodRow)AccountingPeriods.DefaultView[periodCounter - 1].Row).AccountingPeriodDesc;
                    periods.Rows.Add(period);
                }
            }
            else
            {
                for (int periodCounter = 1; periodCounter <= Ledger.NumberOfAccountingPeriods; periodCounter++)
                {
                    period = periods.NewRow();
                    period[ValueMember] = periodCounter;
                    period[DisplayMember] = "(" + periodCounter.ToString("00") + ") ";
                    period[DescrMember] = ((AAccountingPeriodRow)AccountingPeriods.DefaultView[periodCounter - 1].Row).AccountingPeriodDesc;
                    periods.Rows.Add(period);
                }
            }

            periods.DefaultView.Sort = ValueMember + " ASC";
            return periods;
        }

        /// <summary>
        /// This function puts the ICH numbers used of a given ledger into a combobox
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AYearStart"></param>
        /// <param name="AYearEnding"></param>
        public static void InitialiseICHStewardshipList(
            ref TCmbAutoPopulated AControl,
            Int32 ALedgerNumber,
            Int32 APeriodNumber,
            String AYearStart,
            String AYearEnding)
        {
            DataTable ICHNumbers = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.ICHStewardshipList, ALedgerNumber);

            if (AYearStart != null)
            {
                //Filter for current period and date range
                ICHNumbers.DefaultView.RowFilter = String.Format("{0}={1} And {2}>'{3}' And {2}<='{4}'",
                    AIchStewardshipTable.GetPeriodNumberDBName(),
                    APeriodNumber,
                    AIchStewardshipTable.GetDateProcessedDBName(),
                    AYearStart,
                    AYearEnding);
            }
            else
            {
                //Filter for current period
                ICHNumbers.DefaultView.RowFilter = String.Format("{0}={1}",
                    AIchStewardshipTable.GetPeriodNumberDBName(),
                    APeriodNumber);
            }

            ICHNumbers.DefaultView.Sort = AIchStewardshipTable.GetIchNumberDBName();

            //Get the distinct ICH numbers for the specified period
            DataTable newDataTable = ICHNumbers.DefaultView.ToTable(true, AIchStewardshipTable.GetIchNumberDBName(),
                AIchStewardshipTable.GetDateProcessedDBName());

            // add empty row so that SetSelectedString for invalid string will not result in undefined behaviour
            DataRow emptyRow = newDataTable.NewRow();

            emptyRow[0] = 0;  //selecting 0 will mean full HOSA reports for all cost centres
            emptyRow[1] = DateTime.Today;

            newDataTable.Rows.Add(emptyRow);

            AControl.InitialiseUserControl(newDataTable,
                AIchStewardshipTable.GetIchNumberDBName(),
                AIchStewardshipTable.GetDateProcessedDBName(),
                null);
            AControl.AppearanceSetup(new int[] { -1, 150 }, -1);

            //Alternative way to filter the contents of the combo
            //AControl.Filter = AIchStewardshipTable.GetPeriodNumberDBName() + " = " + APeriodNumber.ToString();
        }

        /// <summary>
        /// This function fills the open financial periods of a given ledger into a combobox
        /// </summary>
        public static void InitialiseOpenFinancialPeriodsList(
            ref TCmbAutoPopulated AControl,
            System.Int32 ALedgerNumber)
        {
            DataTable AccountingPeriods = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                ALedgerNumber);

            ALedgerRow Ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            int CurrentPeriod = Ledger.CurrentPeriod;
            int EndPeriod = CurrentPeriod + Ledger.NumberFwdPostingPeriods;

            AControl.InitialiseUserControl(AccountingPeriods,
                AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                AAccountingPeriodTable.GetAccountingPeriodDescDBName(),
                null);

            AControl.AppearanceSetup(new int[] { -1, 200 }, -1);

            AControl.Filter = String.Format("{0} >= {1} And {0} <= {2}",
                AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                CurrentPeriod,
                EndPeriod);
        }

        /// <summary>
        /// return the ledger number and name for readonly text boxes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static string GetLedgerNumberAndName(Int32 ALedgerNumber)
        {
            if (ALedgerNumber <= 0)
            {
                return "None";
            }

            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);

            foreach (DataRow row in Table.Rows)
            {
                if (row["LedgerNumber"].ToString() == ALedgerNumber.ToString())
                {
                    return ALedgerNumber.ToString() + " " + row["LedgerName"];
                }
            }

            TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList); // perhaps the user will fix the problem...
            return "ledger " + ALedgerNumber.ToString();
        }

        /// <summary>
        /// return the ledger's current financial year
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static Int32 GetLedgerCurrentFinancialYear(Int32 ALedgerNumber)
        {
            ALedgerRow row =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            return row.CurrentFinancialYear;
        }

        /// <summary>
        /// return the ledger's number of periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static Int32 GetLedgerNumPeriods(Int32 ALedgerNumber)
        {
            ALedgerRow row =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            return row.NumberOfAccountingPeriods;
        }

        /// <summary>
        /// fill checkedlistbox values with fees payable list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        public static void InitialiseFeesPayableList(ref TClbVersatile AControl,
            Int32 ALedgerNumber)
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = AFeesPayableTable.GetFeeDescriptionDBName();
            string ValueMember = AFeesPayableTable.GetFeeCodeDBName();

            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.FeesPayableList, ALedgerNumber);
            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });

            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            // this unseen column is used to order the table
            //(sorting using the 'CHECKED' column causes problems as sorting takes place as soon as a record is checked)
            NewTable.Columns.Add(new DataColumn("ORDER", typeof(bool)));

            AControl.Columns.Clear();
            AControl.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            AControl.AddTextColumn(Catalog.GetString("Code"), NewTable.Columns[ValueMember], 100);
            AControl.AddTextColumn(Catalog.GetString("Cost Centre Description"), NewTable.Columns[DisplayMember], 228);
            AControl.DataBindGrid(NewTable, "ORDER DESC, " + ValueMember, CheckedMember, ValueMember, false, true, false);
        }

        /// <summary>
        /// fill checkedlistbox values with fees receivable list
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="ALedgerNumber"></param>
        public static void InitialiseFeesReceivableList(ref TClbVersatile AControl,
            Int32 ALedgerNumber)
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = AFeesReceivableTable.GetFeeDescriptionDBName();
            string ValueMember = AFeesReceivableTable.GetFeeCodeDBName();

            DataTable Table = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.FeesReceivableList, ALedgerNumber);
            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });

            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            // this unseen column is used to order the table
            //(sorting using the 'CHECKED' column causes problems as sorting takes place as soon as a record is checked)
            NewTable.Columns.Add(new DataColumn("ORDER", typeof(bool)));

            AControl.Columns.Clear();
            AControl.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            AControl.AddTextColumn(Catalog.GetString("Code"), NewTable.Columns[ValueMember], 100);
            AControl.AddTextColumn(Catalog.GetString("Cost Centre Description"), NewTable.Columns[DisplayMember], 228);
            AControl.DataBindGrid(NewTable, "ORDER DESC, " + ValueMember, CheckedMember, ValueMember, false, true, false);
        }
    }
}