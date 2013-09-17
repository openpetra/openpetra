//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerInterests
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
        }

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            LoadDataOnDemand();

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Category", FMainDS.PPartnerInterest.ColumnInterestCategory);
            grdDetails.AddTextColumn("Interest", FMainDS.PPartnerInterest.ColumnInterest);
            grdDetails.AddTextColumn("Country", FMainDS.PPartnerInterest.ColumnCountry);
            grdDetails.AddPartnerKeyColumn("Field", FMainDS.PPartnerInterest.ColumnFieldKey);
            grdDetails.AddTextColumn("Level", FMainDS.PPartnerInterest.ColumnLevel);
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerInterest.ColumnComment);

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpInterests));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
                ShowDetails(1); // do this as for some reason details are not automatically show here at the moment
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(PartnerEditTDSPPartnerInterestTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PartnerEditTDSPPartnerInterestTable());
            }

            FMainDS.InitVars();
        }

        /// <summary>
        /// Loads Partner Interest Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PPartnerInterest == null)
                {
                    FMainDS.Tables.Add(new PartnerEditTDSPPartnerInterestTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPartnerInterests());

                    // Make DataRows unchanged
                    if (FMainDS.PPartnerInterest.Rows.Count > 0)
                    {
                        FMainDS.PPartnerInterest.AcceptChanges();
                    }
                }

                if (FMainDS.PPartnerInterest.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerInterestRow ARow)
        {
        }

        private void GetDetailDataFromControlsManual(PPartnerInterestRow ARow)
        {
            if (ARow.RowState != DataRowState.Deleted)
            {
                if (!ARow.IsFieldKeyNull())
                {
                    if (ARow.FieldKey == 0)
                    {
                       ARow.SetFieldKeyNull();
                    }
                }
            }
        }

        private void FilterInterestCombo(object sender, EventArgs e)
        {
            PInterestCategoryTable CategoryTable;
            PInterestCategoryRow CategoryRow;
            string SelectedCategory = cmbPPartnerInterestInterestCategory.GetSelectedString();
            string SelectedInterest = cmbPPartnerInterestInterest.GetSelectedString();

            cmbPPartnerInterestInterest.Filter = PInterestTable.GetCategoryDBName() + " = '" + SelectedCategory + "'";

            // reset text to previous value or (if not found) empty text field
            if (cmbPPartnerInterestInterest.GetSelectedString() != String.Empty)
            {
                if (!cmbPPartnerInterestInterest.SetSelectedString(SelectedInterest))
                {
                    cmbPPartnerInterestInterest.SetSelectedString("", -1);
                }
            }

            CategoryTable = (PInterestCategoryTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestCategoryList);
            CategoryRow = (PInterestCategoryRow)CategoryTable.Rows.Find(new object[] { SelectedCategory });

            if ((CategoryRow != null)
                && !CategoryRow.IsLevelRangeLowNull()
                && !CategoryRow.IsLevelRangeHighNull())
            {
                if (CategoryRow.LevelRangeLow == CategoryRow.LevelRangeHigh)
                {
                    lblInterestLevelExplanation.Text = String.Format(Catalog.GetString("(only level {0} is available for category {1})"),
                        CategoryRow.LevelRangeLow, CategoryRow.Category);
                }
                else
                {
                    lblInterestLevelExplanation.Text = String.Format(Catalog.GetString("(from {0} to {1})"),
                        CategoryRow.LevelRangeLow, CategoryRow.LevelRangeHigh);
                }
            }
            else
            {
                lblInterestLevelExplanation.Text = "";
            }
        }

        /// <summary>
        /// adding a new partner relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            if (CreateNewPPartnerInterest())
            {
                cmbPPartnerInterestInterestCategory.Focus();
            }

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PartnerEditTDSPPartnerInterestRow ARow)
        {
            Int32 HighestNumber = 0;
            PPartnerInterestRow PartnerInterestRow;

            // find the highest number so far and increase it by 1 for the new key
            foreach (PPartnerInterestRow row in FMainDS.PPartnerInterest.Rows)
            {
                PartnerInterestRow = (PPartnerInterestRow)row;

                if (PartnerInterestRow.RowState != DataRowState.Deleted)
                {
                    if (PartnerInterestRow.InterestNumber > HighestNumber)
                    {
                        HighestNumber = PartnerInterestRow.InterestNumber;
                    }
                }
            }

            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            ARow.InterestNumber = HighestNumber + 1;
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PartnerEditTDSPPartnerInterestRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2},{0}{3} {4},{0}{5} {6},{0}{7} {8})",
                Environment.NewLine,
                lblPPartnerInterestInterestCategory.Text,
                cmbPPartnerInterestInterestCategory.GetSelectedString(),
                lblPPartnerInterestInterest.Text,
                cmbPPartnerInterestInterest.GetSelectedString(),
                lblPPartnerInterestCountry.Text,
                cmbPPartnerInterestCountry.GetSelectedString(),
                lblPPartnerInterestFieldKey.Text,
                txtPPartnerInterestFieldKey.Text);
            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PartnerEditTDSPPartnerInterestRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private void ValidateDataDetailsManual(PPartnerInterestRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidatePartnerInterestManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, cmbPPartnerInterestInterestCategory.GetSelectedString());
        }

        #endregion
    }
}