//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       dinwiggy
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData_PreviousExperience
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Properties

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

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        // / <summary>
        // / todoComment
        // / </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewPmPastExperience();
        }

        // Sets the key for a new row
        private void NewRowManual(ref PmPastExperienceRow ARow)
        {
            ARow.PartnerKey = FMainDS.PPerson[0].PartnerKey;
            ARow.Key = -1;
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

// TODO: perform a check if the value is already referenced somewhere (similar to what the commented-out code does)
//            int num = TRemote.MFinance.Setup.WebConnectors.CheckDeleteAFreeformAnalysis(FLedgerNumber,
//                FPreviouslySelectedDetailRow.AnalysisTypeCode,
//                FPreviouslySelectedDetailRow.AnalysisValue);
//
//            if (num > 0)
//            {
//                MessageBox.Show(Catalog.GetString(
//                        "This value is already referenced and cannot be deleted."));
//                return;
//            }

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete this value ({0}).\n\nDo you really want to delete it?"),
                        FPreviouslySelectedDetailRow.PrevRole), Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);

                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private void ShowDetailsManual(PmPastExperienceRow ARow)
        {
            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            // Get data out of the Controls only if there is at least one row of data (Note: Column Headers count as one row)
            if (grdDetails.Rows.Count > 1)
            {
                GetDataFromControls();
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
        
        /// <summary>
        /// called for HereFlag event for work location check box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HereFlagChanged(object sender, EventArgs e)
        {
            if (this.chkHereFlag.Checked)
            {
                chkSimilarOrgFlag.Enabled = false;
                chkSimilarOrgFlag.Checked = true;
            }
            else
            {
                chkSimilarOrgFlag.Enabled = true;
                chkSimilarOrgFlag.Checked = false;
            }
             
        }

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);

                pnlDetails.Visible = true;
            }
            else
            {
                pnlDetails.Visible = false;
            }
        }

        /// <summary>
        /// Loads Previous Experience Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PmPastExperience == null)
                {
                    FMainDS.Tables.Add(new PmPastExperienceTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.PmPastExperience.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiPreviousExperiences));

                    // Make DataRows unchanged
                    if (FMainDS.PmPastExperience.Rows.Count > 0)
                    {
                        if (FMainDS.PmPastExperience.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmPastExperience.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PmPastExperience.Rows.Count != 0)
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

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }
    }
}