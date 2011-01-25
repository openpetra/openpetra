//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, markusm, timop
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared.Interfaces; // Implicit references
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// A UserControl that consists of a ComboBox whose entries come from a DataTable
    /// whose contents can be coming from: Cacheable DataTables, Static DataTables, and
    /// DataTables which are loaded on demand.
    /// Next to the ComboBox sits a label that can display a text (eg. description of a
    /// code that is selected in the ComboBox).
    /// <para />
    /// The that should be displayed is selected with the ListTable property.
    /// The control fetches its list entries on its own from the source of the data that
    /// is hard-coded with each ListTable!
    /// </summary>
    public partial class TCmbAutoPopulated : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// Enumeration for the Designer. Holds the possible values for ListTable.
        /// No enum prefixes here since these values are shown in the Designer.
        /// </summary>
        public enum TListTableEnum
        {
            /// user defined list; calls InitializeUserControl(DataTable, ...) and AppearanceSetup(Int32[], Int32)
            UserDefinedList,

            /// <summary>todoComment</summary>
            AccommodationCodeList,

            /// <summary>todoComment</summary>
            AcquisitionCodeList,

            /// <summary>todoComment</summary>
            AddresseeTypeList,

            /// <summary>todoComment</summary>
            AddressDisplayOrderList,

            /// <summary>todoComment</summary>
            AddressLayoutList,

            /// <summary>for Finance module, Analysis Attributes</summary>
            AnalysisTypeList,

            /// <summary>todoComment</summary>
            BusinessCodeList,

            /// <summary>todoComment</summary>
            CountryList,

            /// <summary>todoComment</summary>
            CurrencyCodeList,

            /// <summary>todoComment</summary>
            DataLabelLookupList,

            /// <summary>todoComment</summary>
            DenominationList,

            /// <summary>todoComment</summary>
            DocumentTypeCategoryList,

            /// <summary>todoComment</summary>
            FrequencyList,

            /// <summary>todoComment</summary>
            GenderList,

            /// <summary>todoComment</summary>
            InterestList,

            /// <summary>todoComment</summary>
            InterestCategoryList,

            /// <summary>todoComment</summary>
            InternationalPostalTypeList,

            /// <summary>todoComment</summary>
            LanguageCodeList,

            /// <summary>todoComment</summary>
            LocationTypeList,

            /// <summary>todoComment</summary>
            MaritalStatusList,

            /// <summary>todoComment</summary>
            PartnerClassList,

            /// <summary>todoComment</summary>
            PartnerStatusList,

            /// <summary>todoComment</summary>
            FoundationOwnerList,

            /// <summary>todoComment</summary>
            ProposalStatusList,

            /// <summary>todoComment</summary>
            ProposalSubmissionTypeList,

            /// <summary>todoComment</summary>
            ProposalReviewFrequencyList,

            /// <summary>todoComment</summary>
            ProposalSubmitFrequencyList,

            /// <summary>todoComment</summary>
            PublicationList,

            /// <summary>todoComment</summary>
            ReasonSubscriptionCancelledList,

            /// <summary>todoComment</summary>
            ReasonSubscriptionGivenList,

            /// <summary>todoComment</summary>
            RelationList,

            /// <summary>todoComment</summary>
            RelationCategoryList,

            /// <summary>todoComment</summary>
            SubscriptionStatus,

            /// <summary>todoComment</summary>
            UnitTypeList
        };

        private DataTable FDataCache_ListTable = null;
        private DataView FDataView;
        private TListTableEnum FListTable;
        private Boolean FUserControlInitialised = false;
        private String FFilter;
        private Boolean FAddNotSetValue = false;
        private String FNotSetValue;
        private String FNotSetDisplay;

        /// this allows to set the table manually,
        /// when it cannot come from a cache because it depends on too many other parameters on the screen
        public DataTable Table
        {
            set
            {
                FDataCache_ListTable = value;
            }
        }

        /// <summary>todoComment</summary>
        public System.Object SelectedItem
        {
            get
            {
                return cmbAutoPopulated.cmbCombobox.SelectedItem;
            }

            set
            {
                cmbAutoPopulated.cmbCombobox.SelectedItem = value;
            }
        }

        /// <summary>todoComment</summary>
        public System.Object SelectedValue
        {
            get
            {
                return cmbAutoPopulated.cmbCombobox.SelectedValue;
            }

            set
            {
                cmbAutoPopulated.cmbCombobox.SelectedValue = value;
            }
        }

        /// <summary>todoComment</summary>
        public int ComboBoxWidth
        {
            get
            {
                return cmbAutoPopulated.ComboBoxWidth;
            }

            set
            {
                cmbAutoPopulated.ComboBoxWidth = value;
            }
        }

        /// <summary>todoComment</summary>
        public String Filter
        {
            get
            {
                return FFilter;
            }

            set
            {
                FFilter = value;

                if (FUserControlInitialised)
                {
                    FDataView.RowFilter = FFilter;
                }
            }
        }

        /**
         * This property determines which cached DataTable should make up the list of
         * entries.
         */
        [Category("Behavior"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("Determines which cached DataTable should make up the list of entries.")]
        public TListTableEnum ListTable
        {
            get
            {
                return FListTable;
            }

            set
            {
                // MessageBox.Show('FListTable: ' + FListTable.ToString("G"));
                FListTable = value;
                AppearanceSetup(FListTable);
            }
        }

        /**
         * This Event is thrown when the internal ComboBox throws the SelectedValueChanged Event.
         */
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when the internal ComboBox throws the SelectedValueChanged Event.")]
        public event System.EventHandler SelectedValueChanged;

        private void CmbCombobox_SelectedValueChanged(System.Object sender, EventArgs e)
        {
            if (SelectedValueChanged != null)
            {
                SelectedValueChanged(this, e);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TCmbAutoPopulated()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String Get_SelectedText()
        {
            return this.cmbAutoPopulated.cmbCombobox.SelectedText;
        }

        /// <summary>
        /// set the string that should be selected;
        /// uses TCmbVersatile.SetSelectedString
        /// </summary>
        /// <param name="ASelectedString"></param>
        public bool SetSelectedString(string ASelectedString)
        {
            return this.cmbAutoPopulated.cmbCombobox.SetSelectedString(ASelectedString);
        }

        /// <summary>
        /// get the selected string
        /// uses TCmbVersatile.GetSelectedString
        /// </summary>
        public string GetSelectedString()
        {
            return this.cmbAutoPopulated.cmbCombobox.GetSelectedString();
        }

        /// <summary>
        /// Selects an item with the given Int32 value in the first column. Selects first element if the Int32 value is not existing.
        /// uses TCmbVersatile.SetSelectedInt32
        /// </summary>
        /// <param name="ANr"></param>
        public void SetSelectedInt32(System.Int32 ANr)
        {
            this.cmbAutoPopulated.cmbCombobox.SetSelectedInt32(ANr);
        }

        /// <summary>
        /// gets the Int32 value of the selected item, first column
        /// uses TCmbVersatile.GetSelectedInt32
        /// </summary>
        public Int32 GetSelectedInt32()
        {
            return this.cmbAutoPopulated.cmbCombobox.GetSelectedInt32();
        }

        /// <summary>
        /// initialise user controls for specific tables
        /// it might be better to do this in other functions, see also Client/lib/MFinance/gui/FinanceComboboxes.cs
        /// </summary>
        public void InitialiseUserControl()
        {
            Ict.Common.Data.TTypedDataTable TypedTable;

            if (DesignMode)
            {
                return;
            }

            switch (FListTable)
            {
                case TListTableEnum.AccommodationCodeList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.AccommodationCodeList),
                    "AccommodationCode",
                    null,
                    null);
                    break;

                case TListTableEnum.AcquisitionCodeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AcquisitionCodeList),
                    "p_acquisition_code_c",
                    "p_acquisition_description_c",
                    null);
                    break;

                case TListTableEnum.AddresseeTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AddresseeTypeList),
                    "p_addressee_type_code_c",
                    "p_description_c",
                    null);
                    break;

                case TListTableEnum.AddressDisplayOrderList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.AddressDisplayOrderList),
                    "AddressDisplayOrder",
                    "Description",
                    null);
                    break;


                case TListTableEnum.AddressLayoutList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.AddressLayoutList),
                    "AddressLayout",
                    null,
                    null);
                    break;

                case TListTableEnum.AnalysisTypeList:

                    InitialiseUserControl(
                    TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AnalysisTypeList),
                    AAnalysisTypeTable.GetAnalysisTypeCodeDBName(),
                    AAnalysisTypeTable.GetAnalysisTypeDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.BusinessCodeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.BusinessCodeList),
                    "p_business_code_c",
                    "p_business_description_c",
                    null);
                    break;

                case TListTableEnum.CountryList:

                    InitialiseUserControl(
                    TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CountryList),
                    PCountryTable.GetCountryCodeDBName(),
                    PCountryTable.GetCountryNameDBName(),
                    null
                    );

                    //"#VALUE#, " + PCountryTable.GetCountryNameDBName()
                    break;

                case TListTableEnum.CurrencyCodeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.CurrencyCodeList),
                    ACurrencyTable.GetCurrencyCodeDBName(),
                    ACurrencyTable.GetCurrencyNameDBName(),
                    null);
                    break;

                case TListTableEnum.DenominationList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DenominationList),
                    "p_denomination_code_c",
                    "p_denomination_name_c",
                    null);
                    break;

                case TListTableEnum.DocumentTypeCategoryList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.DocumentTypeCategoryList),
                    "pm_code_c",
                    "pm_description_c",
                    null);
                    break;

                case TListTableEnum.FoundationOwnerList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.FoundationOwnerList),
                    "s_user_id_c",
                    "p_partner_key_n",
                    null,
                    null);
                    break;

                case TListTableEnum.FrequencyList:

                    InitialiseUserControl(
                    TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.FrequencyList),
                    "a_frequency_code_c",
                    "a_frequency_description_c",
                    null);
                    break;

                case TListTableEnum.GenderList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.GenderList),
                    "Gender",
                    null,
                    null);
                    break;

                case TListTableEnum.InterestList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestList),
                    PInterestTable.GetInterestDBName(),
                    PInterestTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.InterestCategoryList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestCategoryList),
                    PInterestCategoryTable.GetCategoryDBName(),
                    PInterestCategoryTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.InternationalPostalTypeList:
                    TRemote.MCommon.DataReader.GetData(PInternationalPostalTypeTable.GetTableDBName(), null, out TypedTable);

                    InitialiseUserControl(
                    TypedTable,
                    PInternationalPostalTypeTable.GetInternatPostalTypeCodeDBName(),
                    PInternationalPostalTypeTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.LanguageCodeList:

                    InitialiseUserControl(
                    TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.LanguageCodeList),
                    "p_language_code_c",
                    "p_language_description_c",
                    null);
                    break;

                case TListTableEnum.LocationTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LocationTypeList),
                    PLocationTypeTable.GetCodeDBName(),
                    null,
                    null);
                    break;

                case TListTableEnum.MaritalStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.MaritalStatusList),
                    "pt_code_c",
                    "pt_description_c",
                    null);
                    break;


                case TListTableEnum.PartnerClassList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.PartnerClassList),
                    "PartnerClass",
                    null,
                    null);
                    break;

                case TListTableEnum.PartnerStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerStatusList),
                    "p_status_code_c",
                    null,
                    null);
                    break;

                case TListTableEnum.ProposalStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ProposalStatusList),
                    "p_status_code_c",
                    "p_status_description_c",
                    null);
                    break;

                case TListTableEnum.ProposalSubmissionTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ProposalSubmissionTypeList),
                    "p_submission_type_code_c",
                    "p_submission_type_description_c",
                    null);
                    break;

                case TListTableEnum.ProposalReviewFrequencyList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.ProposalReviewFrequency),
                    "ProposalReviewFrequency",
                    null,
                    null);
                    break;

                case TListTableEnum.ProposalSubmitFrequencyList:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.ProposalSubmitFrequency),
                    "ProposalSubmitFrequency",
                    null,
                    null);
                    break;

                case TListTableEnum.ReasonSubscriptionCancelledList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.ReasonSubscriptionCancelledList),
                    "p_code_c",
                    "p_description_c",
                    null);
                    break;

                case TListTableEnum.ReasonSubscriptionGivenList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.ReasonSubscriptionGivenList),
                    "p_code_c",
                    "p_description_c",
                    null);
                    break;

                case TListTableEnum.PublicationList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList),
                    PPublicationTable.GetPublicationCodeDBName(),
                    PPublicationTable.GetPublicationDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.SubscriptionStatus:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.SubscriptionStatus),
                    "SubscriptionStatus",
                    null,
                    null);
                    break;

                case TListTableEnum.RelationList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList),
                    PRelationTable.GetRelationNameDBName(),
                    PRelationTable.GetRelationDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.RelationCategoryList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationCategoryList),
                    PRelationCategoryTable.GetCodeDBName(),
                    PRelationCategoryTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.UnitTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.UnitTypeList),
                    UUnitTypeTable.GetUnitTypeCodeDBName(),
                    UUnitTypeTable.GetUnitTypeNameDBName(),
                    null);
                    break;

                case TListTableEnum.DataLabelLookupList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelLookupList),
                    PDataLabelLookupTable.GetValueCodeDBName(),
                    PDataLabelLookupTable.GetValueDescDBName(),
                    null);
                    break;
            }
        }

        /// <summary>
        /// generic function for initialising the user control
        /// does not depend on table implementations
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="AValueDBName">name of the column in the table that has the name</param>
        /// <param name="ADisplayDBName"></param>
        /// <param name="ADescDBName">name of the column in the table that has the description; can be empty</param>
        /// <param name="AColumnsToSearch"></param>
        public void InitialiseUserControl(DataTable ATable, string AValueDBName, string ADisplayDBName, string ADescDBName, string AColumnsToSearch)
        {
            FDataCache_ListTable = ATable;

            // Pass on any set Tag
            cmbAutoPopulated.Tag = this.Tag;
            cmbAutoPopulated.cmbCombobox.Tag = this.Tag;
            this.cmbAutoPopulated.cmbCombobox.SelectedValueChanged += new System.EventHandler(this.CmbCombobox_SelectedValueChanged);

            if (FAddNotSetValue)
            {
                DataRow Dr = FDataCache_ListTable.NewRow();
                Dr[AValueDBName] = FNotSetValue;
                Dr[ADisplayDBName] = FNotSetDisplay;
                FDataCache_ListTable.Rows.InsertAt(Dr, 0);
            }

            string DescriptionDBName = (ADescDBName != null && ADescDBName.Length > 0) ? ADescDBName : null;

            cmbAutoPopulated.LabelDisplaysColumn = DescriptionDBName;

            cmbAutoPopulated.cmbCombobox.BeginUpdate();
            FDataView = new DataView(FDataCache_ListTable);
            FDataView.RowFilter = FFilter;
            FDataView.Sort = ADisplayDBName;
            cmbAutoPopulated.cmbCombobox.DisplayMember = ADisplayDBName;
            cmbAutoPopulated.cmbCombobox.ValueMember = AValueDBName;
            cmbAutoPopulated.cmbCombobox.DisplayInColumn1 = ADisplayDBName;
            cmbAutoPopulated.cmbCombobox.DisplayInColumn2 = DescriptionDBName;
            cmbAutoPopulated.cmbCombobox.DisplayInColumn3 = null;
            cmbAutoPopulated.cmbCombobox.DisplayInColumn4 = null;
            cmbAutoPopulated.cmbCombobox.ColumnsToSearch = AColumnsToSearch;
            cmbAutoPopulated.cmbCombobox.DataSource = FDataView;
            cmbAutoPopulated.cmbCombobox.EndUpdate();
            cmbAutoPopulated.cmbCombobox.Name = this.Name + "_internal_ComboBox";
            cmbAutoPopulated.cmbCombobox.SuppressSelectionColor = true;
            cmbAutoPopulated.cmbCombobox.SelectedItem = null;

            FUserControlInitialised = true;
        }

        /// <summary>
        /// overload
        /// assume that display is equals value
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="AValueDBName">name of the column in the table that has the name</param>
        /// <param name="ADescDBName">name of the column in the table that has the description; can be empty</param>
        /// <param name="AColumnsToSearch"></param>
        public void InitialiseUserControl(DataTable ATable, string AValueDBName, string ADescDBName, string AColumnsToSearch)
        {
            InitialiseUserControl(ATable, AValueDBName, AValueDBName, ADescDBName, AColumnsToSearch);
        }

        /// <summary>
        /// quick general way of setting the appearance of the combobox
        /// assumption: the width of the combobox is equal the width of the first column in the list
        /// assumption: if a value is not greater than 0, the default values are used
        /// assumption: images are not being used when this function is called
        /// </summary>
        /// <param name="AColumnWidth"></param>
        /// <param name="AMaxDropDownItems"></param>
        public void AppearanceSetup(Int32[] AColumnWidth, Int32 AMaxDropDownItems)
        {
            cmbAutoPopulated.ComboBoxWidth = 100;
            cmbAutoPopulated.ColumnWidthCol1 = 100;
            cmbAutoPopulated.ColumnWidthCol2 = 0;
            cmbAutoPopulated.ColumnWidthCol3 = 0;
            cmbAutoPopulated.ColumnWidthCol4 = 0;
            cmbAutoPopulated.ImageColumn = 0;
            cmbAutoPopulated.Images = null;

            for (Int32 Counter = 0; Counter < AColumnWidth.Length; Counter++)
            {
                if (AColumnWidth[Counter] > 0)
                {
                    switch (Counter)
                    {
                        case 0:
                            cmbAutoPopulated.ComboBoxWidth = AColumnWidth[Counter];
                            cmbAutoPopulated.ColumnWidthCol1 = AColumnWidth[Counter];
                            break;

                        case 1:
                            cmbAutoPopulated.ColumnWidthCol2 = AColumnWidth[Counter];
                            break;

                        case 2:
                            cmbAutoPopulated.ColumnWidthCol3 = AColumnWidth[Counter];
                            break;

                        case 3:
                            cmbAutoPopulated.ColumnWidthCol4 = AColumnWidth[Counter];
                            break;
                    }
                }
            }

            if (AMaxDropDownItems > 0)
            {
                cmbAutoPopulated.cmbCombobox.MaxDropDownItems = AMaxDropDownItems;
            }
        }

        /// it might be better to do this in other functions, see also Client/lib/MFinance/gui/FinanceComboboxes.cs
        private void AppearanceSetup(TListTableEnum AListTable)
        {
            cmbAutoPopulated.ComboBoxWidth = 0;
            cmbAutoPopulated.ColumnWidthCol1 = 100;
            cmbAutoPopulated.ColumnWidthCol2 = 0;
            cmbAutoPopulated.ColumnWidthCol3 = 0;
            cmbAutoPopulated.ColumnWidthCol4 = 0;
            cmbAutoPopulated.ImageColumn = 0;
            cmbAutoPopulated.Images = null;

            switch (AListTable)
            {
                case TListTableEnum.AcquisitionCodeList:
                    cmbAutoPopulated.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.AddresseeTypeList:
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.AddressDisplayOrderList:
                    cmbAutoPopulated.ColumnWidthCol1 = 50;
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    break;

                case TListTableEnum.BusinessCodeList:
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.CountryList:
                    cmbAutoPopulated.ColumnWidthCol1 = 50;
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.CurrencyCodeList:
                    cmbAutoPopulated.ColumnWidthCol1 = 60;
                    cmbAutoPopulated.ColumnWidthCol2 = 170;
                    break;

                case TListTableEnum.DenominationList:
                    cmbAutoPopulated.ColumnWidthCol2 = 330;
                    break;

                case TListTableEnum.DocumentTypeCategoryList:
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.FoundationOwnerList:
                    cmbAutoPopulated.ColumnWidthCol1 = 120;
                    break;

                case TListTableEnum.FrequencyList:
                    cmbAutoPopulated.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.GenderList:
                    cmbAutoPopulated.ColumnWidthCol1 = 88;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestList:
                    cmbAutoPopulated.ColumnWidthCol1 = 130;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestCategoryList:
                    cmbAutoPopulated.ColumnWidthCol1 = 130;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InternationalPostalTypeList:
                    cmbAutoPopulated.ColumnWidthCol1 = 100;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    break;

                case TListTableEnum.LanguageCodeList:
                    cmbAutoPopulated.ColumnWidthCol1 = 57;
                    cmbAutoPopulated.ColumnWidthCol2 = 130;
                    break;

                case TListTableEnum.LocationTypeList:
                    cmbAutoPopulated.ColumnWidthCol1 = 110;
                    break;

                case TListTableEnum.MaritalStatusList:
                    cmbAutoPopulated.ColumnWidthCol1 = 39;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 10;
                    break;

                case TListTableEnum.PartnerClassList:
                    cmbAutoPopulated.ColumnWidthCol1 = 130;
                    break;

                case TListTableEnum.PartnerStatusList:
                    cmbAutoPopulated.ColumnWidthCol1 = 95;
                    break;

                case TListTableEnum.ProposalSubmissionTypeList:
                    cmbAutoPopulated.ColumnWidthCol2 = 100;
                    break;

                case TListTableEnum.ReasonSubscriptionCancelledList:
                    cmbAutoPopulated.ColumnWidthCol1 = 110;
                    cmbAutoPopulated.ColumnWidthCol2 = 450;
                    break;

                case TListTableEnum.ReasonSubscriptionGivenList:
                    cmbAutoPopulated.ColumnWidthCol1 = 110;
                    cmbAutoPopulated.ColumnWidthCol2 = 450;
                    break;

                case TListTableEnum.PublicationList:
                    cmbAutoPopulated.ColumnWidthCol1 = 110;
                    cmbAutoPopulated.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.SubscriptionStatus:
                    cmbAutoPopulated.ColumnWidthCol1 = 110;
                    break;

                case TListTableEnum.RelationList:
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 15;
                    break;

                case TListTableEnum.RelationCategoryList:
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    break;

                case TListTableEnum.UnitTypeList:
                    cmbAutoPopulated.ColumnWidthCol1 = 90;
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.DataLabelLookupList:
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    break;
            }

            if (cmbAutoPopulated.ComboBoxWidth == 0)
            {
                cmbAutoPopulated.ComboBoxWidth = cmbAutoPopulated.ColumnWidthCol1;
            }

            if (DesignMode)
            {
                // Put text in ComboBox to make it easier to distinguish different AutoPopulatedComboBoxes on one Form
                cmbAutoPopulated.cmbCombobox.Text = AListTable.ToString("G");
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataSource"></param>
        /// <param name="AColumnName"></param>
        public void PerformDataBinding(System.ComponentModel.MarshalByValueComponent ADataSource, String AColumnName)
        {
            if (!FUserControlInitialised)
            {
                InitialiseUserControl();
            }

            // MessageBox.Show((ADataSource as DataTable).Rows.Count.ToString );
            // if FListTable = TListTableEnum.BusinessCodeList then
            // begin
            // MessageBox.Show((ADataSource as DataSet).Tables['POrganisation'].Rows[0]['p_business_code_c'].ToString);
            // end;
            cmbAutoPopulated.cmbCombobox.DataBindings.Add("SelectedValue", ADataSource, AColumnName);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="NotSetValue"></param>
        /// <param name="NotSetDisplay"></param>
        public void AddNotSetRow(String NotSetValue,
            String NotSetDisplay)
        {
            //store the special instructions
            this.FAddNotSetValue = true;
            this.FNotSetValue = NotSetValue;
            this.FNotSetDisplay = NotSetDisplay;
        }

        /// <summary>
        /// todoComment
        /// Also allow UNBOUND MODE
        /// </summary>
        public void PerformDataBinding()
        {
            //only call this once
            if (!FUserControlInitialised)
            {
                InitialiseUserControl();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SaveValueNow()
        {
            if ((cmbAutoPopulated.cmbCombobox.DataBindings.Count == 1) && (cmbAutoPopulated.cmbCombobox.DataBindings[0].BindingManagerBase != null))
            {
                cmbAutoPopulated.cmbCombobox.DataBindings[0].BindingManagerBase.EndCurrentEdit();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DropDown()
        {
            cmbAutoPopulated.cmbCombobox.DroppedDown = true;
        }

        /// pass through the SelectedIndex property from the combobox
        public Int32 SelectedIndex
        {
            get
            {
                return this.cmbAutoPopulated.cmbCombobox.SelectedIndex;
            }
            set
            {
                this.cmbAutoPopulated.cmbCombobox.SelectedIndex = value;
            }
        }

        /// the number of items in the combobox items list
        public Int32 Count
        {
            get
            {
                return cmbAutoPopulated.cmbCombobox.Items.Count;
            }
        }
    }
}