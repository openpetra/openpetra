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
    public class TUC_RecurringGiftBatches_LoadAndFilter
    {
        // Variables that are set by the constructor
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFilterAndFindPanel FFilterFindPanelObject = null;

        // Table references that we need for drawing the inactive account codes
        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        // Other class variables for tracking the filter
        private string FPrevBaseFilter = String.Empty;
        private string FPrevFilter = String.Empty;

        #region Public Properties

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
        /// <param name="APetraUtilsObject">Reference to the PetraUtilsObject for the form</param>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="AMainDS">The main data set</param>
        /// <param name="AFilterFindPanelObject">The filter panel control object</param>
        public TUC_RecurringGiftBatches_LoadAndFilter(TFrmPetraEditUtils APetraUtilsObject,
            int ALedgerNumber,
            GiftBatchTDS AMainDS,
            TFilterAndFindPanel AFilterFindPanelObject)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FFilterFindPanelObject = AFilterFindPanelObject;

            FMainDS.ARecurringGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ARecurringGiftBatchTable.GetLedgerNumberDBName(),
                ARecurringGiftBatchTable.GetBatchNumberDBName()
                );
        }

        #endregion

        #region Public class methods

        /// <summary>
        /// This method must be called in order to correctly populate the ComboBoxes on the filter panel (by cloning from the main details panel)
        /// </summary>
        /// <param name="ACmbCostCentre"></param>
        /// <param name="ACmbAccountCode"></param>
        public void InitialiseDataSources(TCmbAutoPopulated ACmbCostCentre, TCmbAutoPopulated ACmbAccountCode)
        {
            // We have to do these because the filter/find panel is displayed when the screen is loaded, so they do not get populated
            InitFilterFindComboBox(ACmbCostCentre,
                (TCmbAutoComplete)FFilterFindPanelObject.FilterPanelControls.FindControlByName(ACmbCostCentre.Name));
            InitFilterFindComboBox(ACmbAccountCode,
                (TCmbAutoComplete)FFilterFindPanelObject.FilterPanelControls.FindControlByName(ACmbAccountCode.Name));
            InitFilterFindComboBox(ACmbCostCentre,
                (TCmbAutoComplete)FFilterFindPanelObject.FindPanelControls.FindControlByName(ACmbCostCentre.Name));
            InitFilterFindComboBox(ACmbAccountCode,
                (TCmbAutoComplete)FFilterFindPanelObject.FindPanelControls.FindControlByName(ACmbAccountCode.Name));
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Helper method that we can call to initialise each of the filter/find comboBoxes
        /// </summary>
        private void InitFilterFindComboBox(TCmbAutoPopulated AClonedFromComboBox,
            TCmbAutoComplete AFFInstance)
        {
            AFFInstance.DisplayMember = AClonedFromComboBox.DisplayMember;
            AFFInstance.ValueMember = AClonedFromComboBox.ValueMember;

            if (AClonedFromComboBox.Name.Contains("Account"))
            {
                // This is quicker than getting the cached table again
                DataView dv = new DataView(FAccountTable.Copy());
                dv.RowFilter = TFinanceControls.PrepareAccountFilter(true, false, false, false, "");
                dv.Sort = String.Format("{0}", AAccountTable.GetAccountCodeDBName());
                AFFInstance.DataSource = dv;
            }
            else if (AClonedFromComboBox.Name.Contains("CostCentre"))
            {
                // This is quicker than getting the cached table again
                DataView dv = new DataView(FCostCentreTable.Copy());
                dv.RowFilter = TFinanceControls.PrepareCostCentreFilter(true, false, false, false);
                dv.Sort = String.Format("{0}", ACostCentreTable.GetCostCentreCodeDBName());
                AFFInstance.DataSource = dv;
            }
            else
            {
                throw new Exception("Unexpected ComboBox name");
            }

            AFFInstance.DrawMode = DrawMode.OwnerDrawFixed;
            AFFInstance.DrawItem += new DrawItemEventHandler(DrawComboBoxItem);
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

        #endregion
    }
}