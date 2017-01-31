//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2015 by OM International
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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Ict.Petra.Client.MCommon.Gui
{
    public partial class TFrmCopyPartnerAddressDialog
    {
        private TFormDataPartner FFormData = new TFormDataPartner();
        private TPartnerClass FPartnerClass;

        private void InitializeManualCode()
        {
            cmbLayout.SelectedValueChanged += new System.EventHandler(cmbLayout_SelectedValueChanged);
            this.Width += 20;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (txtPreviewAddress.Text != String.Empty)
            {
                Clipboard.SetDataObject(txtPreviewAddress.Text);
                this.Close();
            }
        }

        private void cmbLayout_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerateAddress();
        }

        private void GenerateAddress()
        {
            String Address = TRemote.MPartner.Partner.WebConnectors.BuildAddressBlock(FFormData, cmbLayout.GetSelectedString(), FPartnerClass);

            txtPreviewAddress.Text = Address;
        }

        /// <summary>
        /// Sets the Data for the Form
        /// </summary>
        /// <param name="GridRows">Grid Information</param>
        public void SetFormData(DataRowView[] GridRows)
        {
            DataRowView Row = GridRows[0];

            FPartnerClass = Ict.Petra.Shared.SharedTypes.PartnerClassStringToEnum(Row[0].ToString());
            FFormData.ShortName = Row[1].ToString();
            FFormData.City = Row[2].ToString();
            FFormData.PostalCode = Row[3].ToString();
            FFormData.Address1 = Row[4].ToString();
            FFormData.AddressStreet2 = Row[5].ToString();
            FFormData.Address3 = Row[6].ToString();
            FFormData.County = Row[7].ToString();
            FFormData.CountryCode = Row[8].ToString();
            FFormData.PartnerKey = Row[9].ToString();
        }
    }
}