//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.InteropServices;
using GNU.Gettext;
using SourceGrid;
using Owf.Controls;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// todoComment
    /// </summary>
    public partial class TFrmPetraEditUtils : TFrmPetraUtils
    {
        #region Resourcestrings

        /// <summary>This Resourcestring needs to be public becaused it is used elsewhere as well.</summary>
        public static readonly string StrFormCaptionPrefixNew = Catalog.GetString("NEW: ");

// TODO        private static readonly string StrFormCaptionPrefixReadonly = Catalog.GetString("READ-ONLY: ");

        ///
        public static readonly string StrSingleRecordToSave = Catalog.GetString("There is 1 record that needs to be saved.");

        ///
        public static readonly string StrPluralRecordsToSave = Catalog.GetString("There are {0} records that need to be saved.");

        ///
        public static readonly string StrConsequenceIfNotSaved = Catalog.GetString(
            "{0}If you close this window without saving, you will lose all the changes that you have made.");

        ///
        public static readonly string StrSingleTableToSave = Catalog.GetString("The following table has data that needs to be saved:");

        ///
        public static readonly string StrPluralTablesToSave = Catalog.GetString("The following tables have data that needs to be saved:");

        ///
        public static readonly string StrRecordsInTable = Catalog.GetString("{0}   {1} {2} in the '{3}' table.");

        #endregion

        /// Tells which mode the screen should be opened in
        protected TScreenMode FScreenMode;

        /// Holds the DataSet that contains most data that is used on the screen
        protected DataSet FMainDS;

        /// Tells whether the Screen has changes that are not yet saved
        protected Boolean FHasChanges;

        /// Tells whether the Screen is working with new data (is not editing existing data)
        protected Boolean FHasNewData;

        /// <summary>Controls whether the SaveChanges function saves the changes or continues a begun save operation.</summary>
        protected Boolean FSubmitChangesContinue;

        /// Tells whether the check if the Form can be closed has already been run
        protected Boolean FCloseFormCheckRun;

        /// Tells whether a Detail of a list of Items is currently beeing edited
        protected Boolean FDetailEditMode;

        /// Tells whether a Detail of a list of Items is in protected mode (readonly)
        protected Boolean FDetailProtectedMode;

        /// Set this to true to prevent the Save button and MenuItem beeing autoenabled when data changes in the form
        protected Boolean FNoAutoEnableOfSaving;

        /// Nasty hack to detect whether a form has "just loaded"
        protected DateTime FFormLoadedTime;

        /// <summary>todoComment</summary>
        protected Boolean FSuppressChangeDetection;

        /// <summary>todoComment</summary>
        public event TDataSavingStartHandler DataSavingStarted;

        /// <summary>Fired when any control is changed</summary>
        public event TValueChangedHandler ControlChanged;

        /// <summary>todoComment</summary>
        public event TDataSavedHandler DataSaved;

        /// <summary>todoComment</summary>
        public event TNoNoMasterDataToSaveHandler NoMasterDataToSave;

        /// <summary>Controls whether the SaveChanges function saves the changes or continues a begun save operation.</summary>
        public bool SubmitChangesContinue
        {
            get
            {
                return FSubmitChangesContinue;
            }

            set
            {
                FSubmitChangesContinue = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean SuppressChangeDetection
        {
            get
            {
                return FSuppressChangeDetection;
            }

            set
            {
                FSuppressChangeDetection = value;
            }
        }

        /// <summary>todoComment</summary>
        public bool NoAutoEnableOfSaving
        {
            get
            {
                return FNoAutoEnableOfSaving;
            }

            set
            {
                FNoAutoEnableOfSaving = value;
            }
        }

        /// Tells whether the Screen has changes that are not yet saved
        public Boolean HasChanges
        {
            get
            {
                return FHasChanges;
            }
            set
            {
                FHasChanges = value;
            }
        }

        /// <summary>
        /// Tells whether the check if the Form can be closed has already been run
        /// </summary>
        public Boolean CloseFormCheckRun
        {
            get
            {
                return FCloseFormCheckRun;
            }
            set
            {
                FCloseFormCheckRun = value;
            }
        }

        /// Tells which mode the screen should be opened in
        public TScreenMode ScreenMode
        {
            get
            {
                return FScreenMode;
            }
            set
            {
                FScreenMode = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACallerForm">the int handle of the form that has opened this window; needed for focusing when this window is closed later</param>
        /// <param name="ATheForm"></param>
        /// <param name="AStatusBar"></param>"
        public TFrmPetraEditUtils(Form ACallerForm, IFrmPetraEdit ATheForm, TExtStatusBarHelp AStatusBar) : base(ACallerForm,
                                                                                                                (IFrmPetra)ATheForm,
                                                                                                                AStatusBar)
        {
            FCloseFormCheckRun = false;
            FFormLoadedTime = DateTime.Now;

            // default behavior is false, DONT supress detecting the change events
            FSuppressChangeDetection = false;
        }

        /** Adds event handlers for the appropiate onChange event to call a central procedure
         */
        public override void HookupAllInContainer(Control container)
        {
            FAllControls = new ArrayList();
            FControlsWithChildren = new ArrayList();
            base.EnumerateControls(container);
            HookupSomeControls();
        }

        /// <summary>todoComment</summary>
        public override void HookupAllControls()
        {
            FAllControls = new ArrayList();
            FControlsWithChildren = new ArrayList();
            base.HookupAllControls();
            HookupSomeControls();
        }

        /// <summary>todoComment</summary>
        public void HookupSomeControls()
        {
            Int32 otherCount = 0;
            String otherString = "";

            foreach (Control ctrl in FAllControls)
            {
                // If the control is used for dataentry then hookup the event
                // for data changing
                // This will call Local ControlValueChanged
                // and Control ValueChanged (virtual method)
                //
                // The first group are the important controls, (actually used for data entry )
                if (ctrl.GetType() == typeof(TextBox))
                {
                    ((TextBox)ctrl).TextChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TTxtMaskedTextBox))
                {
                    ((TTxtMaskedTextBox)ctrl).TextChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl is DateTimePicker)
                {
                    ((DateTimePicker)ctrl).ValueChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)ctrl).CheckedChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctrl).SelectedValueChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(CheckBox)) // This could be removed here to prevent its explicit use in forms.
                {
                    ((CheckBox)ctrl).CheckedChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TchkVisibleFocus))
                {
                    ((CheckBox)ctrl).CheckedChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl is NumericUpDown)
                {
                    ((NumericUpDown)ctrl).ValueChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TCmbAutoComplete))
                {
                    ((TCmbAutoComplete)ctrl).SelectedValueChanged += new EventHandler(this.MultiEventHandler);
                    ((TCmbAutoComplete)ctrl).TextChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TCmbVersatile))
                {
                    ((TCmbVersatile)ctrl).SelectedValueChanged += new EventHandler(this.MultiEventHandler);
                    ((TCmbVersatile)ctrl).TextChanged += new EventHandler(this.MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TClbVersatile))
                {
                    ((TClbVersatile)ctrl).ValueChanged += new EventHandler(MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(TtxtPetraDate))
                {
                    //((TtxtPetraDate)ctrl).DateChanged += new TPetraDateChangedEventHandler(this.TFrmPetraEditUtils_DateChanged);
                    ((TtxtPetraDate)ctrl).TextChanged += new EventHandler(MultiEventHandler);
                }
                else if (ctrl.GetType() == typeof(Ict.Common.Controls.TTxtNumericTextBox))
                {
                    ((Ict.Common.Controls.TTxtNumericTextBox)ctrl).TextChanged += new EventHandler(this.MultiEventHandler);
                }
                /*
                 * The remaining controls are listed in order to be able to
                 * warn the developer if a new control is added to a form
                 * which isn't hooked up to the system!
                 */
                else if ((ctrl.GetType() == typeof(Button))
                         || (ctrl.GetType() == typeof(System.Windows.Forms.ToolStrip))
                         || (ctrl.GetType() == typeof(System.Windows.Forms.MenuStrip))
                         || (ctrl.GetType() == typeof(Label))
                         || (ctrl.GetType() == typeof(LinkLabel))
                         || (ctrl.GetType() == typeof(TabPage))
                         || (ctrl.GetType() == typeof(Splitter))
                         || (ctrl.GetType() == typeof(Panel))
                         || (ctrl.GetType() == typeof(VScrollBar))
                         || (ctrl.GetType() == typeof(HScrollBar))
                         || (ctrl.GetType() == typeof(StatusBar))
                         || (ctrl.GetType() == typeof(Ict.Common.Controls.TExtStatusBarHelp))
                         || (ctrl.GetType() == typeof(GroupBox))
                         || (ctrl.GetType() == typeof(TbtnVarioText))
                         || (ctrl.GetType() == typeof(TreeView))
                         || (ctrl.GetType() == typeof(TTrvTreeView))
                         || (ctrl.GetType() == typeof(TbtnCreated))
                         || (ctrl.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
                         || (ctrl.GetType() == typeof(DevAge.Windows.Forms.Line))
                         || ((ctrl.GetType() == typeof(System.Windows.Forms.RichTextBox))
                             || (ctrl.GetType() == typeof(Owf.Controls.A1Panel))))
                {
                    // nothing to do
                }
                else if (ctrl.GetType().Name == "TexpTextBoxStringLengthCheckControl")
                {
                    // can't check for by type without creating compilation mess
                    // if you "require" ICT.Petra.CommonControls,
                    // compiler gets very upset about PetraForm being declared already
                }
                else
                {
                    // Control found which is new
                    otherCount = otherCount + 1;
                    otherString = otherString + ctrl.GetType().FullName + System.Environment.NewLine;
                }
            }

            if (otherCount > 0)
            {
                // warn the developer
                MessageBox.Show(otherString, "The following controls are not checked for live changes in PetraEditBaseForm:");
            }
        }

        /// <summary>todoComment</summary>
        private void UnhookControl(Control AControl)
        {
            if (AControl.GetType() == typeof(TextBox))
            {
                ((TextBox)AControl).TextChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(TTxtMaskedTextBox))
            {
                ((TTxtMaskedTextBox)AControl).TextChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl is DateTimePicker)
            {
                ((DateTimePicker)AControl).ValueChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(RadioButton))
            {
                ((RadioButton)AControl).CheckedChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(ComboBox))
            {
                ((ComboBox)AControl).SelectedValueChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(CheckBox))
            {
                ((CheckBox)AControl).CheckedChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl is NumericUpDown)
            {
                ((NumericUpDown)AControl).ValueChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(TCmbAutoComplete))
            {
                ((TCmbAutoComplete)AControl).SelectedValueChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(TCmbVersatile))
            {
                ((TCmbVersatile)AControl).SelectedValueChanged -= new EventHandler(this.MultiEventHandler);
                ((TCmbVersatile)AControl).TextChanged -= new EventHandler(this.MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(TClbVersatile))
            {
                ((TClbVersatile)AControl).ValueChanged -= new EventHandler(MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(TtxtPetraDate))
            {
                //((TtxtPetraDate)ctrl).DateChanged += new TPetraDateChangedEventHandler(this.TFrmPetraEditUtils_DateChanged);
                ((TtxtPetraDate)AControl).TextChanged -= new EventHandler(MultiEventHandler);
            }
            else if (AControl.GetType() == typeof(Ict.Common.Controls.TTxtNumericTextBox))
            {
                ((Ict.Common.Controls.TTxtNumericTextBox)AControl).TextChanged -= new EventHandler(this.MultiEventHandler);
            }
        }

        /// <summary>todoComment</summary>
        public void UnhookControl(Control AControl, Boolean AUnhookChildren)
        {
            UnhookControl(AControl);

            if (AUnhookChildren)
            {
                // recursive loop to catch all nested child controls
                foreach (Control ctrl in AControl.Controls)
                {
                    UnhookControl(ctrl, AUnhookChildren);
                }
            }
        }

        /// <summary>
        /// Returns a reference to the control that has the focus on the specified container.  The method is recursive so looks in its own container controls.
        /// </summary>
        /// <param name="AContainerControl">The container control to search (often pnlDetails)</param>
        /// <returns>The focused control or null if no control has the focus</returns>
        public Control GetFocusedControl(Control AContainerControl)
        {
            foreach (Control c in AContainerControl.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    Control c1 = GetFocusedControl(c);

                    if (c1 != null)
                    {
                        return c1;
                    }
                }
                else if (c.Focused)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// Clears the content of the specified control
        /// </summary>
        /// <param name="AControlToClear">The control whose content is to be cleared</param>
        /// <returns>True if the control was a known control type and was cleared successfully.
        /// If the control was a container control (such as a Panel), the method will return false.</returns>
        public bool ClearControl(Control AControlToClear)
        {
            bool ReturnValue = false;
            bool prevSuppressChangeDetection = FSuppressChangeDetection;

            DisableDataChangedEvent();

            try
            {
                if (AControlToClear.GetType() == typeof(TextBox))
                {
                    ((TextBox)AControlToClear).Clear();
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(TTxtMaskedTextBox))
                {
                    ((TTxtMaskedTextBox)AControlToClear).Clear();
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)AControlToClear).SelectedIndex = -1;
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)AControlToClear).Checked = false;
                    ReturnValue = true;
                }
                else if (AControlToClear is NumericUpDown)
                {
                    ((NumericUpDown)AControlToClear).Value = ((NumericUpDown)AControlToClear).Minimum;
                    ReturnValue = true;
                }
                else if ((AControlToClear.GetType() == typeof(TCmbAutoComplete))
                         || (AControlToClear.GetType() == typeof(TCmbVersatile)))
                {
                    // NOTE from AlanP: This code looks odd, but it is the way to clear these boxes....
                    // The first call to set the index to -1 actually sets it to 0 (if it isn't 0 already)
                    // Then the second call sets it from 0 to -1
                    // Then we can clear the text
                    // This behaviour applies particularly on bound data sources.
                    // Don't know if this is a Windows quirk or whether it also applies to Linux - but if it is just Windows, calling twice won't do any harm
                    ((TCmbAutoComplete)AControlToClear).SelectedIndex = -1;
                    ((TCmbAutoComplete)AControlToClear).SelectedIndex = -1;
                    ((TCmbAutoComplete)AControlToClear).Text = String.Empty;
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(TCmbAutoPopulated))
                {
                    ((TCmbAutoPopulated)AControlToClear).cmbCombobox.SelectedIndex = -1;
                    ((TCmbAutoPopulated)AControlToClear).cmbCombobox.SelectedIndex = -1;
                    ((TCmbAutoPopulated)AControlToClear).cmbCombobox.Text = String.Empty;
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(TClbVersatile))
                {
                    ((TClbVersatile)AControlToClear).ClearSelected();
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(TtxtPetraDate))
                {
                    ((TtxtPetraDate)AControlToClear).Clear();
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(Ict.Common.Controls.TTxtNumericTextBox))
                {
                    ((Ict.Common.Controls.TTxtNumericTextBox)AControlToClear).ClearBox();
                    ReturnValue = true;
                }
                else if (AControlToClear.GetType() == typeof(Ict.Common.Controls.TTxtCurrencyTextBox))
                {
                    ((Ict.Common.Controls.TTxtCurrencyTextBox)AControlToClear).NumberValueDecimal = 0;
                    ReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogAtLevel(7,
                    "Exception caught in TFrmPetraEditUtils.ClearControl(): " + AControlToClear.Name + "(" + AControlToClear.ToString() + "): " +
                    ex.Message);
            }

            if (prevSuppressChangeDetection == false)
            {
                // when we started we were tracking changes, so we need to reset that
                // Note we only enable this if it was enabled at the start!!!
                EnableDataChangedEvent();
            }

            return ReturnValue;
        }

        /// <summary>
        /// This is the default method for counting the number of changed records and specifying a message that can be displayed
        ///   as part of the 'Do you want to save changes' dialog.
        /// </summary>
        /// <param name="ADataSet">The DataSet that is to be examined for changes</param>
        /// <param name="AMessage">The message that will be displayed</param>
        /// <returns>The total number of changed records in all tables of the DataSet</returns>
        public int GetChangedRecordCount(DataSet ADataSet, out string AMessage)
        {
            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int allChangesCount = 0;

            foreach (DataTable dt in ADataSet.Tables)
            {
                if (dt != null)
                {
                    int tableChangesCount = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr.RowState != DataRowState.Unchanged)
                        {
                            tableChangesCount++;
                            allChangesCount++;
                        }
                    }

                    if (tableChangesCount > 0)
                    {
                        TableAndCountList.Add(new Tuple <string, int>(((TTypedDataTable)dt).TableDBLabel, tableChangesCount));
                    }
                }
            }

            AMessage = String.Empty;

            if (TableAndCountList.Count > 0)
            {
                if (ADataSet.Tables.Count == 1)
                {
                    // single table DataSet so we are only interested in the count
                    Tuple <string, int>TableAndCount = TableAndCountList[0];

                    if (TableAndCount.Item2 > 0)
                    {
                        if (TableAndCount.Item2 == 1)
                        {
                            AMessage = TFrmPetraEditUtils.StrSingleRecordToSave;
                        }
                        else
                        {
                            AMessage = String.Format(
                                TFrmPetraEditUtils.StrPluralRecordsToSave,
                                TableAndCount.Item2);
                        }

                        AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, "  ");
                    }
                }
                else
                {
                    // multi-table DataSet
                    if (TableAndCountList.Count == 1)
                    {
                        AMessage = TFrmPetraEditUtils.StrSingleTableToSave;
                    }
                    else
                    {
                        AMessage = TFrmPetraEditUtils.StrPluralTablesToSave;
                    }

                    foreach (Tuple <string, int>TableAndCount in TableAndCountList)
                    {
                        AMessage += String.Format(TFrmPetraEditUtils.StrRecordsInTable,
                            Environment.NewLine,
                            TableAndCount.Item2,
                            Catalog.GetPluralString("record", "records", TableAndCount.Item2),
                            TableAndCount.Item1);
                    }

                    AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
                }
            }

            return allChangesCount;
        }

        /// <summary>
        /// Recursively clears the content of all the controls in the specified container
        /// </summary>
        /// <param name="AParentControl">The container control whose controls are to be cleared (often this will be pnlDetails)</param>
        public void ClearControls(Control AParentControl)
        {
            foreach (Control ctrl in AParentControl.Controls)
            {
                if (ClearControl(ctrl))
                {
                    continue;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    // Clear these controls as well
                    ClearControls(ctrl);
                }
            }
        }

        /// <summary>
        /// Gets a list of relevant controls, sorted correctly by multi-level TabIndex in exactly the same way that the system uses to determine screen tab order.
        /// This includes nested controls so the tab order is, say, 100, 110, 120.30, 120.40, 130
        /// The method calls itself recursively for each contained panel
        /// </summary>
        /// <param name="ASortedControlList">Pass in the SortedList that will be the final result</param>
        /// <param name="AContainerControl">The contianer control (a Panel) that is to be searched for controls and sub-Panels</param>
        /// <param name="APrefix">A TabIndex prefix - use an empty string for the initial call</param>
        public void GetSortedControlList(ref SortedList <string, Control>ASortedControlList, Control AContainerControl, string APrefix)
        {
            foreach (Control control in AContainerControl.Controls)
            {
                if (control is Panel)
                {
                    // Recursive call with an extended prefix
                    GetSortedControlList(ref ASortedControlList, control, String.Format("{0}{1}.", APrefix, control.TabIndex.ToString("0000")));
                }
                else if (control is TextBox || control is ComboBox || control is TCmbAutoPopulated)
                {
                    ASortedControlList.Add(String.Format("{0}{1}", APrefix, control.TabIndex.ToString("0000")), control);
                }
            }
        }

        /** This is available for the child form to respond to by overriding
         */
        protected void ControlValueChanged(Control Actrl)
        {
            if (ControlChanged != null)
            {
                ControlChanged(Actrl);
            }
        }

        /** This responds to the fact data has changed at this level
         */
        public void LocalControlValueChanged(Control Actrl)
        {
            SetChangedFlag();
        }

        /** Event handlers for all controls
         * this one is for all controls which have the standard sender : Object, e EventArgs
         */
        protected void MultiEventHandler(System.Object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            string ctrlname = ctrl.Name;

//TLogging.Log(DateTime.Now.ToString() + " MULTIEVENT Handler.  SuppressChangeDetection: " + this.SuppressChangeDetection);
            if (ctrlname == "lblLabel")
            {
                //
                return;
            }

            if ((this.SuppressChangeDetection == false)
                && ((ctrl.Tag == null) || (ctrl.Tag.GetType() != typeof(string))
                    || !((string)ctrl.Tag).Contains(MCommonResourcestrings.StrCtrlSuppressChangeDetection))
                && ctrl.Visible && ctrl.Enabled)
            {
                LocalControlValueChanged(ctrl);
                ControlValueChanged(ctrl);

                // string ctrltype = sender.GetType().FullName;
                //  TLogging.Log(DateTime.Now.ToString() + " MULTIEVENT Ctrl: " + ctrlname + " Type: " + ctrltype);
            }
        }

        /// <summary>
        /// Event Handler for the TtxtPetraDate Control. Simply calls <see cref="MultiEventHandler" />
        /// so we get an Event that the Control's value has changed.
        /// </summary>
        /// <param name="Sender">Sending Object.</param>
        /// <param name="e">Event Arguments (not used).</param>
        protected void TFrmPetraEditUtils_DateChanged(object Sender, TPetraDateChangedEventArgs e)
        {
            MultiEventHandler(Sender, null);
        }

        #region Event Handlers

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MniEdit_Click(System.Object sender, System.EventArgs e)
        {
        }

        #endregion

        #region Custom Events

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAnyDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            if (!FNoAutoEnableOfSaving)
            {
//TLogging.Log("Column_Changing Event: Column=" + e.Column.ColumnName +
//                "; Column content=" + e.Row[e.Column.ColumnName].ToString() +
//                "; " + e.ProposedValue.ToString());
                SetChangedFlag();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAnyDataRowChanging(System.Object sender, DataRowChangeEventArgs e)
        {
            if (!FNoAutoEnableOfSaving)
            {
//TLogging.Log("Row_Changing Event: DataTable=" + e.Row.Table.ToString());
                SetChangedFlag();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataSaved(System.Object sender, TDataSavedEventArgs e)
        {
            if (DataSaved != null)
            {
                DataSaved(this, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNoMasterDataToSave(System.Object sender, TNoMasterDataToSaveEventArgs e)
        {
            if (NoMasterDataToSave != null)
            {
                NoMasterDataToSave(this, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDataSavingStart(System.Object sender, System.EventArgs e)
        {
            if (DataSavingStarted != null)
            {
                DataSavingStarted(this, e);
            }
        }

        #endregion

        #region Helper Functions

        /**
         * Hook events that enable the 'Save' ToolBarButton and File/Save menu entry
         *
         */
        protected void HookupDataChangeEvents()
        {
            // Hook up to ColumnChanging and RowDeleting Events of DataTables that are
            // used in the Form.
        }

        #region Interface Implementation

        /// <summary>
        /// todoComment
        /// </summary>
        public void EnableDataChangedEvent()
        {
            this.SuppressChangeDetection = false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisableDataChangedEvent()
        {
            this.SuppressChangeDetection = true;
        }

        /// <summary>
        /// Enables the 'Save' ToolBarButton and the 'File->Save' MenuItem and sets the Form in a state where it contains changes
        /// (<see cref="HasChanges" />).
        /// </summary>
        /// <remarks>Useful when manual code needs to do these actions (that are normally automatically handled in a Form).</remarks>
        public void SetChangedFlag()
        {
            EnableAction("actSave", true);

            FHasChanges = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        override public void InitActionState()
        {
            EnableAction("actSave", FHasChanges);
        }

        /// <summary>
        /// Disanables the 'Save' ToolBarButton and the 'File->Save' MenuItem and sets the Form in a state where it doesn't contains changes
        /// (<see cref="HasChanges" />).
        /// </summary>
        /// <remarks>Useful when manual code needs to do these actions (that are normally automatically handled in a Form).</remarks>
        public void DisableSaveButton()
        {
            EnableAction("actSave", false);

            FHasChanges = false;
        }

        /// <summary>
        /// Gets the Form / UserControl to show the Default Cursor on the FORM.
        /// </summary>
        /// <remarks>
        /// When called from a UserControl, the Default Cursor will be shown
        /// on the Form, not the UserControl (different to calling this.Cursor = ...
        /// in a UserControl, which sets the Cursor on the UserControl, not the Form)!
        /// </remarks>
        public void ShowDefaultCursor()
        {
            GetForm().Cursor = Cursors.Default;
        }

        /// <summary>
        /// Gets the Form / UserControl to show the Wait Cursor on the FORM.
        /// </summary>
        /// <remarks>
        /// When called from a UserControl, the Wait Cursor will be shown
        /// on the Form, not the UserControl (different to calling this.Cursor = ...
        /// in a UserControl, which sets the Cursor on the UserControl, not the Form)!
        /// </remarks>
        public void ShowWaitCursor()
        {
            GetForm().Cursor = Cursors.WaitCursor;
        }

        #endregion

        /**
         * This Procedure will get called from the SaveChanges procedure whenever a
         * Save operation is finished (successful or unsuccesful).
         *
         * @param sender The Object that throws this Event
         * @param e Event Arguments. Success property is true if saving was successful,
         * otherwise false.
         *
         */
        public void FormDataSaved(System.Object sender, TDataSavedEventArgs e)
        {
            MessageBox.Show("DataSaved Event. Success: " + e.Success.ToString());
        }

        /**
         * This Procedure will get called from the SaveChanges procedure before it
         * actually performs any saving operation.
         *
         * @param sender The Object that throws this Event
         * @param e Event Arguments.
         *
         */
        public void FormDataSavingStarted(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show("DataSavingStarted Event.");
        }

        /**
         * This function checks whether the window can be closed.
         *
         * It can be used to find out e.g. if something is still beeing edited and
         * unsaved or whether a particular screen won't allow closing for other reasons.
         *
         * @return true if window can be closed
         *
         */
        public override bool CanClose()
        {
            return !FHasChanges;
        }

        /// <summary>
        /// don't close window when the details are being edited;
        /// if there are changes, ask the user what to do:
        /// save and close, discard and close, or cancel closing
        /// </summary>
        /// <returns>returns false if the form cannot be closed</returns>
        public Boolean CloseFormCheck()
        {
            CloseFormCheckRun = true;
            Boolean ReturnValue = false;

            if (HasChanges)
            {
                if (InDetailEditMode())
                {
                    CloseFormCheckRun = false;
                    return false;
                }

                // still unsaved data in the DataSet
                string SaveQuestion = MCommonResourcestrings.StrFormHasUnsavedChanges + Environment.NewLine + Environment.NewLine;
                string alternativeMessage;

                // Do we know how many rows have been changed?
                int changedRecordCount = ((IFrmPetraEdit)FTheForm).GetChangedRecordCount(out alternativeMessage);

                if (changedRecordCount > 0)
                {
                    if (alternativeMessage == String.Empty)
                    {
                        // Construct a default message
                        if (changedRecordCount == 1)
                        {
                            SaveQuestion += TFrmPetraEditUtils.StrSingleRecordToSave;
                        }
                        else
                        {
                            SaveQuestion += String.Format(TFrmPetraEditUtils.StrPluralRecordsToSave, changedRecordCount);
                        }

                        SaveQuestion += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, "  ");
                    }
                    else
                    {
                        // Use the alternative message supplied
                        SaveQuestion += alternativeMessage;
                    }

                    SaveQuestion += Environment.NewLine + Environment.NewLine;
                }

                SaveQuestion += MCommonResourcestrings.StrFormHasUnsavedChangesQuestion;

                System.Windows.Forms.DialogResult SaveQuestionAnswer = MessageBox.Show(SaveQuestion,
                    MCommonResourcestrings.StrGenericWarning,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        if (((IFrmPetraEdit)FTheForm).SaveChanges() == false)
                        {
                            // Form contains invalid data that hasn't been corrected yet
                            CloseFormCheckRun = false;
                            return false;
                        }
                        else
                        {
                            HasChanges = false;
                        }
                    }
                    catch (Exception)
                    {
                        CloseFormCheckRun = false;
                        throw;
                    }

                    ReturnValue = true;
                }
                else if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.No)
                {
                    HasChanges = false;
                    ReturnValue = true;
                }
                else if (SaveQuestionAnswer == System.Windows.Forms.DialogResult.Cancel)
                {
                    CloseFormCheckRun = false;
                    ReturnValue = false;
                }
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /**
         * Event Handler that is invoked when the Form is about to close - no matter
         * how the closing was invoked (by calling Form.Close, a Close button, the
         * x Button of a Form, etc).
         *
         * @param sender Event sender
         * @param e EventArgs that allow cancelling of the closing
         *
         */
        public override void TFrmPetra_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((e != null) && !CloseFormCheckRun)
            {
                if (!CloseFormCheck())
                {
                    // MessageBox.Show("TFrmPetra_Closing: e.Cancel = true");
                    e.Cancel = true;
                }
            }

            if ((e == null) || (e.Cancel == false))
            {
                // tidy up
                base.TFrmPetra_Closing(sender, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Boolean InDetailEditMode()
        {
            Boolean ReturnValue;

            // still in Detail Edit Mode?
            if (FDetailEditMode)
            {
                MessageBox.Show(
                    Catalog.GetString("You need to finish editing by choosing the 'Done' button\nbefore you can close the window!"),
                    Catalog.GetString("Need to Finish Editing!"));
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>todoComment</summary>
        public bool DetailEditMode
        {
            get
            {
                return FDetailEditMode;
            }
            set
            {
                FDetailEditMode = value;
            }
        }

        /// <summary>todoComment</summary>
        public bool DetailProtectedMode
        {
            get
            {
                return FDetailProtectedMode;
            }
            set
            {
                FDetailProtectedMode = value;
            }
        }


        /// <summary>todoComment</summary>
        protected void SetScreenCaption()
        {
            String CaptionPrefix = "";

            if (FHasNewData)
            {
                CaptionPrefix = StrFormCaptionPrefixNew;
            }

            FWinForm.Text = CaptionPrefix + Catalog.GetString("New OpenPetra Screen");
        }

        #endregion
    }

    /// <summary>todoComment</summary>
    public class PetraEditForm
    {
        /// <summary>todoComment</summary>
        public const String FORM_CHANGEDDATAINDICATOR = " (*)";
    }

    /// <summary>todoComment</summary>
    public interface IFrmPetraEdit : IFrmPetra
    {
// TODO?        void DisableDataChangedEvent();

// TODO?        void EnableDataChangedEvent();

        /// <summary>
        /// Save the changes
        /// </summary>
        bool SaveChanges();

        /// <summary>
        /// Get the number of changed records and specify a message to incorporate into the 'Do you want to save?' message box
        /// </summary>
        /// <param name="AMessage">An optional message to display.  If the parameter is an empty string a default message will be used</param>
        /// <returns>The number of changed records.  Return -1 to imply 'unknown'.</returns>
        int GetChangedRecordCount(out string AMessage);
    }
}