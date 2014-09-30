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
using System.Windows.Forms;
using System.Data;

using Ict.Common;
using Ict.Common.Controls;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles loading of batches and the display of the filter.
    /// Batches can be filtered by batch status, financial year and period within a year
    /// </summary>
    public class TUC_GLBatches_LoadAndFilter
    {
        // Variables that are set by the constructor
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private TFilterAndFindPanel FFilterFindPanelObject = null;

        // Working variables for the controls on the filter panel
        private TCmbAutoComplete FcmbYearEnding = null;
        private TCmbAutoComplete FcmbPeriod = null;
        private RadioButton FrbtEditing = null;
        private RadioButton FrbtPosting = null;
        private RadioButton FrbtAll = null;

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
        /// Get the Selected Year of the Year ComboBox
        /// </summary>
        public Int32 DatabaseYear
        {
            get
            {
                return FcmbYearEnding.GetSelectedInt32();
            }
        }

        /// <summary>
        /// Get/set the Selected Period of the Period ComboBox (as stored in the database)
        /// </summary>
        public Int32 DatabasePeriod
        {
            get
            {
                return FcmbPeriod.GetSelectedInt32();
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="AMainDS">The main data set</param>
        /// <param name="AFilterFindPanelObject">The filter panel control object</param>
        public TUC_GLBatches_LoadAndFilter(int ALedgerNumber, GLBatchTDS AMainDS, TFilterAndFindPanel AFilterFindPanelObject)
        {
            FFilterFindPanelObject = AFilterFindPanelObject;
            FMainDS = AMainDS;
            FLedgerNumber = ALedgerNumber;

            FcmbYearEnding = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbYearEnding");
            FcmbPeriod = (TCmbAutoComplete)AFilterFindPanelObject.FilterPanelControls.FindControlByName("cmbPeriod");
            FrbtEditing = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtEditing");
            FrbtPosting = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtPosting");
            FrbtAll = (RadioButton)AFilterFindPanelObject.FilterPanelControls.FindControlByName("rbtAll");

            TFinanceControls.InitialiseAvailableFinancialYearsList(ref FcmbYearEnding, FLedgerNumber, false, true);
            //TLogging.Log("GL Financial Years completed");
            FrbtEditing.Checked = true;
            //TLogging.Log("Editing checkbox selected");
        }

        #endregion

        #region Public class methods

        private void RefreshPeriods(Int32 ASelectedYear)
        {
            bool IncludeCurrentAndForwardingItem = true;

            //Determine whether or not to include the "Current and forwarding periods" item in the period combo
            if (FMainDS.ALedger.Rows.Count == 1)
            {
                IncludeCurrentAndForwardingItem = (ASelectedYear == FMainDS.ALedger[0].CurrentFinancialYear);
            }

            //TLogging.Log("Populating Period List ...");
            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref FcmbPeriod, FLedgerNumber, ASelectedYear, 0, IncludeCurrentAndForwardingItem);
            //TLogging.Log("Period List done!");
        }

        /// <summary>
        /// The main method that handles all filtering.  Every change on the filter panel causes this event to fire.
        /// It is important to manage the fact that this method may be called recursively and so nesting can be tricky!
        /// </summary>
        /// <param name="AFilterString">On entry this parameter contains the filter control's best guess for the current filter.
        /// The code can modify this string in the light of current control values.</param>
        public void ApplyFilterManual(ref string AFilterString)
        {
            string workingFilter = String.Empty;
            string additionalFilter = String.Empty;
            bool showingAllPeriods = false;

            // Remove the old base filter
            if (FPrevBaseFilter.Length > 0)
            {
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
                //TLogging.Log(String.Format("RefreshPeriods for Year {0}", newYear));
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

                workingFilter = String.Format("{0} = {1}", ABatchTable.GetBatchYearDBName(), newYear);
                showingAllPeriods = true;
            }
            else
            {
                workingFilter = String.Format(
                    "{0} = {1}",
                    ABatchTable.GetBatchYearDBName(), newYear);

                if (newPeriod == 0)  //All periods for year
                {
                    //Nothing to add to filter
                    showingAllPeriods = true;
                }
                else if (newPeriod == -1)
                {
                    workingFilter += String.Format(" AND {0} >= {1}", ABatchTable.GetBatchPeriodDBName(), CurrentLedgerPeriod);
                }
                else if (newPeriod > 0)
                {
                    workingFilter += String.Format(" AND {0} = {1}", ABatchTable.GetBatchPeriodDBName(), newPeriod);
                }
            }

            if (!BatchYearIsLoaded(newYear))
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, newYear, newPeriod));
            }

            if (FrbtEditing.Checked)
            {
                StringHelper.JoinAndAppend(ref workingFilter, String.Format("{0} = '{1}'",
                        ABatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED),
                    CommonJoinString.JOIN_STRING_SQL_AND);
            }
            else if (FrbtPosting.Checked)
            {
                StringHelper.JoinAndAppend(ref workingFilter,
                    String.Format("({0} = '{1}') AND ({2} = {3}) AND ({2} <> 0) AND (({4} = 0) OR ({4} = {2}))",
                        ABatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED,
                        ABatchTable.GetBatchCreditTotalDBName(),
                        ABatchTable.GetBatchDebitTotalDBName(),
                        ABatchTable.GetBatchControlTotalDBName()),
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
        }

        #endregion

        #region Private helper methods

        private bool BatchYearIsLoaded(Int32 AYear)
        {
            DataView BatchDV = new DataView(FMainDS.ABatch);

            BatchDV.RowFilter = String.Format("{0}={1}",
                ABatchTable.GetBatchYearDBName(),
                AYear);

            return BatchDV.Count > 0;
        }

        #endregion
    }
}