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
        /// Returns the new item count
        /// </summary>
        public int NewCount = 0;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="Count">The new item count</param>
        public CountEventArgs(int Count)
        {
            NewCount = Count;
        }
    }

    /// <summary>
    /// A delegate that can be used to handle a change in the number of items
    /// </summary>
    public delegate void CountChangedEventHandler(object Sender, CountEventArgs e);

    /****************************************************************************************************
    *
    * Our main class methods for Contact Attribute Details
    *
    * **************************************************************************************************/

    public partial class TUC_ContactAttributeDetail
    {
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

        // A variable that keeps track of the current value of the contact code
        private string _currentContactAttribute = String.Empty;

        /// <summary>
        /// Returns the string value for the current Contact Attribute used by the control.
        /// </summary>
        public string ContactAttribute
        {
            get
            {
                return _currentContactAttribute;
            }
        }

        private void InitializeManualCode()
        {
            // Before we start we set the defaultView RowFilter property to something unlikely.
            // The manual code gets a chance to populate the grid before we get our chance to set the correct rowFilter.
            // So this ensures that the grid does not flicker with the wrong rows before we put the right ones in.
            string filter = String.Format("{0}='@#~?!()'", FMainDS.PContactAttributeDetail.ColumnContactAttributeCode.ColumnName);

            FMainDS.PContactAttributeDetail.DefaultView.RowFilter = filter;
        }

        private void NewRowManual(ref PContactAttributeDetailRow ARow)
        {
            string newCode = Catalog.GetString("NEWCODE");
            Int32 countNewCode = 1;

            if (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { _currentContactAttribute, newCode }) != null)
            {
                while (FMainDS.PContactAttributeDetail.Rows.Find(new object[] { _currentContactAttribute,
                                                                                newCode + countNewCode.ToString() }) != null)
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
            if (CreateNewPContactAttributeDetail())
            {
                SetContactAttribute(_currentContactAttribute);
                SelectDetailRowByDataTableIndex(FMainDS.PContactAttributeDetail.Rows.Count - 1);
                OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
                txtDetailContactAttrDetailCode.Focus();
            }
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PContactAttributeDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure that you want to delete the current Contact Detail Attribute?");
            return true;
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
            }
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
                // We must not delete the only row in the grid, nor if the row is read-only
                btnDelete.Enabled = grdDetails.Rows.Count > 2 && !txtDetailContactAttrDetailCode.ReadOnly;
            }
        }

        /// <summary>
        /// Call this method from the parent page's GetDetailDataFromControlsManual method. This will trigger a call to this control's method below
        /// </summary>
        public void GetDetailsFromControls()
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }

        private void GetDetailDataFromControlsManual(PContactAttributeDetailRow ARow)
        {
        }

        /// <summary>
        /// Call this method when the Contact Attribute changes in the Contact Attribute grid on the parent page
        /// </summary>
        /// <param name="NewValue">New value for the contact attribute</param>
        public void SetContactAttribute(string NewValue)
        {
            // Save the current data
            ValidateAllData(true, false);

            // Save the current contact attribute in our member variable
            _currentContactAttribute = NewValue;

            // Use our standard auto-code to create a dataset and bind it to the grid using the correct filter
            FPetraUtilsObject.DisableDataChangedEvent();
            pnlDetails.Enabled = false;

            if (FMainDS.PContactAttributeDetail != null)
            {
                // specify the filter and bind  the data
                string filter = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), NewValue);
                FMainDS.PContactAttributeDetail.DefaultView.RowFilter = filter;
                SelectRowInGrid(1);
            }

            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// Returns the number of detail code items in the database that use the specified attribute code
        /// </summary>
        /// <param name="ForCode">The attribute code for which to return the number of detail codes</param>
        /// <returns></returns>
        public int NumberOfDetails(string ForCode)
        {
            if (FMainDS.PContactAttributeDetail != null)
            {
                // specify the filter, create a view, and return the number of items
                string filter = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), ForCode);
                DataView myDataView = new DataView(FMainDS.PContactAttributeDetail, filter, "", DataViewRowState.CurrentRows);
                return myDataView.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Deletes all the attributes associated with the current contact attribute
        /// </summary>
        public void DeleteAll()
        {
            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = dv.Count - 1; i >= 0; i--)
            {
                dv[i].Delete();
            }

            SelectRowInGrid(1);
            OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
        }

        /// <summary>
        /// Call this method to change the attribute code for the current collection of details
        /// </summary>
        /// <param name="NewCode">The new contact attribute code</param>
        public void ModifyAttributeCode(string NewCode)
        {
            if (NewCode.CompareTo(_currentContactAttribute) == 0)
            {
                return;                                                         // should not happen
            }

            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            // we go round a loop where, as we change the column value, the number of rows in the dataview becomes zero
            int curRow = GetSelectedRowIndex();

            while (dv.Count > 0)
            {
                dv[0][FMainDS.PContactAttributeDetail.ColumnContactAttributeCode.Ordinal] = NewCode;
            }

            // Now we need to display the grid again based on the modified code
            _currentContactAttribute = NewCode;
            string filter = String.Format("{0}='{1}'", PContactAttributeDetailTable.GetContactAttributeCodeDBName(), NewCode);
            FMainDS.PContactAttributeDetail.DefaultView.RowFilter = filter;
            SelectRowInGrid(curRow);

            // The number of rows should be the same as before, so this should be unnecessary!
            OnCountChanged(new CountEventArgs(grdDetails.Rows.Count - 1));
        }

        /// <summary>
        /// Creates an initial attribute detail for a new Attribute.  Call this when a new attribute is created.
        /// </summary>
        /// <param name="AttributeCode">The attribute code associated with the new detail code</param>
        public void CreateFirstAttributeDetail(string AttributeCode)
        {
            _currentContactAttribute = AttributeCode;
            NewRow(null, null);
        }
    }
}