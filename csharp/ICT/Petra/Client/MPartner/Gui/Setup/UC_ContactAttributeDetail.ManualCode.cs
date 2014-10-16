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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TUC_ContactAttributeDetail
    {
        // Keeps track of the current value of the Contact Attribute
        private string FContactAttribute = String.Empty;

        PContactAttributeTable FContactAttributeDT;

        /// <summary>
        /// Raised when there are no more detail records held after the last
        /// detail record has beend deleted.
        /// </summary>
        public event EventHandler <EventArgs>NoMoreDetailRecords;

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

            /* fix tab order */
            pnlButtons.TabIndex = grdDetails.TabIndex + 1;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            if (CreateNewPContactAttributeDetail())
            {
                SetContactAttribute(FContactAttribute);

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
        /// Performs checks to determine whether a deletion of the current row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PContactAttributeDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            // If the last Row in the Grid is to be deleted: check if there are added 'Detail' Rows in *other* 'Master' Rows,
            // and if any of those 'Master' Rows was added too, tell the user that data needs to be saved first before deletion
            // of the present 'Detail' Row can go ahead.
            // The reason for that is that the deletion of that last 'Detail' Row will cause the OnNoMoreDetailRecords Event to
            // be raised by the UserControl, which in turn will cause the Form to call the 'SaveChanges' Method of the
            // UserControl before the Form saves its own data. While this in itself is OK, saving in the 'SaveChanges' Method
            // of the UserControl would fail as a 'Master' Row itself was newly added AND it wouldn't be in the DB yet!
            return TDeleteGridRows.MasterDetailFormsSpecialPreDeleteCheck(this.Count,
                FContactAttributeDT, FMainDS.PContactAttributeDetail,
                PContactAttributeTable.GetContactAttributeCodeDBName(), PContactAttributeDetailTable.GetContactAttrDetailCodeDBName());
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
        /// Creates an initial Attribute Detail for a new Contact Attribute.  Call this when a new Contact Attribute is created.
        /// </summary>
        /// <param name="AttributeCode">The Attribute Code associated with the new Contact Attribute.</param>
        /// <param name="AContactAttributeDT">The ContactAttribute Table held in the Form's FMainDS DataSet.</param>
        public void CreateFirstAttributeDetail(string AttributeCode, PContactAttributeTable AContactAttributeDT)
        {
            FContactAttribute = AttributeCode;

            // We need to know about the Contact Attribute Table for a check in PreDeleteManual!
            FContactAttributeDT = AContactAttributeDT;

            NewRecord(null, null);
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