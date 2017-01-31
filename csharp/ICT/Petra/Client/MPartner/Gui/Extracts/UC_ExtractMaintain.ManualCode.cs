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
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.DB.Exceptions;
using Ict.Common.Data.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TUC_ExtractMaintain
    {
        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        //private ExtractTDS FMainDS = null;

        //private MExtractRow FPreviouslySelectedDetailRow = null;

        // id of the extract that is displayed on this screen
        private int FExtractId;

        // name of the extract that is displayed on this screen
        private string FExtractName;

        private bool FFrozen = false;

        #region Properties
        /// <summary>
        /// id of extract displayed in this screen
        /// </summary>
        public int ExtractId
        {
            set
            {
                FExtractId = value;
            }
        }

        /// <summary>
        /// name of extract displayed in this screen
        /// </summary>
        public string ExtractName
        {
            set
            {
                FExtractName = value;
            }
            get
            {
                return FExtractName;
            }
        }

        /// <summary>
        /// True if extract is frozen and cannot be modifid. Hence disable Add and Delete.
        /// </summary>
        public bool Frozen
        {
            set
            {
                FFrozen = value;

                if (FFrozen)
                {
                    btnAdd.Enabled = false;
                    btnDeleteRow.Enabled = false;
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// initialize internal data before control is shown
        /// </summary>
        public void InitializeData()
        {
            // set fixed column widths as otherwise grid will spend a long time recalculating optimal width with big extracts
            grdDetails.Columns.Clear();
            grdDetails.AddPartnerKeyColumn("Partner Key", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerKeyDBName()] /*, 100*/);
            grdDetails.AddTextColumn("Class", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerClassDBName()] /*, 100*/);
            grdDetails.AddTextColumn("Partner Name", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerShortNameDBName()] /*, 300*/);
            grdDetails.AddTextColumn("Location Key", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetLocationKeyDBName()] /*, 100*/);

            LoadData();

            // get the grid columns to fill the available space
            grdDetails.AutoStretchColumnsToFitWidth = true;
            grdDetails.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch | SourceGrid.AutoSizeMode.EnableAutoSize;
            grdDetails.Rows.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            // allow multiselection of list items so several records can be deleted at once
            grdDetails.Selection.EnableMultiSelection = true;

            // initialize button state
            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
                UpdateRecordNumberDisplay();
            }
            else
            {
                btnEdit.Enabled = false;
            }

            grdDetails.AutoResizeGrid();
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
                foreach (DataRow InspectDR in FMainDS.MExtract.Rows)
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

                    //Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.MExtract.GetChangesTyped();
                    MExtractTable SubmitDT = new MExtractTable();
                    ExtractTDSMExtractTable ChangesDT = FMainDS.MExtract.GetChangesTyped();

                    if (ChangesDT != null)
                    {
                        SubmitDT.Merge(ChangesDT);
                    }
                    else
                    {
                        SubmitDT = null;
                    }

                    if (SubmitDT == null)
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
                        SubmissionResult = TRemote.MPartner.Partner.WebConnectors.SaveExtract
                                               (FExtractId, ref SubmitDT);
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
                            FMainDS.MExtract.AcceptChanges();

                            // Merge back with data from the Server (eg. for getting Sequence values)
                            FMainDS.MExtract.Merge(SubmitDT, false);

                            // need to accept the new modification ID
                            FMainDS.MExtract.AcceptChanges();

                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                            this.Cursor = Cursors.Default;

                            // TODO EnableSave(false);

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();

                            SetPrimaryKeyReadOnly(true);

                            // refresh extract master screen if it is open
                            TFormsMessage BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcExtractCreated);
                            BroadcastMessage.SetMessageDataName(ExtractName);
                            TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);
                            this.Focus(); // keeps the focus on the current form

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
        /// react to menu item / save button
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }

        /// <summary>
        /// Copy partner key of currenly selected partner to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyPartnerKeyToClipboard(System.Object sender, EventArgs e)
        {
            if (GetSelectedDetailRow() != null)
            {
                Clipboard.SetDataObject(GetSelectedDetailRow().PartnerKey.ToString("0000000000"));
            }
        }

        /// <summary>
        /// mark the selected partner record as the one last worked with in the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetPartnerLastWorkedWith(System.Object sender, EventArgs e)
        {
            ExtractTDSMExtractRow SelectedRow = GetSelectedDetailRow();
            TPartnerClass SelectedPartnersPartnerClass = SharedTypes.PartnerClassStringToEnum(SelectedRow.PartnerClass);

            if (SelectedRow != null)
            {
                TUserDefaults.NamedDefaults.SetLastPartnerWorkedWith(SelectedRow.PartnerKey,
                    TLastPartnerUse.lpuMailroomPartner, SelectedPartnersPartnerClass);

                TRemote.MPartner.Partner.WebConnectors.AddRecentlyUsedPartner
                    (SelectedRow.PartnerKey, SelectedPartnersPartnerClass, false, TLastPartnerUse.lpuMailroomPartner);
            }
        }

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VerifyAndUpdateExtract(System.Object sender, EventArgs e)
        {
            bool ChangesMade;
            bool ChangesNeeded;
            ExtractTDSMExtractTable ExtractTable = FMainDS.MExtract;

            TFrmExtractMaster.VerifyAndUpdateExtract(FindForm(), ref ExtractTable, out ChangesMade, out ChangesNeeded);

            if (ChangesMade)
            {
                FPetraUtilsObject.SetChangedFlag();

                MessageBox.Show(String.Format(Catalog.GetString("Verification and Update of Extract {0} was successful. \n\r" +
                            "Please press the Save button to save the changes."), FExtractName),
                    Catalog.GetString("Verify and Update Extract"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(String.Format(Catalog.GetString("Extract {0} was already up to date"), FExtractName),
                    Catalog.GetString("Verify and Update Extract"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Print the screen data using Word or Excel
        /// </summary>
        /// <param name="APrintUsing">The print application</param>
        /// <param name="APreviewOnly"></param>
        public void PrintFormData(TStandardFormPrint.TPrintUsing APrintUsing, bool APreviewOnly)
        {
            if (ValidateAllData(true, TErrorProcessingMode.Epm_All) && FPetraUtilsObject.IsDataSaved())
            {
                TStandardFormPrint.PrintGrid(APrintUsing, APreviewOnly, TModule.mPartner,
                    FPetraUtilsObject.GetForm().Text,
                    grdDetails,
                    new int[]
                    {
                        0, 2, 1, 3
                    },
                    new int[]
                    {
                        ExtractTDSMExtractTable.ColumnPartnerKeyId,
                        ExtractTDSMExtractTable.ColumnPartnerShortNameId,
                        ExtractTDSMExtractTable.ColumnPartnerClassId,
                        ExtractTDSMExtractTable.ColumnLocationKeyId
                    });
            }
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FMainDS = new ExtractTDS();

            // hide field for "created by". This was only introduced in the yaml file so the mechanism
            // with the template for controlMaintainTable works as in this case there is no entry fields
            // in pnlDetails that allow changes to the MExtract record
            txtCreatedBy.Visible = false;
            lblCreatedBy.Visible = false;
            ucoPartnerInfo.Dock = DockStyle.Fill;

            // set this property to false as otherwise save button will get enabled whenever values
            // in the partner info control change
            ucoPartnerInfo.CanBeHookedUpForValueChangedEvent = false;

            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.EditPartner);
            grdDetails.EnterKeyPressed += new TKeyPressedEventHandler(this.EditPartner);
        }

        /// <summary>
        /// Loads Extract Master Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadData()
        {
            Boolean ReturnValue;

            // Load Extract Headers, if not already loaded
            try
            {
                // Make sure that MasterTyped DataTables are already there at Client side
                if (FMainDS.MExtract == null)
                {
                    FMainDS.Tables.Add(new ExtractTDSMExtractTable());
                    FMainDS.InitVars();
                }

                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetExtractRowsWithPartnerData(FExtractId));

                // Make DataRows unchanged
                if (FMainDS.MExtract.Rows.Count > 0)
                {
                    FMainDS.MExtract.AcceptChanges();
                    FMainDS.AcceptChanges();
                }

                if (FMainDS.MExtract.Rows.Count != 0)
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

        private void ShowDetailsManual(ExtractTDSMExtractRow ARow)
        {
            ucoPartnerInfo.ClearControls();
            btnEdit.Enabled = false;

            if (ARow != null)
            {
                ucoPartnerInfo.PassPartnerDataNone(ARow.PartnerKey);
                btnEdit.Enabled = true;
            }

            btnDeleteRow.Enabled = pnlDetails.Enabled && !FFrozen;
        }

        private void EditPartner(System.Object sender, EventArgs e)
        {
            bool ServerCallSuccessful = false;
            bool VerifyPartnerAtLocationResult = false;

            if (CountSelectedRows() > 1)
            {
                MessageBox.Show(Catalog.GetString("Please select only one partner record that you want to edit"),
                    Catalog.GetString("Edit Partner"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            ExtractTDSMExtractRow SelectedRow = GetSelectedDetailRow();

            // TODO: private partners
            // Check if partner is has "restricted" field set to be private and in that
            // case only let the owner access that partner.
            // Make use of SharedConstants.PARTNER_PRIVATE_USER while running the query.

            // Open Partner Edit Screen for selected partner
            if (SelectedRow != null)
            {
                bool CurrentAddress;
                bool MailingAddress;

                this.Cursor = Cursors.WaitCursor;

                try
                {
                    Ict.Common.DB.TServerBusyHelper.CoordinatedAutoRetryCall("Extract Maintenance/Edit Partner", ref ServerCallSuccessful,
                        delegate
                        {
                            VerifyPartnerAtLocationResult = TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartnerAtLocation(
                                SelectedRow.PartnerKey,
                                new TLocationPK(SelectedRow.SiteKey, SelectedRow.LocationKey),
                                out CurrentAddress,
                                out MailingAddress);

                            ServerCallSuccessful = true;
                        });

                    if (!ServerCallSuccessful)
                    {
                        // ServerCallRetries must be equal to MAX_RETRIES when we get here!
                        if (TServerBusyHelperGui.ShowServerBusyDialogWhenOpeningForm(Catalog.GetString("Partner Edit")) == DialogResult.Retry)
                        {
                            EditPartner(null, null);
                        }

                        return;
                    }

                    TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                    if (!VerifyPartnerAtLocationResult)
                    {
                        MessageBox.Show(Catalog.GetString("Cannot find the location that was stored for this partner." +
                                "\r\n" + "Will use any known location for this partner." +
                                "\r\n" + "\r\n" + "(Fix with 'Verify and Update Extract')"),
                            Catalog.GetString("Edit Partner"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        frm.SetParameters(TScreenMode.smEdit,
                            SelectedRow.PartnerKey);
                    }
                    else
                    {
                        frm.SetParameters(TScreenMode.smEdit,
                            SelectedRow.PartnerKey,
                            SelectedRow.SiteKey,
                            SelectedRow.LocationKey);
                    }

                    frm.Show();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void AddPartner(System.Object sender, EventArgs e)
        {
            ExtractTDSMExtractRow NewRow;

            System.Int64 PartnerKey = 0;
            string PartnerShortName;
            TPartnerClass? PartnerClass;
            TPartnerClass PartnerClass2;
            TLocationPK ResultLocationPK;

            DataRow[] ExistingPartnerDataRows;
            ExtractTDSMExtractRow ExisitingPartnerRow;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenPartnerFindScreen.Invoke
                        ("",
                        out PartnerKey,
                        out PartnerShortName,
                        out PartnerClass,
                        out ResultLocationPK,
                        this.ParentForm);

                    if (PartnerKey != -1)
                    {
                        ExistingPartnerDataRows = FMainDS.MExtract.Select(ExtractTDSMExtractTable.GetPartnerKeyDBName() + " = " + PartnerKey.ToString());

                        if (ExistingPartnerDataRows.Length > 0)
                        {
                            // check if partner already exists in extract
                            MessageBox.Show(Catalog.GetString("A record for this partner already exists in this extract"),
                                Catalog.GetString("Add Partner to Extract"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // select the already existing partner record in the grid so the user can see it
                            ExisitingPartnerRow = (ExtractTDSMExtractRow)ExistingPartnerDataRows[0];
                            SelectByPartnerKey(PartnerKey, ExisitingPartnerRow.SiteKey);

                            return;
                        }

                        TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                            PartnerKey,
                            out PartnerShortName,
                            out PartnerClass2);

                        // add new record to extract
                        NewRow = FMainDS.MExtract.NewRowTyped();
                        NewRow.ExtractId = FExtractId;
                        NewRow.PartnerKey = PartnerKey;
                        NewRow.PartnerShortName = PartnerShortName;
                        NewRow.PartnerClass = SharedTypes.PartnerClassEnumToString(PartnerClass2);
                        NewRow.SiteKey = ResultLocationPK.SiteKey;
                        NewRow.LocationKey = ResultLocationPK.LocationKey;
                        FMainDS.MExtract.Rows.Add(NewRow);

                        // Refresh DataGrid to show the added partner record
                        grdDetails.Refresh();

                        // select the added partner record in the grid so the user can see the change
                        SelectByPartnerKey(PartnerKey, ResultLocationPK.SiteKey);

                        // enable save button on screen
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling PartnerFindScreen Delegate!", exp);
                }
                // end try
            }
        }

        private void DeleteRecord(System.Object sender, EventArgs e)
        {
            DeleteMExtract();
        }

        private bool PreDeleteManual(ExtractTDSMExtractRow ARowToDelete, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine,
                ARowToDelete.PartnerKey.ToString(),
                ARowToDelete.PartnerShortName);
            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ExtractTDSMExtractRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (grdDetails.Rows.Count <= 1)
            {
                // hide details part and disable buttons if no record in grid (first row for headings)
                btnEdit.Enabled = false;
            }
        }

        private void SelectByPartnerKey(Int64 APartnerKey, Int64 ASiteKey)
        {
            Int32 Index = -1;
            int ExtractIdTemplate;
            Int64 PartnerKeyTemplate;
            Int64 SiteKeyTemplate;

            for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
            {
                // compare key of item in list with key given
                ExtractIdTemplate = Convert.ToInt32((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][0]);
                PartnerKeyTemplate = Convert.ToInt64((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][1]);
                SiteKeyTemplate = Convert.ToInt64((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][2]);

                if ((FExtractId == ExtractIdTemplate)
                    && (APartnerKey == PartnerKeyTemplate)
                    && (ASiteKey == SiteKeyTemplate))
                {
                    Index = Counter + 1;
                    break;
                }
            }

            // reset grid selection as it is multiselect so the one record can be selected
            grdDetails.Selection.ResetSelection(true);

            grdDetails.SelectRowInGrid(Index, true);
            ShowDetails(Index);
        }

        /// <summary>
        /// return the number of rows selected in the grid
        /// </summary>
        /// <returns></returns>
        private int CountSelectedRows()
        {
            return grdDetails.Selection.GetSelectionRegion().GetRowsIndex().Length;
        }

        #endregion
    }
}
