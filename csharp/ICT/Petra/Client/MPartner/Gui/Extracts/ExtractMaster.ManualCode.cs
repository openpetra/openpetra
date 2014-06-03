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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
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

            // retrieve contents of extract from server
            ExtractTable = TRemote.MPartner.Partner.WebConnectors.GetExtractRowsWithPartnerData(AExtractId);

            VerifyAndUpdateExtract(AForm, ref ExtractTable, out ChangesMade);

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
                MessageBox.Show(Catalog.GetString("Extract was already up to date"),
                    Catalog.GetString("Verify and Update Extract"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // return if no changes were made
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

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        public static void VerifyAndUpdateExtract(System.Windows.Forms.Form AForm,
            ref ExtractTDSMExtractTable AExtractTable,
            out bool AChangesMade)
        {
            bool AddressExists;
            bool AddressNeitherCurrentNorMailing;
            bool ReplaceAddress;
            bool ReplaceAddressYesToAll = false;
            bool ReplaceAddressNoToAll = false;
            string CountryName;
            string EmailAddress;
            PLocationTable LocationTable;
            PLocationRow LocationRow;
            PPartnerLocationTable PartnerLocationTable;
            TFrmExtendedMessageBox MsgBox = new TFrmExtendedMessageBox(AForm);

            TFrmExtendedMessageBox.TResult MsgBoxResult;
            bool DontShowPartnerRemovePartnerKeyNonExistent = false;
            bool DontShowReplaceAddress = false;
            bool DontShowPartnerRemoveNoAddress = false;


            // initialize output parameter
            AChangesMade = false;

            // build a collection of objects to be deleted before actually deleting them (as otherwise
            // indexes may not be valid any longer)
            List <ExtractTDSMExtractRow>RowsToDelete = new List <ExtractTDSMExtractRow>();

            // prepare mouse cursor so user knows something is happening
            AForm.Cursor = Cursors.WaitCursor;

            // look at every single extract row
            foreach (ExtractTDSMExtractRow Row in AExtractTable.Rows)
            {
                // initialize for this row
                ReplaceAddress = false;

                // check if the partner record still exists, otherwise remove from extract
                if (!TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartner(Row.PartnerKey))
                {
                    if (!DontShowPartnerRemovePartnerKeyNonExistent)
                    {
                        MsgBox.ShowDialog(String.Format(Catalog.GetString("The following partner record does not exist any longer and " +
                                    "will therefore be removed from this extract: \n\r\n\r" +
                                    "{0} ({1})"), Row.PartnerShortName, Row.PartnerKey),
                            Catalog.GetString("Verify and Update Extract"),
                            Catalog.GetString("Don't show this message again"),
                            TFrmExtendedMessageBox.TButtons.embbOK,
                            TFrmExtendedMessageBox.TIcon.embiInformation);
                        MsgBoxResult = MsgBox.GetResult(out DontShowPartnerRemovePartnerKeyNonExistent);
                    }

                    RowsToDelete.Add(Row);
                }
                else
                {
                    AddressExists = TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartnerAtLocation
                                        (Row.PartnerKey, new TLocationPK(Row.SiteKey, Row.LocationKey), out AddressNeitherCurrentNorMailing);

                    if (!AddressExists)
                    {
                        if (!DontShowReplaceAddress)
                        {
                            MsgBox.ShowDialog(String.Format(Catalog.GetString("Address for {0} ({1}) in this extract no longer exists and " +
                                        "will therefore be replaced with a current address"),
                                    Row.PartnerShortName, Row.PartnerKey),
                                Catalog.GetString("Verify and Update Extract"),
                                Catalog.GetString("Don't show this message again"),
                                TFrmExtendedMessageBox.TButtons.embbOK,
                                TFrmExtendedMessageBox.TIcon.embiInformation);
                            MsgBoxResult = MsgBox.GetResult(out DontShowReplaceAddress);
                        }

                        ReplaceAddress = true;
                    }
                    else if (AddressNeitherCurrentNorMailing)
                    {
                        if (!ReplaceAddressYesToAll
                            && !ReplaceAddressNoToAll)
                        {
                            MsgBoxResult =
                                MsgBox.ShowDialog(String.Format(Catalog.GetString("Address for {0} ({1}) in this extract is not current. " +
                                            "Do you want to update it with a current address if there is one?"),
                                        Row.PartnerShortName, Row.PartnerKey),
                                    Catalog.GetString("Verify and Update Extract"),
                                    "",
                                    TFrmExtendedMessageBox.TButtons.embbYesYesToAllNoNoToAll,
                                    TFrmExtendedMessageBox.TIcon.embiQuestion);

                            if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYesToAll)
                            {
                                ReplaceAddressYesToAll = true;
                            }
                            else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrYes)
                            {
                                ReplaceAddress = true;
                            }
                            else if (MsgBoxResult == TFrmExtendedMessageBox.TResult.embrNoToAll)
                            {
                                ReplaceAddressNoToAll = true;
                            }
                        }

                        // need to set the flag each time we come through here.
                        if (ReplaceAddressYesToAll)
                        {
                            ReplaceAddress = true;
                        }
                    }

                    if (ReplaceAddress)
                    {
                        if (!TRemote.MPartner.Mailing.WebConnectors.GetBestAddress(Row.PartnerKey,
                                out LocationTable, out PartnerLocationTable, out CountryName, out EmailAddress))
                        {
                            // in this case there is no address at all for this partner (should not really happen)
                            if (!DontShowPartnerRemoveNoAddress)
                            {
                                MsgBox.ShowDialog(String.Format(Catalog.GetString("No address could be found for {0} ({1}). " +
                                            "Therefore the partner record will be removed from this extract"),
                                        Row.PartnerShortName, Row.PartnerKey),
                                    Catalog.GetString("Verify and Update Extract"),
                                    Catalog.GetString("Don't show this message again"),
                                    TFrmExtendedMessageBox.TButtons.embbOK,
                                    TFrmExtendedMessageBox.TIcon.embiInformation);
                                MsgBoxResult = MsgBox.GetResult(out DontShowPartnerRemoveNoAddress);
                            }

                            RowsToDelete.Add(Row);
                        }
                        else
                        {
                            if (LocationTable.Rows.Count > 0)
                            {
                                LocationRow = (PLocationRow)LocationTable.Rows[0];

                                /* it could be that GetBestAddress still returns a non-current address if
                                 * there is no better one */
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
            }

            // now delete the actual rows
            foreach (ExtractTDSMExtractRow Row in RowsToDelete)
            {
                AChangesMade = true;
                Row.Delete();
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

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        public void RefreshExtractList()
        {
            // Do not allow refresh of the extract list if the user has made changes to any of the records
            // as otherwise their changes will be overwritten by reloading of the data.
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString(
                        "Before refreshing the list you need to save changes made in this screen! " + "\r\n" + "\r\n" +
                        "If you don't want to save changes then please exit and reopen this screen."),
                    Catalog.GetString("Refresh List"),
                    MessageBoxButtons.OK);
            }
            else
            {
                ucoExtractMasterList.RefreshExtractList("", true, "", "");
            }
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
            TFrmExtractCombineIntersectSubtractDialog ExtractCombineDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);

            List <Int32>ACombineExtractIdList;

            // initialize dialog
            ExtractCombineDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisCombineMode);

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

                ExtractNameDialog.Dispose();

                if (TRemote.MPartner.Partner.WebConnectors.CombineExtracts
                        (NewExtractName, NewExtractDescription, ACombineExtractIdList,
                        out NewExtractId))
                {
                    ucoExtractMasterList.RefreshExtractList(sender, e);
                }
            }

            ExtractCombineDialog.Dispose();
        }

        /// <summary>
        /// Intersect Extracts (open a screen to select extracts to intersect)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntersectExtracts(System.Object sender, EventArgs e)
        {
            TFrmExtractCombineIntersectSubtractDialog ExtractIntersectDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);

            List <Int32>AIntersectExtractIdList;

            // initialize dialog
            ExtractIntersectDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisIntersectMode);

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

                ExtractNameDialog.Dispose();

                if (TRemote.MPartner.Partner.WebConnectors.IntersectExtracts
                        (NewExtractName, NewExtractDescription, AIntersectExtractIdList,
                        out NewExtractId))
                {
                    ucoExtractMasterList.RefreshExtractList(sender, e);
                }
            }

            ExtractIntersectDialog.Dispose();
        }

        /// <summary>
        /// Subtract Extracts (open a screen to select extracts to subtract from each other)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubtractExtracts(System.Object sender, EventArgs e)
        {
            TFrmExtractCombineIntersectSubtractDialog ExtractSubtractDialog = new TFrmExtractCombineIntersectSubtractDialog(this.ParentForm);
            String BaseExtractName;

            List <Int32>AIntersectExtractIdList;

            // initialize dialog
            ExtractSubtractDialog.SetMode(TFrmExtractCombineIntersectSubtractDialog.TMode.ecisSubtractMode);

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

                ExtractNameDialog.Dispose();

                if (TRemote.MPartner.Partner.WebConnectors.SubtractExtracts
                        (NewExtractName, NewExtractDescription, BaseExtractName, AIntersectExtractIdList,
                        out NewExtractId))
                {
                    ucoExtractMasterList.RefreshExtractList(sender, e);
                }
            }

            ExtractSubtractDialog.Dispose();
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
            TPartnerExtractsMain.FamilyMembersExtract(FindForm());
        }

        /// <summary>
        /// Create Manual Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFamilyExtractForPersons(System.Object sender, EventArgs e)
        {
            TPartnerExtractsMain.FamilyExtractForPersons(FindForm());
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
    }
}