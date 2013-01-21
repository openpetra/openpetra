//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2012 by OM International
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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Subscriptions
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

//        /// <summary>
//        /// Gets the data from all controls on this UserControl.
//        /// The data is stored in the DataTables/DataColumns to which the Controls
//        /// are mapped.
//        /// </summary>
//        public void GetDataFromControls2()
//        {
//            GetDataFromControls(FMainDS.PBank[0]);
//        }

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void RecalculateTabHeaderCounter()
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            /* Fire OnRecalculateScreenParts event to update the Tab Counters */
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // Load Partner Types, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PSubscription == null)
                {
                    FMainDS.Tables.Add(new PSubscriptionTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading)
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataSubscriptions());

                    // Make DataRows unchanged
                    if (FMainDS.PSubscription.Rows.Count > 0)
                    {
                        FMainDS.PSubscription.AcceptChanges();
                    }
                }

                if (FMainDS.PSubscription.Rows.Count != 0)
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
            ValidateAllData(false, false);
            //GetDetailsFromControls(GetSelectedDetailRow());
        }

        /// <summary>
        ///
        /// </summary>
        public void SpecialInitUserControl()
        {
            // Set up screen logic
            PartnerEditUIConnector = FPartnerEditUIConnector;
            LoadDataOnDemand();

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpSubscriptions));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                pnlDetails.Visible = false;
                //ucoDetails.MakeScreenInvisible(true);
                btnDelete.Enabled = false;
                btnCancelAllSubscriptions.Enabled = false;
            }

            // use dictionary of details control as validation is called for dictionary of this class
            FValidationControlsDict = ucoDetails.ValidationControlsDict;
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
            ucoDetails.AdjustAfterResizing();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(PSubscriptionTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PSubscriptionTable());
            }

            FMainDS.InitVars();

            ucoDetails.SpecialInitUserControl();
        }

        /// <summary>
        ///
        /// </summary>
        private void ShowDataManual()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(PSubscriptionRow ARow)
        {
            if (ARow != null)
            {
                ucoDetails.AllowEditIssues = true;
                pnlDetails.Visible = true;
                ucoDetails.ShowDetails(ARow);
            }

            btnDelete.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
                btnCancelAllSubscriptions.Enabled = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ARow"></param>
        private void GetDetailDataFromControlsManual(PSubscriptionRow ARow)
        {
            ucoDetails.GetDetails(ARow);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRecord(this, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            if (CreateNewPSubscription())
            {
                ucoDetails.Focus();
            }

            // reset counter in tab header
            RecalculateTabHeaderCounter();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PSubscriptionRow ARow)
        {
            // apply changes from previous record
            //GetDetailsFromControls(GetSelectedDetailRow());

            // Initialize subscription
            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            ARow.PublicationCode = "";
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            this.DeletePSubscription();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PSubscriptionRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
            ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Subscription {0}?"),
                ARowToDelete.PublicationCode);
            return true;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(PSubscriptionRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            // no message to be shown after deletion
            ACompletionMessage = "";

            try
            {
                ARowToDelete.Delete();
                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PSubscriptionRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            DoRecalculateScreenParts();

            if (grdDetails.Rows.Count <= 1)
            {
                // hide details part and disable buttons if no record in grid (first row for headings)
                btnDelete.Enabled = false;
                pnlDetails.Visible = false;
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditIssues(System.Object sender, EventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAllSubscriptions(System.Object sender, EventArgs e)
        {
            PerformCancelAllSubscriptions(DateTime.MinValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ACancelDate"></param>
        /// <returns></returns>
        public Boolean PerformCancelAllSubscriptions(DateTime ACancelDate)
        {
            Boolean ReturnValue;
            TFrmSubscriptionsCancelAllDialog Scd;
            string ReasonEnded;
            DateTime DateEnded;
            ArrayList SubscrCancelled;
            int UpdateCounter;
            String SubscrCancelledString = "";

            ReturnValue = true;

            /* Check whether there are any Subscriptions that can be cancelled */
            if (CancelAllSubscriptionsCount() > 0)
            {
                /* Open 'Cancel All Subscriptions' Dialog */
                Scd = new TFrmSubscriptionsCancelAllDialog(this.ParentForm);

                if (ACancelDate != DateTime.MinValue)
                {
                    Scd.DateEnded = ACancelDate;
                }

                Scd.ShowDialog();

                if (Scd.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    /* Get values from the Dialog */
                    Scd.GetReturnedParameters(out ReasonEnded, out DateEnded);

                    /* Cancel the Subscriptions */
                    SubscrCancelled = CancelAllSubscriptions(ReasonEnded, DateEnded, false);

                    /* Build a String to tell the user what Subscriptions were cancelled. */
                    for (UpdateCounter = 0; UpdateCounter <= SubscrCancelled.Count - 1; UpdateCounter += 1)
                    {
                        SubscrCancelledString = SubscrCancelledString + "   " + SubscrCancelled[UpdateCounter].ToString() + Environment.NewLine;
                    }

                    /* Update the Grid UserControl to reflect the changes in the records. */
                    grdDetails.Refresh();

                    /* Finally, select the first record in the Grid and update the Detail */
                    /* UserControl (this one might have been Canceled) */
                    grdDetails.SelectRowInGrid(1);

                    /* reset counter in tab header */
                    RecalculateTabHeaderCounter();

                    /* Tell the user that cancelling of Subscriptions was succesful */
                    MessageBox.Show(String.Format(Catalog.GetString("The following {0} Subscription(s) was/were cancelled:" + "\r\n" + "{1}" +
                                "\r\n" + "The Partner has no active Subscriptions left."),
                            SubscrCancelled.Count,
                            SubscrCancelledString),
                        Catalog.GetString("All Subscriptions Cancelled"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    /* User pressed Cancel in the Dialog. Tell the user that nothing was done. */
                    MessageBox.Show(Catalog.GetString("No Subscriptions were cancelled."),
                        Catalog.GetString("Cancel All Subscriptions"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ReturnValue = false;
                }

                Scd.Dispose();
            }
            else
            {
                /* Tell the user that there are no Subscriptions that can be canceled. */
                MessageBox.Show(Catalog.GetString("There are no Subscriptions to cancel."),
                    Catalog.GetString("Cancel All Subscriptions"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Counts all cancelable Subscriptions.
        ///
        /// </summary>
        /// <returns>Number of cancelable Subscriptions
        /// </returns>
        public Int32 CancelAllSubscriptionsCount()
        {
            return CancelAllSubscriptions("", DateTime.MinValue, true).Count;
        }

        /// <summary>
        /// Cancels all Subscriptions (that are not already CANCELED or EXPIRED).
        ///
        /// </summary>
        /// <param name="AReasonEnded">Text that gives the reason for ending the Subscriptions</param>
        /// <param name="ADateEnded">Date when the Subscriptions should end (can be empty)</param>
        /// <param name="ACountOnly">do not actually cancel subscriptions but return arrayList of subscriptions that can be cancelled for counting purposes</param>
        /// <returns>ArrayList holding Publication Codes of the Subscriptions that were
        /// canceled
        /// </returns>
        private ArrayList CancelAllSubscriptions(String AReasonEnded, System.DateTime ADateEnded, Boolean ACountOnly)
        {
            ArrayList ReturnValue;
            DataView CurrentSubsDV;
            PSubscriptionRow SubscriptionRow;
            Int16 RowCounter;

            ReturnValue = new ArrayList();

            /* Loop over all nondeleted Subscriptions and check whether they should be */
            /* Canceled. */
            CurrentSubsDV = new DataView(FMainDS.PSubscription, "", "", DataViewRowState.CurrentRows);

            for (RowCounter = 0; RowCounter <= CurrentSubsDV.Count - 1; RowCounter += 1)
            {
                SubscriptionRow = (PSubscriptionRow)CurrentSubsDV[RowCounter].Row;

                if (SubscriptionRow.IsSubscriptionStatusNull()
                    || (SubscriptionRow.SubscriptionStatus != MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED)
                    && (SubscriptionRow.SubscriptionStatus != MPartnerConstants.SUBSCRIPTIONS_STATUS_EXPIRED))
                {
                    /* this should not happen, but one never knows... */

                    if (!ACountOnly)
                    {
                        SubscriptionRow.BeginEdit();
                        SubscriptionRow.SubscriptionStatus = MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED;
                        SubscriptionRow.DateCancelled = ADateEnded;
                        SubscriptionRow.ReasonSubsCancelledCode = AReasonEnded;
                        SubscriptionRow.GiftFromKey = 0;
                        SubscriptionRow.EndEdit();
                    }

                    ReturnValue.Add(SubscriptionRow.PublicationCode);
                }
            }

            return ReturnValue;
        }

        private void ValidateDataDetailsManual(PSubscriptionRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateSubscriptionManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        #endregion
    }
}