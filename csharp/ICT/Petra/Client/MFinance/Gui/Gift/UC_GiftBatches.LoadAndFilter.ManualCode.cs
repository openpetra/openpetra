//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;

using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles loading of batches and the display of the filter.
    /// Batches can be filtered by batch status, financial year and period within a year
    /// </summary>
    public class TUC_GiftBatches_LoadAndFilter
    {
        // Variables that are set by the constructor
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFilterAndFindPanel FFilterFindPanelObject = null;

        // Working variables for the controls on the filter panel
        private TCmbAutoComplete FcmbYearEnding = null;
        private TCmbAutoComplete FcmbPeriod = null;
        private RadioButton FrbtEditing = null;
        private RadioButton FrbtPosting = null;
        private RadioButton FrbtAll = null;

        // Table references that we need for drawing the inactive account codes
        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        // Other class variables for tracking the filter
        private Int32 FPrevYearEnding = -1;
        private string FPrevBaseFilter = String.Empty;
        private string FPrevFilter = String.Empty;

        #region Public Properties

        /// <summary>
        /// Get/set the SelectedIndex of the Year ComboBox
        /// </summary>
        public Int32 YearIndex
        {
            get
            {
                return FcmbYearEnding.SelectedIndex;
            }
            set
            {
                if (value < FcmbYearEnding.Items.Count)
                {
                    FcmbYearEnding.SelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// Get/set the SelectedIndex of the Period ComboBox
        /// </summary>
        public Int32 PeriodIndex
        {
            get
            {
                return FcmbPeriod.SelectedIndex;
            }
            set
            {
                if (value < FcmbPeriod.Items.Count)
                {
                    FcmbPeriod.SelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// Get/set the status filter to show 'all'
        /// </summary>
        public Boolean StatusAll
        {
            get
            {
                return FrbtAll.Checked;
            }
            set
            {
                FrbtAll.Checked = value;
            }
        }

        /// <summary>
        /// Get/set the status filter to show batches for 'editing'
        /// </summary>
        public Boolean StatusEditing
        {
            get
            {
                return FrbtEditing.Checked;
            }
            set
            {
                FrbtEditing.Checked = value;
            }
        }

        /// <summary>
        /// Get/set the status filter to show batches for 'posting'
        /// </summary>
        public Boolean StatusPosting
        {
            get
            {
                return FrbtPosting.Checked;
            }
            set
            {
                FrbtPosting.Checked = value;
            }
        }

        /// <summary>
        /// Set the CostCentre table that is used to draw active/inactive codes
        /// </summary>
        public ACostCentreTable CostCentreTable
        {
            set
            {
                FCostCentreTable = value;
            }
        }

        /// <summary>
        /// Set the AccountCode table that is used to draw active/inactive codes
        /// </summary>
        public AAccountTable AccountTable
        {
            set
            {
                FAccountTable = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="AMainDS">The main data set</param>
        /// <param name="AFilterFindPanelObject">The filter panel control object</param>
        public TUC_GiftBatches_LoadAndFilter(int ALedgerNumber, GiftBatchTDS AMainDS, TFilterAndFindPanel AFilterFindPanelObject)
        {
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FFilterFindPanelObject = AFilterFindPanelObject;

            FrbtEditing = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtEditing");
            FrbtPosting = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtPosting");
            FrbtAll = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtAll");
            FcmbYearEnding = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbYearEnding");
            FcmbPeriod = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbPeriod");

            FMainDS.AGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                AGiftBatchTable.GetLedgerNumberDBName(),
                AGiftBatchTable.GetBatchNumberDBName()
                );

            // Populate the Year ComboBox with available years for the specified ledger
            TFinanceControls.InitialiseAvailableGiftYearsList(ref FcmbYearEnding, FLedgerNumber);
            //TLogging.Log("Gift Years completed");

            // Ensure that we start with the status set to 'editing'.
            FrbtEditing.Checked = true;
            //TLogging.Log("Editing checkbox selected");
        }

        #endregion

        #region Public class methods

        /// <summary>
        /// This method must be called in order to correctly populate the ComboBoxes on the filter panel (by cloning from the main details panel)
        /// </summary>
        /// <param name="ACmbCostCentre"></param>
        /// <param name="ACmbAccountCode"></param>
        public void OnMainScreenActivation(TCmbAutoPopulated ACmbCostCentre, TCmbAutoPopulated ACmbAccountCode)
        {
            // We have to do these because the filter/find panel is displayed when the screen is loaded, so they do not get populated
            InitFilterFindComboBox(ACmbCostCentre,
                (TCmbAutoComplete)FFilterFindPanelObject.FilterPanelControls.FindControlByName(ACmbCostCentre.Name),
                TCacheableFinanceTablesEnum.CostCentreList);
            InitFilterFindComboBox(ACmbAccountCode,
                (TCmbAutoComplete)FFilterFindPanelObject.FilterPanelControls.FindControlByName(ACmbAccountCode.Name),
                TCacheableFinanceTablesEnum.AccountList);
            InitFilterFindComboBox(ACmbCostCentre,
                (TCmbAutoComplete)FFilterFindPanelObject.FindPanelControls.FindControlByName(ACmbCostCentre.Name),
                TCacheableFinanceTablesEnum.CostCentreList);
            InitFilterFindComboBox(ACmbAccountCode,
                (TCmbAutoComplete)FFilterFindPanelObject.FindPanelControls.FindControlByName(ACmbAccountCode.Name),
                TCacheableFinanceTablesEnum.AccountList);
        }

        /// <summary>
        /// Disable the Year and Period ComboBoxes.  Also optionally select a status of 'All' - or uncheck all status options
        /// </summary>
        public void DisableYearAndPeriod(Boolean AAndSelectAllBatches)
        {
            FcmbYearEnding.Enabled = false;
            FcmbPeriod.Enabled = false;

            if (AAndSelectAllBatches)
            {
                FrbtAll.Checked = true;
            }
            else
            {
                FrbtPosting.Checked = false;
                FrbtEditing.Checked = false;
                FrbtAll.Checked = false;
            }
        }

        /// <summary>
        /// The main method that handles all filtering.  Every change on the filter panel causes this event to fire.
        /// It is important to manage the fact that this method may be called recursively and so nesting can be tricky!
        /// </summary>
        /// <param name="AFilterString">On entry this parameter contains the filter control's best guess for the current filter.
        /// The code can modify this string in the light of current control values.</param>
        public void ApplyFilterManual(ref string AFilterString)
        {
            //int BatchNumber = 0;
            string workingFilter = String.Empty;
            string additionalFilter = String.Empty;
            bool showingAllPeriods = false;

            // Remove the old base filter
            if (FPrevBaseFilter.Length > 0)
            {
                // The additional filter is the part that is coming from the extra filter panel
                additionalFilter = AFilterString.Substring(FPrevBaseFilter.Length);

                if (additionalFilter.StartsWith(CommonJoinString.JOIN_STRING_SQL_AND))
                {
                    additionalFilter = additionalFilter.Substring(CommonJoinString.JOIN_STRING_SQL_AND.Length);
                }
            }

            int newYear = FcmbYearEnding.GetSelectedInt32();

            if (newYear != FPrevYearEnding)
            {
                FPrevYearEnding = newYear;

                // This will trigger a re-entrant call to this method
                RefreshPeriods(newYear);

                // Apply the last good filter as we unwind the nesting
                AFilterString = FPrevFilter;
                return;
            }

            int newPeriod = FcmbPeriod.GetSelectedInt32();

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            int CurrentLedgerYear = LedgerRow.CurrentFinancialYear;
            int CurrentLedgerPeriod = LedgerRow.CurrentPeriod;

            if (newYear == -1)
            {
                newYear = CurrentLedgerYear;

                workingFilter = String.Format("{0} = {1}", AGiftBatchTable.GetBatchYearDBName(), newYear);
                showingAllPeriods = true;
            }
            else
            {
                workingFilter = String.Format("{0} = {1}", AGiftBatchTable.GetBatchYearDBName(), newYear);

                if (newPeriod == 0)  //All periods for year
                {
                    //Nothing to add to filter
                    showingAllPeriods = true;
                }
                else if (newPeriod == -1)  //Current and forwarding
                {
                    workingFilter += String.Format(" AND {0} >= {1}", AGiftBatchTable.GetBatchPeriodDBName(), CurrentLedgerPeriod);
                }
                else if (newPeriod > 0)  //Specific period
                {
                    workingFilter += String.Format(" AND {0} = {1}", AGiftBatchTable.GetBatchPeriodDBName(), newPeriod);
                }
            }

            if (!BatchYearIsLoaded(newYear))
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, newYear, newPeriod));
            }

            if (FrbtEditing.Checked)
            {
                StringHelper.JoinAndAppend(ref workingFilter, String.Format("{0} = '{1}'",
                        AGiftBatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED),
                    CommonJoinString.JOIN_STRING_SQL_AND);
            }
            else if (FrbtPosting.Checked)
            {
                StringHelper.JoinAndAppend(ref workingFilter, String.Format("({0} = '{1}') AND ({2} <> 0) AND (({3} = 0) OR ({3} = {2}))",
                        AGiftBatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED,
                        AGiftBatchTable.GetBatchTotalDBName(),
                        AGiftBatchTable.GetHashTotalDBName()),
                    CommonJoinString.JOIN_STRING_SQL_AND);
            }
            else //(FrbtAll.Checked)
            {
            }

            FFilterFindPanelObject.FilterPanelControls.SetBaseFilter(workingFilter, FrbtAll.Checked && showingAllPeriods);
            FPrevBaseFilter = workingFilter;

            AFilterString = workingFilter;
            StringHelper.JoinAndAppend(ref AFilterString, additionalFilter, CommonJoinString.JOIN_STRING_SQL_AND);

            FPrevFilter = AFilterString;

            //TLogging.Log(String.Format("working filter: {0}", workingFilter));
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Helper method that we can call to initialise each of the filter/find comboBoxes
        /// </summary>
        private void InitFilterFindComboBox(TCmbAutoPopulated AClonedFromComboBox,
            TCmbAutoComplete AFFInstance,
            TCacheableFinanceTablesEnum AListTableEnum)
        {
            AFFInstance.DisplayMember = AClonedFromComboBox.DisplayMember;
            AFFInstance.ValueMember = AClonedFromComboBox.ValueMember;

            AFFInstance.DataSource = TDataCache.TMFinance.GetCacheableFinanceTable(AListTableEnum, FLedgerNumber).DefaultView;
            AFFInstance.DrawMode = DrawMode.OwnerDrawFixed;
            AFFInstance.DrawItem += new DrawItemEventHandler(DrawComboBoxItem);
        }

        /// <summary>
        /// Update the Period ComboBox after a change to the Year
        /// </summary>
        /// <param name="ASelectedYear">The year for which to display the periods</param>
        private void RefreshPeriods(Int32 ASelectedYear)
        {
            bool IncludeCurrentAndForwardingItem = true;

            //Determine whether or not to include the "Current and forwarding periods" item in the period combo
            if (FMainDS.ALedger.Rows.Count == 1)
            {
                IncludeCurrentAndForwardingItem = (ASelectedYear == FMainDS.ALedger[0].CurrentFinancialYear);
            }

            //Update the periods for the newly selected year
            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref FcmbPeriod, FLedgerNumber, ASelectedYear, 0, IncludeCurrentAndForwardingItem);
        }

        /// <summary>
        /// This method is called when the system wants to draw a comboBox item in the list.
        /// We choose the colour and weight for the font, showing inactive codes in bold red text
        /// </summary>
        private void DrawComboBoxItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            TCmbAutoComplete cmb = (TCmbAutoComplete)sender;
            DataRowView drv = (DataRowView)cmb.Items[e.Index];
            string content = drv[1].ToString();
            Brush brush = Brushes.Black;

            if (cmb.Name.StartsWith("cmbDetailBankCostCentre"))
            {
                if (FCostCentreTable != null)
                {
                    ACostCentreRow row = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, content });

                    if ((row != null) && !row.CostCentreActiveFlag)
                    {
                        brush = Brushes.Red;
                    }
                }
            }
            else if (cmb.Name.StartsWith("cmbDetailBankAccount"))
            {
                if (FAccountTable != null)
                {
                    AAccountRow row = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, content });

                    if ((row != null) && !row.AccountActiveFlag)
                    {
                        brush = Brushes.Red;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Unexpected caller of DrawComboBoxItem event");
            }

            Font font = new Font(((Control)sender).Font, (brush == Brushes.Red) ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(content, font, brush, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        /// <summary>
        /// Returns true if the current DefaultView contains rows for the specified year.  False otherwise.
        /// </summary>
        /// <param name="AYear">The database value of the year (not the selected index!)</param>
        private bool BatchYearIsLoaded(Int32 AYear)
        {
            DataView BatchDV = new DataView(FMainDS.AGiftBatch);

            BatchDV.RowFilter = String.Format("{0}={1}",
                AGiftBatchTable.GetBatchYearDBName(),
                AYear);

            return BatchDV.Count > 0;
        }

        #endregion
    }
}