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
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Validation;

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

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            this.SizeChanged += TUC_IndividualData_PreviousExperience_SizeChanged;

            // Cannot resize the grid here because the grid columns have not been defined yet
        }

        private void TUC_IndividualData_PreviousExperience_SizeChanged(object sender, EventArgs e)
        {
            Control control = this.Parent;

            while (control as TUC_IndividualData == null)
            {
                control = control.Parent;
            }

            TUC_IndividualData myParent = control as TUC_IndividualData;

            if (myParent.CurrentControl == TUC_IndividualData.TDynamicLoadableUserControls.dlucPreviousExperience)
            {
                FPetraUtilsObject.RefreshSpecificControlOnWindowMaxOrRestore(pnlDetailGrid);
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            this.CreateNewPmPastExperience();
        }

        // Sets the key for a new row
        private void NewRowManual(ref IndividualDataTDSPmPastExperienceRow ARow)
        {
            ARow.PartnerKey = FMainDS.PPerson[0].PartnerKey;
            ARow.Key = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_past_experience));
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(IndividualDataTDSPmPastExperienceRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2}, {3} {4})",
                Environment.NewLine,
                lblLocation.Text,
                txtLocation.Text,
                lblRole.Text,
                txtRole.Text);
            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(IndividualDataTDSPmPastExperienceRow ARowToDelete,
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

        private void ShowDetailsManual(IndividualDataTDSPmPastExperienceRow ARow)
        {
            txtEventName.Text = ARow.EventName;

            if (ARow.EventName != "")
            {
                txtEventName.Text = "(" + ARow.EventName + ")";
            }

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
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
            pnlDetailGrid.Refresh();
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

        private void ValidateDataDetailsManual(IndividualDataTDSPmPastExperienceRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPersonnelValidation_Personnel.ValidatePreviousExperienceManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ShowDataManual()
        {
            grdDetails.AutoResizeGrid();
        }
    }
}