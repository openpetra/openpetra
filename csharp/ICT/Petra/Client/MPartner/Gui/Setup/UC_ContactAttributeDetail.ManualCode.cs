//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TUC_ContactAttributeDetail
    {
    	// A variable that keeps track of the current value of the contact code
    	private string _currentContactAttribute = String.Empty;
    	
        private void NewRowManual(ref PContactAttributeDetailRow ARow)
        {
            string newCode = Catalog.GetString("NEWCODE");
            Int32 countNewCode = 1;

            if (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { _currentContactAttribute, newCode }) != null)
            {
                while (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { _currentContactAttribute, newCode + countNewCode.ToString() }) != null)
                {
                    countNewCode++;
                }

                newCode += countNewCode.ToString();
            }

            ARow.ContactAttrDetailCode = newCode;
            ARow.ContactAttributeCode = _currentContactAttribute;
        }
        
        private void NewRow(Object sender, EventArgs e)
        {
        	CreateNewPContactAttributeDetail();
        	SetContactAttribute(_currentContactAttribute);
        }

        private void DeleteRow(Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            
            // Get the selected grid row
            int nSelectedRow = grdDetails.DataSourceRowToIndex2(grdDetails.SelectedDataRowsAsDataRowView[0]) + 1;
            // Delete the current row
        	FPreviouslySelectedDetailRow.Delete();
            FPetraUtilsObject.SetChangedFlag();
            
            // Select the next row to show
            int maxRow = grdDetails.Rows.Count - 1;
            if (nSelectedRow > maxRow) nSelectedRow = maxRow;
            if (nSelectedRow > 0) grdDetails.SelectRowInGrid(nSelectedRow);
            
            // Check the enabled states now that we have fewer rows
            btnDelete.Enabled = nSelectedRow > 0 && !txtDetailContactAttrDetailCode.ReadOnly;
            pnlDetails.Enabled = maxRow > 0;
        }

        private void ShowDetailsManual(PContactAttributeDetailRow ARow)
        {
        	if (ARow == null)
        	{
        		pnlDetails.Enabled = false;
        		btnDelete.Enabled = false;
        		txtDetailContactAttrDetailCode.Text = String.Empty;
        		txtDetailContactAttrDetailDescr.Text = String.Empty;
        	}
        	else
        	{
        		pnlDetails.Enabled = true;
        		btnDelete.Enabled = !txtDetailContactAttrDetailCode.ReadOnly;
        	}
        }
        
        /// <summary>
        /// Call this method when the Contact Attribute changes in the Contact Attribute grid on the parent page
        /// </summary>
        /// <param name="NewValue">New value for the contact attribute</param>
        public void SetContactAttribute(string NewValue)
        {
        	// Save the current contact attribute in our member variable
        	_currentContactAttribute = NewValue;
        	
	        
        	// Use our standard auto-code to create a dataset and bind it to the grid using the correct filter
        	FPetraUtilsObject.DisableDataChangedEvent();
	        pnlDetails.Enabled = false;
	        if (FMainDS.PContactAttributeDetail != null)
	        {
	        	// specify the filter and bind  the data
	        	string filter = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), NewValue);
	        	DataView myDataView = new DataView(FMainDS.PContactAttributeDetail, filter, "", DataViewRowState.CurrentRows);
	        	myDataView.AllowNew = false;
	            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
	            if (myDataView.Count > 0)
	            {
	                grdDetails.Selection.ResetSelection(false);
	                grdDetails.Selection.SelectRow(1, true);
	                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
	                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
	            }
	            else
	            {
	            	// extra code to handle the case where the grid is empty.  
	            	// The panel will have the wrong data in it initially if the grid is empty but the table
	            	// has rows that match a different contact attribute
	            	FPreviouslySelectedDetailRow = null;
	            	ShowDetails(null);
	            }
	        }
	        FPetraUtilsObject.EnableDataChangedEvent();
        }
    }
}