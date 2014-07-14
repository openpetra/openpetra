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
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using GNU.Gettext;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>Button Click</summary>
    public delegate void TDelegateClickButton(String LabelStringIn,
        String TextBoxStringIn,
        out String LabelStringOut,
        out String PartnerClassOut,
        out String TextBoxStringOut);

    /// <summary>Text box edited</summary>
    public delegate bool TDelegateVerifyUserEntry(String AValueToVerify, out String AResult);

    /// <summary>Partner found</summary>
    public delegate void TDelegatePartnerFound(Int64 APartnerKey, Int32 ALocationKey);

    /// <summary>Partner found</summary>
    public delegate void TDelegatePartnerChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection);

    /// <summary>Partner found</summary>
    public delegate void TDelegateOccupationFound(string AOccupationCode);

    /// <summary>Dataset changed</summary>
    public delegate void TDelegateDatasetChanged(DataSet ADataset);

    class txtAutoPopulatedButtonLabel
    {
        /// <summary>Exception message</summary>
        public const String EXCEPTION_CLICK_BUTTON = "Delegate data not correct! ";

        /// <summary>Exception message</summary>
        public const String EXCEPTION_WRONG_DATATYPE = "DataSource is not of type System.Data.DataTable or System.Data.DataView";

        /// <summary>Exception message</summary>
        public const String EXCEPTION_VERIFIC_MISSING = "To this type of TextBox the following Delegate has to be assigned: ";
    }

    /// <summary>
    /// Defines the error message
    /// </summary>
    public class TErrorData
    {
        /// <summary>Error message</summary>
        public String RErrorMessage;

        /// <summary>Caption text</summary>
        public String RCaption;

        /// <summary>Error Code</summary>
        public string RErrorCode;

        /// <summary>False if we have an error message</summary>
        public bool RVerified;
    }

    /// <summary>
    /// Petra CustomControls
    /// This unit provides a TextBox with adjacent Button and Label.
    /// The TextBox is prepared for AutoPopulation
    ///
    /// Short description on how to add other TextBox types:
    ///  1. Add the new type to the following list: "TListTableEnum"
    ///  2. Add the code for the appearance of this type to the function "AppearanceSetup"
    ///  3. Add the code for the initialization of this type to the function "InitialiseUserControl"
    ///  4. Add the code for user entry varification to the function "OnDataboundTableColumnChanged"
    ///  5. Add the code for the button action to the function "txtAutoPopulated_ButtonClick"
    ///  6. Add the code for a formating routine if needed in the function "txtAutoPopulated_FormatText"
    ///  7. Add the code for the initial display of the Label in the function "txtAutoPopulated_Paint"
    ///     This step is only necessary if you do not use a LookUpTable!
    ///  8. Add the code for the Label display to the function "txtAutoPopulated_SetLabel"
    ///  9. Add the code for databinding to the function "PerformDataBinding"
    /// </summary>
    public partial class TtxtAutoPopulatedButtonLabel : System.Windows.Forms.UserControl
    {
        /// <summary>Error message</summary>
        private static readonly string UNIT_NO_DATA_MESSAGE = Catalog.GetString("NOT A VALID VALUE!");

        /// <summary>Error message</summary>
        private static readonly string UNIT_NO_VALID_OCCUPATIONCODE = Catalog.GetString("NOT A VALID OCCUPATION CODE!");

// TODO        /// <summary>Error message</summary>
// TODO       private static readonly string UNIT_NO_VALID_PARTNERKEY = Catalog.GetString("NOT A VALID PARTNERKEY!");

        private Color? FOriginalPartnerClassColor = null;

        /// <summary>
        /// Available Types for TtxtAutoPopulatedButtonLabel
        /// </summary>
        public enum TListTableEnum
        {
            /// <summary>Type occupation list</summary>
            OccupationList,

            /// <summary>Type partner key</summary>
            PartnerKey,

            /// <summary>Type extract</summary>
            Extract,

            /// <summary>Type conference</summary>
            Conference,

            /// <summary>Type event</summary>
            Event,

            /// <summary>Type bank</summary>
            Bank
        };

        /// <summary></summary>
        public const String UNIT_ALLOWED_DATATYPES = "System.Data.DataView";

        /// <summary></summary>
        public const Int32 UNIT_DEFAULT_HEIGHT = 23;

        /// <summary>Error message</summary>
        public const String UNIT_DELIMITERS_PARTNERCLASS = ",";

        /// <summary>private bool FAutomaticallyUpdateDataSource;</summary>
        protected TListTableEnum FListTable;

        /// <summary></summary>
        protected DataSet FDataset;

        /// <summary></summary>
        protected bool FUserControlInitialised;

        /// <summary></summary>
        protected bool FASpecialSetting;

        /// <summary></summary>
        protected int FButtonWidth;

        /// <summary></summary>
        protected int FDefaultButtonWidth;

        /// <summary></summary>
        protected int FTextBoxWidth;

        /// <summary></summary>
        protected int FDefaultTextBoxWidth;

        /// <summary></summary>
        protected String FButtonText;

        /// <summary></summary>
        protected String FDefaultButtonText;

        /// <summary></summary>
        protected System.Drawing.ContentAlignment FButtonTextAlign;

        /// <summary></summary>
        protected System.Drawing.ContentAlignment FDefaultButtonTextAlign;

        /// <summary></summary>
        protected String FPartnerClass;

        /// <summary></summary>
        protected String FValueMember;

        /// <summary></summary>
        protected String FVerifiedString;

        /// <summary></summary>
        protected int FLookUpColumnIndex;

        /// <summary></summary>
        protected String FDisplayLabelString;

        /// <summary></summary>
        protected TErrorData FErrorData;

        /// <summary></summary>
        protected bool FPreventFaultyLeaving;

        /// <summary></summary>
        protected TVerificationResultCollection FVerificationResultCollection;

        /// <summary>
        /// Special property to determine whether our code is running in the WinForms Designer.
        /// The result of this property is correct even if InitializeComponent() wasn't run yet
        /// (.NET's DesignMode property returns false in that case)!
        /// </summary>
        private bool InDesignMode
        {
            get
            {
                return (this.GetService(typeof(IDesignerHost)) != null)
                       || (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime);
            }
        }

        /// <summary>
        /// control is in the Special Settings Mode.
        /// If this function returns TRUE it is in the Special Settings Mode. This
        /// means the programmer MUST set all Special Settings!!!
        /// </summary>
        public bool ASpecialSetting
        {
            get
            {
                return this.FASpecialSetting;
            }

            set
            {
                this.FASpecialSetting = value;
            }
        }

        /// <summary>
        /// ButtonWidth if the typ 'PartnerKey' from the ListTable ListTable property is chosen.
        /// </summary>
        public int ButtonWidth
        {
            get
            {
                int ReturnValue;

                if (this.FASpecialSetting == true)
                {
                    ReturnValue = this.FButtonWidth;
                }
                else
                {
                    ReturnValue = this.txtAutoPopulated.ButtonWidth;
                }

                return ReturnValue;
            }

            set
            {
                if (this.FASpecialSetting == true)
                {
                    this.FButtonWidth = value;
                    this.txtAutoPopulated.ButtonWidth = value;
                }
                else
                {
                    this.FButtonWidth = this.txtAutoPopulated.ButtonWidth;
                }
            }
        }

        /// <summary>
        /// ButtonText if the type 'PartnerKey' from the ListTable property is chosen.
        /// </summary>
        public String ButtonText
        {
            get
            {
                String ReturnValue;

                if (this.FASpecialSetting == true)
                {
                    if (this.FButtonText == "")
                    {
                        ReturnValue = this.txtAutoPopulated.ButtonText;
                    }
                    else
                    {
                        ReturnValue = this.FButtonText;
                    }
                }
                else
                {
                    ReturnValue = this.txtAutoPopulated.ButtonText;
                }

                return ReturnValue;
            }

            set
            {
                if (this.FASpecialSetting == true)
                {
                    this.FButtonText = value;
                    this.txtAutoPopulated.ButtonText = value;
                }
                else
                {
                    this.FButtonText = this.txtAutoPopulated.ButtonText;
                }
            }
        }

        /// <summary>
        /// alignmet of the text that will be displayed in the face of the button.
        /// </summary>
        public System.Drawing.ContentAlignment ButtonTextAlign
        {
            get
            {
                System.Drawing.ContentAlignment ReturnValue;

                if (this.FASpecialSetting == true)
                {
                    ReturnValue = this.FButtonTextAlign;
                }
                else
                {
                    ReturnValue = this.txtAutoPopulated.ButtonTextAlign;
                }

                return ReturnValue;
            }

            set
            {
                if (this.FASpecialSetting == true)
                {
                    this.FButtonTextAlign = value;
                    this.txtAutoPopulated.ButtonTextAlign = value;
                }
                else
                {
                    this.FButtonTextAlign = this.txtAutoPopulated.ButtonTextAlign;
                }
            }
        }

        /// <summary>
        /// This property determines which cached DataTable should make up the list of entries.
        /// </summary>
        public TListTableEnum ListTable
        {
            get
            {
                return this.FListTable;
            }

            set
            {
                // Tlogging.Log('Start set_ListTable:');
                this.FListTable = value;
                this.InitialiseUserControl();
                this.AppearanceSetup(this.FListTable);

                // Tlogging.Log('End set_ListTable:');
            }
        }

        /// <summary>
        /// This property allows a filled dataset to be used to make up the list of entries. (Currently only used for Bank.)
        /// </summary>
        public DataSet DataSet
        {
            get
            {
                return this.FDataset;
            }

            set
            {
                this.FDataset = value;
            }
        }

        /// <summary>
        /// This procedure gets whether the user can leave this control even if entered faulty data.
        /// </summary>
        public bool PreventFaultyLeaving
        {
            get
            {
                return this.FPreventFaultyLeaving;
            }

            set
            {
                this.FPreventFaultyLeaving = value;
            }
        }

        /// <summary>
        /// This property gets or sets the maximum number of characters the user can
        /// type or paste into the text box control.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this.txtAutoPopulated.txtTextBox.MaxLength;
            }

            set
            {
                this.txtAutoPopulated.txtTextBox.MaxLength = value;
            }
        }

        /// <summary>
        /// This property determines which cached DataTable should make up the list of
        /// entries.
        /// </summary>
        public String PartnerClass
        {
            get
            {
                return this.FPartnerClass;
            }

            set
            {
                this.FPartnerClass = FormatPartnerClassString(value);

                TCommonControlsHelper.SetPartnerKeyBackColour(value, txtAutoPopulated.txtTextBox, FOriginalPartnerClassColor);
            }
        }

        /// <summary>
        /// This property determines whether the text in the edit control can be
        /// changed or not.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return this.txtAutoPopulated.ReadOnly;
            }

            set
            {
                this.txtAutoPopulated.ReadOnly = value;
            }
        }

        /// <summary>
        /// This property gets and sets the Text of this control.
        /// </summary>
        public new string Text
        {
            get
            {
                return this.txtAutoPopulated.txtTextBox.Text;
            }

            set
            {
                this.txtAutoPopulated.txtTextBox.Text = value;
                UpdateDisplayedValue();
            }
        }

        /// <summary>
        /// This property gets the Text of the text box label control.
        /// </summary>
        public string LabelText
        {
            get
            {
                return this.txtAutoPopulated.lblLabel.Text;
            }
        }

        /// <summary>
        /// TextBox Width if the type 'PartnerKey' from the ListTable property is chosen.
        /// </summary>
        public int TextBoxWidth
        {
            get
            {
                int ReturnValue;

                if (this.FASpecialSetting == true)
                {
                    if (this.FTextBoxWidth == 0)
                    {
                        ReturnValue = this.txtAutoPopulated.TextBoxWidth;
                    }
                    else
                    {
                        ReturnValue = this.FTextBoxWidth;
                    }
                }
                else
                {
                    ReturnValue = this.txtAutoPopulated.TextBoxWidth;
                }

                return ReturnValue;
            }

            set
            {
                if (this.FASpecialSetting == true)
                {
                    this.FTextBoxWidth = value;
                    this.txtAutoPopulated.TextBoxWidth = value;
                }
                else
                {
                    this.FTextBoxWidth = this.txtAutoPopulated.TextBoxWidth;
                }
            }
        }

        /// <summary>
        /// This property gets and sets the Text of this control.
        /// </summary>
        public CharacterCasing CharacterCasing
        {
            set
            {
                this.txtAutoPopulated.txtTextBox.CharacterCasing = value;
            }
        }

        /// <summary>
        /// Sets the BorderStyle of the underlying TextBox.
        /// </summary>
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this.txtAutoPopulated.BorderStyle;
            }
            set
            {
                this.txtAutoPopulated.BorderStyle = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which sets the Label's and TextBox's Texts.
        /// </summary>
        public event TDelegateClickButton ClickButton;

        /// <summary>
        /// This property is used to provide a function which sets the Label's and TextBox's Texts.
        /// </summary>
        public event TDelegatePartnerFound PartnerFound;

        /// <summary>
        /// This property is used to provide a function which sets the Label's and TextBox's Texts.
        /// </summary>
        public event TDelegatePartnerChanged ValueChanged;

        /// <summary>
        /// This property is used to provide a function which sets the Label's and TextBox's Texts.
        /// </summary>
        public event TDelegateOccupationFound OccupationFound;

        /// <summary>
        /// This property is used to provide a function which sets the Label's and TextBox's Texts.
        /// </summary>
        public event TDelegateVerifyUserEntry VerifyUserEntry;

        /// <summary>
        /// This property is used to provide a function which sets calling screen's dataset.
        /// </summary>
        public event TDelegateDatasetChanged DatasetChanged;

        /// <summary>
        /// Here the hosting form has to provide a function to come up with a partner short name.
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }

            set
            {
                FVerificationResultCollection = value;
            }
        }

        /// <summary>
        /// This Property allows the control to automaticall update the datasource. You normally DONT want this!
        /// </summary>
        public bool AutomaticallyUpdateDataSource
        {
            get
            {
                return this.txtAutoPopulated.AutomaticallyUpdateDataSource;
            }

            set
            {
                this.txtAutoPopulated.AutomaticallyUpdateDataSource = value;
            }
        }

        /// <summary>
        /// This property determines whether the label is blank or not.
        /// </summary>
        public bool ShowLabel
        {
            get
            {
                return this.txtAutoPopulated.ShowLabel;
            }

            set
            {
                this.txtAutoPopulated.ShowLabel = value;
            }
        }

        /// <summary>
        /// This property determines whether the label is visible or not.
        /// </summary>
        public bool LabelVisible
        {
            get
            {
                return this.txtAutoPopulated.lblLabel.Visible;
            }

            set
            {
                this.txtAutoPopulated.lblLabel.Visible = value;
            }
        }


        #region Creation and disposal

        /// <summary>
        /// Private Declarations }
        /// This is the constructor of this class.
        /// </summary>
        /// <returns>void</returns>
        public TtxtAutoPopulatedButtonLabel()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            this.FPreventFaultyLeaving = false;
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.txtAutoPopulated.ButtonText = Catalog.GetString("btnButton");
            this.txtAutoPopulated.LabelText = Catalog.GetString("lblLabel");
            this.txtAutoPopulated.TextBoxText = Catalog.GetString("txtTextBox");
            #endregion

            this.FUserControlInitialised = false;
            FDisplayLabelString = "";
            this.txtAutoPopulated.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                           System.Windows.Forms.AnchorStyles.Left |
                                           System.Windows.Forms.AnchorStyles.Right;
            this.FVerifiedString = "";
            this.ASpecialSetting = true;

            // TLogging.Log('Creation and Disposal');
            // TLogging.Log('CreationButtonTextAlign: ' + Enum.GetName(typeof(System.Drawing.ContentAlignment), this.FButtonTextAlign));
            ASpecialSetting = false;
            FErrorData = new TErrorData();
        }

        /// <summary>
        /// This procedure sets the appearance of this control according to the data contained.
        /// </summary>
        /// <returns>void</returns>
        public void AppearanceSetup(TListTableEnum AListTable)
        {
            System.Single mFontSize;
            System.Drawing.Font mFont;

            // TLogging.Log('ButtonTextAlign: ' + Enum.GetName(typeof(System.Drawing.ContentAlignment), this.FButtonTextAlign));
            switch (AListTable)
            {
                case TListTableEnum.OccupationList:
                    #region TListTableEnum.OccupationList

                    // Settings for the button
                    this.FDefaultButtonText = Catalog.GetString("&Occupation...");
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;

                    // Setting the TextBox
                    this.FDefaultTextBoxWidth = 170;

                    // Layout
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    this.txtAutoPopulated.SeparatorWidth = 2;
                    this.txtAutoPopulated.LabelSeparatorWidth = 6;
                    #endregion
                    break;

                case TListTableEnum.PartnerKey:
                    #region TListTableEnum.PartnerKey

                    // Settings for the button
                    this.FDefaultButtonText = String.Format(Catalog.GetString("&{0}"), ApplWideResourcestrings.StrPartnerKey);
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;
                    this.FDefaultTextBoxWidth = 80;
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    mFontSize = this.txtAutoPopulated.txtTextBox.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.txtAutoPopulated.txtTextBox.Font = mFont;
                    this.FLookUpColumnIndex = -1;
                    this.txtAutoPopulated.txtTextBox.Text = "0000000000";
                    this.txtAutoPopulated.Size = this.Size;

                    if (ShowLabel)
                    {
                        this.txtAutoPopulated.SetLabel += new TDelegateSetLabel(this.TxtAutoPopulated_SetLabel);
                    }
            
                    AddCustomContextMenuStrip();

                    #endregion
                    break;

                case TListTableEnum.Extract:
                    #region TListTableEnum.Extract

                    // Settings for the button
                    this.FDefaultButtonText = Catalog.GetString("&Extract");
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;
                    this.FDefaultTextBoxWidth = 80;
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    mFontSize = this.txtAutoPopulated.txtTextBox.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.txtAutoPopulated.txtTextBox.Font = mFont;
                    this.FLookUpColumnIndex = -1;
                    this.txtAutoPopulated.txtTextBox.Text = "";
                    this.txtAutoPopulated.Size = this.Size;

                    if (ShowLabel)
                    {
                        this.txtAutoPopulated.SetLabel += new TDelegateSetLabel(this.TxtAutoPopulated_SetLabel);
                    }

                    #endregion
                    break;

                case TListTableEnum.Conference:
                    #region TListTableEnum.Conference

                    /* Settings for the button */
                    this.FDefaultButtonText = Catalog.GetString("&Conference");
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;
                    this.FDefaultTextBoxWidth = 80;
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    mFontSize = this.txtAutoPopulated.txtTextBox.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.txtAutoPopulated.txtTextBox.Font = mFont;
                    this.FLookUpColumnIndex = -1;
                    this.txtAutoPopulated.txtTextBox.Text = "0000000000";
                    this.txtAutoPopulated.Size = this.Size;
                    this.txtAutoPopulated.SetLabel += new TDelegateSetLabel(this.TxtAutoPopulated_SetLabel);
            
                    AddCustomContextMenuStrip();
                    
                    #endregion
                    break;

                case TListTableEnum.Event:
                    #region TListTableEnum.Event

                    /* Settings for the button */
                    this.FDefaultButtonText = Catalog.GetString("&Event");
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;
                    this.FDefaultTextBoxWidth = 80;
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    mFontSize = this.txtAutoPopulated.txtTextBox.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.txtAutoPopulated.txtTextBox.Font = mFont;
                    this.FLookUpColumnIndex = -1;
                    this.txtAutoPopulated.txtTextBox.Text = "0000000000";
                    this.txtAutoPopulated.Size = this.Size;
                    this.txtAutoPopulated.SetLabel += new TDelegateSetLabel(this.TxtAutoPopulated_SetLabel);
            
                    AddCustomContextMenuStrip();
                    
                    #endregion
                    break;

                case TListTableEnum.Bank:
                    #region TListTableEnum.Event

                    /* Settings for the button */
                    this.FDefaultButtonText = Catalog.GetString("&Bank");
                    this.FDefaultButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.FDefaultButtonWidth = 108;
                    this.FDefaultTextBoxWidth = 80;
                    this.txtAutoPopulated.AdjustButtonWidth = false;
                    mFontSize = this.txtAutoPopulated.txtTextBox.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.txtAutoPopulated.txtTextBox.Font = mFont;
                    this.FLookUpColumnIndex = -1;
                    this.txtAutoPopulated.txtTextBox.Text = "0000000000";
                    this.txtAutoPopulated.Size = this.Size;

                    if (ShowLabel)
                    {
                        this.txtAutoPopulated.SetLabel += new TDelegateSetLabel(this.TxtAutoPopulated_SetLabel);
                    }
            
                    AddCustomContextMenuStrip();

                    #endregion
                    break;
            }

            if (this.FASpecialSetting == true)
            {
                #region Special Settings apply

                // TLogging.Log('this.FASpecialSetting = true  START');
                // ButtonText
                if (this.ButtonText == "")
                {
                    this.ButtonText = this.FDefaultButtonText;
                }

                this.txtAutoPopulated.ButtonText = this.ButtonText;

                // ButtonTextAlign
                // TLogging.Log('FASpecialSettingButtonTextAlign: ' + Enum.GetName(typeof(System.Drawing.ContentAlignment), this.FButtonTextAlign));
                this.txtAutoPopulated.ButtonTextAlign = this.ButtonTextAlign;

                // ButtonWidth
                if (this.ButtonWidth < 0)
                {
                    this.ButtonWidth = this.FDefaultButtonWidth;
                }

                this.txtAutoPopulated.ButtonWidth = this.ButtonWidth;

                // TextBoxWidth
                if (this.TextBoxWidth <= 0)
                {
                    this.TextBoxWidth = this.FDefaultTextBoxWidth;
                }

                this.txtAutoPopulated.TextBoxWidth = this.TextBoxWidth;
                #endregion

                // TLogging.Log('this.FASpecialSetting = true  END');
            }
            else
            {
                // TLogging.Log('this.FASpecialSetting <> true  START');
                #region Default Settings apply

                // ButtonText
                this.ButtonText = this.FDefaultButtonText;

                // TLogging.Log('  this.FDefaultButtonText:          >' + this.FDefaultButtonText + '<');
                // TLogging.Log('  this.ButtonText:                  >' + this.FDefaultButtonText + '<');
                this.txtAutoPopulated.ButtonText = this.ButtonText;

                // TLogging.Log('  this.txtAutoPopulated.ButtonText: >' + this.FDefaultButtonText + '<');
                // ButtonTextAlign
                this.ButtonTextAlign = this.FDefaultButtonTextAlign;
                this.txtAutoPopulated.ButtonTextAlign = this.ButtonTextAlign;

                // ButtonWidth
                this.ButtonWidth = this.FDefaultButtonWidth;
                this.txtAutoPopulated.ButtonWidth = this.ButtonWidth;

                // TextBoxWidth
                this.TextBoxWidth = this.FDefaultTextBoxWidth;
                this.txtAutoPopulated.TextBoxWidth = this.TextBoxWidth;
                #endregion

                // TLogging.Log('this.FASpecialSetting <> true  END');
            }
        }

        /// <summary>
        /// This procedure initialises the user control according to the presets.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            if (InDesignMode == false)
            {
                switch (this.FListTable)
                {
                    case TListTableEnum.OccupationList:
                        #region TListTableEnum.OccupationList

                        // Setup LookUp datatable
                        this.txtAutoPopulated.LookUpDataSource = TDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.OccupationList).DefaultView;
                        this.txtAutoPopulated.TextBoxLookUpMember = "p_occupation_code_c";
                        this.txtAutoPopulated.LabelLookUpMember = "p_occupation_description_c";
                        this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyPrintables;
                        this.txtAutoPopulated.txtTextBox.ControlMode = TMaskedTextBoxMode.NormalTextBox;

                        // Other settings
                        this.txtAutoPopulated.lblLabel.Text = null;
                        this.FPartnerClass = "";

                        // End of "TListTableEnum.OccupationList"
                        #endregion
                        break;

                    case TListTableEnum.PartnerKey:
                        #region TListTableEnum.PartnerKey

                        // this.ASpecialSetting := true;
                        // No LookUp Table to set up since there are to many PartnerKeys
                        this.txtAutoPopulated.LookUpDataSource = null;
                        this.txtAutoPopulated.TextBoxLookUpMember = "";
                        this.txtAutoPopulated.LabelLookUpMember = "";
                        this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyDigits;
                        this.txtAutoPopulated.txtTextBox.ControlMode = TMaskedTextBoxMode.PartnerKey;

                        // DataBinding Settings
                        this.txtAutoPopulated.lblLabel.Text = null;
                        this.FPartnerClass = "";

                        // TLogging.Log('End PartnerKey setup');
                        #endregion
                        break;

                    case TListTableEnum.Extract:
                        #region TListTableEnum.Extract

                        // this.ASpecialSetting := true;
                        // No LookUp Table to set up since there are to many Extracts
                        this.txtAutoPopulated.LookUpDataSource = null;
                        this.txtAutoPopulated.TextBoxLookUpMember = "";
                        this.txtAutoPopulated.LabelLookUpMember = "";
                        this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyLettersOrDigits;
                        this.txtAutoPopulated.txtTextBox.ControlMode = TMaskedTextBoxMode.Extract;

                        // DataBinding Settings
                        this.txtAutoPopulated.lblLabel.Text = null;
                        this.FPartnerClass = "";

                        // TLogging.Log('End Extract setup');
                        #endregion
                        break;

                    case TListTableEnum.Conference:
                    case TListTableEnum.Event:
                        #region TListTableEnum.Conference and TListTableEnum.Event

                        /* this.ASpecialSetting := true; */
                        /* No LookUp Table to set up since there are to many PartnerKeys */
                        this.txtAutoPopulated.LookUpDataSource = null;
                        this.txtAutoPopulated.TextBoxLookUpMember = "";
                        this.txtAutoPopulated.LabelLookUpMember = "";
                        this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyDigits;
                        this.txtAutoPopulated.txtTextBox.ControlMode = TMaskedTextBoxMode.PartnerKey;

                        /* DataBinding Settings */
                        this.txtAutoPopulated.lblLabel.Text = null;
                        this.FPartnerClass = "";

                        /* TLogging.Log('End Conference or Event setup'); */
                        #endregion
                        break;

                    case TListTableEnum.Bank:
                        #region TListTableEnum.Bank

                        // this.ASpecialSetting := true;
                        // No LookUp Table to set up since there are to many Banks
                        this.txtAutoPopulated.LookUpDataSource = null;
                        this.txtAutoPopulated.TextBoxLookUpMember = "";
                        this.txtAutoPopulated.LabelLookUpMember = "";
                        this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyLettersOrDigits;
                        this.txtAutoPopulated.txtTextBox.ControlMode = TMaskedTextBoxMode.PartnerKey;

                        // DataBinding Settings
                        this.txtAutoPopulated.lblLabel.Text = null;
                        this.FPartnerClass = "";

                        // TLogging.Log('End Extract setup');
                        #endregion
                        break;
                }

                // End of "case this.FListTable of"
                // Set controls status to initialised
                this.FUserControlInitialised = true;
                this.txtAutoPopulated.Size = this.Size;
                this.txtAutoPopulated.RelocateTextBox();
                this.txtAutoPopulated.RelocateLabel();
                this.timerGetKey.Interval = 2000;                 // TWO SECONDS
                this.txtAutoPopulated.txtTextBox.TextChanged += new EventHandler(this.TextHasChanged);
            }
        }

        #endregion

        /// <summary>
        /// Property Fields
        /// This function ensures that the format of the PartnerClass property string
        /// is correctly passed to the server. It removes blanks and changes the
        /// case to upper case. The following would be for example a correct
        /// PartnerClass string: PERSON,FAMILY
        /// </summary>
        /// <returns>void</returns>
        private string FormatPartnerClassString(string AnUnformated)
        {
            String mConcatenated;

            System.Collections.Specialized.StringCollection mCollection;

            // Initialization
            mConcatenated = "";

            if ((AnUnformated != "") && (AnUnformated != null))
            {
                // Get single strings
                mCollection = StringHelper.StrSplit(AnUnformated, UNIT_DELIMITERS_PARTNERCLASS);

                // Reassemble strings
                foreach (string mClass in mCollection)
                {
                    mConcatenated += "," + mClass;
                }

                // Delete the leading comma
                if (mConcatenated.Length > 1)
                {
                    mConcatenated = mConcatenated.Substring(1);
                }
                else
                {
                    mConcatenated = "";
                }
            }
            else
            {
                mConcatenated = AnUnformated;
            }

            // Deliver Result
            return mConcatenated.ToUpper();
        }

        #region Event handling

        /// <summary>
        /// This procedure verifies the changes of the DataSource.
        /// </summary>
        /// <returns>void</returns>
        protected void OnDataboundTableColumnChanged(System.Object sender, DataColumnChangeEventArgs e)
        {
            // mVerified:           System.Boolean;
            bool mDelegateVerified;

            // mErrorMessage:       System.String;  mCaption:            System.String;
            String mColumnName;
            TResultSeverity mResultSev;
            TScreenVerificationResult mVerificationResult;
            bool mPartnerExists;
            String mVerifiedString;

            System.Int64 mPartnerKey;
            TPartnerClass mPartnerClass;
            bool mPartnerIsMerged;

            String mExtractName;

            // was a set of TPartnerClass
            Object[] mPartnerClassSet;
            String[] mPartnerValues;
            Char[] mPartnerClassDelimiter;

            /* This works in two different ways depending on whether it is
             * partner key mode of occupation mode.
             *
             * It is triggered by the datatable the control is bound to  firing
             * a column changed event, indicating new data.
             *
             * If occupation mode - we use a VerifyLookupValue in txtButtonLabel to
             * determine whether it is a valid value or not. Because the control is bound
             * to a datatable, it can simply check against the values in the databound table.
             *
             * In partner key mode, we invoke the delegate  VerifyUserEntry, which
             * the hosting form must implement, which returns true if the value is value,
             * or false if not.
             * (hope to do this in-control soon)
             *
             */
            if (e.ProposedValue == null)
            {
                // we can't possibly do anything useful here
                return;
            }

            // Initialization
            mColumnName = e.Column.ColumnName;
            mDelegateVerified = false;
            this.FErrorData.RVerified = false;
            mResultSev = TResultSeverity.Resv_Critical;
            mVerifiedString = null;
            mPartnerClassDelimiter = new char[] {
                ','
            };
            mPartnerValues = this.PartnerClass.Split(mPartnerClassDelimiter);

            // Needs the event handling here?
            if (mColumnName.Equals(this.FValueMember) == false)
            {
                // we are not concerned, out of here!
                return;
            }

            // we need to verify the data:
            switch (this.FListTable)
            {
                case TListTableEnum.OccupationList:
                    #region TListTableEnum.OccupationList

                    // TLogging.Log('Verification Branch: ' + Enum.GetName(typeof(TListTableEnum), TListTableEnum.OccupationList));
                    // TLogging.Log('mColumn.ColumnName:  ' + mColumnName);
                    // Do all things necessary to verify the Occupation code:
                    // mDataRow := this.txtAutoPopulated.GetLookUpRow('p_occupation_code_c', e.ProposedValue.ToString);
                    // SPECIAL CASE: an empty string IS a valid value!
                    if (e.ProposedValue.ToString() == "")
                    {
                        this.FErrorData.RVerified = true;
                        return;
                    }

                    mVerifiedString = this.txtAutoPopulated.VerifyLookUpValue("p_occupation_code_c", e.ProposedValue.ToString(), false);

                    // TLogging.Log('mVerifiedString:  ' + mVerifiedString);
                    if ((mVerifiedString == null) || (mVerifiedString == ""))
                    {
                        // Occupation Code does not exist in the LookUpTable
                        this.FDisplayLabelString = UNIT_NO_VALID_OCCUPATIONCODE;

                        // this.FErrorData.RErrorMessage := 'The specified occupation code "' + e.ProposedValue.ToString + '" is invalid!' + "\n" + 'Please check spelling!';
                        this.FErrorData.RErrorMessage = String.Format(Catalog.GetString("The specified Occupation Code '{0}' is invalid!\r\n" +
                                "Please check spelling!"), e.ProposedValue);
                        this.FErrorData.RCaption = Catalog.GetString("Invalid Occupation Code");

                        //mErrorName = "OccupationCode Error";
                        mResultSev = TResultSeverity.Resv_Noncritical;

                        //mResultField = "OccupationCode";

                        // suppress error message
                        this.FErrorData.RVerified = false;
                    }
                    else
                    {
                        // Occupation Code does exist in the LookUpTable
                        this.FErrorData.RVerified = true;
                    }

                    // End TListTableEnum.OccupationList:
                    #endregion
                    break;

                case TListTableEnum.PartnerKey:
                    #region TListTableEnum.PartnerKey

                    // TLogging.Log('Verification Branch: ' + Enum.GetName(typeof(TListTableEnum), TListTableEnum.PartnerKey));
                    mPartnerKey = Convert.ToInt64(e.ProposedValue);

                    // sort out the set
                    mPartnerClassSet = new Object[0];
                    mPartnerClassDelimiter = new char[] {
                        ','
                    };
                    mPartnerValues = PartnerClass.Split(mPartnerClassDelimiter);

                    // we end up with a set of values
                    if (PartnerClass != "")
                    {
                        for (Int32 counter = 0; counter <= mPartnerValues.Length - 1; counter += 1)
                        {
                            try
                            {
                                mPartnerClass = (TPartnerClass)(System.Enum.Parse(typeof(TPartnerClass), mPartnerValues[counter], true));
                                mPartnerClassSet = Utilities.AddToArray(mPartnerClassSet, mPartnerClass);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Invalid entry in PartnerClass property: " + mPartnerValues[counter]);
                            }

                            // end try
                        }
                    }

                    // If the delegate is defined, the host form is allowed to dictate what is valid
                    if (this.VerifyUserEntry != null)
                    {
                        // delegate IS defined
                        // TLogging.Log('VerifyUserEntry is assigned!', [TLoggingType.ToLogfile]);
                        // TLogging.Log('VerifyUserEntry proposed Value: ' + e.ProposedValue.ToString, [TLoggingType.ToLogfile]);
                        try
                        {
                            mDelegateVerified = this.VerifyUserEntry(mPartnerKey.ToString(), out mVerifiedString);
                            this.FVerifiedString = mVerifiedString;
                        }
                        catch (Exception)
                        {
                            this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                            throw new EVerificationMissing("this.VerifyUserEntry could not be called!");
                        }

                        // end try
                    }
                    // end IS assigned
                    else
                    {
                        // delegate IS NOT defined
                        // look it up for ourself
                        TPartnerClass[] temp = new TPartnerClass[mPartnerClassSet.Length];
                        Int32 counter = 0;

                        foreach (object obj in mPartnerClassSet)
                        {
                            temp[counter] = (TPartnerClass)obj;
                            counter++;
                        }

                        mDelegateVerified = TServerLookup.TMPartner.VerifyPartner(mPartnerKey,
                            temp,
                            out mPartnerExists,
                            out mVerifiedString,
                            out mPartnerClass,
                            out mPartnerIsMerged);
                    }

                    // end delegate IS NOT defined
                    // OK, now process result
                    if (mDelegateVerified == true)
                    {
                        // TLogging.Log('(mDelegateVerified = true)');
                        this.txtAutoPopulated.lblLabel.Text = mVerifiedString;
                        this.FErrorData.RVerified = true;
                    }
                    else
                    {
                        // TLogging.Log('NOT(mDelegateVerified = true))');
                        this.FErrorData.RVerified = false;
                        this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                        this.txtAutoPopulated.lblLabel.Text = UNIT_NO_DATA_MESSAGE;
                        this.FErrorData.RErrorMessage = String.Format(Catalog.GetString("The specified {0} '{1}' is invalid!"),
                            ApplWideResourcestrings.StrPartnerKey, e.ProposedValue);
                        this.FErrorData.RCaption = String.Format(Catalog.GetString("Invalid "), ApplWideResourcestrings.StrPartnerKey);
                        this.FErrorData.RErrorCode = PetraErrorCodes.ERR_PARTNERKEY_INVALID;


                        //mErrorName = "PartnerKey Error";
                        mResultSev = TResultSeverity.Resv_Critical;

                        //mResultField = "PartnerKey";
                    }

                    // End TListTableEnum.PartnerKey:
                    #endregion
                    break;

                case TListTableEnum.Extract:
                    #region TListTableEnum.Extract

                    // TLogging.Log('Verification Branch: ' + Enum.GetName(typeof(TListTableEnum), TListTableEnum.Extract));
                    if (e.ProposedValue.ToString() == "")
                    {
                        this.FErrorData.RVerified = false;
                        this.FErrorData.RErrorMessage = Catalog.GetString("No valid Extract selected");
                        this.FErrorData.RCaption = Catalog.GetString("Error");
                        return;
                    }

                    mExtractName = e.ProposedValue.ToString();
                    String ExtractDescription = mExtractName;

                    if (mExtractName != "")
                    {
                        TServerLookup.TMPartner.GetExtractDescription(mExtractName, out ExtractDescription);
                    }

                    this.txtAutoPopulated.lblLabel.Text = ExtractDescription;

                    this.FErrorData.RVerified = true;

                    // End TListTableEnum.Extract:
                    #endregion
                    break;

                case TListTableEnum.Conference:
                case TListTableEnum.Event:
                    #region TListTableEnum.Conference and TListTableEnum.Event

                    /* TLogging.Log('Verification Branch: ' + Enum.GetName(typeof(TListTableEnum), TListTableEnum.Conference)); */
                    mPartnerKey = Convert.ToInt64(e.ProposedValue);

                    /* If the delegate is defined, the host form is allowed to dictate what is valid */
                    if (this.VerifyUserEntry != null)
                    {
                        /* delegate IS defined */
                        /* TLogging.Log('VerifyUserEntry is assigned!', [TLoggingType.ToLogfile]); */
                        /* TLogging.Log('VerifyUserEntry proposed Value: ' + e.ProposedValue.ToString, [TLoggingType.ToLogfile]); */
                        try
                        {
                            mDelegateVerified = this.VerifyUserEntry(mPartnerKey.ToString(), out mVerifiedString);
                            this.FVerifiedString = mVerifiedString;
                        }
                        catch (Exception)
                        {
                            this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                            throw new EVerificationMissing("this.VerifyUserEntry could not be called!");
                        }

                        /* end try */
                    }
                    /* end IS assigned */
                    else
                    {
                        /* delegate IS NOT defined */
                        /* look it up for ourself */
                        TPartnerClass[] temp = new TPartnerClass[1];

                        temp[0] = TPartnerClass.UNIT;

                        mDelegateVerified = TServerLookup.TMPartner.VerifyPartner(mPartnerKey,
                            temp,
                            out mPartnerExists,
                            out mVerifiedString,
                            out mPartnerClass,
                            out mPartnerIsMerged);
                    }

                    /* end delegate IS NOT defined */
                    /* OK, now process result */
                    if (mDelegateVerified == true)
                    {
                        /* TLogging.Log('(mDelegateVerified = true)'); */
                        this.txtAutoPopulated.lblLabel.Text = mVerifiedString;
                        this.FErrorData.RVerified = true;
                    }
                    else
                    {
                        /* TLogging.Log('NOT(mDelegateVerified = true))'); */
                        this.FErrorData.RVerified = false;
                        this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                        this.txtAutoPopulated.lblLabel.Text = UNIT_NO_DATA_MESSAGE;
                        this.FErrorData.RErrorMessage = String.Format(Catalog.GetString("The specified {0} '{1}' is invalid!"),
                            ApplWideResourcestrings.StrPartnerKey, e.ProposedValue);
                        this.FErrorData.RCaption = String.Format(Catalog.GetString("Invalid "), ApplWideResourcestrings.StrPartnerKey);
                        this.FErrorData.RErrorCode = PetraErrorCodes.ERR_PARTNERKEY_INVALID;

                        mResultSev = TResultSeverity.Resv_Critical;
                    }

                    /* End TListTableEnum.Conference and TListTableEnum.Event: */
                    #endregion
                    break;

                case TListTableEnum.Bank:
                    #region TListTableEnum.Bank

                    // TLogging.Log('Verification Branch: ' + Enum.GetName(typeof(TListTableEnum), TListTableEnum.PartnerKey));
                    mPartnerKey = Convert.ToInt64(e.ProposedValue);

                    // sort out the set
                    mPartnerClassSet = new Object[0];
                    mPartnerClassDelimiter = new char[] {
                        ','
                    };
                    mPartnerValues = PartnerClass.Split(mPartnerClassDelimiter);

                    // we end up with a set of values
                    if (PartnerClass != "")
                    {
                        for (Int32 counter = 0; counter <= mPartnerValues.Length - 1; counter += 1)
                        {
                            try
                            {
                                mPartnerClass = (TPartnerClass)(System.Enum.Parse(typeof(TPartnerClass), mPartnerValues[counter], true));
                                mPartnerClassSet = Utilities.AddToArray(mPartnerClassSet, mPartnerClass);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Invalid entry in PartnerClass property: " + mPartnerValues[counter]);
                            }

                            // end try
                        }
                    }

                    // If the delegate is defined, the host form is allowed to dictate what is valid
                    if (this.VerifyUserEntry != null)
                    {
                        // delegate IS defined
                        // TLogging.Log('VerifyUserEntry is assigned!', [TLoggingType.ToLogfile]);
                        // TLogging.Log('VerifyUserEntry proposed Value: ' + e.ProposedValue.ToString, [TLoggingType.ToLogfile]);
                        try
                        {
                            mDelegateVerified = this.VerifyUserEntry(mPartnerKey.ToString(), out mVerifiedString);
                            this.FVerifiedString = mVerifiedString;
                        }
                        catch (Exception)
                        {
                            this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                            throw new EVerificationMissing("this.VerifyUserEntry could not be called!");
                        }

                        // end try
                    }
                    // end IS assigned
                    else
                    {
                        // delegate IS NOT defined
                        // look it up for ourself
                        TPartnerClass[] temp = new TPartnerClass[mPartnerClassSet.Length];
                        Int32 counter = 0;

                        foreach (object obj in mPartnerClassSet)
                        {
                            temp[counter] = (TPartnerClass)obj;
                            counter++;
                        }

                        mDelegateVerified = TServerLookup.TMPartner.VerifyPartner(mPartnerKey,
                            temp,
                            out mPartnerExists,
                            out mVerifiedString,
                            out mPartnerClass,
                            out mPartnerIsMerged);
                    }

                    // end delegate IS NOT defined
                    // OK, now process result
                    if (mDelegateVerified == true)
                    {
                        // TLogging.Log('(mDelegateVerified = true)');
                        this.txtAutoPopulated.lblLabel.Text = mVerifiedString;
                        this.FErrorData.RVerified = true;
                    }
                    else
                    {
                        // TLogging.Log('NOT(mDelegateVerified = true))');
                        this.FErrorData.RVerified = false;
                        this.FVerifiedString = UNIT_NO_DATA_MESSAGE;
                        this.txtAutoPopulated.lblLabel.Text = UNIT_NO_DATA_MESSAGE;
                        this.FErrorData.RErrorMessage = String.Format(Catalog.GetString("The specified {0} '{1}' is invalid!"),
                            ApplWideResourcestrings.StrPartnerKey, e.ProposedValue);
                        this.FErrorData.RCaption = String.Format(Catalog.GetString("Invalid "), ApplWideResourcestrings.StrPartnerKey);
                        this.FErrorData.RErrorCode = PetraErrorCodes.ERR_PARTNERKEY_INVALID;


                        //mErrorName = "PartnerKey Error";
                        mResultSev = TResultSeverity.Resv_Critical;

                        //mResultField = "PartnerKey";
                    }

                    // End TListTableEnum.PartnerKey:
                    #endregion
                    break;
            }

            // End "case this.FListTable of"
            #region Marking errors

            // If the entered values could not be verified
            // TLogging.Log('Event ColumnName: ' + mColumnName);
            // TLogging.Log('Right ColumnName: ' + this.FValueMember);
            if (this.FErrorData.RVerified == false)
            {
                // Creata and show error message
                mVerificationResult = new TScreenVerificationResult(this.Parent, e.Column,
                    this.FErrorData.RErrorMessage, this.FErrorData.RCaption, FErrorData.RErrorCode, this, mResultSev);

                if (FVerificationResultCollection != null)
                {
                    FVerificationResultCollection.Add(mVerificationResult);
                }

                TMessages.MsgGeneralError(mVerificationResult, this.ParentForm.GetType());
            }
            else
            {
                // If there was previously an error delete it
                if (FVerificationResultCollection != null)
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }
                }

                // if (e.Row.GetColumnError(e.Column) <> '') then
                // begin
                // e.Row.SetColumnError(e.Column, nil);
                // /        TLogging.Log('Error unmarked!');
                // end;
            }

            #endregion
        }

        /// <summary>
        /// Properties
        /// </summary>
        /// <returns>void</returns>
        private void TextHasChanged(System.Object sender, System.EventArgs e)
        {
            if ((FUserControlInitialised == true)
                && ((this.FListTable == TListTableEnum.PartnerKey)
                    || (this.FListTable == TListTableEnum.Extract)
                    || (this.FListTable == TListTableEnum.Conference)
                    || (this.FListTable == TListTableEnum.Event)
                    || (this.FListTable == TListTableEnum.Bank)))
            {
                // reset timer and start it again
                timerGetKey.Stop();
                timerGetKey.Start();
            }
        }

        private void TimerGetKey_Tick(System.Object sender, System.EventArgs e)
        {
            if ((FUserControlInitialised == true)
                && ((this.FListTable == TListTableEnum.PartnerKey)
                    || (this.FListTable == TListTableEnum.Extract)
                    || (this.FListTable == TListTableEnum.Conference)
                    || (this.FListTable == TListTableEnum.Event)
                    || (this.FListTable == TListTableEnum.Bank)))
            {
                timerGetKey.Stop();         //dont loop if exception in UpdateDisplayedValue
                this.UpdateDisplayedValue();
            }
        }

        /// <summary>
        /// This procedure ensures that the height of this control cannot be changed.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnResize(System.EventArgs e)
        {
            System.Drawing.Size mSize;
            System.Int32 mWidth;

            // TLogging.log('TTListTableEnum.OnResize this.Size.Width(TxtAutoPopulateButtonLabel): ' + this.Size.Width.ToString, [TLoggingType.ToLogfile]);
            mWidth = this.Size.Width;
            mSize = new System.Drawing.Size(mWidth, UNIT_DEFAULT_HEIGHT);
            this.Size = mSize;
            this.txtAutoPopulated.Size = mSize;

            // this.AppearanceSetup(this.FListTable);
            this.txtAutoPopulated.Size = this.Size;
            this.txtAutoPopulated.RelocateTextBox();
            this.txtAutoPopulated.RelocateLabel();

            // TLogging.log('this.Size.Width(TxtAutoPopulateButtonLabel' + this.Size.Width.ToString);
        }

        private void TxtAutoPopulated_ButtonClick(string LabelStringIn,
            string TextBoxStringIn,
            out string LabelStringOut,
            out string PartnerClassOut,
            out string TextBoxStringOut)
        {
            String mLabelStringOld;
            String mTextBoxStringOld;
            String mLabelStringNew;
            String mTextBoxStringNew;
            String mExceptionString;
            String mResultStringTxt;
            String mResultStringName;
            String mResultStringLbl;
            String mPartnerClass;
            TPartnerClass? mPartnerClass2;
            String mResultStringExtraInformation;

            System.Int64 mResultIntTxt;
            int mResultShortIntTxt;

            LabelStringOut = "";
            TextBoxStringOut = "";
            PartnerClassOut = null;

            // mResultIntLbl:     System.Int64;  mDummyString:      String;
            TLocationPK mResultLocationPK;
            mLabelStringOld = LabelStringIn;
            mTextBoxStringOld = TextBoxStringIn;

            if (DesignMode == false)
            {
                // If Delegate is assigned do something
                // TLogging.Log('txtAutoPopulated_ButtonClick');
                if (ClickButton != null)
                {
                    // TLogging.Log('(ClickButton' != null);
                    try
                    {
                        mLabelStringOld = LabelStringIn;
                        mTextBoxStringOld = TextBoxStringIn;
                        this.ClickButton(mLabelStringOld, mTextBoxStringOld, out mLabelStringNew, out mPartnerClass, out mTextBoxStringNew);
                        LabelStringOut = mLabelStringNew;
                        TextBoxStringOut = mTextBoxStringNew;

                        if (mPartnerClass != null)
                        {
                            PartnerClassOut = mPartnerClass;
                        }
                    }
                    catch (Exception E)
                    {
                        mExceptionString = E.ToString();
                        MessageBox.Show(mExceptionString);
                        throw new EClickButton(mExceptionString);
                    }
                }
                else
                {
                    switch (this.FListTable)
                    {
                        case TListTableEnum.OccupationList:
                            #region TListTableEnum.OccupationList

                            // call Progress from here
                            // TLogging.log('txtAutoPopulated_ButtonClick');

                            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
                            if (TCommonScreensForwarding.OpenOccupationCodeFindScreen != null)
                            {
                                // delegate IS defined
                                // TLogging.Log('PartnerFindScreen is assigned!', [TLoggingType.ToLogfile]);
                                try
                                {
                                    mResultStringTxt = mTextBoxStringOld;

                                    TCommonScreensForwarding.OpenOccupationCodeFindScreen.Invoke(ref mResultStringTxt,
                                        this.ParentForm);

                                    mResultStringLbl = "";

                                    if (!string.IsNullOrEmpty(mResultStringTxt))
                                    {
                                        TextBoxStringOut = mResultStringTxt;
                                        LabelStringOut = mResultStringLbl;

                                        if (OccupationFound != null)
                                        {
                                            OccupationFound(mResultStringTxt);
                                        }
                                    }
                                    else
                                    {
                                        TextBoxStringOut = "";
                                        LabelStringOut = "";
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling OccupationCodeFindScreen Delegate!", exp);
                                }

                                // end try
                            }
                            // end IS assigned
                            else
                            {
                                // delegate IS NOT defined
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: OpenOccupationCodeFindScreen Delegate must be assigned on this Control to be able to open a Occupation Code screen!");
                            }

                            // End TListTableEnum.OccupationList:
                            #endregion
                            break;

                        case TListTableEnum.PartnerKey:
                            #region TListTableEnum.PartnerKey

                            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
                            if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
                            {
                                // delegate IS defined
                                // TLogging.Log('PartnerFindScreen is assigned!', [TLoggingType.ToLogfile]);
                                try
                                {
                                    TCommonScreensForwarding.OpenPartnerFindScreen.Invoke(this.FPartnerClass,
                                        out mResultIntTxt,
                                        out mResultStringLbl,
                                        out mPartnerClass2,
                                        out mResultLocationPK,
                                        this.ParentForm);

//                                    MessageBox.Show(mResultIntTxt.ToString() + "; " + mResultStringLbl + "; " + mResultLocationPK.LocationKey.ToString());
                                    if (mResultIntTxt != -1)
                                    {
                                        TextBoxStringOut = StringHelper.PartnerKeyToStr(mResultIntTxt);
                                        LabelStringOut = mResultStringLbl;

                                        if (mPartnerClass2.HasValue)
                                        {
                                            PartnerClassOut = SharedTypes.PartnerClassEnumToString(mPartnerClass2.Value);
                                        }

                                        if (PartnerFound != null)
                                        {
                                            PartnerFound(mResultIntTxt, mResultLocationPK.LocationKey);
                                        }

                                        if ((ValueChanged != null) && (mTextBoxStringOld != TextBoxStringOut))
                                        {
                                            bool ValidResult = true;
                                            ValueChanged(mResultIntTxt, mResultStringLbl, ValidResult);
                                        }
                                    }
                                    else
                                    {
                                        TextBoxStringOut = "";
                                        LabelStringOut = "";
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling PartnerFindScreen Delegate!", exp);
                                }

                                // end try
                            }
                            // end IS assigned
                            else
                            {
                                // delegate IS NOT defined
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: OpenPartnerFindScreen Delegate must be assigned on this Control to be able to open a Partner Find screen!");
                            }

                            // End TListTableEnum.PartnerKey:
                            #endregion
                            break;

                        case TListTableEnum.Extract:
                            #region TListTableEnum.Extract

                            /* If the delegate is defined, the host form will launch a Event Find dialog for us */
                            if (TCommonScreensForwarding.OpenExtractFindScreen != null)
                            {
                                /* delegate IS defined */
                                /* TLogging.Log('OpenExtractFindDialog is assigned!', [TLoggingType.ToLogfile]); */
                                try
                                {
                                    TCommonScreensForwarding.OpenExtractFindScreen.Invoke(out mResultShortIntTxt,
                                        out mResultStringName,
                                        out mResultStringLbl,
                                        this.ParentForm);

                                    if (mResultShortIntTxt != -1)
                                    {
                                        TextBoxStringOut = mResultStringName;
                                        LabelStringOut = mResultStringLbl;

                                        if ((ValueChanged != null) && (mTextBoxStringOld != TextBoxStringOut))
                                        {
                                            bool ValidResult = true;
                                            ValueChanged(mResultShortIntTxt, mResultStringName, ValidResult);
                                        }
                                    }
                                    else
                                    {
                                        TextBoxStringOut = "";
                                        LabelStringOut = "";
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling OpenExtractFind Delegate!", exp);
                                }
                            }
                            /* end IS assigned */
                            else
                            {
                                /* delegate IS NOT defined */
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: OpenExtractFind Delegate must be assigned on this Control to be able to open a Event find dialog!");
                            }

                            /* End TListTableEnum.Extract: */

                            #endregion
                            break;

                        case TListTableEnum.Conference:
                            #region TListTableEnum.Conference

                            /* If the delegate is defined, the host form will launch a Conference Find dialog for us */
                            if (TCommonScreensForwarding.OpenConferenceFindScreen != null)
                            {
                                /* delegate IS defined */
                                /* TLogging.Log('OpenConferenceFindDialog is assigned!', [TLoggingType.ToLogfile]); */
                                try
                                {
                                    TCommonScreensForwarding.OpenConferenceFindScreen.Invoke("*", "*",
                                        out mResultIntTxt,
                                        out mResultStringLbl,
                                        this.ParentForm);

                                    if (mResultIntTxt != -1)
                                    {
                                        TextBoxStringOut = StringHelper.PartnerKeyToStr(mResultIntTxt);
                                        LabelStringOut = mResultStringLbl;

                                        if ((ValueChanged != null) && (mTextBoxStringOld != TextBoxStringOut))
                                        {
                                            bool ValidResult = true;
                                            ValueChanged(mResultIntTxt, mResultStringLbl, ValidResult);
                                        }
                                    }
                                    else
                                    {
                                        TextBoxStringOut = "";
                                        LabelStringOut = "";
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling OpenConferenceFind Delegate!", exp);
                                }
                            }
                            /* end IS assigned */
                            else
                            {
                                /* delegate IS NOT defined */
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: OpenConferenceFind Delegate must be assigned on this Control to be able to open a Conference find dialog!");
                            }

                            /* End TListTableEnum.Conference: */
                            #endregion
                            break;

                        case TListTableEnum.Event:
                            #region TListTableEnum.Event

                            /* If the delegate is defined, the host form will launch a Event Find dialog for us */
                            if (TCommonScreensForwarding.OpenEventFindScreen != null)
                            {
                                /* delegate IS defined */
                                /* TLogging.Log('OpenEventFindDialog is assigned!', [TLoggingType.ToLogfile]); */
                                try
                                {
                                    TCommonScreensForwarding.OpenEventFindScreen.Invoke("*",
                                        out mResultIntTxt,
                                        out mResultStringLbl,
                                        out mResultStringExtraInformation,
                                        this.ParentForm);

                                    if (mResultIntTxt != -1)
                                    {
                                        TextBoxStringOut = StringHelper.PartnerKeyToStr(mResultIntTxt);
                                        LabelStringOut = mResultStringLbl;

                                        if ((ValueChanged != null) && (mTextBoxStringOld != TextBoxStringOut))
                                        {
                                            bool ValidResult = true;
                                            ValueChanged(mResultIntTxt, mResultStringLbl, ValidResult);
                                        }
                                    }
                                    else
                                    {
                                        TextBoxStringOut = "";
                                        LabelStringOut = "";
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling OpenEventFind Delegate!", exp);
                                }
                            }
                            /* end IS assigned */
                            else
                            {
                                /* delegate IS NOT defined */
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: OpenEventFind Delegate must be assigned on this Control to be able to open a Event find dialog!");
                            }

                            /* End TListTableEnum.Event: */
                            #endregion
                            break;

                        case TListTableEnum.Bank:
                            #region TListTableEnum.Bank

                            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
                            if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
                            {
                                // delegate IS defined
                                // TLogging.Log('PartnerFindScreen is assigned!', [TLoggingType.ToLogfile]);
                                try
                                {
                                    mResultIntTxt = Convert.ToInt64(this.Text);

                                    BankTDS FBankDataset = (BankTDS) this.DataSet;

                                    TCommonScreensForwarding.OpenBankFindDialog.Invoke(ref FBankDataset,
                                        ref mResultIntTxt,
                                        this.ParentForm);

                                    if ((DatasetChanged != null) && (FBankDataset != this.DataSet))
                                    {
                                        this.DataSet = FBankDataset;
                                        DatasetChanged(DataSet);
                                    }

//                                  MessageBox.Show(mResultIntTxt.ToString() + "; " + mResultStringLbl + "; " + mResultLocationPK.LocationKey.ToString());
                                    if ((mResultIntTxt != -1) && (mResultIntTxt != 0))
                                    {
                                        TextBoxStringOut = StringHelper.PartnerKeyToStr(mResultIntTxt);

                                        if ((ValueChanged != null) && (mTextBoxStringOld != TextBoxStringOut))
                                        {
                                            bool ValidResult = true;
                                            ValueChanged(mResultIntTxt, "", ValidResult);
                                        }
                                    }
                                }
                                catch (Exception exp)
                                {
                                    TextBoxStringOut = "";
                                    LabelStringOut = "";
                                    throw new EOPAppException("Exception occured while calling BankFindDialog Delegate!", exp);
                                }

                                // end try
                            }
                            // end IS assigned
                            else
                            {
                                // delegate IS NOT defined
                                throw new EOPAppException(
                                    "DEVELOPER ERROR: BankFindDialog Delegate must be assigned on this Control to be able to open a Bank Find Dialog!");
                            }

                            // End TListTableEnum.PartnerKey:
                            #endregion
                            break;
                    }

                    // End "case this.FListTable of"
                }
            }
            else
            {
                // messagebox.Show('DesignMode = true')
            }
        }

        /// <summary>
        /// this will update the databound source
        /// </summary>
        public void ResetLabelText()
        {
            this.txtAutoPopulated.UpdateLabelText();
        }

        /// <summary>
        /// Updates the label for the text button
        /// </summary>
        public void UpdateDisplayedValue()
        {
            string currentText;
            string partnerclass = String.Empty;

            currentText = txtAutoPopulated.txtTextBox.Text;

            if (currentText.Length > 0)
            {
                if (ShowLabel)
                {
                    string result = this.txtAutoPopulated.lblLabel.Text;
                    this.TxtAutoPopulated_SetLabel(currentText, ref result, ref partnerclass);
                    this.txtAutoPopulated.lblLabel.Text = result;

                    TCommonControlsHelper.SetPartnerKeyBackColour(partnerclass, txtAutoPopulated.txtTextBox, FOriginalPartnerClassColor);
                }
            }
        }

        private void TxtAutoPopulated_SetLabel(string ALookUpText, ref string ALabelText, ref string APartnerClass)
        {
            string OldLabelText = ALabelText;
            string StrShortnameNotRetrieved = Catalog.GetString("### ShortName not retrieved ###");

            ALabelText = "";

            bool ServerResult;
            bool ValidResult = false;
            System.Int64 mPartnerKey;
            String mPartnerShortName;
            String ExtractDescription;
            TPartnerClass mPartnerClass;

            APartnerClass = String.Empty;

            /* Occupation list mode  seems to work differently.
             * It sets the label to blank, and the label is populated when the input is verified.
             *
             * Partner Key Mode  we call Serverlookups to retrieve the partner key name */

            // TLogging.Log('Start txtAutoPopulated_SetLabel', [TLoggingType.ToLogfile]);
            // Initialisation
            mPartnerShortName = StrShortnameNotRetrieved;

            // Get the label text depending on mode
            switch (this.FListTable)
            {
                case TListTableEnum.OccupationList:
                    #region TListTableEnum.OccupationList

                    // TLogging.Log('  Hello from OccupationList');
                    ALabelText = "";

                    // End TListTableEnum.OccupationList:
                    #endregion
                    break;

                case TListTableEnum.PartnerKey:
                    #region TListTableEnum.PartnerKey

                    // TLogging.Log('Hello from TListTableEnum.PartnerKey', [TLoggingType.ToLogfile]);
                    // TLogging.Log('  Verified String: >' + this.FVerifiedString + '<', [TLoggingType.ToLogfile]);
                    if ((ALookUpText != "") && (ALookUpText != "0000000000"))
                    {
                        mPartnerKey = StringHelper.StrToPartnerKey(ALookUpText);

                        // now call server lookup class
                        ServerResult = TServerLookup.TMPartner.GetPartnerShortName(Convert.ToInt64(
                                mPartnerKey), out mPartnerShortName, out mPartnerClass, true);

                        if (ServerResult == false)
                        {
                            mPartnerShortName = StrShortnameNotRetrieved;
                        }
                        else
                        {
                            ValidResult = true;
                        }

                        APartnerClass = SharedTypes.PartnerClassEnumToString(mPartnerClass);
                    }
                    else
                    {
                        mPartnerKey = 0;
                        mPartnerShortName = "";
                    }

                    if ((ValueChanged != null) && (OldLabelText != mPartnerShortName))
                    {
                        ValueChanged(mPartnerKey, mPartnerShortName, ValidResult);
                    }

                    ALabelText = mPartnerShortName;

                    // End TListTableEnum.PartnerKey:
                    #endregion
                    break;

                case TListTableEnum.Extract:
                    #region TListTableEnum.Extract

                    // TLogging.Log('Hello from TListTableEnum.Extract', [TLoggingType.ToLogfile]);
                    // TLogging.Log('  Verified String: >' + this.FVerifiedString + '<', [TLoggingType.ToLogfile]);

                    if ((ALookUpText != ""))
                    {
                        // now call server lookup class
                        ServerResult = TServerLookup.TMPartner.GetExtractDescription(ALookUpText, out ExtractDescription);

                        if (ServerResult == false)
                        {
                            ExtractDescription = Catalog.GetString("### Extract description not retrieved ###");
                        }
                    }
                    else
                    {
                        ExtractDescription = "";
                    }

                    ALabelText = ExtractDescription;

                    // End TListTableEnum.Extract:
                    #endregion
                    break;

                case TListTableEnum.Conference:
                    #region TListTableEnum.Conference

                    mPartnerKey = StringHelper.StrToPartnerKey(ALookUpText);

                    /* TLogging.Log('  Verified String: >' + this.FVerifiedString + '<', [TLoggingType.ToLogfile]); */
                    if ((ALookUpText != "") && (mPartnerKey != 0))
                    {
                        /* now call server lookup class */
                        ServerResult = TServerLookup.TMPartner.GetPartnerShortName(Convert.ToInt64(
                                mPartnerKey), out mPartnerShortName, out mPartnerClass, true);

                        if (ServerResult == false)
                        {
                            mPartnerShortName = StrShortnameNotRetrieved;
                        }
                        else if (mPartnerClass != TPartnerClass.UNIT)
                        {
                            mPartnerShortName = String.Format(Catalog.GetString(
                                    "### {0} is not a Conference ###"), ApplWideResourcestrings.StrPartnerKey);
                        }
                        else
                        {
                            ValidResult = true;
                        }

                        APartnerClass = SharedTypes.PartnerClassEnumToString(mPartnerClass);
                    }
                    else
                    {
                        mPartnerShortName = "";
                    }

                    if ((ValueChanged != null) && (OldLabelText != mPartnerShortName))
                    {
                        ValueChanged(mPartnerKey, mPartnerShortName, ValidResult);
                    }

                    ALabelText = mPartnerShortName;

                    /* End TListTableEnum.Conference: */
                    #endregion
                    break;

                case TListTableEnum.Event:
                    #region TListTableEnum.Event

                    mPartnerKey = StringHelper.StrToPartnerKey(ALookUpText);

                    /* TLogging.Log('  Verified String: >' + this.FVerifiedString + '<', [TLoggingType.ToLogfile]); */
                    if ((ALookUpText != "") && (mPartnerKey != 0))
                    {
                        /* now call server lookup class */
                        ServerResult = TServerLookup.TMPartner.GetPartnerShortName(Convert.ToInt64(
                                mPartnerKey), out mPartnerShortName, out mPartnerClass, true);

                        if (ServerResult == false)
                        {
                            mPartnerShortName = StrShortnameNotRetrieved;
                        }
                        else if (mPartnerClass != TPartnerClass.UNIT)
                        {
                            mPartnerShortName = String.Format(Catalog.GetString(
                                    "### {0} is not an Event ###"), ApplWideResourcestrings.StrPartnerKey);
                        }
                        else
                        {
                            ValidResult = true;
                        }
                    }
                    else
                    {
                        mPartnerShortName = "";
                    }

                    if ((ValueChanged != null) && (OldLabelText != mPartnerShortName))
                    {
                        ValueChanged(mPartnerKey, mPartnerShortName, ValidResult);
                    }

                    ALabelText = mPartnerShortName;


                    /* End TListTableEnum.Event: */
                    #endregion
                    break;

                case TListTableEnum.Bank:
                    #region TListTableEnum.Bank

                    // TLogging.Log('  Verified String: >' + this.FVerifiedString + '<', [TLoggingType.ToLogfile]);
                    if ((ALookUpText != "") && (ALookUpText != "0000000000"))
                    {
                        mPartnerKey = StringHelper.StrToPartnerKey(ALookUpText);

                        // now call server lookup class
                        ServerResult = TServerLookup.TMPartner.GetPartnerShortName(Convert.ToInt64(
                                mPartnerKey), out mPartnerShortName, out mPartnerClass, true);

                        if (ServerResult == false)
                        {
                            mPartnerShortName = StrShortnameNotRetrieved;
                        }
                        else
                        {
                            ValidResult = true;
                        }

                        APartnerClass = SharedTypes.PartnerClassEnumToString(mPartnerClass);
                    }
                    else
                    {
                        mPartnerKey = 0;
                        mPartnerShortName = "";
                    }

                    if ((ValueChanged != null) && (OldLabelText != mPartnerShortName))
                    {
                        ValueChanged(mPartnerKey, mPartnerShortName, ValidResult);
                    }

                    ALabelText = mPartnerShortName;

                    // End TListTableEnum.PartnerKey:
                    #endregion
                    break;
            }

            // End "case this.FListTable of"
            // TLogging.Log('End txtAutoPopulated_SetLabel', [TLoggingType.ToLogfile]);
        }

        #endregion

        // Event handling

        #region DataBinding Implementation

        /// <summary>
        /// This function gets DataBinding of this System.Object. The function is used by the
        /// expTextBoxStringLengthCheck module in order to expand the TextBox properties.
        /// //    function  GetTextBoxDataBinding(): System.Windows.Forms.Binding
        /// This procedure enables the monitoring of changes to the DataSource.
        /// </summary>
        /// <returns>void</returns>
        protected void MonitorDataSourceChanges(System.Data.DataView ADataSource)
        {
            try
            {
                ((System.Data.DataView)ADataSource).Table.ColumnChanged += new DataColumnChangeEventHandler(this.OnDataboundTableColumnChanged);
            }
            catch (Exception E)
            {
                throw new System.Exception("The include to monitor changes of the DataSource failed! " + "\n" + E.ToString());
            }
        }

        /// <summary>
        /// This procedure does the databinding of this user control.
        /// </summary>
        /// <returns>void</returns>
        public void PerformDataBinding(System.Data.DataView ADataSource, String ATextBoxDataMember)
        {
            String mExceptionMsg;

            // TLogging.Log('TTListTableEnum.PerformDataBinding: ATextBoxDataMember: ' + ATextBoxDataMember, [TLoggingType.ToLogfile]);
            if (!FUserControlInitialised)
            {
                this.InitialiseUserControl();
            }

            // Databind the control
            switch (this.FListTable)
            {
                case TListTableEnum.OccupationList:
                    #region Explanation on the different control modes

                    /*
                     * Currently there are two different control modes for the txtButtonLabel
                     * class: TwoTableMode, FunctionMode.
                     * TwoTableMode:
                     * This mode is used when the TextBox has a look up table. The databound
                     * table is responsible for the TextBox Text. The look up table denotes the
                     * Label Text.
                     *
                     * FunctionMode:
                     * This mode is used then the Label Text comes directly from a different
                     * function. The developer is responsible for implementing the "SetLabel"
                     * delegate function.
                     *
                     * Important:
                     * In all modes this (txtAutoPopulatedButtonLabel) class is databound by
                     * using this function:
                     *   PerformDataBinding(ADataSource: System.Data.DataView; ATextBoxDataMember: String)
                     *
                     * Usage:
                     * Please make sure that the following Properties of the txtButtonLabel
                     * class have the following values according the control mode used:
                     * | Property            | TwoTableMode                      | FunctionMode    |
                     * |---------------------|-----------------------------------|-----------------|
                     * | ADataSource         | A DataView                        | A DataView      |
                     * | ATextBoxDataMember  | A Column Name                     | A Column Name   |
                     * | TextBoxLookUpMember | A Column Name of LookUpDataSource | nil / ''        |
                     * | LabelLookUpMember   | A Column Name of LookUpDataSource | nil / ''        |
                     * | LookUpDataSource    | A DataView                        | nil / ''        |
                     * | SetLabel            | nil                               | A Function Name |
                     */
                    #endregion
                    #region TListTableEnum.OccupationList
                    try
                    {
                        // Databinding
                        // this.txtAutoPopulated.PerformDataBindingTextBox(System.Data.DataView(ADataSource), ATextBoxDataMember);
                        this.txtAutoPopulated.PerformDataBinding(ADataSource, ATextBoxDataMember, TButtonLabelControlMode.TwoTableMode);
                        this.FValueMember = ATextBoxDataMember;

                        // TLogging.Log('txtAutoPopulatedButtonLabel got DataBound!');
                        // Monitor changes in the DataSource and verify them
                        MonitorDataSourceChanges(ADataSource);

                        // TLogging.Log('Monitor changes in the DataSource included!!!');
                        // End try block
                    }
                    catch (Exception E)
                    {
                        mExceptionMsg = "Could not databind OccupationList!!!" + "\n" + E.ToString();
                        throw new System.Exception(mExceptionMsg);
                    }

                    // End except block
                    // End try ... except block
                    // End TListTableEnum.OccupationList
                    #endregion
                    break;

                case TListTableEnum.PartnerKey:
                    #region TListTableEnum.PartnerKey
                    try
                    {
                        // Databinding
                        this.txtAutoPopulated.PerformDataBinding(ADataSource, ATextBoxDataMember, TButtonLabelControlMode.FunctionMode);
                        this.FValueMember = ATextBoxDataMember;

                        // TLogging.Log('txtAutoPopulatedButtonLabel got DataBound!');
                        // Monitor changes in the DataSource and verify them
                        MonitorDataSourceChanges(ADataSource);

                        // TLogging.Log('Monitor changes in the DataSource included!!!');
                        this.txtAutoPopulated.UpdateLabelText();

                        // End try block
                    }
                    catch (Exception E)
                    {
                        mExceptionMsg = "Could not databind PartnerKey!!!" + "\n" + E.ToString();
                        throw new System.Exception(mExceptionMsg);
                    }

                    // End except block
                    // End try ... except block
                    #endregion
                    break;

                case TListTableEnum.Extract:
                    #region TListTableEnum.Extract
                    try
                    {
                        // Databinding
                        this.txtAutoPopulated.PerformDataBinding(ADataSource, ATextBoxDataMember, TButtonLabelControlMode.FunctionMode);
                        this.FValueMember = ATextBoxDataMember;

                        // TLogging.Log('txtAutoPopulatedButtonLabel got DataBound!');
                        // Monitor changes in the DataSource and verify them
                        MonitorDataSourceChanges(ADataSource);

                        // TLogging.Log('Monitor changes in the DataSource included!!!');
                        this.txtAutoPopulated.UpdateLabelText();

                        // End try block
                    }
                    catch (Exception E)
                    {
                        mExceptionMsg = "Could not databind Extract!!!" + "\n" + E.ToString();
                        throw new System.Exception(mExceptionMsg);
                    }

                    // End except block
                    // End try ... except block
                    #endregion
                    break;

                case TListTableEnum.Conference:
                    #region TListTableEnum.Conference
                    try
                    {
                        /* Databinding */
                        this.txtAutoPopulated.PerformDataBinding(ADataSource, ATextBoxDataMember, TButtonLabelControlMode.FunctionMode);
                        this.FValueMember = ATextBoxDataMember;

                        /* TLogging.Log('txtAutoPopulatedButtonLabel got DataBound!'); */
                        /* Monitor changes in the DataSource and verify them */
                        MonitorDataSourceChanges(ADataSource);

                        /* TLogging.Log('Monitor changes in the DataSource included!!!'); */
                        this.txtAutoPopulated.UpdateLabelText();

                        /* End try block */
                    }
                    catch (Exception E)
                    {
                        mExceptionMsg = "Could not databind ConferenceKey!!!" + "\n" + E.ToString();
                        throw new System.Exception(mExceptionMsg);
                    }

                    /* End except block */
                    /* End try ... except block */
                    #endregion
                    break;

                case TListTableEnum.Bank:
                    #region TListTableEnum.Bank
                    try
                    {
                        // Databinding
                        this.txtAutoPopulated.PerformDataBinding(ADataSource, ATextBoxDataMember, TButtonLabelControlMode.FunctionMode);
                        this.FValueMember = ATextBoxDataMember;

                        // TLogging.Log('txtAutoPopulatedButtonLabel got DataBound!');
                        // Monitor changes in the DataSource and verify them
                        MonitorDataSourceChanges(ADataSource);

                        // TLogging.Log('Monitor changes in the DataSource included!!!');
                        this.txtAutoPopulated.UpdateLabelText();

                        // End try block
                    }
                    catch (Exception E)
                    {
                        mExceptionMsg = "Could not databind PartnerKey!!!" + "\n" + E.ToString();
                        throw new System.Exception(mExceptionMsg);
                    }

                    // End except block
                    // End try ... except block
                    #endregion
                    break;
            }
        }

        #endregion
        
        #region Custom ContextMenuStrip
        
        // create ContextMenuStrip with default items
        private void AddCustomContextMenuStrip(bool AOpenPartnerEditScreen = true)
        {
        	ContextMenuStrip CustomContextMenuStrip = new ContextMenuStrip();
        	
        	if (AOpenPartnerEditScreen)
        	{
        		CustomContextMenuStrip.Items.Add(Catalog.GetString("Partner Edit Screen"), null, new EventHandler(this.OpenPartnerEditScreen));
        	}
        	
    		CustomContextMenuStrip.Items.Add("-");
        	CustomContextMenuStrip.Items.Add(Catalog.GetString("Copy"), null, new EventHandler(this.ClickCopy));
        	CustomContextMenuStrip.Items.Add(new ToolStripSeparator());
        	CustomContextMenuStrip.Items.Add(Catalog.GetString("Select All"), null, new EventHandler(this.ClickSelectAll));
        	
        	this.txtAutoPopulated.lblLabel.ContextMenuStrip = CustomContextMenuStrip;
        	
        	// event is fired just before the contextmenustrip is displayed
        	this.txtAutoPopulated.lblLabel.ContextMenuStrip.Opening += new CancelEventHandler(CustomContextMenuStrip_Opening);
        }
        
        /// <summary>
        /// Adds items to controls ContextMenuStrip
        /// </summary>
        /// <param name="AMenuItems">Item name and corresponding EventHandler</param>
        public void AddCustomContextMenuItems(List<Tuple<string, EventHandler>> AMenuItems)
        {
        	if (AMenuItems.Count == 0)
        	{
        		return;
        	}

        	ContextMenuStrip CustomContextMenuStrip = this.txtAutoPopulated.lblLabel.ContextMenuStrip;
        	int Index = 1;
        	
        	// if a custom ContextMenuIndex has not already been created then do that now (i.e. all textboxes not using Partner Keys)
        	if (CustomContextMenuStrip == null)
        	{
        		AddCustomContextMenuStrip(false);
        		CustomContextMenuStrip = this.txtAutoPopulated.lblLabel.ContextMenuStrip;
        		Index = 0;
        	}
        	
        	foreach (Tuple<string, EventHandler> MenuItem in AMenuItems)
        	{
        		CustomContextMenuStrip.Items.Insert(Index, new ToolStripMenuItem(MenuItem.Item1, null, MenuItem.Item2));
        		Index++;
        	}
        	
        	this.txtAutoPopulated.lblLabel.ContextMenuStrip = CustomContextMenuStrip;
        }
        
        private void CustomContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
    		ContextMenuStrip CustomContextMenuStrip = this.txtAutoPopulated.lblLabel.ContextMenuStrip;
        	
    		// if textbox label is blank then disable each item or vica versa
    		if (string.IsNullOrEmpty(this.txtAutoPopulated.lblLabel.Text))
	    	{
        		foreach (ToolStripItem Item in CustomContextMenuStrip.Items)
        		{
        			Item.Enabled = false;
        		}
	    	}
    		else
			{
        		foreach (ToolStripItem Item in CustomContextMenuStrip.Items)
        		{
        			Item.Enabled = true;
        		}
	    	}
        	
        	// Set Cancel to false.
        	e.Cancel = false;
        }

        // Open's partner's PArtner Edit screen
	    private void OpenPartnerEditScreen(object sender, System.EventArgs e)
	    {
	        TCommonScreensForwarding.OpenPartnerEditScreen(Convert.ToInt64(this.txtAutoPopulated.txtTextBox.Text), this.ParentForm);
	    }
	    
	    // Copy selected text
	    private void ClickCopy(Object sender, EventArgs args)
	    {
	    	this.txtAutoPopulated.lblLabel.Copy();
	    }
	    
	    // Select all text
	    private void ClickSelectAll(Object sender, EventArgs args)
	    {
	    	this.txtAutoPopulated.lblLabel.Focus(); this.txtAutoPopulated.lblLabel.SelectAll();
	    }
        
        #endregion
    }

    /// <summary>
    /// Exception
    /// </summary>
    public class EClickButton : System.ArgumentException
    {
        /// <summary>
        /// </summary>
        public EClickButton()
            : base(txtAutoPopulatedButtonLabel.EXCEPTION_CLICK_BUTTON)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="AMessage"></param>
        public EClickButton(String AMessage)
            : base(txtAutoPopulatedButtonLabel.EXCEPTION_CLICK_BUTTON + AMessage)
        {
        }
    }

    /// <summary>
    /// Exception
    /// </summary>
    public class EWrongDataType : System.ArgumentException
    {
        /// <summary>
        /// </summary>
        public EWrongDataType()
            : base(txtAutoPopulatedButtonLabel.EXCEPTION_WRONG_DATATYPE)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="AMessage"></param>
        public EWrongDataType(String AMessage)
            : base(txtAutoPopulatedButtonLabel.EXCEPTION_WRONG_DATATYPE + AMessage)
        {
        }
    }

    /// <summary>
    /// Exception
    /// </summary>
    public class EVerificationMissing : System.ArgumentException
    {
        /// <summary>
        /// </summary>
        /// <param name="AMessage"></param>
        public EVerificationMissing(String AMessage)
            : base(txtAutoPopulatedButtonLabel.EXCEPTION_VERIFIC_MISSING + AMessage)
        {
        }
    }
}