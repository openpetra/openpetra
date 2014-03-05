//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// A UserControl that consists of a ComboBox with Country Code entries and a
    /// label to its right that displays the name of the country.
    ///
    /// Interesting feature: this control fetches its list entries on its own from
    /// the Client DataCache!
    ///
    /// @Comment Have a look at the AddressTab_UserControl to see how DataBinding
    ///          works with this UserControl (it's quite easy!)
    ///          Note: DataBinding is currently specific to the 'Locations'
    ///          DataTable but can easily be extended other DataTables.
    ///          Important: Always DataBind this UserControl to a DataColumn of a
    ///          DataTable - otherwise a changed Country selection will not be
    ///          automatically reflected in a DataTable/DataSet!
    /// </summary>
    public partial class TUC_CountryLabelledComboBox : System.Windows.Forms.UserControl
    {
        private DataTable FDataCache_CountryListTable;
        private bool FDataLoading;
        private string FText;
        private Boolean FUserControlInitialised;

        /// <summary>todoComment</summary>
        public new String Text
        {
            get
            {
                return FText;
            }
        }

        /// <summary>returns true if the ComboBox in the user control is DroppedDown</summary>
        public bool DroppedDown
        {
            get
            {
                return this.cmbCountry.DroppedDown;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_CountryLabelledComboBox()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblCountryName.Text = Catalog.GetString("Country Name");
            #endregion
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            FDataCache_CountryListTable = TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CountryList);
            FDataLoading = true;
            cmbCountry.BeginUpdate();
            cmbCountry.DataSource = FDataCache_CountryListTable.DefaultView;
            cmbCountry.DisplayMember = "p_country_code_c";
            cmbCountry.ValueMember = "p_country_code_c";
            cmbCountry.EndUpdate();
            FDataLoading = false;
            lblCountryName.Text = "";
            cmbCountry.SelectedItem = null;

            /*
             * dummyCountries:= new ArrayList();
             * dummyCountries.Add('AT');
             * dummyCountries.Add('NL');
             * dummyCountries.Add('DE');
             *
             * cmbCountry.DataSource := dummyCountries;
             */
            FUserControlInitialised = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataTable"></param>
        /// <param name="AColumnName"></param>
        public void PerformDataBinding(DataTable ADataTable, String AColumnName)
        {
            if (!FUserControlInitialised)
            {
                InitialiseUserControl();
            }

            cmbCountry.DataBindings.Add("SelectedValue", ADataTable, AColumnName);
            UpdateCountryLabel();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataView"></param>
        /// <param name="AColumnName"></param>
        public void PerformDataBinding(DataView ADataView, String AColumnName)
        {
            if (!FUserControlInitialised)
            {
                InitialiseUserControl();
            }

            cmbCountry.DataBindings.Add("SelectedValue", ADataView, AColumnName);
            UpdateCountryLabel();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="multiTableDS"></param>
        /// <param name="LocationKey"></param>
        public void PerformDataBinding(DataSet multiTableDS, System.Int32 LocationKey)
        {
            DataView dvLocations;

            // countryIndex: System.Int32;
            // dummyCountries: ArrayList;
            // countryColumn: DataColumn;
            if (!FUserControlInitialised)
            {
                InitialiseUserControl();
            }

            // create a DataView
            dvLocations = new DataView(multiTableDS.Tables["Locations"]);

            // filter DataView to contain only a certain record
            dvLocations.RowFilter = "p_location_key_i = " + LocationKey.ToString();

            // MessageBox.Show( dvLocations.Count.ToString);
            // countryColumn := dvLocations[0]['p_country_code_c'];
            cmbCountry.DataBindings.Add("SelectedValue", dvLocations, "p_country_code_c");

            // Select the country in the ComboBox
            // countryIndex := cmbCountry.FindString(dvLocations[0]['p_country_code_c'].ToString);
            // cmbCountry.SelectedIndex := countryIndex;
            UpdateCountryLabel();
        }

        private void UpdateCountryLabel()
        {
            string selectedCountry;
            DataView dvCountry;

            if ((cmbCountry.SelectedIndex != -1) && (!FDataLoading))
            {
                selectedCountry = cmbCountry.SelectedValue.ToString();

                // messagebox.show( selectedCountry );
                // create a DataView
                dvCountry = new DataView(FDataCache_CountryListTable);
                dvCountry.RowFilter = "p_country_code_c = '" + selectedCountry + "'";

                // MessageBox.Show( dvCountry.Count.ToString);
                lblCountryName.Text = dvCountry[0]["p_country_name_c"].ToString();
                cmbCountry.Text = dvCountry[0]["p_country_code_c"].ToString();
                FText = dvCountry[0]["p_country_code_c"].ToString() + "          " + dvCountry[0]["p_country_name_c"].ToString();
            }
            else if (cmbCountry.SelectedIndex == -1)
            {
                lblCountryName.Text = "";
            }
        }

        // procedure TCountry_DropDown_UserControl.cmbCountry_TextChanged(sender: System.Object;
        // e: System.EventArgs);
        // begin
        /// <summary>
        /// /  messagebox.show( 'Text changed!'); */
        /// </summary>
        /// <returns>void</returns>
        private void CmbCountry_Leave(System.Object sender, System.EventArgs e)
        {
            // messagebox.show('Leaving cmbCountry!');
            cmbCountry.Text = cmbCountry.Text.ToUpper();
        }

        private void CmbCountry_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            UpdateCountryLabel();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CustomDisable()
        {
            CustomEnablingDisabling.DisableControl(this, cmbCountry);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CustomEnable()
        {
            CustomEnablingDisabling.EnableControl(this, cmbCountry);
        }
    }
}