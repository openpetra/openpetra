// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2014 by OM International
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using GNU.Gettext;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_AccountsList
    {
        private TFrmGLAccountHierarchy FParentForm = null;
        private TUC_AccountsListFilterFind FFilterFindLogicObject = null;

        private TSgrdDataGrid grdDetails = null;
        private int FPrevRowChangedRow = -1;
        private DataRow FPreviouslySelectedDetailRow = null;
        private bool FSelectionMadeInList = false;

        // The account selected in the parent form
        AccountNodeDetails FSelectedAccount;
//        Int32 FLedgerNumber;
//        String FSelectedHierarchy;
        DataView FDataView = null;

        /// <summary>
        /// I don't want this, but the auto-generated code references it:
        /// </summary>
        public GLSetupTDS MainDS;

        /// <summary>
        /// The Account may have been selected in the tree view, and copied here.
        /// </summary>
        public AccountNodeDetails SelectedAccount
        {
            set
            {
                FSelectedAccount = value;

                // FSelectionMadeInList will be tree if the change was made in the list view.
                // We only need to run this code if selection was made in tree view
                if ((FDataView != null) && !FSelectionMadeInList)
                {
                    Int32 RowIdx = -1;

                    if (FSelectedAccount != null)
                    {
                        // The grid could be sorted by any column. We need to make sure that the grid is sorted 1st or 2nd by a_account_code_c.
                        // Otherwise we may not be able to find the account and retrieve the accurate row ID.
                        if (FDataView.Sort.Contains("a_account_code_short_desc_c"))
                        {
                            if (FDataView.Sort.Contains("DESC"))
                            {
                                FDataView.Sort = "a_account_code_short_desc_c DESC, a_account_code_c";
                            }
                            else
                            {
                                FDataView.Sort = "a_account_code_short_desc_c, a_account_code_c";
                            }

                            RowIdx = FDataView.Find(new object[] { FSelectedAccount.AccountRow.AccountCodeShortDesc,
                                                                   FSelectedAccount.AccountRow.AccountCode }) + 1;
                        }
                        else if (FDataView.Sort.Contains("a_ytd_actual_base_n"))
                        {
                            if (FDataView.Sort.Contains("DESC"))
                            {
                                FDataView.Sort = "a_ytd_actual_base_n DESC, a_account_code_c";
                            }
                            else
                            {
                                FDataView.Sort = "a_ytd_actual_base_n, a_account_code_c";
                            }

                            // YTD Actual might be DBNull
                            if (FSelectedAccount.AccountRow.IsYtdActualBaseNull())
                            {
                                RowIdx = FDataView.Find(new object[] { null, FSelectedAccount.AccountRow.AccountCode }) + 1;
                            }
                            else
                            {
                                RowIdx = FDataView.Find(new object[] { FSelectedAccount.AccountRow.YtdActualBase,
                                                                       FSelectedAccount.AccountRow.AccountCode }) + 1;
                            }
                        }
                        else if (FDataView.Sort.Contains("a_ytd_actual_foreign_n"))
                        {
                            if (FDataView.Sort.Contains("DESC"))
                            {
                                FDataView.Sort = "a_ytd_actual_foreign_n DESC, a_account_code_c";
                            }
                            else
                            {
                                FDataView.Sort = "a_ytd_actual_foreign_n, a_account_code_c";
                            }

                            // YTD Actual might be DBNull
                            if (FSelectedAccount.AccountRow.IsYtdActualForeignNull())
                            {
                                RowIdx = FDataView.Find(new object[] { null, FSelectedAccount.AccountRow.AccountCode }) + 1;
                            }
                            else
                            {
                                RowIdx = FDataView.Find(new object[] { FSelectedAccount.AccountRow.YtdActualForeign,
                                                                       FSelectedAccount.AccountRow.AccountCode }) + 1;
                            }
                        }
                        else
                        {
                            RowIdx = FDataView.Find(new object[] { FSelectedAccount.AccountRow.AccountCode }) + 1;
                        }
                    }

                    FParentForm.FIAmUpdating++;
                    grdAccounts.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(grdAccounts_FocusRowLeaving);
                    grdAccounts.SelectRowInGrid(RowIdx);
                    grdAccounts.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdAccounts_FocusRowLeaving);
                    FParentForm.FIAmUpdating--;
                }
            }
        }

        private void InitializeManualCode()
        {
            // The auto-generated code requires that the grid be named grdDetails (for filter/find), but that doesn't work for another part of the autogenerated code!
            // So we make grdDetails reference grdAccounts here at initialization
            grdDetails = grdAccounts;
        }

        /// <summary>
        /// Perform initialisation
        /// (Actually called earlier than the parent RunOnceOnActivationManual)
        /// </summary>
        public void RunOnceOnActivationManual(TFrmGLAccountHierarchy ParentForm)
        {
            FParentForm = ParentForm;
            grdAccounts.Selection.SelectionChanged += Selection_SelectionChanged;
            grdAccounts.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdAccounts_FocusRowLeaving);
        }

        private void grdAccounts_FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
        {
            if (!FParentForm.CheckControlsValidateOk())
            {
                e.Cancel = true;
            }
        }

        void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (FParentForm.FIAmUpdating == 0)
            {
                int previousRowId = FPrevRowChangedRow;
                int newRowId = grdAccounts.Selection.ActivePosition.Row;
                DataRowView rowView = (DataRowView)grdAccounts.Rows.IndexToDataSourceRow(newRowId);

                if (rowView == null)
                {
                    FPreviouslySelectedDetailRow = null;
                    FParentForm.SetSelectedAccount(null);
                    FParentForm.PopulateControlsAfterRowSelection();
                    Console.WriteLine("Selected row is NULL");
                }
                else
                {
                    FPreviouslySelectedDetailRow = rowView.Row;
                    String SelectedAccountCode = ((GLSetupTDSAAccountRow)rowView.Row).AccountCode;

                    FSelectionMadeInList = true;
                    FParentForm.SetSelectedAccountCode(SelectedAccountCode);
                    FSelectionMadeInList = false;

                    if (previousRowId == -1)
                    {
                        FParentForm.PopulateControlsAfterRowSelection();
                    }

                    Console.WriteLine("Row is {0}", FPreviouslySelectedDetailRow.ItemArray[1]);
                }

                FPrevRowChangedRow = newRowId;
            }
            else
            {
                Console.WriteLine("Skipping selection_changed...");
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Show all the data (Account Code and description)
        /// </summary>
        public void PopulateListView(GLSetupTDS MainDS, Int32 LedgerNumber, String SelectedHierarchy)
        {
//            FLedgerNumber = LedgerNumber;
//            FSelectedHierarchy = SelectedHierarchy;

            FDataView = new DataView(MainDS.AAccount);
            FDataView.Sort = "a_account_code_c";
            FDataView.AllowNew = false;
            grdAccounts.DataSource = new DevAge.ComponentModel.BoundDataView(FDataView);

            grdAccounts.Columns.Clear();
            grdAccounts.AddTextColumn(Catalog.GetString("Code"), MainDS.AAccount.ColumnAccountCode);
            grdAccounts.AddTextColumn(Catalog.GetString("Descr"), MainDS.AAccount.ColumnAccountCodeShortDesc);
//          grdAccounts.AddCurrencyColumn(Catalog.GetString("YTD Actual"), MainDS.AAccount.ColumnYtdActualBase);
//          grdAccounts.AddCurrencyColumn(Catalog.GetString("Foreign"), MainDS.AAccount.ColumnYtdActualForeign);

            if (FSelectedAccount != null)
            {
                this.SelectedAccount = FSelectedAccount;
            }
            else
            {
                grdAccounts.SelectRowInGrid(0);
            }
        }

        /// <summary>
        /// Method to collapse the filter panel if it is open
        /// </summary>
        public void CollapseFilterFind()
        {
            if (pnlFilterAndFind.Width > 0)
            {
                // Get the current row
                DataRow currentRow = FPreviouslySelectedDetailRow;

                FFilterAndFindObject.ToggleFilter();

                if (currentRow != null)
                {
                    FParentForm.SetSelectedAccountCode(currentRow.ItemArray[1].ToString());
                }
            }
        }

        private void CreateFilterFindPanelsManual()
        {
            // remove this control if suspense accounts are not allowed for this ledger
            if (!FParentForm.SuspenseAccountsAllowed)
            {
                CheckBox chkSuspenseAccount = new CheckBox();
                chkSuspenseAccount.Name = "chkSuspenseAccount";

                FFilterAndFindObject.FilterPanelControls.FStandardFilterPanels.Remove(
                    FFilterAndFindObject.FilterPanelControls.FindPanelByClonedFrom(chkSuspenseAccount));
            }
        }

        private void FilterToggledManual(bool AFilterPanelIsCollapsed)
        {
            if ((FFilterFindLogicObject == null) && !AFilterPanelIsCollapsed)
            {
                FFilterFindLogicObject = new TUC_AccountsListFilterFind(FFilterAndFindObject, FParentForm.SuspenseAccountsAllowed);
            }
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            FFilterFindLogicObject.ApplyFilterManual(ref AFilterString, MainDS.AAccount);
        }

        private bool IsMatchingRowManual(DataRow ARow)
        {
            return FFilterFindLogicObject.IsMatchingRowManual(ARow);
        }

        /// <summary>
        /// Interface method
        /// </summary>
        public void SelectRowInGrid(int ARowToSelect)
        {
            grdDetails.SelectRowInGrid(ARowToSelect, true);
        }
    }
}