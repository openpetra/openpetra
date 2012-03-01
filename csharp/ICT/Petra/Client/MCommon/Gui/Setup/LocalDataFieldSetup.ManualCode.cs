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
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

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
            clbUsedBy.AddCheckBoxColumn(GUIUsedBy, DTUsedBy.Columns[0], 70, false);
            clbUsedBy.AddTextColumn(GUICol2, DTUsedBy.Columns[2], 125);
            clbUsedBy.DataBindGrid(DTUsedBy, DBCol3, DBCol1, DBCol2, DBCol2, false, false, false);
            FPetraUtilsObject.SetStatusBarText(clbUsedBy, Catalog.GetString("Choose the screens when this label will be used"));

            // Set up the label control that we will use to indicate the current sub-type of data
            // We need to right align the text so it looks nice when we change it
            lblDataSubType.AutoSize = false;
            lblDataSubType.Left = 0;
            lblDataSubType.Width = 170;                         // 5 less than the column width in the YAML file
            lblDataSubType.TextAlign = System.Drawing.ContentAlignment.TopRight;

            // Now we have to deal with the form controls that depend on the selection of DataType
            // and we only want one visible at a time - so hide these three
            pnlCurrencyCode.Visible = false;
            pnlCategoryCode.Visible = false;
            txtDetailNumDecimalPlaces.Visible = false;

            // We need to capture the 'DataSaved' event, so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
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
            // This is the point at which we can add our additional column to the details grid
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

            // The details panel will likely be showing data from the wrong context now that we have applied a rowfilter
            // So we have to make sure the panel is displaying data from the first row
            FPreviouslySelectedDetailRow = null;

            if (GetSelectedDetailRow() == null)
            {
                ShowDetails(null);
            }
            else
            {
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(0));
            }
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
                    ARow.CharLength = CurrentRow.CharLength;
                    VisibleIndex = 0;
                }
                else if (String.Compare(ARow.DataType, "float", true) == 0)
                {
                    ARow.NumDecimalPlaces = CurrentRow.NumDecimalPlaces;
                    VisibleIndex = 1;
                }
                else if (String.Compare(ARow.DataType, "currency", true) == 0)
                {
                    ARow.CurrencyCode = CurrentRow.CurrencyCode;
                    VisibleIndex = 2;
                }
                else if (String.Compare(ARow.DataType, "lookup", true) == 0)
                {
                    ARow.LookupCategoryCode = CurrentRow.LookupCategoryCode;
                    VisibleIndex = 6;
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
            // We use non-standard code here because of our three contexts
            // We cannot call CreateNewPDataLabel() because it won't have the correct RowFilter
            // So we use a modified version of the auto-generated code
            PDataLabelRow NewRow = FMainDS.PDataLabel.NewRowTyped();

            NewRowManual(ref NewRow);
            FMainDS.PDataLabel.Rows.Add(NewRow);

            FPetraUtilsObject.SetChangedFlag();

            DataView contextView = new DataView(FMainDS.PDataLabel, "Context=" + ((int)CurrentContext).ToString(), "", DataViewRowState.CurrentRows);
            contextView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(contextView);
            grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.PDataLabel.Rows.Count - 1);

            //CreateNewPDataLabel();
            txtDetailText.SelectAll();
            txtDetailText.Focus();
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

            if (!pnlCurrencyCode.Visible)
            {
                ARow.SetCurrencyCodeNull();
            }

            if (!pnlCategoryCode.Visible)
            {
                ARow.SetLookupCategoryCodeNull();
            }

            // Validate that there is a local data option category if 'lookup' was selected.
            // If not, use char for now until the user creates a new category
            if ((cmbDetailDataType.SelectedIndex == 6) && (ARow.LookupCategoryCode == String.Empty))
            {
                // That's bad!  Must be no data in the Category Code table
                ARow.DataType = "char";
                ARow.CharLength = DefaultCharLength;
                ARow.SetLookupCategoryCodeNull();
                MessageBox.Show(Catalog.GetString(
                        "You cannot select the 'Lookup Option' because there are no options defined. The application will choose 'Text'.  Then go to the 'Local Data Options' menu item and define some options.  Then return to this menu and reconfigure this entry."),
                    Catalog.GetString("Error in Data Input"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }

            // Get the checked items from the UsedBy ListBox and update our UsedBy column
            // Validate that at least one checkbox is chaecked
            string stringList = clbUsedBy.GetCheckedStringList();

            if (stringList == String.Empty)
            {
                if (CurrentContext == Context.Partner)
                {
                    stringList = "Church";
                }
                else if (CurrentContext == Context.Application)
                {
                    stringList = "LongTermApp";
                }

                MessageBox.Show(Catalog.GetString(
                        "You must check at least one box in the 'Used By' list. The system will check one box for you, but you should validate the entry yourself."),
                    Catalog.GetString("Error in Data Input"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }

            ARow[UsedByColumnOrdinal] = stringList;
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
                string stringList = clbUsedBy.GetCheckedStringList();
                FPreviouslySelectedDetailRow[UsedByColumnOrdinal] = stringList;
            }

            // Now we need to save the PDataLabelUse table info using our data from our UsedBy column
            // Go round all the rows, seeing which rows have a new UsedBy value
            foreach (PDataLabelRow labelRow in FMainDS.PDataLabel.Rows)
            {
                if (labelRow[UsedByColumnOrdinal].ToString() != labelRow[UsedByColumnOrdinal - 1].ToString())
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
                            ((PDataLabelUseRow)r).Delete();
                        }
                    }
                }
            }

            Ict.Common.Data.TTypedDataTable SubmitDT = FExtraDS.PDataLabelUse.GetChangesTyped();

            if (SubmitDT == null)
            {
                return;                                 // nothing to save
            }

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
                MessageBox.Show(Catalog.GetString("The PETRA Server cannot be reached! Data cannot be saved!"),
                    Catalog.GetString("No Server response"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            catch (EDBConcurrencyException)
            {
                MessageBox.Show(Catalog.GetString(
                        "The 'UsedBy' part of the data could not be saved! There has been a conflict with another user's data entry."),
                    Catalog.GetString("Cached Table Data Conflict"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
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

                    MessageBox.Show(Catalog.GetString(
                        "The 'UsedBy' part of the data could not be saved! There has been an error while making changes to the table."),
                    Catalog.GetString("Submit Changes to Table Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                    break;

                case TSubmitChangesResult.scrInfoNeeded:

                    MessageBox.Show(Catalog.GetString(
                        "The 'UsedBy' part of the data could not be saved! Insufficient information was provided when making changes to the table."),
                    Catalog.GetString("Submit Changes to Table Error"),
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
            if ((cmbDetailDataType.SelectedIndex < 0) || (cmbDetailDataType.SelectedIndex > 7))
            {
                return;
            }

            // Start by hiding everything
            txtDetailCharLength.Visible = false;
            txtDetailNumDecimalPlaces.Visible = false;
            pnlCurrencyCode.Visible = false;
            pnlCategoryCode.Visible = false;

            // Show the relevant panel or text box and modify the label text
            switch (cmbDetailDataType.SelectedIndex)
            {
                case 0:                 // Text
                    txtDetailCharLength.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Maximum length") + ":";
                    break;

                case 1:                 // Numeric
                    txtDetailNumDecimalPlaces.Visible = true;
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
                    pnlCategoryCode.Visible = true;
                    lblDataSubType.Text = Catalog.GetString("Option list name") + ":";
                    break;
            }
        }
    }
}