//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
//
// Copyright 2004-2011 by OM International
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
    /// delegate for when the data source changes
    /// </summary>
    public delegate void TDataSourceChangingEventHandler(System.Object Sender, System.EventArgs e);

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
    /// event arguments for the data source changing event
    /// </summary>
    public class TDataSourceChangingEventArgs : System.EventArgs
    {
        /// <summary>
        /// the old data source
        /// </summary>
        public object OldDataSource;

        /// <summary>
        /// the new data source
        /// </summary>
        public object NewDataSource;
    }

    /// <summary>
    /// The cmbAutoComplete ComboBox behaves just like the default ComboBox from .Net.
    /// It does only have two additional Property, namely 'AcceptNewValues' and
    /// 'CaseSensitiveSearch'. If the 'AcceptNewValues' property is set to 'true',
    /// the user may add new items to the list of items of this ComboBox, otherwise
    /// he cannot add new item. The 'CaseSensitiveSearch' property allows to switch
    /// between a case sensitive search and a non case sensitive search in the items
    /// of the Combobox. However the ComboBox searches its internal data for the text
    /// being entered and returns the first occurance of that text. The searching is
    /// done while typing, saving the end user some time.
    /// </summary>
    public class TCmbAutoComplete : System.Windows.Forms.ComboBox
    {
        // const UNIT_SUPPORTED_DATA_TYPES = 'System.Data.DataView, System.Data.DataTable, System.Data.DataSet';
        /// <summary>
        /// which data types are supported at the moment
        /// </summary>
        private const String UNIT_SUPPORTED_DATA_TYPES = "System.Data.DataView";
        private const String DEFAULT_COMPLAIN =
            "The type used for the DataSource Property is not supported \r\nby TcmbAutoComplete. The following types are supported:\r\n" +
            UNIT_SUPPORTED_DATA_TYPES + "\r\nYour type: ";
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
        protected StringCollection FColumnsToSearch;

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
        /// This property determines which columns are searched, when the user enters
        /// text into the combobox.
        ///
        /// </summary>
        public System.Collections.Specialized.StringCollection ColumnsToSearchCollection
        {
            get
            {
                return this.FColumnsToSearch;
            }
        }

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
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
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
        /// </summary>
        public new object DataSource
        {
            get
            {
                return base.DataSource;
            }

            set
            {
                object OldDataSource = base.DataSource;
                TDataSourceChangingEventArgs Args = new TDataSourceChangingEventArgs();
                Args.OldDataSource = OldDataSource;
                Args.NewDataSource = value;

                if (!(DesignMode))
                {
                    OnDataSourceChanging(Args);
                }

                if (value == null)
                {
                    return;
                }

                if (HasDataSourceCorrectDataType(value) == true)
                {
                    base.DataSource = value;
                }
                else if (!DesignMode)
                {
                    throw new Exception("Datasource cannot be assigned with this datatype");
                }

                // problem to set it here, because the datasource is still being updated, and the indexchanged triggers give trouble
                // this.SelectedIndex := 1;
                // this.SelectedItem := nil;
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
        /// property for the current selection;
        /// SelectedItem is about the display member, SelectedValue reflects the value member, at the moment only Strings are supported
        /// </summary>
        public new System.Object SelectedItem
        {
            get
            {
                if (DesignMode)
                {
                    // messagebox.Show('I am in DesignMode!');
                    return base.SelectedItem;
                }

                if ((DataSource == null) && (Items.Count > 0) && (SelectedIndex > -1))
                {
                    // use the normal Items values, not the datasource etc
                    return Convert.ToString(Items[this.SelectedIndex]);
                }

                DataRowView mRowView = GetSelectedRowView();

                if (mRowView == null)
                {
                    return System.DBNull.Value;
                }
                else
                {
                    return mRowView[GetColumnNrOfValueMember()];
                }
            }

            set
            {
                if ((value == null) || (value.ToString() == ""))
                {
                    base.SelectedIndex = -1;
                }
                else
                {
                    Int32 m_SelectedIndex = this.FindStringSortedByLength(value.ToString(), GetColumnNrOfValueMember());
                    base.SelectedIndex = m_SelectedIndex;
                }
            }
        }
        /// <summary>
        /// This is yet another version of getting and setting the current selection:
        ///
        /// SelectedValueCell is for setting and getting exact the value column if datasource is used:
        /// we take the object from the table at the special marked value column and at the selected row
        /// </summary>
        public System.Object SelectedValueCell
        {
            get
            {
                if (DesignMode)
                {
                    return base.SelectedItem;
                }

                if (DataSource == null)
                {
                    if ((Items.Count > 0) && (SelectedIndex > -1))
                    {
                        // use the normal Items values, not the datasource etc
                        Object mySelectedItem = Items[this.SelectedIndex];
                        //TODO for composed values return the correct cell instead
                        return mySelectedItem;
                    }
                    else
                    {
                        return System.DBNull.Value;
                    }
                }
                else
                {
                    DataRowView mRowView = GetSelectedRowView();

                    if (mRowView == null)
                    {
                        return System.DBNull.Value;
                    }
                    else
                    {
                        return mRowView[GetColumnNrOfValueMember()];
                    }
                }
            }

            set
            {
                if ((value == null) || (value.ToString() == ""))
                {
                    base.SelectedIndex = -1;
                    base.ResetText();
                }
                else
                {
                    Int32 m_SelectedIndex = this.FindExactString(value.ToString(), GetColumnNrOfValueMember());
                    base.SelectedIndex = m_SelectedIndex;
                }
            }
        }
        /// <summary>
        /// property for the current selection;
        /// SelectedItem is about the display member, SelectedValue reflects the value member
        /// </summary>
        public new System.Object SelectedValue
        {
            get
            {
                return base.SelectedValue;
            }
            set
            {
                if (DesignMode)
                {
                    base.SelectedValue = value;
                }

                if (DataSource == null)
                {
                    SelectedItem = value;
                }
                else
                {
                    // TODO??? something special about DataSource situation?
                    base.SelectedItem = value;
                }
            }
        }

        /// <summary>
        /// This property manages the new entry event
        /// </summary>
        public event TAcceptNewEntryEventHandler AcceptNewEntries;

        /// <summary>
        /// This property manages the new entry event
        /// </summary>
        public event TDataSourceChangingEventHandler DataSourceChanging;

        #region "Hide Some Unhelpful Parent Properties"

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
        ///
        /// </summary>
        /// <returns>void</returns>
        private StringCollection BuildColumnStringCollection(String AString)
        {
            StringCollection ReturnValue;
            StringCollection mRawStringCollection;

            ReturnValue = new StringCollection();
            mRawStringCollection = StringHelper.StrSplit(AString, ",");

            foreach (String mString in mRawStringCollection)
            {
                if (!(ReturnValue.Contains(mString)))
                {
                    if (mString != "")
                    {
                        ReturnValue.Add(mString);
                    }
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

            if ((AColumnName != null) || (AColumnName == ""))
            {
                DataTable mDataTable = null;

                if (this.DataSource != null)
                {
                    CheckDataSourceType();

                    if (this.DataSource is System.Data.DataTable)
                    {
                        mDataTable = (System.Data.DataTable) this.DataSource;
                    }
                    else if (this.DataSource is System.Data.DataView)
                    {
                        mDataTable = ((System.Data.DataView)(this.DataSource)).Table;
                    }
                }

                if (mDataTable == null)
                {
                    if (!(DesignMode))
                    {
                        throw new ArgumentException(
                            "DataSourceTable could not be built. Since DataSource is \"null\". " + this.Name);
                    }

                    return -1;
                }

                return mDataTable.Columns.IndexOf(AColumnName);
            }

            return -1;
        }

        /// <summary>
        /// </summary>
        /// <returns>the number of the column that has its name stored in DisplayMember
        /// </returns>
        private System.Int32 GetColumnNrOfDisplayMember()
        {
            return GetColumnNr(DisplayMember);
        }

        /// <summary>
        /// </summary>
        /// <returns>the number of the column that has its name stored in ValueMember
        /// </returns>
        private System.Int32 GetColumnNrOfValueMember()
        {
            return GetColumnNr(ValueMember);
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
            this.CheckDataSourceType();
            this.CheckColumnStringCollection();
            base.OnDataSourceChanged(e);
        }

        /// <summary>
        /// This procedure is called when the value of the DataSource property is
        /// changed on ListControl.
        /// </summary>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void OnDataSourceChanging(TDataSourceChangingEventArgs e)
        {
            if ((DataSourceChanging != null) && (!(DesignMode)))
            {
                DataSourceChanging(this, e);
            }
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
            String TypedText;

            System.Int32 FoundIndex;
            System.Object FoundItem;
            String FoundText;

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
                TypedText = this.Text;

                // If the text is found in the Items list then Autocomplete
                FoundIndex = this.FindStringSortedByLength(TypedText);

                if (FoundIndex >= 0)
                {
                    // Get item from the list (Return type depends if Datasource ws bound or list created
                    FoundItem = this.Items[FoundIndex];

                    // Get the text of the item
                    FoundText = this.GetItemText(FoundItem);

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
                TypedText = this.Text;

                if (TypedText == "")
                {
                    this.SelectedItem = DBNull.Value;
                    this.SelectedValue = DBNull.Value;

                    // TLogging.Log('SelectedItem: ' + this.SelectedItem.ToString);
                    // TLogging.Log('Selected Index: ' + this.SelectedIndex.ToString);
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
                    // TLogging.Log('User may NOT enter ANY new values.');
                    if (this.SelectedItem.Equals(System.DBNull.Value))
                    {
                        this.Text = System.DBNull.Value.ToString();
                        this.UInitialString = System.DBNull.Value.ToString();

                        // TLogging.Log('Set the whole stuff to dbnull');
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
        /// <param name="ItemString">System.String The string of the item to add.
        /// </param>
        /// <returns>void</returns>
        private void AddItemToDataSource(String ItemString)
        {
            System.Int32 mNumDataSourceCols;
            System.Data.DataTable mDataTable;
            System.Data.DataRow mNewRow;
            System.Data.DataColumn mDataColumn;
            String mColName;

            if (this.DataSource is System.Data.DataTable)
            {
                mDataTable = (System.Data.DataTable) this.DataSource;
            }
            else if (this.DataSource is System.Data.DataView)
            {
                mDataTable = ((System.Data.DataView) this.DataSource).Table;
            }
            else // if this.DataSource is System.Data.DataSet then
            {
                return;
            }

            mNumDataSourceCols = mDataTable.Columns.Count;

            if (mNumDataSourceCols > 1)
            {
                return;
            }

            mDataColumn = mDataTable.Columns[0];
            mColName = mDataColumn.ColumnName;
            mNewRow = mDataTable.NewRow();
            mNewRow[0] = ItemString;
            mDataTable.Rows.InsertAt(mNewRow, 0);
            this.BeginUpdate();
            this.DisplayMember = mColName;
            this.ValueMember = mColName;
            this.DataSource = mDataTable.DefaultView;
            this.EndUpdate();
        }

        /// <summary>
        /// This function checks the existance of a column name within the datasource
        /// specified.
        ///
        /// </summary>
        /// <returns>void</returns>
        private bool DoColumnNamesExistInDataSource()
        {
            if (DataSource == null)
            {
                return true;
            }

            System.Data.DataColumnCollection mComboboxItems = null;

            // Get the Column collection of the datasource
            if (this.DataSource is System.Data.DataTable)
            {
                mComboboxItems = ((System.Data.DataTable) this.DataSource).Columns;
            }
            else if (this.DataSource is System.Data.DataView)
            {
                mComboboxItems = ((System.Data.DataView) this.DataSource).Table.Columns;
            }

            // Replace the placeholders #VALUE# and #DISPLAY# with the real stuff.
            System.Int32 mIndex = this.FColumnsToSearch.IndexOf(StrValueMember);

            if (mIndex >= 0)
            {
                this.FColumnsToSearch[mIndex] = this.ValueMember;
            }

            mIndex = this.FColumnsToSearch.IndexOf(StrDisplayMember);

            if (mIndex >= 0)
            {
                this.FColumnsToSearch[mIndex] = this.DisplayMember;
            }

            // If there is nothing to check then the result is FALSE anyway
            if ((mComboboxItems == null) || (mComboboxItems.Count < 1))
            {
                return false;
            }

            System.Int32 mCountExistance = 0;

            // Check whether the column names in this string collections are in the datasource
            foreach (String mColumnName in this.FColumnsToSearch)
            {
                if (mComboboxItems.Contains(mColumnName))
                {
                    mCountExistance = mCountExistance + 1;
                }
            }

            if (mCountExistance == this.FColumnsToSearch.Count)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function check the string collection of Columns to search against the
        /// datasource given to the combobox.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void CheckColumnStringCollection()
        {
            System.Collections.Specialized.StringCollection mStringCollection;
            System.Collections.Specialized.StringCollection mColumnNamesCollection;
            System.Data.DataColumnCollection mColumns;
            System.Data.DataTable mDataTable;

            // Initialization
            mDataTable = new System.Data.DataTable();
            mColumns = null;
            CheckDataSourceType();

            // Get the DataColumns of the DataSource
            if (DataSource is System.Data.DataView)
            {
                mColumns = ((System.Data.DataView)DataSource).Table.Columns;
            }
            else if (DataSource is System.Data.DataTable)
            {
                mColumns = ((System.Data.DataTable)DataSource).Columns;
            }

            // If the FColumnsToString collection has not yet initialized it must be done now.
            if ((this.FColumnsToSearch == null) || (this.FColumnsToSearch.Count < 1))
            {
                this.ColumnsToSearch = "";
            }

            mStringCollection = this.FColumnsToSearch;

            // Put the Columnnames of the DataSource into a StringCollection
            mColumnNamesCollection = new System.Collections.Specialized.StringCollection();

            foreach (DataColumn mColumn in mColumns)
            {
                if (!(mColumnNamesCollection.Contains(mColumn.ColumnName)))
                {
                    mColumnNamesCollection.Add(mColumn.ColumnName);
                }
            }

            mColumnNamesCollection.Add(StrValueMember);
            mColumnNamesCollection.Add(StrDisplayMember);

            // Remove fishy columns
            foreach (string mString in mStringCollection)
            {
                if (!(mColumnNamesCollection.Contains(mString)))
                {
                    mStringCollection.Remove(mString);
                }
            }

            this.FColumnsToSearch = mStringCollection;
        }

        /// <summary>
        /// This procedure complains if the type of the DataSource property of this
        /// ComboBox is not of the following:
        /// - System.Data.DataTable
        /// - System.Data.DataView
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void CheckDataSourceType()
        {
            if (!(DesignMode))
            {
                if (!HasDataSourceCorrectDataType(DataSource))
                {
                    throw new ArgumentException(DEFAULT_COMPLAIN + DataSource.GetType().ToString());
                }
            }
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
            int ReturnValue;

            System.Data.DataTable IndexTable;
            System.Data.DataColumn IndexCol;
            System.Data.DataRow IndexRow;
            System.Data.DataView IndexView;
            System.Int32 IndexValue;
            String ItemString;
            System.Int32 ItemIndex;
            System.Int32 ItemLength;
            System.Int32 ColumnIndex;
            System.Data.DataRowView TmpRowView;
            System.Type ItemType;
            bool IsCorrectDataType;

            // Set return value
            ReturnValue = -1;

            if (DataSource == null)
            {
                // TODO: proper implementation of FindStringSortedByLength for simple string lists
                return FindString(SearchString);
            }

            // Check whether the DataSource is of DataSet, DataTable, or DataView
            IsCorrectDataType = this.HasDataSourceCorrectDataType(this.DataSource);

            if (IsCorrectDataType == false)
            {
                return -1;
            }

            DoColumnNamesExistInDataSource();

            // Create the IndexTable where the results are stored. The table consists out
            // of a column that holds the index and a column which holds the length of the
            // found string.
            IndexTable = new System.Data.DataTable("IndexTable");
            IndexCol = new System.Data.DataColumn("Index", typeof(System.Int32));
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
                TmpRowView = (System.Data.DataRowView)ComboboxItem;

                // Iterate through the columns to search in the datasource
                foreach (String ColumnName in this.FColumnsToSearch)
                {
                    ColumnIndex = this.GetColumnNr(ColumnName);
                    ItemType = TmpRowView[ColumnIndex].GetType();

                    // todo: what about ledger numbers (Int32)?
                    if (ItemType == typeof(String))
                    {
                        ItemString = TmpRowView[ColumnIndex].ToString();
                        ItemString = ItemString.ToUpper();
                        SearchString = SearchString.ToUpper();

                        if (ItemString.StartsWith(SearchString))
                        {
                            // Store the result.
                            ItemIndex = this.Items.IndexOf(ComboboxItem);
                            ItemLength = ItemString.Length;
                            IndexRow = IndexTable.NewRow();
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

            // End of "for ComboboxItem in this.Items do"
            // Search for the shortest string in the IndexTable with the smallest indices.
            IndexView = new System.Data.DataView(IndexTable);

            // IndexView.Sort := 'Column ASC';
            IndexView.Sort = "Content ASC, ColumnName ASC";

            if (IndexTable.Rows.Count > 0)
            {
                IndexValue = System.Convert.ToInt32(IndexView[0]["Index"]);
                ReturnValue = IndexValue;
            }

            return ReturnValue;
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
                    if (TmpRowView[ColumnIndex].GetType() == typeof(String))
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

        // End of function

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
            int ReturnValue;

            System.Data.DataTable mDataTable;
            ReturnValue = -1;

            if (this.DataSource is System.Data.DataTable)
            {
                mDataTable = (System.Data.DataTable) this.DataSource;
            }
            else if (this.DataSource is System.Data.DataView)
            {
                mDataTable = ((System.Data.DataView)DataSource).Table;
            }
            else if (this.DataSource == null)
            {
                // for simple string list
                return 1;
            }
            else
            {
                // if this.DataSource is System.Data.DataSet then
                return -1;
            }

            try
            {
                ReturnValue = mDataTable.Columns.Count;
            }
            finally
            {
            }
            return ReturnValue;
        }

        /// <summary>
        /// This function returns the Int32 value of the selected item, first column
        /// </summary>
        /// <param name="ColumnNumber">The column number of the data source; if -1, then the value column is used</param>
        /// <returns>-1 if nothing is selected
        /// </returns>
        public Int32 GetSelectedInt32(int ColumnNumber)
        {
            if (ColumnNumber == -1)
            {
                ColumnNumber = GetColumnNrOfValueMember();
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
            if (ColumnNumber == -1)
            {
                ColumnNumber = GetColumnNrOfValueMember();
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
                ColumnNumber = GetColumnNrOfValueMember();
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
            else
            {
                return this.Text;
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
            Int32 PreviousSelectedIndex = SelectedIndex;
            Int32 NewSelectedIndex = FindStringInComboBox(AStr);

            if ((NewSelectedIndex == -1) && (Items.Count > 0))
            {
                if (PreviousSelectedIndex != -1)
                {
                    SelectedIndex = PreviousSelectedIndex;
                }
                else
                {
                    SelectedIndex = 0;
                }

                return false;
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
        /// This function checks whether the DataSource property of this combobox has
        /// the correct data type. The following types are correct data types:
        /// - System.Data.DataSet
        /// - System.Data.DataTable
        /// - System.Data.DataView
        /// </summary>
        /// <param name="ASource">DataSource to be checked.</param>
        /// <returns>true if the data type of the DataSource property is one of the types above, false otherwise.
        /// </returns>
        private bool HasDataSourceCorrectDataType(System.Object ASource)
        {
            if (DesignMode)
            {
                return true;
            }

            if (ASource == null)
            {
                return true;
            }

            if (UNIT_SUPPORTED_DATA_TYPES.IndexOf(ASource.GetType().ToString()) >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This procedure should prevent databinding from manipulating the original
        /// source.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void ProhibitChangeToDataSource()
        {
            if ((DesignMode == false) || (this.DataSource == null))
            {
                if (this.DataSource is System.Data.DataTable)
                {
                    ((System.Data.DataTable)DataSource).RejectChanges();
                }

                if (this.DataSource is System.Data.DataView)
                {
                    ((System.Data.DataView) this.DataSource).AllowDelete = false;
                    ((System.Data.DataView) this.DataSource).AllowEdit = false;
                    ((System.Data.DataView) this.DataSource).AllowNew = false;
                }
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
            String mTestString;

            System.Int32 mFoundIndex;
            System.Int32 mSelectedIndex;
            mTestString = this.UInitialString;
            mSelectedIndex = this.SelectedIndex;
            TLogging.Log("RestoreOriginalItem:mSelectedIndex: " + mSelectedIndex.ToString());

            // Get test for nil first and then set to '' (empty string) otherwise trim would
            // yield an exception.
            if ((mTestString == null) || (mSelectedIndex < 0))
            {
                mTestString = "";
            }

            mTestString = mTestString.Trim();
            TLogging.Log("RestoreOriginalItem:mTestString: " + mTestString);

            if (mTestString == "")
            {
                // It is only an empty string => ComboBox is not set to a certain item
                TLogging.Log("It is only an empty string => ComboBox is not set to a certain item");
                this.Text = "";
                this.SelectedIndex = -1;
                this.SelectedIndex = -1;
                SetBoundValueToDBNull();
            }
            else
            {
                // It is a valid string => The string must be in the items collection
                TLogging.Log("It is a valid string => The string must be in the items collection");
                this.Text = this.UInitialString;
                mFoundIndex = this.FindStringExact(this.UInitialString);
                this.SelectedIndex = mFoundIndex;
            }
        }

        /// <summary>
        /// This procedure sets the data bound value to DBNull. The procedure obtains
        /// the databound field through the databinding.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetBoundValueToDBNull()
        {
            System.Windows.Forms.ControlBindingsCollection mAllBindings;
            String mBindingField;
            String mBindingMember;
            String mBindingPath;
            System.Int32 mBindingPosition;
            System.ComponentModel.MarshalByValueComponent mDataSource;
            System.Data.DataTable mDataTable;
            System.Data.DataView mDataView;
            System.Data.DataColumn mDataColumn;
            mAllBindings = this.DataBindings;
            mDataTable = new System.Data.DataTable();

            // Iterate through all bindings
            foreach (System.Windows.Forms.Binding mSingleBinding in mAllBindings)
            {
                mDataSource = (System.ComponentModel.MarshalByValueComponent)mSingleBinding.DataSource;
                mBindingField = mSingleBinding.BindingMemberInfo.BindingField;
                mBindingMember = mSingleBinding.BindingMemberInfo.BindingMember;
                mBindingPath = mSingleBinding.BindingMemberInfo.BindingPath;
                mBindingPosition = mSingleBinding.BindingManagerBase.Position;

                if (mDataSource is System.Data.DataTable)
                {
                    mDataTable = (System.Data.DataTable)mDataSource;
                    mDataColumn = mDataTable.Columns[mBindingField];
                    mDataColumn.AllowDBNull = true;

                    // mDataTable.Rows.Item[mBindingPosition].Item[mBindingField] := System.DBNull.Value;
                    mDataTable.Rows[mBindingPosition][mBindingMember] = System.DBNull.Value;
                }
                else if (mDataSource is System.Data.DataView)
                {
                    mDataView = (System.Data.DataView)mDataSource;
                    mDataView.Table.Columns[mBindingField].AllowDBNull = true;

                    // mDataView.Item[mBindingPosition].Item[mBindingField] := System.DBNull.Value;
                    mDataView[mBindingPosition][mBindingMember] = System.DBNull.Value;
                }
                else
                {
                    MessageBox.Show("Not implemented yet!!!");
                }
            }
        }
    }
}