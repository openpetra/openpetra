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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Controls;
using System.Globalization;
using GNU.Gettext;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// UserControl that holds the same fields as the address/location part in the
    /// old Progress Partner Edit screen.
    ///
    /// Since this UserControl can be re-used, it is no longer necessary to recreate
    /// all the layout and fields and verification rules in other places than the
    /// Partner Edit screen, eg. in the Personnel Shepherds!
    ///
    /// Interesting feature: this UserControl contains another UserControl -
    /// CountryDropDown_UserControl - which is also DataBound and retrieves its
    /// Country entries on its own from the Client-side DataCache.
    ///
    /// @Comment Have a look at UC_PartnerAddresses to see how DataBinding works
    /// with this UserControl.
    ///
    /// </summary>
    public partial class TUC_PartnerAddress : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary>Holds a reference to the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;

        /// <summary>DataTable Key value for the record we are working with</summary>
        private Int32 FKey;

        /// <summary>DataView for the p_location record we are working with</summary>
        private DataView FLocationDV;

        /// <summary>DataView for the p_partner_location record we are working with</summary>
        private DataView FPartnerLocationDV;

        /// <summary>Current Address Order (used for optimising the number of TabIndex changes of certain Controls)</summary>
        private Int32 FCurrentAddressOrder;
        private Int32 FLastNonChangedAddressFieldTabIndex;
        private CustomEnablingDisabling.TDelegateDisabledControlClick FDelegateDisabledControlClick;

        /// <summary>DataTable Key value for the record we are working with</summary>
        public Int32 Key
        {
            get
            {
                return FKey;
            }

            set
            {
                FKey = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public CustomEnablingDisabling.TDelegateDisabledControlClick DisabledControlClickHandler
        {
            set
            {
                FDelegateDisabledControlClick = value;
            }
        }

        /// <summary>
        /// Processes SelectedValueChanged Event of the cmbCountry ComboBox.
        ///
        /// </summary>
        /// <param name="Sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void CmbCountry_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            SetAddressFieldOrder();
        }

        /// <summary>
        /// Default Constructor.
        ///
        /// Initialises Fields.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUC_PartnerAddress() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblDateGoodUntil.Text = Catalog.GetString("Valid To") + ":" +;
            this.lblDateEffective.Text = Catalog.GetString("Vali&d From") + ":" +;
            this.lblTelephoneNo.Text = Catalog.GetString("&Phone") + ":" +;
            this.lblLocationType.Text = Catalog.GetString("&Location Type") + ":" +;
            this.lblCounty.Text = Catalog.GetString("County/St&ate") + ":" +;
            this.lblCountry.Text = Catalog.GetString("Co&untry") + ":" +;
            this.lblPostalCode.Text = Catalog.GetString("Po&st Code") + ":" +;
            this.lblCity.Text = Catalog.GetString("Cit&y/Town") + ":" +;
            this.lblAddLine3.Text = Catalog.GetString("Addr&3") + ":" +;
            this.lblStreetName.Text = Catalog.GetString("Street-&2") + ":" +;
            this.lblLocality.Text = Catalog.GetString("Addr&1") + ":" +;
            this.txtTelephoneExt.Text = Catalog.GetString("TelExt");
            this.txtTelephoneNo.Text = Catalog.GetString("Phone No");
            this.txtPostalCode.Text = Catalog.GetString("Postal Code");
            this.txtCounty.Text = Catalog.GetString("County");
            this.txtCity.Text = Catalog.GetString("City");
            this.txtAddLine3.Text = Catalog.GetString("Address Line 3");
            this.txtStreetName.Text = Catalog.GetString("Street Name");
            this.txtLocality.Text = Catalog.GetString("Locality");
            this.lblFaxNo.Text = Catalog.GetString("Fa&x") + ":" +;
            this.txtFaxNo.Text = Catalog.GetString("Fax No");
            this.txtFaxExt.Text = Catalog.GetString("FaxExt");
            this.lblEmail.Text = Catalog.GetString("E&mail") + ":" +;
            this.txtEmail.Text = Catalog.GetString("Email");
            this.lblURL.Text = Catalog.GetString("Websi&te") + ":" +;
            this.txtURL.Text = Catalog.GetString("Web URL");
            this.lblAltTelephoneNo.Text = Catalog.GetString("Alte&rnate") + ":" +;
            this.txtAltTelephoneNo.Text = Catalog.GetString("Alternate Phone No");
            this.txtMobileTelephoneNo.Text = Catalog.GetString("Mobile Phone No");
            this.lblMobileTelephoneNo.Text = Catalog.GetString("Mo&bile") + ":" +;
            this.grpAddress.Text = Catalog.GetString("Address");
            this.grpPartnerLocation.Text = Catalog.GetString("Partner-specific Data for this Address");
            this.txtValidFrom.Text = Catalog.GetString("Valid From");
            this.lblMailingAddress.Text = Catalog.GetString("Mailin&g Address") + ":" +;
            this.txtValidTo.Text = Catalog.GetString("Valid To");
            #endregion

            FCurrentAddressOrder = 0;
            FLastNonChangedAddressFieldTabIndex = txtAddLine3.TabIndex;

            // I18N: assign proper font which helps to read asian characters
            txtAddLine3.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtStreetName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtCity.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtCounty.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtLocality.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPostalCode.Font = TAppSettingsManager.GetDefaultBoldFont();
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
                FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(DataSavingStartedHandler);
            }
        }

        private void PnlEmail_MouseEnter(System.Object sender, System.EventArgs e)
        {
            if (this.txtEmail.TextLength > 25)
            {
                this.ToolTipEmail.SetToolTip(this.pnlEmail, this.txtEmail.Text);
                this.ToolTipEmail.AutomaticDelay = 10000;
                this.ToolTipEmail.AutoPopDelay = 30000;
                this.ToolTipEmail.Active = true;
            }
        }

        private void TxtEmail_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtEmail.Text = txtEmail.Text.Trim();
        }

        /// <summary>
        /// Sets the TabIndexes of certain address fields depending on the Country's
        /// AddressOrder.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetAddressFieldOrder()
        {
            Int32 AddressOrder = 0;

            try
            {
                AddressOrder = TAddressHandling.GetAddressOrder(cmbCountry.SelectedValue.ToString());
            }
            catch (Exception)
            {
            }

            if (AddressOrder == 0)
            {
                // MessageBox.Show('Address Order = 0');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else if (AddressOrder == 1)
            {
                // MessageBox.Show('Address Order = 1');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else
            {
                if (AddressOrder == 2)
                {
                    if (FCurrentAddressOrder != AddressOrder)
                    {
                        // MessageBox.Show('Peforming Address Order change');
                        lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                        txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                        lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                        txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                        lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                        txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                        lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                        cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                        FCurrentAddressOrder = AddressOrder;
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SetFocusToFirstField()
        {
            txtLocality.Focus();
        }

        /// <summary>
        /// Processes Validating Event of the txtPostalCode TextBox.
        ///
        /// Just converts the Postal Code to all uppercase characters.
        ///
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void TxtPostalCode_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtPostalCode.Text = txtPostalCode.Text.ToUpper();
        }

        /// <summary>
        /// Processes DataColumnChanging Event of the PPartnerLocation DataTable.
        ///
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void OnPartnerLocationDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl = null;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerAddressVerification.VerifyPartnerLocationData(e, FPetraUtilsObject.VerificationResultCollection,
                        out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.ResultCode != PetraErrorCodes.ERR_VALUEUNASSIGNABLE)
                    {
                        TMessages.MsgVerificationError(VerificationResultReturned, this.GetType());

// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerLocationDV], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();

                        if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                        {
                            FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                        }

                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FPetraUtilsObject.VerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        // MessageBox.Show(e.Row[e.Column.ColumnName, DataRowVersion.Current].ToString);
                        // e.ProposedValue := e.Row[e.Column.ColumnName, DataRowVersion.Current];
                        e.ProposedValue = e.Row[e.Column.ColumnName];

                        // need to assign this to make the change actually visible...
                        cmbLocationType.SetSelectedString(e.ProposedValue.ToString());
// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerLocationDV], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                    {
                        FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        /// <summary>
        /// DataBinds all fields on the UserControl, sets Status Bar Texts and hooks up
        /// an Event that allows data verification.
        ///
        /// This procedure needs to be called when the UserControl is to be DataBound for
        /// the first time.
        ///
        /// </summary>
        /// <param name="AMainDS">DataSet that contains most data that is used on the screen</param>
        /// <param name="AKey">DataTable Key value of the record to which the UserControl should
        /// be DataBound to
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(PartnerEditTDS AMainDS, System.Int32 AKey)
        {
            Binding DateFormatBinding;
            Binding NullableNumberFormatBinding;

            FMainDS = AMainDS;

            // create a DataView for each of the two DataTables
            FLocationDV = new DataView(FMainDS.PLocation);
            FPartnerLocationDV = new DataView(FMainDS.PPartnerLocation);

            // filter each DataView to show only a certain record
            FLocationDV.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + AKey.ToString();
            FPartnerLocationDV.RowFilter = PPartnerLocationTable.GetLocationKeyDBName() + " = " + AKey.ToString();

            // DataBind the controls to the DataViews
            this.txtLocality.DataBindings.Add("Text", FLocationDV, PLocationTable.GetLocalityDBName());
            this.txtStreetName.DataBindings.Add("Text", FLocationDV, PLocationTable.GetStreetNameDBName());
            this.txtAddLine3.DataBindings.Add("Text", FLocationDV, PLocationTable.GetAddress3DBName());
            this.txtCity.DataBindings.Add("Text", FLocationDV, PLocationTable.GetCityDBName());
            this.txtCounty.DataBindings.Add("Text", FLocationDV, PLocationTable.GetCountyDBName());
            this.txtPostalCode.DataBindings.Add("Text", FLocationDV, PLocationTable.GetPostalCodeDBName());
            this.chkMailingAddress.DataBindings.Add("Checked", FPartnerLocationDV, PPartnerLocationTable.GetSendMailDBName());
            this.txtTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetTelephoneNumberDBName());
            this.txtFaxNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetFaxNumberDBName());
            this.txtAltTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetAlternateTelephoneDBName());
            this.txtMobileTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetMobileNumberDBName());
            this.txtEmail.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetEmailAddressDBName());
            this.txtURL.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetUrlDBName());
            NullableNumberFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetExtensionDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber_2);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtTelephoneExt.DataBindings.Add(NullableNumberFormatBinding);
            NullableNumberFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetFaxExtensionDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber_2);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtFaxExt.DataBindings.Add(NullableNumberFormatBinding);
            DateFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetDateEffectiveDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtValidFrom.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetDateGoodUntilDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtValidTo.DataBindings.Add(DateFormatBinding);


            // DataBind AutoPopulatingComboBoxes

            //When these controls are bound, many strange problems happen with
            //data-binding. Towards the goal of not using databind for detail view,
            //we now don't databind these two.


            cmbCountry.InitialiseUserControl();
            cmbCountry.AddNotSetRow("--", "NOT SET");

            cmbLocationType.InitialiseUserControl();

            btnCreatedLocation.UpdateFields(FLocationDV);
            btnCreatedPartnerLocation.UpdateFields(FPartnerLocationDV);

            // Extender Provider
            this.expTextBoxStringLengthCheckPartnerAddress.RetrieveTextboxes(this);

            // Set StatusBar Texts
            // TODO: translate help from database
