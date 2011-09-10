//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using DevAge.ComponentModel;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using SourceGrid;
using SourceGrid.Cells.Controllers;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Client.App.Core;
using System.Drawing;
using DevAge.Drawing;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using SourceGrid.Cells;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TTellGuiToEnableSaveButton();

    /// <summary>
    /// Contains logic for the UC_DataLabels_OfficeSpecific UserControl.
    /// </summary>
    public class TUCOfficeSpecificDataLabelsLogic
    {
        private static OfficeSpecificDataLabelsTDS UMainDS;
        private static Int64 UPartnerKey;
        private static Int32 UApplicationKey;
        private static Int64 URegistrationOffice;
        private static SourceGrid.Grid UOfficeSpecificGrid;

//TODO        private static StatusBarTextProvider UStatusTextProvider;
        private static StatusBar UStatusBar;
        private static TOfficeSpecificDataLabelUseEnum UOfficeSpecificDataLabelUse;

        /// <summary>todoComment</summary>
        public static TGridRowInfo UGridRowInfo;

        /// <summary>todoComment</summary>
        public static Boolean UInFocussingMode;

        private DataView FLabelUseDV;
        private TCellEventNotificationController FEnterNotificationController;
        private PDataLabelTable FDataLabelDT;
        private PDataLabelUseTable FDataLabelUseDT;
        private PDataLabelLookupCategoryTable FDataLabelLookupCategoryDT;
        private PDataLabelLookupTable FDataLabelLookupDT;
        private PDataLabelValuePartnerTable FDataLabelValuePartnerDT;
        private PDataLabelValueApplicationTable FDataLabelValueApplicationDT;
        private Boolean FGridIsSetUp;

        /// <summary>todoComment</summary>
        public OfficeSpecificDataLabelsTDS MainDS
        {
            get
            {
                return UMainDS;
            }

            set
            {
                UMainDS = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int64 PartnerKey
        {
            get
            {
                return UPartnerKey;
            }

            set
            {
                UPartnerKey = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int32 ApplicationKey
        {
            get
            {
                return UApplicationKey;
            }

            set
            {
                UApplicationKey = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int64 RegistrationOffice
        {
            get
            {
                return URegistrationOffice;
            }

            set
            {
                URegistrationOffice = value;
            }
        }

        /// <summary>todoComment</summary>
        public TOfficeSpecificDataLabelUseEnum OfficeSpecificDataLabelUse
        {
            get
            {
                return UOfficeSpecificDataLabelUse;
            }

            set
            {
                UOfficeSpecificDataLabelUse = value;
            }
        }

        /// <summary>todoComment</summary>
        public SourceGrid.Grid OfficeSpecificGrid
        {
            get
            {
                return UOfficeSpecificGrid;
            }

            set
            {
                UOfficeSpecificGrid = value;
            }
        }

        /// <summary>todoComment</summary>
        public StatusBar UCStatusBar
        {
            get
            {
                return UStatusBar;
            }

            set
            {
                UStatusBar = value;
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
        /// test code
        /// </summary>
        /// <returns>void</returns>
        private void TestDataColumnChanging(System.Object sender, System.Data.DataColumnChangeEventArgs e)
        {
            // MessageBox.Show('Column: ' + e.Column.ColumnName.ToString);
            // MessageBox.Show('New Value: ' + e.ProposedValue.toString());
        }

        /// <summary>
        /// Initialises DataSets and Tables
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDataStructures(PDataLabelValuePartnerTable ADataTableValuePartner,
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
            UMainDS = new OfficeSpecificDataLabelsTDS("OfficeSpecificData");

            // Merge in cached Typed DataTables (should be done differently in the future when TypedDataSet.Tables.Add works fine)
            UMainDS.Merge(FDataLabelDT);
            UMainDS.Merge(FDataLabelUseDT);
            UMainDS.Merge(FDataLabelLookupDT);
            UMainDS.Merge(FDataLabelLookupCategoryDT);
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
            SourceGrid.EditableMode DefaultEditableMode;
            SourceGrid.Cells.Editors.TextBox TextBoxEditor;
            SourceGrid.Cells.Editors.TextBoxNumeric TextBoxNumericEditor;
            SourceGrid.Cells.Editors.TextBoxCurrency TextBoxCurrencyEditor;
            TCmbAutoPopulated LookupValueEditor;
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            CultureInfo CurrencyCultureInfo;
            CultureInfo NumericCultureInfo;
            NumberFormatInfo LocalNumberFormatInfo;
            SourceGrid.Cells.Views.Cell ValueModel;
            SourceGrid.Cells.Views.Cell SuffixModel;
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;

            // Set default values for the Grid
            // set attributes for default editable mode of a cell
            // (this mode must not be used with the check box, because otherwise the space
            // bar will not work to tick/untick the check box)
            DefaultEditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;

            // prepare model for the value cells
            ValueModel = new SourceGrid.Cells.Views.Cell();
            ValueModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            ValueModel.Font = new Font(UOfficeSpecificGrid.Font.FontFamily.Name, UOfficeSpecificGrid.Font.Size, FontStyle.Bold);

            // prepare model for suffix cells (e.g. for currency)
            SuffixModel = new SourceGrid.Cells.Views.Cell();
            SuffixModel.BackColor = UOfficeSpecificGrid.BackColor;
            SuffixModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            if ((ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY)
                || (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP))
            {
                // In this case the data value rows need to be created because they are needed for data binding
                // in the controls.
                GetOrCreateDataLabelValueRow(true, ADataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);
            }
            else
            {
                // In this case the data value rows will only be created once a value is entered
                GetOrCreateDataLabelValueRow(false, ADataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);
            }

            // Create field, according to specified data type
            // Create character field
            if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR)
            {
                TextBoxEditor = new SourceGrid.Cells.Editors.TextBox(typeof(String));

                // Limit length of entry field according to database setting
                TextBoxEditor.Control.MaxLength = ADataLabelRow.CharLength;

                if (DataLabelValuePartnerRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(DataLabelValuePartnerRow.ValueChar);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(DataLabelValueApplicationRow.ValueChar);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell("", typeof(String));
                }

                UOfficeSpecificGrid[ARowIndex, 1].Editor = TextBoxEditor;
                UOfficeSpecificGrid[ARowIndex, 1].View = ValueModel;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EditableMode = DefaultEditableMode;
            }
            // Create float field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT)
            {
                TextBoxNumericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));

                // prepare numeric editor (set number of decimal places according to label)
                NumericCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.Name);
                TextBoxNumericEditor.CultureInfo = NumericCultureInfo;

                // TextBoxNumericEditor.CultureInfo.NumberFormat.CurrentInfo.NumberDecimalDigits := ADataLabelRow.NumDecimalPlaces;
                LocalNumberFormatInfo = TextBoxNumericEditor.CultureInfo.NumberFormat;
                LocalNumberFormatInfo.NumberDecimalDigits = ADataLabelRow.NumDecimalPlaces;
                TextBoxNumericEditor.CultureInfo.NumberFormat = LocalNumberFormatInfo;

                if (DataLabelValuePartnerRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValuePartnerRow.ValueNum);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValueApplicationRow.ValueNum);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null);
                }

                UOfficeSpecificGrid[ARowIndex, 1].Editor = TextBoxNumericEditor;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.AllowNull = true;
                UOfficeSpecificGrid[ARowIndex, 1].View = ValueModel;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EditableMode = DefaultEditableMode;
            }
            // Create data field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    if (DataLabelValuePartnerRow.IsValueDateNull())
                    {
                        UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null, typeof(System.DateTime));
                    }
                    else
                    {
                        UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(DataLabelValuePartnerRow.ValueDate, typeof(System.DateTime));
                    }
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    if (DataLabelValueApplicationRow.IsValueDateNull())
                    {
                        UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null, typeof(System.DateTime));
                    }
                    else
                    {
                        UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(DataLabelValueApplicationRow.ValueDate, typeof(System.DateTime));
                    }
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null, typeof(System.DateTime));
                }

                UOfficeSpecificGrid[ARowIndex, 1].Editor.AllowNull = true;
                UOfficeSpecificGrid[ARowIndex, 1].View = ValueModel;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EditableMode = DefaultEditableMode;
            }
            // Create integer field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER)
            {
                TextBoxNumericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(System.Int32));

                if (DataLabelValuePartnerRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex,
                                        1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValuePartnerRow.ValueInt, typeof(System.Int32));
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex,
                                        1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValueApplicationRow.ValueInt, typeof(System.Int32));
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null, typeof(System.Int32));
                }

                UOfficeSpecificGrid[ARowIndex, 1].Editor = TextBoxNumericEditor;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.AllowNull = true;
                UOfficeSpecificGrid[ARowIndex, 1].View = ValueModel;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EditableMode = DefaultEditableMode;
            }
            // Create currency field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
            {
                TextBoxCurrencyEditor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(decimal));

                // prepare currency editor (suppress currency symbol since it is shown in cell behind amount cell)
                CurrencyCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.Name);
                TextBoxCurrencyEditor.CultureInfo = CurrencyCultureInfo;
                TextBoxCurrencyEditor.CultureInfo.NumberFormat.CurrencySymbol = "";

                if (DataLabelValuePartnerRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValuePartnerRow.ValueCurrency);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell((System.Object)DataLabelValueApplicationRow.ValueCurrency);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell(null);
                }

                UOfficeSpecificGrid[ARowIndex, 1].Editor = TextBoxCurrencyEditor;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.AllowNull = true;
                UOfficeSpecificGrid[ARowIndex, 1].View = ValueModel;
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EditableMode = DefaultEditableMode;

                // create the cell for the currency code
                UOfficeSpecificGrid[ARowIndex, 2] = new SourceGrid.Cells.Cell(ADataLabelRow.CurrencyCode);
                UOfficeSpecificGrid[ARowIndex, 2].View = SuffixModel;
                UOfficeSpecificGrid[ARowIndex, 2].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
            }
            // Create boolean field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN)
            {
                if (DataLabelValuePartnerRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.CheckBox("", DataLabelValuePartnerRow.ValueBool);
                }
                else if (DataLabelValueApplicationRow != null)
                {
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.CheckBox("", DataLabelValueApplicationRow.ValueBool);
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.CheckBox("", false);
                }

                UOfficeSpecificGrid[ARowIndex, 1].View = SourceGrid.Cells.Views.CheckBox.MiddleLeftAlign;
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

                // AutomaticallyUpdateDataSource: very rare, but needed here
                PartnerKeyEditor.AutomaticallyUpdateDataSource = true;
                PartnerKeyEditor.Enter += new EventHandler(this.UpdateGridFocusFromExternalControl);

                // perform data binding for user control depending on type etc.
                PerformDataBinding(ADataLabelRow, DataLabelValuePartnerRow, DataLabelValueApplicationRow, PartnerKeyEditor);

                UOfficeSpecificGrid[ARowIndex, 0] = new SourceGrid.Cells.Cell();
                UOfficeSpecificGrid.LinkedControls.Add(new LinkedControlValue(PartnerKeyEditor, new Position(ARowIndex, 0)));
                UOfficeSpecificGrid[ARowIndex, 0].Tag = PartnerKeyEditor;
                UOfficeSpecificGrid[ARowIndex, 0].ColumnSpan = 3;

                // UOfficeSpecificGrid[ARowIndex, 1].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                // remember the added control to get the value back lateron
                UGridRowInfo.SetControl(ARowIndex, PartnerKeyEditor);
            }
            // Create lookup field
            else if (ADataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP)
            {
                // Get the instance of the combobox (created in the actual user interface class)
                LookupValueEditor = new TCmbAutoPopulated();
                LookupValueEditor.Filter = PDataLabelLookupTable.GetCategoryCodeDBName() + " = '" + ADataLabelRow.LookupCategoryCode + "'";
                LookupValueEditor.ListTable = TCmbAutoPopulated.TListTableEnum.DataLabelLookupList;
                LookupValueEditor.InitialiseUserControl();
                LookupValueEditor.Enter += new EventHandler(this.UpdateGridFocusFromExternalControl);

                // perform data binding for user control depending on type etc.
                PerformDataBinding(ADataLabelRow, DataLabelValuePartnerRow, DataLabelValueApplicationRow, LookupValueEditor);
                LookupValueEditor.TabStop = false;

                UOfficeSpecificGrid[ARowIndex, 1] = new SourceGrid.Cells.Cell();
                UOfficeSpecificGrid.LinkedControls.Add(new LinkedControlValue((Control)LookupValueEditor, new Position(ARowIndex, 1)));
                UOfficeSpecificGrid[ARowIndex, 1].Tag = LookupValueEditor;
                UOfficeSpecificGrid[ARowIndex, 1].ColumnSpan = 2;

                // UOfficeSpecificGrid[ARowIndex, 1].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                // remember the added control to get the value back lateron
                UGridRowInfo.SetControl(ARowIndex, LookupValueEditor);
            }

            // check if value is editable
            if (!ADataLabelRow.Editable)
            {
                UOfficeSpecificGrid[ARowIndex, 1].Editor.EnableEdit = false;
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

            switch (UOfficeSpecificDataLabelUse)
            {
                case TOfficeSpecificDataLabelUseEnum.Person:
                case TOfficeSpecificDataLabelUseEnum.Family:
                case TOfficeSpecificDataLabelUseEnum.Church:
                case TOfficeSpecificDataLabelUseEnum.Organisation:
                case TOfficeSpecificDataLabelUseEnum.Unit:
                case TOfficeSpecificDataLabelUseEnum.Bank:
                case TOfficeSpecificDataLabelUseEnum.Venue:
                case TOfficeSpecificDataLabelUseEnum.Personnel:
                    ADataLabelValuePartnerRow = (PDataLabelValuePartnerRow)FDataLabelValuePartnerDT.Rows.Find(new Object[] { UPartnerKey,
                                                                                                                             ADataLabelRow.Key });

                    // if value record does not exist yet then create it here
                    if ((ADataLabelValuePartnerRow == null) && ACreateIfNotExisting)
                    {
                        ADataLabelValuePartnerRow = FDataLabelValuePartnerDT.NewRowTyped(true);
                        ADataLabelValuePartnerRow.PartnerKey = UPartnerKey;
                        ADataLabelValuePartnerRow.DataLabelKey = (int)ADataLabelRow.Key;
                        FDataLabelValuePartnerDT.Rows.Add(ADataLabelValuePartnerRow);
                    }

                    break;

                case TOfficeSpecificDataLabelUseEnum.LongTermApp:
                case TOfficeSpecificDataLabelUseEnum.ShortTermApp:
                    ADataLabelValueApplicationRow =
                        (PDataLabelValueApplicationRow)FDataLabelValueApplicationDT.Rows.Find(new Object[] { UPartnerKey, UApplicationKey,
                                                                                                             URegistrationOffice,
                                                                                                             ADataLabelRow.Key });

                    // if value record does not exist yet then create it here
                    if ((ADataLabelValueApplicationRow == null) && ACreateIfNotExisting)
                    {
                        ADataLabelValueApplicationRow = FDataLabelValueApplicationDT.NewRowTyped(true);
                        ADataLabelValueApplicationRow.PartnerKey = UPartnerKey;
                        ADataLabelValueApplicationRow.ApplicationKey = UApplicationKey;
                        ADataLabelValueApplicationRow.RegistrationOffice = URegistrationOffice;
                        ADataLabelValueApplicationRow.DataLabelKey = (int)ADataLabelRow.Key;
                        FDataLabelValueApplicationDT.Rows.Add(ADataLabelValueApplicationRow);
                    }

                    break;
            }
        }

        /// <summary>
        /// perform data binding for user control depending on type etc.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void PerformDataBinding(PDataLabelRow ADataLabelRow,
            PDataLabelValuePartnerRow ADataLabelValuePartnerRow,
            PDataLabelValueApplicationRow ADataLabelValueApplicationRow,
            UserControl AUserControl)
        {
            TCmbAutoPopulated LookupValueEditor;
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            String ValueRowFilter;
            DataView ValueRowDataView;

            if (AUserControl == null)
            {
                return;
            }

            if (AUserControl.GetType() == typeof(TtxtAutoPopulatedButtonLabel))
            {
                PartnerKeyEditor = (TtxtAutoPopulatedButtonLabel)AUserControl;

                if (ADataLabelValuePartnerRow != null)
                {
                    ValueRowFilter = PDataLabelValuePartnerTable.GetPartnerKeyDBName() + " = '" + PartnerKey.ToString() + "' AND " +
                                     PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + " = '" + ADataLabelRow.Key.ToString() + "'";
                    ValueRowDataView = new DataView(FDataLabelValuePartnerDT, ValueRowFilter, "", DataViewRowState.CurrentRows);
                    PartnerKeyEditor.PerformDataBinding(ValueRowDataView, PDataLabelValuePartnerTable.GetValuePartnerKeyDBName());

                    // test code
                    ValueRowDataView.Table.ColumnChanging += new DataColumnChangeEventHandler(this.@TestDataColumnChanging);

                    // end of test code
                }
                else if (ADataLabelValueApplicationRow != null)
                {
                    ValueRowFilter = PDataLabelValueApplicationTable.GetPartnerKeyDBName() + " = '" + PartnerKey.ToString() + "' AND " +
                                     PDataLabelValueApplicationTable.GetApplicationKeyDBName() + " = '" + ApplicationKey.ToString() + "' AND " +
                                     PDataLabelValueApplicationTable.GetRegistrationOfficeDBName() + " = '" + RegistrationOffice.ToString() +
                                     "' AND " +
                                     PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + " = '" + ADataLabelRow.Key.ToString() + "'";
                    ValueRowDataView = new DataView(FDataLabelValueApplicationDT, ValueRowFilter, "", DataViewRowState.CurrentRows);
                    PartnerKeyEditor.PerformDataBinding(ValueRowDataView, PDataLabelValueApplicationTable.GetValuePartnerKeyDBName());
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    PartnerKeyEditor.Text = StringHelper.PartnerKeyToStr(0);
                }
            }
            else if (AUserControl.GetType() == typeof(TCmbAutoPopulated))
            {
                LookupValueEditor = (TCmbAutoPopulated)AUserControl;

                if (ADataLabelValuePartnerRow != null)
                {
                    ValueRowFilter = PDataLabelValuePartnerTable.GetPartnerKeyDBName() + " = '" + PartnerKey.ToString() + "' AND " +
                                     PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + " = '" + ADataLabelRow.Key.ToString() + "'";
                    ValueRowDataView = new DataView(FDataLabelValuePartnerDT, ValueRowFilter, "", DataViewRowState.CurrentRows);
                    LookupValueEditor.PerformDataBinding(ValueRowDataView, PDataLabelValuePartnerTable.GetValueLookupDBName());
                }
                else if (ADataLabelValueApplicationRow != null)
                {
                    ValueRowFilter = PDataLabelValueApplicationTable.GetPartnerKeyDBName() + " = '" + PartnerKey.ToString() + "' AND " +
                                     PDataLabelValueApplicationTable.GetApplicationKeyDBName() + " = '" + ApplicationKey.ToString() + "' AND " +
                                     PDataLabelValueApplicationTable.GetRegistrationOfficeDBName() + " = '" + RegistrationOffice.ToString() +
                                     "' AND " +
                                     PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + " = '" + ADataLabelRow.Key.ToString() + "'";
                    ValueRowDataView = new DataView(FDataLabelValueApplicationDT, ValueRowFilter, "", DataViewRowState.CurrentRows);
                    LookupValueEditor.PerformDataBinding(ValueRowDataView, PDataLabelValueApplicationTable.GetValueLookupDBName());
                }
                else
                {
                    // Default value if no Label data exists for the Partner
                    LookupValueEditor.SelectedItem = "";
                }
            }
        }

        private void UpdateGridFocusFromExternalControl(System.Object sender, System.EventArgs e)
        {
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            TCmbAutoPopulated LookupValueEditor;
            Int32 Row;

            // exit;
            if (UInFocussingMode)
            {
                return;
            }

            // set mode to prevent recursive calls
            UInFocussingMode = true;

            if (sender.GetType() == typeof(TtxtAutoPopulatedButtonLabel))
            {
                PartnerKeyEditor = (TtxtAutoPopulatedButtonLabel)sender;
                Row = UGridRowInfo.GetRow(PartnerKeyEditor);
                UOfficeSpecificGrid.Selection.Focus(
                    new Position(UOfficeSpecificGrid[Row, 1].Row.Index, UOfficeSpecificGrid[Row, 1].Column.Index), true);
            }
            else if (sender.GetType() == typeof(TCmbAutoPopulated))
            {
                LookupValueEditor = (TCmbAutoPopulated)sender;
                Row = UGridRowInfo.GetRow(LookupValueEditor);
                UOfficeSpecificGrid.Selection.Focus(
                    new Position(UOfficeSpecificGrid[Row, 1].Row.Index, UOfficeSpecificGrid[Row, 1].Column.Index), true);
            }

            // reset mode
            UInFocussingMode = false;
        }

        private Boolean InUseForApplication()
        {
            Boolean ReturnValue;

            ReturnValue = false;

            // only return true if the control is used to display application values
            switch (UOfficeSpecificDataLabelUse)
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
        public void SetupGridColumnsAndRows()
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
            FLabelUseDV = new DataView(UMainDS.DataLabelUseList);

            // Create DataColumns that contain data from the DataLabel DataTable
            // add column: group of the data label
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PDataLabelTable.GetGroupDBName();
            ForeignTableColumn.Expression = "Parent." + PDataLabelTable.GetGroupDBName();
            UMainDS.DataLabelUseList.Columns.Add(ForeignTableColumn);

            // add column: is this label to be displayed
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.Boolean");
            ForeignTableColumn.ColumnName = "Parent_" + PDataLabelTable.GetDisplayedDBName();
            ForeignTableColumn.Expression = "Parent." + PDataLabelTable.GetDisplayedDBName();
            UMainDS.DataLabelUseList.Columns.Add(ForeignTableColumn);
            LabelUse = Enum.GetName(typeof(TOfficeSpecificDataLabelUseEnum), UOfficeSpecificDataLabelUse);

            // Sort by Index
            FLabelUseDV.Sort = PDataLabelUseTable.GetIdx1DBName() + " ASC";

            // Show only Labels that should get displayed and are of the type that this UserControl is for
            FLabelUseDV.RowFilter = PDataLabelUseTable.GetUseDBName() + " = '" + LabelUse + "' AND Parent_" + PDataLabelTable.GetDisplayedDBName() +
                                    " = 1";

            // MessageBox.Show('FLabelUseDV.RowFilter: ' + FLabelUseDV.RowFilter);
            // MessageBox.Show('FLabelUseDV.Count: ' + FLabelUseDV.Count.ToString);
            GroupHeaderCount = CalculateGroupHeaderCount();

            // Set up Grid focus style
            // UOfficeSpecificGrid.Selection.FocusStyle := SourceGrid.FocusStyle.RemoveSelectionOnLeave;
            // Set up Grid size
            UOfficeSpecificGrid.Redim((FLabelUseDV.Count + 1) + GroupHeaderCount, 3);

            // Initialize Size of Helper Object GridRowInfo (mapping between data and grid rows)
            UGridRowInfo = new TGridRowInfo();
            UGridRowInfo.Initialize(FLabelUseDV.Count + GroupHeaderCount);
            TitleModel = new SourceGrid.Cells.Views.Cell();
            TitleModel.BackColor = Color.SteelBlue;
            TitleModel.ForeColor = Color.White;
            TitleModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            LabelModel = new SourceGrid.Cells.Views.Cell();
            LabelModel.BackColor = UOfficeSpecificGrid.BackColor;
            LabelModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            CurrentRow = 0;

            for (Counter = 0; Counter <= FLabelUseDV.Count - 1; Counter += 1)
            {
                // Find the corresponding DataLabel Row
                DataLabelRow = (PDataLabelRow)UMainDS.DataLabelList.Rows.Find(FLabelUseDV[Counter][PDataLabelUseTable.GetDataLabelKeyDBName()]);
                CurrentGroup = DataLabelRow.Group;

                if (CurrentGroup != LastGroup)
                {
                    // Create group header if Group changed
                    UOfficeSpecificGrid.Rows.SetHeight(CurrentRow, 23);
                    UOfficeSpecificGrid[CurrentRow, 0] = new SourceGrid.Cells.Cell(CurrentGroup);
                    UOfficeSpecificGrid[CurrentRow, 0].View = TitleModel;
                    UOfficeSpecificGrid[CurrentRow, 0].ColumnSpan = 3;
                    UOfficeSpecificGrid[CurrentRow, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);
                    CurrentRow = CurrentRow + 1;
                }

                // Create label (left of the field)
                UOfficeSpecificGrid.Rows.SetHeight(CurrentRow, 23);
                UOfficeSpecificGrid[CurrentRow, 0] = new SourceGrid.Cells.Cell(DataLabelRow.Text + ':');
                UOfficeSpecificGrid[CurrentRow, 0].View = LabelModel;
                UOfficeSpecificGrid[CurrentRow, 0].AddController(SourceGrid.Cells.Controllers.Unselectable.Default);

                // MessageBox.Show('Counter: ' + Counter.ToString);
                // MessageBox.Show('UPartnerKey: ' + UPartnerKey.ToString + '; UMainDS.DataLabelList.Row[Counter].Key: ' + UMainDS.DataLabelList.Row[Counter].Key.ToString);
                // set up the value cell(s) for the current row
                SetupGridValueCell(CurrentRow, DataLabelRow);

                // Some TODOs
                // Check for the p_editable_l flag and make column 1 readonly if true
                // should go like this: UOfficeSpecificGrid[CurrentRow, 1].Editor.EnableEdit := false;
                // Check the value of p_char_length_i and limit the length of the editable value
                // see commentedout line above 'TextBoxEditor.Control.MaxLength := '
                // Check the value of p_num_decimal_places_i build the corresponding format for the cell
                // see 'Editor' section on webpage
                // Implement currency (take p_currency_code_c into consideration), boolean and PartnerKey types
                // Implement cmbAutoComplete ComboBox (in Ict_Common_Controls.dll) for Labels where p_lookup_category_code_c is set
                // see file AutoTranslatedCSExampleWinForm, lines 465470 for a basic example of how to host an arbitraty control in a Cell of the Grid
                // MessageBox.Show('CurrentRow: ' + CurrentRow.ToString);
                // Hook up Grid Cell Enter event notification (for displaying Help text in the Status Bar)
                UOfficeSpecificGrid[CurrentRow, 1].AddController(FEnterNotificationController);

                // Store the mapping of the current Grid Row to the PDataLabel Row that holds the Cell's Label information
                UGridRowInfo.SetDataRowKey(CurrentRow, (int)DataLabelRow.Key);
                CurrentRow = CurrentRow + 1;
                LastGroup = CurrentGroup;
            }

            // UOfficeSpecificGrid.Columns[0].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            // UOfficeSpecificGrid.Columns[1].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            // UOfficeSpecificGrid.Columns[2].AutoSizeMode := (SourceGrid.AutoSizeMode.EnableAutoSize);   or SourceGrid.AutoSizeMode.Default
            UOfficeSpecificGrid.Columns[0].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);
            UOfficeSpecificGrid.Columns[1].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);
            UOfficeSpecificGrid.Columns[2].AutoSizeMode = (SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default);

            // UOfficeSpecificGrid.AutoSize;
            UOfficeSpecificGrid.AutoStretchColumnsToFitWidth = true;
            UOfficeSpecificGrid.Columns.StretchToFit();
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
                UGridRowInfo.SetControlWidth(UOfficeSpecificGrid.Columns.GetWidth(0), UOfficeSpecificGrid.Columns.GetWidth(
                        1), UOfficeSpecificGrid.Columns.GetWidth(2));
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
        /// Apply value in specified cell to underlying data structure
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean ApplyCellValue(int ARow, int AColumn)
        {
            Boolean ReturnValue;

            System.Object CellValue;
            System.Windows.Forms.UserControl CurrentControl;
            PDataLabelRow DataLabelRow;
            PDataLabelValuePartnerRow DataLabelValuePartnerRow;
            PDataLabelValueApplicationRow DataLabelValueApplicationRow;
            ReturnValue = true;

            if (AColumn == 1)
            {
                CellValue = UOfficeSpecificGrid[ARow, AColumn].Value;

                // MessageBox.Show('CellValue: ' + CellValue.ToString());
                // find the datarow in the label table to find out about the data type
                DataLabelRow = (PDataLabelRow)UMainDS.DataLabelList.Rows.Find(UGridRowInfo.GetDataRowKey(ARow));

                if (DataLabelRow == null)
                {
                    // we can not continue if there is no label row
                    return false;
                }

                // Get the data label value row to save the value in (this time only if
                // the row is already existing)
                GetOrCreateDataLabelValueRow(false, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

                // if there is not already a row existing and if the value of the cell is
                // not set, then no row needs to be created (we can just return there)
                if (!this.InUseForApplication())
                {
                    if (DataLabelValuePartnerRow == null)
                    {
                        if (CellValue == null)
                        {
                            return true;
                        }
                        else
                        {
                            // if there is a value in the cell but no row created yet then get
                            // a row to store the value in
                            GetOrCreateDataLabelValueRow(true, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);
                        }
                    }
                }
                else
                {
                    if (DataLabelValueApplicationRow == null)
                    {
                        if (CellValue == null)
                        {
                            return true;
                        }
                        else
                        {
                            // if there is a value in the cell but no row created yet then get
                            // a row to store the value in
                            GetOrCreateDataLabelValueRow(true, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);
                        }
                    }
                }

                // Apply cell value according to data type
                // apply character value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CHAR)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueChar = CellValue.ToString();
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueCharNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueChar = CellValue.ToString();
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueCharNull();
                        }
                    }
                }

                // apply float value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_FLOAT)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueNum = (decimal)CellValue;
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueNumNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueNum = (decimal)CellValue;
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueNumNull();
                        }
                    }
                }

                // apply date value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_DATE)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueDate = (System.DateTime)CellValue;
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueDateNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueDate = (System.DateTime)CellValue;
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueDateNull();
                        }
                    }
                }

                // apply integer value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_INTEGER)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueInt = (Int32)CellValue;
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueIntNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueInt = (Int32)CellValue;
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueIntNull();
                        }
                    }
                }

                // apply currency value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_CURRENCY)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueCurrency = (decimal)CellValue;
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueCurrencyNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueCurrency = (decimal)CellValue;
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueCurrencyNull();
                        }
                    }
                }

                // apply boolean value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_BOOLEAN)
                {
                    if (DataLabelValuePartnerRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValuePartnerRow.ValueBool = (Boolean)CellValue;
                        }
                        else
                        {
                            DataLabelValuePartnerRow.SetValueBoolNull();
                        }
                    }
                    else if (DataLabelValueApplicationRow != null)
                    {
                        if (CellValue != null)
                        {
                            DataLabelValueApplicationRow.ValueBool = (Boolean)CellValue;
                        }
                        else
                        {
                            DataLabelValueApplicationRow.SetValueBoolNull();
                        }
                    }
                }

                // apply partner key value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_PARTNERKEY)
                {
                    // In theory, the control will update the datasource itself as it is databound.
                    // In practise, we can't trust the BindingManagerBase to actually do it
                    // So we find the control, and call ResetLabeltext.
                    // This in turn calls UpdateLabeltext in txtBtnLabel(Ict.Common.Controls)
                    // This ensures the new value makes it to the datasource!
                    CurrentControl = (System.Windows.Forms.UserControl)((SourceGrid.Cells.Cell)UOfficeSpecificGrid.GetCell(ARow, AColumn)).Tag;
                    ((TtxtAutoPopulatedButtonLabel)CurrentControl).ResetLabelText();
                }

                // apply lookup value
                if (DataLabelRow.DataType == MCommonConstants.OFFICESPECIFIC_DATATYPE_LOOKUP)
                {
                    CurrentControl = (System.Windows.Forms.UserControl)((SourceGrid.Cells.Cell)UOfficeSpecificGrid.GetCell(ARow, AColumn)).Tag;
                    ((TCmbAutoPopulated)CurrentControl).SaveValueNow();

                    // CurrentControl := (UOfficeSpecificGrid.GetCell(ARow, AColumn) as CellControl).Control as System.Windows.Forms.UserControl;
                }
            }

            return ReturnValue;
        }

        private void ApplySecurity()
        {
            String MissingTableAccessRight;
            int Counter;
            UserControl TmpUserControl;

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

                // MessageBox.Show('UOfficeSpecificGrid.RowsCount: ' + UOfficeSpecificGrid.RowsCount.ToString);
                for (Counter = 1; Counter <= UOfficeSpecificGrid.RowsCount - 2; Counter += 1)
                {
                    if (UOfficeSpecificGrid[Counter, 1].Editor != null)
                    {
                        // MessageBox.Show('Disabled editing of Grid cell in Row # ' + Counter.ToString +'...');
                        // Cell has an Editor, therefore disable the editing there
                        UOfficeSpecificGrid[Counter, 1].Editor.EnableEdit = false;
                    }

                    TmpUserControl = UGridRowInfo.GetControl(Counter);

                    if (TmpUserControl != null)
                    {
                        // MessageBox.Show('Disabled special control of Grid cell in Row # ' + Counter.ToString +'...');
                        // Disable special control to make sure it can't be edited
                        TmpUserControl.Enabled = false;
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
            UserControl TmpUserControl;
            TtxtAutoPopulatedButtonLabel PartnerKeyEditor;
            PDataLabelRow DataLabelRow;

#if DEBUGMODE
            TLogging.Log("Data Save Event Fired");
#endif

            if (UGridRowInfo != null)
            {
                // UserControl was properly initialised
                Count = UGridRowInfo.Count();

                for (Counter = 0; Counter <= Count - 1; Counter += 1)
                {
                    TmpUserControl = UGridRowInfo.GetControl(Counter);

                    if (TmpUserControl != null)
                    {
                        DataLabelRow = (PDataLabelRow)UMainDS.DataLabelList.Rows.Find(UGridRowInfo.GetDataRowKey(Counter));

                        if (DataLabelRow != null)
                        {
                            // In this case the data value rows need to be recreated because they are needed for data binding
                            // in the controls.
                            GetOrCreateDataLabelValueRow(true, DataLabelRow, out DataLabelValuePartnerRow, out DataLabelValueApplicationRow);

                            // Force the partner key editors to redraw since it is possibly displaying
                            // a new row now.
                            if (TmpUserControl.GetType() == typeof(TtxtAutoPopulatedButtonLabel))
                            {
                                PartnerKeyEditor = (TtxtAutoPopulatedButtonLabel)TmpUserControl;

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
        /// constructor
        /// </summary>
        public TUCOfficeSpecificDataLabelsLogic() : base()
        {
            // initialize members
            FGridIsSetUp = false;
        }

        /// <summary>
        /// Needs to be called from the instantiator of this Class once it is
        /// done with using this class to ensure that no memory leak occurs!!!
        /// </summary>
        public void FreeStaticObjects()
        {
            UGridRowInfo = null;
            UOfficeSpecificGrid = null;
        }

        /// <summary>
        /// This provides a means for the control host to tell the control to ensure the datasource is uptodate
        /// </summary>
        /// <returns>void</returns>
        public void SaveAllChanges()
        {
            Int32 row;
            Int32 col;
            CellContext scg;

            // THIS IS VITAL
            // OTHERWISE YOU DON'T GET THE CURRENTLY SELECTED CELLS VALUE

            scg = new CellContext(UOfficeSpecificGrid, UOfficeSpecificGrid.Selection.ActivePosition);
            scg.EndEdit(false);

            // iterate through each cell in the grid
            // and call ApplyCellValue

//          MessageBox.Show("TUCOfficeSpecificDataLabelsLogic calling SaveAllChanged");

            for (row = 0; row <= OfficeSpecificGrid.RowsCount - 1; row += 1)
            {
                for (col = 0; col <= (OfficeSpecificGrid.Rows[row].Range.ColumnsCount - 1); col += 1)
                {
                    try
                    {
//                      TLogging.Log("SAVEALLCHANGES: Row: " + row.ToString() + " Column: " + col.ToString());
                        FEnterNotificationController.FGridLogic.ApplyCellValue(row, col);
                    }
                    catch (Exception)
                    {
                        // not all cells like this being called
                        // so ignore any errors
                    }
                }
            }
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TGridRowInfo
    {
        private Int32[] FDataRowsMappingArray;
        private Boolean[] FHeaderMappingArray;
        private System.Windows.Forms.UserControl[] FControlArray;

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
            FControlArray = new UserControl[ANumberOfRows];
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
        public void SetControl(Int32 AGridRowIndex, System.Windows.Forms.UserControl AControl)
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
        public System.Windows.Forms.UserControl GetControl(Int32 AGridRowIndex)
        {
            System.Windows.Forms.UserControl ReturnValue;
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
        public Int32 GetRow(System.Windows.Forms.UserControl AControl)
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
        public TUCOfficeSpecificDataLabelsLogic FGridLogic;

        /// <summary>
        /// constructor
        /// </summary>
        public TCellEventNotificationController() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridLogic"></param>
        public void Initialize(TUCOfficeSpecificDataLabelsLogic AGridLogic)
        {
            FGridLogic = AGridLogic;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        public override void OnFocusEntered(SourceGrid.CellContext ASender, EventArgs AEventArgs)
        {
            base.OnFocusEntered(ASender, AEventArgs);

            // so that we can use the global variables via the properties
            TUCOfficeSpecificDataLabelsLogic logic = new TUCOfficeSpecificDataLabelsLogic();

            try
            {
                // find the datarow in the label table to get the help text
                PDataLabelRow DataLabelRow =
                    (PDataLabelRow)logic.MainDS.DataLabelList.Rows.Find(TUCOfficeSpecificDataLabelsLogic.UGridRowInfo.GetDataRowKey(ASender.Position.
                            Row));

                // set statusbar text for office specific data labels
                string HelpText = (DataLabelRow != null ? DataLabelRow.Description : "");
                // logic.UCStatusBarTextProvider.SetStatusBarText(logic.OfficeSpecificGrid, HelpText);
                // Note: using InstanceDefaultText is a kind of 'brutal' method, but I (ChristianK) couldn't find another solution to instantly show the HelpText in the StatusBar
                // logic.UCStatusBarTextProvider.InstanceDefaultText = HelpText;
                logic.UCStatusBar.Text = HelpText;

                if (TUCOfficeSpecificDataLabelsLogic.UGridRowInfo.GetControl(ASender.Position.Row) != null)
                {
                    if (!TUCOfficeSpecificDataLabelsLogic.UInFocussingMode)
                    {
                        TUCOfficeSpecificDataLabelsLogic.UInFocussingMode = true;
                        TUCOfficeSpecificDataLabelsLogic.UGridRowInfo.GetControl(ASender.Position.Row).Focus();
                        TUCOfficeSpecificDataLabelsLogic.UInFocussingMode = false;
                    }
                }

                FGridLogic.ExecutePossiblySomethingToSave();
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
            // so that we can use the global variables via the properties
            TUCOfficeSpecificDataLabelsLogic logic = new TUCOfficeSpecificDataLabelsLogic();

            CellContext scg;

            // THIS IS VITAL
            // OTHERWISE YOU DON'T GET THE CURRENTLY SELECTED CELLS VALUE
            scg = new CellContext(logic.OfficeSpecificGrid, logic.OfficeSpecificGrid.Selection.ActivePosition);
            scg.EndEdit(false);
            try
            {
                if (true)
                {
                    // inherited;
                    // TLogging.Log('OnFocusLeaving: Row:' + ASender.Position.Row.toString() + ' Column:' + ASender.Position.Column.toString());
                    // Applied :=
                    FGridLogic.ApplyCellValue(ASender.Position.Row, ASender.Position.Column);

                    // TLogging.Log('OnFocusLeaving: Result:' + Applied.toString());
                    // TODO:
                    // Check for p_entry_mandatory_l and set ACancelEventArgs.Cancel := true if nothing entered and give warning
                }

                /*
                 * else
                 * {
                 * ACancelEventArgs.Cancel = true;
                 * }
                 */
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.Message);
            }
        }
    }
}