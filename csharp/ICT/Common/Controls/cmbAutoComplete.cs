//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using Ict.Common;
using Accessibility;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Ict.Common.Controls
{
    /// <summary>
    /// delegate for when a new item is added
    /// </summary>
    public delegate void TAcceptNewEntryEventHandler(System.Object Sender, TAcceptNewEntryEventArgs e);

    /// <summary>
    /// event arguments for when a new item is added
    /// </summary>
    public class TAcceptNewEntryEventArgs : System.ComponentModel.CancelEventArgs
    {
        /// <summary>
        /// the new item
        /// </summary>
        public String ItemString;
    }

    /// <summary>
    /// The cmbAutoComplete ComboBox behaves just like the default ComboBox from .Net.
    /// It does only have two additional Properties, namely 'AcceptNewValues' and
    /// 'CaseSensitiveSearch'. If the 'AcceptNewValues' property is set to 'true',
    /// the user may add new items to the list of items of this ComboBox, otherwise
    /// he cannot add a new item. The 'CaseSensitiveSearch' property allows to switch
    /// between a case sensitive search and a non case sensitive search in the items
    /// of the Combobox. However the ComboBox searches its internal data for the text
    /// being entered and returns the first occurance of that text. The searching is
    /// done while typing, saving the end user some time.
    /// </summary>
    public class TCmbAutoComplete : System.Windows.Forms.ComboBox
    {
        private const String StrValueMember = "#VALUE#";
        private const String StrDisplayMember = "#DISPLAY#";

        private bool UPressedKey;
        private String UInitialString;
        private bool FAcceptNewValues;
        private bool FCaseSensitiveSearch;
        private bool FSuppressSelectionColor;
        private String FColumnsToSearchDesignTime;

        /// <summary>
        /// which columns to search
        /// </summary>
        protected StringCollection FColumnsToSearch = null;

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
        /// </summary>
        public bool AcceptNewValues
        {
            get
            {
                return this.FAcceptNewValues;
            }

            set
            {
                this.FAcceptNewValues = value;
            }
        }

        /// <summary>
        /// This property determines which columns are searched, when the user enters
        /// text into the combobox.
        ///
        /// </summary>
        public String ColumnsToSearch
        {
            get
            {
                return this.FColumnsToSearchDesignTime;
            }

            set
            {
                if (value == null)
                {
                    value = "";
                }

                // If the value does not contain the standard columns add them
                if (value.IndexOf(StrValueMember) < 0)
                {
                    value = value + ", " + StrValueMember;
                }

                if (value.IndexOf(StrDisplayMember) < 0)
                {
                    value = value + ", " + StrDisplayMember;
                }

                value = value.ToUpper();

                // Get rid of a leading comma
                if (value.IndexOf(',') == 0)
                {
                    value = value.Substring(1);
                }

                value = value.Trim();
                this.FColumnsToSearchDesignTime = value;

                // Set the Collection as well
                this.FColumnsToSearch = BuildColumnStringCollection(this.FColumnsToSearchDesignTime);
            }
        }

        /// <summary>
        /// case sensitive search
        /// </summary>
        public bool CaseSensitiveSearch
        {
            get
            {
                return this.FCaseSensitiveSearch;
            }

            set
            {
                this.FCaseSensitiveSearch = value;
            }
        }

        /// <summary>
        /// data source
        /// </summary>
        public new object DataSource
        {
            get
            {
                return base.DataSource;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                if (value is DataView)
                {
                    base.DataSource = value;
                }
                else if (!DesignMode)
                {
                    throw new Exception("Datasource cannot be assigned with this datatype");
                }

                // problem to set it here, because the datasource is still being updated, and the indexchanged triggers give trouble
                this.SelectedIndex = -1;
                this.Text = string.Empty;
            }
        }

        /// <summary>
        /// This property manages the colour of the selection. If set to TRUE the
        /// selected item of the ComboBox is not coloured in selection mode colours.
        ///
        /// </summary>
        public bool SuppressSelectionColor
        {
            get
            {
                return this.FSuppressSelectionColor;
            }

            set
            {
                this.FSuppressSelectionColor = value;
            }
        }

        /// <summary>
        /// This property manages the new entry event
        /// </summary>
        public event TAcceptNewEntryEventHandler AcceptNewEntries;


        #region Hide Some Unhelpful Parent Properties

        /// <summary>
        /// required to be overwritten from Parent
        /// </summary>
        [BrowsableAttribute(false)]
        public new System.Windows.Forms.AutoCompleteSource AutoCompleteSource {
            get
            {
                return AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// required to be overwritten from Parent
        /// </summary>
        [BrowsableAttribute(false)]
        public new System.Windows.Forms.AutoCompleteMode AutoCompleteMode {
            get
            {
                return AutoCompleteMode.None;
            }
        }

        /// <summary>
        /// required to be overwritten from Parent
        /// </summary>
        [BrowsableAttribute(false)]
        public new AutoCompleteStringCollection AutoCompleteCustomSource {
            get
            {
                return new AutoCompleteStringCollection();
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public TCmbAutoComplete() : base()
        {
            this.UPressedKey = false;
            ProhibitChangeToDataSource();
            this.FSuppressSelectionColor = true;
        }

        /// <summary>
        /// overload
        /// </summary>
        public void SetDataSourceStringList(string ACSVList)
        {
            SetDataSourceStringList(StringHelper.StrSplit(ACSVList, ","));
        }

        /// <summary>
        /// This procedure is an alternative to set_datasource.
        /// This is helpful if there is only one column, and the values are all strings.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetDataSourceStringList(StringCollection AList)
        {
            this.BeginUpdate();
            this.DataSource = null;
            this.SelectedIndex = -1;
            this.Text = string.Empty;

            foreach (String s in AList)
            {
                this.Items.Add(s);
            }

            this.EndUpdate();
        }

        /// <summary>
        /// This procedure helps with modifying the datasource.
        /// This is helpful if there is only one column, and the values are all strings.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void AddStringItem(String AItem)
        {
            string currentText = this.Text;

            this.BeginUpdate();
            this.Items.Add(AItem);
            this.EndUpdate();
            this.Text = currentText;
        }

        /// <summary>
        /// This function builds a string collection out of a comma seperated list given
        /// in the format of a string. The occurance of each column can only be one.
        /// Therefore Duplicates are being removed.
        /// </summary>
        private StringCollection BuildColumnStringCollection(String AString)
        {
            StringCollection ReturnValue = new StringCollection();
            StringCollection mRawStringCollection = StringHelper.StrSplit(AString, ",");

            foreach (String mString in mRawStringCollection)
            {
                string stringToAdd = mString;

                // Replace the placeholders #VALUE# and #DISPLAY# with the real stuff.
                if (stringToAdd == StrValueMember)
                {
                    stringToAdd = this.ValueMember;
                }

                if (stringToAdd == StrDisplayMember)
                {
                    stringToAdd = this.DisplayMember;
                }

                if (!ReturnValue.Contains(stringToAdd) && (stringToAdd.Length > 0))
                {
                    ReturnValue.Add(stringToAdd);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure helps with modifying the datasource.
        /// This is helpful if there is only one column, and the values are all strings.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RemoveStringItem(String AItem)
        {
            this.Items.Remove(AItem);
        }

        /// <summary>
        /// </summary>
        /// <returns>the number of the given column
        /// </returns>
        private System.Int32 GetColumnNr(String AColumnName)
        {
            if (this.DataSource == null)
            {
                return -1;
            }

            return ((DataView)(this.DataSource)).Table.Columns.IndexOf(AColumnName);
        }

        /// <summary>
        /// This function returns the currently selected row
        /// </summary>
        /// <returns>the currently selected row
        /// </returns>
        protected System.Data.DataRowView GetSelectedRowView()
        {
            if (SelectedIndex == -1)
            {
                return null;
            }
            else
            {
                return (System.Data.DataRowView)(this.Items[SelectedIndex]);
            }
        }

        #region User Control events

        /// <summary>
        /// This procedure is called when a new item should be put in the item list.
        /// </summary>
        /// <param name="Args">TAcceptNewEntryEventArgs.
        /// </param>
        /// <returns>void</returns>
        private void OnAcceptNewEntryEvent(TAcceptNewEntryEventArgs Args)
        {
            System.Int32 mNumDataSourceCols = this.GetNumberOfDataSourceCols();

            if ((mNumDataSourceCols != 1) || (Args.Cancel == true))
            {
                MessageBox.Show("Item could not be added to the items collection!", "Confirm adding this item!");
                return;
            }

            this.AddItemToDataSource(Args.ItemString);
            this.SelectedIndex = this.FindStringExact(Args.ItemString);
            this.Text = Args.ItemString;
        }

        /// <summary>
        /// This procedure is called when the value of the DataSource property is
        /// changed on ListControl.
        /// </summary>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        protected override void OnDataSourceChanged(System.EventArgs e)
        {
            if ((DisplayMember == null) || (DisplayMember.Length == 0))
            {
                throw new Exception(
                    "cmbAutoComplete: need to first initialise the DisplayMember and the ValueMember before assigning the DataSource!");
            }

            // If the FColumnsToString collection has not yet initialized it must be done now.
            if ((this.FColumnsToSearch == null) || (this.FColumnsToSearch.Count < 1))
            {
                this.ColumnsToSearch = FColumnsToSearchDesignTime;
            }

            base.OnDataSourceChanged(e);
        }

        /// <summary>
        /// This procedure is called when this control becomes the active control of the form.
        /// </summary>
        /// <param name="e">System.EventArgs.
        /// </param>
        /// <returns>void</returns>
        protected override void OnEnter(System.EventArgs e)
        {
            base.OnEnter(e);
            this.UInitialString = this.Text;
        }

        /// <summary>
        /// This procedure is called when a key is down. This event is the first
        /// event to be called in the key events.
        /// </summary>
        /// <param name="e">The Key Event Arguments.
        /// </param>
        /// <returns>void</returns>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            // If the cmbAutoComplete style is of DropDownList does bit allow editing
            if ((this.DropDownStyle == ComboBoxStyle.DropDownList) && (e.KeyCode == Keys.Delete))
            {
                if (this.Text != this.SelectedText)
                {
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// This procedure is called when a key is pressed. This event is the second
        /// event to be called in the key events.
        /// </summary>
        /// <param name="e">The Key Press Event Arguments</param>
        /// <returns>void</returns>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            String TypedText;

            System.Int32 FoundIndex;
            String CurrentText;

            if (this.DropDownStyle == ComboBoxStyle.DropDown)
            {
                this.UPressedKey = true;
            }
            else
            {
                if ((e.KeyChar) == (char)(8))
                {
                    if (this.SelectedText == this.Text)
                    {
                        this.UPressedKey = true;
                        base.OnKeyPress(e);
                        return;
                    }
                }

                if (this.SelectionLength > 0)
                {
                    Int32 Start = this.SelectionStart;
                    Int32 SelLength = this.SelectionLength;

                    // This is equivalent to this.Text, but sometimes
                    // using this.Text doesn't work
                    CurrentText = this.AccessibilityObject.Value;
                    CurrentText = CurrentText.Remove(Start, SelLength);
                    CurrentText = CurrentText.Insert(Start, new String(e.KeyChar, 1));
                    TypedText = CurrentText;
                }
                else
                {
                    Int32 Start = this.SelectionStart;
                    TypedText = this.Text.Insert(Start, new String(e.KeyChar, 1));
                }

                FoundIndex = this.FindStringSortedByLength(TypedText);

                if (FoundIndex >= 0)
                {
                    this.UPressedKey = true;
                }
                else
                {
                    e.Handled = true;
                }
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// This procedure is called when a key goes up again. This event is the last
        /// event to be called in the key events.
        /// </summary>
        /// <param name="e">The Key Event Arguments.</param>
        /// <returns>void</returns>
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (this.UPressedKey == true)
            {
                // Ignoring Control Characters
                switch (e.KeyCode)
                {
                    case Keys.Back:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Delete:
                    case Keys.Down:
                    case Keys.End:
                    case Keys.Home:
                        return;

                        //break;
                }

                // Get the text in the text box
                string TypedText = this.Text;

                // If the text is found in the Items list then Autocomplete
                System.Int32 FoundIndex = this.FindStringSortedByLength(TypedText);

                if (FoundIndex >= 0)
                {
                    // Get item from the list (Return type depends if Datasource ws bound or list created
                    object FoundItem = this.Items[FoundIndex];

                    // Get the text of the item
                    string FoundText = this.GetItemText(FoundItem);

                    // Append then found text to the typed text to preserve case
                    // AppendText := FoundText.Substring(TypedText.Length);
                    // this.Text := TypedText + AppendText;
                    this.Text = FoundText;

                    // Select Appended Text
                    this.SelectionStart = TypedText.Length;

                    // this.SelectionLength := AppendText.Length;
                    this.SelectionLength = FoundText.Length;

                    // Select exact item
                    if (e.KeyCode == Keys.Enter)
                    {
                        FoundIndex = this.FindStringSortedByLength(this.Text);
                        this.SelectedIndex = FoundIndex;
                        SendKeys.Send("{TAB}");
                        e.Handled = true;
                    }
                }
            }
            else
            {
                if (this.Text == "")
                {
                    this.SelectedItem = DBNull.Value;
                    this.SelectedValue = DBNull.Value;
                }
            }

            this.UPressedKey = false;
            base.OnKeyUp(e);
        }

        /// <summary>
        /// ask the user if he wants to add this new item to the combobox
        /// </summary>
        /// <param name="mArgs"></param>
        protected void AskUserAcceptNewEntries(TAcceptNewEntryEventArgs mArgs)
        {
            if (mArgs.Cancel == true)
            {
                return;
            }

            string mMessage = "Do you want to add this item >" + mArgs.ItemString + "< to the list of this combobox?";
            string mCaption = "Confirm adding this item!";
            System.Windows.Forms.MessageBoxButtons mButtons = System.Windows.Forms.MessageBoxButtons.OKCancel;
            System.Windows.Forms.MessageBoxIcon mIcon = System.Windows.Forms.MessageBoxIcon.Question;
            System.Windows.Forms.DialogResult mDialogResult = MessageBox.Show(mMessage, mCaption, mButtons, mIcon);

            if (mDialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                mArgs.Cancel = true;
            }
        }

        /// <summary>
        /// This procedure is called when this control looses its active status and is
        /// no longer the active control of a form.
        /// </summary>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        protected override void OnLeave(System.EventArgs e)
        {
            base.OnLeave(e);

            if (DesignMode)
            {
                return;
            }

            String mItemString = this.Text;
            TAcceptNewEntryEventArgs mArgs = new TAcceptNewEntryEventArgs();
            mArgs.ItemString = mItemString;

            // Try to find the text entered in the set of items contained in cmbAutoComplete
            System.Int32 mFoundIndex = this.FindStringExact(mItemString);

            if (mFoundIndex >= 0)
            {
                // Text found and identified.
                // TLogging.Log('Text found and identified. mFoundIndex: ' + mFoundIndex.ToString);
                this.SelectedIndex = mFoundIndex;

                if (AcceptNewEntries != null)
                {
                    // we might want to move the value upwards in the history
                    AcceptNewEntries(this, mArgs);
                }
            }
            else
            {
                // Text could not be found.
                if (this.AcceptNewValues == true)
                {
                    // User may enter new values.
                    if (AcceptNewEntries != null)
                    {
                        // User wants to do something before the new entry is added
                        AcceptNewEntries(this, mArgs);

                        // could make a call to AskUserAcceptNewEntries instead?

                        if (mArgs.Cancel == true)
                        {
                            // User wants to cancel the adding of a new entry => Original value prevails
                            RestoreOriginalItem();
                        }
                        else
                        {
                            // Add the new item to the combobox collection
                            OnAcceptNewEntryEvent(mArgs);
                        }
                    }
                    else
                    {
                        OnAcceptNewEntryEvent(mArgs);
                    }
                }
                else
                {
                    if ((this.SelectedItem != null) && this.SelectedItem.Equals(System.DBNull.Value))
                    {
                        this.Text = System.DBNull.Value.ToString();
                        this.UInitialString = System.DBNull.Value.ToString();
                    }
                    else
                    {
                        RestoreOriginalItem();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// This procedure adds an item to the DataSource if the DataSource consists
        /// out of a DataTable with only one column. Otherwise it does nothing.
        /// </summary>
        /// <param name="ItemString">The string of the item to add.
        /// </param>
        /// <returns>void</returns>
        private void AddItemToDataSource(String ItemString)
        {
            if (this.DataSource == null)
            {
                return;
            }

            DataTable mDataTable = ((DataView) this.DataSource).Table;

            if (mDataTable.Columns.Count > 1)
            {
                return;
            }

            DataColumn mDataColumn = mDataTable.Columns[0];
            string mColName = mDataColumn.ColumnName;
            DataRow mNewRow = mDataTable.NewRow();
            mNewRow[0] = ItemString;
            mDataTable.Rows.InsertAt(mNewRow, 0);
            this.BeginUpdate();
            this.DisplayMember = mColName;
            this.ValueMember = mColName;
            this.DataSource = mDataTable.DefaultView;
            this.EndUpdate();
        }

        /// <summary>
        /// This function returns the index of the combobox items with the following
        /// characteristics:
        /// - item is exactly the searched String and in the given column (value column)
        /// </summary>
        /// <param name="SearchString">The string which is search for in the ComboBox</param>
        /// <param name="ColumnIndex">The index of table Column where to search for the String.</param>
        /// <returns>The index of the item if found or -1 if nothing is found.
        /// </returns>
        public int FindExactString(string SearchString, int ColumnIndex)
        {
            foreach (object ComboboxItem in this.Items)
            {
                System.Data.DataRowView TmpRowView = (System.Data.DataRowView)ComboboxItem;
                Object Item = TmpRowView[ColumnIndex];
                String ItemString;

                if (Item != null)
                {
                    ItemString = Item.ToString();

                    if (SearchString.Equals(ItemString))
                    {
                        return this.Items.IndexOf(ComboboxItem);
                    }
                }
            }

            //not found
            return -1;
        }

        /// <summary>
        /// This function returns the index of the combobox items with the following
        /// characteristics:
        /// - item starts with the specified string
        /// - if there are more items which fulfill this criterion the index of the
        /// shortest item is returned
        /// </summary>
        /// <param name="SearchString">The string which is search for in the ComboBox</param>
        /// <param name="StartIndex">The index where the comparison should start.</param>
        /// <returns>The index of the item if found or -1 if nothing is found.
        /// </returns>
        public int FindStringSortedByLength(string SearchString, int StartIndex)
        {
            if (DataSource == null)
            {
                // TODO: proper implementation of FindStringSortedByLength for simple string lists
                return FindString(SearchString);
            }

            // Check whether the DataSource is DataView
            if (!(this.DataSource is DataView))
            {
                return -1;
            }

            // Create the IndexTable where the results are stored. The table consists out
            // of a column that holds the index and a column which holds the length of the
            // found string.
            DataTable IndexTable = new System.Data.DataTable("IndexTable");
            DataColumn IndexCol = new System.Data.DataColumn("Index", typeof(System.Int32));
            IndexTable.Columns.Add(IndexCol);
            IndexCol = new System.Data.DataColumn("Column", typeof(System.Int32));
            IndexTable.Columns.Add(IndexCol);
            IndexCol = new System.Data.DataColumn("Length", typeof(System.Int32));
            IndexTable.Columns.Add(IndexCol);
            IndexCol = new System.Data.DataColumn("Content", typeof(String));
            IndexTable.Columns.Add(IndexCol);
            IndexCol = new System.Data.DataColumn("ColumnName", typeof(String));
            IndexTable.Columns.Add(IndexCol);
            IndexTable.PrimaryKey = new DataColumn[] {
                IndexTable.Columns["Index"],
                IndexTable.Columns["Column"],
                IndexTable.Columns["Length"],
                IndexTable.Columns["Content"],
                IndexTable.Columns["ColumnName"]
            };

            // Iterate through the items of the combobox.
            foreach (object ComboboxItem in this.Items)
            {
                DataRowView TmpRowView = (System.Data.DataRowView)ComboboxItem;

                // Iterate through the columns to search in the datasource
                foreach (String ColumnName in this.FColumnsToSearch)
                {
                    Int32 ColumnIndex = this.GetColumnNr(ColumnName);
                    Type ItemType = TmpRowView[ColumnIndex].GetType();

                    // todo: what about ledger numbers (Int32)?
                    if (ItemType == typeof(String))
                    {
                        string ItemString = TmpRowView[ColumnIndex].ToString();
                        ItemString = ItemString.ToUpper();
                        SearchString = SearchString.ToUpper();

                        if (ItemString.StartsWith(SearchString))
                        {
                            // Store the result.
                            Int32 ItemIndex = this.Items.IndexOf(ComboboxItem);
                            Int32 ItemLength = ItemString.Length;
                            DataRow IndexRow = IndexTable.NewRow();
                            IndexRow["Index"] = (System.Object)ItemIndex;
                            IndexRow["Column"] = (System.Object)ColumnIndex;
                            IndexRow["Length"] = (System.Object)ItemLength;
                            IndexRow["Content"] = (System.Object)ItemString;
                            IndexRow["ColumnName"] = (System.Object)ColumnName;

                            // Insert the found row in the search table only at the first appearance.
                            if (!(IndexTable.Rows.Contains(IndexRow.ItemArray)))
                            {
                                IndexTable.Rows.InsertAt(IndexRow, IndexTable.Rows.Count);
                            }
                        }
                    }
                }
            }

            // Search for the shortest string in the IndexTable with the smallest indices.
            DataView IndexView = new DataView(IndexTable);

            // IndexView.Sort := 'Column ASC';
            IndexView.Sort = "Content ASC, ColumnName ASC";

            if (IndexTable.Rows.Count > 0)
            {
                return System.Convert.ToInt32(IndexView[0]["Index"]);
            }

            return -1;
        }

        /// <summary>
        /// This function returns the index of the combobox items with the following
        /// characteristics:
        /// - item starts with the specified string
        /// - if there are more items which fulfill this criterion the index of the
        /// shortest item is returned
        /// </summary>
        /// <param name="SearchString">The string which is search for in the ComboBox</param>
        /// <returns>The index of the item if found or -1 if nothing is found.
        /// </returns>
        public int FindStringSortedByLength(string SearchString)
        {
            return FindStringSortedByLength(SearchString, 0);
        }

        /// <summary>
        /// This function searches the ObjectCollection of a given ComboBox for a
        /// string specified by SearchString.
        /// </summary>
        /// <param name="SearchString">The string which is searched for.</param>
        /// <returns>The index of the item if found or -1 if nothing is found.</returns>
        public int FindStringInComboBox(string SearchString)
        {
            // try to find the best match, if there is no exact match
            int BestMatch = -1;

            SearchString = SearchString.ToUpper();

            if (DataSource == null)
            {
                // use the normal Items list with string object
                foreach (string value in Items)
                {
                    if (value.ToUpper() == SearchString)
                    {
                        return Items.IndexOf(value);
                    }
                }

                return -1;
            }

            foreach (object Item in Items)
            {
                System.Data.DataRowView TmpRowView = (System.Data.DataRowView)Item;

                if ((TmpRowView == null) || (TmpRowView.DataView == null) || (TmpRowView.DataView.Table == null))
                {
                    TLogging.Log("FindStringInComboBox: problem with DataView.Table (" + this.Name + ')');
                    return -1;
                }

                for (System.Int32 ColumnIndex = 0; ColumnIndex <= TmpRowView.DataView.Table.Columns.Count - 1; ColumnIndex += 1)
                {
                    if (this.FColumnsToSearch.Contains(TmpRowView.DataView.Table.Columns[ColumnIndex].ColumnName)
                        && (TmpRowView[ColumnIndex].GetType() == typeof(String)))
                    {
                        string ItemString = TmpRowView[ColumnIndex].ToString().ToUpper();

                        if (ItemString.IndexOf(SearchString) >= 0)
                        {
                            if (ItemString.Length == SearchString.Length)
                            {
                                return Items.IndexOf(Item);
                            }
                            else
                            {
                                // deactivated BestMatch for the moment
                                BestMatch = -1;
                            }
                        }
                    }
                }
            }

            return BestMatch;
        }

        /// <summary>
        /// This function searches the ObjectCollection of a given ComboBox for a
        /// string specified by SearchInt32.
        /// </summary>
        /// <param name="SearchInt32">The Int32 which is searched for.</param>
        /// <returns>The index of the item if found or -1 if nothing is found.
        /// </returns>
        public int FindInt32InComboBox(int SearchInt32)
        {
            System.Int32 ItemInt32;
            System.Int32 ColumnIndex;
            System.Int32 ColumnNum;
            System.Data.DataRowView TmpRowView;
            int ReturnValue = -1;

            if (DataSource == null)
            {
                // use the normal Items list with integer object
                foreach (int value in Items)
                {
                    if (value == SearchInt32)
                    {
                        return Items.IndexOf(value);
                    }
                }

                return -1;
            }
            
            foreach (object Item in Items)
            {
                TmpRowView = (System.Data.DataRowView)Item;
                ColumnNum = (TmpRowView.DataView.Table.Columns.Count) - 1;

                if (ColumnNum > 0)
                {
                    for (ColumnIndex = 0; ColumnIndex <= (ColumnNum); ColumnIndex += 1)
                    {
                        if (TmpRowView[ColumnIndex].GetType() == typeof(System.Int32))
                        {
                            ItemInt32 = System.Convert.ToInt32(TmpRowView[ColumnIndex]);

                            if (ItemInt32 == SearchInt32)
                            {
                                ReturnValue = Items.IndexOf(Item);

                                // Messagebox.Show('Result: ' + Result.ToString);
                                break;
                            }
                        }
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function searches the ObjectCollection of a given ComboBox for a
        /// string specified by SearchInt64.
        /// </summary>
        /// <param name="SearchInt64">The Int64 which is searched for.</param>
        /// <returns>The index of the item if found or -1 if nothing is found.
        /// </returns>
        public int FindInt64InComboBox(Int64 SearchInt64)
        {
            if (DataSource == null)
            {
                // use the normal Items list with integer object
                foreach (Int64 value in Items)
                {
                    if (value == SearchInt64)
                    {
                        return Items.IndexOf(value);
                    }
                }

                return -1;
            }

            foreach (object Item in Items)
            {
                System.Data.DataRowView TmpRowView = (System.Data.DataRowView)Item;
                System.Int32 ColumnNum = (TmpRowView.DataView.Table.Columns.Count) - 1;

                if (ColumnNum > 0)
                {
                    for (System.Int32 ColumnIndex = 0; ColumnIndex <= (ColumnNum); ColumnIndex += 1)
                    {
                        if (TmpRowView[ColumnIndex].GetType() == typeof(System.Int64))
                        {
                            Int64 ItemInt64 = System.Convert.ToInt64(TmpRowView[ColumnIndex]);

                            if (ItemInt64 == SearchInt64)
                            {
                                return Items.IndexOf(Item);
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// This function gets the number of columns of the current DataSource.
        /// </summary>
        /// <returns>The number of columns of the current DataSource
        /// </returns>
        private int GetNumberOfDataSourceCols()
        {
            if (this.DataSource == null)
            {
                // for simple string list
                return 1;
            }

            return ((DataView)DataSource).Table.Columns.Count;
        }

        /// <summary>
        /// This function returns the Int32 value of the selected item, first column
        /// </summary>
        /// <param name="ColumnNumber">The column number of the data source; if -1, then the value column is used</param>
        /// <returns>-1 if nothing is selected
        /// </returns>
        public Int32 GetSelectedInt32(int ColumnNumber)
        {
            if (DataSource == null)
            {
                if ((Items.Count > 0) && (this.SelectedIndex != -1))
                {
                    // use the normal Items values, not the datasource etc
                    return Convert.ToInt32(Items[this.SelectedIndex]);
                }
            }

            if (ColumnNumber == -1)
            {
                ColumnNumber = GetColumnNr(ValueMember);
            }

            if (ColumnNumber == -1)
            {
                // combobox has not been initialised properly
                return -1;
            }

            if ((this.SelectedItem != null) && (this.SelectedItem != System.DBNull.Value))
            {
                DataRowView rowView = this.GetSelectedRowView();

                if (rowView != null)
                {
                    return System.Convert.ToInt32(rowView[ColumnNumber]);
                }
            }

            return -1;
        }

        /// <summary>
        /// get the int value of the default column
        /// </summary>
        /// <returns>selected int value</returns>
        public Int32 GetSelectedInt32()
        {
            return GetSelectedInt32(-1);
        }

        /// <summary>
        /// This function returns the Int64 value of the selected item, first column
        /// </summary>
        /// <param name="ColumnNumber">The column number of the data source; if -1, then the value column is used</param>
        /// <returns>-1 if nothing is selected
        /// </returns>
        public Int64 GetSelectedInt64(int ColumnNumber)
        {
            if (DataSource == null)
            {
                if ((Items.Count > 0) && (this.SelectedIndex != -1))
                {
                    // use the normal Items values, not the datasource etc
                    return Convert.ToInt64(Items[this.SelectedIndex]);
                }
            }
            
            if (ColumnNumber == -1)
            {
                ColumnNumber = GetColumnNr(ValueMember);
            }

            if ((this.SelectedItem != null) && (this.SelectedItem != System.DBNull.Value))
            {
                DataRowView rowView = this.GetSelectedRowView();

                if (rowView != null)
                {
                    return System.Convert.ToInt64(rowView[ColumnNumber]);
                }
            }

            return -1;
        }

        /// <summary>
        /// get the int value of the default column
        /// </summary>
        /// <returns>selected int value</returns>
        public Int64 GetSelectedInt64()
        {
            return GetSelectedInt64(-1);
        }

        /// <summary>
        /// This function returns the string value of the selected item, first column
        /// </summary>
        /// <param name="ColumnNumber">The column number of the data source; if -1, then the value column is used</param>
        /// <returns>empty string if nothing is selected
        /// </returns>
        public string GetSelectedString(int ColumnNumber)
        {
            string ReturnValue = "";

            if (DataSource == null)
            {
                if ((Items.Count > 0) && (this.SelectedIndex != -1))
                {
                    // use the normal Items values, not the datasource etc
                    return Convert.ToString(Items[this.SelectedIndex]);
                }
            }

            if (ColumnNumber == -1)
            {
                ColumnNumber = GetColumnNr(ValueMember);
            }

            if ((this.SelectedItem != null) && (SelectedIndex != -1) && (this.SelectedItem != System.DBNull.Value))
            {
                if (this.Items[SelectedIndex] is System.Data.DataRowView)
                {
                    ReturnValue = ((System.Data.DataRowView) this.Items[SelectedIndex])[ColumnNumber].ToString();
                }
                else if (this.Items[SelectedIndex] is System.String)
                {
                    ReturnValue = (string)this.Items[SelectedIndex];
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// get selected value as a string, from the default column
        /// </summary>
        /// <returns>get selected string value</returns>
        public string GetSelectedString()
        {
            return GetSelectedString(-1);
        }

        /// <summary>
        /// get the display string for the selected value
        /// </summary>
        public string GetSelectedDisplayString()
        {
            return GetSelectedString(GetColumnNr(DisplayMember));
        }

        /// <summary>
        /// the column that is used for the description in the label
        /// </summary>
        public string DescriptionMember
        {
            get;
            set;
        }

        /// <summary>
        /// get the description string for the selected value (for the label beside the combobox)
        /// </summary>
        public string GetSelectedDescription()
        {
            return GetSelectedString(GetColumnNr(DescriptionMember));
        }

        /// <summary>
        /// This function selects an item with the given Int32 value.
        /// Select alternative index if the int value is not existing
        /// </summary>
        /// <param name="ANr">Int value to search for</param>
        /// <param name="AAlternativeIndex">if the ANr cannot be found in the list, select the item with this index; it is by default -1</param>
        /// <returns>false if the int value is not existing
        /// </returns>
        public bool SetSelectedInt32(System.Int32 ANr, System.Int32 AAlternativeIndex)
        {
            bool ReturnValue;

            SelectedIndex = FindInt32InComboBox(ANr);
            ReturnValue = true;

            if ((SelectedIndex == -1) && (Items.Count > 0))
            {
                SelectedIndex = AAlternativeIndex;
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function selects an item with the given Int32 value.
        /// Select first element if the int value is not existing
        /// </summary>
        /// <param name="ANr">Int value to search for</param>
        /// <returns>false if the int value is not existing
        /// </returns>
        public bool SetSelectedInt32(System.Int32 ANr)
        {
            return SetSelectedInt32(ANr, -1);
        }

        /// <summary>
        /// This function selects an item with the given Int64 value.
        /// Select alternative index if the int value is not existing
        /// </summary>
        /// <param name="ANr">Int value to search for</param>
        /// <param name="AAlternativeIndex">if the ANr cannot be found in the list, select the item with this index; it is by default -1</param>
        /// <returns>false if the int value is not existing
        /// </returns>
        public bool SetSelectedInt64(System.Int64 ANr, System.Int32 AAlternativeIndex)
        {
            bool ReturnValue;

            SelectedIndex = FindInt64InComboBox(ANr);
            ReturnValue = true;

            if ((SelectedIndex == -1) && (Items.Count > 0))
            {
                SelectedIndex = AAlternativeIndex;
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function selects an item with the given Int64 value.
        /// Select first element if the int value is not existing
        /// </summary>
        /// <param name="ANr">Int value to search for</param>
        /// <returns>false if the int value is not existing
        /// </returns>
        public bool SetSelectedInt64(System.Int64 ANr)
        {
            return SetSelectedInt64(ANr, -1);
        }

        /// <summary>
        /// This function selects an item with the given string value.
        /// Select first element if the string value is not existing
        /// </summary>
        /// <returns>false if the string value is not existing
        /// </returns>
        public bool SetSelectedString(String AStr)
        {
            return SetSelectedString(AStr, 0);
        }

        /// <summary>
        /// This function selects an item with the given string value.
        /// Set index to ADefaultIndex if the string value is not existing
        /// </summary>
        /// <returns>false if the string value is not existing
        /// </returns>
        public bool SetSelectedString(String AStr, Int32 ADefaultIndex)
        {
            Int32 PreviousSelectedIndex = SelectedIndex;

            // FindStringExact looks for the displayed value only
            Int32 NewSelectedIndex = this.FindStringExact(AStr);

            if ((NewSelectedIndex == -1) && (Items.Count > 0))
            {
                // now search the value members as well
                NewSelectedIndex = FindStringInComboBox(AStr);
            }

            if ((NewSelectedIndex == -1) && (Items.Count > 0))
            {
                if ((PreviousSelectedIndex != -1) && (AStr.Length > 0))
                {
                    SelectedIndex = PreviousSelectedIndex;
                    return false;
                }
                else
                {
                    // The following statement has to be called twice. For whatever reason if ADefaultIndex
                    // is -1 then the first statement sets the SelectedIndex value to 0 and only the second
                    // statement sets it to -1. Not sure why that is the case but just for your information.
                    // THEREFORE: DON'T REMOVE SECOND STATEMENT FOR NOW!
                    SelectedIndex = ADefaultIndex;
                    SelectedIndex = ADefaultIndex;
                    return true;
                }
            }

            SelectedIndex = NewSelectedIndex;

            return true;
        }

        /// <summary>
        /// This procedure sets the selection colouring length to the desired value.
        /// If the property SuppressSelectionColor is TRUE then the selection length
        /// is set to 0. If the property SuppressSelectionColor is FALSE then the
        /// selection length is set to the length of the current string in its textbox.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetSelectionColorLength()
        {
            if (this.FSuppressSelectionColor == false)
            {
                this.SelectionLength = 100;
            }
            else
            {
                this.SelectionLength = 0;
            }
        }

        /// <summary>
        /// This procedure should prevent databinding from manipulating the original
        /// source.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void ProhibitChangeToDataSource()
        {
            if ((DesignMode == false) && (this.DataSource != null))
            {
                ((DataView) this.DataSource).AllowDelete = false;
                ((DataView) this.DataSource).AllowEdit = false;
                ((DataView) this.DataSource).AllowNew = false;
            }
        }

        /// <summary>
        /// This procedure restores the original value of the combobox if someone
        /// typed in something that could not be found in the items collection of the
        /// combobox.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RestoreOriginalItem()
        {
            string mTestString = this.UInitialString;

            // if mTestString is NULL, set to '' (empty string)
            // otherwise trim would yield an exception.
            if (mTestString == null)
            {
                mTestString = string.Empty;
            }

            mTestString = mTestString.Trim();

            if (mTestString == string.Empty)
            {
                // It is only an empty string => ComboBox is not set to a certain item
                this.Text = string.Empty;
                this.SelectedIndex = -1;
            }
            else
            {
                // It is a valid string => The string must be in the items collection
                this.SelectedIndex = this.FindStringExact(this.UInitialString);
                this.Text = this.SelectedIndex == -1 ? string.Empty : this.UInitialString;
            }
        }
    }
}