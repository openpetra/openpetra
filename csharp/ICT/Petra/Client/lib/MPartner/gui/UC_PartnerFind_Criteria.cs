/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner;
using System.Globalization;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Common;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl that automatically arranges panels with Find Criteria for
    /// Partner Find.
    /// Allows Find Criteria to be added, removed and moved up, down, left, right
    /// in conjunction with the PartnerFind_Options screen.
    /// </summary>
    public class TUC_PartnerFindCriteria : System.Windows.Forms.UserControl, IPetraUserControl
    {
        private const String StrSpacer = "Spacer";
        private const String StrBeginGroup = "BeginGroup";
        private const String StrPartnerClassFindHelpText = "Restricts search to Partners of specified Partner Class (* = all Classes)";
        private const String StrWorkerFamilyOnlyFindHelpText = "Filters the search to Worker Families";
        private const String StrPreviousNameFindHelpText = "Enter a Previous Name (eg. Maiden Name, old name of an Organisation or Church)";
        private const String StrPartnerKeyNonExactInfoTest = "Trailing zero(es) in the Partner Key are used as wilcard characters.\n\r" +
                                                             "Example: Entering Partner Key '0029000000' will return all Partners whose Partner Key was generated\n\r"
                                                             +
                                                             "for Unit 0029000000 (Netherlands).\n\r" +
                                                             "This is because the 'Exact Partner Key Match' Option is turned off in the Partner Find Options.";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlLeftColumn;
        private System.Windows.Forms.Panel pnlRightColumn;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.Panel pnlPartnerName;
        private System.Windows.Forms.Panel pnlPersonalName;
        private System.Windows.Forms.TextBox txtPersonalName;
        private System.Windows.Forms.Label lblPersonalName;
        private System.Windows.Forms.Panel pnlPartnerClass;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.ComboBox cmbPartnerClass;
        private System.Windows.Forms.Panel pnlPartnerKey;

        /// <summary>System.Windows.Forms.TextBox;</summary>
        private TTxtMaskedTextBox txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblPartnerKeyNonExactMatch;
        private System.Windows.Forms.Panel pnlPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.Panel pnlCounty;
        private System.Windows.Forms.Label lblCounty;
        private System.Windows.Forms.Panel pnlAddress1;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Panel pnlCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Panel pnlCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Panel pnlPostCode;
        private System.Windows.Forms.Label lblPostCode;
        private System.Windows.Forms.TextBox txtPostCode;
        private System.Windows.Forms.Panel pnlPartnerStatus;
        private System.Windows.Forms.Label lblPartnerStatus;
        private System.Windows.Forms.RadioButton rbtStatusActive;
        private System.Windows.Forms.RadioButton rbtStatusAll;
        private System.Windows.Forms.Panel pnlMailingAddressOnly;
        private System.Windows.Forms.Label lblMailingAddressOnly;
        private System.Windows.Forms.CheckBox chkMailingAddressOnly;
        private System.Windows.Forms.Panel pnlPersonnelCriteria;
        private TUC_PartnerFind_PersonnelCriteria_CollapsiblePart ucoPartnerFind_PersonnelCriteria_CollapsiblePart;
        private System.Data.DataSet FFindCriteriaDataSet;
        private System.Data.DataTable FFindCriteriaDataTable;
        private System.Data.DataColumn PartnerNameDataColumn;
        private System.Data.DataColumn PersonalNameDataColumn;
        private System.Data.DataColumn PreviousNameDataColumn;
        private System.Data.DataColumn Address1DataColumn;
        private System.Data.DataColumn CityDataColumn;
        private System.Data.DataColumn PostCodeDataColumn;
        private System.Data.DataColumn CountyDataColumn;
        private System.Data.DataColumn CountryDataColumn;
        private System.Data.DataColumn MailingAddressOnlyDataColumn;
        private System.Data.DataColumn PartnerClassDataColumn;
        private System.Data.DataColumn PartnerKeyDataColumn;
        private System.Data.DataColumn PartnerStatusDataColumn;
        private System.Windows.Forms.TextBox txtCounty;
        private TUC_CountryComboBox ucoCountryComboBox;
        private SplitButton critPartnerName;
        private SplitButton critPersonalName;
        private SplitButton critPreviousName;
        private SplitButton critAddress1;
        private SplitButton critCity;
        private SplitButton critPostCode;
        private SplitButton critCounty;
        private System.Data.DataColumn PartnerNameMatchColumn;
        private System.Windows.Forms.Panel pnlEmail;
        private SplitButton critEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Data.DataColumn EmailDataColumn;
        private System.Data.DataColumn EmailMatchColumn;
        private System.Windows.Forms.Panel pnlAddress2;
        private SplitButton critAddress2;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.Label lblAddress2;
        private System.Data.DataColumn Address2DataColumn;
        private System.Data.DataColumn Address2Match;
        private System.Data.DataColumn Address3Column;
        private System.Data.DataColumn Address3MatchColumn;
        private System.Windows.Forms.Panel pnlAddress3;
        private SplitButton critAddress3;
        private System.Windows.Forms.TextBox txtAddress3;
        private System.Windows.Forms.Label lblAddress3;
        private System.Windows.Forms.Panel pnlLocationKey;
        private System.Windows.Forms.TextBox txtLocationKey;
        private System.Data.DataColumn LocationKeyColumn;
        private System.Data.DataColumn PersonalNameMatchColumn;
        private System.Data.DataColumn PreviousNameMatchColumn;
        private System.Data.DataColumn Address1Match;
        private System.Data.DataColumn PostCodeMatchColumn;
        private System.Data.DataColumn CountyMatchColumn;
        private System.Data.DataColumn CityMatchColumn;
        private System.Windows.Forms.CheckBox chkWorkerFamOnly;
        private System.Data.DataColumn WorkerFamOnlyColumn;
        private System.Data.DataColumn ExactPartnerKeyMatch;
        private System.Data.DataColumn PhoneNumberColumn;
        private System.Data.DataColumn PhoneNumberMatchColumn;
        private System.Windows.Forms.Panel pnlPhoneNumber;
        private SplitButton critPhoneNumber;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Button btnLocationKey;
        private System.Windows.Forms.RadioButton rbtPrivate;
        private System.Windows.Forms.ToolTip tipUC;

        /// <summary>Private Declarations</summary>
        private ArrayList FCriteriaFieldsLeft;
        private ArrayList FCriteriaFieldsRight;
        private Boolean FCriteriaSetupMode;
        private TSelectedCriteriaPanel FSelectedPanel;
        private DataTable FPartnerClassDataTable;
        private Boolean FWorkerFamOnly;
        private string[] FRestrictedParterClasses;
        private String FDefaultPartnerClass;
        private DataRow FDefaultValues;

// TODO        private int FPreviousSelectedPartnerClass;
        private Boolean FShowAllPartnerClasses;

        private string PartnerStatus
        {
            get
            {
                string ReturnValue = "";

                if (rbtStatusActive.Checked)
                {
                    ReturnValue = "ACTIVE";
                }

                if (rbtStatusAll.Checked)
                {
                    ReturnValue = "ALL";
                }

                if (rbtPrivate.Checked)
                {
                    ReturnValue = "PRIVATE";
                }

                return ReturnValue;
            }

            set
            {
                // start off with default colours
                rbtStatusAll.BackColor = System.Drawing.SystemColors.Control;
                rbtPrivate.BackColor = System.Drawing.SystemColors.Control;

                if (value == "ACTIVE")
                {
                    rbtStatusActive.Checked = true;
                }

                if (value == "ALL")
                {
                    rbtStatusAll.Checked = true;
                    rbtStatusAll.BackColor = System.Drawing.Color.PeachPuff;
                }

                if (value == "PRIVATE")
                {
                    rbtPrivate.Checked = true;
                    rbtPrivate.BackColor = System.Drawing.Color.PeachPuff;
                }

                try
                {
                    FFindCriteriaDataSet.Tables[0].Rows[0]["PartnerStatus"] = value;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>todoComment</summary>
        public ArrayList CriteriaFieldsLeft
        {
            get
            {
                return FCriteriaFieldsLeft;
            }

            set
            {
                FCriteriaFieldsLeft = value;
            }
        }

        /// <summary>todoComment</summary>
        public ArrayList CriteriaFieldsRight
        {
            get
            {
                return FCriteriaFieldsRight;
            }

            set
            {
                FCriteriaFieldsRight = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean CriteriaSetupMode
        {
            get
            {
                return FCriteriaSetupMode;
            }

            set
            {
                FCriteriaSetupMode = value;

                // MessageBox.Show('CriteriaSetupMode: ' + FCriteriaSetupMode.ToString);
                if (FCriteriaSetupMode == true)
                {
                    // CustomEnablingDisabling.DisableControlGroup(pnlLeftColumn);
                    // CustomEnablingDisabling.DisableControlGroup(pnlRightColumn);
                    CustomDisablePanels(pnlLeftColumn);
                    CustomDisablePanels(pnlRightColumn);
                }
                else
                {
                }

                // CustomEnablingDisabling.EnableControlGroup(pnlLeftColumn);
                // CustomEnablingDisabling.EnableControlGroup(pnlRightColumn);
            }
        }

        /// <summary>todoComment</summary>
        public DataTable CriteriaData
        {
            get
            {
                // MessageBox.Show('PartnerName: ' + FFindCriteriaDataTable.Rows[0]['PartnerName'].ToString);
                return FFindCriteriaDataTable;
            }
        }

        /// Event that fires when one of the SearchCriteria's contents is changed.following line appeers to be brokne by the disigner regularlycorrect line is: property OnCriteriaContentChanged: System.EventHandler add OnCriteriaContentChanged remove
        /// <summary>OnCriteriaContentChanged;</summary>
        public event System.EventHandler OnCriteriaContentChanged;

        /// <summary>todoComment</summary>
        public String[] RestrictedPartnerClass
        {
            get
            {
                return FRestrictedParterClasses;
            }

            set
            {
                DataRow PartnerClassDataRow;
                String tmpString;
                Boolean miWorkerFamOnly;

                // MessageBox.Show('set_RestrictedPartnerClass called');
                FRestrictedParterClasses = value;

                // this flag is set to true IF the FIRST item in list will be WORKER-FAM
                miWorkerFamOnly = false;

                // are there any restrictions specified?
                if ((value != null) && (value.Length > 0))
                {
                    // clear table combo is bound to , and start again
                    FPartnerClassDataTable.Rows.Clear();
                    tmpString = value[0];

                    if (tmpString.IndexOf("WORKER-FAM") >= 0)
                    {
                        // first item selected will be WORKER-FAM
                        miWorkerFamOnly = true;
                    }

                    tmpString = tmpString.Replace("WORKER-FAM", "FAMILY");
                    FDefaultPartnerClass = tmpString;

                    foreach (String eachPart in value)
                    {
                        // .Split(new (array [] of Char, (','))) do

                        // MessageBox.Show('eachPart: ' + eachPart);
                        if (eachPart == "WORKER-FAM")
                        {
                            // add FAMILY Row has own special case
                            PartnerClassDataRow = FPartnerClassDataTable.NewRow();

                            // Add FAMILY not WORKER-FAM
                            PartnerClassDataRow["PartnerClass"] = "FAMILY";
                            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);

                            // set the flag, so combo box handler knows this is the case
                            FWorkerFamOnly = true;
                        }
                        else
                        {
                            // just add item
                            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                            PartnerClassDataRow["PartnerClass"] = eachPart;
                            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                        }
                    }

                    // end for each
                    // ensure top choice is databound
                    FFindCriteriaDataTable.Rows[0]["PartnerClass"] = FDefaultPartnerClass;
                    FFindCriteriaDataTable.Rows[0]["WORKERFAMONLY"] = miWorkerFamOnly;
                    cmbPartnerClass.ResetBindings();
                }

                if (cmbPartnerClass.Items.Count > 0)
                {
                    cmbPartnerClass.SelectedIndex = 0;

                    // this ensures that the checkbox is visible if needed
                    // and disabled if needed
                    // and checked if needed
                    HandlePartnerClassGui();
                }
            }
        }

        /// <summary>todoComment</summary>
        public event FindCriteriaSelectionChangedHandler FindCriteriaSelectionChanged;

        #region Windows Form Designer generated code

        /// <summary>
        /// GridPanel1: GridPanel.GridPanel; <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this
        /// method with the code editor. </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.FFindCriteriaDataTable = new System.Data.DataTable();
            this.PartnerNameDataColumn = new System.Data.DataColumn();
            this.PartnerNameMatchColumn = new System.Data.DataColumn();
            this.PersonalNameDataColumn = new System.Data.DataColumn();
            this.PersonalNameMatchColumn = new System.Data.DataColumn();
            this.PreviousNameDataColumn = new System.Data.DataColumn();
            this.PreviousNameMatchColumn = new System.Data.DataColumn();
            this.Address1DataColumn = new System.Data.DataColumn();
            this.Address1Match = new System.Data.DataColumn();
            this.CityDataColumn = new System.Data.DataColumn();
            this.CityMatchColumn = new System.Data.DataColumn();
            this.PostCodeDataColumn = new System.Data.DataColumn();
            this.PostCodeMatchColumn = new System.Data.DataColumn();
            this.CountyDataColumn = new System.Data.DataColumn();
            this.CountyMatchColumn = new System.Data.DataColumn();
            this.CountryDataColumn = new System.Data.DataColumn();
            this.MailingAddressOnlyDataColumn = new System.Data.DataColumn();
            this.PartnerClassDataColumn = new System.Data.DataColumn();
            this.PartnerKeyDataColumn = new System.Data.DataColumn();
            this.PartnerStatusDataColumn = new System.Data.DataColumn();
            this.EmailDataColumn = new System.Data.DataColumn();
            this.EmailMatchColumn = new System.Data.DataColumn();
            this.Address2DataColumn = new System.Data.DataColumn();
            this.Address2Match = new System.Data.DataColumn();
            this.Address3Column = new System.Data.DataColumn();
            this.Address3MatchColumn = new System.Data.DataColumn();
            this.LocationKeyColumn = new System.Data.DataColumn();
            this.WorkerFamOnlyColumn = new System.Data.DataColumn();
            this.ExactPartnerKeyMatch = new System.Data.DataColumn();
            this.PhoneNumberColumn = new System.Data.DataColumn();
            this.PhoneNumberMatchColumn = new System.Data.DataColumn();
            this.FFindCriteriaDataSet = new System.Data.DataSet();
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart = new Ict.Petra.Client.MPartner.TUC_PartnerFind_PersonnelCriteria_CollapsiblePart();
            this.spcCriteria = new System.Windows.Forms.SplitContainer();
            this.pnlRightColumn = new System.Windows.Forms.Panel();
            this.pnlLocationKey = new System.Windows.Forms.Panel();
            this.btnLocationKey = new System.Windows.Forms.Button();
            this.txtLocationKey = new System.Windows.Forms.TextBox();
            this.pnlPartnerClass = new System.Windows.Forms.Panel();
            this.chkWorkerFamOnly = new System.Windows.Forms.CheckBox();
            this.cmbPartnerClass = new System.Windows.Forms.ComboBox();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.pnlPartnerKey = new System.Windows.Forms.Panel();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtMaskedTextBox(this.components);
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblPartnerKeyNonExactMatch = new System.Windows.Forms.Label();
            this.pnlPartnerStatus = new System.Windows.Forms.Panel();
            this.rbtPrivate = new System.Windows.Forms.RadioButton();
            this.rbtStatusActive = new System.Windows.Forms.RadioButton();
            this.rbtStatusAll = new System.Windows.Forms.RadioButton();
            this.lblPartnerStatus = new System.Windows.Forms.Label();
            this.pnlPersonnelCriteria = new System.Windows.Forms.Panel();
            this.pnlLeftColumn = new System.Windows.Forms.Panel();
            this.pnlPhoneNumber = new System.Windows.Forms.Panel();
            this.critPhoneNumber = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.pnlAddress3 = new System.Windows.Forms.Panel();
            this.critAddress3 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress3 = new System.Windows.Forms.TextBox();
            this.lblAddress3 = new System.Windows.Forms.Label();
            this.pnlAddress2 = new System.Windows.Forms.Panel();
            this.critAddress2 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.pnlEmail = new System.Windows.Forms.Panel();
            this.critEmail = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.pnlPartnerName = new System.Windows.Forms.Panel();
            this.critPartnerName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.pnlPersonalName = new System.Windows.Forms.Panel();
            this.critPersonalName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPersonalName = new System.Windows.Forms.TextBox();
            this.lblPersonalName = new System.Windows.Forms.Label();
            this.pnlPreviousName = new System.Windows.Forms.Panel();
            this.critPreviousName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.pnlAddress1 = new System.Windows.Forms.Panel();
            this.critAddress1 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.pnlPostCode = new System.Windows.Forms.Panel();
            this.critPostCode = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblPostCode = new System.Windows.Forms.Label();
            this.txtPostCode = new System.Windows.Forms.TextBox();
            this.pnlCity = new System.Windows.Forms.Panel();
            this.critCity = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.pnlCounty = new System.Windows.Forms.Panel();
            this.critCounty = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtCounty = new System.Windows.Forms.TextBox();
            this.lblCounty = new System.Windows.Forms.Label();
            this.pnlCountry = new System.Windows.Forms.Panel();
            this.ucoCountryComboBox = new Ict.Petra.Client.CommonControls.TUC_CountryComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.pnlMailingAddressOnly = new System.Windows.Forms.Panel();
            this.chkMailingAddressOnly = new System.Windows.Forms.CheckBox();
            this.lblMailingAddressOnly = new System.Windows.Forms.Label();
            this.tipUC = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.FFindCriteriaDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFindCriteriaDataSet)).BeginInit();
            this.spcCriteria.Panel1.SuspendLayout();
            this.spcCriteria.Panel2.SuspendLayout();
            this.spcCriteria.SuspendLayout();
            this.pnlRightColumn.SuspendLayout();
            this.pnlLocationKey.SuspendLayout();
            this.pnlPartnerClass.SuspendLayout();
            this.pnlPartnerKey.SuspendLayout();
            this.pnlPartnerStatus.SuspendLayout();
            this.pnlLeftColumn.SuspendLayout();
            this.pnlPhoneNumber.SuspendLayout();
            this.pnlAddress3.SuspendLayout();
            this.pnlAddress2.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlPartnerName.SuspendLayout();
            this.pnlPersonalName.SuspendLayout();
            this.pnlPreviousName.SuspendLayout();
            this.pnlAddress1.SuspendLayout();
            this.pnlPostCode.SuspendLayout();
            this.pnlCity.SuspendLayout();
            this.pnlCounty.SuspendLayout();
            this.pnlCountry.SuspendLayout();
            this.pnlMailingAddressOnly.SuspendLayout();
            this.SuspendLayout();

            //
            // FFindCriteriaDataTable
            //
            this.FFindCriteriaDataTable.Columns.AddRange(new System.Data.DataColumn[] {
                    this.PartnerNameDataColumn,
                    this.PartnerNameMatchColumn,
                    this.PersonalNameDataColumn,
                    this.PersonalNameMatchColumn,
                    this.PreviousNameDataColumn,
                    this.PreviousNameMatchColumn,
                    this.Address1DataColumn,
                    this.Address1Match,
                    this.CityDataColumn,
                    this.CityMatchColumn,
                    this.PostCodeDataColumn,
                    this.PostCodeMatchColumn,
                    this.CountyDataColumn,
                    this.CountyMatchColumn,
                    this.CountryDataColumn,
                    this.MailingAddressOnlyDataColumn,
                    this.PartnerClassDataColumn,
                    this.PartnerKeyDataColumn,
                    this.PartnerStatusDataColumn,
                    this.EmailDataColumn,
                    this.EmailMatchColumn,
                    this.Address2DataColumn,
                    this.Address2Match,
                    this.Address3Column,
                    this.Address3MatchColumn,
                    this.LocationKeyColumn,
                    this.WorkerFamOnlyColumn,
                    this.ExactPartnerKeyMatch,
                    this.PhoneNumberColumn,
                    this.PhoneNumberMatchColumn
                });

            this.FFindCriteriaDataTable.TableName = "CriteriaDataTable";

            //
            // PartnerNameDataColumn
            //
            this.PartnerNameDataColumn.ColumnName = "PartnerName";

            //
            // PartnerNameMatchColumn
            //
            this.PartnerNameMatchColumn.ColumnName = "PartnerNameMatch";
            this.PartnerNameMatchColumn.DefaultValue = "BEGINS";

            //
            // PersonalNameDataColumn
            //
            this.PersonalNameDataColumn.ColumnName = "PersonalName";

            //
            // PersonalNameMatchColumn
            //
            this.PersonalNameMatchColumn.Caption = "PersonalNameMatch";
            this.PersonalNameMatchColumn.ColumnName = "PersonalNameMatch";
            this.PersonalNameMatchColumn.DefaultValue = "BEGINS";

            //
            // PreviousNameDataColumn
            //
            this.PreviousNameDataColumn.ColumnName = "PreviousName";

            //
            // PreviousNameMatchColumn
            //
            this.PreviousNameMatchColumn.Caption = "PreviousNameMatch";
            this.PreviousNameMatchColumn.ColumnName = "PreviousNameMatch";
            this.PreviousNameMatchColumn.DefaultValue = "BEGINS";

            //
            // Address1DataColumn
            //
            this.Address1DataColumn.ColumnName = "Address1";

            //
            // Address1Match
            //
            this.Address1Match.Caption = "Address1Match";
            this.Address1Match.ColumnName = "Address1Match";
            this.Address1Match.DefaultValue = "BEGINS";

            //
            // CityDataColumn
            //
            this.CityDataColumn.ColumnName = "City";

            //
            // CityMatchColumn
            //
            this.CityMatchColumn.Caption = "CityMatch";
            this.CityMatchColumn.ColumnName = "CityMatch";
            this.CityMatchColumn.DefaultValue = "BEGINS";

            //
            // PostCodeDataColumn
            //
            this.PostCodeDataColumn.ColumnName = "PostCode";

            //
            // PostCodeMatchColumn
            //
            this.PostCodeMatchColumn.Caption = "PostCodeMatch";
            this.PostCodeMatchColumn.ColumnName = "PostCodeMatch";
            this.PostCodeMatchColumn.DefaultValue = "BEGINS";

            //
            // CountyDataColumn
            //
            this.CountyDataColumn.ColumnName = "County";

            //
            // CountyMatchColumn
            //
            this.CountyMatchColumn.Caption = "CountyMatch";
            this.CountyMatchColumn.ColumnName = "CountyMatch";
            this.CountyMatchColumn.DefaultValue = "BEGINS";

            //
            // CountryDataColumn
            //
            this.CountryDataColumn.ColumnName = "Country";

            //
            // MailingAddressOnlyDataColumn
            //
            this.MailingAddressOnlyDataColumn.ColumnName = "MailingAddressOnly";
            this.MailingAddressOnlyDataColumn.DataType = typeof(bool);
            this.MailingAddressOnlyDataColumn.DefaultValue = false;

            //
            // PartnerClassDataColumn
            //
            this.PartnerClassDataColumn.ColumnName = "PartnerClass";

            //
            // PartnerKeyDataColumn
            //
            this.PartnerKeyDataColumn.ColumnName = "PartnerKey";
            this.PartnerKeyDataColumn.DataType = typeof(long);

            //
            // PartnerStatusDataColumn
            //
            this.PartnerStatusDataColumn.ColumnName = "PartnerStatus";
            this.PartnerStatusDataColumn.DefaultValue = "ACTIVE";

            //
            // EmailDataColumn
            //
            this.EmailDataColumn.ColumnName = "Email";

            //
            // EmailMatchColumn
            //
            this.EmailMatchColumn.ColumnName = "EmailMatch";
            this.EmailMatchColumn.DefaultValue = "CONTAINS";

            //
            // Address2DataColumn
            //
            this.Address2DataColumn.ColumnName = "Address2";

            //
            // Address2Match
            //
            this.Address2Match.ColumnName = "Address2Match";
            this.Address2Match.DefaultValue = "BEGINS";

            //
            // Address3Column
            //
            this.Address3Column.ColumnName = "Address3";

            //
            // Address3MatchColumn
            //
            this.Address3MatchColumn.ColumnName = "Address3Match";
            this.Address3MatchColumn.DefaultValue = "BEGINS";

            //
            // LocationKeyColumn
            //
            this.LocationKeyColumn.ColumnName = "LocationKey";

            //
            // WorkerFamOnlyColumn
            //
            this.WorkerFamOnlyColumn.ColumnName = "WorkerFamOnly";
            this.WorkerFamOnlyColumn.DataType = typeof(bool);
            this.WorkerFamOnlyColumn.DefaultValue = false;

            //
            // ExactPartnerKeyMatch
            //
            this.ExactPartnerKeyMatch.Caption = "ExactPartnerKeyMatch";
            this.ExactPartnerKeyMatch.ColumnName = "ExactPartnerKeyMatch";
            this.ExactPartnerKeyMatch.DataType = typeof(bool);
            this.ExactPartnerKeyMatch.DefaultValue = false;

            //
            // PhoneNumberColumn
            //
            this.PhoneNumberColumn.ColumnName = "PhoneNumber";

            //
            // PhoneNumberMatchColumn
            //
            this.PhoneNumberMatchColumn.Caption = "PhoneNumberMatch";
            this.PhoneNumberMatchColumn.ColumnName = "PhoneNumberMatch";

            //
            // FFindCriteriaDataSet
            //
            this.FFindCriteriaDataSet.DataSetName = "FindCriteriaDataSet";
            this.FFindCriteriaDataSet.Locale = new System.Globalization.CultureInfo("en-GB");
            this.FFindCriteriaDataSet.Tables.AddRange(new System.Data.DataTable[] {
                    this.FFindCriteriaDataTable
                });

            //
            // ucoPartnerFind_PersonnelCriteria_CollapsiblePart
            //
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.
                                                       Left) |
                                                      System.Windows.Forms.AnchorStyles.
                                                      Right)));
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.BackColor = System.Drawing.SystemColors.Control;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Caption = "Personnel Criteria";
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Location = new System.Drawing.Point(4, 0);
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Name = "ucoPartnerFind_PersonnelCriteria_CollapsiblePart";
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Size = new System.Drawing.Size(302, 126);
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.SubCaption = null;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.TabIndex = 0;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Visible = false;

            //
            // spcCriteria
            //
            this.spcCriteria.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.spcCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcCriteria.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcCriteria.Location = new System.Drawing.Point(0, 0);
            this.spcCriteria.Name = "spcCriteria";

            //
            // spcCriteria.Panel1
            //
            this.spcCriteria.Panel1.AutoScroll = true;
            this.spcCriteria.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.spcCriteria.Panel1.Controls.Add(this.pnlLeftColumn);

            //
            // spcCriteria.Panel2
            //
            this.spcCriteria.Panel2.AutoScroll = true;
            this.spcCriteria.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.spcCriteria.Panel2.Controls.Add(this.pnlRightColumn);
            this.spcCriteria.Size = new System.Drawing.Size(655, 213);
            this.spcCriteria.SplitterDistance = 326;
            this.spcCriteria.SplitterIncrement = 5;
            this.spcCriteria.SplitterWidth = 2;
            this.spcCriteria.TabIndex = 4;
            this.spcCriteria.TabStop = false;

            //
            // pnlRightColumn
            //
            this.pnlRightColumn.AutoSize = true;
            this.pnlRightColumn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlRightColumn.BackColor = System.Drawing.Color.Transparent;
            this.pnlRightColumn.Controls.Add(this.pnlLocationKey);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerClass);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerKey);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerStatus);
            this.pnlRightColumn.Controls.Add(this.pnlPersonnelCriteria);
            this.pnlRightColumn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRightColumn.Location = new System.Drawing.Point(0, 0);
            this.pnlRightColumn.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRightColumn.Name = "pnlRightColumn";
            this.pnlRightColumn.Size = new System.Drawing.Size(310, 240);
            this.pnlRightColumn.TabIndex = 2;

            //
            // pnlLocationKey
            //
            this.pnlLocationKey.Controls.Add(this.btnLocationKey);
            this.pnlLocationKey.Controls.Add(this.txtLocationKey);
            this.pnlLocationKey.Location = new System.Drawing.Point(0, 76);
            this.pnlLocationKey.Name = "pnlLocationKey";
            this.pnlLocationKey.Size = new System.Drawing.Size(304, 24);
            this.pnlLocationKey.TabIndex = 7;

            //
            // btnLocationKey
            //
            this.btnLocationKey.BackColor = System.Drawing.SystemColors.Control;
            this.btnLocationKey.Location = new System.Drawing.Point(44, 0);
            this.btnLocationKey.Name = "btnLocationKey";
            this.btnLocationKey.Size = new System.Drawing.Size(98, 23);
            this.btnLocationKey.TabIndex = 1;
            this.btnLocationKey.Text = "Location Key";
            this.btnLocationKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLocationKey.UseVisualStyleBackColor = false;
            this.btnLocationKey.Click += new System.EventHandler(this.BtnLocationKey_Click);

            //
            // txtLocationKey
            //
            this.txtLocationKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "LocationKey", true));
            this.txtLocationKey.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.txtLocationKey.Location = new System.Drawing.Point(146, 0);
            this.txtLocationKey.Name = "txtLocationKey";
            this.txtLocationKey.Size = new System.Drawing.Size(76, 21);
            this.txtLocationKey.TabIndex = 2;
            this.txtLocationKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLocationKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtLocationKey_KeyUp);

            //
            // pnlPartnerClass
            //
            this.pnlPartnerClass.Controls.Add(this.chkWorkerFamOnly);
            this.pnlPartnerClass.Controls.Add(this.cmbPartnerClass);
            this.pnlPartnerClass.Controls.Add(this.lblPartnerClass);
            this.pnlPartnerClass.Location = new System.Drawing.Point(0, 176);
            this.pnlPartnerClass.Name = "pnlPartnerClass";
            this.pnlPartnerClass.Size = new System.Drawing.Size(304, 40);
            this.pnlPartnerClass.TabIndex = 2;

            //
            // chkWorkerFamOnly
            //
            this.chkWorkerFamOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.FFindCriteriaDataTable, "WorkerFamOnly", true));
            this.chkWorkerFamOnly.Location = new System.Drawing.Point(150, 22);
            this.chkWorkerFamOnly.Name = "chkWorkerFamOnly";
            this.chkWorkerFamOnly.Size = new System.Drawing.Size(126, 17);
            this.chkWorkerFamOnly.TabIndex = 2;
            this.chkWorkerFamOnly.Text = "Worker Families O&nly";

            //
            // cmbPartnerClass
            //
            this.cmbPartnerClass.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PartnerClass", true));
            this.cmbPartnerClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartnerClass.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.cmbPartnerClass.Location = new System.Drawing.Point(146, 0);
            this.cmbPartnerClass.MaxDropDownItems = 9;
            this.cmbPartnerClass.Name = "cmbPartnerClass";
            this.cmbPartnerClass.Size = new System.Drawing.Size(124, 21);
            this.cmbPartnerClass.TabIndex = 1;
            this.cmbPartnerClass.SelectedValueChanged += new System.EventHandler(this.CmbPartnerClass_SelectedValueChanged);

            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerClass.TabIndex = 0;
            this.lblPartnerClass.Text = "Partner C&lass:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPartnerKey
            //
            this.pnlPartnerKey.Controls.Add(this.txtPartnerKey);
            this.pnlPartnerKey.Controls.Add(this.lblPartnerKey);
            this.pnlPartnerKey.Controls.Add(this.lblPartnerKeyNonExactMatch);
            this.pnlPartnerKey.Location = new System.Drawing.Point(0, 26);
            this.pnlPartnerKey.Name = "pnlPartnerKey";
            this.pnlPartnerKey.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerKey.TabIndex = 2;

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.ControlMode = Ict.Common.Controls.TMaskedTextBoxMode.PartnerKey;
            this.txtPartnerKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PartnerKey", true));
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.Location = new System.Drawing.Point(146, 0);
            this.txtPartnerKey.Mask = "##########";
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PlaceHolder = "0";
            this.txtPartnerKey.Size = new System.Drawing.Size(76, 20);
            this.txtPartnerKey.TabIndex = 1;
            this.txtPartnerKey.Text = "0000000000";
            this.txtPartnerKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPartnerKey_KeyUp);

            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerKey.TabIndex = 0;
            this.lblPartnerKey.Text = "Partner &Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerKeyNonExactMatch
            //
            this.lblPartnerKeyNonExactMatch.Font = new System.Drawing.Font("Verdana", 6.5F);
            this.lblPartnerKeyNonExactMatch.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPartnerKeyNonExactMatch.Location = new System.Drawing.Point(225, 2);
            this.lblPartnerKeyNonExactMatch.Name = "lblPartnerKeyNonExactMatch";
            this.lblPartnerKeyNonExactMatch.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerKeyNonExactMatch.TabIndex = 0;
            this.lblPartnerKeyNonExactMatch.Text = "(trailing 0 = --*)";
            this.lblPartnerKeyNonExactMatch.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPartnerKeyNonExactMatch.Visible = false;
            this.tipUC.SetToolTip(this.lblPartnerKeyNonExactMatch, StrPartnerKeyNonExactInfoTest);

            //
            // pnlPartnerStatus
            //
            this.pnlPartnerStatus.Controls.Add(this.rbtPrivate);
            this.pnlPartnerStatus.Controls.Add(this.rbtStatusActive);
            this.pnlPartnerStatus.Controls.Add(this.rbtStatusAll);
            this.pnlPartnerStatus.Controls.Add(this.lblPartnerStatus);
            this.pnlPartnerStatus.Location = new System.Drawing.Point(0, 108);
            this.pnlPartnerStatus.Name = "pnlPartnerStatus";
            this.pnlPartnerStatus.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerStatus.TabIndex = 2;

            //
            // rbtPrivate
            //
            this.rbtPrivate.BackColor = System.Drawing.SystemColors.Control;
            this.rbtPrivate.Location = new System.Drawing.Point(245, 0);
            this.rbtPrivate.Name = "rbtPrivate";
            this.rbtPrivate.Size = new System.Drawing.Size(66, 24);
            this.rbtPrivate.TabIndex = 2;
            this.rbtPrivate.TabStop = true;
            this.rbtPrivate.Tag = "PRIVATE";
            this.rbtPrivate.Text = "Private";
            this.rbtPrivate.UseVisualStyleBackColor = false;
            this.rbtPrivate.Click += new System.EventHandler(this.RbtPrivate_Click);

            //
            // rbtStatusActive
            //
            this.rbtStatusActive.BackColor = System.Drawing.SystemColors.Control;
            this.rbtStatusActive.Checked = true;
            this.rbtStatusActive.Location = new System.Drawing.Point(146, 0);
            this.rbtStatusActive.Name = "rbtStatusActive";
            this.rbtStatusActive.Size = new System.Drawing.Size(60, 24);
            this.rbtStatusActive.TabIndex = 1;
            this.rbtStatusActive.TabStop = true;
            this.rbtStatusActive.Tag = "ACTIVE";
            this.rbtStatusActive.Text = "Acti&ve";
            this.rbtStatusActive.UseVisualStyleBackColor = false;
            this.rbtStatusActive.Click += new System.EventHandler(this.RbtStatusActive_Click);

            //
            // rbtStatusAll
            //
            this.rbtStatusAll.BackColor = System.Drawing.SystemColors.Control;
            this.rbtStatusAll.Location = new System.Drawing.Point(206, 0);
            this.rbtStatusAll.Name = "rbtStatusAll";
            this.rbtStatusAll.Size = new System.Drawing.Size(39, 24);
            this.rbtStatusAll.TabIndex = 1;
            this.rbtStatusAll.Tag = "ALL";
            this.rbtStatusAll.Text = "All";
            this.rbtStatusAll.UseVisualStyleBackColor = false;
            this.rbtStatusAll.Click += new System.EventHandler(this.RbtStatusAll_Click);

            //
            // lblPartnerStatus
            //
            this.lblPartnerStatus.Location = new System.Drawing.Point(2, 4);
            this.lblPartnerStatus.Name = "lblPartnerStatus";
            this.lblPartnerStatus.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerStatus.TabIndex = 0;
            this.lblPartnerStatus.Text = "Status:";
            this.lblPartnerStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPersonnelCriteria
            //
            this.pnlPersonnelCriteria.Location = new System.Drawing.Point(2, 138);
            this.pnlPersonnelCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPersonnelCriteria.Name = "pnlPersonnelCriteria";
            this.pnlPersonnelCriteria.Size = new System.Drawing.Size(308, 102);
            this.pnlPersonnelCriteria.TabIndex = 2;

            //
            // pnlLeftColumn
            //
            this.pnlLeftColumn.AutoSize = true;
            this.pnlLeftColumn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLeftColumn.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeftColumn.Controls.Add(this.pnlPhoneNumber);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress3);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress2);
            this.pnlLeftColumn.Controls.Add(this.pnlEmail);
            this.pnlLeftColumn.Controls.Add(this.pnlPartnerName);
            this.pnlLeftColumn.Controls.Add(this.pnlPersonalName);
            this.pnlLeftColumn.Controls.Add(this.pnlPreviousName);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress1);
            this.pnlLeftColumn.Controls.Add(this.pnlPostCode);
            this.pnlLeftColumn.Controls.Add(this.pnlCity);
            this.pnlLeftColumn.Controls.Add(this.pnlCounty);
            this.pnlLeftColumn.Controls.Add(this.pnlCountry);
            this.pnlLeftColumn.Controls.Add(this.pnlMailingAddressOnly);
            this.pnlLeftColumn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeftColumn.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftColumn.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeftColumn.Name = "pnlLeftColumn";
            this.pnlLeftColumn.Size = new System.Drawing.Size(309, 294);
            this.pnlLeftColumn.TabIndex = 1;

            //
            // pnlPhoneNumber
            //
            this.pnlPhoneNumber.BackColor = System.Drawing.Color.Transparent;
            this.pnlPhoneNumber.Controls.Add(this.critPhoneNumber);
            this.pnlPhoneNumber.Controls.Add(this.lblPhoneNumber);
            this.pnlPhoneNumber.Controls.Add(this.txtPhoneNumber);
            this.pnlPhoneNumber.Location = new System.Drawing.Point(2, 124);
            this.pnlPhoneNumber.Name = "pnlPhoneNumber";
            this.pnlPhoneNumber.Size = new System.Drawing.Size(304, 21);
            this.pnlPhoneNumber.TabIndex = 6;

            //
            // critPhoneNumber
            //
            this.critPhoneNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPhoneNumber.BackColor = System.Drawing.SystemColors.Control;
            this.critPhoneNumber.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPhoneNumber.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PhoneNumberMatch",
                    true));
            this.critPhoneNumber.Location = new System.Drawing.Point(260, 2);
            this.critPhoneNumber.Name = "critPhoneNumber";
            this.critPhoneNumber.SelectedValue = "BEGINS";
            this.critPhoneNumber.Size = new System.Drawing.Size(41, 18);
            this.critPhoneNumber.TabIndex = 4;
            this.critPhoneNumber.TabStop = false;

            //
            // lblPhoneNumber
            //
            this.lblPhoneNumber.Location = new System.Drawing.Point(2, 2);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(142, 23);
            this.lblPhoneNumber.TabIndex = 0;
            this.lblPhoneNumber.Text = "Phone Number:";
            this.lblPhoneNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtPhoneNumber
            //
            this.txtPhoneNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhoneNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PhoneNumber", true));
            this.txtPhoneNumber.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(146, 0);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(112, 21);
            this.txtPhoneNumber.TabIndex = 1;
            this.txtPhoneNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPhoneNumber_KeyUp);
            this.txtPhoneNumber.Leave += new EventHandler(this.TxtPhoneNumber_Leave);

            //
            // pnlAddress3
            //
            this.pnlAddress3.Controls.Add(this.critAddress3);
            this.pnlAddress3.Controls.Add(this.txtAddress3);
            this.pnlAddress3.Controls.Add(this.lblAddress3);
            this.pnlAddress3.Location = new System.Drawing.Point(2, 124);
            this.pnlAddress3.Name = "pnlAddress3";
            this.pnlAddress3.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress3.TabIndex = 5;
            this.pnlAddress3.Tag = "";

            //
            // critAddress3
            //
            this.critAddress3.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress3.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress3.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress3.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address3Match", true));
            this.critAddress3.Location = new System.Drawing.Point(260, 2);
            this.critAddress3.Name = "critAddress3";
            this.critAddress3.SelectedValue = "BEGINS";
            this.critAddress3.Size = new System.Drawing.Size(41, 18);
            this.critAddress3.TabIndex = 4;
            this.critAddress3.TabStop = false;

            //
            // txtAddress3
            //
            this.txtAddress3.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address3", true));
            this.txtAddress3.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress3.Location = new System.Drawing.Point(146, 0);
            this.txtAddress3.Name = "txtAddress3";
            this.txtAddress3.Size = new System.Drawing.Size(112, 21);
            this.txtAddress3.TabIndex = 1;
            this.txtAddress3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress3_KeyUp);
            this.txtAddress3.Leave += new EventHandler(this.TxtAddress3_Leave);

            //
            // lblAddress3
            //
            this.lblAddress3.Location = new System.Drawing.Point(2, 2);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(142, 23);
            this.lblAddress3.TabIndex = 0;
            this.lblAddress3.Text = "Address &3:";
            this.lblAddress3.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAddress2
            //
            this.pnlAddress2.Controls.Add(this.critAddress2);
            this.pnlAddress2.Controls.Add(this.txtAddress2);
            this.pnlAddress2.Controls.Add(this.lblAddress2);
            this.pnlAddress2.Location = new System.Drawing.Point(2, 124);
            this.pnlAddress2.Name = "pnlAddress2";
            this.pnlAddress2.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress2.TabIndex = 4;
            this.pnlAddress2.Tag = "";

            //
            // critAddress2
            //
            this.critAddress2.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress2.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress2.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address2Match", true));
            this.critAddress2.Location = new System.Drawing.Point(260, 2);
            this.critAddress2.Name = "critAddress2";
            this.critAddress2.SelectedValue = "BEGINS";
            this.critAddress2.Size = new System.Drawing.Size(41, 18);
            this.critAddress2.TabIndex = 4;
            this.critAddress2.TabStop = false;

            //
            // txtAddress2
            //
            this.txtAddress2.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address2", true));
            this.txtAddress2.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress2.Location = new System.Drawing.Point(146, 0);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(112, 21);
            this.txtAddress2.TabIndex = 1;
            this.txtAddress2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress2_KeyUp);
            this.txtAddress2.Leave += new EventHandler(this.TxtAddress2_Leave);

            //
            // lblAddress2
            //
            this.lblAddress2.Location = new System.Drawing.Point(2, 2);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(142, 23);
            this.lblAddress2.TabIndex = 0;
            this.lblAddress2.Text = "Address &2:";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlEmail
            //
            this.pnlEmail.Controls.Add(this.critEmail);
            this.pnlEmail.Controls.Add(this.txtEmail);
            this.pnlEmail.Controls.Add(this.lblEmail);
            this.pnlEmail.Location = new System.Drawing.Point(2, 122);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(304, 21);
            this.pnlEmail.TabIndex = 3;

            //
            // critEmail
            //
            this.critEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critEmail.BackColor = System.Drawing.SystemColors.Control;
            this.critEmail.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critEmail.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "EmailMatch", true));
            this.critEmail.Location = new System.Drawing.Point(260, 2);
            this.critEmail.Name = "critEmail";
            this.critEmail.SelectedValue = "BEGINS";
            this.critEmail.Size = new System.Drawing.Size(41, 18);
            this.critEmail.TabIndex = 3;
            this.critEmail.TabStop = false;

            //
            // txtEmail
            //
            this.txtEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Email", true));
            this.txtEmail.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(146, 0);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(112, 21);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtEmail_KeyUp);
            this.txtEmail.Leave += new EventHandler(this.TxtEmail_Leave);

            //
            // lblEmail
            //
            this.lblEmail.Location = new System.Drawing.Point(2, 2);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(142, 22);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "&Email:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPartnerName
            //
            this.pnlPartnerName.Controls.Add(this.critPartnerName);
            this.pnlPartnerName.Controls.Add(this.txtPartnerName);
            this.pnlPartnerName.Controls.Add(this.lblPartnerName);
            this.pnlPartnerName.Location = new System.Drawing.Point(2, 2);
            this.pnlPartnerName.Name = "pnlPartnerName";
            this.pnlPartnerName.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerName.TabIndex = 2;

            //
            // critPartnerName
            //
            this.critPartnerName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPartnerName.BackColor = System.Drawing.SystemColors.Control;
            this.critPartnerName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PartnerNameMatch",
                    true));
            this.critPartnerName.Location = new System.Drawing.Point(260, 2);
            this.critPartnerName.Name = "critPartnerName";
            this.critPartnerName.SelectedValue = "BEGINS";
            this.critPartnerName.Size = new System.Drawing.Size(41, 18);
            this.critPartnerName.TabIndex = 2;
            this.critPartnerName.TabStop = false;

            //
            // txtPartnerName
            //
            this.txtPartnerName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PartnerName", true));
            this.txtPartnerName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartnerName.Location = new System.Drawing.Point(146, 0);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.Size = new System.Drawing.Size(112, 21);
            this.txtPartnerName.TabIndex = 1;
            this.txtPartnerName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPartnerName_KeyUp);
            this.txtPartnerName.Leave += new EventHandler(this.TxtPartnerName_Leave);

            //
            // lblPartnerName
            //
            this.lblPartnerName.BackColor = System.Drawing.Color.Transparent;
            this.lblPartnerName.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerName.TabIndex = 0;
            this.lblPartnerName.Text = "Partner &Name:";
            this.lblPartnerName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPersonalName
            //
            this.pnlPersonalName.Controls.Add(this.critPersonalName);
            this.pnlPersonalName.Controls.Add(this.txtPersonalName);
            this.pnlPersonalName.Controls.Add(this.lblPersonalName);
            this.pnlPersonalName.Location = new System.Drawing.Point(2, 28);
            this.pnlPersonalName.Name = "pnlPersonalName";
            this.pnlPersonalName.Size = new System.Drawing.Size(304, 21);
            this.pnlPersonalName.TabIndex = 2;

            //
            // critPersonalName
            //
            this.critPersonalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPersonalName.BackColor = System.Drawing.SystemColors.Control;
            this.critPersonalName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPersonalName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PersonalNameMatch",
                    true));
            this.critPersonalName.Location = new System.Drawing.Point(260, 2);
            this.critPersonalName.Name = "critPersonalName";
            this.critPersonalName.SelectedValue = "BEGINS";
            this.critPersonalName.Size = new System.Drawing.Size(41, 18);
            this.critPersonalName.TabIndex = 3;
            this.critPersonalName.TabStop = false;

            //
            // txtPersonalName
            //
            this.txtPersonalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPersonalName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PersonalName", true));
            this.txtPersonalName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPersonalName.Location = new System.Drawing.Point(146, 0);
            this.txtPersonalName.Name = "txtPersonalName";
            this.txtPersonalName.Size = new System.Drawing.Size(112, 21);
            this.txtPersonalName.TabIndex = 1;
            this.txtPersonalName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPersonalName_KeyUp);
            this.txtPersonalName.Leave += new EventHandler(this.TxtPersonalName_Leave);

            //
            // lblPersonalName
            //
            this.lblPersonalName.Location = new System.Drawing.Point(2, 2);
            this.lblPersonalName.Name = "lblPersonalName";
            this.lblPersonalName.Size = new System.Drawing.Size(142, 22);
            this.lblPersonalName.TabIndex = 0;
            this.lblPersonalName.Text = "Personal &(First) Name:";
            this.lblPersonalName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPreviousName
            //
            this.pnlPreviousName.Controls.Add(this.critPreviousName);
            this.pnlPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreviousName.Location = new System.Drawing.Point(2, 54);
            this.pnlPreviousName.Name = "pnlPreviousName";
            this.pnlPreviousName.Size = new System.Drawing.Size(304, 21);
            this.pnlPreviousName.TabIndex = 2;
            this.pnlPreviousName.Visible = false;

            //
            // critPreviousName
            //
            this.critPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPreviousName.BackColor = System.Drawing.SystemColors.Control;
            this.critPreviousName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPreviousName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PreviousNameMatch",
                    true));
            this.critPreviousName.Location = new System.Drawing.Point(260, 2);
            this.critPreviousName.Name = "critPreviousName";
            this.critPreviousName.SelectedValue = "BEGINS";
            this.critPreviousName.Size = new System.Drawing.Size(41, 18);
            this.critPreviousName.TabIndex = 4;
            this.critPreviousName.TabStop = false;

            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PreviousName", true));
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreviousName.Location = new System.Drawing.Point(146, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(112, 21);
            this.txtPreviousName.TabIndex = 1;
            this.txtPreviousName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPreviousName_KeyUp);
            this.txtPreviousName.Leave += new EventHandler(this.TxtPreviousName_Leave);

            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(2, 2);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(142, 23);
            this.lblPreviousName.TabIndex = 0;
            this.lblPreviousName.Text = "Previous Name:";
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAddress1
            //
            this.pnlAddress1.Controls.Add(this.critAddress1);
            this.pnlAddress1.Controls.Add(this.txtAddress1);
            this.pnlAddress1.Controls.Add(this.lblAddress1);
            this.pnlAddress1.Location = new System.Drawing.Point(2, 78);
            this.pnlAddress1.Name = "pnlAddress1";
            this.pnlAddress1.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress1.TabIndex = 2;
            this.pnlAddress1.Tag = "BeginGroup";

            //
            // critAddress1
            //
            this.critAddress1.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress1.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress1.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address1Match", true));
            this.critAddress1.Location = new System.Drawing.Point(260, 2);
            this.critAddress1.Name = "critAddress1";
            this.critAddress1.SelectedValue = "BEGINS";
            this.critAddress1.Size = new System.Drawing.Size(41, 18);
            this.critAddress1.TabIndex = 4;
            this.critAddress1.TabStop = false;

            //
            // txtAddress1
            //
            this.txtAddress1.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address1", true));
            this.txtAddress1.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress1.Location = new System.Drawing.Point(146, 0);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(112, 21);
            this.txtAddress1.TabIndex = 1;
            this.txtAddress1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress1_KeyUp);
            this.txtAddress1.Leave += new EventHandler(this.TxtAddress1_Leave);

            //
            // lblAddress1
            //
            this.lblAddress1.Location = new System.Drawing.Point(2, 2);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(142, 23);
            this.lblAddress1.TabIndex = 0;
            this.lblAddress1.Text = "Address &1:";
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPostCode
            //
            this.pnlPostCode.BackColor = System.Drawing.Color.Transparent;
            this.pnlPostCode.Controls.Add(this.critPostCode);
            this.pnlPostCode.Controls.Add(this.lblPostCode);
            this.pnlPostCode.Controls.Add(this.txtPostCode);
            this.pnlPostCode.Location = new System.Drawing.Point(2, 197);
            this.pnlPostCode.Name = "pnlPostCode";
            this.pnlPostCode.Size = new System.Drawing.Size(304, 21);
            this.pnlPostCode.TabIndex = 0;

            //
            // critPostCode
            //
            this.critPostCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPostCode.BackColor = System.Drawing.SystemColors.Control;
            this.critPostCode.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPostCode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PostCodeMatch", true));
            this.critPostCode.Location = new System.Drawing.Point(260, 2);
            this.critPostCode.Name = "critPostCode";
            this.critPostCode.SelectedValue = "BEGINS";
            this.critPostCode.Size = new System.Drawing.Size(41, 18);
            this.critPostCode.TabIndex = 4;
            this.critPostCode.TabStop = false;

            //
            // lblPostCode
            //
            this.lblPostCode.Location = new System.Drawing.Point(2, 2);
            this.lblPostCode.Name = "lblPostCode";
            this.lblPostCode.Size = new System.Drawing.Size(142, 23);
            this.lblPostCode.TabIndex = 0;
            this.lblPostCode.Text = "P&ost Code:";
            this.lblPostCode.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtPostCode
            //
            this.txtPostCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PostCode", true));
            this.txtPostCode.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostCode.Location = new System.Drawing.Point(146, 0);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(112, 21);
            this.txtPostCode.TabIndex = 1;
            this.txtPostCode.Leave += new System.EventHandler(this.TxtPostCode_Leave);
            this.txtPostCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPostCode_KeyUp);

            //
            // pnlCity
            //
            this.pnlCity.Controls.Add(this.critCity);
            this.pnlCity.Controls.Add(this.lblCity);
            this.pnlCity.Controls.Add(this.txtCity);
            this.pnlCity.Location = new System.Drawing.Point(2, 100);
            this.pnlCity.Name = "pnlCity";
            this.pnlCity.Size = new System.Drawing.Size(304, 21);
            this.pnlCity.TabIndex = 0;

            //
            // critCity
            //
            this.critCity.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critCity.BackColor = System.Drawing.SystemColors.Control;
            this.critCity.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critCity.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "CityMatch", true));
            this.critCity.Location = new System.Drawing.Point(260, 2);
            this.critCity.Name = "critCity";
            this.critCity.SelectedValue = "BEGINS";
            this.critCity.Size = new System.Drawing.Size(41, 18);
            this.critCity.TabIndex = 4;
            this.critCity.TabStop = false;

            //
            // lblCity
            //
            this.lblCity.Location = new System.Drawing.Point(2, 2);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(142, 23);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "Cit&y/Town:";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtCity
            //
            this.txtCity.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "City", true));
            this.txtCity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.Location = new System.Drawing.Point(146, 0);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(112, 21);
            this.txtCity.TabIndex = 1;
            this.txtCity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtCity_KeyUp);
            this.txtCity.Leave += new EventHandler(this.TxtCity_Leave);

            //
            // pnlCounty
            //
            this.pnlCounty.Controls.Add(this.critCounty);
            this.pnlCounty.Controls.Add(this.txtCounty);
            this.pnlCounty.Controls.Add(this.lblCounty);
            this.pnlCounty.Location = new System.Drawing.Point(2, 174);
            this.pnlCounty.Name = "pnlCounty";
            this.pnlCounty.Size = new System.Drawing.Size(304, 21);
            this.pnlCounty.TabIndex = 2;
            this.pnlCounty.Visible = false;

            //
            // critCounty
            //
            this.critCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critCounty.BackColor = System.Drawing.SystemColors.Control;
            this.critCounty.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critCounty.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "CountyMatch", true));
            this.critCounty.Location = new System.Drawing.Point(260, 2);
            this.critCounty.Name = "critCounty";
            this.critCounty.SelectedValue = "BEGINS";
            this.critCounty.Size = new System.Drawing.Size(41, 18);
            this.critCounty.TabIndex = 4;
            this.critCounty.TabStop = false;

            //
            // txtCounty
            //
            this.txtCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtCounty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "County", true));
            this.txtCounty.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCounty.Location = new System.Drawing.Point(146, 0);
            this.txtCounty.Name = "txtCounty";
            this.txtCounty.Size = new System.Drawing.Size(112, 21);
            this.txtCounty.TabIndex = 2;
            this.txtCounty.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtCounty_KeyUp);
            this.txtCounty.Leave += new EventHandler(this.TxtCounty_Leave);

            //
            // lblCounty
            //
            this.lblCounty.Location = new System.Drawing.Point(2, 2);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(142, 23);
            this.lblCounty.TabIndex = 0;
            this.lblCounty.Text = "Co&unty:";
            this.lblCounty.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlCountry
            //
            this.pnlCountry.BackColor = System.Drawing.Color.Transparent;
            this.pnlCountry.Controls.Add(this.ucoCountryComboBox);
            this.pnlCountry.Controls.Add(this.lblCountry);
            this.pnlCountry.Location = new System.Drawing.Point(2, 226);
            this.pnlCountry.Name = "pnlCountry";
            this.pnlCountry.Size = new System.Drawing.Size(304, 22);
            this.pnlCountry.TabIndex = 2;

            //
            // ucoCountryComboBox
            //
            this.ucoCountryComboBox.Location = new System.Drawing.Point(146, 0);
            this.ucoCountryComboBox.Name = "ucoCountryComboBox";
            this.ucoCountryComboBox.SelectedValue = null;
            this.ucoCountryComboBox.Size = new System.Drawing.Size(154, 22);
            this.ucoCountryComboBox.TabIndex = 2;

            //
            // lblCountry
            //
            this.lblCountry.Location = new System.Drawing.Point(2, 2);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(142, 23);
            this.lblCountry.TabIndex = 0;
            this.lblCountry.Text = "Co&untry:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlMailingAddressOnly
            //
            this.pnlMailingAddressOnly.Controls.Add(this.chkMailingAddressOnly);
            this.pnlMailingAddressOnly.Controls.Add(this.lblMailingAddressOnly);
            this.pnlMailingAddressOnly.Location = new System.Drawing.Point(2, 270);
            this.pnlMailingAddressOnly.Name = "pnlMailingAddressOnly";
            this.pnlMailingAddressOnly.Size = new System.Drawing.Size(304, 21);
            this.pnlMailingAddressOnly.TabIndex = 2;
            this.pnlMailingAddressOnly.Tag = "BeginGroup";

            //
            // chkMailingAddressOnly
            //
            this.chkMailingAddressOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.FFindCriteriaDataTable, "MailingAddressOnly",
                    true));
            this.chkMailingAddressOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkMailingAddressOnly.Location = new System.Drawing.Point(146, 4);
            this.chkMailingAddressOnly.Name = "chkMailingAddressOnly";
            this.chkMailingAddressOnly.Size = new System.Drawing.Size(12, 12);
            this.chkMailingAddressOnly.TabIndex = 1;

            //
            // lblMailingAddressOnly
            //
            this.lblMailingAddressOnly.Location = new System.Drawing.Point(0, 2);
            this.lblMailingAddressOnly.Name = "lblMailingAddressOnly";
            this.lblMailingAddressOnly.Size = new System.Drawing.Size(144, 23);
            this.lblMailingAddressOnly.TabIndex = 0;
            this.lblMailingAddressOnly.Text = "Mailin&g Addresses Only:";
            this.lblMailingAddressOnly.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // TUC_PartnerFindCriteria
            //
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.spcCriteria);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUC_PartnerFindCriteria";
            this.Size = new System.Drawing.Size(655, 213);
            this.Load += new System.EventHandler(this.TUC_PartnerFindCriteria_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FFindCriteriaDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFindCriteriaDataSet)).EndInit();
            this.spcCriteria.Panel1.ResumeLayout(false);
            this.spcCriteria.Panel1.PerformLayout();
            this.spcCriteria.Panel2.ResumeLayout(false);
            this.spcCriteria.Panel2.PerformLayout();
            this.spcCriteria.ResumeLayout(false);
            this.pnlRightColumn.ResumeLayout(false);
            this.pnlLocationKey.ResumeLayout(false);
            this.pnlLocationKey.PerformLayout();
            this.pnlPartnerClass.ResumeLayout(false);
            this.pnlPartnerKey.ResumeLayout(false);
            this.pnlPartnerKey.PerformLayout();
            this.pnlPartnerStatus.ResumeLayout(false);
            this.pnlLeftColumn.ResumeLayout(false);
            this.pnlPhoneNumber.ResumeLayout(false);
            this.pnlPhoneNumber.PerformLayout();
            this.pnlAddress3.ResumeLayout(false);
            this.pnlAddress3.PerformLayout();
            this.pnlAddress2.ResumeLayout(false);
            this.pnlAddress2.PerformLayout();
            this.pnlEmail.ResumeLayout(false);
            this.pnlEmail.PerformLayout();
            this.pnlPartnerName.ResumeLayout(false);
            this.pnlPartnerName.PerformLayout();
            this.pnlPersonalName.ResumeLayout(false);
            this.pnlPersonalName.PerformLayout();
            this.pnlPreviousName.ResumeLayout(false);
            this.pnlPreviousName.PerformLayout();
            this.pnlAddress1.ResumeLayout(false);
            this.pnlAddress1.PerformLayout();
            this.pnlPostCode.ResumeLayout(false);
            this.pnlPostCode.PerformLayout();
            this.pnlCity.ResumeLayout(false);
            this.pnlCity.PerformLayout();
            this.pnlCounty.ResumeLayout(false);
            this.pnlCounty.PerformLayout();
            this.pnlCountry.ResumeLayout(false);
            this.pnlMailingAddressOnly.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.SplitContainer spcCriteria;

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFindCriteria() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FDefaultValues = FFindCriteriaDataTable.NewRow();
        }

        private TFrmPetraUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for screens
        /// </summary>
        public TFrmPetraUtils PetraUtilsObject
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

        private void RbtStatusActive_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "ACTIVE";
        }

        private void RbtStatusAll_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "ALL";
        }

        private void RbtPrivate_Click(System.Object sender, System.EventArgs e)
        {
            this.PartnerStatus = "PRIVATE";
        }

        private void CmbPartnerClass_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            // This is not very nice but it seems to be the only way to handle changes in
            // Partner Class combo box
            if (FFindCriteriaDataTable.Rows.Count > 0)
            {
                DataRow SingleDataRow = FFindCriteriaDataTable.Rows[0];
                SingleDataRow.BeginEdit();
                SingleDataRow["PersonalName"] = txtPersonalName.Text;
                SingleDataRow["PartnerClass"] = cmbPartnerClass.SelectedValue;
                SingleDataRow.EndEdit();

                if (txtPersonalName.Text.Length == 1)
                {
                    // Set manually the cursor at the end of the textbox when user there is only
                    // one character left.
                    // Reason: When the user types the first character the cursor is somehow set
                    // back to position 0. So here we set it back to position 1.
                    txtPersonalName.SelectionStart = 1;
                }
            }

            HandlePartnerClassGui();
        }

        private void HandlePartnerClassGui()
        {
            chkWorkerFamOnly.Visible = false;
            chkWorkerFamOnly.Checked = false;
            pnlPartnerClass.Height = cmbPartnerClass.Height + 3;

            // the partner class combo might not be on the form!
            if (cmbPartnerClass.SelectedValue == null)
            {
                return;

                // get out of here!
            }

            if (cmbPartnerClass.SelectedValue.ToString() == "FAMILY")
            {
                chkWorkerFamOnly.Visible = true;
                chkWorkerFamOnly.Checked = false;

                if (FWorkerFamOnly == true)
                {
                    chkWorkerFamOnly.Checked = true;
                    chkWorkerFamOnly.Enabled = false;
                }

                pnlPartnerClass.Height = cmbPartnerClass.Height + chkWorkerFamOnly.Height + 3;

                // enable personal Name field
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            // enable txtPersonalName only if we have *, Family or Person as PartnerClass
            // and when it is not enabled.
            else if (cmbPartnerClass.SelectedValue.ToString() == "PERSON")
            {
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            else if (cmbPartnerClass.SelectedValue.ToString() == "*")
            {
                if (txtPersonalName.Enabled == false)
                {
                    txtPersonalName.Enabled = true;
                }
            }
            else if (txtPersonalName.Enabled == true)
            {
                // disable pesonal name field when we don't have *, Family or Person as Partner Class
                // and when it is enabled
                txtPersonalName.Enabled = false;
            }
        }

        private void BtnLocationKey_Click(System.Object sender, System.EventArgs e)
        {
// TODO BtnLocationKey_Click
#if TODO
            TPartnerLocationFind frmPartnerLS;

            // hourglass cursor
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            // give windows half a chance to show cursor
            frmPartnerLS = new TPartnerLocationFind();

            if (frmPartnerLS.ShowDialog() == DialogResult.OK)
            {
                // fill in the location key
                txtLocationKey.Text = frmPartnerLS.SelectedLocation.LocationKey.ToString();

                // disable all other controls
                this.DisableAllPanel(pnlLocationKey);

                // KICK the databinding which hasn't notices the textbox has changed
                txtLocationKey.Focus();
            }

            // normal mouse cursor
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();

            // give windows half a chance to show cursor
#endif
        }

        private void LblAddress2_Click(System.Object sender, System.EventArgs e)
        {
        }

        #region User Defaults for Match Criteria Buttons

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="parentctrl"></param>
        /// <returns></returns>
        public String GetMatchButtonsString(Control parentctrl)
        {
            String strOutput;
            ArrayList outList;
            Boolean firstOne;

            outList = new ArrayList();
            strOutput = "";
            firstOne = true;

            foreach (Control ctrl in parentctrl.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    foreach (Control innerCtrl in ctrl.Controls)
                    {
                        if (innerCtrl.GetType() == typeof(SplitButton))
                        {
                            outList.Add(innerCtrl.Name.ToString() + ',' + ((SplitButton)innerCtrl).SelectedValue);
                        }
                    }

                    // for innerCtrl
                }

                // if Panel;
            }

            // for ctrl
            foreach (String strItem in outList)
            {
                if (firstOne == false)
                {
                    strOutput = strOutput + '|';
                }

                strOutput = strOutput + strItem;
                firstOne = false;
            }

            return strOutput;
        }

        /// <summary>
        /// function
        /// </summary>
        public void SaveMatchButtonSettings()
        {
            String strLeft;
            String strRight;

            strLeft = GetMatchButtonsString(pnlLeftColumn);
            strRight = GetMatchButtonsString(pnlRightColumn);
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_LEFT, strLeft);
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_RIGHT, strRight);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RestoreSplitterSetting()
        {
            spcCriteria.SplitterDistance = TUserDefaults.GetInt32Default(TUserDefaults.PARTNER_FIND_SPLITPOS_CRITERIA, 326);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SaveSplitterSetting()
        {
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_FIND_SPLITPOS_CRITERIA, spcCriteria.SplitterDistance);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void LoadMatchButtonSettings()
        {
            String strLeft;
            String strRight;

            // retrieve defaults
            strLeft = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_LEFT, "");
            strRight = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_EDIT_MATCHSETTINGS_RIGHT, "");

            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                FFindCriteriaDataTable.Rows[0].BeginEdit();
            }

            // FIRST DO LEFT LIST
            if (strLeft != "")
            {
                SetMatchButtonValues(strLeft);
            }

            // if strLeft
            // NOW RIGHT LIST
            if (strRight != "")
            {
                SetMatchButtonValues(strRight);
            }

            // if strRight
            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                FFindCriteriaDataTable.Rows[0].EndEdit();
            }

            AssociateCriteriaButtons();
        }

        /// <summary>
        /// Takes a delimited string and sets the controls properties
        /// </summary>
        /// <param name="AValues"></param>
        public void SetMatchButtonValues(String AValues)
        {
            String[] miLineArray;
            String ctrlName;
            String ctrlValue;
            String tempString;
            Control ctrl;
            Int32 x;

            if (AValues != "")
            {
                // setup delimiters
                char[] lineDelimiter = new char[] {
                    '|'
                };
                char[] fieldDelimiter = new char[] {
                    ','
                };
                miLineArray = AValues.Split(lineDelimiter);

                for (x = 0; x <= miLineArray.Length - 1; x += 1)
                {
                    tempString = Convert.ToString(miLineArray[x]);
                    ctrlName = tempString.Split(fieldDelimiter)[0];
                    ctrlValue = tempString.Split(fieldDelimiter)[1];
                    ctrl = FindControl(this, ctrlName);

                    if (ctrl != null)
                    {
                        FFindCriteriaDataTable.Rows[0].BeginEdit();
                        TLogging.Log("Running SetMatchButtonValues for " + ctrl.Name + ".");
                        ((SplitButton)ctrl).SelectedValue = ctrlValue;
                        FFindCriteriaDataTable.Rows[0].EndEdit();

                        //TODO: It seems databinding is broken on this control
                        // this needs to happen in the SplitButton control really
                        string fieldname = ((SplitButton)ctrl).DataBindings[0].BindingMemberInfo.BindingMember;
                        FFindCriteriaDataTable.Rows[0][fieldname] = ctrlValue;

                        // MessageBox.Show('control found:' + ctrl.Name);
                    }
                }

                // for
            }

            // if strRight
        }

        /// <summary>
        /// Recursively searches for a control by name in a given container
        /// </summary>
        /// <param name="AContainer"></param>
        /// <param name="AName"></param>
        /// <returns></returns>
        public Control FindControl(Control AContainer, String AName)
        {
            Control miResult;

            miResult = null;

            foreach (Control ctrl in AContainer.Controls)
            {
                if ((miResult == null) && (ctrl.Name == AName))
                {
                    miResult = ctrl;
                }

                if ((miResult == null) && (ctrl.HasChildren == true))
                {
                    miResult = FindControl(ctrl, AName);
                }
            }

            // for ctrl
            return miResult;
        }

        #endregion

        #region Key Press Events for showing context menus

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATextBox"></param>
        /// <param name="ACriteriaControl"></param>
        /// <param name="e"></param>
        public void GeneralKeyHandler(TextBox ATextBox, SplitButton ACriteriaControl, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                // This is needed
                // without it, databinding fails
                ACriteriaControl.ShowContextMenu();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATextBox"></param>
        /// <param name="ACriteriaControl"></param>
        public void GeneralLeaveHandler(TextBox ATextBox, SplitButton ACriteriaControl)
        {
            TMatches NewMatchValue;
            string TextBoxText = ATextBox.Text;
            string CriteriaValue;

            //            TLogging.Log("GeneralLeaveHandler for " + ATextBox.Name + ". SplitButton: " + ACriteriaControl.Name);

            if (ATextBox.Text.Contains("*"))
            {
                if (ATextBox.Text.StartsWith("*")
                    && !(ATextBox.Text.EndsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " starts with *");
                    NewMatchValue = TMatches.ENDS;
                }
                else if (ATextBox.Text.EndsWith("*")
                         && !(ATextBox.Text.StartsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " ends with *");
                    NewMatchValue = TMatches.BEGINS;
                }
                else
                {
                    //                    TLogging.Log(ATextBox.Name + " contains *");
                    NewMatchValue = TMatches.CONTAINS;
                }

                // See what the Criteria Value would be without any 'joker' characters ( * )
                CriteriaValue = TextBoxText.Replace("*", String.Empty);

                if (CriteriaValue != String.Empty)
                {
                    // There is still a valid CriteriaValue

                    FFindCriteriaDataTable.Rows[0].BeginEdit();
                    ACriteriaControl.SelectedValue = Enum.GetName(typeof(TMatches), NewMatchValue);
                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    //TODO: It seems databinding is broken on this control
                    // this needs to happen in the SplitButton control really
                    string fieldname = ((SplitButton)ACriteriaControl).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), NewMatchValue);

                    //TODO: DataBinding is really doing strange things here; we have to
                    //assign the just entered Text again, otherwise it is lost!!!
                    ATextBox.Text = TextBoxText;
                }
                else
                {
                    // No valid Criteria Value, therefore empty the TextBox's Text.
                    ATextBox.Text = String.Empty;
                }
            }
        }

        private void RemoveJokersFromTextBox(SplitButton ASplitButton,
            TextBox AAssociatedTextBox,
            TMatches ALastSelection)
        {
            string NewText;

            try
            {
                if (AAssociatedTextBox != null)
                {
                    NewText = AAssociatedTextBox.Text.Replace("*", String.Empty);

//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (1): " + AAssociatedTextBox.Text);

//                    FFindCriteriaDataTable.Rows[0].BeginEdit();
//                    AAssociatedTextBox.Text = AAssociatedTextBox.Text.Replace("*", String.Empty);
//                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    string fieldname = ((TextBox)AAssociatedTextBox).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = NewText;
                    fieldname = ((SplitButton)ASplitButton).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), ALastSelection);

//
//                    AAssociatedTextBox.Text = NewText;
//
//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (2): " + AAssociatedTextBox.Text);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception in RemoveJokersFromTextBox: " + exp.ToString());
            }
        }

        private void TxtCounty_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCounty, critCounty, e);
        }

        private void TxtCounty_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCounty, critCounty);
        }

        private void TxtEmail_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtEmail, critEmail, e);
        }

        private void TxtEmail_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtEmail, critEmail);
        }

        private void TxtPhoneNumber_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPhoneNumber, critPhoneNumber, e);
        }

        private void TxtPhoneNumber_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPhoneNumber, critPhoneNumber);
        }

        private void TxtPostCode_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPostCode, critPostCode, e);
        }

        private void TxtPostCode_Leave(System.Object sender, EventArgs e)
        {
            // capitalise when leaving control
            txtPostCode.Text = txtPostCode.Text.ToUpper();

            GeneralLeaveHandler(txtPostCode, critPostCode);
        }

        private void TxtCity_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCity, critCity, e);
        }

        private void TxtCity_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCity, critCity);
        }

        private void TxtAddress1_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress1, critAddress1, e);
        }

        private void TxtAddress1_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress1, critAddress1);
        }

        private void TxtAddress2_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress2, critAddress2, e);
        }

        private void TxtAddress2_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress2, critAddress2);
        }

        private void TxtAddress3_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress3, critAddress3, e);
        }

        private void TxtAddress3_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress3, critAddress3);
        }

        private void TxtPreviousName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPreviousName, critPreviousName, e);
        }

        private void TxtPreviousName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPreviousName, critPreviousName);
        }

        private void TxtPersonalName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPersonalName, critPersonalName);
        }

        private void TxtPersonalName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPersonalName, critPersonalName, e);
            DataRow PartnerClassDataRow;
            int selectedIndex = 0;

            if ((txtPersonalName.Text.Length > 0)
                && FShowAllPartnerClasses)
            {
                // Here we have a personal name
                // So make sure that only family and persons in the partner class
                // combo box

                selectedIndex = cmbPartnerClass.SelectedIndex;
                FPartnerClassDataTable.Rows.Clear();

                if (FRestrictedParterClasses.Length > 0)
                {
                    InsertRestrictedPartnerClassComboBox(false);
                }
                else
                {
                    PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                    PartnerClassDataRow["PartnerClass"] = "PERSON";
                    FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                    PartnerClassDataRow["PartnerClass"] = "FAMILY";
                    FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                }

                FShowAllPartnerClasses = false;

                // the partner class combo might not be on the form!
                if ((cmbPartnerClass.SelectedValue != null)
                    && (cmbPartnerClass.Items.Count > 0))
                {
                    // We need to perform these things to make the gui work properly
                    cmbPartnerClass.ResetBindings();

                    switch (cmbPartnerClass.Items.Count)
                    {
                        case 0:
                            break;

                        case 1:
                            cmbPartnerClass.SelectedIndex = 0;

                            if (txtPersonalName.Text.Length == 1)
                            {
                                txtPersonalName.SelectionStart = 1;
                            }

                            break;

                        default:
                        {
                            // Set Partner Class to Family or Person, depending on what the previous value was
                            // Need to change it twice to get a selected value changed event
                            switch (selectedIndex)
                            {
                                case 1:
                                    cmbPartnerClass.SelectedIndex = 1;
                                    cmbPartnerClass.SelectedIndex = 0;
                                    break;

                                default:
                                    cmbPartnerClass.SelectedIndex = 0;
                                    cmbPartnerClass.SelectedIndex = 1;
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            else if ((cmbPartnerClass.SelectedValue != null)
                     && (txtPersonalName.Text.Length == 0)
                     && !FShowAllPartnerClasses)
            {
                // We don't have a personal name
                // We show all available types in the partner class combo box.
                FPartnerClassDataTable.Rows.Clear();

                if (FRestrictedParterClasses.Length > 0)
                {
                    InsertRestrictedPartnerClassComboBox(true);
                }
                else
                {
                    InsertDefaultPartnerClassComboBox();
                }

                FShowAllPartnerClasses = true;
                cmbPartnerClass.ResetBindings();

                // Need to change the selection value twice to get a selected value changed event
                switch (cmbPartnerClass.Items.Count)
                {
                    case 0:
                        break;

                    case 1:
                        cmbPartnerClass.SelectedIndex = 0;
                        break;

                    default:
                        cmbPartnerClass.SelectedIndex = 1;
                        cmbPartnerClass.SelectedIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Insert the restricted values into the partner class combo box
        /// </summary>
        /// <param name="AllPartnerClasses">true: all items are inserted. false: only family and person items are inserted</param>
        private void InsertRestrictedPartnerClassComboBox(Boolean AllPartnerClasses)
        {
            DataRow PartnerClassDataRow;

            if (!AllPartnerClasses)
            {
                foreach (String ClassItem in FRestrictedParterClasses)
                {
                    if ((ClassItem == "PERSON")
                        || (ClassItem == "FAMILY"))
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = ClassItem;
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }

                    if (ClassItem == "WORKER-FAM")
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = "FAMILY";
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                }
            }
            else
            {
                foreach (String ClassItem in FRestrictedParterClasses)
                {
                    if (ClassItem == "WORKER-FAM")
                    {
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = "FAMILY";
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                    else
                    {
                        // just add item
                        PartnerClassDataRow = FPartnerClassDataTable.NewRow();
                        PartnerClassDataRow["PartnerClass"] = ClassItem;
                        FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
                    }
                }
            }
        }

        private void TxtPartnerName_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPartnerName, critPartnerName, e);
        }

        private void TxtPartnerName_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtPartnerName, critPartnerName);
        }

        #endregion

        #region KeyPress Events which enable/disable other controls
        private void TxtPartnerKey_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (TUserDefaults.GetBooleanDefault(
                    TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                    true))
            {
                if (System.Convert.ToInt64(txtPartnerKey.Text) == 0)
                {
                    this.EnableAllPanel();
                }

                if (System.Convert.ToInt64(txtPartnerKey.Text) != 0)
                {
                    this.DisableAllPanel(pnlPartnerKey);
                }
            }
        }

        private void TxtLocationKey_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (txtLocationKey.Text.Length == 0)
            {
                this.EnableAllPanel();
            }

            if (txtLocationKey.Text.Length > 0)
            {
                this.DisableAllPanel(pnlLocationKey);
            }
        }

        #endregion

        private void SetupPartnerClassComboBox()
        {
            // MessageBox.Show('SetupPartnerClassComboBox called');
            FPartnerClassDataTable = new DataTable("PartnerClass");
            FPartnerClassDataTable.Columns.Add("PartnerClass");
            FPartnerClassDataTable.PrimaryKey = new DataColumn[] {
                (FPartnerClassDataTable.Columns[0])
            };

            InsertDefaultPartnerClassComboBox();

            cmbPartnerClass.DataSource = FPartnerClassDataTable;
            cmbPartnerClass.DisplayMember = "PartnerClass";
            cmbPartnerClass.ValueMember = "PartnerClass";

            FShowAllPartnerClasses = true;
        }

        /// <summary>
        /// Insert default values into the partner class combo box.
        /// </summary>
        private void InsertDefaultPartnerClassComboBox()
        {
            DataRow PartnerClassDataRow;

            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = '*';
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "PERSON";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "FAMILY";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "CHURCH";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "ORGANISATION";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "BANK";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "UNIT";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
            PartnerClassDataRow = FPartnerClassDataTable.NewRow();
            PartnerClassDataRow["PartnerClass"] = "VENUE";
            FPartnerClassDataTable.Rows.Add(PartnerClassDataRow);
        }

        private void SetSearchCriteriaDefaultValues(ref DataRow SingleDataRow)
        {
            // MessageBox.Show('SetSearchCriteriaDefaultValues called');
            SingleDataRow["PartnerName"] = "";
            SingleDataRow["PersonalName"] = "";
            SingleDataRow["PreviousName"] = "";
            SingleDataRow["Address1"] = "";
            SingleDataRow["Address2"] = "";
            SingleDataRow["Address3"] = "";
            SingleDataRow["City"] = "";
            SingleDataRow["PostCode"] = "";
            SingleDataRow["County"] = "";
            SingleDataRow["Country"] = "";
            SingleDataRow["MailingAddressOnly"] = false;
            SingleDataRow["PartnerClass"] = FDefaultPartnerClass;
            SingleDataRow["PartnerKey"] = 0;
            SingleDataRow["PartnerStatus"] = "ACTIVE";
            SingleDataRow["Email"] = "";
            SingleDataRow["LocationKey"] = "";
            SingleDataRow["WORKERFAMONLY"] = false;
            SingleDataRow["PhoneNumber"] = "";
            SingleDataRow["ExactPartnerKeyMatch"] = true;
            SingleDataRow["PartnerNameMatch"] = "BEGINS";
            SingleDataRow["PersonalNameMatch"] = "BEGINS";
            SingleDataRow["PreviousNameMatch"] = "BEGINS";
            SingleDataRow["Address1Match"] = "BEGINS";
            SingleDataRow["Address2Match"] = "BEGINS";
            SingleDataRow["Address3Match"] = "BEGINS";
            SingleDataRow["CityMatch"] = "BEGINS";
            SingleDataRow["PostCodeMatch"] = "BEGINS";
            SingleDataRow["CountyMatch"] = "BEGINS";
            SingleDataRow["EmailMatch"] = "BEGINS";
            SingleDataRow["PhoneNumberMatch"] = "BEGINS";

            if (cmbPartnerClass.Items.Count > 0)
            {
                FPartnerClassDataTable.Rows.Clear();

                if ((FRestrictedParterClasses == null)
                    || ((FRestrictedParterClasses.Length) == 0))
                {
                    // use these default values only if we don't have the restricted partner classen
                    // which means if we are not in modal mode.
                    InsertDefaultPartnerClassComboBox();
                }
                else
                {
                    InsertRestrictedPartnerClassComboBox(true);
                }

                cmbPartnerClass.ResetBindings();
                cmbPartnerClass.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void ResetSearchCriteriaValuesToDefault()
        {
            DataRow SingleDataRow;

            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                SingleDataRow = FFindCriteriaDataTable.Rows[0];
                SingleDataRow.BeginEdit();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                SingleDataRow.EndEdit();

                if (cmbPartnerClass.SelectedValue != null)
                {
                    // only do this if it is there on screen!
                    cmbPartnerClass.SelectedIndex = 0;
                }

                this.PartnerStatus = "ACTIVE";
            }
            else
            {
                SingleDataRow = FFindCriteriaDataTable.NewRow();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                FFindCriteriaDataTable.Rows.Add(SingleDataRow);
            }

            FFindCriteriaDataTable.Rows[0].BeginEdit();
            FDefaultValues.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;
            FShowAllPartnerClasses = true;
            this.EnableAllPanel();
            HandlePartnerClassGui();
        }

        /// <summary>
        /// Checks if the internal CriteriaDataTable has any criteria to search for.
        /// </summary>
        /// <returns>true if there are any search criterias </returns>
        public bool HasSearchCriteria()
        {
            if (FFindCriteriaDataTable.Rows.Count != 1)
            {
                return true;
            }

            DataRow SearchDataRow = FFindCriteriaDataTable.NewRow();
            SearchDataRow.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;

            for (int Counter = 0; Counter < SearchDataRow.ItemArray.Length; ++Counter)
            {
                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.EndsWith("Match"))
                {
                    // ignore changes of the Values like "ExactPartnerKeyMatch" or
                    // "EmailMatch" or "Address3Match"...
                    // because just a change in these values doesn't mean that there is a search criteria
                    continue;
                }

                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.CompareTo("PartnerStatus") == 0)
                {
                    if ((SearchDataRow[Counter].ToString() == "ALL")
                        || (SearchDataRow[Counter].ToString() == "ACTIVE"))
                    {
                        // if there is partner status "All" or "Active" marked
                        // treat it as if there is no search criteria selected
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }

                if (SearchDataRow[Counter].ToString() != FDefaultValues[Counter].ToString())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseCriteriaFields()
        {
            String LocalisedCountyLabel;
            String Dummy;

            ucoCountryComboBox.InitialiseUserControl();
            ucoCountryComboBox.AddNotSetRow("", "");
            ucoCountryComboBox.PerformDataBinding(FFindCriteriaDataTable, "Country");

            SetupPartnerClassComboBox();

            if (cmbPartnerClass.Items.Count > 0)
            {
                cmbPartnerClass.SelectedIndex = 0;
            }

            // Use Localised String for County Label
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Dummy);

            if (LocalisedCountyLabel == Ict.Petra.Client.App.Gui.LocalisedStrings.COUNTY_DEFAULT_LABEL)
            {
                /*
                 * / The default label text of the County Label has a shortcut character
                 * / that conflicts with other shortcut character on this screen, so we
                 * / remove the shortcut character.
                 */
                LocalisedCountyLabel = LocalisedCountyLabel.Replace("&", "");
            }

            lblCounty.Text = LocalisedCountyLabel;

            /*
             * Restore 'Mailing Addresses Only' and 'Partner Status' Criteria
             * settings from UserDefaults
             */
            FindCriteriaUserDefaultRestore();

            ShowOrHidePartnerKeyMatchInfoText();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AssociateCriteriaButtons()
        {
            critPartnerName.AssociatedTextBox = txtPartnerName;
            critPersonalName.AssociatedTextBox = txtPersonalName;
            critPreviousName.AssociatedTextBox = txtPreviousName;
            critAddress1.AssociatedTextBox = txtAddress1;
            critAddress2.AssociatedTextBox = txtAddress2;
            critAddress3.AssociatedTextBox = txtAddress3;
            critPostCode.AssociatedTextBox = txtPostCode;
            critCity.AssociatedTextBox = txtCity;
            critCounty.AssociatedTextBox = txtCounty;
            critEmail.AssociatedTextBox = txtEmail;
            critPhoneNumber.AssociatedTextBox = txtPhoneNumber;

            critPartnerName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPersonalName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPreviousName.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress1.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress2.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress3.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPostCode.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCity.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCounty.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critEmail.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPhoneNumber.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
        }

        /// <summary>
        /// Restores 'Mailing Addresses Only' and 'Partner Status' Criteria
        /// settings from UserDefaults.
        /// </summary>
        void FindCriteriaUserDefaultRestore()
        {
            String FindCurrentAddrOnlyUserDefault;
            String PartnerStatusUserDefault;

            FindCurrentAddrOnlyUserDefault = TUserDefaults.GetStringDefault(
                TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "NO").ToUpper();

//            MessageBox.Show("FindCriteriaUserDefaultRestore:  FindCurrentAddrOnlyUserDefault: " + FindCurrentAddrOnlyUserDefault);

            if (FindCurrentAddrOnlyUserDefault == "YES")
            {
                FFindCriteriaDataTable.Rows[0]["MailingAddressOnly"] = true;
            }

            PartnerStatusUserDefault = TUserDefaults.GetStringDefault(
                TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ACTIVE").ToUpper();

//            MessageBox.Show("FindCriteriaUserDefaultRestore:  PartnerStatusUserDefault: " + PartnerStatusUserDefault);

            if (PartnerStatusUserDefault == "ACTIVE")
            {
                rbtStatusActive.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "ACTIVE";
            }
            else if (PartnerStatusUserDefault == "ALL")
            {
                rbtStatusAll.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "ALL";
            }
            else
            {
                rbtPrivate.Checked = true;
                FFindCriteriaDataTable.Rows[0]["PartnerStatus"] = "PRIVATE";
            }
        }

        /// <summary>
        /// Saves 'Mailing Addresses Only' and 'Partner Status' Criteria
        /// settings to UserDefaults.
        /// </summary>
        public void FindCriteriaUserDefaultSave()
        {
            if (chkMailingAddressOnly.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "YES");
            }
            else
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_FINDCURRADDRONLY, "NO");
            }

            if (rbtStatusActive.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ACTIVE");
            }
            else if (rbtStatusAll.Checked)
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "ALL");
            }
            else
            {
                TUserDefaults.SetDefault(
                    TUserDefaults.PARTNER_FIND_CRIT_PARTNERSTATUS, "PRIVATE");
            }
        }

        private void TUC_PartnerFindCriteria_Load(System.Object sender, System.EventArgs e)
        {
            SingleLineFlow LayoutManagerLeftColumn;
            SingleLineFlow LayoutManagerRightColumn;

            LayoutManagerLeftColumn = new SingleLineFlow(pnlLeftColumn, 1, 1);
            LayoutManagerLeftColumn.SpacerDistance = 7;
            LayoutManagerRightColumn = new SingleLineFlow(pnlRightColumn, 1, 1);
            LayoutManagerRightColumn.SpacerDistance = 7;
            FWorkerFamOnly = false;
            FDefaultPartnerClass = "*";

            if (!DesignMode)
            {
                ResetSearchCriteriaValuesToDefault();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            ucoPartnerFind_PersonnelCriteria_CollapsiblePart.InitialiseUserControl();
            FCriteriaFieldsLeft = new ArrayList();
            FCriteriaFieldsRight = new ArrayList();

            // FFindCriteriaDataTable := new DataTable('FindCriteria');
            FFindCriteriaDataTable.ColumnChanging += new DataColumnChangeEventHandler(this.OnCriteriaChanging);

            // Set status bar texts
            FPetraUtilsObject.SetStatusBarText(txtPartnerName, Resourcestrings.StrPartnerNameFindHelptext);
            FPetraUtilsObject.SetStatusBarText(txtPersonalName, Resourcestrings.StrPersonalNameFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPreviousName, StrPreviousNameFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtEmail, Resourcestrings.StrEmailAddressHelpText);
            FPetraUtilsObject.SetStatusBarText(txtAddress1, Resourcestrings.StrAddress1Helptext);
            FPetraUtilsObject.SetStatusBarText(txtAddress2, Resourcestrings.StrAddress2Helptext);
            FPetraUtilsObject.SetStatusBarText(txtAddress3, Resourcestrings.StrAddress3Helptext);
            FPetraUtilsObject.SetStatusBarText(txtCity, Resourcestrings.StrCityHelptext);
            FPetraUtilsObject.SetStatusBarText(txtPostCode, Resourcestrings.StrPostCodeHelpText);
            FPetraUtilsObject.SetStatusBarText(txtCounty, Resourcestrings.StrCountyHelpText);
            FPetraUtilsObject.SetStatusBarText(cmbPartnerClass, StrPartnerClassFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtLocationKey, Resourcestrings.StrLocationKeyHelpText + Resourcestrings.StrLocationKeyExtraHelpText);
            FPetraUtilsObject.SetStatusBarText(btnLocationKey, Resourcestrings.StrLocationKeyButtonFindHelpText);
            FPetraUtilsObject.SetStatusBarText(chkWorkerFamOnly, StrWorkerFamilyOnlyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(rbtStatusActive, Resourcestrings.StrActivePartnersFindHelpText);
            FPetraUtilsObject.SetStatusBarText(rbtStatusAll, Resourcestrings.StrAllPartnersFindHelpText);
            FPetraUtilsObject.SetStatusBarText(chkMailingAddressOnly, Resourcestrings.StrMailingOnlyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPhoneNumber, Resourcestrings.StrPhoneNumberFindHelpText);
            FPetraUtilsObject.SetStatusBarText(txtPartnerKey, Resourcestrings.StrPartnerKeyFindHelpText);
            FPetraUtilsObject.SetStatusBarText(ucoCountryComboBox, Resourcestrings.StrCountryHelpText);
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns>void</returns>
        private void ReturnCriteriaFieldControls(out TSelfExpandingArrayList CriteriaFieldsLeftControls,
            out TSelfExpandingArrayList CriteriaFieldsRightControls)
        {
            Control TheControl;

            FieldInfo[] FieldsInForm;
            TSelfExpandingArrayList InternalCriteriaFieldLeftControls;
            TSelfExpandingArrayList InternalCriteriaFieldRightControls;
            Int32 Counter1;
            Int32 Counter3;
            Int32 PositionInArray;
            String SimplifiedControlName;
            Boolean FoundInLeftColumnArray;
            Boolean FoundInRightColumnArray;

            // init some variables:
            InternalCriteriaFieldLeftControls = null;
            InternalCriteriaFieldRightControls = null;

            // MessageBox.Show('FCriteriaFieldsLeft.Count: ' + FCriteriaFieldsLeft.Count.ToString + '; FCriteriaFieldsRight.Count: ' + FCriteriaFieldsRight.Count.ToString);
            if ((FCriteriaFieldsLeft.Count > 0) || (FCriteriaFieldsRight.Count > 0))
            {
                // FFindCriteriaDataTable.Columns.Clear();
                InternalCriteriaFieldLeftControls = new TSelfExpandingArrayList();
                InternalCriteriaFieldRightControls = new TSelfExpandingArrayList();
                FieldsInForm = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                for (Counter1 = 0; Counter1 <= FieldsInForm.Length - 1; Counter1 += 1)
                {
                    FoundInLeftColumnArray = false;
                    FoundInRightColumnArray = false;

                    // MessageBox.Show('Inspecting Field ''' + FieldsInForm[Counter1].Name + ''': FieldType: ' + FieldsInForm[Counter1].FieldType.ToString);
                    if (FieldsInForm[Counter1].FieldType.ToString() == "System.Windows.Forms.Panel")
                    {
                        if (FieldsInForm[Counter1].GetValue(this) is Control)
                        {
                            TheControl = (Control)FieldsInForm[Counter1].GetValue(this);

                            // MessageBox.Show('Inspecting Panel ''' + TheControl.Name + '''...' );
                            SimplifiedControlName = (TheControl.Name).Substring(3);
                            PositionInArray = FCriteriaFieldsLeft.IndexOf(SimplifiedControlName);

                            if (PositionInArray != -1)
                            {
                                FoundInLeftColumnArray = true;
                                FoundInRightColumnArray = false;
                            }
                            else
                            {
                                // MessageBox.Show('Processing SimplifiedControlName: ' + SimplifiedControlName);
                                PositionInArray = FCriteriaFieldsRight.IndexOf(SimplifiedControlName);

                                if (PositionInArray != -1)
                                {
                                    FoundInRightColumnArray = true;
                                    FoundInLeftColumnArray = false;
                                }
                            }

                            // MessageBox.Show('FoundInLeftColumnArray: ' + FoundInLeftColumnArray.ToString + '; FoundInRightColumnArray: ' +FoundInRightColumnArray.ToString);
                            TheControl.Tag = null;
                            TheControl.Visible = true;

                            if ((FoundInLeftColumnArray) || (FoundInRightColumnArray))
                            {
                                if (FoundInLeftColumnArray)
                                {
                                    // MessageBox.Show('Found Panel ''' + TheControl.Name + ''' in FCriteriaFieldsLeft @ position ' + PositionInArray.ToString );
                                    if (PositionInArray != 0)
                                    {
                                        if (FCriteriaFieldsLeft[PositionInArray - 1].ToString() == StrSpacer)
                                        {
                                            TheControl.Tag = StrBeginGroup;
                                        }
                                    }

                                    // MessageBox.Show('Adding TheControl ''' + TheControl.Name + ''' to InternalCriteriaFieldLeftControls @ Position ' + PositionInArray.ToString);
                                    InternalCriteriaFieldLeftControls[PositionInArray] = TheControl;

                                    // FFindCriteriaDataTable.Columns.Add(TheControl.Name.Substring(3));
                                }
                                else
                                {
                                    // MessageBox.Show('Found Panel ''' + TheControl.Name + ''' in FCriteriaFieldsRight @ position ' + PositionInArray.ToString );
                                    if (PositionInArray != 0)
                                    {
                                        if (FCriteriaFieldsRight[PositionInArray - 1].ToString() == StrSpacer)
                                        {
                                            TheControl.Tag = StrBeginGroup;
                                        }
                                    }

                                    // if InternalCriteriaFieldRightControls.Count  1 <= PositionInArray then
                                    // begin
                                    // InternalCriteriaFieldRightControls = new SomeType[PositionInArray + 1];
                                    // end;
                                    // MessageBox.Show('Adding TheControl ''' + TheControl.Name + ''' to InternalCriteriaFieldRightControls @ Position ' + PositionInArray.ToString);
                                    InternalCriteriaFieldRightControls[PositionInArray] = TheControl;

                                    // FFindCriteriaDataTable.Columns.Add(TheControl.Name.Substring(3));
                                }

                                // FoundInLeftColumnArray
                            }

                            // (FoundInLeftColumnArray) or (FoundInRightColumnArray
                        }

                        // FieldsInForm[Counter1].GetValue(this) is Control
                    }

                    // FieldsInForm[Counter1].FieldType.ToString = 'System.Windows.Forms.Panel'
                }

                // Counter1 := 0 To Length(FieldsInForm)  1
                // Check for and remove all empty entries (which could come from Spacers or invalid panel names)
                // MessageBox.Show('Compacting...');
                InternalCriteriaFieldLeftControls.Compact();
            }

            // (FCriteriaFieldsLeft.Count > 0) or (FCriteriaFieldsRight.Count > 0)
            // MessageBox.Show('Building dynamic output array...');
            // Build output dynamic array
            if (InternalCriteriaFieldLeftControls != null)
            {
                InternalCriteriaFieldLeftControls.TrimToSize();
                CriteriaFieldsLeftControls = InternalCriteriaFieldLeftControls;
            }
            else
            {
                CriteriaFieldsLeftControls = new TSelfExpandingArrayList();
            }

            if (InternalCriteriaFieldRightControls != null)
            {
                InternalCriteriaFieldRightControls.TrimToSize();
                CriteriaFieldsRightControls = InternalCriteriaFieldRightControls;
            }
            else
            {
                CriteriaFieldsRightControls = new TSelfExpandingArrayList();
            }

            if (FCriteriaSetupMode)
            {
                // If the number of items in FCriteriaFieldsLeft is not equal to
                // the number of items in CriteriaFieldsLeftControls then there
                // were invalid entries in FCriteriaFieldsLeft (that don't represent valid panels).
                // In this case rebuild FCriteriaFieldsLeft so that it contains only valid
                // entries.
                // MessageBox.Show('FCriteriaFieldsLeft.Count: ' + FCriteriaFieldsLeft.Count.ToString +
                // '; CriteriaFieldsLeftControls.Count  1: ' + Int16(CriteriaFieldsLeftControls.Count  1).ToString );
                if (FCriteriaFieldsLeft.Count != CriteriaFieldsLeftControls.Count - 1)
                {
                    FCriteriaFieldsLeft.Clear();

                    for (Counter3 = 0; Counter3 <= CriteriaFieldsLeftControls.Count - 1; Counter3 += 1)
                    {
                        if (((Control)CriteriaFieldsLeftControls[Counter3]).Tag != null)
                        {
                            // MessageBox.Show('CriteriaFieldsLeftControls[' + Counter3.ToString + ']:' + CriteriaFieldsLeftControls[Counter3].ToString + ' has Tag.');
                            if (((Control)CriteriaFieldsLeftControls[Counter3]).Tag.ToString() == StrBeginGroup)
                            {
                                FCriteriaFieldsLeft.Add(StrSpacer);

                                // MessageBox.Show('Adding Spacer before control ' + (CriteriaFieldsLeftControls[Counter3] as Control).Name);
                            }
                        }

                        FCriteriaFieldsLeft.Add((((Control)CriteriaFieldsLeftControls[Counter3]).Name).Substring(3));
                    }
                }

                // If the number of items in FCriteriaFieldsRight is not equal to
                // the number of items in CriteriaFieldsRightControls then there
                // were invalid entries in FCriteriaFieldsRight (that don't represent valid panels).
                // In this case rebuild FCriteriaFieldsRight so that it contains only valid
                // entries.
                // MessageBox.Show('FCriteriaFieldsRight.Count: ' + FCriteriaFieldsRight.Count.ToString +
                // '; CriteriaFieldsRightControls.Count  1: ' + Int16(CriteriaFieldsRightControls.Count  1).ToString );
                if (FCriteriaFieldsRight.Count != CriteriaFieldsRightControls.Count - 1)
                {
                    FCriteriaFieldsRight.Clear();

                    for (Counter3 = 0; Counter3 <= CriteriaFieldsRightControls.Count - 1; Counter3 += 1)
                    {
                        if (((Control)CriteriaFieldsRightControls[Counter3]).Tag != null)
                        {
                            // MessageBox.Show('CriteriaFieldsRightControls[' + Counter3.ToString + ']:' + CriteriaFieldsRightControls[Counter3].ToString + ' has Tag.');
                            if (((Control)CriteriaFieldsRightControls[Counter3]).Tag.ToString() == StrBeginGroup)
                            {
                                FCriteriaFieldsRight.Add(StrSpacer);

                                // MessageBox.Show('Adding Spacer before control' + (CriteriaFieldsRightControls[Counter3] as Control).Name);
                            }
                        }

                        FCriteriaFieldsRight.Add((((Control)CriteriaFieldsRightControls[Counter3]).Name).Substring(3));
                    }
                }
            }
        }

        private void CustomDisablePanels(Control ControlGroup)
        {
            Int16 Counter1;

            // Counter3: Int16;
            // InvisiblePanel: System.Windows.Forms.Panel;
            // TransparentRegion: System.Drawing.Region;
            for (Counter1 = 0; Counter1 <= ControlGroup.Controls.Count - 1; Counter1 += 1)
            {
                CustomDisableCriteria(ControlGroup.Controls[Counter1]);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="PanelName"></param>
        public void DisableCriteria(String PanelName)
        {
            Control TheControl;

            FieldInfo[] FieldsInForm;
            Int16 Counter1;
            FieldsInForm = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            for (Counter1 = 0; Counter1 <= FieldsInForm.Length - 1; Counter1 += 1)
            {
                // MessageBox.Show('Inspecting Field ''' + FieldsInForm[Counter1].Name + ''': FieldType: ' + FieldsInForm[Counter1].FieldType.ToString);
                if (FieldsInForm[Counter1].FieldType.ToString() == "System.Windows.Forms.Panel")
                {
                    if (FieldsInForm[Counter1].GetValue(this) is Control)
                    {
                        TheControl = (Control)FieldsInForm[Counter1].GetValue(this);

                        if (TheControl.Name == "pnl" + PanelName)
                        {
                            CustomDisableCriteria(TheControl);
                            return;
                        }
                    }
                }
            }
        }

        private void CustomDisableCriteria(Control ControlGroup)
        {
            Int16 Counter2;

            if (ControlGroup is Panel)
            {
                // MessageBox.Show('Disabling Controls in Panel: ' + ControlGroup.Name);
                for (Counter2 = 0; Counter2 <= ControlGroup.Controls.Count - 1; Counter2 += 1)
                {
                    // MessageBox.Show('Disabling Control: ' + ControlGroup.Controls[Counter2].Name);
                    if (ControlGroup.Controls[Counter2] is System.Windows.Forms.Label)
                    {
                        ControlGroup.Controls[Counter2].Click += new EventHandler(this.CorrespondingLabel_Click);
                    }
                    else if (ControlGroup.Controls[Counter2] is System.Windows.Forms.UserControl)
                    {
                        ControlGroup.Controls[Counter2].Click += new EventHandler(this.CorrespondingLabel_Click);
                        (ControlGroup.Controls[Counter2]).BackColor = this.BackColor;

                        // MessageBox.Show('Calling CustomDisablePanels for UC: ' + ControlGroup.Controls[Counter2].Name);
                        CustomDisablePanels(ControlGroup.Controls[Counter2]);
                    }
                    else
                    {
                        ControlGroup.Controls[Counter2].Enabled = false;
                    }
                }
            }
            else
            {
                CustomEnablingDisabling.DisableControl(ControlGroup.Parent, ControlGroup);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CompleteBindings()
        {
            this.BindingContext[CriteriaData].EndCurrentEdit();
        }

        private void CorrespondingLabel_Click(System.Object sender, System.EventArgs e)
        {
            if (!((((System.Windows.Forms.Control)sender).Parent).Parent is UserControl))
            {
                FSelectedPanel = SelectCriteriaPanel(((System.Windows.Forms.Control)sender).Name.Substring(3));
            }
            else
            {
                // MessageBox.Show('Klicked on Panel in UC ''' + ((sender as System.Windows.Forms.Control).Parent).Parent.Name + ''', selecting Panel ''' + (((sender as System.Windows.Forms.Control).Parent).Parent).Parent.Name + '''.');
                FSelectedPanel = SelectCriteriaPanel(((((System.Windows.Forms.Control)sender).Parent).Parent).Parent.Name.Substring(3));
                (((UserControl)((System.Windows.Forms.Control)sender).Parent).Parent).BackColor = System.Drawing.SystemColors.InactiveCaption;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACriteriaName"></param>
        /// <returns></returns>
        public TSelectedCriteriaPanel SelectCriteriaPanel(String ACriteriaName)
        {
            System.Windows.Forms.Panel SearchedPanel;
            System.Windows.Forms.Panel OtherPanel;
            Int32 Counter1;
            Int32 Counter2;
            Int32 NotFoundInPanelCounter;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            SearchedPanel = pnlLeftColumn;
            OtherPanel = pnlRightColumn;
            NotFoundInPanelCounter = 0;

            while (1 == 1)
            {
                for (Counter1 = 0; Counter1 <= SearchedPanel.Controls.Count - 1; Counter1 += 1)
                {
                    if (SearchedPanel.Controls[Counter1] is Panel)
                    {
                        if (SearchedPanel.Controls[Counter1].Name == "pnl" + ACriteriaName)
                        {
                            // Highlight the correct panel
                            // MessageBox.Show('Highlighting Panel: ' + SearchedPanel.Controls[Counter1].Name );
                            SearchedPanel.Controls[Counter1].BackColor = System.Drawing.SystemColors.InactiveCaption;

                            // Dehighlight all other panels in the SearchedPanel
                            for (Counter2 = 0; Counter2 <= SearchedPanel.Controls.Count - 1; Counter2 += 1)
                            {
                                if ((SearchedPanel.Controls[Counter2] is Panel)
                                    && (SearchedPanel.Controls[Counter2].Name != SearchedPanel.Controls[Counter1].Name))
                                {
                                    SearchedPanel.Controls[Counter2].BackColor = this.BackColor;

                                    // pnlPersonnelCriteria contains a UserControls which BackColor needs also to be reset
                                    if (SearchedPanel.Controls[Counter2].Name == "pnlPersonnelCriteria")
                                    {
                                        SearchedPanel.Controls[Counter2].Controls[0].BackColor = this.BackColor;
                                    }
                                }
                            }

                            // Dehighlight all panels in the OtherPanel
                            for (Counter2 = 0; Counter2 <= OtherPanel.Controls.Count - 1; Counter2 += 1)
                            {
                                if (OtherPanel.Controls[Counter2] is Panel)
                                {
                                    OtherPanel.Controls[Counter2].BackColor = this.BackColor;

                                    // pnlPersonnelCriteria contains a UserControls which BackColor needs also to be reset
                                    if (OtherPanel.Controls[Counter2].Name == "pnlPersonnelCriteria")
                                    {
                                        OtherPanel.Controls[Counter2].Controls[0].BackColor = this.BackColor;
                                    }
                                }
                            }

                            // Raise FindCriteriaSelectionChangedArgs event
                            FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                            FindCriteriaSelectionChangedArgs.SelectedCriteria = SearchedPanel.Controls[Counter1].Name.Substring(3);

                            if (SearchedPanel == pnlLeftColumn)
                            {
                                FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                            }
                            else
                            {
                                FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccRight;
                            }

                            FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                            FindCriteriaSelectionChangedArgs.IsLastInColumn = false;

                            if (Counter1 == 0)
                            {
                                FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                            }

                            if (Counter1 == SearchedPanel.Controls.Count - 1)
                            {
                                FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                            }

                            // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                            OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
                            return new TSelectedCriteriaPanel(((Panel)SearchedPanel.Controls[Counter1]),
                                FindCriteriaSelectionChangedArgs.CriteriaColumn,
                                FindCriteriaSelectionChangedArgs.IsFirstInColumn,
                                FindCriteriaSelectionChangedArgs.IsLastInColumn,
                                Counter1);
                        }
                    }
                }

                NotFoundInPanelCounter = NotFoundInPanelCounter + 1;

                if (NotFoundInPanelCounter == 1)
                {
                    // Panel with matching name not found in pnlLeftColumn, try in pnlRightColumn
                    SearchedPanel = pnlRightColumn;
                    OtherPanel = pnlLeftColumn;
                }
                else
                {
                    // No panel with matching name found!
                    return null;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACriteriaName"></param>
        /// <returns></returns>
        public Boolean UnSelectCriteriaPanel(String ACriteriaName)
        {
            System.Windows.Forms.Panel SearchedPanel;
            System.Windows.Forms.Panel OtherPanel;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 Counter1;
            Int32 NotFoundInPanelCounter;
            SearchedPanel = pnlLeftColumn;
            OtherPanel = pnlRightColumn;
            NotFoundInPanelCounter = 0;

            while (1 == 1)
            {
                for (Counter1 = 0; Counter1 <= SearchedPanel.Controls.Count - 1; Counter1 += 1)
                {
                    if (SearchedPanel.Controls[Counter1] is Panel)
                    {
                        if (SearchedPanel.Controls[Counter1].Name == "pnl" + ACriteriaName)
                        {
                            // Dehighlight the correct panel
                            // MessageBox.Show('Dehighlighting Panel: ' + SearchedPanel.Controls[Counter1].Name );
                            SearchedPanel.Controls[Counter1].BackColor = SearchedPanel.Controls[Counter1].Parent.BackColor;

                            if (!(FSelectedPanel == null))
                            {
                                if ("pnl" + ACriteriaName == FSelectedPanel.SelectedCriteriaPanel.Name)
                                {
                                    // Raise FindCriteriaSelectionChangedArgs event to signal that no criteria is selected
                                    FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                                    FindCriteriaSelectionChangedArgs.SelectedCriteria = null;
                                    FindCriteriaSelectionChangedArgs.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                                    FindCriteriaSelectionChangedArgs.IsLastInColumn = false;

                                    // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                                    OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
                                    FSelectedPanel = null;
                                }
                            }

                            return true;
                        }
                    }
                }

                NotFoundInPanelCounter = NotFoundInPanelCounter + 1;

                if (NotFoundInPanelCounter == 1)
                {
                    // Panel with matching name not found in pnlLeftColumn, try in pnlRightColumn
                    SearchedPanel = pnlRightColumn;
                    OtherPanel = pnlLeftColumn;
                }
                else
                {
                    // No panel with matching name found!
                    return false;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisplayCriteriaFieldControls()
        {
            TSelfExpandingArrayList CriteriaFieldsLeftControls;
            TSelfExpandingArrayList CriteriaFieldsRightControls;

            Control[] TempCriteriaFieldsLeftControls;
            Control[] TempCriteriaFieldsRightControls;

            Boolean MatchButtonsSetting;

            ReturnCriteriaFieldControls(out CriteriaFieldsLeftControls, out CriteriaFieldsRightControls);

            if (CriteriaFieldsLeftControls.Count != 0)
            {
                TempCriteriaFieldsLeftControls = new Control[CriteriaFieldsLeftControls.Count + 1];
                CriteriaFieldsLeftControls.CopyTo(TempCriteriaFieldsLeftControls);

                // remove all controls first
                pnlLeftColumn.Controls.Clear();

                // add specified controls
                pnlLeftColumn.Controls.AddRange(TempCriteriaFieldsLeftControls);
            }
            else
            {
                // remove all controls
                pnlLeftColumn.Controls.Clear();
            }

            MatchButtonsSetting = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_FINDOPTIONS_SHOWMATCHBUTTONS, true);

            if (CriteriaFieldsRightControls.Count != 0)
            {
                TempCriteriaFieldsRightControls = new Control[CriteriaFieldsRightControls.Count + 1];
                CriteriaFieldsRightControls.CopyTo(TempCriteriaFieldsRightControls);

                // remove all controls first
                pnlRightColumn.Controls.Clear();

                // add specified controls
                pnlRightColumn.Controls.AddRange(TempCriteriaFieldsRightControls);
            }
            else
            {
                // remove all controls
                pnlRightColumn.Controls.Clear();
            }

            // This section shows / hides the MatchButtons
            // and adjusts the textbox widths accordingly
            TidyPanel(ref pnlLeftColumn, 38, 7, MatchButtonsSetting);
            TidyPanel(ref pnlRightColumn, 38, 7, MatchButtonsSetting);
        }

        private void TidyPanel(ref Panel p, Int32 MatchButtonWidth, Int32 GapWidth, Boolean WithMatchButtons)
        {
            foreach (Control TempControl in p.Controls)
            {
                // tempcontroll
                foreach (Control TempInnerControl in TempControl.Controls)
                {
                    // tempInnerControl
                    if (TempInnerControl.GetType() == typeof(SplitButton))
                    {
                        // tempInnerControl.Type =      SplitButton
                        TempInnerControl.Visible = WithMatchButtons;
                    }

                    // tempInnerControl.Type =     SplitButton
                }

                // tempInnerControl
            }

            // tempcontroll
        }

        /// <summary>
        /// Shows or hides Match info text for Partner Key Crtieria
        /// </summary>
        public void ShowOrHidePartnerKeyMatchInfoText()
        {
            // Show Match info for Partner Key if Non-Exact Matching is enabled
            if (!TUserDefaults.GetBooleanDefault(
                    TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                    true))
            {
                lblPartnerKeyNonExactMatch.Visible = true;
            }
            else
            {
                lblPartnerKeyNonExactMatch.Visible = false;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="args"></param>
        protected void OnFindCriteriaSelectionChanged(TPartnerFindCriteriaSelectionChangedEventArgs args)
        {
            if (FindCriteriaSelectionChanged != null)
            {
                FindCriteriaSelectionChanged(this, args);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveUpSelectedCriteriaPanel()
        {
            ArrayList CriteriaFieldsArrayList;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 CurrentIndexInFieldsArrayList;

            if (!FSelectedPanel.IsFirstInColumn)
            {
                // MessageBox.Show('Moving item up');
                if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsLeft;
                }
                else
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsRight;
                }

                CurrentIndexInFieldsArrayList = CriteriaFieldsArrayList.IndexOf(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Insert(CurrentIndexInFieldsArrayList - 1, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                FSelectedPanel.PositionInCriteriaColumn = FSelectedPanel.PositionInCriteriaColumn - 1;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (CurrentIndexInFieldsArrayList - 1 == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveDownSelectedCriteriaPanel()
        {
            ArrayList CriteriaFieldsArrayList;
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;
            Int32 CurrentIndexInFieldsArrayList;

            if (!FSelectedPanel.IsLastInColumn)
            {
                // MessageBox.Show('Moving item down');
                if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsLeft;
                }
                else
                {
                    CriteriaFieldsArrayList = FCriteriaFieldsRight;
                }

                CurrentIndexInFieldsArrayList = CriteriaFieldsArrayList.IndexOf(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                CriteriaFieldsArrayList.Insert(CurrentIndexInFieldsArrayList + 1, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                FSelectedPanel.PositionInCriteriaColumn = FSelectedPanel.PositionInCriteriaColumn + 1;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (CurrentIndexInFieldsArrayList + 1 == CriteriaFieldsArrayList.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveLeftSelectedCriteriaPanel()
        {
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;

            if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccRight)
            {
                FCriteriaFieldsRight.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));

                // MessageBox.Show('PositionInCriteriaColumn: ' + FSelectedPanel.PositionInCriteriaColumn.ToString +
                // 'FCriteriaFieldsLeft.Count  1: ' + Int16(FCriteriaFieldsLeft.Count  1).ToString);
                if (FSelectedPanel.PositionInCriteriaColumn <= FCriteriaFieldsLeft.Count - 1)
                {
                    FCriteriaFieldsLeft.Insert(FSelectedPanel.PositionInCriteriaColumn, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }
                else
                {
                    FSelectedPanel.PositionInCriteriaColumn = FCriteriaFieldsLeft.Add(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }

                FSelectedPanel.CriteriaColumn = TFindCriteriaColumn.fccLeft;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (FSelectedPanel.PositionInCriteriaColumn == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                if (FSelectedPanel.PositionInCriteriaColumn == FCriteriaFieldsLeft.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void MoveRightSelectedCriteriaPanel()
        {
            TPartnerFindCriteriaSelectionChangedEventArgs FindCriteriaSelectionChangedArgs;

            if (FSelectedPanel.CriteriaColumn == TFindCriteriaColumn.fccLeft)
            {
                FCriteriaFieldsLeft.Remove(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));

                // MessageBox.Show('PositionInCriteriaColumn: ' + FSelectedPanel.PositionInCriteriaColumn.ToString +
                // 'FCriteriaFieldsRight.Count  1: ' + Int16(FCriteriaFieldsRight.Count  1).ToString);
                if (FSelectedPanel.PositionInCriteriaColumn <= FCriteriaFieldsRight.Count - 1)
                {
                    FCriteriaFieldsRight.Insert(FSelectedPanel.PositionInCriteriaColumn, FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }
                else
                {
                    FSelectedPanel.PositionInCriteriaColumn = FCriteriaFieldsRight.Add(FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3));
                }

                FSelectedPanel.CriteriaColumn = TFindCriteriaColumn.fccRight;
                DisplayCriteriaFieldControls();

                // Raise FindCriteriaSelectionChangedArgs event and update IsFirstInColumn + IsLastInColumn
                FindCriteriaSelectionChangedArgs = new TPartnerFindCriteriaSelectionChangedEventArgs();
                FindCriteriaSelectionChangedArgs.SelectedCriteria = FSelectedPanel.SelectedCriteriaPanel.Name.Substring(3);
                FindCriteriaSelectionChangedArgs.CriteriaColumn = FSelectedPanel.CriteriaColumn;
                FindCriteriaSelectionChangedArgs.IsFirstInColumn = false;
                FindCriteriaSelectionChangedArgs.IsLastInColumn = false;
                FSelectedPanel.IsFirstInColumn = false;
                FSelectedPanel.IsLastInColumn = false;

                if (FSelectedPanel.PositionInCriteriaColumn == 0)
                {
                    FindCriteriaSelectionChangedArgs.IsFirstInColumn = true;
                    FSelectedPanel.IsFirstInColumn = true;
                }

                if (FSelectedPanel.PositionInCriteriaColumn == FCriteriaFieldsRight.Count - 1)
                {
                    FindCriteriaSelectionChangedArgs.IsLastInColumn = true;
                    FSelectedPanel.IsLastInColumn = true;
                }

                // MessageBox.Show('Raising FindCriteriaSelectionChanged event.');
                OnFindCriteriaSelectionChanged(FindCriteriaSelectionChangedArgs);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnCriteriaChanging(System.Object sender, DataColumnChangeEventArgs args)
        {
            // MessageBox.Show(System.String.Format("Column_Changing Event: name=0; Column=1; proposed name=2",
            //                                      args.Row["name"], args.Column.ColumnName, args.ProposedValue) );

            if (args.ProposedValue.ToString() != FFindCriteriaDataTable.Rows[0][args.Column.ColumnName].ToString())
            {
                FFindCriteriaDataTable.Rows[0].EndEdit();

                //         MessageBox.Show("Value changed in " + FFindCriteriaDataTable.Columns[args.Column.ColumnName].ColumnName + ": " +Environment.NewLine +
                //                         "FFindCriteriaDataTable.Rows[0][args.Column.ColumnName]: " + FFindCriteriaDataTable.Rows[0][args.Column.ColumnName].ToString() + "; " +
                //                         "args.ProposedValue: " + args.ProposedValue.ToString());

                // Fire event
                if (OnCriteriaContentChanged != null)
                {
                    OnCriteriaContentChanged(this, EventArgs.Empty);
                }
            }
        }

        private void DisableAllPanel(Panel butNotThisOne)
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                if (TempPanel.Equals(butNotThisOne) == false)
                {
                    TempPanel.Enabled = false;
                }
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                if (TempPanel.Equals(butNotThisOne) == false)
                {
                    TempPanel.Enabled = false;
                }
            }
        }

        private void EnableAllPanel()
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                TempPanel.Enabled = true;
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                TempPanel.Enabled = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APassedLocationKey"></param>
        public void FocusLocationKey(Int32 APassedLocationKey)
        {
            // First make sure that the LocationKey Panel is there...
            if (!pnlRightColumn.Controls.Contains(pnlLocationKey))
            {
                pnlRightColumn.Controls.Add(pnlLocationKey);
            }

            // Set Focus on txtLocationKey
            txtLocationKey.Focus();

            // Set Text if APassedLocationKey is passed in
            if (APassedLocationKey != -1)
            {
                txtLocationKey.Text = APassedLocationKey.ToString();
                txtLocationKey.SelectAll();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AEnabled"></param>
        public void SetMatchButtonFunctionality(Boolean AEnabled)
        {
            foreach (Control TempPanel in pnlLeftColumn.Controls)
            {
                foreach (Control TempCtrl in TempPanel.Controls)
                {
                    if (TempCtrl.GetType() == typeof(SplitButton))
                    {
                        TempCtrl.Enabled = AEnabled;
                    }
                }
            }

            foreach (Control TempPanel in pnlRightColumn.Controls)
            {
                foreach (Control TempCtrl in TempPanel.Controls)
                {
                    if (TempCtrl.GetType() == typeof(SplitButton))
                    {
                        TempCtrl.Enabled = AEnabled;
                    }
                }
            }
        }
    }

    /// <summary>todoComment</summary>
    public delegate void FindCriteriaSelectionChangedHandler(System.Object Sender, TPartnerFindCriteriaSelectionChangedEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public class TSelectedCriteriaPanel : System.Object
    {
        private System.Windows.Forms.Panel FSelectedCriteriaPanel;
        private TFindCriteriaColumn FCriteriaColumn;
        private Boolean FIsFirstInColumn;
        private Boolean FIsLastInColumn;
        private Int32 FPositionInCriteriaColumn;

        /// <summary>todoComment</summary>
        public System.Windows.Forms.Panel SelectedCriteriaPanel
        {
            get
            {
                return FSelectedCriteriaPanel;
            }

            set
            {
                FSelectedCriteriaPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public TFindCriteriaColumn CriteriaColumn
        {
            get
            {
                return FCriteriaColumn;
            }

            set
            {
                FCriteriaColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsFirstInColumn
        {
            get
            {
                return FIsFirstInColumn;
            }

            set
            {
                FIsFirstInColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean IsLastInColumn
        {
            get
            {
                return FIsLastInColumn;
            }

            set
            {
                FIsLastInColumn = value;
            }
        }

        /// <summary>todoComment</summary>
        public Int32 PositionInCriteriaColumn
        {
            get
            {
                return FPositionInCriteriaColumn;
            }

            set
            {
                FPositionInCriteriaColumn = value;
            }
        }


        #region TSelectedCriteriaPanel

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="SelectedCriteriaPanel"></param>
        /// <param name="CriteriaColumn"></param>
        /// <param name="IsFirstInColumn"></param>
        /// <param name="IsLastInColumn"></param>
        /// <param name="PositionInCriteriaColumn"></param>
        public TSelectedCriteriaPanel(System.Windows.Forms.Panel SelectedCriteriaPanel,
            TFindCriteriaColumn CriteriaColumn,
            Boolean IsFirstInColumn,
            Boolean IsLastInColumn,
            Int32 PositionInCriteriaColumn) :
            base()
        {
            FSelectedCriteriaPanel = SelectedCriteriaPanel;
            FCriteriaColumn = CriteriaColumn;
            FIsFirstInColumn = IsFirstInColumn;
            FIsLastInColumn = IsLastInColumn;
            FPositionInCriteriaColumn = PositionInCriteriaColumn;
        }

        #endregion
    }
}