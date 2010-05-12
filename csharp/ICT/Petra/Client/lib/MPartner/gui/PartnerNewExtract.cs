//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Dialog to create a new extract in m_extract_master.
    /// User can insert the extract name and the description.
    /// The result will be a new extract ID.
    /// </summary>
    public partial class TPartnerNewExtract : TFrmPetraDialog
    {
        private const string StrScreenCaption = "New Extract";
        private const string StrExtractAlreadyExists = "Cannot create a new Extract because the Extract name already exists.\r\n\r\n" +
                                                       "Please change the name and try again.";
        private const string StrExtractAlreadyExistsTitle = "Problem creating new Extract";
        private const string StrExtractCreationFailed = "Something went wrong during the creation of the Extract.\r\n\r\n" +
                                                        "The Extract was not created as a result of this.";
        private const string StrExtractCreationFailedTitle = "Problem creating new Extract";

        /// <summary>Reference to the screen's UIConnector (serverside Business Object)</summary>
        private IPartnerUIConnectorsPartnerNewExtract FUIConnector;

        /// <summary>Field that holds the ID of the new created extract</summary>
        private Int32 FExtractId;

        /// <summary>Keeps track of the last location where the Form was shown
        /// when the OK Button was pressed.</summary>
        private Point FLastShownLocation = new Point(-1, -1);

        /// <summary>
        /// Constructor
        /// </summary>
        public TPartnerNewExtract() : base()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblExtractName.Text = Catalog.GetString("Extract &Name:");
            this.lblDescription.Text = Catalog.GetString("&Description:");
            this.Text = Catalog.GetString("New Extract");
            #endregion

            txtExtractName.MaxLength = MExtractMasterTable.GetExtractNameLength();
            txtExtractDescription.MaxLength = MExtractMasterTable.GetExtractDescLength();
            this.btnOK.Enabled = false;
        }

        #region event handler
        private void BtnOKClick(object sender, EventArgs e)
        {
            bool ServerCallSuccessful = false;
            bool CreateExtractOK = false;
            bool ExtractNameAlreadyExists = false;
            TVerificationResultCollection VerificationResults;
            DialogResult ServerBusyDialogResult;

            FExtractId = -1;

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            // Get the Extract ID and return it
            do
            {
                try
                {
                    CreateExtractOK = FUIConnector.CreateNewExtract(txtExtractName.Text,
                        txtExtractDescription.Text,
                        out FExtractId,
                        out ExtractNameAlreadyExists,
                        out VerificationResults);

                    ServerCallSuccessful = true;
                }
                catch (EDBTransactionBusyException)
                {
                    ServerBusyDialogResult = MessageBox.Show(
                        String.Format(CommonResourcestrings.StrPetraServerTooBusy, "create an Extract"),
                        CommonResourcestrings.StrPetraServerTooBusyTitle,
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                    if (ServerBusyDialogResult == System.Windows.Forms.DialogResult.Retry)
                    {
                        // retry will happen because of the repeat block
                    }
                    else
                    {
                        // break out of repeat block
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (!(ServerCallSuccessful));

            if (!CreateExtractOK)
            {
                // No Success
                if (ExtractNameAlreadyExists)
                {
                    this.Cursor = Cursors.Default;
                    Application.DoEvents();

                    MessageBox.Show(StrExtractAlreadyExists, StrExtractAlreadyExistsTitle);

                    txtExtractName.Focus();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    Application.DoEvents();

                    MessageBox.Show(StrExtractCreationFailed,
                        StrExtractCreationFailedTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                // Successfull call. Save the extract name as the last used one.
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_EXTRAC_LAST_EXTRACT_NAME, txtExtractName.Text);
                TUserDefaults.SaveChangedUserDefault(TUserDefaults.PARTNER_EXTRAC_LAST_EXTRACT_NAME);

                FLastShownLocation = this.Location;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void TPartnerNewExtractVisibleChanged(object sender, System.EventArgs e)
        {
            if ((this.Visible)
                && (FLastShownLocation.X != -1))
            {
                /*
                 * Restore the Form's Location to what it was when the OK Button was
                 * pressed. This is needed in case the programmer shows the Modal Form
                 * again after it was closed with the OK button - Windows automatically
                 * assigns a new Location for the Form in that case...
                 */
                this.Location = FLastShownLocation;
            }
        }

        private void txtExtractName_KeyUp(object sender, System.EventArgs e)
        {
            if (txtExtractName.Text.Length > 0)
            {
                this.btnOK.Enabled = true;
            }
            else
            {
                this.btnOK.Enabled = false;
            }
        }

        private void PartnerNewExtract_Closing(object sender, FormClosingEventArgs e)
        {
            UnRegisterUIConnector();
        }

        private void PartnerNewExtract_Loading(object sender, EventArgs e)
        {
            if (!GetNewExtractUIConnector())
            {
                this.Close();
            }
        }

        private void PartnerNewExtract_Shown(object sender, EventArgs e)
        {
            txtExtractName.Focus();
        }

        #endregion

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        /// </summary>
        /// <param name="NewExtractId">The Id of the newly created extract</param>
        /// <returns>true</returns>
        public Boolean GetReturnedParameters(out Int32 NewExtractId)
        {
            NewExtractId = FExtractId;

            return true;
        }

        /// <summary>
        /// Updates the Status Bar Text and disables the GUI.
        /// </summary>
        /// <param name="AStatusBarText">Text to be displayed in the StatusBar.</param>
        public void ShowProgressAfterOK(string AStatusBarText)
        {
TODO: what about translation ?
            this.sbtForm.SetStatusBarText(this.btnOK, AStatusBarText);
            btnCancel.Focus();
            btnOK.Focus();

            // Disable all Controls
            grpExtractDetails.Enabled = false;
            pnlBtnOKCancelHelpLayout.Enabled = false;

            Application.DoEvents();
        }

        /// <summary>
        /// Instantiates the Screen's UIConnector.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        private Boolean GetNewExtractUIConnector()
        {
            System.Windows.Forms.DialogResult ServerBusyDialogResult;
            Boolean ServerCallSuccessful = false;

            do
            {
                try
                {
                    FUIConnector = TRemote.MPartner.Partner.UIConnectors.CreateNewExtract();
                    ServerCallSuccessful = true;
                }
                catch (EDBTransactionBusyException)
                {
                    /*
                     * Note: The current implementation of the UIConnector won't ever throw
                     * EDBTransactionBusyException because it doesn't access the DB in any way.
                     * Still leaving this code in here because it might do that in the future...
                     */
                    ServerBusyDialogResult = MessageBox.Show(
                        String.Format(CommonResourcestrings.StrPetraServerTooBusy, "open the " + StrScreenCaption + " screen"),
                        CommonResourcestrings.StrPetraServerTooBusyTitle,
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                    if (ServerBusyDialogResult == System.Windows.Forms.DialogResult.Retry)
                    {
                        // retry will happen because of the repeat block
                    }
                    else
                    {
                        // break out of repeat block; this function will return false because of that.
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (!(ServerCallSuccessful));

            if (ServerCallSuccessful)
            {
                // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                TEnsureKeepAlive.Register(FUIConnector);
            }

            return ServerCallSuccessful;
        }

        /// <summary>
        /// Deletes the Extract again that was created by this Screen.
        /// This is needed if a client-side process that involved creating a
        /// new Extract failed somehow and the created Extract needs to be
        /// deleted again.
        /// </summary>
        public void DeleteExtractAgain()
        {
            FUIConnector.DeleteExtractAgain();
        }

        /// <summary>
        /// Frees the UIConnector so it can be GC'ed on the server side.
        /// </summary>
        /// <returns>void</returns>
        private void UnRegisterUIConnector()
        {
            if (FUIConnector != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FUIConnector);
            }
        }
    }
}