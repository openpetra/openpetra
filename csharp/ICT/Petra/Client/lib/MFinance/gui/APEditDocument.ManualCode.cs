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
using System.Data;
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    public partial class TFrmAccountsPayableEditDocument
    {
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

            FMainDS.AApDocument.Rows[0].BeginEdit();
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
                        SubmissionResult = TRemote.MFinance.AccountsPayable.WebConnectors.SaveAApDocument(ref SubmitDS, out VerificationResult);
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

        private void UpdateCreditTerms(object sender, EventArgs e)
        {
            if (sender == dtpDateDue)
            {
                int diffDays = (dtpDateDue.Value - dtpDateIssued.Value).Days;

                if (diffDays < 0)
                {
                    diffDays = 0;
                    dtpDateDue.Value = dtpDateIssued.Value;
                }

                nudCreditTerms.Value = diffDays;
            }
            else if ((sender == dtpDateIssued) || (sender == nudCreditTerms))
            {
                dtpDateDue.Value = dtpDateIssued.Value.AddDays((double)nudCreditTerms.Value);
            }
        }
    }
}