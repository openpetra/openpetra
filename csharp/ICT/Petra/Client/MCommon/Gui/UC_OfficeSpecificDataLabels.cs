//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using DevAge.ComponentModel;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using DevAge.Drawing;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data; // Implicit reference
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using System.Globalization;
using GNU.Gettext;


namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TTellGuiToEnableSaveButton();

    /// <summary>
    /// Contains User Control Office Specific Data Labels
    /// </summary>
    public partial class TUC_LocalDataLabelValues : System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
    {
        private TFrmPetraEditUtils FPetraUtilsObject;

        // private Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS FMainDS;

        private Int64 FPartnerKey;
        private Int32 FApplicationKey;
        private Int64 FRegistrationOffice;
        private Boolean FUserControlInitialised;

        private OfficeSpecificDataLabelsTDS FLocalDataDS;
        private SourceGrid.Grid FLocalDataLabelValuesGrid;

        private TOfficeSpecificDataLabelUseEnum FOfficeSpecificDataLabelUse;

        /// <summary>todoComment</summary>
        public TGridRowInfo FGridRowInfo;

        /// <summary>todoComment</summary>
        public Boolean FInFocussingMode;

        private DataView FLabelUseDV;
        private TCellEventNotificationController FEnterNotificationController;
        private PDataLabelTable FDataLabelDT;
        private PDataLabelUseTable FDataLabelUseDT;
        private PDataLabelLookupCategoryTable FDataLabelLookupCategoryDT;
        private PDataLabelLookupTable FDataLabelLookupDT;
        private PDataLabelValuePartnerTable FDataLabelValuePartnerDT;
        private PDataLabelValueApplicationTable FDataLabelValueApplicationDT;
        private Boolean FGridIsSetUp;


        /// <summary>
        /// constructor
        /// </summary>
        public TUC_LocalDataLabelValues() : base()
        {
            FGridIsSetUp = false;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
        }

        /// helper object for the whole screen
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }

            set
            {
                FPetraUtilsObject = value;
            }
        }

        /// dataset for the whole screen
        public DataSet MainDS
        {
            set
            {
                // FMainDS = value;
            }
        }

        //public Ict.Petra.Shared.MPartner.Partner.Data.PartnerEditTDS MainDS

        /// dataset for local data structure
        public Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS LocalDataDS
        {
            get
            {
                return FLocalDataDS;
            }
        }

        /// helper object for the whole screen
        public SourceGrid.Grid LocalDataLabelValuesGrid
        {
            get
            {
                return FLocalDataLabelValuesGrid;
            }
        }

        #region Implement interface functions

        /// auto generated
        public void RunOnceOnActivation()
        {
        }

        /// auto generated
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// auto generated
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }

        #endregion

        /// <summary>
        /// event handler
        /// </summary>
        public void GrdLocalDataLabelValues_SizeChanged(System.Object sender, System.EventArgs e)
        {
            // inform the logic that the grid has been resized
            ActUponGridSizeChanged();
        }

        /// <summary>
        /// frees some static objects
        /// </summary>
        /// <param name="Disposing"></param>
        protected void SpecialDispose(Boolean Disposing)
        {
            FreeStaticObjects();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitializeDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            // this variable is never actually used
            // set the delegate function from the calling object
            // FDelegateGetPartnerShortName = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitUserControl()
        {
            // dummy version needed by code generator
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        public void InitialiseUserControlAndShowData(PDataLabelValuePartnerTable ADataTable,
            System.Int64 APartnerKey,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            FPartnerKey = APartnerKey;

            if (!FUserControlInitialised)
            {
                InitialiseUserControlInternal(ADataTable, null, AOfficeSpecificDataLabelUse);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AApplicationKey"></param>
        /// <param name="ARegistrationOffice"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        public void InitialiseUserControlAndShowData(PDataLabelValueApplicationTable ADataTable,
            Int64 APartnerKey,
            Int32 AApplicationKey,
            Int64 ARegistrationOffice,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            FPartnerKey = APartnerKey;
            FApplicationKey = AApplicationKey;
            FRegistrationOffice = ARegistrationOffice;

            if (!FUserControlInitialised)
            {
                InitialiseUserControlInternal(null, ADataTable, AOfficeSpecificDataLabelUse);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTableValuePartner"></param>
        /// <param name="ADataTableValueApplication"></param>
        /// <param name="AOfficeSpecificDataLabelUse"></param>
        private void InitialiseUserControlInternal(PDataLabelValuePartnerTable ADataTableValuePartner,
            PDataLabelValueApplicationTable ADataTableValueApplication,
            TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            // Set the delegate function in the logic System.Object (so it can call back).
            // This can not be done in procedure InitializeDelegateGetPartnerShortName because FLogic
            // does not yet exist then.
            // Set up Data Sets and Tables
            InitialiseDataStructures(ADataTableValuePartner, ADataTableValueApplication);

            // Set up screen logic
            FOfficeSpecificDataLabelUse = AOfficeSpecificDataLabelUse;
            FLocalDataLabelValuesGrid = grdLocalDataLabelValues;

            //PossiblySomethingToSave += new TTellGuiToEnableSaveButton(this.@PossiblySaveSomething);

            // Set up Grid
            SetupGridColumnsAndRows();

            // Signalize that the initialisation is done
            FUserControlInitialised = true;
        }

        /// <summary>
        ///
        /// </summary>
        private void PossiblySaveSomething()
        {
            if (this.ParentForm != null)
            {
                ((TFrmPetraEditUtils)((IFrmPetraEdit) this.ParentForm).GetPetraUtilsObject()).SetChangedFlag();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ControlValueHasChanged(System.Object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                ((TFrmPetraEditUtils)((IFrmPetraEdit) this.ParentForm).GetPetraUtilsObject()).SetChangedFlag();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void PartnerKeyControlValueHasChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (this.ParentForm != null)
            {
                ((TFrmPetraEditUtils)((IFrmPetraEdit) this.ParentForm).GetPetraUtilsObject()).SetChangedFlag();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public event TTellGuiToEnableSaveButton PossiblySomethingToSave;

        /// <summary>
        /// todoComment
        /// </summary>
        public void ExecutePossiblySomethingToSave()
        {
            // can only be called from within the class
            if (PossiblySomethingToSave != null)
            {
                PossiblySomethingToSave();
            }
        }

        /// <summary>
        /// Initialises DataSets and Tables
        ///
        /// </summary>
        /// <returns>void</returns>
        private void InitialiseDataStructures(PDataLabelValuePartnerTable ADataTableValuePartner,
            PDataLabelValueApplicationTable ADataTableValueApplication)
        {
            FDataLabelDT = (PDataLabelTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelList);

            // FDataLabelDT.Tablename := 'PDataLabel';
            // MessageBox.Show('FDataLabelDT.Rows[0].Key: ' + FDataLabelDT.Row[0].Key.ToString);
            FDataLabelUseDT = (PDataLabelUseTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelUseList);

            // MessageBox.Show('FDataLabelUseDT.Rows[0].Key: ' + FDataLabelUseDT.Row[0].Idx1.ToString);
            FDataLabelLookupDT = (PDataLabelLookupTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelLookupList);

            // MessageBox.Show('FDataLabelLookupDT.Row[0].CategoryCode: ' + FDataLabelLookupDT.Row[0].CategoryCode);
            FDataLabelLookupCategoryDT = (PDataLabelLookupCategoryTable)TDataCache.TMPartner.GetCacheablePartnerTable(
                TCacheablePartnerTablesEnum.DataLabelLookupCategoryList);

            // MessageBox.Show('FDataLabelLookupCategoryDT.Row[0].CategoryCode: ' + FDataLabelLookupCategoryDT.Row[0].CategoryCode);
            // Set up DataSet that holds all DataTables
            FLocalDataDS = new OfficeSpecificDataLabelsTDS("OfficeSpecificData");

            // Merge in cached Typed DataTables (should be done differently in the future when TypedDataSet.Tables.Add works fine)
            FLocalDataDS.Merge(FDataLabelDT);
            FLocalDataDS.Merge(FDataLabelUseDT);
            FLocalDataDS.Merge(FDataLabelLookupDT);
            FLocalDataDS.Merge(FDataLabelLookupCategoryDT);
            FDataLabelValuePartnerDT = ADataTableValuePartner;
            FDataLabelValueApplicationDT = ADataTableValueApplication;
        }

        /// <summary>
        /// Sets up the value cell(s) for a specific data label
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupGridValueCell(Int32 ARowIndex, PDataLabelRow ADataLabelRow)
        {
            Control cellControl;

            System.Windows.Forms.TextBox TextBoxEditor;
            TtxtPetraDate DateEditor;
            System.Windows.Forms.CheckBox CheckBoxEditor;
            TTxtNumericTextBox TextBoxNumericEditor;
            TTxtCurrencyTextBox TextBoxCurrencyEditor;
            TCmbAutoPopulated LookupValueEditor;
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            SourceGrid.Cells.Views.Cell ValueModel;
            SourceGrid.Cells.Views.Cell SuffixModel;
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;

            // prepare model for the value cells
            ValueModel = new SourceGrid.Cells.Views.Cell();
            ValueModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            ValueModel.Font = new Font(FLocalDataLabelValuesGrid.Font.FontFamily.Name, FLocalDataLabelValuesGrid.Font.Size, FontStyle.Bold);

            // prepare model for suffix cells (e.g. for currency)
            SuffixModel = new SourceGrid.Cells.Views.Cell();
            SuffixModel.BackColor = FLocalDataLabelValuesGrid.BackColor;
            SuffixModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            // In this case the data value rows will only be created once a value is entered
            GetOrCreateDataLabelValueRow(false, ADataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

            // initialize cell control
            cellControl = null;

            // Create field, according to specified data type
            // Create character field
            if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR)
            {
                TextBoxEditor = new System.Windows.Forms.TextBox();
                cellControl = TextBoxEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    TextBoxEditor.Text = DataLabelValuePartnerRow.ValueChar;
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    TextBoxEditor.Text = DataLabelValueApplicationRow.ValueChar;
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    TextBoxEditor.Text = "";
                }

                // enable save button in editor when cell contents have changed
                TextBoxEditor.TextChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)TextBoxEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = TextBoxEditor;
            }
            // Create float field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT)
            {
                TextBoxNumericEditor = new TTxtNumericTextBox();
                TextBoxNumericEditor.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
                TextBoxNumericEditor.DecimalPlaces = ADataLabelRow.NumDecimalPlaces;
                TextBoxNumericEditor.NullValueAllowed = true;
                cellControl = TextBoxNumericEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    TextBoxNumericEditor.NumberValueDecimal = DataLabelValuePartnerRow.ValueNum;
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    TextBoxNumericEditor.NumberValueDecimal = DataLabelValueApplicationRow.ValueNum;
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    TextBoxNumericEditor.NumberValueDecimal = null;
                }

                // enable save button in editor when cell contents have changed
                TextBoxNumericEditor.TextChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)TextBoxNumericEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = TextBoxNumericEditor;
            }
            // Create data field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE)
            {
                DateEditor = new TtxtPetraDate();
                DateEditor.Date = null;
                cellControl = DateEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    if (!DataLabelValuePartnerRow.IsValueDateNull())
                    {
                        DateEditor.Date = DataLabelValuePartnerRow.ValueDate;
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (!DataLabelValueApplicationRow.IsValueDateNull())
                    {
                        DateEditor.Date = DataLabelValueApplicationRow.ValueDate;
                    }
                }

                // enable save button in editor when cell contents have changed
                DateEditor.DateChanged += new TPetraDateChangedEventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)DateEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = DateEditor;
            }
            // Create integer field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER)
            {
                TextBoxNumericEditor = new TTxtNumericTextBox();
                TextBoxNumericEditor.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
                TextBoxNumericEditor.NullValueAllowed = true;
                cellControl = TextBoxNumericEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    TextBoxNumericEditor.NumberValueInt = DataLabelValuePartnerRow.ValueInt;
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    TextBoxNumericEditor.NumberValueInt = DataLabelValueApplicationRow.ValueInt;
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    TextBoxNumericEditor.NumberValueInt = null;
                }

                // enable save button in editor when cell contents have changed
                TextBoxNumericEditor.TextChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)TextBoxNumericEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = TextBoxNumericEditor;
            }
            // Create currency field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
            {
                TextBoxCurrencyEditor = new TTxtCurrencyTextBox();
                TextBoxCurrencyEditor.DecimalPlaces = 2;
                TextBoxCurrencyEditor.CurrencySymbol = ADataLabelRow.CurrencyCode;
                TextBoxCurrencyEditor.NullValueAllowed = true;
                cellControl = TextBoxCurrencyEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    TextBoxCurrencyEditor.NumberValueDecimal = DataLabelValuePartnerRow.ValueCurrency;
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    TextBoxCurrencyEditor.NumberValueDecimal = DataLabelValueApplicationRow.ValueCurrency;
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    TextBoxCurrencyEditor.NumberValueDecimal = null;
                }

                // enable save button in editor when cell contents have changed
                TextBoxCurrencyEditor.TextChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)TextBoxCurrencyEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = TextBoxCurrencyEditor;
            }
            // Create boolean field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN)
            {
                CheckBoxEditor = new System.Windows.Forms.CheckBox();
                cellControl = CheckBoxEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    CheckBoxEditor.Checked = DataLabelValuePartnerRow.ValueBool;
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    CheckBoxEditor.Checked = DataLabelValueApplicationRow.ValueBool;
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    CheckBoxEditor.Checked = false;
                }

                // enable save button in editor when cell contents have changed
                CheckBoxEditor.CheckedChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)CheckBoxEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = CheckBoxEditor;
            }
            // Create partner key field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY)
            {
                PartnerKeyEditor = new TtxtAutoPopulatedButtonLabel();
                PartnerKeyEditor.ASpecialSetting = true;
                PartnerKeyEditor.ButtonText = ADataLabelRow.Text + ':';
                PartnerKeyEditor.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleRight;
                PartnerKeyEditor.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
                PartnerKeyEditor.TabStop = false;
                cellControl = PartnerKeyEditor;

                // AutomaticallyUpdateDataSource: very rare, but needed here
                PartnerKeyEditor.AutomaticallyUpdateDataSource = true;

                if (DataLabelValuePartnerRow != null)
                {
                    PartnerKeyEditor.Text = StringHelper.PartnerKeyToStr(DataLabelValuePartnerRow.ValuePartnerKey);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    PartnerKeyEditor.Text = StringHelper.PartnerKeyToStr(DataLabelValueApplicationRow.ValuePartnerKey);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    PartnerKeyEditor.Text = StringHelper.PartnerKeyToStr(0);
                }

                // display partner name linked to partner key
                PartnerKeyEditor.ResetLabelText();

                // enable save button in editor when cell contents have changed
                PartnerKeyEditor.ValueChanged += new TDelegatePartnerChanged(this.PartnerKeyControlValueHasChanged);
                PartnerKeyEditor.TextChanged += new System.EventHandler(this.ControlValueHasChanged);


                FLocalDataLabelValuesGrid[ARowIndex, 0] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue(PartnerKeyEditor, new Position(ARowIndex, 0)));
                FLocalDataLabelValuesGrid[ARowIndex, 0].Tag = PartnerKeyEditor;
                FLocalDataLabelValuesGrid[ARowIndex, 0].ColumnSpan = 3;
            }
            // Create lookup field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP)
            {
                // Get the instance of the combobox (created in the actual user interface class)
                LookupValueEditor = new TCmbAutoPopulated();
                LookupValueEditor.Filter = PDataLabelLookupTable.GetCategoryCodeDBName() + " = '" + ADataLabelRow.LookupCategoryCode + "'";
                LookupValueEditor.ListTable = TCmbAutoPopulated.TListTableEnum.DataLabelLookupList;
                LookupValueEditor.InitialiseUserControl();
                cellControl = LookupValueEditor;

                if (DataLabelValuePartnerRow != null)
                {
                    LookupValueEditor.SetSelectedString(DataLabelValuePartnerRow.ValueLookup);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    LookupValueEditor.SetSelectedString(DataLabelValueApplicationRow.ValueLookup);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    LookupValueEditor.Text = "";
                }

                // enable save button in editor when cell contents have changed
                LookupValueEditor.SelectedValueChanged += new EventHandler(this.ControlValueHasChanged);
                LookupValueEditor.TextChanged += new EventHandler(this.ControlValueHasChanged);

                FLocalDataLabelValuesGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                FLocalDataLabelValuesGrid.LinkedControls.Add(new LinkedControlValue((Control)LookupValueEditor, new Position(ARowIndex, 1)));
                FLocalDataLabelValuesGrid[ARowIndex, 1].Tag = LookupValueEditor;
                FLocalDataLabelValuesGrid[ARowIndex, 1].ColumnSpan = 2;
            }

            // perform actions that need to be done for each control
            if (cellControl != null)
            {
                // remember the added control to get the value back lateron
                FGridRowInfo.SetControl(ARowIndex, cellControl);

                // handle focus change when field is entered
                cellControl.Enter += new EventHandler(this.UpdateGridFocusFromExternalControl);

                // set help text for control
                PetraUtilsObject.SetStatusBarText(cellControl, ADataLabelRow.Description);
            }

            // check if value is editable
            if (!ADataLabelRow.Editable)
            {
                FLocalDataLabelValuesGrid[ARowIndex, 1].Editor.EnableEdit = false;
            }
        }

        /// <summary>
        /// Get or create a data column for a given data label
        ///
        /// </summary>
        /// <returns>void</returns>
        private void GetOrCreateDataLabelValueRow(Boolean ACreateIfNotExisting,
            PDataLabelRow ADataLabelRow,
            out PDataLabelValuePartnerRow ADataLabelValuePartnerRow,
            out PDataLabelValueApplicationRow ADataLabelValueApplicationRow)
        {
            // Find out whether this Partner has already got a value for this Label and put
            // it into the corresponding table or create a new row if it does not exist yet
            ADataLabelValuePartnerRow = null;
            ADataLabelValueApplicationRow = null;

            switch (FOfficeSpecificDataLabelUse)
            {
                case TOfficeSpecificDataLabelUseEnum.Person:
                case TOfficeSpecificDataLabelUseEnum.Family:
                case TOfficeSpecificDataLabelUseEnum.Church:
                case TOfficeSpecificDataLabelUseEnum.Organisation:
                case TOfficeSpecificDataLabelUseEnum.Unit:
                case TOfficeSpecificDataLabelUseEnum.Bank:
                case TOfficeSpecificDataLabelUseEnum.Venue:
                case TOfficeSpecificDataLabelUseEnum.Personnel:
                    ADataLabelValuePartnerRow = (PDataLabelValuePartnerRow)FDataLabelValuePartnerDT.Rows.Find(new Object[] { FPartnerKey,
                                                                                                                             ADataLabelRow.Key });

                    // if value record does not exist yet then create it here
                    if ((ADataLabelValuePartnerRow == null) && ACreateIfNotExisting)
                    {
                        ADataLabelValuePartnerRow = FDataLabelValuePartnerDT.NewRowTyped(true);
                        ADataLabelValuePartnerRow.PartnerKey = FPartnerKey;
                        ADataLabelValuePartnerRow.DataLabelKey = (int)ADataLabelRow.Key;
                        FDataLabelValuePartnerDT.Rows.Add(ADataLabelValuePartnerRow);
                    }

                    break;

                case TOfficeSpecificDataLabelUseEnum.LongTermApp:
                case TOfficeSpecificDataLabelUseEnum.ShortTermApp:
                    ADataLabelValueApplicationRow =
                        (PDataLabelValueApplicationRow)FDataLabelValueApplicationDT.Rows.Find(new Object[] { FPartnerKey, FApplicationKey,
                                                                                                             FRegistrationOffice,
                                                                                                             ADataLabelRow.Key });

                    // if value record does not exist yet then create it here
                    if ((ADataLabelValueApplicationRow == null) && ACreateIfNotExisting)
                    {
                        ADataLabelValueApplicationRow = FDataLabelValueApplicationDT.NewRowTyped(true);
                        ADataLabelValueApplicationRow.PartnerKey = FPartnerKey;
                        ADataLabelValueApplicationRow.ApplicationKey = FApplicationKey;
                        ADataLabelValueApplicationRow.RegistrationOffice = FRegistrationOffice;
                        ADataLabelValueApplicationRow.DataLabelKey = (int)ADataLabelRow.Key;
                        FDataLabelValueApplicationDT.Rows.Add(ADataLabelValueApplicationRow);
                    }

                    break;
            }
        }

        private void UpdateGridFocusFromExternalControl(System.Object sender, System.EventArgs e)
        {
            Int32 Row;

            // exit;
            if (FInFocussingMode)
            {
                return;
            }

            // set mode to prevent recursive calls
            FInFocussingMode = true;

            if ((sender.GetType() == typeof(System.Windows.Forms.TextBox))
                || (sender.GetType() == typeof(TtxtPetraDate))
                || (sender.GetType() == typeof(System.Windows.Forms.CheckBox))
                || (sender.GetType() == typeof(TTxtNumericTextBox))
                || (sender.GetType() == typeof(TTxtCurrencyTextBox))
                || (sender.GetType() == typeof(TCmbAutoPopulated))
                || (sender.GetType() == typeof(TtxtAutoPopulatedButtonLabel)))
            {
                Row = FGridRowInfo.GetRow((Control)sender);
                FLocalDataLabelValuesGrid.Selection.Focus(
                    new Position(FLocalDataLabelValuesGrid[Row, 1].Row.Index, FLocalDataLabelValuesGrid[Row, 1].Column.Index), true);
            }

            // reset mode
            FInFocussingMode = false;
        }

        private Boolean InUseForApplication()
        {
            Boolean ReturnValue;

            ReturnValue = false;

            // only return true if the control is used to display application values
            switch (FOfficeSpecificDataLabelUse)
            {
                case TOfficeSpecificDataLabelUseEnum.LongTermApp:
                case TOfficeSpecificDataLabelUseEnum.ShortTermApp:
                    ReturnValue = true;
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets up the columns and rows of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupGridColumnsAndRows()
        {
            Int32 GroupHeaderCount;
            Int32 Counter;
            Int32 CurrentRow;

            SourceGrid.Cells.Views.Cell LabelModel;
            SourceGrid.Cells.Views.Cell TitleModel;
            String CurrentGroup;
            String LastGroup = "";
            String LabelUse;
            PDataLabelRow DataLabelRow;
            DataColumn ForeignTableColumn;

            // Create class that hooks up Grid Cell Enter event notification
            FEnterNotificationController = new TCellEventNotificationController();
            FEnterNotificationController.Initialize(this);
            FLabelUseDV = new DataView(FLocalDataDS.DataLabelUseList);

            // Create DataColumns that contain data from the DataLabel DataTable
            // add column: group of the data label
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PDataLabelTable.GetGroupDBName();
            ForeignTableColumn.Expression = "Parent." + PDataLabelTable.GetGroupDBName();
            FLocalDataDS.DataLabelUseList.Columns.Add(ForeignTableColumn);

            // add column: is this label to be displayed
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.Boolean");
            ForeignTableColumn.ColumnName = "Parent_" + PDataLabelTable.GetDisplayedDBName();
            ForeignTableColumn.Expression = "Parent." + PDataLabelTable.GetDisplayedDBName();
            FLocalDataDS.DataLabelUseList.Columns.Add(ForeignTableColumn);
            LabelUse = Enum.GetName(typeof(TOfficeSpecificDataLabelUseEnum), FOfficeSpecificDataLabelUse);

            // Sort by Index
            FLabelUseDV.Sort = PDataLabelUseTable.GetIdx1DBName() + " ASC";

            // Show only Labels that should get displayed and are of the type that this UserControl is for
            FLabelUseDV.RowFilter = PDataLabelUseTable.GetUseDBName() + " = '" + LabelUse + "' AND Parent_" + PDataLabelTable.GetDisplayedDBName() +
                                    " = 1";

            // MessageBox.Show('FLabelUseDV.RowFilter: ' + FLabelUseDV.RowFilter);
            // MessageBox.Show('FLabelUseDV.Count: ' + FLabelUseDV.Count.ToString);
            GroupHeaderCount = CalculateGroupHeaderCount();

            // Set up Grid focus style
            // FLocalDataLabelValuesGrid.Selection.FocusStyle := SourceGrid.FocusStyle.RemoveSelectionOnLeave;
            // Set up Grid size
            FLocalDataLabelValuesGrid.Redim((FLabelUseDV.Count) + GroupHeaderCount, 3);

            // Initialize Size of Helper Object GridRowInfo (mapping between data and grid rows)
            FGridRowInfo = new TGridRowInfo();
            FGridRowInfo.Initialize(FLabelUseDV.Count + GroupHeaderCount);
            TitleModel = new SourceGrid.Cells.Views.Cell();
            TitleModel.BackColor = Color.SteelBlue;
            TitleModel.ForeColor = Color.White;
            TitleModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            LabelModel = new SourceGrid.Cells.Views.Cell();
            LabelModel.BackColor = FLocalDataLabelValuesGrid.BackColor;
            LabelModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            CurrentRow = 0;

            for (Counter = 0; Counter <= FLabelUseDV.Count - 1; Counter += 1)
            {
                // Find the corresponding DataLabel Row
                DataLabelRow = (PDataLabelRow)FLocalDataDS.DataLabelList.Rows.Find(FLabelUseDV[Counter][PDataLabelUseTable.GetDataLabelKeyDBName()]);
                CurrentGroup = DataLabelRow.Group;

                if (CurrentGroup != LastGroup)
                {
                    // Create group header if Group changed
                    FLocalDataLabelValuesGrid.Rows.SetHeight(CurrentRow, 23);
                    FLocalDataLabelValuesGrid[CurrentRow, 0] = new SourceGrid.Cells.Cell(CurrentGroup);
                    FLocalDataLabelValuesGrid[CurrentRow, 0].View = TitleModel;
                    FLocalDataLabelValuesGrid[CurrentRow, 0].ColumnSpan = 3;
                    FLocalDataLabelValuesGrid[CurrentRow, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    CurrentRow = CurrentRow + 1;
                }

                // Create label (left of the field)
                FLocalDataLabelValuesGrid.Rows.SetHeight(CurrentRow, 23);
                FLocalDataLabelValuesGrid[CurrentRow, 0] = new SourceGrid.Cells.Cell(DataLabelRow.Text + ':');
                FLocalDataLabelValuesGrid[CurrentRow, 0].View = LabelModel;
                FLocalDataLabelValuesGrid[CurrentRow, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

                // MessageBox.Show('Counter: ' + Counter.ToString);
                // MessageBox.Show('FPartnerKey: ' + FPartnerKey.ToString + '; FLocalDataDS.DataLabelList.Row[Counter].Key: ' + FLocalDataDS.DataLabelList.Row[Counter].Key.ToString);
                // set up the value cell(s) for the current row
                SetupGridValueCell(CurrentRow, DataLabelRow);

                // Some TODOs
                // Check for the p_editable_l flag and make column 1 readonly if true
                // should go like this: FLocalDataLabelValuesGrid[CurrentRow, 1].Editor.EnableEdit := false;
                // Check the value of p_char_length_i and limit the length of the editable value
                // see commentedout line above 'TextBoxEditor.Control.MaxLength := '
                // Check the value of p_num_decimal_places_i build the corresponding format for the cell
                // see 'Editor' section on webpage
                // Implement currency (take p_currency_code_c into consideration), boolean and PartnerKey types
                // Implement cmbAutoComplete ComboBox (in Ict_Common_Controls.dll) for Labels where p_lookup_category_code_c is set
                // see file AutoTranslatedCSExampleWinForm, lines 465470 for a basic example of how to host an arbitraty control in a Cell of the Grid
                // MessageBox.Show('CurrentRow: ' + CurrentRow.ToString);
                // Hook up Grid Cell Enter event notification (for displaying Help text in the Status Bar)
                FLocalDataLabelValuesGrid[CurrentRow, 1].AddController(FEnterNotificationController);

                // Store the mapping of the current Grid Row to the PDataLabel Row that holds the Cell's Label information
                FGridRowInfo.SetDataRowKey(CurrentRow, (int)DataLabelRow.Key);
                CurrentRow = CurrentRow + 1;
                LastGroup = CurrentGroup;
            }

            // FLocalDataLabelValuesGrid.Columns[0].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            // FLocalDataLabelValuesGrid.Columns[1].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            // FLocalDataLabelValuesGrid.Columns[2].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            FLocalDataLabelValuesGrid.Columns[0].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);
            FLocalDataLabelValuesGrid.Columns[1].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);
            FLocalDataLabelValuesGrid.Columns[2].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);

            // FLocalDataLabelValuesGrid.AutoSize;
            FLocalDataLabelValuesGrid.AutoStretchColumnsToFitWidth = true;
            FLocalDataLabelValuesGrid.Columns.StretchToFit();
            ApplySecurity();
            FGridIsSetUp = true;

            // set width for controls (comboboxes and buttons)
            ActUponGridSizeChanged();
        }

        /// <summary>
        /// React on a resized grid
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ActUponGridSizeChanged()
        {
            if (FGridIsSetUp)
            {
                // set width for controls (comboboxes and buttons)
                FGridRowInfo.SetControlWidth(FLocalDataLabelValuesGrid.Columns.GetWidth(0), FLocalDataLabelValuesGrid.Columns.GetWidth(
                        1), FLocalDataLabelValuesGrid.Columns.GetWidth(2));
            }
        }

        private Int32 CalculateGroupHeaderCount()
        {
            Int32 GroupHeaderCount;
            String CurrentGroup;
            String LastGroup = "";

            GroupHeaderCount = 0;

            for (Int32 Counter = 0; Counter <= FLabelUseDV.Count - 1; Counter += 1)
            {
                CurrentGroup = FLabelUseDV[Counter]["Parent_" + PDataLabelTable.GetGroupDBName()].ToString();

                if (CurrentGroup != LastGroup)
                {
                    GroupHeaderCount = GroupHeaderCount + 1;
                }

                LastGroup = CurrentGroup;
            }

            // MessageBox.Show('GroupHeaderCount: ' + GroupHeaderCount.ToString);
            return GroupHeaderCount;
        }

        /// <summary>
        ///
        /// </summary>
        public void GetDataFromControlsManual()
        {
            Int32 row;
            Int32 col;

            // iterate through each cell in the grid
            // and call ApplyCellValue

            for (row = 0; row <= FLocalDataLabelValuesGrid.RowsCount - 1; row += 1)
            {
                for (col = 0; col <= (FLocalDataLabelValuesGrid.Rows[row].Range.ColumnsCount - 1); col += 1)
                {
                    try
                    {
                        FEnterNotificationController.FGridControl.ApplyCellValue(row, col);
                    }
                    catch (Exception)
                    {
                        // not all cells like this being called
                        // so ignore any errors
                    }
                }
            }
        }

        /// <summary>
        /// Apply value in specified cell to underlying data structure
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean ApplyCellValue(int ARow, int AColumn)
        {
            Boolean ReturnValue;

            System.Windows.Forms.Control CurrentControl;
            PDataLabelRow DataLabelRow;
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;
            Boolean CellValueIsNull = false;
            ReturnValue = true;

            if (AColumn == 1)
            {
                // find the datarow in the label table to find out about the data type
                DataLabelRow = (PDataLabelRow)FLocalDataDS.DataLabelList.Rows.Find(FGridRowInfo.GetDataRowKey(ARow));

                if (DataLabelRow == null)
                {
                    // we can not continue if there is no label row
                    return false;
                }

                // check if cell value is null
                CellValueIsNull = IsCellValueNull(DataLabelRow, ARow, AColumn);

                // Get the data label value row to save the value in. (this time only if
                // the row is already existing)
                GetOrCreateDataLabelValueRow(false, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

                // remove data value row if it exists but the cell value is null so the row is not needed any longer
                if (!InUseForApplication())
                {
                    if (CellValueIsNull)
                    {
                        if (DataLabelValuePartnerRow != null)
                        {
                            DataLabelValuePartnerRow.Delete();
                        }

                        return true;
                    }
                }
                else
                {
                    if (CellValueIsNull)
                    {
                        if (DataLabelValueApplicationRow != null)
                        {
                            DataLabelValueApplicationRow.Delete();
                        }

                        return true;
                    }
                }

                // now we definitely have a cell value, so we need to create a row if we don't have one yet
                GetOrCreateDataLabelValueRow(true, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

                // Apply cell value according to data type
                // apply character value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueChar = ((System.Windows.Forms.TextBox)CurrentControl).Text;
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueChar = ((System.Windows.Forms.TextBox)CurrentControl).Text;
                    }
                }

                // apply float value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueNum = ((TTxtNumericTextBox)CurrentControl).NumberValueDecimal.GetValueOrDefault();
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueNum = ((TTxtNumericTextBox)CurrentControl).NumberValueDecimal.GetValueOrDefault();
                    }
                }

                // apply date value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueDate = ((TtxtPetraDate)CurrentControl).Date;
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueDate = ((TtxtPetraDate)CurrentControl).Date;
                    }
                }

                // apply integer value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueInt = ((TTxtNumericTextBox)CurrentControl).NumberValueInt.GetValueOrDefault();
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueInt = ((TTxtNumericTextBox)CurrentControl).NumberValueInt.GetValueOrDefault();
                    }
                }

                // apply currency value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueCurrency = ((TTxtCurrencyTextBox)CurrentControl).NumberValueDecimal.GetValueOrDefault();
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueCurrency = ((TTxtCurrencyTextBox)CurrentControl).NumberValueDecimal.GetValueOrDefault();
                    }
                }

                // apply boolean value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueBool = ((System.Windows.Forms.CheckBox)CurrentControl).Checked;
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueBool = ((System.Windows.Forms.CheckBox)CurrentControl).Checked;
                    }
                }

                // apply partner key value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValuePartnerKey = Convert.ToInt64(((TtxtAutoPopulatedButtonLabel)CurrentControl).Text);
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValuePartnerKey = Convert.ToInt64(((TtxtAutoPopulatedButtonLabel)CurrentControl).Text);
                    }
                }

                // apply lookup value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP)
                {
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (DataLabelValuePartnerRow != null)
                    {
                        DataLabelValuePartnerRow.ValueLookup = ((TCmbAutoPopulated)CurrentControl).GetSelectedString();
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        DataLabelValueApplicationRow.ValueLookup = ((TCmbAutoPopulated)CurrentControl).GetSelectedString();
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns true if the value of the cell is null
        /// </summary>
        /// <param name="ADataLabelRow"></param>
        /// <param name="ARow"></param>
        /// <param name="AColumn"></param>
        /// <returns></returns>
        private Boolean IsCellValueNull(PDataLabelRow ADataLabelRow, int ARow, int AColumn)
        {
            Boolean ReturnValue = false;

            System.Windows.Forms.Control CurrentControl;

            switch (ADataLabelRow.DataType)
            {
                case MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR:

                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (((System.Windows.Forms.TextBox)CurrentControl).Text.Trim() == "")
                    {
                        ReturnValue = true;
                    }

                    break;

                case MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT:
                case MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER:
                case MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY:

                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
                    {
                        if (((TTxtCurrencyTextBox)CurrentControl).Text.Trim() == "")
                        {
                            ReturnValue = true;
                        }
                    }
                    else if (((TTxtNumericTextBox)CurrentControl).Text.Trim() == "")
                    {
                        ReturnValue = true;
                    }

                    break;

                case MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE:
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (((TtxtPetraDate)CurrentControl).Text.Trim() == "")
                    {
                        ReturnValue = true;
                    }

                    break;

                case MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN:
                    // can't determine at the moment if "unchecked" is set on purpose
                    ReturnValue = false;
                    break;

                case MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY:
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if ((((TtxtAutoPopulatedButtonLabel)CurrentControl).Text.Trim() == "")
                        || (Convert.ToInt64(((TtxtAutoPopulatedButtonLabel)CurrentControl).Text) == 0))
                    {
                        ReturnValue = true;
                    }

                    break;

                case MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP:
                    CurrentControl = (System.Windows.Forms.Control)((SourceGrid.Cells.Cell)FLocalDataLabelValuesGrid.GetCell(ARow, AColumn)).Tag;

                    if (((TCmbAutoPopulated)CurrentControl).GetSelectedString().Trim() == "")
                    {
                        ReturnValue = true;
                    }

                    break;

                default:
                    break;
            }

            return ReturnValue;
        }

        private void ApplySecurity()
        {
            String MissingTableAccessRight;
            int Counter;
            Control TmpControl;

            MissingTableAccessRight = "";

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PDataLabelValuePartnerTable.GetTableDBName()))
            {
                MissingTableAccessRight = "create";
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PDataLabelValuePartnerTable.GetTableDBName()))
            {
                MissingTableAccessRight = "modify";
            }
            else if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapDELETE, PDataLabelValuePartnerTable.GetTableDBName()))
            {
                MissingTableAccessRight = "delete";
            }

            if (MissingTableAccessRight != "")
            {
                /*
                 * Disable anything editable in Column 1 of the Grid
                 */

                // MessageBox.Show('FLocalDataLabelValuesGrid.RowsCount: ' + FLocalDataLabelValuesGrid.RowsCount.ToString);
                for (Counter = 0; Counter <= FLocalDataLabelValuesGrid.RowsCount - 1; Counter += 1)
                {
                    if (FLocalDataLabelValuesGrid[Counter, 1].Editor != null)
                    {
                        // MessageBox.Show('Disabled editing of Grid cell in Row # ' + Counter.ToString +'...');
                        // Cell has an Editor, therefore disable the editing there
                        FLocalDataLabelValuesGrid[Counter, 1].Editor.EnableEdit = false;
                    }

                    TmpControl = FGridRowInfo.GetControl(Counter);

                    if (TmpControl != null)
                    {
                        // MessageBox.Show('Disabled special control of Grid cell in Row # ' + Counter.ToString +'...');
                        // Disable special control to make sure it can't be edited
                        TmpControl.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// React upon data saved event
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DataSavedEventFired(Boolean ASuccess)
        {
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;
            Int32 Count;
            Int32 Counter;
            Control TmpControl;
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            PDataLabelRow DataLabelRow;

            TLogging.LogAtLevel(1, "Data Save Event Fired");

            if (FGridRowInfo != null)
            {
                // UserControl was properly initialised
                Count = FGridRowInfo.Count();

                for (Counter = 0; Counter <= Count - 1; Counter += 1)
                {
                    TmpControl = FGridRowInfo.GetControl(Counter);

                    if (TmpControl != null)
                    {
                        DataLabelRow = (PDataLabelRow)FLocalDataDS.DataLabelList.Rows.Find(FGridRowInfo.GetDataRowKey(Counter));

                        if (DataLabelRow != null)
                        {
                            // In this case the data value rows need to be recreated because they are needed for data binding
                            // in the controls.
                            GetOrCreateDataLabelValueRow(true, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

                            // Force the partner key editors to redraw since it is possibly displaying
                            // a new row now.
                            if (TmpControl.GetType() == typeof(TtxtAutoPopulatedButtonLabel))
                            {
                                PartnerKeyEditor = (TtxtAutoPopulatedButtonLabel)TmpControl;

                                // PartnerKeyEditor.Invalidate();
                                PartnerKeyEditor.ResetLabelText();

                                // TODO: why doesn't this deisplay the partners name again! TimH
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Needs to be called from the instantiator of this Class once it is
        /// done with using this class to ensure that no memory leak occurs!!!
        /// </summary>
        public void FreeStaticObjects()
        {
            FGridRowInfo = null;
            FLocalDataLabelValuesGrid = null;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TGridRowInfo
    {
        private Int32[] FDataRowsMappingArray;
        private Boolean[] FHeaderMappingArray;
        private System.Windows.Forms.Control[] FControlArray;

        #region TGridRowInfo

        /// <summary>
        /// constructor
        /// </summary>
        public TGridRowInfo() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ANumberOfRows"></param>
        public void Initialize(Int32 ANumberOfRows)
        {
            FDataRowsMappingArray = new Int32[ANumberOfRows];
            FHeaderMappingArray = new Boolean[ANumberOfRows];
            FControlArray = new Control[ANumberOfRows];
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <param name="ADataRowKey"></param>
        public void SetDataRowKey(Int32 AGridRowIndex, Int32 ADataRowKey)
        {
            try
            {
                FDataRowsMappingArray[AGridRowIndex] = ADataRowKey;
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <param name="AIsHeader"></param>
        public void SetHeader(Int32 AGridRowIndex, Boolean AIsHeader)
        {
            try
            {
                FHeaderMappingArray[AGridRowIndex] = AIsHeader;
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <param name="AControl"></param>
        public void SetControl(Int32 AGridRowIndex, System.Windows.Forms.Control AControl)
        {
            try
            {
                FControlArray[AGridRowIndex] = AControl;
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AColumn1Width"></param>
        /// <param name="AColumn2Width"></param>
        /// <param name="AColumn3Width"></param>
        public void SetControlWidth(Int32 AColumn1Width, Int32 AColumn2Width, Int32 AColumn3Width)
        {
            Int32 ArrayLength = FControlArray.Length;

            for (Int32 count = 0; count <= ArrayLength - 1; count += 1)
            {
                if (FControlArray != null)
                {
                    if (FControlArray[count] is TCmbAutoPopulated)
                    {
                        ((TCmbAutoPopulated)FControlArray[count]).ComboBoxWidth = AColumn2Width;
                    }
                    else if (FControlArray[count] is TtxtAutoPopulatedButtonLabel)
                    {
                        ((TtxtAutoPopulatedButtonLabel)FControlArray[count]).ButtonWidth = AColumn1Width - 4;
                        ((TtxtAutoPopulatedButtonLabel)FControlArray[count]).TextBoxWidth = AColumn2Width;
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            Int32 ReturnValue;

            ReturnValue = 0;

            if (FDataRowsMappingArray != null)
            {
                ReturnValue = FDataRowsMappingArray.Length;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <returns></returns>
        public Int32 GetDataRowKey(Int32 AGridRowIndex)
        {
            Int32 ReturnValue;

            try
            {
                ReturnValue = FDataRowsMappingArray[AGridRowIndex];
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
                ReturnValue = 0;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <returns></returns>
        public Boolean IsHeader(Int32 AGridRowIndex)
        {
            Boolean ReturnValue;

            try
            {
                ReturnValue = FHeaderMappingArray[AGridRowIndex];
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridRowIndex"></param>
        /// <returns></returns>
        public System.Windows.Forms.Control GetControl(Int32 AGridRowIndex)
        {
            System.Windows.Forms.Control ReturnValue;
            try
            {
                ReturnValue = FControlArray[AGridRowIndex];
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
                ReturnValue = null;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControl"></param>
        /// <returns></returns>
        public Int32 GetRow(System.Windows.Forms.Control AControl)
        {
            Int32 Count;
            Int32 ArrayLength;

            ArrayLength = FControlArray.Length;

            for (Count = 0; Count <= ArrayLength - 1; Count += 1)
            {
                if (FControlArray[Count] == AControl)
                {
                    return Count;
                }
            }

            return -1;
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TCellEventNotificationController : SourceGrid.Cells.Controllers.ControllerBase
    {
        /// <summary>todoComment</summary>
        public TUC_LocalDataLabelValues FGridControl;

        /// <summary>
        /// constructor
        /// </summary>
        public TCellEventNotificationController() : base()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AGridControl"></param>
        public void Initialize(TUC_LocalDataLabelValues AGridControl)
        {
            FGridControl = AGridControl;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        public override void OnFocusEntered(SourceGrid.CellContext ASender, EventArgs AEventArgs)
        {
            base.OnFocusEntered(ASender, AEventArgs);

            try
            {
                if (FGridControl.FGridRowInfo.GetControl(ASender.Position.Row) != null)
                {
                    if (!FGridControl.FInFocussingMode)
                    {
                        FGridControl.FInFocussingMode = true;
                        FGridControl.FGridRowInfo.GetControl(ASender.Position.Row).Focus();
                        FGridControl.FInFocussingMode = false;
                    }
                }

                FGridControl.ExecutePossiblySomethingToSave();
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="ACancelEventArgs"></param>
        public override void OnFocusLeaving(SourceGrid.CellContext ASender, CancelEventArgs ACancelEventArgs)
        {
        }
    }
}