//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, markusm, timop, andreww
//
// Copyright 2004-2014 by OM International
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
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

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
    public partial class TCmbAutoPopulated : TCmbLabelled
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
            AbilityAreaNameList,

            /// <summary>todoComment</summary>
            AbilityLevelList,

            /// <summary>todoComment</summary>
            ArrivalDeparturePointList,

            /// <summary>todoComment</summary>
            AccommodationCodeList,

            /// <summary>todoComment</summary>
            AcquisitionCodeList,

            /// <summary>todoComment</summary>
            AddresseeTypeList,

            /// <summary>todoComment</summary>
            AddressBlockElementList,

            /// <summary>todoComment</summary>
            AddressDisplayOrderList,

            /// <summary>todoComment</summary>
            AddressLayoutList,

            /// <summary>todoComment</summary>
            AddressLayoutCodeList,

            /// <summary>todoComment</summary>
            ApplicantStatusList,

            /// <summary>todoComment</summary>
            BusinessCodeList,

            /// <summary>todoComment</summary>
            CommitmentStatusList,

            /// <summary>todoComment</summary>
            ContactCode,

            /// <summary>todoComment</summary>
            ContactList,

            /// <summary>todoComment</summary>
            CountryList,

            /// <summary>todoComment</summary>
            CurrencyCodeList,

            /// <summary>todoComment</summary>
            DataLabelLookupList,

            /// <summary>todoComment</summary>
            DataLabelLookupCategoryList,

            /// <summary>todoComment</summary>
            DenominationList,

            /// <summary>todoComment</summary>
            DocumentTypeCategoryList,

            /// <summary>todoComment</summary>
            DocumentTypeList,

            /// <summary>todoComment</summary>
            EventApplicationTypeList,

            /// <summary>todoComment</summary>
            EventRoleList,

            /// <summary>todoComment</summary>
            FieldApplicationTypeList,

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
            JobAssignmentTypeList,

            /// <summary>todoComment</summary>
            LanguageCodeList,

            /// <summary>todoComment</summary>
            LanguageLevelList,

            /// <summary>todoComment</summary>
            LedgerNameList,

            /// <summary>todoComment</summary>
            LedgerNameCurrentUserPermissionList,

            /// <summary>todoComment</summary>
            LocationTypeList,

            /// <summary>todoComment</summary>
            MailingList,

            /// <summary>todoComment</summary>
            MaritalStatusList,

            /// <summary>todoComment</summary>
            MethodOfGivingList,

            /// <summary>todoComment</summary>
            MethodOfPaymentList,

            /// <summary>todoComment</summary>
            Module,

            /// <summary>todoComment</summary>
            PartnerAttributeCategoryList,

            /// <summary>todoComment</summary>
            PartnerAttributeTypeList,

            /// <summary>todoComment</summary>
            PartnerClassList,

            /// <summary>todoComment</summary>
            PartnerStatusList,

            /// <summary>todoComment</summary>
            PassportDetailsTypeList,

            /// <summary>todoComment</summary>
            PassportNationalityCodeList,

            /// <summary>todoComment</summary>
            PositionList,

            /// <summary>todoComment</summary>
            PostCodeRegionList,

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
            PublicationInfoList,

            /// <summary>todoComment</summary>
            ReasonSubscriptionCancelledList,

            /// <summary>todoComment</summary>
            ReasonSubscriptionGivenList,

            /// <summary>todoComment</summary>
            RelationList,

            /// <summary>todoComment</summary>
            RelationCategoryList,

            /// <summary>todoComment</summary>
            SkillCategoryList,

            /// <summary>todoComment</summary>
            SkillLevelList,

            /// <summary>todoComment</summary>
            SubscriptionStatus,

            /// <summary>todoComment</summary>
            TransportTypeList,

            /// <summary>todoComment</summary>
            UnitTypeList,

            /// <summary>todoComment</summary>
            UserList
        };

        private DataTable FDataCache_ListTable = null;
        private DataView FDataView;
        private TListTableEnum FListTable;
        private Boolean FUserControlInitialised = false;
        private String FFilter;
        private Boolean FAddNotSetValue = false;
        private String FNotSetValue;
        private String FNotSetDisplay;
        private Boolean FAllowDbNull = false;
        private string FNullValueDesciption = ApplWideResourcestrings.StrUndefined;

        #region Properties

        /// <summary>
        /// Gets or sets the text associated with this Control (in this case: the text in the editable part of the ComboBox)
        /// </summary>
        public new string Text
        {
            get
            {
                return cmbCombobox.Text;
            }

            set
            {
                cmbCombobox.Text = value;
            }
        }

        /// <summary>
        /// Exposes the DataTable that defines the items that are shown as drop-down items.
        /// <para>
        /// Use this Property only to access the underlying DataTable of the items that
        /// are displayed in the ComboBox in a <em>read-only</em> manner!
        /// </para>
        /// </summary>
        /// <remarks>Do not modifiy the DataTable that is returned to achieve an update of
        /// the ComboBox's drop-down items, as this will not reliably be reflected in the
        /// CombBox's drop-down items! To achieve that, initialise the ComboBox again by
        /// calling one of the InitialiseUserControl Methods.
        /// </remarks>
        public DataTable Table
        {
            get
            {
                return FDataCache_ListTable;
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

        /// <summary>
        /// Set this to true to include a first row in the combo list that has a value of DbNull, a display of empty string and a description of 'Undefined'.
        /// This row will not be added to the List table that is supplying the other values.
        /// </summary>
        public Boolean AllowDbNull
        {
            get
            {
                return FAllowDbNull;
            }

            set
            {
                FAllowDbNull = value;

                // we also set this property of the labelled combo so that even though the value member is empty text
                //  we still see our description of 'Undefined'
                FIncludeDescriptionForEmptyValue = FAllowDbNull;
            }
        }

        /// <summary>
        /// The description of the Null value row (if enabled)
        /// </summary>
        /// <value>
        /// The null value desciption.
        /// </value>
        public string NullValueDesciption
        {
            set
            {
                FNullValueDesciption = value;
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the control's underlying cmbCombobox.
        /// </summary>
        public new object Tag
        {
            get
            {
                return cmbCombobox.Tag;
            }

            set
            {
                cmbCombobox.Tag = value;
            }
        }

        #endregion

        #region Events

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

        /**
         * This Event is thrown when the internal ComboBox throws the SelectedValueChanged Event.
         */
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when the internal ComboBox throws the TextChanged Event.")]
        public new event System.EventHandler TextChanged;

        private void CmbCombobox_TextChanged(System.Object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        /**
         * This Event is thrown when the internal ComboBox throws the SelectedValueChanged Event.
         */
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when the internal ComboBox throws the DropDownClosed Event.")]
        public event System.EventHandler DropDownClosed;

        private void CmbCombobox_DropDownClosed(System.Object sender, EventArgs e)
        {
            if (DropDownClosed != null)
            {
                DropDownClosed(this, e);
            }
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TCmbAutoPopulated()
            : base()
        {
        }

        /// <summary>
        /// initialise user controls for specific tables
        /// it might be better to do this in other functions, see also Client/lib/MFinance/gui/FinanceComboboxes.cs
        /// </summary>
        public void InitialiseUserControl()
        {
            Ict.Common.Data.TTypedDataTable TypedTable;
            DataTable SortedCacheableDataTable;

            if (DesignMode)
            {
                return;
            }

            switch (FListTable)
            {
                case TListTableEnum.AbilityAreaNameList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.AbilityAreaList),
                    PtAbilityAreaTable.GetAbilityAreaNameDBName(),
                    PtAbilityAreaTable.GetAbilityAreaDescrDBName(),
                    null);
                    break;

                case TListTableEnum.AbilityLevelList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.AbilityLevelList),
                    PtAbilityLevelTable.GetAbilityLevelDBName(),
                    PtAbilityLevelTable.GetAbilityLevelDescrDBName(),
                    null);
                    break;

                case TListTableEnum.ArrivalDeparturePointList:
                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.ArrivalDeparturePointList),
                    PtArrivalPointTable.GetCodeDBName(),
                    PtArrivalPointTable.GetDescriptionDBName(),
                    null);
                    break;

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

                case TListTableEnum.AddressLayoutCodeList:

                    SortedCacheableDataTable = TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.AddressLayoutCodeList);
                    SortedCacheableDataTable.DefaultView.Sort = PAddressLayoutCodeTable.GetDisplayIndexDBName() + " ASC";

                    InitialiseUserControl(SortedCacheableDataTable,
                    PAddressLayoutCodeTable.GetCodeDBName(),
                    PAddressLayoutCodeTable.GetDescriptionDBName(),
                    null
                    );
                    break;

                case TListTableEnum.AddressBlockElementList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.AddressBlockElementList),
                    PAddressBlockElementTable.GetAddressElementCodeDBName(),
                    PAddressBlockElementTable.GetAddressElementDescriptionDBName(),
                    null
                    );
                    break;

                case TListTableEnum.ApplicantStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.ApplicantStatusList),
                    PtApplicantStatusTable.GetCodeDBName(),
                    PtApplicantStatusTable.GetDescriptionDBName(),
                    null
                    );
                    break;

                case TListTableEnum.BusinessCodeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.BusinessCodeList),
                    "p_business_code_c",
                    "p_business_description_c",
                    null);
                    break;

                case TListTableEnum.CommitmentStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.CommitmentStatusList),
                    PmCommitmentStatusTable.GetCodeDBName(),
                    PmCommitmentStatusTable.GetDescDBName(),
                    null
                    );
                    break;

                case TListTableEnum.ContactCode:
                    InitialiseUserControl(TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.MethodOfContactList),
                    PMethodOfContactTable.GetMethodOfContactCodeDBName(),
                    PMethodOfContactTable.GetDescriptionDBName(),
                    null
                    );
                    break;

                case TListTableEnum.ContactList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.ContactList),
                    PtContactTable.GetContactNameDBName(),
                    PtContactTable.GetContactDescrDBName(),
                    null
                    );
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

                case TListTableEnum.DataLabelLookupList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelLookupList),
                    PDataLabelLookupTable.GetValueCodeDBName(),
                    PDataLabelLookupTable.GetValueDescDBName(),
                    null);
                    break;

                case TListTableEnum.DataLabelLookupCategoryList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelLookupCategoryList),
                    PDataLabelLookupCategoryTable.GetCategoryCodeDBName(),
                    PDataLabelLookupCategoryTable.GetCategoryDescDBName(),
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

                case TListTableEnum.DocumentTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.DocumentTypeList),
                    "pm_doc_code_c",
                    "pm_description_c",
                    null);
                    break;

                case TListTableEnum.EventApplicationTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.EventApplicationTypeList),
                    PtApplicationTypeTable.GetAppTypeNameDBName(),
                    PtApplicationTypeTable.GetAppTypeDescrDBName(),
                    null
                    );
                    break;

                case TListTableEnum.EventRoleList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.EventRoleList),
                    PtCongressCodeTable.GetCodeDBName(),
                    PtCongressCodeTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.FieldApplicationTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.FieldApplicationTypeList),
                    PtApplicationTypeTable.GetAppTypeNameDBName(),
                    PtApplicationTypeTable.GetAppTypeDescrDBName(),
                    null
                    );
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
                    TRemote.MCommon.DataReader.WebConnectors.GetData(PInternationalPostalTypeTable.GetTableDBName(), null, out TypedTable);

                    InitialiseUserControl(
                    TypedTable,
                    PInternationalPostalTypeTable.GetInternatPostalTypeCodeDBName(),
                    PInternationalPostalTypeTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.JobAssignmentTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheableUnitsTable(TCacheableUnitTablesEnum.JobAssignmentTypeList),
                    "pt_assignment_type_code_c",
                    "pt_assignment_code_descr_c",
                    null);
                    break;

                case TListTableEnum.LanguageCodeList:

                    InitialiseUserControl(
                    TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.LanguageCodeList),
                    "p_language_code_c",
                    "p_language_description_c",
                    null);
                    break;

                case TListTableEnum.LanguageLevelList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.LanguageLevelList),
                    PtLanguageLevelTable.GetLanguageLevelDBName(),
                    PtLanguageLevelTable.GetLanguageLevelDescrDBName(),
                    null);
                    break;

                case TListTableEnum.LedgerNameList:

                    InitialiseUserControl(
                    TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList),
                    "LedgerNumber",
                    "LedgerName",
                    null);
                    break;

                case TListTableEnum.LedgerNameCurrentUserPermissionList:

                    SortedCacheableDataTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);

                    for (int i = 0; i < SortedCacheableDataTable.Rows.Count; i++)
                    {
                        if (!UserInfo.GUserInfo.IsInLedger(Convert.ToInt32(SortedCacheableDataTable.Rows[i]["LedgerNumber"])))
                        {
                            SortedCacheableDataTable.Rows.RemoveAt(i);
                        }
                    }

                    SortedCacheableDataTable.DefaultView.Sort = "LedgerNumber ASC";

                    InitialiseUserControl(
                    SortedCacheableDataTable,
                    "LedgerNumber",
                    "LedgerName",
                    null);
                    break;

                case TListTableEnum.LocationTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LocationTypeList),
                    PLocationTypeTable.GetCodeDBName(),
                    null,
                    null);
                    break;

                case TListTableEnum.MailingList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.MailingList),
                    PMailingTable.GetMailingCodeDBName(),
                    PMailingTable.GetMailingDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.MaritalStatusList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.MaritalStatusList),
                    "pt_code_c",
                    "pt_description_c",
                    null);
                    break;

                case TListTableEnum.MethodOfGivingList:

                    InitialiseUserControl(
                    TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MethodOfGivingList),
                    AMethodOfGivingTable.GetMethodOfGivingCodeDBName(),
                    AMethodOfGivingTable.GetMethodOfGivingDescDBName(),
                    null);
                    break;

                case TListTableEnum.MethodOfPaymentList:

                    InitialiseUserControl(
                    TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MethodOfPaymentList),
                    AMethodOfPaymentTable.GetMethodOfPaymentCodeDBName(),
                    AMethodOfPaymentTable.GetMethodOfPaymentDescDBName(),
                    null);
                    break;

                case TListTableEnum.Module:
                    InitialiseUserControl(TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ModuleList),
                    SModuleTable.GetModuleIdDBName(),
                    SModuleTable.GetModuleNameDBName(),
                    null);
                    break;

                case TListTableEnum.PartnerAttributeCategoryList:
                    SortedCacheableDataTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ContactCategoryList);

                    SortedCacheableDataTable.DefaultView.Sort = PPartnerAttributeCategoryTable.GetIndexDBName() + " ASC";

                    InitialiseUserControl(SortedCacheableDataTable,
                    "p_category_code_c",
                    "p_category_desc_c",
                    null);
                    break;

                case TListTableEnum.PartnerAttributeTypeList:
                    SortedCacheableDataTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ContactTypeList);

                    SortedCacheableDataTable.DefaultView.Sort = PPartnerAttributeTypeTable.GetIndexDBName() + " ASC";

                    InitialiseUserControl(SortedCacheableDataTable,
                    PPartnerAttributeTypeTable.GetAttributeTypeDBName(),
                    PPartnerAttributeTypeTable.GetDescriptionDBName(),
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

                case TListTableEnum.PositionList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheableUnitsTable(TCacheableUnitTablesEnum.PositionList),
                    "pt_position_name_c",
                    "pt_position_descr_c",
                    null);
                    break;

                case TListTableEnum.PassportDetailsTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.PassportTypeList),

                    /*PmPassportDetailsTable.GetPassportDetailsTypeDBName(),*/
                    PtPassportTypeTable.GetCodeDBName(),
                    PtPassportTypeTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.PassportNationalityCodeList:

                    InitialiseUserControl(
                    TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CountryList),
                    PCountryTable.GetCountryCodeDBName(),
                    PCountryTable.GetCountryNameDBName(),
                    null);
                    break;

                case TListTableEnum.PostCodeRegionList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.PostcodeRegionList),
                    PPostcodeRegionTable.GetRegionDBName(),
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

                case TListTableEnum.PublicationInfoList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationInfoList),
                    PPublicationTable.GetPublicationCodeDBName(),
                    PPublicationTable.GetPublicationDescriptionDBName(),
                    null);
                    // add extra column to show user if Publication is valid or not
                    cmbCombobox.DisplayInColumn3 = MPartnerConstants.PUBLICATION_VALID_TEXT_COLUMNNAME;
                    break;

                case TListTableEnum.SubscriptionStatus:

                    InitialiseUserControl(
                    TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.SubscriptionStatus),
                    "SubscriptionStatus",
                    null,
                    null);
                    break;

                case TListTableEnum.TransportTypeList:
                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.TransportTypeList),
                    PtTravelTypeTable.GetCodeDBName(),
                    PtTravelTypeTable.GetDescriptionDBName(),
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

                case TListTableEnum.SkillCategoryList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.SkillCategoryList),
                    PtSkillCategoryTable.GetCodeDBName(),
                    PtSkillCategoryTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.SkillLevelList:

                    InitialiseUserControl(
                    TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.SkillLevelList),
                    PtSkillLevelTable.GetLevelDBName(),
                    PtSkillLevelTable.GetDescriptionDBName(),
                    null);
                    break;

                case TListTableEnum.UnitTypeList:

                    InitialiseUserControl(
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.UnitTypeList),
                    UUnitTypeTable.GetUnitTypeCodeDBName(),
                    UUnitTypeTable.GetUnitTypeNameDBName(),
                    null);
                    break;

                case TListTableEnum.UserList:

                    InitialiseUserControl(
                    TDataCache.TMSysMan.GetCacheableSysManTable(TCacheableSysManTablesEnum.UserList),
                    SUserTable.GetUserIdDBName(),
                    MSysManConstants.USER_LAST_AND_FIRST_NAME_COLUMNNAME,
                    null);
                    break;
            }
        }

        /// <summary>
        /// generic function for initialising the combobox
        /// does not depend on table implementations
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="AValueDBName">name of the column in the table that has the name</param>
        /// <param name="ADisplayDBName"></param>
        /// <param name="ADescDBName">name of the column in the table that has the description; can be empty</param>
        /// <param name="AColumnsToSearch"></param>
        /// <param name="AActiveDBName"></param>
        public void InitialiseUserControl(DataTable ATable,
            string AValueDBName,
            string ADisplayDBName,
            string ADescDBName,
            string AColumnsToSearch,
            string AActiveDBName)
        {
            FDataCache_ListTable = ATable;

            // Pass on any set Tag
            cmbCombobox.Tag = this.Tag;

            // only add event handlers once
            if (!FUserControlInitialised)
            {
                this.cmbCombobox.SelectedValueChanged += new System.EventHandler(this.CmbCombobox_SelectedValueChanged);
                this.cmbCombobox.TextChanged += new System.EventHandler(this.CmbCombobox_TextChanged);
                this.cmbCombobox.DropDownClosed += new System.EventHandler(this.CmbCombobox_DropDownClosed);
            }

            if (FAddNotSetValue)
            {
                DataRow Dr = FDataCache_ListTable.NewRow();
                Dr[AValueDBName] = FNotSetValue;
                Dr[ADisplayDBName] = FNotSetDisplay;

                if (AActiveDBName != null)
                {
                    Dr[AActiveDBName] = true;
                }

                FDataCache_ListTable.Rows.InsertAt(Dr, 0);
            }
            else if (FAllowDbNull)
            {
                // We need to add a row to the table that has a NULL value
                // We don't want this new row to end up in the referenced data table so we make a copy first
                FDataCache_ListTable = FDataCache_ListTable.Copy();

                // Now add the row
                DataRow Dr = FDataCache_ListTable.NewRow();
                Dr[AValueDBName] = DBNull.Value;
                Dr[ADisplayDBName] = String.Empty;

                if (ADescDBName != null)
                {
                    Dr[ADescDBName] = FNullValueDesciption;
                }

                FDataCache_ListTable.Rows.InsertAt(Dr, 0);
            }

            if ((ADescDBName == null) || (ADescDBName.Length == 0))
            {
                RemoveDescriptionLabel();
            }
            else
            {
                LabelDisplaysColumn = ADescDBName;
            }

            cmbCombobox.BeginUpdate();
            FDataView = new DataView(FDataCache_ListTable);
            FDataView.RowFilter = FFilter;

            if ((ATable.DefaultView.Sort == null) || (ATable.DefaultView.Sort.Length == 0))
            {
                FDataView.Sort = ADisplayDBName;
            }
            else
            {
                FDataView.Sort = ATable.DefaultView.Sort;
            }

            cmbCombobox.DescriptionMember = ADescDBName;
            cmbCombobox.DisplayMember = ADisplayDBName;
            cmbCombobox.ValueMember = AValueDBName;
            cmbCombobox.DisplayInColumn1 = ADisplayDBName;
            cmbCombobox.DisplayInColumn2 = (ADescDBName != null && ADescDBName.Length > 0) ? ADescDBName : null;
            cmbCombobox.DisplayInColumn3 = null;
            cmbCombobox.DisplayInColumn4 = null;
            cmbCombobox.ColumnsToSearch = AColumnsToSearch;
            cmbCombobox.DataSource = FDataView;
            cmbCombobox.EndUpdate();
            cmbCombobox.Name = this.Name + "_internal_ComboBox";
            cmbCombobox.SuppressSelectionColor = true;
            cmbCombobox.SelectedItem = null;

            FUserControlInitialised = true;
        }

        /// <summary>
        /// overload activeColumn not given/ needed
        /// does not depend on table implementations
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="AValueDBName">name of the column in the table that has the name</param>
        /// <param name="ADisplayDBName"></param>
        /// <param name="ADescDBName">name of the column in the table that has the description; can be empty</param>
        /// <param name="AColumnsToSearch"></param>
        public void InitialiseUserControl(DataTable ATable, string AValueDBName, string ADisplayDBName, string ADescDBName, string AColumnsToSearch)
        {
            InitialiseUserControl(ATable, AValueDBName, ADisplayDBName, ADescDBName, AColumnsToSearch, null);
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
            this.ComboBoxWidth = 100;
            this.ColumnWidthCol1 = 100;
            this.ColumnWidthCol2 = 0;
            this.ColumnWidthCol3 = 0;
            this.ColumnWidthCol4 = 0;
            this.ImageColumn = 0;
            this.Images = null;

            for (Int32 Counter = 0; Counter < AColumnWidth.Length; Counter++)
            {
                if (AColumnWidth[Counter] > 0)
                {
                    switch (Counter)
                    {
                        case 0:
                            this.ComboBoxWidth = AColumnWidth[Counter];
                            this.ColumnWidthCol1 = AColumnWidth[Counter];
                            break;

                        case 1:
                            this.ColumnWidthCol2 = AColumnWidth[Counter];
                            break;

                        case 2:
                            this.ColumnWidthCol3 = AColumnWidth[Counter];
                            break;

                        case 3:
                            this.ColumnWidthCol4 = AColumnWidth[Counter];
                            break;
                    }
                }
            }

            if (this.Width < this.ColumnWidthCol1)
            {
                // Ensure that the ComboBox itself is never 'cut off'
                this.ComboBoxWidth = this.Width;
            }
            else
            {
                this.ComboBoxWidth = this.ColumnWidthCol1;
            }

            if (AMaxDropDownItems > 0)
            {
                cmbCombobox.MaxDropDownItems = AMaxDropDownItems;
            }
        }

        /// it might be better to do this in other functions, see also Client/lib/MFinance/gui/FinanceComboboxes.cs
        private void AppearanceSetup(TListTableEnum AListTable)
        {
            this.ComboBoxWidth = 0;     // This line ensures that setting ComboBoxWidth in YAML is useless,
            // but even without this line here the YAML setting will get overruled by AppearanceSetup(Int32[] AColumnWidth, Int32 AMaxDropDownItems).
            this.ColumnWidthCol1 = 100;
            this.ColumnWidthCol2 = 0;
            this.ColumnWidthCol3 = 0;
            this.ColumnWidthCol4 = 0;
            this.ImageColumn = 0;
            this.Images = null;

            switch (AListTable)
            {
                case TListTableEnum.AbilityAreaNameList:
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.AbilityLevelList:
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.ArrivalDeparturePointList:
                    this.ColumnWidthCol1 = 150;
                    this.ColumnWidthCol2 = 300;
                    break;

                case TListTableEnum.AcquisitionCodeList:
                    this.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.AddresseeTypeList:
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.AddressDisplayOrderList:
                    this.ColumnWidthCol1 = 50;
                    this.ColumnWidthCol2 = 150;
                    break;

                case TListTableEnum.ApplicantStatusList:
                    this.ColumnWidthCol1 = 80;
                    this.ColumnWidthCol2 = 300;
                    break;

                case TListTableEnum.BusinessCodeList:
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.CommitmentStatusList:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 300;
                    break;

                case TListTableEnum.ContactCode:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 250;
                    break;

                case TListTableEnum.ContactList:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.CountryList:
                    this.ColumnWidthCol1 = 50;
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.CurrencyCodeList:
                    this.ColumnWidthCol1 = 60;
                    this.ColumnWidthCol2 = 170;
                    break;

                case TListTableEnum.DataLabelLookupList:
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.DataLabelLookupCategoryList:
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.DenominationList:
                    this.ColumnWidthCol2 = 330;
                    break;

                case TListTableEnum.DocumentTypeList:
                    this.ColumnWidthCol1 = 150;
                    this.ColumnWidthCol2 = 275;
                    break;

                case TListTableEnum.EventApplicationTypeList:
                    this.ColumnWidthCol1 = 150;
                    this.ColumnWidthCol2 = 300;
                    break;

                case TListTableEnum.EventRoleList:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 250;
                    break;

                case TListTableEnum.DocumentTypeCategoryList:
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.FieldApplicationTypeList:
                    this.ColumnWidthCol1 = 150;
                    this.ColumnWidthCol2 = 300;
                    break;

                case TListTableEnum.FoundationOwnerList:
                    this.ColumnWidthCol1 = 120;
                    break;

                case TListTableEnum.FrequencyList:
                    this.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.GenderList:
                    this.ColumnWidthCol1 = 88;
                    cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestList:
                    this.ColumnWidthCol1 = 130;
                    this.ColumnWidthCol2 = 230;
                    cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InterestCategoryList:
                    this.ColumnWidthCol1 = 130;
                    this.ColumnWidthCol2 = 230;
                    cmbCombobox.MaxDropDownItems = 3;
                    break;

                case TListTableEnum.InternationalPostalTypeList:
                    this.ColumnWidthCol1 = 100;
                    this.ColumnWidthCol2 = 230;
                    break;

                case TListTableEnum.JobAssignmentTypeList:
                    this.ColumnWidthCol1 = 40;
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.LanguageCodeList:
                    this.ColumnWidthCol1 = 57;
                    this.ColumnWidthCol2 = 130;
                    break;

                case TListTableEnum.LanguageLevelList:
                    this.ColumnWidthCol1 = 57;
                    this.ColumnWidthCol2 = 130;
                    break;

                case TListTableEnum.LedgerNameList:
                    this.ColumnWidthCol1 = 55;
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.LedgerNameCurrentUserPermissionList:
                    this.ColumnWidthCol1 = 55;
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.LocationTypeList:
                    this.ColumnWidthCol1 = 110;
                    break;

                case TListTableEnum.MailingList:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 250;
                    break;

                case TListTableEnum.MaritalStatusList:
                    this.ColumnWidthCol1 = 39;
                    this.ColumnWidthCol2 = 230;
                    cmbCombobox.MaxDropDownItems = 10;
                    break;

                case TListTableEnum.Module:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 230;
                    break;

                case TListTableEnum.PartnerAttributeCategoryList:
                    this.ColumnWidthCol1 = 200;
                    this.ColumnWidthCol2 = 305;
                    break;

                case TListTableEnum.PartnerAttributeTypeList:
                    this.ColumnWidthCol1 = 200;
                    this.ColumnWidthCol2 = 295;
                    break;

                case TListTableEnum.PartnerClassList:
                    this.ColumnWidthCol1 = 130;
                    break;

                case TListTableEnum.PartnerStatusList:
                    this.ColumnWidthCol1 = 95;
                    break;

                case TListTableEnum.PositionList:
                    this.ColumnWidthCol1 = 200;
                    this.ColumnWidthCol2 = 350;
                    break;

                case TListTableEnum.PassportDetailsTypeList:
                    this.ColumnWidthCol1 = 50;
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 4;
                    break;

                case TListTableEnum.PassportNationalityCodeList:
                    this.ColumnWidthCol1 = 50;
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 9;
                    break;

                case TListTableEnum.PostCodeRegionList:
                    this.ColumnWidthCol1 = 110;
                    break;

                case TListTableEnum.ProposalSubmissionTypeList:
                    this.ColumnWidthCol2 = 100;
                    break;

                case TListTableEnum.ReasonSubscriptionCancelledList:
                    this.ColumnWidthCol1 = 110;
                    this.ColumnWidthCol2 = 450;
                    break;

                case TListTableEnum.ReasonSubscriptionGivenList:
                    this.ColumnWidthCol1 = 110;
                    this.ColumnWidthCol2 = 450;
                    break;

                case TListTableEnum.PublicationInfoList:
                    this.ColumnWidthCol1 = 110;
                    this.ColumnWidthCol2 = 350;
                    this.ColumnWidthCol3 = 80;
                    break;

                case TListTableEnum.SubscriptionStatus:
                    this.ColumnWidthCol1 = 110;
                    break;

                case TListTableEnum.TransportTypeList:
                    this.ColumnWidthCol1 = 80;
                    this.ColumnWidthCol2 = 130;
                    break;

                case TListTableEnum.SkillCategoryList:
                    this.ColumnWidthCol1 = 110;
                    this.ColumnWidthCol2 = 250;
                    break;

                case TListTableEnum.SkillLevelList:
                    this.ColumnWidthCol1 = 57;
                    this.ColumnWidthCol2 = 130;
                    break;

                case TListTableEnum.RelationList:
                    this.ColumnWidthCol2 = 150;
                    cmbCombobox.MaxDropDownItems = 15;
                    break;

                case TListTableEnum.RelationCategoryList:
                    this.ColumnWidthCol2 = 150;
                    break;

                case TListTableEnum.UnitTypeList:
                    this.ColumnWidthCol1 = 90;
                    this.ColumnWidthCol2 = 200;
                    break;

                case TListTableEnum.UserList:
                    this.ColumnWidthCol1 = 120;
                    this.ColumnWidthCol2 = 200;
                    break;
            }

            if (this.ComboBoxWidth == 0)
            {
                if (this.Width < this.ColumnWidthCol1)
                {
                    // Ensure that the ComboBox itself is never 'cut off'
                    this.ComboBoxWidth = this.Width;
                }
                else
                {
                    this.ComboBoxWidth = this.ColumnWidthCol1;
                }
            }

            if (DesignMode)
            {
                // Put text in ComboBox to make it easier to distinguish different AutoPopulatedComboBoxes on one Form
                cmbCombobox.Text = AListTable.ToString("G");
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
            cmbCombobox.DataBindings.Add("SelectedValue", ADataSource, AColumnName);
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
            if ((cmbCombobox.DataBindings.Count == 1) && (cmbCombobox.DataBindings[0].BindingManagerBase != null))
            {
                cmbCombobox.DataBindings[0].BindingManagerBase.EndCurrentEdit();
            }
        }

        /// <summary>
        /// Returns the DataRow that underlies the currently selected ComboBox Item.
        /// </summary>
        public DataRow GetSelectedItemsDataRow()
        {
            DataRow ReturnValue = null;

            if (cmbCombobox.SelectedItem != null)
            {
                DataRowView Drv = (DataRowView)cmbCombobox.SelectedItem;
                ReturnValue = Drv.Row;
            }

            return ReturnValue;
        }
    }
}