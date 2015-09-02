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
using Ict.Common.Data;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TFrmExtractMaintain
    {
        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private ExtractTDS FMainDS = null;

        #region Properties
        /// <summary>
        /// id of extract displayed in this screen
        /// </summary>
        public int ExtractId
        {
            set
            {
                ucoExtractMaintain.ExtractId = value;
            }
        }

        /// <summary>
        /// name of extract displayed in this screen
        /// </summary>
        public string ExtractName
        {
            set
            {
                ucoExtractMaintain.ExtractName = value;
            }
        }

        /// <summary>
        /// True if extract is frozen and cannot be modifid.
        /// </summary>
        public bool Frozen
        {
            set
            {
                ucoExtractMaintain.Frozen = value;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Loads Extract Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadData()
        {
            Boolean ReturnValue = true;

            return ReturnValue;
        }

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
            return ucoExtractMaintain.SaveChanges();
        }

        /// <summary>
        /// react to menu item / save button
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        private void InitializeManualCode()
        {
            this.mniCopyPartnerKey.ShortcutKeys = Keys.C | Keys.Control;
            this.mniSetLastWorkedWith.ShortcutKeys = Keys.L | Keys.Control;
        }

        private void RunOnceOnActivationManual()
        {
            ucoExtractMaintain.InitializeData();
            this.Text = "Maintenance of Extract: " + ucoExtractMaintain.ExtractName;
        }

        /// <summary>
        /// Copy partner key of currenly selected partner to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyPartnerKeyToClipboard(System.Object sender, EventArgs e)
        {
            ucoExtractMaintain.CopyPartnerKeyToClipboard(sender, e);
        }

        /// <summary>
        /// mark the selected partner record as the one last worked with in the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPartnerLastWorkedWith(System.Object sender, EventArgs e)
        {
            ucoExtractMaintain.SetPartnerLastWorkedWith(sender, e);
        }

        /// <summary>
        /// Verify and if necessary update partner data in an extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerifyAndUpdateExtract(System.Object sender, EventArgs e)
        {
            ucoExtractMaintain.VerifyAndUpdateExtract(sender, e);
        }

        #endregion

        #region Keyboard and Filter/Find Menu

        /// ///////////  These methods just delegate to the user control to handle

        private void MniFilterFind_Click(object sender, EventArgs e)
        {
            ucoExtractMaintain.MniFilterFind_Click(sender, e);
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

            if (ucoExtractMaintain.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}