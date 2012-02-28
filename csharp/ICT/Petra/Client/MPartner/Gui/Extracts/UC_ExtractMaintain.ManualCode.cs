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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner.Gui;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    public partial class TUC_ExtractMaintain
    {
        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        //private ExtractTDS FMainDS = null;
        
	    private MExtractRow FPreviouslySelectedDetailRow = null;
        
	    // id of the extract that is displayed on this screen
        private int FExtractId;

        #region Properties
        /// <summary>
        /// id of extract displayed in this screen
        /// </summary>
        public int ExtractId
        {
            set
            {
            	FExtractId = value;
            }
        }
		#endregion        

        #region Public Methods
        
        /// <summary>
        /// initialize internal data before control is shown
        /// </summary>
        public void InitializeData()
        {
        	// set fixed column widths as otherwise grid will spend a long time recalculating optimal width with big extracts
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Partner Key", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerKeyDBName()], 100);
            grdDetails.AddTextColumn("Class", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerClassDBName()], 100);
            grdDetails.AddTextColumn("Partner Name", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetPartnerShortNameDBName()], 300);
            grdDetails.AddTextColumn("Location Key", FMainDS.MExtract.Columns[ExtractTDSMExtractTable.GetLocationKeyDBName()], 100);

            LoadData();
        }
        
        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            //TODO
            return false;
        }
        
        /// <summary>
        /// react to menu item / save button
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
        	SaveChanges();
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        //public void FileSave(object sender, EventArgs e)
        //{
        //    SaveChanges();
        //}
        
        #endregion
        
        #region Private Methods
        
        private void InitializeManualCode()
        {
            FMainDS = new ExtractTDS();
            
            // enable grid to react to insert and delete keyboard keys
            grdDetails.DeleteKeyPressed += new TKeyPressedEventHandler(grdDetails_DeleteKeyPressed);
        }

        /// <summary>
        /// Loads Extract Master Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadData()
        {
            Boolean ReturnValue;

            // Load Extract Headers, if not already loaded
            try
            {
                // Make sure that MasterTyped DataTables are already there at Client side
                if (FMainDS.MExtract == null)
                {
                    FMainDS.Tables.Add(new ExtractTDSMExtractTable());
                    FMainDS.InitVars();
                }

                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetExtractRowsWithPartnerData(FExtractId));

                // Make DataRows unchanged
                if (FMainDS.MExtract.Rows.Count > 0)
                {
                    FMainDS.MExtract.AcceptChanges();
                    FMainDS.AcceptChanges();
                }

                if (FMainDS.MExtract.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }
        
        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            // display the details of the currently selected row
//            ShowData(GetSelectedRow());
        }

        private void EditPartner(System.Object sender, EventArgs e)
        {
        	ExtractTDSMExtractRow SelectedRow = GetSelectedDetailRow();
        	
            // Open Partner Edit Screen for selected partner
            if (SelectedRow != null)
            {
	            this.Cursor = Cursors.WaitCursor;
	
	            try
	            {
	                TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

//MA0006: "% is a private partner of another user"	                

	                frm.SetParameters(TScreenMode.smEdit, 
	                                  SelectedRow.PartnerKey, 
	                                  SelectedRow.SiteKey, 
	                                  SelectedRow.LocationKey);
	                frm.Show();
	            }
	            finally
	            {
	                this.Cursor = Cursors.Default;
	            }
            }
        }

        private void AddPartner(System.Object sender, EventArgs e)
        {
           	ExtractTDSMExtractRow NewRow;
            System.Int64 PartnerKey = 0;
            string PartnerShortName;
            TPartnerClass PartnerClass;
            TLocationPK ResultLocationPK;
            
            DataRow[] ExisitingPartnerRow;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenPartnerFindScreen.Invoke
                        ("",
                        out PartnerKey,
                        out PartnerShortName,
                        out ResultLocationPK,
                        this.ParentForm);

                    if (PartnerKey != -1)
                    {
                    	ExisitingPartnerRow = FMainDS.MExtract.Select(ExtractTDSMExtractTable.GetPartnerKeyDBName() + " = " + PartnerKey.ToString());
                    	if (ExisitingPartnerRow.Length > 0)
                    	{
	                    	// check if partner already exists in extract
                            MessageBox.Show(Catalog.GetString("A record for this partner already exists in this extract"),
                                Catalog.GetString("Add Partner to Extract"));
	                    	
	                    	return;
                    	}

                        TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
                            PartnerKey,
                            out PartnerShortName,
                            out PartnerClass);
                    	
                    	// add new record to extract
                    	NewRow = FMainDS.MExtract.NewRowTyped();
                    	NewRow.ExtractId = FExtractId;
                    	NewRow.PartnerKey = PartnerKey;
                    	NewRow.PartnerShortName = PartnerShortName;
                    	NewRow.PartnerClass = SharedTypes.PartnerClassEnumToString(PartnerClass);
                    	NewRow.SiteKey = ResultLocationPK.SiteKey;
                    	NewRow.LocationKey = ResultLocationPK.LocationKey;
                    	FMainDS.MExtract.Rows.Add(NewRow);
                    	
                        // Refresh DataGrid to show the added partner record
                        grdDetails.Refresh();
                        
                        // select the added partner record in the grid so the user can see the change
                        SelectByPartnerKey(PartnerKey,ResultLocationPK.SiteKey);
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling PartnerFindScreen Delegate!",
                        exp);
                }
                // end try
            }
        }

        private void DeletePartner(System.Object sender, EventArgs e)
        {
            // delete selected record from extract
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete this partner record from the extractvalue ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.PartnerKey.ToString()), Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);

                if (grdDetails.Rows.Count <= 1)
                {
                    // hide details part and disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                    pnlDetails.Visible = false;
                }
            }
            
        }
        
        private void ShowDetails(MExtractRow ARow)
        {
        }
        
	    private bool GetDetailsFromControls(MExtractRow ARow)
	    {
	    	// TODO: dummy implementation of this method so yaml file creation works while pnlDetails has
	    	// no controls yet
	    	return true;
	    }
	    
//        private void NewRow(System.Object sender, EventArgs e)
//        {
//            // TODO
//        }

//        private void DeleteRow(System.Object sender, EventArgs e)
//        {
//            // TODO
//        }

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
                
                // scroll to row
        		grdDetails.ShowCell(new SourceGrid.Position(rowIndex, 0), true);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void SelectByPartnerKey(Int64 APartnerKey, Int64 ASiteKey)
        {
        	Int32 Index = -1;
	        int   ExtractIdTemplate;
	        Int64 PartnerKeyTemplate;
	        Int64 SiteKeyTemplate;
	        
	        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
	        {
	        	// compare key of item in list with key given
	        	ExtractIdTemplate  = Convert.ToInt32((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][0]);
	        	PartnerKeyTemplate = Convert.ToInt64((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][1]);
	        	SiteKeyTemplate    = Convert.ToInt64((grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][2]);

	        	if (   FExtractId  == ExtractIdTemplate
	        	    && APartnerKey == PartnerKeyTemplate
	        	    && ASiteKey    == SiteKeyTemplate)
	        	{
	                Index = Counter + 1;
	                break;
	        	}
	        }
	        
        	// select row with given index
        	SelectByIndex(Index);
        }
        
        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeletePartner(this, null);
            }
        }
        
		#endregion

    }
}