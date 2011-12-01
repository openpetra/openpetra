//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

/****************************************************************************************************
 * 
 * The TFrmLocalDataFieldSetup class acts as a base class for three derived classes.
 * This is because this screen (and the database table behind it) is used by three different parts
 *   of the application.
 * 1.  The Partner module
 * 2.  The personnel module application tables (short-term and long-term)
 * 3.  The personnel module general tables
 * 
 * We define the three sub-classes so the the main menu system can use each form in turn for 
 *    its ActionOpenScreen.
 * 
 * The 'InitializeManualCode' method allows us to set up the screen differently for each launch.
 * But this code runs in the form constructor.  I did try having a public property called 'Context'
 *    that the launcher would set, and it did get set - but not until after the constructor code runs.
 * 
 * So this way we use reflection to work out which class wa were launched as ....
 * 
 * *************************************************************************************************/
 
namespace Ict.Petra.Client.MCommon.Gui.Setup
{
	/// <summary>
	/// This is the class that we launch to configure the screen for the Partner module
	/// </summary>
	public class TFrmLocalPartnerDataFieldSetup : TFrmLocalDataFieldSetup
	{
		/// <summary>
		/// Constructor for the Partner module-related screen
		/// </summary>
		/// <param name="AParentForm">The screen form object</param>
		public TFrmLocalPartnerDataFieldSetup(Form AParentForm) : base(AParentForm)
		{
		}
	}
	
	/// <summary>
	/// This is the class that we launch for the Personnel module
	/// </summary>
	public class TFrmLocalPersonnelDataFieldSetup : TFrmLocalDataFieldSetup
	{
		/// <summary>
		/// Constructor for the Personnel module-related screen
		/// </summary>
		/// <param name="AParentForm">The screen form object</param>
		public TFrmLocalPersonnelDataFieldSetup(Form AParentForm) : base(AParentForm)
		{
		}
	}
	
	/// <summary>
	/// This is the class that we launch for the Application module
	/// </summary>
	public class TFrmLocalApplicationDataFieldSetup : TFrmLocalDataFieldSetup
	{
		/// <summary>
		/// Constructor for the Application module-related screen
		/// </summary>
		/// <param name="AParentForm">The screen form object</param>
		public TFrmLocalApplicationDataFieldSetup(Form AParentForm) : base(AParentForm)
		{
		}
	}
	
    public partial class TFrmLocalDataFieldSetup
    {
	    private class FExtraDS
	    {
	        public static PDataLabelUseTable PDataLabelUse;
	    }
	    
    	// These are the possible contexts in which we could have been launched
    	enum Context
    	{
    		Partner,
    		Application,
    		Personnel
    	}
    	
    	private Context CurrentContext;
    	
    	/// <summary>
    	/// Data table used by the list box
    	/// </summary>
    	public DataTable DTUsedBy = new DataTable();
        
    	private void InitializeManualCode()
        {
    		string ScreenName = this.GetType().Name;
        	string Col1 = "Used By";
        	string Col2 = String.Empty;
        	if (String.Compare(ScreenName, "TFrmLocalPartnerDataFieldSetup", true) == 0)
        	{
        		CurrentContext = Context.Partner;
	        	Col2 = "Partner Class";
        	}
        	else if (String.Compare(ScreenName, "TFrmLocalApplicationDataFieldSetup", true) == 0)
        	{
        		CurrentContext = Context.Application;
	        	Col2 = "Application";
        	}
        	else if (String.Compare(ScreenName, "TFrmLocalPersonnelDataFieldSetup", true) == 0)
        	{
        		CurrentContext = Context.Personnel;
	        	Col2 = "Personnel";
	        	clbUsedBy.Visible = false;
        	}
        	DTUsedBy.Columns.Add(Col1).DataType = Type.GetType("System.Boolean");
        	DTUsedBy.Columns.Add(Col2).DataType = Type.GetType("System.String");
        	
        	// Set the form title and list box content depending on our context
        	DataView grdDataView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
        	if (CurrentContext == Context.Partner)
        	{
        		this.Text = "Maintain Local Partner Data Fields";
	        	AddRowToUsedByList(DTUsedBy, "Person");
	        	AddRowToUsedByList(DTUsedBy, "Family");
	        	AddRowToUsedByList(DTUsedBy, "Church");
	        	AddRowToUsedByList(DTUsedBy, "Organisation");
	        	AddRowToUsedByList(DTUsedBy, "Bank");
	        	AddRowToUsedByList(DTUsedBy, "Unit");
	        	AddRowToUsedByList(DTUsedBy, "Venue");
        	}
        	else if (CurrentContext == Context.Application)
        	{
        		this.Text = "Maintain Local Application Data Fields";
	        	AddRowToUsedByList(DTUsedBy, "Long Term");
	        	AddRowToUsedByList(DTUsedBy, "Short Term");
        	}
        	else if (CurrentContext == Context.Personnel)
        	{
        		this.Text = "Maintain Local Personnel Data Fields";
	        	AddRowToUsedByList(DTUsedBy, "Personnel");
	        	grdDataView.RowFilter = "";
        	}
        	
        	clbUsedBy.AddCheckBoxColumn(Col1, DTUsedBy.Columns[0]);
        	clbUsedBy.AddTextColumn(Col2, DTUsedBy.Columns[1]);
        	clbUsedBy.DataBindGrid(DTUsedBy, Col2, Col1, Col2, Col2, false, false, false);
        	clbUsedBy.Columns[0].Width = 60;
        	clbUsedBy.Columns[1].Width = 90;
        	
        	tableLayoutPanel3.SetRow(txtDetailNumDecimalPlaces, 0);
        	tableLayoutPanel3.SetRow(cmbDetailCurrencyCode, 0);
        	tableLayoutPanel3.SetRow(cmbDetailLookupCategoryCode, 0);
        	
        	txtDetailNumDecimalPlaces.Visible = false;
        	cmbDetailCurrencyCode.Visible = false;
        	cmbDetailLookupCategoryCode.Visible = false;
        }
        
