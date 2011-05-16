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
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
	public partial class TFrmPersonnelStaffData
	{
		private void InitializeManualCode()
        {
            pnlDetails.Enabled = false;
            cmbDetailStatusCode.ComboBoxWidth = 200;
            
        }
		/// <summary>The new button was pressed;create a new row</summary></summary>
		public void NewRow(System.Object sender, EventArgs e)
		{
			CreateNewPmStaffData();		
			UpdatePartnerKey();
		}

		private void UpdatePartnerKey()
		{
			txtDetailPartnerKey.Text = Convert.ToString(FPartnerKey);
		}
		private Int64 GetSiteKey()
		{
			return Convert.ToInt64(TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, ""));
		}
		public void NewRowManual(ref PmStaffDataRow ANewRow)
		{
			ANewRow.SiteKey = GetSiteKey();
			//search the latest used (max) id to build the new key, ignore sitekey
			long max=0;
			foreach (PmStaffDataRow am in FMainDS.PmStaffData.Rows)
			{
				if (am.Key > max) max=am.Key;
			}
			ANewRow.Key = max + 1;                  
			ANewRow.PartnerKey = FPartnerKey;
			ANewRow.ReceivingField = 0;
			ANewRow.SetReceivingFieldOfficeNull();
			ANewRow.OfficeRecruitedBy = 0;
			ANewRow.HomeOffice = 0;
		}

		private Int64 FPartnerKey;

		/// <summary>Partnerkey is the part of the "real" primary key for this table</summary>
		public long PartnerKey {
			get
			{
				return FPartnerKey;
			}
			set
			{
				FPartnerKey = value;
				UpdatePartnerKey();
				LoadPersonellStaffData();
			}
		}
		private void  LoadPersonellStaffData()
		{
			
			FMainDS = TRemote.MPersonnel.WebConnectors.LoadPersonellStaffData( FPartnerKey);
			cmbDetailStatusCode.InitialiseUserControl(FMainDS.PmCommitmentStatus, PmCommitmentStatusTable.GetCodeDBName(),PmCommitmentStatusTable.GetDescDBName(),null);
			int a=FMainDS.PmStaffData.Rows.Count;
			System.Diagnostics.Debug.Print(""+a);
           	if (FMainDS != null)
            {
            	DataView myDataView = FMainDS.PmStaffData.DefaultView;
      			myDataView.AllowNew = false;
      			grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);       
            }
		}	
		private void DeleteRow(System.Object sender, EventArgs e)
		{
		}

		private TSubmitChangesResult StoreManualCode(ref PersonnelTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
		{
			return TRemote.MPersonnel.WebConnectors.SavePersonnelTDS(ref ASubmitChanges, out AVerificationResult);
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
			//TODO THis is a workaround, where is the input of ReceivingFieldOffice? 
			ARow.ReceivingFieldOffice = Convert.ToInt64(txtDetailReceivingField.Text);
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