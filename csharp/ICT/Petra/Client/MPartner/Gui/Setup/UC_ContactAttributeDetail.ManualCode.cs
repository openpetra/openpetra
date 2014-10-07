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
using System.Data;

using Ict.Common;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    /*************************************************************************************************
     *
     * An EventArgs subclass we can use to notify our parent of changes in our grid
     *
     * **********************************************************************************************/

    /// <summary>
    /// An eventargs class that knows the current number of items
    /// </summary>
    public class CountEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new item count.
        /// </summary>
        public int NewCount = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Count">New item count.</param>
        public CountEventArgs(int Count)
        {
            NewCount = Count;
        }
    }

    /// <summary>
    /// A Delegate that can be used to handle a change in the number of items
    /// </summary>
    public delegate void CountChangedEventHandler(object Sender, CountEventArgs e);

    /****************************************************************************************************
    *
    * Our main class methods for Contact Attribute Details
    *
    * **************************************************************************************************/

    public partial class TUC_ContactAttributeDetail
    {
        // Keeps track of the current value of the Contact Attribute
        private string FContactAttribute = String.Empty;

        /// <summary>
        /// Raised when there are no more detail records held after the last
        /// detail record has beend deleted.
        /// </summary>
        public event EventHandler<EventArgs> NoMoreDetailRecords;
        
        /// <summary>
        /// An event that will be fired when the number of rows in the details grid changes
        /// </summary>
        public event CountChangedEventHandler CountChanged;

        /// <summary>
        /// Call this method when the number of rows in the details grid changes
        /// </summary>
        /// <param name="e">A CountEventArgs that has 'NewCount' specified</param>
        protected virtual void OnCountChanged(CountEventArgs e)
        {
            if (CountChanged != null)
            {
                CountChanged(this, e);
            }
        }

        /// <summary>
        /// The Contact Details maintained in this UserControl are for this Contact Attribute.
        /// </summary>
        public string ContactAttribute
        {
            get
            {
                return FContactAttribute;
            }
        }

        /// <summary>
        /// The number of values in the grid for the current Contact Attribute.  This may not be the full number if the grid is filtered.
        /// </summary>
        public int GridCount
        {
            get
            {
                return grdDetails.Rows.Count - 1;
            }
        }

        /// <summary>
        /// The unfiltered number of values for the current Contact Attribute.
        /// </summary>
        public int Count
        {
            get
            {
                // Need to create our own view because the grid may be filtered
                return new DataView(FMainDS.PContactAttributeDetail, 
                                 PContactAttributeDetailTable.GetContactAttributeCodeDBName() + 
                                 " = '" + FContactAttribute + "'",
                                 "", DataViewRowState.CurrentRows).Count;
            }
        }        
        
        private void InitializeManualCode()
        {
            // Before we start we set the defaultView RowFilter property to something unlikely.
            // The manual code gets a chance to populate the grid before we get our chance to set the correct rowFilter.
            // So this ensures that the grid does not flicker with the wrong rows before we put the right ones in.
            string FilterStr = String.Format("{0}='@#~?!()'", FMainDS.PContactAttributeDetail.ColumnContactAttributeCode.ColumnName);

            FMainDS.PContactAttributeDetail.DefaultView.RowFilter = FilterStr;
        }

        private void NewRow(Object sender, EventArgs e)
        {
            if (CreateNewPContactAttributeDetail())
            {
                SetContactAttribute(FContactAttribute);

                OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
                
                txtDetailContactAttrDetailCode.ReadOnly = false;
                
                txtDetailContactAttrDetailCode.Focus();
            }
        }

        private void NewRowManual(ref PContactAttributeDetailRow ARow)
        {
            string NewName = Catalog.GetString("NEWDETAIL");
            Int32 CountNewDetail = 0;

            if (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { FContactAttribute, NewName }) != null)
            {
                while (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { FContactAttribute,
                                                                                  NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.ContactAttrDetailCode = NewName;
            ARow.ContactAttributeCode = FContactAttribute;
        }

        /// <summary>
        /// Call this method from the parent page's GetDetailDataFromControls Manual method. This will trigger a call to this control's method below
        /// </summary>
        public void GetDetailsFromControls()
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
        
        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PContactAttributeDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
                
                // If we have no Attribute Details anymore: Inform the Form
                if (this.Count == 0) 
                {
                    OnNoMoreDetailRecords(null);
                }                    
            }
        }
        
        /// <summary>
        /// Call this method when the Contact Attribute changes in the Contact Attribute grid on the parent Form.
        /// </summary>
        /// <param name="ANewCode">The Contact Attribute code as in the parent Form.</param>
        public void SetContactAttribute(string ANewCode)
        {
            // Save the current data
            ValidateAllData(true, false);

            // Save the current contact attribute in our member variable
            FContactAttribute = ANewCode;

            FPetraUtilsObject.DisableDataChangedEvent();
            
            pnlDetails.Enabled = false;

            if (FMainDS.PContactAttributeDetail != null)
            {
                FilterOnCode(ANewCode, GetSelectedRowIndex());
            }

            FPetraUtilsObject.EnableDataChangedEvent();

            UpdateRecordNumberDisplay();
        }

        /// <summary>
        /// Call this method to change the Contact Attribute code for all Contact Details that are presently held in this UserControl
        /// </summary>
        /// <param name="ANewCode">New value for the Contact Attribute.</param>
        public void ModifyAttributeCode(string ANewCode)
        {
            if (ANewCode.CompareTo(FContactAttribute) == 0)  // should not happen
            {
                return;                                                         
            }

            DataView UpdateRowsDV = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            // We go round a loop where, as we change the column value, the number of rows in the dataview becomes zero
            int CurrentRowIndex = GetSelectedRowIndex();

            while (UpdateRowsDV.Count > 0)
            {
                UpdateRowsDV[0][FMainDS.PContactAttributeDetail.ColumnContactAttributeCode.Ordinal] = ANewCode;
            }

            FContactAttribute = ANewCode;

            // Now we need to display the grid again based on the modified code
            FilterOnCode(ANewCode, CurrentRowIndex);
            
            // The number of rows should be the same as before, so this should be unnecessary!
            OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
        }

        /// <summary>
        /// Specifies a new Filter and applies it, then selects the Row passed in with <paramref name="ACurrentRowIndex"/>.
        /// </summary>
        /// <param name="ANewCode">New Code to filter on.</param>
        /// <param name="ACurrentRowIndex">The index of the Row that should get displayed (the 'current' Row).</param>
    	private void FilterOnCode(string ANewCode, int ACurrentRowIndex)
    	{
    	    string FilterStr = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), ANewCode);
     	    
     	    FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FilterStr, true);			
    	    FFilterAndFindObject.ApplyFilter();
    			
    	    grdDetails.SelectRowWithoutFocus(ACurrentRowIndex);
    	}
                
        /// <summary>
        /// Returns the number of detail code items in the database that use the specified attribute code
        /// </summary>
        /// <param name="AForCode">The attribute code for which to return the number of detail codes</param>
        /// <returns></returns>
        public int NumberOfDetails(string AForCode)
        {
            if (FMainDS.PContactAttributeDetail != null)
            {
                // specify the filter, create a view, and return the number of items
                string filter = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), AForCode);
                DataView myDataView = new DataView(FMainDS.PContactAttributeDetail, filter, "", DataViewRowState.CurrentRows);
                
                return myDataView.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Creates an initial Attribute Detail for a new Contact Attribute.  Call this when a new Contact Attribute is created.
        /// </summary>
        /// <param name="AttributeCode">The Attribute Code associated with the new Contact Attribute.</param>
        public void CreateFirstAttributeDetail(string AttributeCode)
        {
            FContactAttribute = AttributeCode;
            
            NewRow(null, null);
        }
        
        /// <summary>
        /// Raises the 'NoMoreDetailRecords' Event.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
	protected virtual void OnNoMoreDetailRecords(EventArgs e)
	{
		var Eventhandler = NoMoreDetailRecords;
			
		if (Eventhandler != null)
		{
			Eventhandler(this, e);
		}
	}        
    }
}