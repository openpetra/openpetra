//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TUC_ExtractMasterList
    {
        /// <summary>
        /// Delegate for call to parent window to trigger refreshing of extract list
        /// (needed here as filter criteria exist in parent and are unknown in this object)
        /// </summary>
        public delegate bool TDelegateRefreshExtractList();

        /// <summary>
        /// Reference to the Delegate in parent window
        /// </summary>
        private TDelegateRefreshExtractList FDelegateRefreshExtractList;

        #region Public Methods
        /// <summary>
        /// This property is used to provide a function which is called when refresh button is clicked
        /// </summary>
        /// <description></description>
        public TDelegateRefreshExtractList DelegateRefreshExtractList
        {
            set
            {
                FDelegateRefreshExtractList = value;
            }
        }

        /// <summary>
        /// export all partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ExportPartnersInExtract(System.Object sender, EventArgs e)
        {
            Boolean Result = false;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Export Partners in Extract"))
                && (GetSelectedDetailRow() != null))
            {
                String FileName = TImportExportDialogs.GetExportFilename(Catalog.GetString("Save Partners into File"));

                if (FileName.Length > 0)
                {
                    if (FileName.EndsWith("ext"))
                    {
                        String doc = TRemote.MPartner.ImportExport.WebConnectors.ExportExtractPartnersExt(GetSelectedDetailRow().ExtractId, false);
                        Result = TImportExportDialogs.ExportTofile(doc, FileName);

                        if (!Result)
                        {
                            MessageBox.Show(Catalog.GetString("Export of Partners in Extract failed!"), Catalog.GetString(
                                    "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(Catalog.GetString("Export of Partners in Extract finished"), Catalog.GetString(
                                    "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // XmlDocument doc = new XmlDocument();
                        MessageBox.Show(Catalog.GetString("Export with this format is not yet supported!"), Catalog.GetString(
                                "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // doc.LoadXml(TRemote.MPartner.ImportExport.WebConnectors.ExportExtractPartners(GetSelectedDetailRow().ExtractId, false));
                        // Result = TImportExportDialogs.ExportTofile(doc, FileName);
                    }
                }

                return Result;
            }

            return false;
        }

        /// <summary>
        /// save the changes on the screen (code is copied from auto-generated code)
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            bool ReturnValue = false;

            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

            if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
            {
                foreach (DataRow InspectDR in FMainDS.MExtractMaster.Rows)
                {
                    InspectDR.EndEdit();
                }

                if (!FPetraUtilsObject.HasChanges)
                {
                    return true;
                }
                else
                {
                    FPetraUtilsObject.WriteToStatusBar("Saving data...");
                    this.Cursor = Cursors.WaitCursor;

                    TSubmitChangesResult SubmissionResult;

                    MExtractMasterTable SubmitDT = new MExtractMasterTable();

                    if (FMainDS.MExtractMaster.GetChangesTyped() != null)
                    {
                        SubmitDT.Merge(FMainDS.MExtractMaster.GetChangesTyped());
                    }
                    else
                    {
                        // There is nothing to be saved.
                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(Catalog.GetString("There is nothing to be saved."));
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        return true;
                    }

                    // Submit changes to the PETRAServer
                    try
                    {
                        SubmissionResult = TRemote.MPartner.Partner.WebConnectors.SaveExtractMaster
                                               (ref SubmitDT);
                    }
                    catch (ESecurityDBTableAccessDeniedException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        TMessages.MsgSecurityException(Exp, this.GetType());

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (EDBConcurrencyException Exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (Exception)
                    {
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                        this.Cursor = Cursors.Default;

                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        throw;
                    }

                    switch (SubmissionResult)
                    {
                        case TSubmitChangesResult.scrOK:

                            // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                            FMainDS.MExtractMaster.AcceptChanges();

                            // Merge back with data from the Server (eg. for getting Sequence values)
                            FMainDS.MExtractMaster.Merge(SubmitDT, false);

                            // need to accept the new modification ID
                            FMainDS.MExtractMaster.AcceptChanges();

                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                            this.Cursor = Cursors.Default;

                            // TODO EnableSave(false);

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();

                            SetPrimaryKeyReadOnly(true);

                            // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            return true;

                        case TSubmitChangesResult.scrError:

                            // TODO scrError
                            this.Cursor = Cursors.Default;
                            break;

                        case TSubmitChangesResult.scrNothingToBeSaved:

                            // TODO scrNothingToBeSaved
                            this.Cursor = Cursors.Default;
                            return true;

                        case TSubmitChangesResult.scrInfoNeeded:

                            // TODO scrInfoNeeded
                            this.Cursor = Cursors.Default;
                            break;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MaintainExtract(System.Object sender, EventArgs e)
        {
            if (!WarnIfNotSingleSelection(Catalog.GetString("Maintain Extract"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmExtractMaintain frm = new TFrmExtractMaintain(this.FindForm());
                frm.ExtractId = GetSelectedDetailRow().ExtractId;
                frm.ExtractName = GetSelectedDetailRow().ExtractName;
                frm.Show();
            }
        }

        private bool PreDeleteManual(MExtractMasterRow ARowToDelete, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine,
                lblExtractName.Text,
                txtExtractName.Text);
            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(MExtractMasterRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            UpdateButtonStatus();
        }

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshExtractList(System.Object sender, EventArgs e)
        {
            FDelegateRefreshExtractList();
        }

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        /// <param name="AExtractNameFilter"></param>
        /// <param name="AAllUsers"></param>
        /// <param name="ACreatedByUser"></param>
        /// <param name="AModifiedByUser"></param>
        public void RefreshExtractList(String AExtractNameFilter, Boolean AAllUsers,
            String ACreatedByUser, String AModifiedByUser)
        {
            // only react here if screen is initialized
            if (FPetraUtilsObject != null)
            {
                this.LoadData(AExtractNameFilter, AAllUsers, ACreatedByUser, AModifiedByUser);

                // data can have changed completely, so easiest for now is to select first row
                grdDetails.SelectRowInGrid(1, true);
                ShowDetails(1);

                // enable/disable buttons
                UpdateButtonStatus();
            }
        }

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VerifyAndUpdateExtract(System.Object sender, EventArgs e)
        {
            if (!WarnIfNotSingleSelection(Catalog.GetString("Verify and Update Extract"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmExtractMaster.VerifyAndUpdateExtract(FindForm(), GetSelectedDetailRow().ExtractId);
            }
        }

        /// <summary>
        /// Add subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddSubscription(System.Object sender, EventArgs e)
        {
            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();
            PSubscriptionRow SubscriptionRow = SubscriptionTable.NewRowTyped();
            PPartnerTable PartnersWithExistingSubs = new PPartnerTable();
            int SubscriptionsAdded;
            String MessageText;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Add Subscription"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractAddSubscriptionDialog dialog = new TFrmUpdateExtractAddSubscriptionDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(ref SubscriptionRow);
                    SubscriptionTable.Rows.Add(SubscriptionRow);

                    // perform update of extract data on server side
                    TRemote.MPartner.Partner.WebConnectors.AddSubscription
                        (GetSelectedDetailRow().ExtractId, ref SubscriptionTable, out PartnersWithExistingSubs, out SubscriptionsAdded);

                    MessageText =
                        String.Format(Catalog.GetString(
                                "Subscription {0} successfully added for {1} out of {2} Partner(s) in Extract {3}."),
                            SubscriptionRow.PublicationCode,
                            SubscriptionsAdded, GetSelectedDetailRow().KeyCount, GetSelectedDetailRow().ExtractName);

                    if (PartnersWithExistingSubs.Rows.Count > 0)
                    {
                        MessageText += "\r\n\r\n" +
                                       String.Format(Catalog.GetString(
                                "See the following Dialog for the {0} Partner(s) that were already subscribed for this Publication. The Subscription was not added for those Partners."),
                            PartnersWithExistingSubs.Rows.Count);
                    }

                    MessageBox.Show(MessageText,
                        Catalog.GetString("Add Subscription"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (PartnersWithExistingSubs.Rows.Count > 0)
                    {
                        TFrmSimplePartnerListDialog partnerDialog = new TFrmSimplePartnerListDialog(this.FindForm());
                        partnerDialog.SetExplanation("These partners already have a Subscription for " + SubscriptionRow.PublicationCode,
                            "The Subscription was not added to the following Partners:");
                        partnerDialog.SetPartnerList(PartnersWithExistingSubs);
                        partnerDialog.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Delete subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteSubscription(System.Object sender, EventArgs e)
        {
            String PublicationCode;
            Boolean DeleteAllSubscriptions = false;
            Boolean DeleteThisSubscription = false;
            Boolean AllDeletionsSucceeded = true;
            int CountDeleted = 0;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Delete Subscription"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractDeleteSubscriptionDialog dialog = new TFrmUpdateExtractDeleteSubscriptionDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(out PublicationCode);

                    ExtractTDSMExtractTable ExtractTable;

                    // retrieve all partners of extract from server
                    ExtractTable = TRemote.MPartner.Partner.WebConnectors.GetExtractRowsWithPartnerData(GetSelectedDetailRow().ExtractId);

                    foreach (ExtractTDSMExtractRow Row in ExtractTable.Rows)
                    {
                        if (TRemote.MPartner.Partner.WebConnectors.SubscriptionExists
                                (Row.PartnerKey, PublicationCode))
                        {
                            DeleteThisSubscription = false;

                            if (!DeleteAllSubscriptions)
                            {
                                TFrmExtendedMessageBox.TResult Result;
                                TFrmExtendedMessageBox ExtMsgBox = new TFrmExtendedMessageBox(this.FindForm());
                                Result = ExtMsgBox.ShowDialog(Catalog.GetString("You have chosen to delete the subscription of ") +
                                    PublicationCode + "\r\n" +
                                    Catalog.GetString("for Partner ") +
                                    Row.PartnerShortName + " (" + Row.PartnerKey + ")\r\n\r\n" +
                                    Catalog.GetString("Do you really want to delete it?"),
                                    Catalog.GetString("Delete Subscription"),
                                    "",
                                    TFrmExtendedMessageBox.TButtons.embbYesYesToAllNoCancel,
                                    TFrmExtendedMessageBox.TIcon.embiQuestion);

                                switch (Result)
                                {
                                    case TFrmExtendedMessageBox.TResult.embrYesToAll:
                                        DeleteAllSubscriptions = true;
                                        break;

                                    case TFrmExtendedMessageBox.TResult.embrYes:
                                        DeleteThisSubscription = true;
                                        break;

                                    case TFrmExtendedMessageBox.TResult.embrNo:
                                        DeleteThisSubscription = false;
                                        break;

                                    case TFrmExtendedMessageBox.TResult.embrCancel:
                                        MessageBox.Show(Catalog.GetString("Further deletion of Subscriptions cancelled"),
                                        Catalog.GetString("Delete Subscription"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                        return;

                                    default:
                                        break;
                                }
                            }

                            if (DeleteAllSubscriptions
                                || DeleteThisSubscription)
                            {
                                if (!TRemote.MPartner.Partner.WebConnectors.DeleteSubscription
                                        (GetSelectedDetailRow().ExtractId, Row.PartnerKey, PublicationCode))
                                {
                                    MessageBox.Show(Catalog.GetString("Error while deleting Subscription ") +
                                        PublicationCode + Catalog.GetString(" for Partner ") +
                                        Row.PartnerShortName + " (" + Row.PartnerKey + ")",
                                        Catalog.GetString("Delete Subscription"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    AllDeletionsSucceeded = false;
                                }
                                else
                                {
                                    CountDeleted++;
                                }
                            }
                        }
                    }

                    if (AllDeletionsSucceeded)
                    {
                        MessageBox.Show(String.Format(Catalog.GetString("Subscription {0} successfully deleted for {1} Partners in Extract {2}"),
                                PublicationCode, CountDeleted, GetSelectedDetailRow().ExtractName),
                            Catalog.GetString("Delete Subscription"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(String.Format(Catalog.GetString(
                                    "Error while deleting Subscription {0} for some Partners in Extract {1}. Subscription deleted for {2} Partners."),
                                PublicationCode, GetSelectedDetailRow().ExtractName, CountDeleted),
                            Catalog.GetString("Delete Subscription"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Change subscription for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeSubscription(System.Object sender, EventArgs e)
        {
            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();
            PSubscriptionRow SubscriptionRow = SubscriptionTable.NewRowTyped();
            PPartnerTable PartnersWithoutSubs = new PPartnerTable();
            int SubscriptionsChanged;
            String MessageText;

            List <String>FieldsToChange = new List <string>();

            if (!WarnIfNotSingleSelection(Catalog.GetString("Add Subscription"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractChangeSubscriptionDialog dialog = new TFrmUpdateExtractChangeSubscriptionDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dialog.GetReturnedParameters(ref SubscriptionRow, ref FieldsToChange))
                    {
                        SubscriptionTable.Rows.Add(SubscriptionRow);

                        // perform update of extract data on server side
                        TRemote.MPartner.Partner.WebConnectors.ChangeSubscription
                            (GetSelectedDetailRow().ExtractId, ref SubscriptionTable, FieldsToChange, out PartnersWithoutSubs,
                            out SubscriptionsChanged);

                        MessageText =
                            String.Format(Catalog.GetString(
                                    "Subscription {0} successfully changed for {1} out of {2} Partner(s) in Extract {3}."),
                                SubscriptionRow.PublicationCode,
                                SubscriptionsChanged, GetSelectedDetailRow().KeyCount, GetSelectedDetailRow().ExtractName);

                        if (PartnersWithoutSubs.Rows.Count > 0)
                        {
                            MessageText += "\r\n\r\n" +
                                           String.Format(Catalog.GetString(
                                    "See the following Dialog for the {0} Partner(s) that are not subscribed for this Publication and therefore no change was made for them."),
                                PartnersWithoutSubs.Rows.Count);
                        }

                        MessageBox.Show(MessageText,
                            Catalog.GetString("Change Subscription"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        if (PartnersWithoutSubs.Rows.Count > 0)
                        {
                            TFrmSimplePartnerListDialog partnerDialog = new TFrmSimplePartnerListDialog(this.FindForm());
                            partnerDialog.SetExplanation("These partners do not have a Subscription for " + SubscriptionRow.PublicationCode,
                                "The Subscription was therefore not changed for the following Partners:");
                            partnerDialog.SetPartnerList(PartnersWithoutSubs);
                            partnerDialog.ShowDialog();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add Contact Log record for Partners in selected Extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddContactLog(object sender, EventArgs e)
        {
            PContactLogTable ContactLogTable = new PContactLogTable();
            PContactLogRow ContactLogRow = ContactLogTable.NewRowTyped();

            string MessageText;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Add Contact Log"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractAddContactLogDialog dialog = new TFrmUpdateExtractAddContactLogDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(ref ContactLogRow);
                    ContactLogTable.Rows.Add(ContactLogRow);
                    // perform update of extract data on server side
                    TRemote.MPartner.Partner.WebConnectors.AddContactLog(
                        GetSelectedDetailRow().ExtractId, ref ContactLogTable);

                    MessageText =
                        string.Format(Catalog.GetString(
                                "Contact Log {0} - {1} successfully added for {2} Partner(s) in Extract {3}."),
                            ContactLogRow.ContactCode, ContactLogRow.ContactDate, GetSelectedDetailRow().KeyCount, GetSelectedDetailRow().ExtractName);

                    MessageBox.Show(MessageText,
                        Catalog.GetString("Add Contact Log"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Add Partner Type for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddPartnerType(System.Object sender, EventArgs e)
        {
            String TypeCode;
            String Message;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Add Partner Type"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractPartnerTypeDialog dialog = new TFrmUpdateExtractPartnerTypeDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);
                dialog.SetMode(true);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dialog.GetReturnedParameters(out TypeCode))
                    {
                        // perform update of extract data on server side
                        TRemote.MPartner.Partner.WebConnectors.UpdatePartnerType
                            (GetSelectedDetailRow().ExtractId, true, TypeCode);

                        Message = String.Format(Catalog.GetString("Partner Type {0} successfully added for all Partners in Extract {1}"),
                            TypeCode, GetSelectedDetailRow().ExtractName);

                        MessageBox.Show(Message,
                            Catalog.GetString("Add Partner Type"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Delete Partner Type for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeletePartnerType(System.Object sender, EventArgs e)
        {
            String TypeCode;
            String Message;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Delete Partner Type"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractPartnerTypeDialog dialog = new TFrmUpdateExtractPartnerTypeDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);
                dialog.SetMode(false);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dialog.GetReturnedParameters(out TypeCode))
                    {
                        // perform update of extract data on server side
                        TRemote.MPartner.Partner.WebConnectors.UpdatePartnerType
                            (GetSelectedDetailRow().ExtractId, false, TypeCode);

                        Message = String.Format(Catalog.GetString("Partner Type {0} successfully deleted for all Partners in Extract {1}"),
                            TypeCode, GetSelectedDetailRow().ExtractName);

                        MessageBox.Show(Message,
                            Catalog.GetString("Delete Partner Type"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Update 'No Solicitations' flag for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateSolicitationFlag(System.Object sender, EventArgs e)
        {
            bool NoSolicitations;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Update 'No Solicitations' Flag"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractSolicitationFlagDialog dialog = new TFrmUpdateExtractSolicitationFlagDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(out NoSolicitations);

                    // perform update of extract data on server side
                    if (TRemote.MPartner.Partner.WebConnectors.UpdateSolicitationFlag
                            (GetSelectedDetailRow().ExtractId, NoSolicitations))
                    {
                        MessageBox.Show(Catalog.GetString("'No Solicitations' flag successfully updated for all Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update 'No Solicitations' Flag"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Error while updating 'No Solicitations' flag for Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update 'No Solicitations' Flag"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Update Receipt Frequency for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateReceiptFrequency(System.Object sender, EventArgs e)
        {
            bool UpdateReceiptLetterFrequency;
            String ReceiptLetterFrequency;
            bool UpdateReceiptEachGift;
            bool ReceiptEachGift;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Update Receipt Frequency"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractReceiptFrequencyDialog dialog = new TFrmUpdateExtractReceiptFrequencyDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(out UpdateReceiptLetterFrequency, out ReceiptLetterFrequency,
                        out UpdateReceiptEachGift, out ReceiptEachGift);

                    // perform update of extract data on server side
                    if (TRemote.MPartner.Partner.WebConnectors.UpdateReceiptFrequency
                            (GetSelectedDetailRow().ExtractId, UpdateReceiptLetterFrequency,
                            ReceiptLetterFrequency, UpdateReceiptEachGift, ReceiptEachGift))
                    {
                        MessageBox.Show(Catalog.GetString("Receipt Frequency successfully updated for all Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update Receipt Frequency"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Error while updating Receipt Frequency for Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update Receipt Frequency"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Update Email Gift Statement flag for Partners in selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateEmailGiftStatement(System.Object sender, EventArgs e)
        {
            bool EmailGiftStatement;

            if (!WarnIfNotSingleSelection(Catalog.GetString("Update Email Gift Statement"))
                && (GetSelectedDetailRow() != null))
            {
                TFrmUpdateExtractEmailGiftStatementDialog dialog = new TFrmUpdateExtractEmailGiftStatementDialog(this.FindForm());
                dialog.SetExtractName(GetSelectedDetailRow().ExtractName);

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dialog.GetReturnedParameters(out EmailGiftStatement);

                    // perform update of extract data on server side
                    if (TRemote.MPartner.Partner.WebConnectors.UpdateEmailGiftStatement
                            (GetSelectedDetailRow().ExtractId, EmailGiftStatement))
                    {
                        MessageBox.Show(Catalog.GetString("Email Gift Statement successfully updated for all Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update Email Gift Statement"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Error while updating Email Gift Statement for Partners in Extract ") +
                            GetSelectedDetailRow().ExtractName,
                            Catalog.GetString("Update Email Gift Statement"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FMainDS = new ExtractTDS();
            LoadData("", true, "", "");

            // allow multiselection of list items so several records can be deleted at once
            grdDetails.Selection.EnableMultiSelection = true;

            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.MaintainExtract);
            grdDetails.EnterKeyPressed += new TKeyPressedEventHandler(this.MaintainExtract);
        }

        /// <summary>
        /// Loads Extract Master Data from Petra Server into FMainDS.
        /// </summary>
        /// <param name="AExtractNameFilter"></param>
        /// <param name="AAllUsers"></param>
        /// <param name="ACreatedByUser"></param>
        /// <param name="AModifiedByUser"></param>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadData(String AExtractNameFilter, Boolean AAllUsers,
            String ACreatedByUser, String AModifiedByUser)
        {
            Boolean ReturnValue;

            // Load Extract Headers, if not already loaded
            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.MExtractMaster == null)
                {
                    FMainDS.Tables.Add(new MExtractMasterTable());
                    FMainDS.InitVars();
                }
                else
                {
                    // clear table so a load also works if records on the server have been removed
                    FMainDS.MExtractMaster.Clear();
                }

                // add filter data
                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetAllExtractHeaders(AExtractNameFilter,
                        AAllUsers, ACreatedByUser, AModifiedByUser));

                // Make DataRows unchanged
                if (FMainDS.MExtractMaster.Rows.Count > 0)
                {
                    FMainDS.MExtractMaster.AcceptChanges();
                    FMainDS.AcceptChanges();
                }

                if (FMainDS.MExtractMaster.Rows.Count != 0)
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
        ///
        /// </summary>
        private void ShowDataManual()
        {
            // enable/disable buttons
            UpdateButtonStatus();
        }

        /// <summary>
        /// return the number of rows selected in the grid
        /// </summary>
        /// <returns></returns>
        private int CountSelectedRows()
        {
            return grdDetails.Selection.GetSelectionRegion().GetRowsIndex().Length;
        }

        /// <summary>
        /// update button status according to number of list
        /// </summary>
        private void UpdateButtonStatus()
        {
            if (grdDetails.Rows.Count <= 1)
            {
                // hide details part and disable buttons if no record in grid (first row for headings)
                btnMaintain.Enabled = false;
            }
            else
            {
                btnMaintain.Enabled = true;
            }
        }

        /// <summary>
        /// show a warning to the user if there is no or more than one item selected
        /// </summary>
        /// <returns>true if more or less than one record is selected</returns>
        private bool WarnIfNotSingleSelection(string AAction)
        {
            if ((grdDetails.Selection.GetSelectionRegion().GetRowsIndex().Length < 1)
                || (grdDetails.Selection.GetSelectionRegion().GetRowsIndex().Length > 1))
            {
                MessageBox.Show(Catalog.GetString("Please select the one Extract that you want to use for this action: ") + AAction,
                    AAction,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Forms Messaging Interface Implementation

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
            bool MessageProcessed = false;

            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcExtractCreated)
            {
                FPetraUtilsObject.GetForm().BringToFront();

                if (FDelegateRefreshExtractList())
                {
                    // this is required as extracts are created on a different thread
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker) delegate()
                            {
                                // show filter panel
                                MniFilterFind_Click(GetPetraUtilsObject().GetForm(), null);
                                // show the screen in case it has been hidden
                                FPetraUtilsObject.GetForm().Show();
                                // filter results to show the new extract
                                ((TextBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("txtExtractName")).Text =
                                    ((TFormsMessage.FormsMessageName)AFormsMessage.MessageObject).Name;
                            });
                    }
                }

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}
