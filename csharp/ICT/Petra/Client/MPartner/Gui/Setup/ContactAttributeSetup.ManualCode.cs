//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       apanp
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
using SourceGrid;
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
    public partial class TFrmContactAttributeSetup
    {
        private void InitializeManualCode()
        {
        	// Initialise the user control variables
    		ucContactDetail.MainDS = null;
    		ucContactDetail.PetraUtilsObject = FPetraUtilsObject;

            // We need to capture the 'DataSaved' event, so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
        }
        
    	private void RunOnceOnActivationManual()
    	{
    		// Initialise the GUI of the user control
    		ucContactDetail.InitUserControl();
    		
			// Set up the correct filter for the bottom grid, based on our initial contact attribute
    		if (FMainDS.PContactAttribute.Rows.Count > 0)
			{
				ucContactDetail.SetContactAttribute(txtDetailContactAttributeCode.Text);
			}

    	}
    	
        private void NewRowManual(ref PContactAttributeRow ARow)
        {
            string newCode = Catalog.GetString("NEWCODE");
            Int32 countNewCode = 1;

            if (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode }) != null)
            {
                while (FMainDS.PContactAttribute.Rows.Find(new object[] { newCode + countNewCode.ToString() }) != null)
                {
                    countNewCode++;
                }

                newCode += countNewCode.ToString();
            }

            ARow.ContactAttributeCode = newCode;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPContactAttribute();
        }
        
        private void DeleteRecord(Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            
//            int rowIndex = CurrentRowIndex();
//        	FPreviouslySelectedDetailRow.Delete();
//            FPetraUtilsObject.SetChangedFlag();
//            SelectByIndex(rowIndex);
        }
        
        private void ShowDetailsManual(PContactAttributeRow ARow)
        {
        	if (ARow == null)
        	{
        		pnlDetails.Enabled = false;
        		ucContactDetail.Enabled = false;
        		btnDelete.Enabled = false;
        	}
        	else
        	{
        		pnlDetails.Enabled = true;
        		ucContactDetail.Enabled = true;
        		btnDelete.Enabled = !txtDetailContactAttributeCode.ReadOnly;
        		// Pass the contact attribute to the user control - it will then update itself
        		ucContactDetail.SetContactAttribute(ARow.ContactAttributeCode);
        	}
        }
        
        private void GetDetailDataFromControlsManual(PContactAttributeRow ARow)
        {
        	ucContactDetail.GetDataFromControls();
        }
        
        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
        	// Save the changes in the user control
        	if (e.Success)
        	{
        		FPetraUtilsObject.SetChangedFlag();
        		ucContactDetail.SaveChanges();
        	}
        }
    }
}