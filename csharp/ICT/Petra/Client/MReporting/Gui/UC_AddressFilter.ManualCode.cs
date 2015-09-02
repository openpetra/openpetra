//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of TFrmUC_AddressFilter
    /// </summary>
    public partial class TFrmUC_AddressFilter
    {
        /// <summary>
        /// initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            // hide those fields by default
            ShowCountyStateField(false);
            ShowAddressDateFields(false);

            // make sure date fields are not initialized with today's date but later on with default settings
            dtpAddressStartFrom.Text = "";
            dtpAddressStartTo.Text = "";
            dtpAddressEndFrom.Text = "";
            dtpAddressEndTo.Text = "";

            txtPostCodeFrom.Validating += new CancelEventHandler(PostCode_Validating);
            txtPostCodeTo.Validating += new CancelEventHandler(PostCode_Validating);

            cmbRegion.cmbCombobox.AllowBlankValue = true;
            cmbCountry.cmbCombobox.AllowBlankValue = true;
        }

        /// <summary>
        /// Reads the selected values from the controls, and stores them into the parameter system of FCalculator
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_city", this.txtCity.Text);
            ACalc.AddParameter("param_county", this.txtCounty.Text);
            ACalc.AddParameter("param_postcode_from", this.txtPostCodeFrom.Text);
            ACalc.AddParameter("param_postcode_to", this.txtPostCodeTo.Text);
            ACalc.AddParameter("param_region", this.cmbRegion.GetSelectedString());
            ACalc.AddParameter("param_country", this.cmbCountry.GetSelectedString());

            // manually add validity date here until we introduce a checkbox and/or date field in control
            ACalc.AddParameter("param_only_addresses_valid_on", this.chkCurrentAddressesOnly.Checked);
            ACalc.AddParameter("param_address_date_valid_on", DateTime.Today);

            if (!chkCurrentAddressesOnly.Checked)
            {
                // make sure that date values are only added as parameters if fields are visible
                if (dtpAddressStartFrom.Visible)
                {
                    ACalc.AddParameter("param_address_start_from", this.dtpAddressStartFrom.Date);
                }

                if (dtpAddressStartTo.Visible)
                {
                    ACalc.AddParameter("param_address_start_to", this.dtpAddressStartTo.Date);
                }

                if (dtpAddressEndFrom.Visible)
                {
                    ACalc.AddParameter("param_address_end_from", this.dtpAddressEndFrom.Date);
                }

                if (dtpAddressEndTo.Visible)
                {
                    ACalc.AddParameter("param_address_end_to", this.dtpAddressEndTo.Date);
                }
            }
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            txtCity.Text = AParameters.Get("param_city").ToString();
            txtCounty.Text = AParameters.Get("param_county").ToString();
            txtPostCodeFrom.Text = AParameters.Get("param_postcode_from").ToString();
            txtPostCodeTo.Text = AParameters.Get("param_postcode_to").ToString();
            cmbRegion.SetSelectedString(AParameters.Get("param_region").ToString(), -1);
            cmbCountry.SetSelectedString(AParameters.Get("param_country").ToString(), -1);

            chkCurrentAddressesOnly.Checked = AParameters.Get("param_only_addresses_valid_on").ToBool();
            dtpAddressStartFrom.Date = AParameters.Get("param_address_start_from").ToDate();
            dtpAddressStartTo.Date = AParameters.Get("param_address_start_to").ToDate();
            dtpAddressEndFrom.Date = AParameters.Get("param_address_end_from").ToDate();
            dtpAddressEndTo.Date = AParameters.Get("param_address_end_to").ToDate();

            EnableDisableDateFields();

            if (!cmbRegion.Table.Rows.Contains(new object[] { "" }))
            {
                // add a blank row to the combobox
                DataRow BlankRow = cmbRegion.Table.NewRow();
                BlankRow[PPostcodeRegionTable.GetRegionDBName()] = "";
                cmbRegion.Table.Rows.InsertAt(BlankRow, 0);
            }

            cmbRegion.cmbCombobox.SelectedIndex = 0;
        }

        /// <summary>
        /// This will add functions to the list of available functions
        /// </summary>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            //TODO
        }

        /// <summary>
        /// hide/show field for "County/State"
        /// </summary>
        public void ShowCountyStateField(bool AShow)
        {
            lblCounty.Visible = AShow;
            txtCounty.Visible = AShow;
        }

        /// <summary>
        /// hide/show fields for address valid from/to
        /// </summary>
        public void ShowAddressDateFields(bool AShow)
        {
            lblAddressStartFrom.Visible = AShow;
            dtpAddressStartFrom.Visible = AShow;
            lblAddressStartTo.Visible = AShow;
            dtpAddressStartTo.Visible = AShow;
            lblAddressEndFrom.Visible = AShow;
            dtpAddressEndFrom.Visible = AShow;
            lblAddressEndTo.Visible = AShow;
            dtpAddressEndTo.Visible = AShow;
        }

        private void OnCurrentAddressBoxChecked(object sender, EventArgs e)
        {
            EnableDisableDateFields();
        }

        private void EnableDisableDateFields()
        {
            bool SetEnabled = !chkCurrentAddressesOnly.Checked;

            // if checkbox for "current addresses only" is ticked then disable date fields
            lblAddressStartFrom.Enabled = SetEnabled;
            dtpAddressStartFrom.Enabled = SetEnabled;
            lblAddressStartTo.Enabled = SetEnabled;
            dtpAddressStartTo.Enabled = SetEnabled;
            lblAddressEndFrom.Enabled = SetEnabled;
            dtpAddressEndFrom.Enabled = SetEnabled;
            lblAddressEndTo.Enabled = SetEnabled;
            dtpAddressEndTo.Enabled = SetEnabled;
        }

        private void PostCode_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtPostCodeFrom.Text = txtPostCodeFrom.Text.ToUpper();
            txtPostCodeTo.Text = txtPostCodeTo.Text.ToUpper();
        }
    }
}