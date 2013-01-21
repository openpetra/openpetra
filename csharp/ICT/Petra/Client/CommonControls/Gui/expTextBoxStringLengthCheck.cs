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
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MSysMan.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Enumeration for the Designer. Holds the possible values for ListTable.
    /// No enum prefixes here since these values are shown in the Designer.
    ///
    /// </summary>
    public enum TListTextBoxTypes
    {
        /// <summary>todoComment</summary>
        System_Windows_Forms_Textbox,

        /// <summary>todoComment</summary>
        PartnerKey
    };

    /// <summary>
    /// This unit provides an autocomplete ComboBox
    /// Short description:
    /// This extender provider gets the maximum length of a TextBox string. The
    /// datasource of the TextBoxes has to be in the database and the Petra datastore.
    /// This extender provider uses the following static function:
    /// - TPetraDataStore.GetLengthOfTableField(mTableName, mColumnName)
    /// User Guide:
    /// In order to use this TextBox check you need to perform the following steps in
    /// the order given below:
    /// 1 Select the TexpTextBoxStringLengthCheck from the category "General" of
    ///   the Tool Palette and drag it onto the UserControl
    /// 2 Give this new component a name (here refered to as Name)
    /// 3 Add the following lines into the routine, where the databinding takes place
    ///   after all other controls of this UserControl have been databound:
    ///
    ///    // Extender Provider
    ///    this.&lt;Name&gt;.RetrieveTextboxes(Self);
    ///
    /// 4 Recompile the project the UserControl belongs to.
    /// </summary>
    public partial class TexpTextBoxStringLengthCheck : System.ComponentModel.Component, IExtenderProvider
    {
        #region Resourcetexts - DON'T TRANSLATE AS THEY ARE FOR LOGGING/DEBUGGING PUPROSES

//        private static readonly string strUnknownControl = "Unkown control! Databinding System.Object could not be retreived!";

        #endregion

        /// <summary>todoComment</summary>
        private const String strFoundControlKey = "Key";

        /// <summary>todoComment</summary>
        private const String strFoundControlTextBox = "TextBox";

        /// <summary>todoComment</summary>
        private const String strFoundControlMaxLength = "MaxLength";

        /// <summary>todoComment</summary>
        private const String strFoundControlTableName = "TableName";

        /// <summary>todoComment</summary>
        private const String strFoundControlColumnName = "ColumnName";

        /// <summary>todoComment</summary>
        private const String strFoundControlDataSource = "DataSource";

        private System.Windows.Forms.UserControl UExtenderHost;
        private System.Windows.Forms.Control.ControlCollection UExtenderControlCollection;
        private System.Data.DataTable UFoundControlTable;

        /// <summary>
        /// This property gets or sets the creator.
        ///
        /// </summary>
        protected System.Windows.Forms.UserControl ExtenderHost
        {
            get
            {
                return this.UExtenderHost;
            }

            set
            {
                this.UExtenderHost = value;
            }
        }

        /// <summary>
        /// This property gets or sets the creator.
        ///
        /// </summary>
        public String ExtenderHostName
        {
            get
            {
                return this.ExtenderHost.Name;
            }
        }

        #region Creation and Disposal

        /// <summary>
        /// This is the constructor of this class.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TexpTextBoxStringLengthCheck() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
            InitProvider();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// This is the constructor of this class.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TexpTextBoxStringLengthCheck(System.ComponentModel.IContainer Container)
            : base()
        {
            Container.Add(this);

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            InitProvider();
        }

        /// <summary>
        /// This procedure initializes a table where all the data from the forms
        /// textboxes are stored.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void InitializeFoundControlTable()
        {
            DataColumn[] mPrimaryKeys = null;
            System.Data.DataColumn mDataColumn;
            this.UFoundControlTable = new System.Data.DataTable("FoundControls");

            // Add primary key columns
            mDataColumn = new System.Data.DataColumn(strFoundControlKey, typeof(String));
            this.UFoundControlTable.Columns.Add(mDataColumn);

            // Add primary key
            mPrimaryKeys = new DataColumn[] {
                this.UFoundControlTable.Columns[0]
            };
            this.UFoundControlTable.PrimaryKey = mPrimaryKeys;

            // Add additional columns here
            mDataColumn = new System.Data.DataColumn(strFoundControlTextBox, typeof(System.Windows.Forms.Control));
            this.UFoundControlTable.Columns.Add(mDataColumn);
            mDataColumn = new System.Data.DataColumn(strFoundControlMaxLength, typeof(int));
            this.UFoundControlTable.Columns.Add(mDataColumn);
            mDataColumn = new System.Data.DataColumn(strFoundControlTableName, typeof(String));
            this.UFoundControlTable.Columns.Add(mDataColumn);
            mDataColumn = new System.Data.DataColumn(strFoundControlColumnName, typeof(String));
            this.UFoundControlTable.Columns.Add(mDataColumn);
            mDataColumn = new System.Data.DataColumn(strFoundControlDataSource, typeof(System.Object));
            this.UFoundControlTable.Columns.Add(mDataColumn);
        }

        /// <summary>
        /// This procedure does some initializations for this class
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitProvider()
        {
            // this.UExtenderHost := new System.Windows.Forms.UserControl();
            this.InitializeFoundControlTable();
        }

        /// <summary>
        /// This procedure checks whether the control in its parameters contains a
        /// textbox, if so the textboxes detail are added to a table, if not it
        /// it does nothing.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool IsControlAllowed(System.Windows.Forms.Control AControl)
        {
            bool ReturnValue;

            ReturnValue = true;

            if (AControl is TtxtAutoPopulatedButtonLabel)
            {
                TLogging.Log("TexpTextBoxStringLengthCheck.IsControlAllowed: " + AControl.Name + " Control is NOT allowed", TLoggingType.ToLogfile);
                ReturnValue = false;
            }

            return ReturnValue;
        }

        #endregion

        /// <summary>
        /// This procedure adds the controls to the table.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void AddTextBoxToFoundControlTable(String AKey, System.Windows.Forms.Control ATextBox)
        {
            System.Data.DataRow mDataRow;
            DataRow[] mFoundRows = null;
            String mSelectStr;
            System.Int32 mCount;

            // Initialization
            mCount = 0;
            mDataRow = this.UFoundControlTable.NewRow();
            mDataRow[strFoundControlKey] = AKey;
            mDataRow[strFoundControlTextBox] = ATextBox;

            // Add the row to the table
            mSelectStr = strFoundControlKey + " = '" + AKey + "'";
            try
            {
                mFoundRows = this.UFoundControlTable.Select(mSelectStr);
                mCount = mFoundRows.Length;
            }
            catch (Exception)
            {
                MessageBox.Show("unexpected behaviour in AddTextBoxToFoundControlTable");
            }

            if (mCount <= 0)
            {
                this.UFoundControlTable.Rows.Add(mDataRow);
                this.UFoundControlTable.AcceptChanges();
            }
        }

        /// <summary>
        /// This function is called from the designer in order to evaluate when and
        /// when not to display any new properties.
        ///
        /// </summary>
        /// <returns>void</returns>
        public bool CanExtend(System.Object AnObject)
        {
            bool ReturnValue = false;

            // If the System.Object is derived from System.Windows.Forms.UserControl then we have to deal with it
            if (AnObject is System.Windows.Forms.UserControl)
            {
                this.ExtenderHost = (System.Windows.Forms.UserControl)AnObject;
                this.UExtenderControlCollection = new System.Windows.Forms.Control.ControlCollection(this.UExtenderHost);
                this.UExtenderControlCollection = this.UExtenderHost.Controls;

                // Check whether we need to extend these
                foreach (System.Windows.Forms.Control mControl in this.UExtenderControlCollection)
                {
                    if ((mControl is System.Windows.Forms.TextBox) || (mControl is TTxtPartnerKeyTextBox)
                        || (mControl is System.Windows.Forms.TextBox))
                    {
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure checks for the ExtenderProvider System.Object in its host. If
        /// the UserControl contains this type of ExtenderProvider it returns TRUE,
        /// if not it returns FALSE.
        ///
        /// </summary>
        /// <returns>void</returns>
        private bool CheckForExtenderProvider(System.Windows.Forms.UserControl AUserControl)
        {
            // TODO CheckForExtenderProvider
            return false;
        }

        /// <summary>
        /// This function checks whether the control is disabled (enabled = false)
        /// and/or ReadOnly (ReadOnly = true). Then this functions returns a FALSE.
        /// The function returns TRUE when (ReadOnly = FALSE AND Enabled = TRUE).
        ///
        /// </summary>
        /// <returns>void</returns>
        private bool ControlIsEnabled(System.Windows.Forms.Control AControl)
        {
            bool ReturnValue;

            System.Windows.Forms.TextBox mTextBox;

            // mAutoPopulatedButtonLabel: txtAutoPopulatedButtonLabel.TtxtAutoPopulatedButtonLabel;
            TTxtPartnerKeyTextBox mPartnerKeyTextBox;
            System.Windows.Forms.ControlBindingsCollection mDataBindings;

            // mDataBinding:             System.Windows.Forms.Binding;
            ReturnValue = false;

            // TLogging.Log('=== START ============= ControlIsEnabled(AControl: System.Windows.Forms.Control ==============', [TLoggingType.ToLogfile]);
            // TLogging.Log('  ControlName: ' + AControl.Name + ';    ControlType: ' + AControl.GetType().ToString, [TLoggingType.ToLogfile]);
            if (AControl is System.Windows.Forms.TextBox)
            {
                mTextBox = (System.Windows.Forms.TextBox)AControl;

                // TLogging.Log('  Yes it is really a TextBox, Enabled: ' + mTextBox.Enabled.ToString + ', ReadOnly: ' + mTextBox.ReadOnly.ToString + ', Visible: ' + mTextBox.Visible.ToString, [TLoggingType.ToLogfile]);
                mDataBindings = mTextBox.DataBindings;

                if ((mTextBox.Visible == true) && (mTextBox.Enabled == true) && (mTextBox.ReadOnly == false) && (mDataBindings.Count > 0))
                {
                    ReturnValue = true;
                }
            }
            else if (AControl is TTxtPartnerKeyTextBox)
            {
                mPartnerKeyTextBox = (TTxtPartnerKeyTextBox)AControl;
                mDataBindings = mPartnerKeyTextBox.DataBindings;

                if ((mPartnerKeyTextBox.Enabled == true) && (mPartnerKeyTextBox.ReadOnly == false) && (mDataBindings.Count > 0))
                {
                    ReturnValue = true;
                }
            }

            // TLogging.Log('=== END   ============= ControlIsEnabled(AControl: System.Windows.Forms.Control ==============', [TLoggingType.ToLogfile]);
            return ReturnValue;
        }

        /// <summary>
        /// This procedure registers this very ExtenderProvider System.Object to its host.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RegisterExtenderProvider(System.Windows.Forms.UserControl AUserControl)
        {
            // TODO RegisterExtenderProvider
        }

        /// <summary>
        /// This procedure retrieves the datastructure of the textboxes
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RetrieveDataSourceDetails(System.Windows.Forms.Binding ABinding, System.Data.DataRow ADataRow)
        {
            System.Object mDataSource;
            String mTableName = "";
            String mField;
            String mColumnName = "";
            String mBindingMember;
            System.Collections.Specialized.StringCollection mStringArray;
            mDataSource = ABinding.DataSource;

            if (mDataSource is System.Data.DataView)
            {
                // Messagebox.Show('DataType: ' + mDataSource.GetType().ToString  + "\n" + 'BindingMember: ' + ABinding.BindingMemberInfo.BindingMember.ToString);
                mTableName = ((System.Data.DataView)mDataSource).Table.ToString();
                mField = ABinding.BindingMemberInfo.BindingField;
                mColumnName = StringHelper.UpperCamelCase(mField, true, true);
            }
            else if (mDataSource is System.Data.DataTable)
            {
                // Messagebox.Show('DataType: ' + mDataSource.GetType().ToString  + "\n" + 'BindingMember: ' + ABinding.BindingMemberInfo.BindingMember.ToString);
                mTableName = ((System.Data.DataTable)mDataSource).ToString();
                mField = ABinding.BindingMemberInfo.BindingField;
                mColumnName = StringHelper.UpperCamelCase(mField, true, true);
            }
            else if (mDataSource is System.Data.DataSet)
            {
                mBindingMember = ABinding.BindingMemberInfo.BindingMember.ToString();
                mStringArray = StringHelper.StrSplit(mBindingMember, ".");
                mTableName = String.Copy(mStringArray[0]);
                mField = String.Copy(mStringArray[1]);
                mColumnName = StringHelper.UpperCamelCase(mField, true, true);

                // Messagebox.Show('DataSet!!!' + "\n" + 'TableName: ' + mTableName + "\n" + 'ColumnName: ' + mField);
            }
            else
            {
                MessageBox.Show(
                    mDataSource.GetType().ToString() + "\n" + ABinding.BindingMemberInfo.BindingMember.ToString() + "\n" +
                    " Please ask the programmer you trust to put in some more code here!!!");
            }

            ADataRow[strFoundControlTableName] = mTableName;
            ADataRow[strFoundControlColumnName] = mColumnName;

            // Messagebox.Show('TableName: ' + ADataRow[strFoundControlTableName].ToString + ' ColumnName: ' + ADataRow[strFoundControlColumnName].ToString);
        }

        /// <summary>
        /// This procedure retrieves the DataBindings of the TextBoxes and imposes the
        /// correct MaxLength.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void ImposeMaxLength()
        {
            System.Object mValueObject;
            System.Windows.Forms.Control mControl;
            TTxtPartnerKeyTextBox mPartnerKey;
            System.Windows.Forms.ControlBindingsCollection mDataBindings;
            System.Windows.Forms.Binding mDataBinding;

            foreach (DataRow mDataRow in this.UFoundControlTable.Rows)
            {
                mValueObject = mDataRow[strFoundControlTextBox];
                mControl = (System.Windows.Forms.Control)mValueObject;

                if (mControl is System.Windows.Forms.TextBox)
                {
                    mDataBindings = mControl.DataBindings;

                    // messagebox.Show('DataBindingsCollection: ' + mDataBindings.ToString);
                    try
                    {
                        mDataBinding = (System.Windows.Forms.Binding)mDataBindings[0];
                        this.RetrieveDataSourceDetails(mDataBinding, mDataRow);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show("Exception for control: " + mControl.Name + "\n" + E.ToString());

                        // exit;
                    }
                    this.RetrieveTextBoxMaxLength(mDataRow);
                }
                else if (mControl is TtxtAutoPopulatedButtonLabel)
                {
                }
                // TLogging.Log('ImposeMaxLength: AControl is txtAutoPopulatedButtonLabel.TtxtAutoPopulatedButtonLabel', [TLoggingType.ToLogfile]);
                // mButtonLabel := txtAutoPopulatedButtonLabel.TtxtAutoPopulatedButtonLabel(mControl);
                // mDataBinding := mButtonLabel.GetTextBoxDataBinding;
                // this.RetrieveDataSourceDetails(mDataBinding, mDataRow);
                // this.RetrieveTextBoxMaxLength(mDataRow);
                else if (mControl is TTxtPartnerKeyTextBox)
                {
                    mPartnerKey = (TTxtPartnerKeyTextBox)mControl;
                    mDataBinding = mPartnerKey.txtTextBox.DataBindings[0];
                    this.RetrieveDataSourceDetails(mDataBinding, mDataRow);
                    this.RetrieveTextBoxMaxLength(mDataRow);
                }
                else
                {
                    // TLogging.Log(strUnknownControl, [TLoggingType.ToLogfile]);
                }
            }
        }

        /// <summary>
        /// This procedure sets the MaxLength property of the textbox control.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetMaxLength(System.Windows.Forms.Control AControl, System.Int32 AMaxLength)
        {
            // messagebox.Show('ControlName: ' + AControl.Name + "\n" + 'ControlType: ' + AControl.GetType().ToString + "\n" + AMaxLength.ToString);
            // TLogging.Log('ControlName: ' + AControl.Name + "\n" + 'ControlType: ' + AControl.GetType().ToString + "\n" + AMaxLength.ToString, [TLoggingType.ToLogfile]);
            if (AControl is System.Windows.Forms.TextBox)
            {
                ((System.Windows.Forms.TextBox)AControl).MaxLength = AMaxLength;

                // messagebox.Show('MaxLength set!!!');
            }
            else if (AControl is TtxtAutoPopulatedButtonLabel)
            {
                // TLogging.Log('SetMaxLength: AControl is txtAutoPopulatedButtonLabel.TtxtAutoPopulatedButtonLabel', [TLoggingType.ToLogfile]);
                // (txtAutoPopulatedButtonLabel.TtxtAutoPopulatedButtonLabel(AControl)).MaxLength := AMaxLength;
                // /    messagebox.Show('MaxLength set!!!');
            }
            else if (AControl is TTxtPartnerKeyTextBox)
            {
                ((TTxtPartnerKeyTextBox)AControl).MaxLength = AMaxLength;

                // messagebox.Show('MaxLength set!!!');
            }
            else
            {
                MessageBox.Show("Please report this error to the programmer of your trust! Cheers mate!");
            }
        }

        /// <summary>
        /// This procedure retrieves the length of the textbox specified in the datarow
        /// of the UFoundControlTable table.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RetrieveTextBoxMaxLength(System.Data.DataRow ADataRow)
        {
            String mTableName;
            String mColumnName;

            System.Int32 mMaxLength;
            String mKey;
            String mControlName;
            String mMessageStr;
            System.Windows.Forms.Control mControl;

            // Initialization
            mTableName = ADataRow[strFoundControlTableName].ToString();
            mColumnName = ADataRow[strFoundControlColumnName].ToString();
            try
            {
                mMaxLength = TTypedDataTable.GetLength(mTableName, mColumnName);
            }
            catch (Exception e)
            {
                mMaxLength = 32767;
                MessageBox.Show("TableName: " + mTableName + "ColumnName: " + mColumnName + "\n" + "\n" + e.ToString());
            }
            mControl = (System.Windows.Forms.Control)ADataRow[strFoundControlTextBox];

            // Compile some information to check the results
            mKey = ADataRow[strFoundControlKey].ToString();
            mControlName = mControl.Name;
            mMessageStr = "Got the information for the following TextBox: " + "\n" + "  TableName:         " + mTableName + "\n" +
                          "  ColumnName:        " + mColumnName + "\n" + "  ControlName:       " + mControlName + "\n" + "  MaxLength:         " +
                          mMaxLength.ToString() + "\n" + "  Key internal info: " + mKey + "\n";

            // Check whether the result is still invalid and if possible provide reasons
            if (mMaxLength < 0)
            {
                // TLogging.Log(mMessageStr, [TLoggingType.ToLogfile]);
                throw new System.Exception(mMessageStr);
            }

            // Messagebox.Show(mMessageStr);
            SetMaxLength(mControl, mMaxLength);
        }

        /// <summary>
        /// This procedure checks whether the control in its parameters contains a
        /// textbox, if so the textboxes detail are added to a table, if not it
        /// it does nothing.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void GetTextBox(System.Windows.Forms.Control AControl)
        {
            System.DateTime mDate;
            String mKey;
            mDate = System.DateTime.Now;

            if ((AControl is System.Windows.Forms.TextBox)
                || (AControl is TtxtAutoPopulatedButtonLabel)
                || (AControl is TTxtPartnerKeyTextBox))
            {
                // Only iterate through the controls which are enabled and where ReadOnly is false!
                // TLogging.Log('TextBox added: ' + AControl.Name, [TLoggingType.ToLogfile]);
                if ((ControlIsEnabled(AControl) == true) && (this.IsControlAllowed(AControl) == true))
                {
                    mKey = AControl.Name + '_' + mDate.Millisecond.ToString();
                    this.AddTextBoxToFoundControlTable(mKey, AControl);

                    // TLogging.Log('TextBox added: ' + AControl.Name, [TLoggingType.ToLogfile]);
                }
            }
            else
            {
                // If we encounter a container control we have to iterate through this control
                // If the container control happens to be a UserControl then we might not
                // have to iterate through it.
                if (AControl is System.Windows.Forms.UserControl)
                {
                    // TLogging.Log('=== Start ============= AControl is System.Windows.Forms.UserControl ==============', [TLoggingType.ToLogfile]);
                    // TLogging.Log('Start to iterate through the following UserControl: ' + AControl.Name, [TLoggingType.ToLogfile]);
                    if (CheckForExtenderProvider((System.Windows.Forms.UserControl)AControl) == false)
                    {
                        foreach (Control mControl in AControl.Controls)
                        {
                            // We were not lucky
                            // TLogging.Log('Unlucky job, we have to dive into this control: ' + AControl.Name, [TLoggingType.ToLogfile]);
                            GetTextBox(mControl);
                        }
                    }

                    // TLogging.Log('=== END   ============= AControl is System.Windows.Forms.UserControl ==============', [TLoggingType.ToLogfile]);
                }
                // If the container control happens not to be a UserControl then we must!!
                // iterate through it.
                else if ((AControl is System.Windows.Forms.Panel) || (AControl is System.Windows.Forms.GroupBox)
                         || (AControl is System.Windows.Forms.TabControl))
                {
                    // TLogging.Log('=== Start ============= AControl is System.Windows.Forms.Panel=====================', [TLoggingType.ToLogfile]);
                    // TLogging.Log('=== Start ============= AControl is System.Windows.Forms.GroupBox =================', [TLoggingType.ToLogfile]);
                    // TLogging.Log('Start to iterate through the following control: ' + AControl.Name, [TLoggingType.ToLogfile]);
                    foreach (Control mControl in AControl.Controls)
                    {
                        // TLogging.Log('Dutyful job we dive into this control: ' + AControl.Name, [TLoggingType.ToLogfile]);
                        GetTextBox(mControl);
                    }

                    // TLogging.Log('End of iteration through the following control: ' + AControl.Name, [TLoggingType.ToLogfile]);
                    // TLogging.Log('=== End =============== AControl is System.Windows.Forms.GroupBox =================', [TLoggingType.ToLogfile]);
                    // TLogging.Log('=== End =============== AControl is System.Windows.Forms.Panel=====================', [TLoggingType.ToLogfile]);
                }
            }
        }

        /// <summary>
        /// This procedure ensures that all textboxes in the form have the MaxLength
        /// property set to the value given from the database.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RetrieveTextboxes(System.Object AnObject)
        {
            // If the System.Object is derived from System.Windows.Forms.UserControl then we have to deal with it
            if (AnObject is System.Windows.Forms.UserControl)
            {
                // Get the host System.Object
                this.ExtenderHost = (System.Windows.Forms.UserControl)AnObject;

                // TLogging.Log('Host: ' + this.ExtenderHost.Name, [TLoggingType.ToLogfile]);
                this.RegisterExtenderProvider(this.ExtenderHost);

                // Messagebox.Show(this.CheckForExtenderProvider(this.ExtenderHost).ToString);
                try
                {
                    this.UExtenderControlCollection = new System.Windows.Forms.Control.ControlCollection(this.UExtenderHost);
                    this.UExtenderControlCollection = this.UExtenderHost.Controls;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error happened!!!");
                }

                // Check whether we find any TextBoxes in this collection then we have to add
                // these to this.UTextBoxCollection
                foreach (System.Windows.Forms.Control mControl in this.UExtenderControlCollection)
                {
                    GetTextBox(mControl);
                }

                // Here the DataSources and the maximum length of the text of the TextBoxes is retrieved
                this.ImposeMaxLength();
            }
        }
    }
}