        private void AddRowToUsedByList(DataTable t, string item)
        {
        	DataRow dr = t.NewRow();
        	dr[0] = false;
        	dr[1] = item;
        	t.Rows.Add(dr);
        }

        private void NewRowManual(ref PDataLabelRow ARow)
        {
            Int32 labelKey = 1;

            while (FMainDS.PDataLabel.Rows.Find(new object[] { labelKey }) != null)
            {
                labelKey++;
            }
            ARow.Key = labelKey;
            ARow.Text = "NewLabel";
            ARow.DataType = "char";
            ARow.CharLength = 24;
            ARow.NumDecimalPlaces = 0;
            ARow.CurrencyCode = (cmbDetailCurrencyCode.SelectedIndex <= 1) ? "USD" : cmbDetailCurrencyCode.Text;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPDataLabel();
            txtDetailText.SelectAll();
            txtDetailText.Focus();
        }
        
        private void ShowDetailsManual(PDataLabelRow ARow)
        {
        	if (String.Compare(ARow.DataType, "float", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 1;
        	}
        	else if (String.Compare(ARow.DataType, "currency", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 2;
        	}
        	else if (String.Compare(ARow.DataType, "boolean", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 3;
        	}
        	else if (String.Compare(ARow.DataType, "date", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 4;
        	}
        	else if (String.Compare(ARow.DataType, "time", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 5;
        	}
        	else if (String.Compare(ARow.DataType, "lookup", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 6;
        	}
        	else if (String.Compare(ARow.DataType, "partnerkey", true) == 0)
        	{
        		cmbDetailDataType.SelectedIndex = 7;
        	}
        	else 
        	{
        		// Use char
        		cmbDetailDataType.SelectedIndex = 0;
        	}
        }

    	private void GetDetailDataFromControlsManual(PDataLabelRow ARow)
    	{
    		switch (cmbDetailDataType.SelectedIndex)
    		{
    			case 1:
    				ARow.DataType = "float";
    				break;
    			case 2:
    				ARow.DataType = "currency";
    				break;
    			case 3:
    				ARow.DataType = "boolean";
    				break;
    			case 4:
    				ARow.DataType = "date";
    				break;
    			case 5:
    				ARow.DataType = "time";
    				break;
    			case 6:
    				ARow.DataType = "lookup";
    				break;
    			case 7:
    				ARow.DataType = "partnerkey";
    				break;
    			default:
    				ARow.DataType = "char";
    				break;
    		}
    	}
    	
        /// <summary>
        /// Fired when the user clicks on the combo-box that sets the data type for the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDataTypeChange(Object sender, EventArgs e)
        {
        	if (cmbDetailDataType.SelectedIndex < 0 || cmbDetailDataType.SelectedIndex > 7) return;
        	
			txtDetailCharLength.Visible = false;
			txtDetailNumDecimalPlaces.Visible = false;
			cmbDetailCurrencyCode.Visible = false;
			cmbDetailLookupCategoryCode.Visible = false;
			
			// We cannot hide the labels because this changes the layout positions if everything is hidden!
			// So we alter the text of the Charlength label instead
			Label lblExtraInfo = lblDetailCharLength;
	       	switch (cmbDetailDataType.SelectedIndex)
        	{
        		case 0:		// Text
        			txtDetailCharLength.Visible = true;
        			lblExtraInfo.Text = "Length:";
        			break;
        		case 1:		// Numeric
        			txtDetailNumDecimalPlaces.Visible = true;
        			lblExtraInfo.Text = "Decimal places:";
        			break;
        		case 2:		// Currency
        			cmbDetailCurrencyCode.Visible = true;
        			lblExtraInfo.Text = "Currency code:";
        			break;
        		case 3:		// Yes/No
        		case 4:		// Date
        		case 5:		// Time
        		case 7:		// PartnerKey
        			lblExtraInfo.Text = String.Empty;
        			break;
        		case 6:		// OptionList
        			cmbDetailLookupCategoryCode.Visible = true;
        			lblExtraInfo.Text = "Option list name:";
        			break;
        	}
        }
    }
}