#if TODO
            FPetraUtilsObject.SetStatusBarText(txtLocality, PLocationTable.GetLocalityHelp());
            FPetraUtilsObject.SetStatusBarText(txtStreetName, PLocationTable.GetStreetNameHelp());
            FPetraUtilsObject.SetStatusBarText(txtAddLine3, PLocationTable.GetAddress3Help());
            FPetraUtilsObject.SetStatusBarText(txtCity, PLocationTable.GetCityHelp());
            FPetraUtilsObject.SetStatusBarText(txtCounty, PLocationTable.GetCountyHelp());
            FPetraUtilsObject.SetStatusBarText(txtPostalCode, PLocationTable.GetPostalCodeHelp());
            FPetraUtilsObject.SetStatusBarText(cmbCountry, PLocationTable.GetCountryCodeHelp());
            FPetraUtilsObject.SetStatusBarText(cmbLocationType, PPartnerLocationTable.GetLocationTypeHelp());
            FPetraUtilsObject.SetStatusBarText(chkMailingAddress, PPartnerLocationTable.GetSendMailHelp());
            FPetraUtilsObject.SetStatusBarText(txtTelephoneNo, PPartnerLocationTable.GetTelephoneNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtFaxNo, PPartnerLocationTable.GetFaxNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtAltTelephoneNo, PPartnerLocationTable.GetAlternateTelephoneHelp());
            FPetraUtilsObject.SetStatusBarText(txtMobileTelephoneNo, PPartnerLocationTable.GetMobileNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtEmail, PPartnerLocationTable.GetEmailAddressHelp());
            FPetraUtilsObject.SetStatusBarText(txtURL, PPartnerLocationTable.GetUrlHelp());
            FPetraUtilsObject.SetStatusBarText(txtTelephoneExt, PPartnerLocationTable.GetExtensionHelp());
            FPetraUtilsObject.SetStatusBarText(txtFaxExt, PPartnerLocationTable.GetFaxExtensionHelp());
            FPetraUtilsObject.SetStatusBarText(txtValidFrom, PPartnerLocationTable.GetDateEffectiveHelp());
            FPetraUtilsObject.SetStatusBarText(txtValidTo, PPartnerLocationTable.GetDateGoodUntilHelp());
