/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.UIConnectors;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner.Gui;

namespace Ict.Petra.Client.MFinance.Gui
{
    public partial class TFrmAccountsPayableEditSupplier
    {
        AccountsPayableTDS FMainDS;

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitializeManualCode()
        {
            FMainDS = new AccountsPayableTDS();
        }

        /// <summary>
        /// called from APMain when adding new supplier;
        /// initialises a new dataset
        /// </summary>
        /// <param name="APartnerKey"></param>
        public void CreateNewSupplier(Int64 APartnerKey)
        {
            FPetraUtilsObject.SetChangedFlag();

            // check for existing supplier record
            if (FUIConnector.CanFindSupplier(APartnerKey))
            {
                MessageBox.Show(Catalog.GetString("There is already a supplier record for this partner!"));
                EditSupplier(APartnerKey);
                return;
            }

            AApSupplierRow row = FMainDS.AApSupplier.NewRowTyped();
            row.PartnerKey = APartnerKey;

            // TODO: use currency code from ledger
            // TODO: verification: don't store with currency NULL value
            row.CurrencyCode = "EUR";
            FMainDS.AApSupplier.Rows.Add(row);

            ShowData();
        }

        /// <summary>
        /// edit an existing supplier
        /// </summary>
        /// <param name="APartnerKey"></param>
        public void EditSupplier(Int64 APartnerKey)
        {
            FMainDS = FUIConnector.GetData(APartnerKey);
            ShowData();
        }

        /// <summary>
        /// open the Partner Edit screen for the supplier
        /// </summary>
        private void EditPartner(object sender, EventArgs e)
        {
            FPetraUtilsObject.WriteToStatusBar("Opening Partner in Partner Edit screen...");
            this.Cursor = Cursors.WaitCursor;

            try
            {
                TPartnerEditDSWinForm frm = new TPartnerEditDSWinForm(this.Handle);
                frm.SetParameters(TScreenMode.smEdit, FMainDS.AApSupplier[0].PartnerKey);
                frm.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// displays the data from the local datatable
        /// </summary>
        private void ShowDataManual()
        {
        }

        /// <summary>
        /// get the data from the controls into the dataset
        /// </summary>
        private void GetDataFromControlsManual()
        {
        }

        /// <summary>
        /// save the current supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            SaveChanges(FMainDS);
        }

        /// <summary>
        /// needed for interface
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return SaveChanges(FMainDS);
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        public bool SaveChanges(AccountsPayableTDS AInspectDS)
        {
            FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

            // Don't allow saving if user is still editing a Detail of a List
            if (FPetraUtilsObject.InDetailEditMode())
            {
                return false;
            }

            FMainDS.AApSupplier.Rows[0].BeginEdit();
            GetDataFromControls();

            if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
            {
                foreach (DataTable InspectDT in AInspectDS.Tables)
                {
                    foreach (DataRow InspectDR in InspectDT.Rows)
                    {
                        InspectDR.EndEdit();
                    }
                }

                if (FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.WriteToStatusBar("Saving data...");
                    this.Cursor = Cursors.WaitCursor;

                    AccountsPayableTDS SubmitDS = AInspectDS.GetChangesTyped(true);

                    TSubmitChangesResult SubmissionResult;
                    TVerificationResultCollection VerificationResult;

                    // Submit changes to the PETRAServer
                    try
                    {
                        SubmissionResult = FUIConnector.SubmitChanges(ref SubmitDS, out VerificationResult);
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
                        bool ReturnValue = false;

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return ReturnValue;
                    }

                    switch (SubmissionResult)
                    {
                        case TSubmitChangesResult.scrOK:

                            // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                            AInspectDS.AcceptChanges();

                            // Merge back with data from the Server (eg. for getting Sequence values)
                            AInspectDS.Merge(SubmitDS, false);

                            // need to accept the new modification ID
                            AInspectDS.AcceptChanges();

                            // Update UI
                            FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                            this.Cursor = Cursors.Default;

                            // TODO EnableSave(false);

                            // We don't have unsaved changes anymore
                            FPetraUtilsObject.DisableSaveButton();

                            // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                            return true;

                        case TSubmitChangesResult.scrError:

                            // TODO scrError
                            break;

                        case TSubmitChangesResult.scrNothingToBeSaved:

                            // TODO scrNothingToBeSaved
                            break;

                        case TSubmitChangesResult.scrInfoNeeded:

                            // TODO scrInfoNeeded
                            break;
                    }
                }
            }

            return false;
        }
    }
}