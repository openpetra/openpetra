/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, markusm
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// A UserControl that consists of a ComboBox with entries coming from a cached
    /// DataTable and a label to its right that can display a text.
    ///
    /// The control fetches its list entries on its own from the Client DataCache!
    /// The cached DataTable is selected with the ListTable property.
    /// </summary>
    public partial class TCmbAutoPopulated : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// Enumeration for the Designer. Holds the possible values for ListTable.
        /// No enum prefixes here since these values are shown in the Designer.
        /// </summary>
        public enum TListTableEnum
        {
            /// <summary>todoComment</summary>
            AccommodationCodeList,

            /// <summary>todoComment</summary>
            AcquisitionCodeList,

            /// <summary>todoComment</summary>
            AddresseeTypeList,

            /// <summary>todoComment</summary>
            AddressLayoutList,

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
            GenderList,

            /// <summary>todoComment</summary>
            InterestList,

            /// <summary>todoComment</summary>
            InterestCategoryList,

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
            SubscriptionStatus,

            /// <summary>todoComment</summary>
            UnitTypeList
        };

        private DataTable FDataCache_ListTable;
        private DataView FDataView;
        private TListTableEnum FListTable;
        private Boolean FUserControlInitialised;
        private String FFilter;
        private Boolean FAddNotSetValue = false;
        private String FNotSetValue;
        private String FNotSetDisplay;

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
        public event TSelectedValueChangedEventHandler SelectedValueChanged;

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
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            String TmpDisplayMember = "";
            String TmpValueMember = "";
            String TmpColumnsToSearch = "";
            String TmpDisplayInColumn1 = "";
            String TmpDisplayInColumn2 = "";
            String TmpDisplayInColumn3 = "";
            String TmpDisplayInColumn4 = "";

            TmpColumnsToSearch = "";

            if (!(DesignMode))
            {
                // Pass on any set Tag
                cmbAutoPopulated.Tag = this.Tag;
                cmbAutoPopulated.cmbCombobox.Tag = this.Tag;
                this.cmbAutoPopulated.cmbCombobox.SelectedValueChanged += new System.EventHandler(this.CmbCombobox_SelectedValueChanged);

                switch (FListTable)
                {
                    case TListTableEnum.AccommodationCodeList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.AccommodationCodeList);
                        TmpDisplayMember = "AccommodationCode";
                        TmpValueMember = "AccommodationCode";
                        TmpDisplayInColumn1 = "AccommodationCode";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.AcquisitionCodeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AcquisitionCodeList);
                        TmpDisplayMember = "p_acquisition_code_c";
                        TmpValueMember = "p_acquisition_code_c";
                        TmpDisplayInColumn1 = "p_acquisition_code_c";
                        TmpDisplayInColumn2 = "p_acquisition_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_acquisition_description_c";
                        break;

                    case TListTableEnum.AddresseeTypeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AddresseeTypeList);
                        TmpDisplayMember = "p_addressee_type_code_c";
                        TmpValueMember = "p_addressee_type_code_c";
                        TmpDisplayInColumn1 = "p_addressee_type_code_c";
                        TmpDisplayInColumn2 = "p_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.AddressLayoutList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.AddressLayoutList);
                        TmpDisplayMember = "AddressLayout";
                        TmpValueMember = "AddressLayout";
                        TmpDisplayInColumn1 = "AddressLayout";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.BusinessCodeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.BusinessCodeList);

                        // MessageBox.Show('BusinessCodeList Entries: ' + FDataCache_ListTable.Rows.Count.ToString);
                        TmpDisplayMember = "p_business_code_c";
                        TmpValueMember = "p_business_code_c";
                        TmpDisplayInColumn1 = "p_business_code_c";
                        TmpDisplayInColumn2 = "p_business_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.CountryList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.CountryList);
                        TmpDisplayMember = "p_country_code_c";
                        TmpValueMember = "p_country_code_c";
                        TmpColumnsToSearch = "#VALUE#, p_country_name_c";
                        TmpDisplayInColumn1 = "p_country_code_c";
                        TmpDisplayInColumn2 = "p_country_name_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_country_name_c";
                        break;

                    case TListTableEnum.CurrencyCodeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.CurrencyCodeList);
                        TmpDisplayMember = ACurrencyTable.GetCurrencyCodeDBName();
                        TmpValueMember = ACurrencyTable.GetCurrencyCodeDBName();
                        TmpDisplayInColumn1 = ACurrencyTable.GetCurrencyCodeDBName();
                        TmpDisplayInColumn2 = ACurrencyTable.GetCurrencyNameDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = ACurrencyTable.GetCurrencyNameDBName();
                        break;

                    case TListTableEnum.DenominationList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DenominationList);
                        TmpDisplayMember = "p_denomination_code_c";
                        TmpValueMember = "p_denomination_code_c";
                        TmpDisplayInColumn1 = "p_denomination_code_c";
                        TmpDisplayInColumn2 = "p_denomination_name_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_denomination_name_c";
                        break;

                    case TListTableEnum.FoundationOwnerList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.FoundationOwnerList);
                        TmpDisplayMember = "s_user_id_c";
                        TmpValueMember = "p_partner_key_n";
                        TmpDisplayInColumn1 = "s_user_id_c";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.GenderList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.GenderList);
                        TmpDisplayMember = "Gender";
                        TmpValueMember = "Gender";
                        TmpDisplayInColumn1 = "Gender";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.InterestList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestList);
                        TmpDisplayMember = PInterestTable.GetInterestDBName();
                        TmpValueMember = PInterestTable.GetInterestDBName();
                        TmpDisplayInColumn1 = PInterestTable.GetInterestDBName();
                        TmpDisplayInColumn2 = PInterestTable.GetDescriptionDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = PInterestTable.GetDescriptionDBName();
                        break;

                    case TListTableEnum.InterestCategoryList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestCategoryList);
                        TmpDisplayMember = PInterestCategoryTable.GetCategoryDBName();
                        TmpValueMember = PInterestCategoryTable.GetCategoryDBName();
                        TmpDisplayInColumn1 = PInterestCategoryTable.GetCategoryDBName();
                        TmpDisplayInColumn2 = PInterestCategoryTable.GetDescriptionDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = PInterestCategoryTable.GetDescriptionDBName();
                        break;

                    case TListTableEnum.LanguageCodeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LanguageCodeList);
                        TmpDisplayMember = "p_language_code_c";
                        TmpValueMember = "p_language_code_c";
                        TmpColumnsToSearch = "#VALUE#, p_language_description_c";
                        TmpDisplayInColumn1 = "p_language_code_c";
                        TmpDisplayInColumn2 = "p_language_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_language_description_c";
                        break;

                    case TListTableEnum.LocationTypeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LocationTypeList);
                        TmpDisplayMember = PLocationTypeTable.GetCodeDBName();
                        TmpValueMember = PLocationTypeTable.GetCodeDBName();
                        TmpDisplayInColumn1 = PLocationTypeTable.GetCodeDBName();
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.MaritalStatusList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.MaritalStatusList);
                        TmpDisplayMember = "pt_code_c";
                        TmpValueMember = "pt_code_c";
                        TmpDisplayInColumn1 = "pt_code_c";
                        TmpDisplayInColumn2 = "pt_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "pt_description_c";
                        break;

                    case TListTableEnum.PartnerClassList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.PartnerClassList);
                        TmpDisplayMember = "PartnerClass";
                        TmpValueMember = "PartnerClass";
                        TmpDisplayInColumn1 = "PartnerClass";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.PartnerStatusList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerStatusList);
                        TmpDisplayMember = "p_status_code_c";
                        TmpValueMember = "p_status_code_c";
                        TmpDisplayInColumn1 = "p_status_code_c";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.ProposalStatusList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ProposalStatusList);
                        TmpDisplayMember = "p_status_code_c";
                        TmpValueMember = "p_status_code_c";
                        TmpDisplayInColumn1 = "p_status_code_c";
                        TmpDisplayInColumn2 = "p_status_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_status_description_c";
                        break;

                    case TListTableEnum.ProposalSubmissionTypeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ProposalSubmissionTypeList);
                        TmpDisplayMember = "p_submission_type_code_c";
                        TmpValueMember = "p_submission_type_code_c";
                        TmpDisplayInColumn1 = "p_submission_type_code_c";
                        TmpDisplayInColumn2 = "p_submission_type_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_submission_type_description_c";
                        break;

                    case TListTableEnum.ProposalReviewFrequencyList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.ProposalReviewFrequency);
                        TmpDisplayMember = "ProposalReviewFrequency";
                        TmpValueMember = "ProposalReviewFrequency";
                        TmpDisplayInColumn1 = "ProposalReviewFrequency";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.ProposalSubmitFrequencyList:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.ProposalSubmitFrequency);
                        TmpDisplayMember = "ProposalSubmitFrequency";
                        TmpValueMember = "ProposalSubmitFrequency";
                        TmpDisplayInColumn1 = "ProposalSubmitFrequency";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.ReasonSubscriptionCancelledList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                        TCacheableSubscriptionsTablesEnum.ReasonSubscriptionCancelledList);
                        TmpDisplayMember = "p_code_c";
                        TmpValueMember = "p_code_c";
                        TmpDisplayInColumn1 = "p_code_c";
                        TmpDisplayInColumn2 = "p_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_description_c";
                        break;

                    case TListTableEnum.ReasonSubscriptionGivenList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                        TCacheableSubscriptionsTablesEnum.ReasonSubscriptionGivenList);
                        TmpDisplayMember = "p_code_c";
                        TmpValueMember = "p_code_c";
                        TmpDisplayInColumn1 = "p_code_c";
                        TmpDisplayInColumn2 = "p_description_c";
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = "p_description_c";
                        break;

                    case TListTableEnum.PublicationList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
                        TmpDisplayMember = PPublicationTable.GetPublicationCodeDBName();
                        TmpValueMember = PPublicationTable.GetPublicationCodeDBName();
                        TmpDisplayInColumn1 = PPublicationTable.GetPublicationCodeDBName();
                        TmpDisplayInColumn2 = PPublicationTable.GetPublicationDescriptionDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = PPublicationTable.GetPublicationDescriptionDBName();
                        break;

                    case TListTableEnum.SubscriptionStatus:

                        // Setup Data
                        FDataCache_ListTable = TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.SubscriptionStatus);
                        TmpDisplayMember = "SubscriptionStatus";
                        TmpValueMember = "SubscriptionStatus";
                        TmpDisplayInColumn1 = "SubscriptionStatus";
                        TmpDisplayInColumn2 = null;
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = null;
                        break;

                    case TListTableEnum.UnitTypeList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.UnitTypeList);
                        TmpDisplayMember = UUnitTypeTable.GetUnitTypeCodeDBName();
                        TmpValueMember = UUnitTypeTable.GetUnitTypeCodeDBName();
                        TmpDisplayInColumn1 = UUnitTypeTable.GetUnitTypeCodeDBName();
                        TmpDisplayInColumn2 = UUnitTypeTable.GetUnitTypeNameDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = UUnitTypeTable.GetUnitTypeNameDBName();
                        break;

                    case TListTableEnum.DataLabelLookupList:

                        // Setup Data
                        FDataCache_ListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelLookupList);
                        TmpDisplayMember = PDataLabelLookupTable.GetValueCodeDBName();
                        TmpValueMember = PDataLabelLookupTable.GetValueCodeDBName();
                        TmpDisplayInColumn1 = PDataLabelLookupTable.GetValueCodeDBName();
                        TmpDisplayInColumn2 = PDataLabelLookupTable.GetValueDescDBName();
                        TmpDisplayInColumn3 = null;
                        TmpDisplayInColumn4 = null;
                        cmbAutoPopulated.LabelDisplaysColumn = PDataLabelLookupTable.GetValueDescDBName();
                        break;
                }

                if (FAddNotSetValue)
                {
                    DataRow Dr = FDataCache_ListTable.NewRow();
                    Dr[TmpValueMember] = FNotSetValue;
                    Dr[TmpDisplayMember] = FNotSetDisplay;
                    FDataCache_ListTable.Rows.InsertAt(Dr, 0);
                }

                cmbAutoPopulated.cmbCombobox.BeginUpdate();

                // Create a data view so the filter can be applied. If the filter string
                // that needs to be set from the outside is empty then it does not have
                // any effect
                FDataView = new DataView(FDataCache_ListTable);
                FDataView.RowFilter = FFilter;
                cmbAutoPopulated.cmbCombobox.DataSource = FDataView;
                cmbAutoPopulated.cmbCombobox.DisplayMember = TmpDisplayMember;
                cmbAutoPopulated.cmbCombobox.ValueMember = TmpValueMember;
                cmbAutoPopulated.cmbCombobox.DisplayInColumn1 = TmpDisplayInColumn1;
                cmbAutoPopulated.cmbCombobox.DisplayInColumn2 = TmpDisplayInColumn2;
                cmbAutoPopulated.cmbCombobox.DisplayInColumn3 = TmpDisplayInColumn3;
                cmbAutoPopulated.cmbCombobox.DisplayInColumn4 = TmpDisplayInColumn4;
                cmbAutoPopulated.cmbCombobox.ColumnsToSearch = TmpColumnsToSearch;
                cmbAutoPopulated.cmbCombobox.EndUpdate();
                cmbAutoPopulated.cmbCombobox.Name = this.Name + "_internal_ComboBox";
                cmbAutoPopulated.cmbCombobox.SuppressSelectionColor = true;
                cmbAutoPopulated.cmbCombobox.SelectedItem = null;
                FUserControlInitialised = true;
            }
        }

        private void AppearanceSetup(TListTableEnum AListTable)
        {
            switch (AListTable)
            {
                case TListTableEnum.AcquisitionCodeList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 350;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.AddresseeTypeList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.AddressLayoutList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.BusinessCodeList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 150;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.CountryList:
                    cmbAutoPopulated.ComboBoxWidth = 50;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.CurrencyCodeList:
                    cmbAutoPopulated.ComboBoxWidth = 60;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 170;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.DenominationList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 330;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.FoundationOwnerList:
                    cmbAutoPopulated.ComboBoxWidth = 120;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.GenderList:
                    cmbAutoPopulated.ComboBoxWidth = 88;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestList:
                    cmbAutoPopulated.ComboBoxWidth = 130;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestCategoryList:
                    cmbAutoPopulated.ComboBoxWidth = 130;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.LanguageCodeList:
                    cmbAutoPopulated.ComboBoxWidth = 57;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 130;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.LocationTypeList:
                    cmbAutoPopulated.ComboBoxWidth = 110;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.MaritalStatusList:
                    cmbAutoPopulated.ComboBoxWidth = 39;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 230;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    cmbAutoPopulated.cmbCombobox.MaxDropDownItems = 10;
                    break;

                case TListTableEnum.PartnerClassList:
                    cmbAutoPopulated.ComboBoxWidth = 130;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.PartnerStatusList:
                    cmbAutoPopulated.ComboBoxWidth = 95;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ProposalReviewFrequencyList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ProposalSubmitFrequencyList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ProposalStatusList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ProposalSubmissionTypeList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 100;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ReasonSubscriptionCancelledList:
                    cmbAutoPopulated.ComboBoxWidth = 110;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 450;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.ReasonSubscriptionGivenList:
                    cmbAutoPopulated.ComboBoxWidth = 110;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 450;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.PublicationList:
                    cmbAutoPopulated.ComboBoxWidth = 110;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 350;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.SubscriptionStatus:
                    cmbAutoPopulated.ComboBoxWidth = 110;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 0;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.UnitTypeList:
                    cmbAutoPopulated.ComboBoxWidth = 90;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;

                case TListTableEnum.DataLabelLookupList:
                    cmbAutoPopulated.ComboBoxWidth = 100;
                    cmbAutoPopulated.ColumnWidthCol1 = ComboBoxWidth;
                    cmbAutoPopulated.ColumnWidthCol2 = 200;
                    cmbAutoPopulated.ColumnWidthCol3 = 0;
                    cmbAutoPopulated.ColumnWidthCol4 = 0;
                    cmbAutoPopulated.ImageColumn = 0;
                    cmbAutoPopulated.Images = null;
                    break;
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

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AColumnName"></param>
        public void RecoverSelectedValue(String AColumnName)
        {
            System.Data.DataRowView mRowView;
            String mSelectedText;
            MessageBox.Show("AColumnName: " + AColumnName);
            MessageBox.Show("Selected Index: " + this.cmbAutoPopulated.cmbCombobox.SelectedIndex.ToString());
            mRowView = (System.Data.DataRowView) this.cmbAutoPopulated.cmbCombobox.SelectedItem;
            mSelectedText = mRowView.Row[AColumnName].ToString();
            MessageBox.Show(mSelectedText);
            this.cmbAutoPopulated.cmbCombobox.SelectedText = mSelectedText;
            this.cmbAutoPopulated.cmbCombobox.Text = mSelectedText;
        }
    }
}