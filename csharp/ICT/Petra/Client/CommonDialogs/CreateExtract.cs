//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// Description of CreateExtract.
    /// </summary>
    public partial class TFrmCreateExtract : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TFrmCreateExtract()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnCreateExtract.Text = Catalog.GetString("Create Extract");
            this.lblExtractName.Text = Catalog.GetString("Extract Name") + ":";
            this.lblDescription.Text = Catalog.GetString("Description") + ":";
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.Text = Catalog.GetString("Create Extract");
            #endregion
        }

        private BestAddressTDSLocationTable FBestAddress = null;

        /// <summary>
        /// set the list of addresses that should be stored in the extract
        /// </summary>
        public BestAddressTDSLocationTable BestAddress
        {
            set
            {
                FBestAddress = value;
            }
        }

        /// <summary>
        /// should invalid addresses from the best address table be included in the extract?
        /// </summary>
        public bool IncludeNonValidAddresses = false;

        private void BtnCreateExtractClick(object sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResult;
            Int32 ExtractID;
            bool ExtractAlreadyExists;

            if (FBestAddress == null)
            {
                MessageBox.Show("Note to Developer: need to set BestAddress list first", "Failure");
                return;
            }

            if (!TRemote.MPartner.Mailing.WebConnectors.CreateExtractFromBestAddressTable(
                    txtExtractName.Text,
                    txtDescription.Text,
                    out ExtractID,
                    out ExtractAlreadyExists,
                    out VerificationResult,
                    FBestAddress,
                    IncludeNonValidAddresses))
            {
                if (ExtractAlreadyExists)
                {
                    MessageBox.Show(Catalog.GetString(
                            "The extract was not created because an extract with that name already exists"), Catalog.GetString("Failure"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("The extract was not created"), Catalog.GetString("Failure"));
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}