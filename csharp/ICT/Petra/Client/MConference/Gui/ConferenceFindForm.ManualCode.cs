//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, peters
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MReporting.Gui.MPersonnel;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MConference.Gui
{
    /// <summary>
    /// Description of TFrmConferenceFindForm.ManualCode.
    /// </summary>
    public partial class TFrmConferenceFindForm
    {
        private String FSelectedConferenceName;
        private Int64 FSelectedConferenceKey;

        /// <summary>
        /// publish the selected conference name
        /// </summary>
        public String SelectedConferenceName
        {
            get
            {
                return FSelectedConferenceName;
            }
        }

        /// <summary>
        /// publish the selected conference key
        /// </summary>
        public Int64 SelectedConferenceKey
        {
            get
            {
                return FSelectedConferenceKey;
            }
        }

        private void InitGridManually()
        {
            LoadDataGrid(true);

            grdConferences.DoubleClickCell += new TDoubleClickCellEventHandler(grdConferences_DoubleClickCell);
            
            // Attempt to obtain conference key from parent form or parent's parent form and use this to focus the currently selected
            // conference in the grid. If no conference key is found then the first conference in the grid will be focused.
            Form MainWindow = FPetraUtilsObject.GetCallerForm();
            MethodInfo Method = MainWindow.GetType().GetMethod("GetSelectedConferenceKey");
            
            if (Method == null)
            {
                Method = MainWindow.GetType().GetMethod("GetPetraUtilsObject");
                
                if (Method != null)
                {
                    TFrmPetraUtils ParentPetraUtilsObject = (TFrmPetraUtils) Method.Invoke(MainWindow, null);
                    MainWindow = ParentPetraUtilsObject.GetCallerForm();
                    Method = MainWindow.GetType().GetMethod("GetSelectedConferenceKey");
                }
            }
            
            if (Method != null)
            {
                FSelectedConferenceKey = Convert.ToInt64(Method.Invoke(MainWindow, null));
                int RowPos = 1;

                foreach (DataRowView rowView in FMainDS.PcConference.DefaultView)
                {
                    PcConferenceRow Row = (PcConferenceRow) rowView.Row;
    
                    if (Row.ConferenceKey == FSelectedConferenceKey)
                    {
                        break;
                    }

                    RowPos++;
                }
            
                grdConferences.SelectRowInGrid(RowPos, true);
            }
        }

        void grdConferences_DoubleClickCell(object Sender, SourceGrid.CellContextEventArgs e)
        {
            Accept(e, null);
        }
        
        /// <summary>
        /// Create a new conference
        /// </summary>
        public void NewConference(System.Object sender, EventArgs e)
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();
            
            System.Int64 PartnerKey = 0;
            string PartnerShortName;
            String OutreachCode;
            
            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenEventFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenEventFindScreen.Invoke
                        ("",
                        out PartnerKey,
                        out PartnerShortName,
                        out OutreachCode,
                        MainWindow);
                    
                    // check if a conference already exists for chosen event
                    Boolean ConferenceExists = TRemote.MConference.Conference.WebConnectors.ConferenceExists(PartnerKey);
                    
                    if (PartnerKey != -1 && !ConferenceExists)
                    {
                        TRemote.MConference.Conference.WebConnectors.CreateNewConference(PartnerKey);
                        
                        FSelectedConferenceKey = PartnerKey;
                        FSelectedConferenceName = PartnerShortName;
                        
                        ReloadNavigation();
                    }
                    else if (PartnerKey != -1 && ConferenceExists)
                    {
                        MessageBox.Show(Catalog.GetString("This conference already exists"), Catalog.GetString("New Conference"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        NewConference(sender, e);
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling OpenEventFindScreen Delegate!", exp);
                }
            }
        }

        private void Accept(System.Object sender, EventArgs e)
        {
            if (grdConferences.SelectedDataRows.Length == 1)
            {
                FSelectedConferenceKey = (Int64)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PcConferenceTable.GetConferenceKeyDBName()];
                FSelectedConferenceName = (String)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()];
                
                ReloadNavigation();
            }
        }
        
        // reload navigation
        private void ReloadNavigation()
        {
            // update user defaults table
            TUserDefaults.SetDefault(TUserDefaults.CONFERENCE_LASTCONFERENCEWORKEDWITH, FSelectedConferenceKey);
                
            Form MainWindow = FPetraUtilsObject.GetCallerForm();
            MethodInfo method = MainWindow.GetType().GetMethod("LoadNavigationUI");

            if (method != null)
            {
                method.Invoke(MainWindow, new object[] { true });
            }
                
            method = MainWindow.GetType().GetMethod("SelectConferenceFolder");
                
            if (method != null)
            {
                method.Invoke(MainWindow, null);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Search(System.Object sender, EventArgs e)
        {
            LoadDataGrid(false);
        }

        private void LoadDataGrid(bool AFirstTime)
        {
            FMainDS.PcConference.Clear();
            FMainDS.PPartner.Clear();

            FMainDS.Merge(TRemote.MConference.WebConnectors.GetConferences(txtConference.Text, txtPrefix.Text));

            if (FMainDS.PcConference.Rows.Count == FMainDS.PPartner.Rows.Count)
            {
                if (AFirstTime)
                {
                    FMainDS.PcConference.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), Type.GetType("System.String"));
                    FMainDS.PcConference.DefaultView.AllowNew = false;
                }

                for (int Counter = 0; Counter < FMainDS.PcConference.Rows.Count; ++Counter)
                {
                    FMainDS.PcConference.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()] =
                        FMainDS.PPartner.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()];
                }
            }

            grdConferences.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PcConference.DefaultView);
        }
        
        // TODO validation is not completed
        // Delete functionality needs to be done manually. Automatic delete is not included for this template.
        private void RemoveRecord(System.Object sender, EventArgs e)
        {
            string CompletionMessage = String.Empty;

            if (grdConferences.SelectedDataRows.Length != 1)
            {
                return;
            }
            
            PcConferenceRow SelectedPcConferenceRow = (PcConferenceRow) (((DataRowView)grdConferences.SelectedDataRows[0]).Row);
                                           
            TVerificationResultCollection VerificationResults = null;

            if (!FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors)
            {
                this.Cursor = Cursors.WaitCursor;
                
                // TODO this does not work yet
                TRemote.MConference.Conference.WebConnectors.GetNonCacheableRecordReferenceCountManual(
                //TRemote.MConference.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
                    FMainDS.PcConference,
                    DataUtilities.GetPKValuesFromDataRow(SelectedPcConferenceRow),
                    out VerificationResults);
                
                
                this.Cursor = Cursors.Default;
            }

            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult(
                        Catalog.GetString("Record cannot be deleted!") +
                        Environment.NewLine +
                        Catalog.GetPluralString("Reason:", "Reasons:", VerificationResults.Count),
                        VerificationResults),
                        Catalog.GetString("Record Deletion"));
                return;
            }

            string DeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            DeletionQuestion += String.Format("{0}{0}({1})",
                    Environment.NewLine,
                    "Conference Key: " + FSelectedConferenceKey);

            bool AllowDeletion = true;
            bool DeletionPerformed = false;

            if(AllowDeletion)
            {
                if ((MessageBox.Show(DeletionQuestion,
                         Catalog.GetString("Confirm Delete"),
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
                {
                    try
                    {
                        SelectedPcConferenceRow.Delete();
                        DeletionPerformed = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(Catalog.GetString("An error occurred while deleting this record.{0}{0}{1}"),
                            Environment.NewLine, ex.Message),
                            Catalog.GetString("Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    if (DeletionPerformed)
                    {
                        // no save option to save on this screen so save automatically
                        SaveDelete();
                    }

                    // Clear any errors left over from  the deleted row
                    FPetraUtilsObject.VerificationResultCollection.Clear();
                }
            }

            if(DeletionPerformed && CompletionMessage.Length > 0)
            {
                MessageBox.Show(CompletionMessage,
                                 Catalog.GetString("Deletion Completed"));
            }
        }
        
        private bool SaveDelete()
        {
            bool ReturnValue = false;
            
            FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
            this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.PcConference.GetChangesTyped();

                if (SubmitDT == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TRemote.MCommon.DataReader.WebConnectors.SaveData(PcConferenceTable.GetTableDBName(), ref SubmitDT, out VerificationResult);
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgSecurityException(Exp, this.GetType());

                    ReturnValue = false;
                    //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (EDBConcurrencyException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                    ReturnValue = false;
                    //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    throw;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:
                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.PcConference.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        SubmitDT.AcceptChanges();
                        FMainDS.PcConference.Merge(SubmitDT, false);

                        // need to accept the new modification ID
                        FMainDS.PcConference.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        //FPetraUtilsObject.DisableSaveButton();

                        //SetPrimaryKeyReadOnly(true);

                        ReturnValue = true;
                        //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));

                        if((VerificationResult != null)
                            && (VerificationResult.HasCriticalOrNonCriticalErrors))
                        {
                            TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                this.GetType(), null);
                        }

                        break;

                    case TSubmitChangesResult.scrError:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                        TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                            this.GetType(), null);

                        //FPetraUtilsObject.SubmitChangesContinue = false;

                        ReturnValue = false;
                        //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);

                        // We don't have unsaved changes anymore
                        //FPetraUtilsObject.DisableSaveButton();

                        ReturnValue = true;
                        //FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
                
                return ReturnValue;
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
    /// </summary>
    public static class TConferenceFindScreenManager
    {
        /// <summary>
        /// Opens a Modal instance of the Conference Find screen.
        /// </summary>
        /// <param name="AConferenceNamePattern">Mathcing pattern for the conference name</param>
        /// <param name="AOutreachCodePattern">Matching patterns for the outreach code</param>
        /// <param name="AConferenceKey">Conference key of the found conference</param>
        /// <param name="AConferenceName">Partner ShortName name of the found conference</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if a conference was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String AConferenceNamePattern,
            String AOutreachCodePattern,
            out Int64 AConferenceKey,
            out String AConferenceName,
            Form AParentForm)
        {
            DialogResult dlgResult;

            AConferenceKey = -1;
            AConferenceName = String.Empty;

            TFrmConferenceFindForm FindConference = new TFrmConferenceFindForm(AParentForm);

            dlgResult = FindConference.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                AConferenceKey = FindConference.SelectedConferenceKey;
                AConferenceName = FindConference.SelectedConferenceName;

                return true;
            }

            return false;
        }
    }
}