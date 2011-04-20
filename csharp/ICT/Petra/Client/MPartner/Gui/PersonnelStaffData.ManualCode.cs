//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPersonnelStaffData
    {
      
        private void NewRow(System.Object sender, EventArgs e)
        {
           
        }

     	private Int64 FPartnerKey;
     	
		/// <summary>Partnerkey is the part of the "real" primary key for this table</summary>
     	public long PartnerKey {
			get { return FPartnerKey; }
			set { FPartnerKey = value; }
		}

        private void DeleteRow(System.Object sender, EventArgs e)
        {
          
        }

      
		private TSubmitChangesResult StoreManualCode(ref PersonnelTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
		{
			return TRemote.MPersonnel.WebConnectors.SavePersonnelTDS( ref ASubmitChanges, out AVerificationResult);
		}
		
        private void ShowDetailsManual(PmStaffDataRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
            }
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
        private void GetDetailDataFromControlsManual(PmStaffDataRow ARow)
        {
        	//TODO
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

       
    }
}