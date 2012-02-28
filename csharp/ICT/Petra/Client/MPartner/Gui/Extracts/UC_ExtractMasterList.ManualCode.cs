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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TUC_ExtractMasterList
    {
        #region Public Methods

        /// <summary>
        /// save the changes on the screen (code is copied from auto-generated code)
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());
    
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
    
            // TODO: verification
    
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
                    TVerificationResultCollection VerificationResult;
    
                    Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.MExtractMaster.GetChangesTyped();
    
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
                        SubmissionResult = TRemote.MCommon.DataReader.SaveData(MExtractMasterTable.GetTableDBName(), ref SubmitDT, out VerificationResult);
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                            "No Server response",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        bool ReturnValue = false;
    
                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
    /* TODO ESecurityDBTableAccessDeniedException
    *                  catch (ESecurityDBTableAccessDeniedException Exp)
    *                  {
    *                      FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
    *                      this.Cursor = Cursors.Default;
    *                      // TODO TMessages.MsgSecurityException(Exp, this.GetType());
    *                      bool ReturnValue = false;
    *                      // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
    *                      return ReturnValue;
    *                  }
    */
                    catch (EDBConcurrencyException)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
    
                        // TODO TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                        bool ReturnValue = false;
    
                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }
                    catch (Exception exp)
                    {
                        FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                        this.Cursor = Cursors.Default;
                        TLogging.Log(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                            TLoggingType.ToLogfile);
                        MessageBox.Show(
                            "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                            "For details see the log file: " + TLogging.GetLogFileName(),
                            "Server connection error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
    
                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return false;
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
            if (GetSelectedDetailRow() != null)
            {
                TFrmExtractMaintain frm = new TFrmExtractMaintain(this.FindForm());
                frm.ExtractId = GetSelectedDetailRow().ExtractId;
                frm.Show();
            }
            
        }
        
        /// <summary>
        /// Delete the currently selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteRow(System.Object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete this value ({0}).\n\nDo you really want to delete it?"),
                        FPreviouslySelectedDetailRow.ExtractName), Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = CurrentRowIndex();

                // delete extract on the server (needs cascading delete)
                TRemote.MPartner.Partner.WebConnectors.DeleteExtract(FPreviouslySelectedDetailRow.ExtractId);

                // now delete it in the grid
                FPreviouslySelectedDetailRow.Delete();


                SelectByIndex(rowIndex);

                if (grdDetails.Rows.Count <= 1)
                {
                    // disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Open a new screen to show details and maintain the currently selected extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshExtractList(System.Object sender, EventArgs e)
        {
            // Do not allow refresh of the extract list if the user has made changes to any of the records
            // as otherwise their changes will be overwritten by reloading of the data.
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString(
                                    "Before refreshing the list you need to save changes made in this screen! " + "\r\n" + "\r\n" 
                                    + "If you don't want to save changes then please exit and reopen this screen."),
                                Catalog.GetString("Refresh List"),
                                MessageBoxButtons.OK);
                
            }
            else
            {
                this.LoadData();
            }
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FMainDS = new ExtractTDS();
            LoadData();

            // enable grid to react to insert and delete keyboard keys
            grdDetails.DeleteKeyPressed += new TKeyPressedEventHandler(grdDetails_DeleteKeyPressed);
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
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.MExtractMaster == null)
                {
                    FMainDS.Tables.Add(new MExtractMasterTable());
                    FMainDS.InitVars();
                }

                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetAllExtractHeaders());

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
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRow(this, null);
            }
        }
        
        #endregion
    }
}