//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
//
// Copyright 2004-2014 by OM International
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
using System.Data;

using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TUC_ContactCategoriesAndTypesDetail
    {
        // Keeps track of the current value of the Contact Category
        private String FContactCategory;

        PPartnerAttributeCategoryTable FPartnerAttributeCategoryDT;

        // Instance of a 'Helper Class' for handling the Indexes of the DataRows. (The Grid is sorted by the Index.)
        TSgrdDataGrid.IndexedGridRowsHelper FIndexedGridRowsHelper;

        /// <summary>
        /// Raised when there are no more detail records held after the last
        /// detail record has beend deleted.
        /// </summary>
        public event EventHandler <EventArgs>NoMoreDetailRecords;

        /// <summary>
        /// The Contact Types maintained in this UserControl are for this Contact Category.
        /// </summary>
        public String ContactCategory
        {
            get
            {
                return FContactCategory;
            }
        }

        /// <summary>
        /// The number of values in the grid for the current Contact Category.  This may not be the full number if the grid is filtered.
        /// </summary>
        public int GridCount
        {
            get
            {
                return grdDetails.Rows.Count - 1;
            }
        }

        /// <summary>
        /// The unfiltered number of values for the current Contact Category.
        /// </summary>
        public int Count
        {
            get
            {
                // Need to create our own view because the grid may be filtered
                return new DataView(FMainDS.PPartnerAttributeType,
                    PPartnerAttributeTypeTable.GetAttributeCategoryDBName() +
                    " = '" + FContactCategory + "'",
                    "", DataViewRowState.CurrentRows).Count;
            }
        }

        private void InitializeManualCode()
        {
            // Initialize 'Helper Class' for handling the Indexes of the DataRows.
            FIndexedGridRowsHelper = new TSgrdDataGrid.IndexedGridRowsHelper(
                grdDetails, PPartnerAttributeTypeTable.ColumnIndexId, btnDemote, btnPromote,
                delegate { FPetraUtilsObject.SetChangedFlag(); });

            // Before we start we set the defaultView RowFilter property to something unlikely.
            // The manual code gets a chance to populate the grid before we get our chance to set the correct rowFilter.
            // So this ensures that the grid does not flicker with the wrong rows before we put the right ones in.
            string FilterStr = String.Format("{0}='@#~?!()'", FMainDS.PPartnerAttributeType.ColumnAttributeCategory.ColumnName);

            FMainDS.PPartnerAttributeType.DefaultView.RowFilter = FilterStr;

            lblLinkFormatTip.Text = Catalog.GetString("Enter the URL that should be launched for the Contact Type ( e.g. http://www.facebook.com/" +
                THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER + " ).");
            lblLinkFormatTip.Font = new System.Drawing.Font(lblLinkFormatTip.Font.FontFamily, 7, FontStyle.Regular);
            lblLinkFormatTip.Top -= 5;

            pnlDetails.MinimumSize = new Size(700, 145);              // To prevent shrinkage!

            /* fix tab order */
            pnlButtons.TabIndex = grdDetails.TabIndex + 1;
        }

        private void RunOnceOnParentActivationManual()
        {
            // Hide 'Index' Grid Column - it is only used for debugging
            grdDetails.Columns.HideColumn(5);
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            if (CreateNewPPartnerAttributeType())
            {
                SetCategoryCode(FContactCategory);

                txtDetailCode.ReadOnly = false;

                txtDetailCode.Focus();
            }
        }

        private void NewRowManual(ref PPartnerAttributeTypeRow ARow)
        {
            string NewName = Catalog.GetString("NEWTYPE");
            Int32 CountNewDetail = 0;

            if (FMainDS.PPartnerAttributeType.Rows.Find(new object[] { FContactCategory, NewName }) != null)
            {
                while (FMainDS.PPartnerAttributeType.Rows.Find(new object[] { FContactCategory,
                                                                              NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.Code = NewName;
            ARow.SpecialLabel = "PLEASE ENTER LABEL";
            ARow.AttributeCategory = FContactCategory;
            ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
            ARow.Deletable = true;  // all manually created Contact Types are deletable

            // Determine and set the 'Index' (ARow.Index in this case) of the new Row
            FIndexedGridRowsHelper.DetermineIndexForNewRow(ARow);

            cmbDetailAttributeTypeValueKind.SelectedIndex = 1;
        }

        private void ShowDetailsManual(PPartnerAttributeTypeRow ARow)
        {
            if (ARow == null)
            {
                cmbDetailAttributeTypeValueKind.SelectedIndex = 0;

                return;
            }
            else
            {
                switch (ARow.AttributeTypeValueKind)
                {
                    case "CONTACTDETAIL_GENERAL":
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 0;
                        break;

                    case "CONTACTDETAIL_HYPERLINK":
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 2;
                        break;

                    case "CONTACTDETAIL_HYPERLINK_WITHVALUE":
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 3;
                        break;

                    case "CONTACTDETAIL_EMAILADDRESS":
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 1;
                        break;

                    case "CONTACTDETAIL_SKYPEID":
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 4;
                        break;

                    default:
                        cmbDetailAttributeTypeValueKind.SelectedIndex = 0;
                        break;
                }
            }

            FIndexedGridRowsHelper.UpdateButtons(GetSelectedRowIndex());
        }

        /// <summary>
        /// Call this method from the parent page's GetDetailDataFromControls Manual method. This will trigger a call to this control's method below
        /// </summary>
        public void GetDetailsFromControls()
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeTypeRow ARow)
        {
            switch (cmbDetailAttributeTypeValueKind.SelectedIndex)
            {
                case 0:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
                    // Remove any HyperlinkFormat that might have been set if the record previously held AttributeTypeValueKind "CONTACTDETAIL_HYPERLINK_WITHVALUE"
                    ARow.HyperlinkFormat = String.Empty;

                    break;

                case 1:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_EMAILADDRESS";
                    // Remove any HyperlinkFormat that might have been set if the record previously held AttributeTypeValueKind "CONTACTDETAIL_HYPERLINK_WITHVALUE"
                    ARow.HyperlinkFormat = String.Empty;

                    break;

                case 2:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_HYPERLINK";
                    // Remove any HyperlinkFormat that might have been set if the record previously held AttributeTypeValueKind "CONTACTDETAIL_HYPERLINK_WITHVALUE"
                    ARow.HyperlinkFormat = String.Empty;

                    break;

                case 3:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_HYPERLINK_WITHVALUE";

                    break;

                case 4:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_SKYPEID";
                    // Remove any HyperlinkFormat that might have been set if the record previously held AttributeTypeValueKind "CONTACTDETAIL_HYPERLINK_WITHVALUE"
                    ARow.HyperlinkFormat = String.Empty;

                    break;

                default:
                    ARow.AttributeTypeValueKind = "CONTACTDETAIL_GENERAL";
                    // Remove any HyperlinkFormat that might have been set if the record previously held AttributeTypeValueKind "CONTACTDETAIL_HYPERLINK_WITHVALUE"
                    ARow.HyperlinkFormat = String.Empty;

                    break;
            }
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PPartnerAttributeTypeRow ARowToDelete, ref string ADeletionQuestion)
        {
            // If the last Row in the Grid is to be deleted: check if there are added 'Detail' Rows in *other* 'Master' Rows,
            // and if any of those 'Master' Rows was added too, tell the user that data needs to be saved first before deletion
            // of the present 'Detail' Row can go ahead.
            // The reason for that is that the deletion of that last 'Detail' Row will cause the OnNoMoreDetailRecords Event to
            // be raised by the UserControl, which in turn will cause the Form to call the 'SaveChanges' Method of the
            // UserControl before the Form saves its own data. While this in itself is OK, saving in the 'SaveChanges' Method
            // of the UserControl would fail as a 'Master' Row itself was newly added AND it wouldn't be in the DB yet!
            return TDeleteGridRows.MasterDetailFormsSpecialPreDeleteCheck(this.Count,
                FPartnerAttributeCategoryDT, FMainDS.PPartnerAttributeType,
                PPartnerAttributeCategoryTable.GetCategoryCodeDBName(), PPartnerAttributeTypeTable.GetAttributeCategoryDBName());
        }

        private void PostDeleteManual(PPartnerAttributeTypeRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                // If we have no Contact Types anymore: Inform the Form
                if (this.Count == 0)
                {
                    OnNoMoreDetailRecords(null);
                }
            }
        }

        /// <summary>
        /// Call this method when the Contact Category changes in the Contact Category grid on the parent Form.
        /// </summary>
        /// <param name="ANewCode">The Contact Category code as in the parent Form.</param>
        public void SetCategoryCode(string ANewCode)
        {
            // Save the current data
            ValidateAllData(true, false);

            // Save the current contact attribute in our member variable
            FContactCategory = ANewCode;

            FPetraUtilsObject.DisableDataChangedEvent();

            pnlDetails.Enabled = false;

            if (FMainDS.PPartnerAttributeType != null)
            {
                FilterOnCode(ANewCode, GetSelectedRowIndex());
            }

            FPetraUtilsObject.EnableDataChangedEvent();

            UpdateRecordNumberDisplay();
        }

        /// <summary>
        /// Call this method to change the Contact Category for all Contact Types that are presently held in this UserControl.
        /// </summary>
        /// <param name="ANewCode">New code for the Contact Category.</param>
        public void ModifyCategoryCode(string ANewCode)
        {
            if (ANewCode.CompareTo(FContactCategory) == 0)   // should not happen
            {
                return;
            }

            DataView UpdateRowsDV = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            // We go round a loop where, as we change the column value, the number of rows in the dataview becomes zero
            int CurrentRowIndex = GetSelectedRowIndex();

            while (UpdateRowsDV.Count > 0)
            {
                UpdateRowsDV[0][FMainDS.PPartnerAttributeType.ColumnAttributeCategory.Ordinal] = ANewCode;
            }

            FContactCategory = ANewCode;

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
            string FilterStr = String.Format("{0}='{1}'", PPartnerAttributeTypeTable.GetAttributeCategoryDBName(), ANewCode);

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FilterStr, true);
            FFilterAndFindObject.ApplyFilter();

            grdDetails.SelectRowWithoutFocus(ACurrentRowIndex);
        }

        /// <summary>
        /// Creates an initial Contact Type for a new Contact Category.  Call this when a new Contact Category is created.
        /// </summary>
        /// <param name="ACategoryCode">The Category Code associated with the new Contact Category.</param>
        /// <param name="APartnerAttributeCategoryDT">The PartnerAttributeCategory Table held in the Form's FMainDS DataSet.</param>
        public void CreateFirstContactType(string ACategoryCode, PPartnerAttributeCategoryTable APartnerAttributeCategoryDT)
        {
            FContactCategory = ACategoryCode;

            // We need to know about the Partner Attribute Category Table for a check in PreDeleteManual!
            FPartnerAttributeCategoryDT = APartnerAttributeCategoryDT;

            NewRecord(null, null);
        }

        private void ValidateDataDetailsManual(PPartnerAttributeTypeRow ARow)
        {
            var VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateContactTypesSetupManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
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

        private void ContactTypePromote(object sender, EventArgs e)
        {
            FIndexedGridRowsHelper.PromoteRow();
        }

        private void ContactTypeDemote(object sender, EventArgs e)
        {
            FIndexedGridRowsHelper.DemoteRow();
        }

        /// <summary>
        /// Updates the enabled/disabled state of the dtpDetailUnassignableDate TextBox.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailUnassignableDate.Enabled = chkDetailUnassignable.Checked;

            if (!chkDetailUnassignable.Checked)
            {
                dtpDetailUnassignableDate.Date = null;

                // Hide any shown Data Validation ToolTip as the Data Validation ToolTip for an
                // empty Unassignable Date might otherwise be left shown
                FPetraUtilsObject.ValidationToolTip.RemoveAll();
            }
            else
            {
                dtpDetailUnassignableDate.Date = DateTime.Now.Date;
                dtpDetailUnassignableDate.Focus();
            }
        }

        /// <summary>
        /// Updates the enabled/disabled state of the txtDetailHyperlinkFormat TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Ignored.</param>
        private void EnableDisableDetailHyperlinkFormatTextBox(object sender, EventArgs e)
        {
            string SelectedAttributeTypeValueKind;
            var SenderAsComboBox = sender as ComboBox;

            if (SenderAsComboBox != null)
            {
                SelectedAttributeTypeValueKind = SenderAsComboBox.Text;

                if (SelectedAttributeTypeValueKind == "Hyperlink With Value")
                {
                    txtDetailHyperlinkFormat.Visible = true;
                    lblDetailHyperlinkFormat.Visible = true;
                    lblLinkFormatTip.Visible = true;
                }
                else
                {
                    txtDetailHyperlinkFormat.Visible = false;
                    lblDetailHyperlinkFormat.Visible = false;
                    lblLinkFormatTip.Visible = false;
                }
            }
        }
    }
}