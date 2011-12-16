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
using Ict.Petra.Client.App.Core;
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
    	// This is the extra dataset that we need that gives us the UsedBy information
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
    	
    	private int UsedByColumnID = -1;
    	
    	/// <summary>
    	/// This is the simple two column data table used by the list box.
    	/// The row content (and column headings) depend on our launch context
    	/// </summary>
    	public DataTable DTUsedBy = new DataTable();
        
    	// This is the main area where we set up to use the additional information from the extra table
    	private void InitializeManualCode()
        {
    		// Get our screenClassName and initialise the list box column headings
    		string ScreenName = this.GetType().Name;
        	string Col1 = "Used By";
        	string Col2 = String.Empty;
        	if (String.Compare(ScreenName, "TFrmLocalPartnerDataFieldSetup", true) == 0)
        	{
        		CurrentContext = Context.Partner;
	        	Col2 = "PartnerClass";
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
        	
        	// Now we can initialise our little data table that backs the list view
        	DTUsedBy.Columns.Add(Col1).DataType = Type.GetType("System.Boolean");
        	DTUsedBy.Columns.Add(Col2).DataType = Type.GetType("System.String");
        	
        	// Add a 'Used By' column to our main dataset
        	FMainDS.PDataLabel.Columns.Add("UsedByInit", typeof(String));
        	FMainDS.PDataLabel.Columns.Add("UsedBy", typeof(String));
        	UsedByColumnID = FMainDS.PDataLabel.Columns.Count - 1;
			
			// Load the Extra Data from DataLabelUse table
			Type DataTableType;
			FExtraDS.PDataLabelUse = new PDataLabelUseTable();
			DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("DataLabelUseList", String.Empty, null, out DataTableType);
			FExtraDS.PDataLabelUse.Merge(CacheDT);
        	
			// Take each row of our main dataset and populate the new column with relevant data
			//   from the DataLabelUse table
			foreach (PDataLabelRow labelRow in FMainDS.PDataLabel.Rows)
			{
				string usedBy = String.Empty;
				int key = labelRow.Key;
				DataRow[] rows = FExtraDS.PDataLabelUse.Select("p_data_label_key_i=" + key.ToString());
				foreach(PDataLabelUseRow useRow in rows)
				{
					if (usedBy != String.Empty) usedBy += ",";
					usedBy += useRow.Use;
				}
				
				// Make 2 new colums:  Initially they hold the same values, but if we make a change we modify the second one.
				labelRow[UsedByColumnID - 1] = usedBy;	// since we added this column manually, it does not have a handy property
				labelRow[UsedByColumnID] = usedBy;	// since we added this column manually, it does not have a handy property
			}
			// So now our main data set has everything we need
			// But the grid doesn't know about our new column yet
			
        	// Set the form title and list box content depending on our context
        	if (CurrentContext == Context.Partner)
        	{
        		this.Text = "Maintain Local Partner Data Fields";
	        	AddRowToUsedByList("Person");
	        	AddRowToUsedByList("Family");
	        	AddRowToUsedByList("Church");
	        	AddRowToUsedByList("Organisation");
	        	AddRowToUsedByList("Bank");
	        	AddRowToUsedByList("Unit");
	        	AddRowToUsedByList("Venue");
        	}
        	else if (CurrentContext == Context.Application)
        	{
        		this.Text = "Maintain Local Application Data Fields";
	        	AddRowToUsedByList("Long Term");
	        	AddRowToUsedByList("Short Term");
        	}
        	else if (CurrentContext == Context.Personnel)
        	{
        		this.Text = "Maintain Local Personnel Data Fields";
	        	AddRowToUsedByList("Personnel");
        	}
        	
        	clbUsedBy.AddCheckBoxColumn(Col1, DTUsedBy.Columns[0], 60, false);
        	clbUsedBy.AddTextColumn(Col2, DTUsedBy.Columns[1], 90);
        	clbUsedBy.DataBindGrid(DTUsedBy, Col2, Col1, Col2, Col2, false, false, false);
        	
			// Set up the label control that we will use to indicate the current sub-type of data
			// We need to right align the text so it looks nice when we change it
        	lblDataSubType.AutoSize = false;
			lblDataSubType.Left = 0;
			lblDataSubType.Width = 115;		// 5 less than the column width
			lblDataSubType.TextAlign = System.Drawing.ContentAlignment.TopRight;
			
        	// Now we have to deal with the form controls that depend on the selction of DataType
        	// and we only want one visible at a time - so hide these three
        	pnlCurrencyCode.Visible = false;
        	pnlCategoryCode.Visible = false;
        	txtDetailNumDecimalPlaces.Visible = false;
        	
        	// We need to capture the starting to save event
        	FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
    	}
        
    	// Simple helper that adds a string item as a row in our UsedBy list
        private void AddRowToUsedByList(string item)
        {
        	DataRow dr = DTUsedBy.NewRow();
        	dr[0] = false;
        	dr[1] = item;
        	DTUsedBy.Rows.Add(dr);
        }
        
        private void RunOnceOnActivationManual()
        {
        	// This is the point at which we can add our additional column to the details grid
        	grdDetails.AddTextColumn("Used By", FMainDS.PDataLabel.Columns["UsedBy"]);
			DataView myDataView = FMainDS.PDataLabel.DefaultView;
			myDataView.AllowNew = false;
			grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);		}

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
            ARow[UsedByColumnID] = "";
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
        	
        	// Set the checkboxes in the UsedBy list
			DTUsedBy.DefaultView.AllowEdit = true;
        	// We don't have a nice typed property for our manually added column - but we know it will be the last one
			string stringList = ARow[UsedByColumnID].ToString();
        	clbUsedBy.SetCheckedStringList(stringList);
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
    		
    		string stringList = clbUsedBy.GetCheckedStringList();
			ARow[UsedByColumnID] = stringList;
    	}

    	void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
    	{
    		// Ensure we get the current row's information
    		if (FPreviouslySelectedDetailRow != null)
    		{
	    		string stringList = clbUsedBy.GetCheckedStringList();
	    		FPreviouslySelectedDetailRow[UsedByColumnID] = stringList;
    		}
    		
    		// Now we need to save the PDataLabelUse table info using our data from our UsedBy column
    		// Go round all the rows, seeing which rows have a new UsedBy value
    		foreach(PDataLabelRow labelRow in FMainDS.PDataLabel.Rows)
    		{
    			if (labelRow[UsedByColumnID].ToString() != labelRow[UsedByColumnID - 1].ToString())
    			{
    				// This row's UsedBy column has been edited
    				// Delete the old rows that applied to this key
					int key = labelRow.Key;
		    		DataRow[] Userows = FExtraDS.PDataLabelUse.Select("p_data_label_key_i=" + key.ToString());
		    		foreach(DataRow r in Userows)
		    		{
		    			r.Delete();
		    		}
		    		
		    		// Create new rows and add them
		    		string usedByList = labelRow[UsedByColumnID].ToString();
		    		string[] uses = usedByList.Split(',');
		    		int nIndex = 1;
		    		foreach(string use in uses)
		    		{
			    		PDataLabelUseRow newRow = FExtraDS.PDataLabelUse.NewRowTyped();
			    		newRow.DataLabelKey = key;
			    		newRow.Use = use;
			    		newRow.Idx1 = nIndex++;
			    		FExtraDS.PDataLabelUse.Rows.Add(newRow);
		    		}
    			}
    		}
    		Ict.Common.Data.TTypedDataTable SubmitDT = FExtraDS.PDataLabelUse.GetChangesTyped();
            if (SubmitDT == null) return;		// nothing to save
            
            // Submit changes to the PETRAServer for the DataLabelUse table
            // This code is basically lifted from a typical auto-generated equivalent
            // TODO: If the standard code changes because TODO's get done, we will need to change this manual code
            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection VerificationResult;
            try
            {
                SubmissionResult = TDataCache.SaveChangedCacheableDataTableToPetraServer("DataLabelUseList", ref SubmitDT, out VerificationResult);
            }
            catch (System.Net.Sockets.SocketException)
            {
                MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                    "No Server response",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            catch (EDBConcurrencyException)
            {
                MessageBox.Show("The 'UsedBy' part of the data could not be saved! There has been a conflict with another user's data entry.",
                    "Cached Table Data Conflict",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            catch (Exception exp)
            {
                TLogging.Log(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                    TLoggingType.ToLogfile);
                MessageBox.Show(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                    "For details see the log file: " + TLogging.GetLogFileName(),
                    "Server connection error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                return;
            }

            switch (SubmissionResult)
            {
                case TSubmitChangesResult.scrOK:

                    // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                    FExtraDS.PDataLabelUse.AcceptChanges();

                    // Merge back with data from the Server (eg. for getting Sequence values)
                    FExtraDS.PDataLabelUse.Merge(SubmitDT, false);

                    // need to accept the new modification ID
                    FExtraDS.PDataLabelUse.AcceptChanges();

                    return;

                case TSubmitChangesResult.scrNothingToBeSaved:

                    return;

                case TSubmitChangesResult.scrError:

	                MessageBox.Show("The 'UsedBy' part of the data could not be saved! There has been an error while making changes to the table.",
	                    "Submit Changes to Table Error",
	                    MessageBoxButtons.OK,
	                    MessageBoxIcon.Stop);
                    break;

                case TSubmitChangesResult.scrInfoNeeded:

	                MessageBox.Show("The 'UsedBy' part of the data could not be saved! Insufficient information was provided when making changes to the table.",
	                    "Submit Changes to Table Error",
	                    MessageBoxButtons.OK,
	                    MessageBoxIcon.Stop);
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
        	
        	// Start by hiding everything
        	txtDetailCharLength.Visible = false;
			txtDetailNumDecimalPlaces.Visible = false;
			pnlCurrencyCode.Visible = false;
			pnlCategoryCode.Visible = false;
			
			// Show the relevant panel or text box and modify the label text
	       	switch (cmbDetailDataType.SelectedIndex)
        	{
        		case 0:		// Text
        			txtDetailCharLength.Visible = true;
        			lblDataSubType.Text = "Maximum length:";
        			break;
        		case 1:		// Numeric
        			txtDetailNumDecimalPlaces.Visible = true;
        			lblDataSubType.Text = "Decimal places:";
        			break;
        		case 2:		// Currency
        			pnlCurrencyCode.Visible = true;
        			lblDataSubType.Text = "Currency code:";
        			break;
        		case 3:		// Yes/No
        		case 4:		// Date
        		case 5:		// Time
        		case 7:		// PartnerKey
        			lblDataSubType.Text = String.Empty;
        			break;
        		case 6:		// OptionList
					pnlCategoryCode.Visible = true;
        			lblDataSubType.Text = "Option list name:";
        			break;
        	}
        }
    }
}