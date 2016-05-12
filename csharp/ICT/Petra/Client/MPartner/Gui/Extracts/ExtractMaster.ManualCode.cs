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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MReporting.Gui.MPartner;
using Ict.Petra.Client.MReporting.Gui.MPersonnel;
using Ict.Petra.Client.MReporting.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TFrmExtractMaster
    {
        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private ExtractTDS FMainDS = null;

        #region Public Methods

        #region Public Static Methods

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        public static void VerifyAndUpdateExtract(System.Windows.Forms.Form AForm, int AExtractId)
        {
            ExtractTDSMExtractTable ExtractTable;
            bool ChangesMade;
            bool ChangesNeeded;

            AForm.Cursor = Cursors.WaitCursor;

            try
            {
                // retrieve contents of extract from server
                ExtractTable = TRemote.MPartner.Partner.WebConnectors.GetExtractRowsWithPartnerData(AExtractId);

                VerifyAndUpdateExtract(AForm, ref ExtractTable, out ChangesMade, out ChangesNeeded);

                foreach (DataRow InspectDR in ExtractTable.Rows)
                {
                    InspectDR.EndEdit();
                }

                TSubmitChangesResult SubmissionResult;

                MExtractTable SubmitDT = new MExtractTable();

                if (ExtractTable.GetChangesTyped() != null)
                {
                    SubmitDT.Merge(ExtractTable.GetChangesTyped());
                }

                if ((SubmitDT.Rows.Count == 0)
                    || !ChangesMade)
                {
                    if (ChangesNeeded)
                    {
                        MessageBox.Show(Catalog.GetString("Verification and Update of Extract completed"),
                            Catalog.GetString("Verify and Update Extract"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else if (!ChangesNeeded)
                    {
                        MessageBox.Show(Catalog.GetString("Verification and Update of Extract completed.") + "\r\n\r\n" +
                            Catalog.GetString("Extract was already up to date."),
                            Catalog.GetString("Verify and Update Extract"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    // return since no changes were made
                    return;
                }

                // Submit changes to the PETRAServer
                SubmissionResult = TRemote.MPartner.Partner.WebConnectors.SaveExtract
                                       (AExtractId, ref SubmitDT);

                if (SubmissionResult == TSubmitChangesResult.scrError)
                {
                    MessageBox.Show(Catalog.GetString("Verify and Update of Extract failed"),
                        Catalog.GetString("Verify and Update Extract"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Verification and Update of Extract was successful"),
                        Catalog.GetString("Verify and Update Extract"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            finally
            {
                AForm.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        public static void VerifyAndUpdateExtract(System.Windows.Forms.Form AForm,
            ref ExtractTDSMExtractTable AExtractTable,
            out bool AChangesMade, out bool AChangesNeeded)
        {
            bool AddressExists;
            bool OtherAddressExists;
            bool AddressIsBestOne;
            bool AddressIsCurrent;
            bool OtherAddressIsCurrent;
            bool AddressIsMailing;
            bool OtherAddressIsMailing;
            bool ReplaceAddress;
            bool ReplaceAddressCurrentAndMailingYesToAll = false;
            bool ReplaceAddressCurrentAndMailingNoToAll = false;
            bool ReplaceAddressCurrentYesToAll = false;
            bool ReplaceAddressCurrentNoToAll = false;
            bool ReplaceAddressMailingYesToAll = false;
            bool ReplaceAddressMailingNoToAll = false;
            string CountryName;
            PLocationTable LocationTable = new PLocationTable();
            PLocationRow LocationRow;
            TFrmExtendedMessageBox MsgBox = new TFrmExtendedMessageBox(AForm);;

            TFrmExtendedMessageBox.TResult MsgBoxResult;
            bool DontShowPartnerRemovePartnerKeyNonExistent = false;
            bool DontShowReplaceMissingAddress = false;
            bool DontShowPartnerRemoveNoAddress = false;
            bool DontShowAddressWillNotBeReplaced = false;

            // initialize output parameters
            AChangesMade = false;
            AChangesNeeded = false;

            // collection of partners that need their address updated
            List <ExtractTDSMExtractRow>RowsToDelete = new List <ExtractTDSMExtractRow>();

            // prepare mouse cursor so user knows something is happening
            AForm.Cursor = Cursors.WaitCursor;

            // look at every single extract row
            foreach (ExtractTDSMExtractRow Row in AExtractTable.Rows)
            {
                ReplaceAddress = false;

                // check if the partner record still exists, otherwise remove from extract
                if (!TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartner(Row.PartnerKey))
                {
                    AChangesNeeded = true;

                    if (!DontShowPartnerRemovePartnerKeyNonExistent) // if user has not requested to not see these messages
                    {
                        // warn the user what is happening
                        // (this message really never come up as constraints with this extracts should have prevented the partner to be deleted
                        MsgBox.ShowDialog(String.Format(Catalog.GetString("The following partner record does not exist any longer and " +
                                    "will therefore be removed from this extract: \r\n\r\n" +
                                    "{0} ({1})"), Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                            Catalog.GetString("Verify and Update Extract"),
                            "",
                            TFrmExtendedMessageBox.TButtons.embbOK,
                            TFrmExtendedMessageBox.TIcon.embiInformation);

                        MsgBoxResult = MsgBox.GetResult(out DontShowPartnerRemovePartnerKeyNonExistent);
                    }

                    RowsToDelete.Add(Row);
                }
                else
                {
                    AddressExists = TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartnerAtLocation
                                        (Row.PartnerKey, new TLocationPK(Row.SiteKey, Row.LocationKey), out AddressIsCurrent, out AddressIsMailing);

                    if (!AddressExists) // if address no longer exists
                    {
                        AChangesNeeded = true;

                        // prepare: get best address for later use
                        if (TRemote.MPartner.Mailing.WebConnectors.GetBestAddress(Row.PartnerKey,
                                out LocationTable, out CountryName))
                        {
                            if (!DontShowReplaceMissingAddress) // if user has not requested to not see these messages
                            {
                                // warn the user what is happening
                                MsgBox = new TFrmExtendedMessageBox(AForm);
                                MsgBox.ShowDialog(String.Format(Catalog.GetString("Address for {0} ({1}) in this extract no longer exists. \r\n\r\n"
                                            +
                                            "It will therefore be replaced with a current address."),
                                        Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                    Catalog.GetString("Verify and Update Extract"),
                                    Catalog.GetString("Don't show this message again"),
                                    TFrmExtendedMessageBox.TButtons.embbOK,
                                    TFrmExtendedMessageBox.TIcon.embiInformation);

                                MsgBoxResult = MsgBox.GetResult(out DontShowReplaceMissingAddress);
                            }

                            ReplaceAddress = true;
                        }
                        else
                        {
                            // in this case there is no address at all for this partner (should not really happen)
                            if (!DontShowPartnerRemoveNoAddress)
                            {
                                MsgBox = new TFrmExtendedMessageBox(AForm);
                                MsgBox.ShowDialog(String.Format(Catalog.GetString("No address could be found for {0} ({1}). \r\n\r\n" +
                                            "Therefore the partner record will be removed from this extract"),
                                        Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                    Catalog.GetString("Verify and Update Extract"),
                                    Catalog.GetString("Don't show this message again"),
                                    TFrmExtendedMessageBox.TButtons.embbOK,
                                    TFrmExtendedMessageBox.TIcon.embiInformation);
                                MsgBoxResult = MsgBox.GetResult(out DontShowPartnerRemoveNoAddress);
                            }

                            RowsToDelete.Add(Row);
                        }
                    }
                    else if (!AddressIsCurrent
                             || !AddressIsMailing) // handle addresses that still exist but are no longer current or mailing
                    {
                        AddressIsBestOne = false;
                        ReplaceAddress = false;

                        // first check if any other address is better than the existing one

                        /* it could be that GetBestAddress still returns a non-current address if
                         * there is no better one */
                        OtherAddressExists = TRemote.MPartner.Mailing.WebConnectors.GetBestAddress(Row.PartnerKey,
                            out LocationTable, out CountryName);

                        if (OtherAddressExists)
                        {
                            if (LocationTable.Rows.Count > 0)
                            {
                                LocationRow = (PLocationRow)LocationTable.Rows[0];

                                TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartnerAtLocation
                                    (Row.PartnerKey, new TLocationPK(LocationRow.SiteKey, LocationRow.LocationKey),
                                    out OtherAddressIsCurrent, out OtherAddressIsMailing);

                                /* check if existing address is best one */
                                if ((Row.SiteKey == LocationRow.SiteKey)
                                    && (Row.LocationKey == LocationRow.LocationKey))
                                {
                                    AddressIsBestOne = true;
                                }
                            }
                        }

                        if (!AddressIsCurrent
                            && !AddressIsMailing) // address still exists but is not longer current and also not mailing
                        {
                            if (AddressIsBestOne)
                            {
                                if (!DontShowAddressWillNotBeReplaced)
                                {
                                    // warn the user what is happening
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                "Address for {0} ({1}) in this extract is not current and not a mailing address. \r\n\r\n" +
                                                "But it will not be changed as it is still the best one."),
                                            Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                        Catalog.GetString("Verify and Update Extract"),
                                        Catalog.GetString("Don't show this message again"),
                                        TFrmExtendedMessageBox.TButtons.embbOK,
                                        TFrmExtendedMessageBox.TIcon.embiInformation);
                                    MsgBoxResult = MsgBox.GetResult(out DontShowAddressWillNotBeReplaced);
                                }
                            }
                            else
                            {
                                AChangesNeeded = true;

                                if (!ReplaceAddressCurrentAndMailingYesToAll && !ReplaceAddressCurrentAndMailingNoToAll) // if user has not requested to not see these messages
                                {
                                    // ask the user if the address should be updates
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBoxResult =
                                        MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                    "Address for {0} ({1}) in this extract is not current and not a mailing address. \r\n\r\n" +
                                                    "Do you want to update it with a better one?"),
                                                Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                            Catalog.GetString("Verify and Update Extract"),
                                            "",
                                            TFrmExtendedMessageBox.TButtons.embbYesYesToAllNoNoToAll,
                                            TFrmExtendedMessageBox.TIcon.embiQuestion);

                                    if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYesToAll)
                                    {
                                        ReplaceAddress = true;
                                        ReplaceAddressCurrentAndMailingYesToAll = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYes)
                                    {
                                        ReplaceAddress = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrNoToAll)
                                    {
                                        ReplaceAddressCurrentAndMailingNoToAll = true;
                                    }
                                }
                                else if (ReplaceAddressCurrentAndMailingYesToAll)
                                {
                                    ReplaceAddress = true;
                                }
                                else if (ReplaceAddressCurrentAndMailingNoToAll)
                                {
                                    ReplaceAddress = false;
                                }
                            }
                        }
                        else if (!AddressIsCurrent)
                        {
                            if (AddressIsBestOne)
                            {
                                if (!DontShowAddressWillNotBeReplaced)
                                {
                                    // warn the user what is happening
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                "Address for {0} ({1}) in this extract is not current. \r\n\r\n" +
                                                "But it will not be changed as it is still the best one."),
                                            Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                        Catalog.GetString("Verify and Update Extract"),
                                        Catalog.GetString("Don't show this message again"),
                                        TFrmExtendedMessageBox.TButtons.embbOK,
                                        TFrmExtendedMessageBox.TIcon.embiInformation);
                                    MsgBoxResult = MsgBox.GetResult(out DontShowAddressWillNotBeReplaced);
                                }
                            }
                            else
                            {
                                AChangesNeeded = true;

                                if (!ReplaceAddressCurrentYesToAll && !ReplaceAddressCurrentNoToAll) // if user has not requested to not see these messages
                                {
                                    // ask the user if the address should be updated
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBoxResult =
                                        MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                    "Address for {0} ({1}) in this extract is not current. \r\n\r\n" +
                                                    "Do you want to update it with a better one?"),
                                                Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                            Catalog.GetString("Verify and Update Extract"),
                                            "",
                                            TFrmExtendedMessageBox.TButtons.embbYesYesToAllNoNoToAll,
                                            TFrmExtendedMessageBox.TIcon.embiQuestion);

                                    if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYesToAll)
                                    {
                                        ReplaceAddress = true;
                                        ReplaceAddressCurrentYesToAll = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYes)
                                    {
                                        ReplaceAddress = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrNoToAll)
                                    {
                                        ReplaceAddressCurrentNoToAll = true;
                                    }
                                }
                                else if (ReplaceAddressCurrentYesToAll)
                                {
                                    ReplaceAddress = true;
                                }
                                else if (ReplaceAddressCurrentNoToAll)
                                {
                                    ReplaceAddress = false;
                                }
                            }
                        }
                        else if (!AddressIsMailing)
                        {
                            if (AddressIsBestOne)
                            {
                                if (!DontShowAddressWillNotBeReplaced)
                                {
                                    // warn the user what is happening
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                "Address for {0} ({1}) in this extract is not a mailing address. \r\n\r\n" +
                                                "But it will not be changed as it is still the best one."),
                                            Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                        Catalog.GetString("Verify and Update Extract"),
                                        Catalog.GetString("Don't show this message again"),
                                        TFrmExtendedMessageBox.TButtons.embbOK,
                                        TFrmExtendedMessageBox.TIcon.embiInformation);
                                    MsgBoxResult = MsgBox.GetResult(out DontShowAddressWillNotBeReplaced);
                                }
                            }
                            else
                            {
                                AChangesNeeded = true;

                                if (!ReplaceAddressMailingYesToAll && !ReplaceAddressMailingNoToAll) // if user has not requested to not see these messages
                                {
                                    // ask the user if the address should be updates
                                    MsgBox = new TFrmExtendedMessageBox(AForm);
                                    MsgBoxResult =
                                        MsgBox.ShowDialog(String.Format(Catalog.GetString(
                                                    "Address for {0} ({1}) in this extract is not a mailing address. \r\n\r\n" +
                                                    "Do you want to update it with a better one?"),
                                                Row.PartnerShortName, String.Format("{0:0000000000}", Row.PartnerKey)),
                                            Catalog.GetString("Verify and Update Extract"),
                                            "",
                                            TFrmExtendedMessageBox.TButtons.embbYesYesToAllNoNoToAll,
                                            TFrmExtendedMessageBox.TIcon.embiQuestion);

                                    if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYesToAll)
                                    {
                                        ReplaceAddress = true;
                                        ReplaceAddressMailingYesToAll = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYes)
                                    {
                                        ReplaceAddress = true;
                                    }
                                    else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrNoToAll)
                                    {
                                        ReplaceAddressMailingNoToAll = true;
                                    }
                                }
                                else if (ReplaceAddressMailingYesToAll)
                                {
                                    ReplaceAddress = true;
                                }
                                else if (ReplaceAddressMailingNoToAll)
                                {
                                    ReplaceAddress = false;
                                }
                            }
                        }
                    }

                    if (ReplaceAddress)
                    {
                        if (LocationTable.Rows.Count > 0)
                        {
                            LocationRow = (PLocationRow)LocationTable.Rows[0];

                            if ((Row.SiteKey != LocationRow.SiteKey)
                                || (Row.LocationKey != LocationRow.LocationKey))
                            {
                                AChangesMade = true;

                                Row.SiteKey = LocationRow.SiteKey;
                                Row.LocationKey = LocationRow.LocationKey;
                            }
                        }
                    }
                }
            }

            // for each partner that needs removed from the extract
            foreach (ExtractTDSMExtractRow Row in RowsToDelete)
            {
                AChangesMade = true;
                Row.Delete();  // now delete the actual row
            }

            // prepare mouse cursor so user knows something is happening
            AForm.Cursor = Cursors.Default;
        }

        #endregion

        #region Public Non-Static Methods

        ///// <summary>
        ///// Loads Partner Types Data from Petra Server into FMainDS.
        ///// </summary>
        ///// <returns>true if successful, otherwise false.</returns>
        //public Boolean LoadData()
        //{
        //    Boolean ReturnValue = true;

        //    return ReturnValue;
        //}

        /// <summary>
        /// Get the number of changed records and specify a message to incorporate into the 'Do you want to save?' message box
        /// </summary>
        /// <param name="AMessage">An optional message to display.  If the parameter is an empty string a default message will be used</param>
        /// <returns>The number of changed records.  Return -1 to imply 'unknown'.</returns>
        public int GetChangedRecordCount(out string AMessage)
        {
            AMessage = String.Empty;
            return -1;
        }

        /// <summary>
        /// save the changes on the screen (code is copied from auto-generated code)
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return ucoExtractMasterList.SaveChanges();
        }

        /// <summary>
        /// react to menu item / save button
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        private void InitializeManualCode()
        {
            ucoExtractMasterList.DelegateRefreshExtractList = @RefreshExtractList;
        }

        private void RunOnceOnActivationManual()
        {
            ucoExtractMasterList.RunOnceOnParentActivation();
        }

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        public bool RefreshExtractList()
        {
            // Do not allow refresh of the extract list if the user has made changes to any of the records
            // as otherwise their changes will be overwritten by reloading of the data.
            if (FPetraUtilsObject.HasChanges)
            {
                if (MessageBox.Show(Catalog.GetString(
                            "Before refreshing the list you need to save changes made in this screen. " + "\r\n" + "\r\n" +
                            "Would you like to save changes now?"),
                        Catalog.GetString("Refresh List"),
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveChanges();
                }
                else
                {
                    return false;
                }
            }

            ucoExtractMasterList.RefreshExtractList("", true, "", "");

            return true;
        }

        #region Purge, Combine, Intersect and Subtract

        /// <summary>
        /// Purge Extracts (open a screen for user to set parameters)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PurgeExtracts(System.Object sender, EventArgs e)
        {
            // open dialog to prompt the user to enter a name for new extract
            TFrmExtractPurgingDialog ExtractPurgingDialog = new TFrmExtractPurgingDialog(this.ParentForm);
            Boolean PurgingSuccessful = false;

            // purging of extracts happens in the dialog
            ExtractPurgingDialog.ShowDialog();

            if (ExtractPurgingDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
            {
                /* Get values from the Dialog */
                ExtractPurgingDialog.GetReturnedParameters(out PurgingSuccessful);

                if (PurgingSuccessful)
                {
                    ucoExtractMasterList.RefreshExtractList(sender, e);
                }
            }

            ExtractPurgingDialog.Dispose();
        }

        /// <summary>
        /// Combine Extracts (open a screen to select extracts to combine)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombineExtracts(System.Object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                TFrmExtractCombineIntersectSubtractDialog ExtractCombineDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);

                List <Int32>ACombineExtractIdList;

                // initialize dialog
                ExtractCombineDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisCombineMode);

                if (ucoExtractMasterList.GetSelectedDetailRow() != null)
                {
                    // Show currently selected extract in the grid in the dialog
                    ExtractCombineDialog.PreSelectExtract(ucoExtractMasterList.GetSelectedDetailRow());
                }

                // combining/intersecting of extracts happens in the dialog
                ExtractCombineDialog.ShowDialog();

                if (ExtractCombineDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    /* Get values from the Dialog */
                    ExtractCombineDialog.GetReturnedParameters(out ACombineExtractIdList);

                    // now first the user needs to give the new combined extract a name
                    TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(this);
                    int NewExtractId = 0;
                    string NewExtractName;
                    string NewExtractDescription;

                    ExtractNameDialog.ShowDialog();

                    if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        /* Get values from the Dialog */
                        ExtractNameDialog.GetReturnedParameters(out NewExtractName, out NewExtractDescription);
                    }
                    else
                    {
                        // dialog was cancelled, do not continue with extract generation
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;

                    ExtractNameDialog.Dispose();

                    if (TRemote.MPartner.Partner.WebConnectors.CombineExtracts
                            (NewExtractName, NewExtractDescription, ACombineExtractIdList,
                            out NewExtractId))
                    {
                        ucoExtractMasterList.RefreshExtractList(sender, e);

                        MessageBox.Show(Catalog.GetString("Combine Extracts successful."));
                    }
                }

                ExtractCombineDialog.Dispose();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Intersect Extracts (open a screen to select extracts to intersect)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntersectExtracts(System.Object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                TFrmExtractCombineIntersectSubtractDialog ExtractIntersectDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);

                List <Int32>AIntersectExtractIdList;

                // initialize dialog
                ExtractIntersectDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisIntersectMode);

                if (ucoExtractMasterList.GetSelectedDetailRow() != null)
                {
                    // Show currently selected extract in the grid in the dialog
                    ExtractIntersectDialog.PreSelectExtract(ucoExtractMasterList.GetSelectedDetailRow());
                }

                // combining/intersecting of extracts happens in the dialog
                ExtractIntersectDialog.ShowDialog();

                if (ExtractIntersectDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    /* Get values from the Dialog */
                    ExtractIntersectDialog.GetReturnedParameters(out AIntersectExtractIdList);

                    // now first the user needs to give the new intersected extract a name
                    TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(this);
                    int NewExtractId = 0;
                    string NewExtractName;
                    string NewExtractDescription;

                    ExtractNameDialog.ShowDialog();

                    if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        /* Get values from the Dialog */
                        ExtractNameDialog.GetReturnedParameters(out NewExtractName, out NewExtractDescription);
                    }
                    else
                    {
                        // dialog was cancelled, do not continue with extract generation
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;

                    ExtractNameDialog.Dispose();

                    if (TRemote.MPartner.Partner.WebConnectors.IntersectExtracts
                            (NewExtractName, NewExtractDescription, AIntersectExtractIdList,
                            out NewExtractId))
                    {
                        ucoExtractMasterList.RefreshExtractList(sender, e);

                        MessageBox.Show(Catalog.GetString("Intersect Extracts successful."));
                    }
                }

                ExtractIntersectDialog.Dispose();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Subtract Extracts (open a screen to select extracts to subtract from each other)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubtractExtracts(System.Object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                TFrmExtractCombineIntersectSubtractDialog ExtractSubtractDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);
                String BaseExtractName;

                List <Int32>AIntersectExtractIdList;

                // initialize dialog
                ExtractSubtractDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisSubtractMode);

                if (ucoExtractMasterList.GetSelectedDetailRow() != null)
                {
                    // Show currently selected extract in the grid in the dialog
                    ExtractSubtractDialog.PreSelectExtract(ucoExtractMasterList.GetSelectedDetailRow());
                }

                // show dialog so the user can select extracts to be subtracted
                ExtractSubtractDialog.ShowDialog();

                if (ExtractSubtractDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    // Get values from the Dialog
                    ExtractSubtractDialog.GetReturnedParameters(out BaseExtractName, out AIntersectExtractIdList);

                    // now first the user needs to give the new intersected extract a name
                    TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(this);
                    int NewExtractId = 0;
                    string NewExtractName;
                    string NewExtractDescription;

                    ExtractNameDialog.ShowDialog();

                    if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        /* Get values from the Dialog */
                        ExtractNameDialog.GetReturnedParameters(out NewExtractName, out NewExtractDescription);
                    }
                    else
                    {
                        // dialog was cancelled, do not continue with extract generation
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;

                    ExtractNameDialog.Dispose();

                    if (TRemote.MPartner.Partner.WebConnectors.SubtractExtracts
                            (NewExtractName, NewExtractDescription, BaseExtractName, AIntersectExtractIdList,
                            out NewExtractId))
                    {
                        ucoExtractMasterList.RefreshExtractList(sender, e);

                        MessageBox.Show(Catalog.GetString("Subtract Extracts successful."));
                    }
                }

                ExtractSubtractDialog.Dispose();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Override of Form.Show(IWin32Window owner) Method. Caters for singleton Forms. Does not actually display the form.
        /// </summary>
        /// <param name="owner">Any object that implements <see cref="IWin32Window" /> and represents the top-level window that will own this Form. </param>
        public void ShowInvisible(IWin32Window owner)
        {
            Form OpenScreen = TFormsList.GFormsList[this.GetType().FullName];
            bool OpenSelf = true;

            if ((OpenScreen != null)
                && (OpenScreen.Modal != true))
            {
                if (TFormsList.GSingletonForms.Contains(this.GetType().Name))
                {
//                      MessageBox.Show("Activating singleton screen of Type '" + this.GetType().FullName + "'.");

                    OpenSelf = false;
                    this.Visible = false;       // needed as this.Close() would otherwise bring this Form to the foreground and OpenScreen.BringToFront() would not help...
                    this.Close();
                }
            }

            if (OpenSelf)
            {
                // add Form to TFormsList.GFormsList but do not show it
                FPetraUtilsObject.TFrmPetra_Load(this, null);

                // removing this event stops the above command be called when the Form is eventually shown
                this.Load -= new System.EventHandler(this.TFrmPetra_Load);
            }
        }

        #endregion

        #region Create Various Extracts

        /// <summary>
        /// Create General Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateGeneralExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerByGeneralCriteriaExtract(FindForm());
        }

        /// <summary>
        /// Create Manual Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateManualExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerNewManualExtract(FindForm());
        }

        /// <summary>
        /// Create Partner By City Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByCityExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerByCityExtract(FindForm());
        }

        /// <summary>
        /// Create Partner By Subscription Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerBySubscriptionExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerBySubscriptionExtract(FindForm());
        }

        /// <summary>
        /// Create Partner By Relationship Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByRelationshipExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerByRelationshipExtract(FindForm());
        }

        /// <summary>
        /// Create Partner By Special Type Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerBySpecialTypeExtract(System.Object sender, EventArgs e)
        {
            TFrmPartnerBySpecialType frm = new TFrmPartnerBySpecialType(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Manual Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFamilyMembersExtract(System.Object sender, EventArgs e)
        {
            if (ucoExtractMasterList.GetSelectedDetailRow() != null)
            {
                TPartnerExtractsMain.CreateFamilyMembersExtract(FindForm(), ucoExtractMasterList.GetSelectedDetailRow());
            }
            else
            {
                TPartnerExtractsMain.CreateFamilyMembersExtract(FindForm(), null);
            }
        }

        /// <summary>
        /// Create Manual Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFamilyExtractForPersons(System.Object sender, EventArgs e)
        {
            if (ucoExtractMasterList.GetSelectedDetailRow() != null)
            {
                TPartnerExtractsMain.CreateFamilyExtractForPersons(FindForm(), ucoExtractMasterList.GetSelectedDetailRow());
            }
            else
            {
                TPartnerExtractsMain.CreateFamilyExtractForPersons(FindForm(), null);
            }
        }

        private void CreateContactLogExtract(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.PartnerByContactLogExtract(FindForm());
        }

        /// <summary>
        /// Create Partner By Conference Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByEventExtract(System.Object sender, EventArgs e)
        {
            TFrmPartnerByEvent frm = new TFrmPartnerByEvent(FindForm());

            frm.CalledFromExtracts = true;
            frm.CalledForConferences = true;
            frm.Show();
        }

        /// <summary>
        /// Create Partner By Event Role Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByEventRoleExtract(System.Object sender, EventArgs e)
        {
            TFrmPartnerByEventRole frm = new TFrmPartnerByEventRole(FindForm());

            frm.CalledFromExtracts = true;
            frm.CalledForConferences = true;
            frm.Show();
        }

        /// <summary>
        /// Create Partner By Commitment Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByCommitmentExtract(System.Object sender, EventArgs e)
        {
            TFrmPartnerByCommitmentExtract frm = new TFrmPartnerByCommitmentExtract(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Partner By Field Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePartnerByFieldExtract(System.Object sender, EventArgs e)
        {
            TFrmPartnerByField frm = new TFrmPartnerByField(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Recipient By Field Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateRecipientByFieldExtract(System.Object sender, EventArgs e)
        {
            TFrmRecipientByField frm = new TFrmRecipientByField(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Donor By Field Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDonorByFieldExtract(System.Object sender, EventArgs e)
        {
            TFrmDonorByField frm = new TFrmDonorByField(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Donor By Motivation Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDonorByMotivationExtract(System.Object sender, EventArgs e)
        {
            TFrmDonorByMotivation frm = new TFrmDonorByMotivation(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Donor By Amount Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDonorByAmountExtract(System.Object sender, EventArgs e)
        {
            TFrmDonorByAmount frm = new TFrmDonorByAmount(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// Create Donor By Miscellaneous Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDonorByMiscellaneousExtract(System.Object sender, EventArgs e)
        {
            TFrmDonorByMiscellaneous frm = new TFrmDonorByMiscellaneous(FindForm());

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        #endregion

        #region Simple tasks performed by user control

        /// <summary>
        /// Export partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportPartnersInExtract(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.ExportPartnersInExtract(sender, e);
        }

        /// <summary>
        /// Export partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportPartnersInExtractToPetra(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.ExportPartnersInExtractToPetra(sender, e);
        }

        /// <summary>
        /// Open screen to maintain contents of an extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaintainExtract(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.MaintainExtract(sender, e);
        }

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerifyAndUpdateExtract(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.VerifyAndUpdateExtract(sender, e);
        }

        /// <summary>
        /// Add subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSubscription(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.AddSubscription(sender, e);
        }

        /// <summary>
        /// Delete subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSubscription(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.DeleteSubscription(sender, e);
        }

        /// <summary>
        /// Change subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSubscription(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.ChangeSubscription(sender, e);
        }

        /// <summary>
        /// Add Contact Log record for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddContactLog(object sender, EventArgs e)
        {
            ucoExtractMasterList.AddContactLog(sender, e);
        }

        /// <summary>
        /// Add Partner Type for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPartnerType(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.AddPartnerType(sender, e);
        }

        /// <summary>
        /// Delete Partner Type for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePartnerType(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.DeletePartnerType(sender, e);
        }

        /// <summary>
        /// Update Solicitation Flag for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateSolicitationFlag(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.UpdateSolicitationFlag(sender, e);
        }

        /// <summary>
        /// Update Receipt Frequency for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateReceiptFrequency(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.UpdateReceiptFrequency(sender, e);
        }

        /// <summary>
        /// Update Email Gift Statement flag for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEmailGiftStatement(System.Object sender, EventArgs e)
        {
            ucoExtractMasterList.UpdateEmailGiftStatement(sender, e);
        }

        #endregion

        #endregion

        #region Keyboard and Filter/Find Menu

        /// ///////////  These methods just delegate to the user control to handle

        private void MniFilterFind_Click(object sender, EventArgs e)
        {
            ucoExtractMasterList.MniFilterFind_Click(sender, e);
        }

        /// <summary>
        /// Handler for shortcuts
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.S | Keys.Control))
            {
                if (FPetraUtilsObject.HasChanges)
                {
                    SaveChanges();
                }

                return true;
            }

            if (ucoExtractMasterList.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>The Partner Edit 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            return ucoExtractMasterList.ProcessFormsMessage(AFormsMessage);
        }
    }

    /// <summary>
    /// Manages the opening of a new Instance of the Extract Master screen.
    /// </summary>
    public static class TExtractMasterScreenManager
    {
        /// <summary>
        /// Opens an instance of the Extract Master screen.
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void OpenForm(Form AParentForm)
        {
            new TFrmExtractMaster(AParentForm).Show();
        }

        /// <summary>
        /// Opens an instance of the Extract Master screen but keep it hidden.
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void OpenFormHidden(Form AParentForm)
        {
            new TFrmExtractMaster(AParentForm).ShowInvisible(null);
        }
    }
}