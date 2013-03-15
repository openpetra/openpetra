//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmBusinessCodeSetup
    {
        private void NewRowManual(ref PBusinessRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PBusiness.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PBusiness.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.BusinessCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPBusiness();
        }
        
        private void DeleteRecord(Object sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResults;

            int Count = TRemote.MPartner.Partner.Cacheable.WebConnectors.GetCacheableRecordReferenceCount(
                TCacheablePartnerTablesEnum.BusinessCodeList, DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow), 
                out VerificationResults);
            
            MessageBox.Show("Delete: reference count = " + Count.ToString());
            
            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult(
                        Catalog.GetString("Record cannot be deleted!\r\n") +
                        Catalog.GetPluralString("Reason:", "Reasons:", VerificationResults.Count),
                        VerificationResults), Catalog.GetString("Record Deletion"));
            }
            else
            {
                MessageBox.Show("No references pointing to Row, delete can go ahead!");
            }
        }
    }
}