#endif

            // hook events that allow data verification
            FMainDS.PPartnerLocation.ColumnChanging += new DataColumnChangeEventHandler(this.OnPartnerLocationDataColumnChanging);
        }

        /// <summary>
        /// Refreshes the data that is displayed in DataBound fields with changed data
        /// from the underlying DataTable.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RefreshDataBoundFields()
        {
            System.Windows.Forms.CurrencyManager LocationsCurrencyManager;
            System.Windows.Forms.CurrencyManager PartnerLocationsCurrencyManager;
            LocationsCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FLocationDV];
            PartnerLocationsCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FPartnerLocationDV];
            LocationsCurrencyManager.Refresh();
            PartnerLocationsCurrencyManager.Refresh();
        }

        /// <summary>
        /// Sets up the Localised County Label and hooks up a data change Event.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitUserControl()
        {
            String LocalisedCountyLabel;
            String Dummy;

            // Use Localised String for County Label
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Dummy);
            lblCounty.Text = LocalisedCountyLabel;

            // Hook up data change events
            cmbCountry.SelectedValueChanged += new System.EventHandler(this.CmbCountry_SelectedValueChanged);
        }

        /// <summary>
        /// DataBinds all fields on the UserControl to a different record.
        ///
        /// This procedure can be called when the UserControl should be DataBound to a
        /// different record (after the first time).
        ///
        /// </summary>
        /// <param name="AKey">DataTable Key value of the record to which the UserControl
        /// should be DataBound to
        /// </param>
        /// <returns>void</returns>
        public void UpdateDataBinding(Int32 AKey)
        {
            FKey = AKey;

            if (FLocationDV != null)
            {
                // filter each DataView to show only a certain record
                FLocationDV.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + FKey.ToString();
                FPartnerLocationDV.RowFilter = PPartnerLocationTable.GetLocationKeyDBName() + " = " + FKey.ToString();
                btnCreatedLocation.UpdateFields(FLocationDV);
                btnCreatedPartnerLocation.UpdateFields(FPartnerLocationDV);

                cmbLocationType.SetSelectedString(((PPartnerLocationRow)FPartnerLocationDV[0].Row).LocationType);

                if (((PLocationRow)FLocationDV[0].Row).IsCountryCodeNull())
                {
                    cmbCountry.SelectNotSetRow();
                }
                else
                {
                    cmbCountry.SelectedValue = ((PLocationRow)FLocationDV[0].Row).CountryCode;
                }
            }
        }

        private void DataSavingStartedHandler(System.Object obj, EventArgs e)
        {
            SaveUnboundControlData();
        }

        /// <summary>
        /// Used to store the values of any unbound controls back to the dataviews.
        /// </summary>
        public void SaveUnboundControlData()
        {
            if (cmbCountry.NoValueSet())
            {
                ((PLocationRow)FLocationDV[0].Row).SetCountryCodeNull();
            }
            else
            {
                ((PLocationRow)FLocationDV[0].Row).CountryCode = cmbCountry.SelectedValue.ToString();
            }

            if (cmbLocationType.SelectedIndex == -1)
            {
                ((PPartnerLocationRow)FPartnerLocationDV[0].Row).SetLocationTypeNull();
            }
            else
            {
                ((PPartnerLocationRow)FPartnerLocationDV[0].Row).LocationType = cmbLocationType.GetSelectedString();
            }
        }

        /// <summary>
        /// Switches between 'Read Only Mode' and 'Edit Mode' of the UserControl.
        ///
        /// </summary>
        /// <param name="ADataMode">Specify dmBrowse for read-only mode or dmEdit for edit mode.
        /// </param>
        /// <returns>void</returns>
        public void SetMode(TDataModeEnum ADataMode)
        {
            // messagebox.show('SetMode (' + aDataMode.ToString("G") + ')');
            if (ADataMode == TDataModeEnum.dmBrowse)
            {
                CustomEnablingDisabling.DisableControlGroup(grpAddress, @HandleDisabledControlClick);
                CustomEnablingDisabling.DisableControlGroup(grpPartnerLocation, @HandleDisabledControlClick);
            }
            else
            {
                CustomEnablingDisabling.EnableControlGroup(grpAddress);
                CustomEnablingDisabling.EnableControlGroup(grpPartnerLocation);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AdjustLabelControlsAfterResizing()
        {
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpAddress);
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpPartnerLocation);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleDisabledControlClick(System.Object sender, System.EventArgs e)
        {
            if (FDelegateDisabledControlClick != null)
            {
                // Call the Delegate in the Control that instantiated this Control
                FDelegateDisabledControlClick(sender, e);

                // Set the Focus on the clicked control (it is stored in the senders (=Label's) Tag Property...)
                ((Control)((Control)sender).Tag).Focus();
            }
        }
    }
}