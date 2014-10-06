//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Data.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared;

// The TFrmLocalDataFieldSetup class acts as a base class for three derived classes.
// This is because this screen (and the database table behind it) is used by three different parts
//   of the application.
// 1.  The Partner module
// 2.  The personnel module application tables (short-term and long-term)
// 3.  The personnel module general tables
//
// We define the three sub-classes so the the main menu system can use each form in turn for
//    its ActionOpenScreen.
//
// The 'InitializeManualCode' method allows us to set up the screen differently for each launch.
// But this code runs in the form constructor.  I did try having a public property called 'Context'
//    that the launcher would set, and it did get set - but not until after the constructor code runs.
//
// So this way we use reflection to work out which class wa were launched as ....

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

    // This is the base class from which the other three are derived
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
            Partner = 1,
            Application,
            Personnel
        }
        private Context CurrentContext;

        // These are some constants or semi-constants used by the class
        // ColumnIndex of our new columns in the extended dataset
        private int UsedByColumnOrdinal = -1;
        private int ContextColumnOrdinal = -1;
        private int DataTypeColumnOrdinal = -1;
        // These are the database values for the DataLabelUse table
        // We default to using in all screens - this is better than defaulting to none (which is not allowed)
        private const string DefaultPartnerUsedBy = "Bank,Church,Family,Organisation,Person,Unit,Venue";
        private const string DefaultPersonnelUsedBy = "Personnel";
        private const string DefaultApplicationUsedBy = "LongTermApp,ShortTermApp";

        // UsedBy column title for DB and GUI
        private const string DBUsedBy = "UsedBy";
        private string GUIUsedBy = Catalog.GetString("Used By");

        private const int DefaultCharLength = 24;
        private const int DefaultNumDecimalPlaces = 0;
        private const string DefaultCurrencyCode = "USD";

        // Keep track of the maximum Idx1 value that occurs in the PDataLabelUse table
        private int MaxIdx1Value = 0;

        /// <summary>
        /// This is the simple two column data table used by the list box.
        /// The row content (and column headings) depend on our launch context
        /// </summary>
        public DataTable DTUsedBy = new DataTable();

        // This is the main area where we set up to use the additional information from the extra table
        private void InitializeManualCode()
        {
            // Get our screenClassName
            string ScreenName = this.GetType().Name;

            // Initialise the list box column headings in the working DataTable and the GUI
            string DBCol1 = DBUsedBy;
            string DBCol2 = String.Empty;
            string GUICol2 = String.Empty;
            string DBCol3 = "Label";

            if (String.Compare(ScreenName, "TFrmLocalPartnerDataFieldSetup", true) == 0)
            {
                CurrentContext = Context.Partner;
                DBCol2 = "PartnerClass";
                GUICol2 = Catalog.GetString("Partner Class");
            }
            else if (String.Compare(ScreenName, "TFrmLocalApplicationDataFieldSetup", true) == 0)
            {
                CurrentContext = Context.Application;
                DBCol2 = "Application";
                GUICol2 = Catalog.GetString("Application");
            }
            else if (String.Compare(ScreenName, "TFrmLocalPersonnelDataFieldSetup", true) == 0)
            {
                CurrentContext = Context.Personnel;
                DBCol2 = "Personnel";
                GUICol2 = Catalog.GetString("Personnel");
            }

            // Now we can initialise our little data table that backs the checkbox list view
            DTUsedBy.Columns.Add(DBCol1).DataType = typeof(Boolean);
            DTUsedBy.Columns.Add(DBCol2).DataType = typeof(String);
            DTUsedBy.Columns.Add(DBCol3).DataType = typeof(String);

            // Add a 'Context' column so we can filter the data
            ContextColumnOrdinal = FMainDS.PDataLabel.Columns.Add("Context", typeof(int)).Ordinal;

            // Add a 'Used By' column to our main dataset (Do it twice so we track changes)
            FMainDS.PDataLabel.Columns.Add("UsedByInit", typeof(String));
            UsedByColumnOrdinal = FMainDS.PDataLabel.Columns.Add(DBUsedBy, typeof(String)).Ordinal;

            // Add a 'Converted Data Type' column to the dataset because we want to 'translate' from geeky database entries to friendly user ones
            // This column data is derived from an expression
            string expression = String.Format("IIF(p_data_type_c='char','{0}',", Catalog.GetString("Text"));
            expression += String.Format("IIF(p_data_type_c='float','{0}',", Catalog.GetString("Number"));
            expression += String.Format("IIF(p_data_type_c='currency','{0}',", Catalog.GetString("Currency"));
            expression += String.Format("IIF(p_data_type_c='boolean','{0}',", Catalog.GetString("Yes/No"));
            expression += String.Format("IIF(p_data_type_c='date','{0}',", Catalog.GetString("Date"));
            expression += String.Format("IIF(p_data_type_c='time','{0}',", Catalog.GetString("Time"));
            expression += String.Format("IIF(p_data_type_c='lookup','{0}',", Catalog.GetString("Option List"));
            expression += String.Format("IIF(p_data_type_c='partnerkey','{0}',", Catalog.GetString("Partner Key"));
            expression += String.Format("'{0}'))))))))", Catalog.GetString("Unknown"));
            DataTypeColumnOrdinal = FMainDS.PDataLabel.Columns.Add("DataTypeEx", typeof(System.String), expression).Ordinal;

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

                foreach (PDataLabelUseRow useRow in rows)
                {
                    if (usedBy != String.Empty)
                    {
                        usedBy += ",";
                    }

                    usedBy += useRow.Use;

                    // Update the value of our MaxIdx1Value variable
                    if (useRow.Idx1 > MaxIdx1Value)
                    {
                        MaxIdx1Value = useRow.Idx1;
                    }
                }

                // Initially our two new columns hold the same values, but if we make a change we modify the second one.
                labelRow[UsedByColumnOrdinal - 1] = usedBy;                     // since we added this column manually, it does not have a handy property
                labelRow[UsedByColumnOrdinal] = usedBy;                         // since we added this column manually, it does not have a handy property

                if (usedBy.IndexOf("personnel", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    labelRow[ContextColumnOrdinal] = (int)Context.Personnel;
                }
                else if (usedBy.IndexOf("termapp", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    labelRow[ContextColumnOrdinal] = (int)Context.Application;
                }
                else
                {
                    labelRow[ContextColumnOrdinal] = (int)Context.Partner;
                }
            }

            // So now our main data set has everything we need
            // But the grid doesn't know about our new column yet
            // We do that in 'RunOnceOnActivationManual()'

            // Set the form title and list box content depending on our context
            if (CurrentContext == Context.Partner)
            {
                this.Text = "Maintain Local Partner Data Fields";
                AddRowToUsedByList("Person", Catalog.GetString("Person"));
                AddRowToUsedByList("Family", Catalog.GetString("Family"));
                AddRowToUsedByList("Church", Catalog.GetString("Church"));
                AddRowToUsedByList("Organisation", Catalog.GetString("Organisation"));
                AddRowToUsedByList("Bank", Catalog.GetString("Bank"));
                AddRowToUsedByList("Unit", Catalog.GetString("Unit"));
                AddRowToUsedByList("Venue", Catalog.GetString("Venue"));
            }
            else if (CurrentContext == Context.Application)
            {
                this.Text = "Maintain Local Application Data Fields";
                AddRowToUsedByList("LongTermApp", Catalog.GetString("Long Term"));
                AddRowToUsedByList("ShortTermApp", Catalog.GetString("Short Term"));
            }
            else if (CurrentContext == Context.Personnel)
            {
                this.Text = "Maintain Local Personnel Data Fields";
                AddRowToUsedByList("Personnel", Catalog.GetString("Personnel"));
                clbUsedBy.Visible = false;
            }

            //Set up our checked list box columns and bind to our DTUsedBy table
            clbUsedBy.AddCheckBoxColumn(GUIUsedBy, DTUsedBy.Columns[0], 17, false);
            clbUsedBy.AddTextColumn(GUICol2, DTUsedBy.Columns[2], 169);
            clbUsedBy.DataBindGrid(DTUsedBy, DBCol3, DBCol1, DBCol2, false, false, false);
            FPetraUtilsObject.SetStatusBarText(clbUsedBy, Catalog.GetString("Choose the screens when this label will be used"));

            // Now we have to deal with the form controls that depend on the selection of DataType
            // and we only want one visible at a time - so hide these three
            pnlCurrencyCode.Visible = false;
            pnlLookupCategoryCode.Visible = false;
            pnlNumDecimalPlaces.Visible = false;

            // We can prevent screen 'flicker' by setting the DefaultView RowFilter to some stupid setting that finds no rows
            // This stops the auto-genertaed code populating the list with incorrect data before we get it right in our code
            //  that runs later (RunOnceOnActivationManual)
            // Actually this line was added after completing the rest of the code.  Having added it I could probably
            //   remove a few lines elsewhere, but I am not going to because I don't want to break anything!
            FMainDS.PDataLabel.DefaultView.RowFilter = "Context=0";

            // We need to capture the 'DataSaved' event, so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
        }

        // Simple helper that adds a string item as a row in our UsedBy list
        private void AddRowToUsedByList(string DBItem, string GUIListItem)
        {
            DataRow dr = DTUsedBy.NewRow();

            dr[0] = false;
            dr[1] = DBItem;
            dr[2] = GUIListItem;
            DTUsedBy.Rows.Add(dr);
        }

        private void RunOnceOnActivationManual()
        {
            // Call for validation on the checked list box (which is not associated with the database, so the auto-generated code does not pick this up)
            FPetraUtilsObject.ValidationControlsDict.Add(FMainDS.PDataLabel.Columns[UsedByColumnOrdinal],
                new TValidationControlsData(clbUsedBy, GUIUsedBy));

            // This is the point at which we can add our additional column to the details grid
            grdDetails.AddTextColumn(Catalog.GetString("Data Type"), FMainDS.PDataLabel.Columns[DataTypeColumnOrdinal]);
            grdDetails.AddCheckBoxColumn(Catalog.GetString("Displayed"), FMainDS.PDataLabel.ColumnDisplayed);

            if (CurrentContext != Context.Personnel)
            {
                grdDetails.AddTextColumn(GUIUsedBy, FMainDS.PDataLabel.Columns[DBUsedBy]);
            }

            // And now we can set up our context-specific view and override the original binding to the grid
            // that the generated code did earlier
            // Note that we do not use the table.defaultView property but we create a new view specifically for our context
            // The rows are sorted by Group and code(text)
            DataView contextView = new DataView(FMainDS.PDataLabel,
                "Context=" + ((int)CurrentContext).ToString(), "p_group_c, p_text_c", DataViewRowState.CurrentRows);
            contextView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(contextView);
            grdDetails.Refresh();

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter("Context=" + ((int)CurrentContext).ToString(), true);

            grdDetails.SelectRowWithoutFocus(1);
        }

        private void NewRowManual(ref PDataLabelRow ARow)
        {
            // Get the first available key, which is our unique primary key field
            // Note that this field is not displayed - it is internal to the DB
            Int32 labelKey = 1;

            while (FMainDS.PDataLabel.Rows.Find(new object[] { labelKey }) != null)
            {
                labelKey++;
            }

            ARow.Key = labelKey;

            // Initialise the other values that always apply to new records
            ARow.Text = Catalog.GetString("NewLabel");
            ARow[ContextColumnOrdinal] = (int)CurrentContext;
            ARow[UsedByColumnOrdinal - 1] = String.Empty;

            // Now initialise other values
            PDataLabelRow CurrentRow = GetSelectedDetailRow();

            DTUsedBy.DefaultView.AllowEdit = true;

            if (CurrentRow == null)
            {
                // This is the first row of an empty grid
                // Default to a char type
                ARow.DataType = "char";
                ARow.CharLength = DefaultCharLength;
                ARow.Group = String.Empty;
                txtDetailNumDecimalPlaces.NumberValueInt = DefaultNumDecimalPlaces;
                cmbDetailCurrencyCode.SetSelectedString(DefaultCurrencyCode);
                cmbDetailLookupCategoryCode.SelectedIndex = (cmbDetailLookupCategoryCode.Count > 0) ? 0 : -1;

                switch (CurrentContext)
                {
                    case Context.Partner:
                        ARow[UsedByColumnOrdinal] = DefaultPartnerUsedBy;
                        break;

                    case Context.Application:
                        ARow[UsedByColumnOrdinal] = DefaultApplicationUsedBy;
                        break;

                    case Context.Personnel:
                        ARow[UsedByColumnOrdinal] = DefaultPersonnelUsedBy;
                        break;
                }

                // Check all the boxes
                clbUsedBy.SetCheckedStringList(clbUsedBy.GetAllStringList());
            }
            else
            {
                // New row in a grid that has existing rows, so default to values from the current row
                GetDetailsFromControls(CurrentRow);
                ARow.Group = CurrentRow.Group;
                ARow.DataType = CurrentRow.DataType;
                int VisibleIndex = -1;

                if (String.Compare(ARow.DataType, "char", true) == 0)
                {
                    if (!CurrentRow.IsCharLengthNull())
                    {
                        ARow.CharLength = CurrentRow.CharLength;
                        VisibleIndex = 0;
                    }
                }
                else if (String.Compare(ARow.DataType, "float", true) == 0)
                {
                    if (!CurrentRow.IsNumDecimalPlacesNull())
                    {
                        ARow.NumDecimalPlaces = CurrentRow.NumDecimalPlaces;
                        VisibleIndex = 1;
                    }
                }
                else if (String.Compare(ARow.DataType, "currency", true) == 0)
                {
                    if (!CurrentRow.IsCurrencyCodeNull())
                    {
                        ARow.CurrencyCode = CurrentRow.CurrencyCode;
                        VisibleIndex = 2;
                    }
                }
                else if (String.Compare(ARow.DataType, "lookup", true) == 0)
                {
                    if (!CurrentRow.IsLookupCategoryCodeNull())
                    {
                        ARow.LookupCategoryCode = CurrentRow.LookupCategoryCode;
                        VisibleIndex = 6;
                    }
                }

                // Now set the hidden fields to default values in case the user selects them
                if (VisibleIndex != 0)
                {
                    txtDetailCharLength.NumberValueInt = DefaultCharLength;
                }

                if (VisibleIndex != 1)
                {
                    txtDetailNumDecimalPlaces.NumberValueInt = DefaultNumDecimalPlaces;
                }

                if (VisibleIndex != 2)
                {
                    cmbDetailCurrencyCode.SetSelectedString(DefaultCurrencyCode);
                }

                if (VisibleIndex != 6)
                {
                    cmbDetailLookupCategoryCode.SelectedIndex = (cmbDetailLookupCategoryCode.Count > 0) ? 0 : -1;
                }

                ARow[UsedByColumnOrdinal] = clbUsedBy.GetCheckedStringList();
            }
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPDataLabel();
        }

        // We have to have our own handler for the delete button because we need to check references
        //  while making an allowance for any reference to PDataLabelUse.
        // All the UsedBy checkboxes are backed by the PdataLLabel Use table so we know that these references exist.
        // The DeleteRecordManual method test for reference conflicts but allows for PDataLabelUse.
        // If all is ok it calls the standard code, which does NOT check for conflicts.
        private void DeleteRecordManual(Object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow == null) || (FPrevRowChangedRow == -1))
            {
                return;
            }

            DataRowView[] HighlightedRows = grdDetails.SelectedDataRowsAsDataRowView;

            if ((HighlightedRows.Length == 1) && (!TVerificationHelper.IsNullOrOnlyNonCritical(FPetraUtilsObject.VerificationResultCollection)))
            {
                // If we only have 1 row highlighted and it has validation errors we can quit because the standard code will work fine
                return;
            }

            List <string>listConflicts = new List <string>();

            this.Cursor = Cursors.WaitCursor;

            foreach (DataRowView rv in HighlightedRows)
            {
                TVerificationResultCollection VerificationResults = null;

                // Get the number of references for the row, and the number of rows in PDataLabelUse
                int NumReferences = TRemote.MCommon.ReferenceCount.WebConnectors.GetCacheableRecordReferenceCount(
                    "DataLabelList",
                    DataUtilities.GetPKValuesFromDataRow(rv.Row),
                    FPetraUtilsObject.MaxReferenceCountOnDelete,
                    out VerificationResults);
                int NumDataLabelUses = rv.Row[UsedByColumnOrdinal].ToString().Split(new char[] { ',' }).Length;

                // If there are more references we need to build a message and add it to our list
                if ((NumReferences > NumDataLabelUses) && (VerificationResults != null) && (VerificationResults.Count > 0))
                {
                    string groupName = rv.Row[FMainDS.PDataLabel.ColumnGroup].ToString();
                    string labelName = rv.Row[FMainDS.PDataLabel.ColumnText].ToString();
                    string strRowID = groupName;

                    if (strRowID != String.Empty)
                    {
                        strRowID += " - ";
                    }

                    strRowID += labelName;

                    string msg = Messages.BuildMessageFromVerificationResult(
                        String.Format(
                            Catalog.GetString("The record '{0}' cannot be deleted!{1}{2}"),
                            strRowID,
                            Environment.NewLine,
                            Catalog.GetPluralString("Reason:", "Reasons:", VerificationResults.Count)),
                        VerificationResults);
                    msg += Catalog.GetString(
                        "You can ignore the references to the 'Data Label Use' table.  They will be removed automatically.  But you must resolve the other references before this row can be deleted.");

                    listConflicts.Add(msg);
                }
            }

            this.Cursor = Cursors.Default;

            // Did we get any conflicts?
            for (int i = 0; i < listConflicts.Count; i++)
            {
                if (i < listConflicts.Count - 1)
                {
                    // There is another one to show after this so include a chance for the user to quit...
                    if (MessageBox.Show(listConflicts[i] + Environment.NewLine + Environment.NewLine + "Do you want to see the next conflict?",
                            Catalog.GetString("Record Deletion"), MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        break;
                    }
                }
                else
                {
                    MessageBox.Show(listConflicts[i], Catalog.GetString("Record Deletion"));
                }
            }

            if (listConflicts.Count == 0)
            {
                // All OK to delete the highlighted row(s) so call the standard method
                DeletePDataLabel();
            }
        }

        private bool PreDeleteManual(PDataLabelRow ARowToDelete, ref string ADeletionQuestion)
        {
            string question = ADeletionQuestion;

            question += (Environment.NewLine + Environment.NewLine + "(");

            if (txtDetailGroup.Text.Length > 0)
            {
                question += String.Format("{0}: {1},  ", Catalog.GetString("Group"), txtDetailGroup.Text);
            }

            question += String.Format("{0} {1})", lblDetailText.Text, txtDetailText.Text);

            int classesCount = ARowToDelete[UsedByColumnOrdinal].ToString().Split(new char[] { ',' }).Length;
            string s = Catalog.GetPluralString("{0}{0}This {1} is used in {2} Partner Class",
                "{0}{0}This {1} is used in {2} Partner Classes",
                classesCount);
            question += String.Format(s, Environment.NewLine, lblDetailText.Text.Substring(0, lblDetailText.Text.Length - 1), classesCount);

            ADeletionQuestion = question;

            return true;
        }

        private bool DeleteRowManual(PDataLabelRow ARowToDelete, ref string ACompletionMessage)
        {
            bool ReturnValue = false;

            // We need to delete the rows in PDataLabelUse that reference the current row
            int key = ARowToDelete.Key;

            DataRow[] MatchingRows = FExtraDS.PDataLabelUse.Select("p_data_label_key_i=" + key.ToString());

            foreach (DataRow row in MatchingRows)
            {
                row.Delete();
            }

            // Now we can delete the row in the main table
            ARowToDelete.Delete();
            ReturnValue = true;

            return ReturnValue;
        }

        private void ShowDetailsManual(PDataLabelRow ARow)
        {
            // In this special case we have to handle an empty grid differently from normal
            // Because we applied a rowFilter to the dataset during activation, the panel may contain initial information
            // about a row that has now been filtered out.
            if (ARow == null)
            {
                txtDetailText.Text = String.Empty;
                txtDetailGroup.Text = String.Empty;
                cmbDetailDataType.SelectedIndex = 0;
                txtDetailCharLength.NumberValueInt = DefaultCharLength;
                txtDetailDescription.Text = String.Empty;
                pnlDetails.Enabled = false;
                return;
            }

            int defaultCategoryCode = (cmbDetailLookupCategoryCode.Count > 0) ? 0 : -1;

            // Now 'translate' the database values like 'float' to comboBox values like 'Number'
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

            // Initialise the hidden controls to something sensible, in case they get shown
            if (cmbDetailDataType.SelectedIndex != 0)
            {
                txtDetailCharLength.NumberValueInt = DefaultCharLength;
            }

            if (cmbDetailDataType.SelectedIndex != 1)
            {
                txtDetailNumDecimalPlaces.NumberValueInt = DefaultNumDecimalPlaces;
            }

            if (cmbDetailDataType.SelectedIndex != 2)
            {
                cmbDetailCurrencyCode.SetSelectedString(DefaultCurrencyCode);
            }

            if (cmbDetailDataType.SelectedIndex != 6)
            {
                cmbDetailLookupCategoryCode.SelectedIndex = defaultCategoryCode;
            }

            // Set the checkboxes in the UsedBy list
            DTUsedBy.DefaultView.AllowEdit = true;
            clbUsedBy.SetCheckedStringList(ARow[UsedByColumnOrdinal].ToString());
        }

        private void GetDetailDataFromControlsManual(PDataLabelRow ARow)
        {
            // Translate comboBox items like 'Number' back to database entries like 'float'
            // and make sure that unused fields in the database are set to null (where controls are currently hidden)
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

            // Set all fields to null where the control is not visible, because the information is not required
            if (!txtDetailCharLength.Visible)
            {
                ARow.SetCharLengthNull();
            }

            if (!txtDetailNumDecimalPlaces.Visible)
            {
                ARow.SetNumDecimalPlacesNull();
            }

            if (!cmbDetailCurrencyCode.Visible)
            {
                ARow.SetCurrencyCodeNull();
            }

            if (!cmbDetailLookupCategoryCode.Visible)
            {
                ARow.SetLookupCategoryCodeNull();
            }

            // Get the checked items from the UsedBy ListBox and update our UsedBy column
            ARow[UsedByColumnOrdinal] = clbUsedBy.GetCheckedStringList();
        }

        void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            // We need to delete the rows in the extra data set first so that we will be able to save the deleted rows in the main data set
            DataTable DeletedDT = FExtraDS.PDataLabelUse.GetChanges(DataRowState.Deleted);

            if (DeletedDT == null)
            {
                return;
            }

            TTypedDataTable SubmitDT = (TTypedDataTable)DeletedDT;
            SubmitDT.InitVars();

            SaveDataLabelUseChanges(SubmitDT);
        }

        void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            // Do not save anything if the main table did not save correctly
            if (!e.Success)
            {
                return;
            }

            // Ensure we get the current row's information
            if (FPreviouslySelectedDetailRow != null)
            {
                FPreviouslySelectedDetailRow[UsedByColumnOrdinal] = clbUsedBy.GetCheckedStringList();
            }

            // Now we need to save the PDataLabelUse table info using our data from our UsedBy column
            // Go round all the rows, seeing which rows have a new UsedBy value
            foreach (PDataLabelRow labelRow in FMainDS.PDataLabel.Rows)
            {
                if ((labelRow.RowState != DataRowState.Deleted)
                    && (labelRow[UsedByColumnOrdinal].ToString() != labelRow[UsedByColumnOrdinal - 1].ToString()))
                {
                    // This row's UsedBy column has been edited
                    // Get the key and the list of usedBy's for this row
                    int key = labelRow.Key;
                    string usedByList = labelRow[UsedByColumnOrdinal].ToString();
                    string[] uses = usedByList.Split(',');

                    // Get the usedBy's that are in the database at the moment
                    DataRow[] UseRows = FExtraDS.PDataLabelUse.Select("p_data_label_key_i=" + key.ToString());

                    // For each current UsedBy, make sure it has a row in the database.
                    // If not, we need to add a new row, using an Idx1 value greater than anything used before
                    foreach (string use in uses)
                    {
                        bool bUseExistsAlready = false;

                        foreach (DataRow r in UseRows)
                        {
                            string tryUse = r.ItemArray[PDataLabelUseTable.ColumnUseId].ToString();

                            if (String.Compare(tryUse, use, true) == 0)
                            {
                                bUseExistsAlready = true;
                                break;
                            }
                        }

                        if (!bUseExistsAlready)
                        {
                            PDataLabelUseRow newRow = FExtraDS.PDataLabelUse.NewRowTyped();
                            newRow.DataLabelKey = key;
                            newRow.Use = use;
                            newRow.Idx1 = ++MaxIdx1Value;
                            FExtraDS.PDataLabelUse.Rows.Add(newRow);
                        }
                    }

                    // Now go round the other way
                    // Go round each database row and check if its UsedBy is still in our current usedBy List
                    // If we don't find it in the current list we need to delete this row
                    foreach (DataRow r in UseRows)
                    {
                        string tryUse = r.ItemArray[PDataLabelUseTable.ColumnUseId].ToString();
                        bool bUseStillExists = false;

                        foreach (string use in uses)
                        {
                            if (String.Compare(tryUse, use, true) == 0)
                            {
                                bUseStillExists = true;
                                break;
                            }
                        }

                        if (!bUseStillExists)
                        {
                            // We no longer need this row for this usedBy/LabelKey
                            r.Delete();
                        }
                    }
                }
            }

            Ict.Common.Data.TTypedDataTable SubmitDT = FExtraDS.PDataLabelUse.GetChangesTyped();

            if (SubmitDT == null)
            {
                return;                                 // nothing to save
            }

            SaveDataLabelUseChanges(SubmitDT);
        }

        private void SaveDataLabelUseChanges(TTypedDataTable ASubmitChanges)
        {
            // Submit changes to the PETRAServer for the DataLabelUse table
            // This code is basically lifted from a typical auto-generated equivalent
            // TODO: If the standard code changes because TODO's get done, we will need to change this manual code
            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection VerificationResult;

            try
            {
                SubmissionResult = TDataCache.SaveChangedCacheableDataTableToPetraServer("DataLabelUseList",
                    ref ASubmitChanges,
                    out VerificationResult);
            }
            catch (ESecurityDBTableAccessDeniedException Exp)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;

                TMessages.MsgSecurityException(Exp, this.GetType());

                return;
            }
            catch (EDBConcurrencyException Exp)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;

                TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                return;
            }
            catch (Exception exp)
            {
                TLogging.Log(
                    "An error occured while saving the 'used by' data" + Environment.NewLine + exp.ToString(),
                    TLoggingType.ToLogfile);
                MessageBox.Show(
                    Catalog.GetString("An error occured while saving the 'used by' data") + Environment.NewLine +
                    Catalog.GetString("For details see the log file: ") + TLogging.GetLogFileName(),
                    Catalog.GetString("Failed to Save 'Used By' Data"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            switch (SubmissionResult)
            {
                case TSubmitChangesResult.scrOK:

                    // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                    FExtraDS.PDataLabelUse.AcceptChanges();

                    // Merge back with data from the Server (eg. for getting Sequence values)
                    ASubmitChanges.AcceptChanges();
                    FExtraDS.PDataLabelUse.Merge(ASubmitChanges, false);

                    // need to accept the new modification ID
                    FExtraDS.PDataLabelUse.AcceptChanges();

                    // need to refresh the cacheable DataTable 'DataLabelsForPartnerClassesList' (used by Partner Find's Maintain Menu)
                    TDataCache.TMPartner.RefreshCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelsForPartnerClassesList);

                    return;

                case TSubmitChangesResult.scrNothingToBeSaved:

                    return;

                case TSubmitChangesResult.scrError:

                    MessageBox.Show(Catalog.GetString(
                        "The 'UsedBy' part of the data could not be saved! There has been an error while making changes to the table."),
                    Catalog.GetString("Submit Changes to Table Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    break;

                case TSubmitChangesResult.scrInfoNeeded:

                    MessageBox.Show(Catalog.GetString(
                        "The 'UsedBy' part of the data could not be saved! Insufficient information was provided when making changes to the table."),
                    Catalog.GetString("Submit Changes to Table Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            if ((cmbDetailDataType.SelectedIndex < 0) || (cmbDetailDataType.SelectedIndex > 7))
            {
                return;
            }

            // Start by hiding everything
            pnlCharLength.Visible = false;
            pnlNumDecimalPlaces.Visible = false;
            pnlCurrencyCode.Visible = false;
            pnlLookupCategoryCode.Visible = false;

            // Show the relevant panel or text box and modify the label text
            switch (cmbDetailDataType.SelectedIndex)
            {
                case 0:                 // Text
                    pnlCharLength.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Maximum length") + ":";
                    break;

                case 1:                 // Numeric
                    pnlNumDecimalPlaces.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Decimal places") + ":";
                    break;

                case 2:                 // Currency
                    pnlCurrencyCode.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Currency code") + ":";
                    break;

                case 3:                 // Yes/No
                case 4:                 // Date
                case 5:                 // Time
                case 7:                 // PartnerKey
                    lblDataSubType.Text = String.Empty;
                    break;

                case 6:                 // OptionList
                    pnlLookupCategoryCode.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Option list name") + ":";
                    break;
            }
        }

        private void ValidateDataDetailsManual(PDataLabelRow ARow)
        {
            // For this validation we have to validate the UsedBy data here in the manual code.
            // This is because it is not backed directly by a row in a data table.
            // Nor is the control associated with a column in any data table
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult = null;

            // Personnel context is bound to be valid because it has no UsedBy UI
            if ((CurrentContext == Context.Partner) || (CurrentContext == Context.Application))
            {
                // The added column at the end of the table, which is a concatenated string of checkedListBox entries, must not be empty
                ValidationColumn = ARow.Table.Columns[UsedByColumnOrdinal];
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow[UsedByColumnOrdinal].ToString(),
                    GUIUsedBy,
                    this, ValidationColumn, clbUsedBy);

                if (VerificationResult != null)
                {
                    if (CurrentContext == Context.Partner)
                    {
                        VerificationResult.OverrideResultText(Catalog.GetString("You must check at least one box in the list of Partner classes."));
                    }
                    else if (CurrentContext == Context.Application)
                    {
                        VerificationResult.OverrideResultText(Catalog.GetString("You must check at least one box in the list of Application types."));
                    }
                }

                // Handle addition to/removal from TVerificationResultCollection.
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, false);
            }

            // Now call the central validation routine for the other verification tasks
            TSharedValidation_CacheableDataTables.ValidateLocalDataFieldSetup(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void CreateFilterFindPanelsManual()
        {
            if (CurrentContext == Context.Partner)
            {
                // For Partner we create a bunch of checkboxes
                AddCheckBoxFilter("Person");
                AddCheckBoxFilter("Family");
                AddCheckBoxFilter("Church");
                AddCheckBoxFilter("Organisation");
                AddCheckBoxFilter("Bank");
                AddCheckBoxFilter("Unit");
                AddCheckBoxFilter("Venue");
            }
        }

        private void AddCheckBoxFilter(string AnItemName)
        {
            TIndividualFilterFindPanel iffp;

            CheckBox chkAny = new CheckBox();

            chkAny.Name = "chk" + AnItemName;
            chkAny.Text = AnItemName;

            iffp = new TIndividualFilterFindPanel(
                null,
                TCloneFilterFindControl.ShallowClone <CheckBox>(chkAny, TFilterPanelControls.FILTER_NAME_SUFFIX),
                null,
                "bit",
                "HasManualFilter");
            FFilterAndFindObject.FilterPanelControls.FStandardFilterPanels.Add(iffp);
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            if (CurrentContext == Context.Partner)
            {
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkPerson"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkFamily"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkChurch"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkOrganisation"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkBank"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkUnit"), ref AFilterString);
                ApplyFilterFromCheckBox((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByName("chkVenue"), ref AFilterString);
            }
        }

        private void ApplyFilterFromCheckBox(CheckBox ACheckBox, ref string AFilterString)
        {
            if (ACheckBox.CheckState == CheckState.Indeterminate)
            {
                return;
            }

            if (AFilterString.Length > 0)
            {
                AFilterString += " AND ";
            }

            if (ACheckBox.Checked)
            {
                AFilterString += String.Format("({0} LIKE '%{1}%')", DBUsedBy, ACheckBox.Text);
            }
            else
            {
                AFilterString += String.Format("NOT ({0} LIKE '%{1}%')", DBUsedBy, ACheckBox.Text);
            }
        }
    }
}