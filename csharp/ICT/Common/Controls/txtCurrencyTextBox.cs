//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Contains a TTxtNumericTextBox which restricts the data entry to decimal numbers and shows a currency symbol next to the TextBox.
    /// </summary>
    public partial class TTxtCurrencyTextBox : UserControl
    {
        private const string COLUMNNAME_CURRENCY_NAME = "a_currency_name_c";
        private const string COLUMNNAME_DISPLAYFORMAT_NAME = "a_display_format_c";

        private int FOriginalTxtNumericWidth;
        private int FLastControlWidth = -1;
        private string FCurrencyName;

        private static TRetrieveCurrencyList FRetrieveCurrencyList;
        private static DataTable GCurrencyList;
        string FCurrencyDisplayFormat = "->>>,>>>,>>>,>>9.99";

        #region Properties (handed through to TTxtNumericTextBox!)

        /// <summary>
        /// This Property is ignored (!) unless ControlMode is 'NormalTextMode'! For all other cases, the value to be displayed needs to be set programmatically through the 'NumberValueDecimal' or 'NumberValueInt' Properties.
        /// </summary>
        [Description(
             "This Property is ignored (!) unless ControlMode is 'NormalTextMode'! For all other cases, the value to be displayed needs to be set programmatically through the 'NumberValueDecimal' or 'NumberValueInt' Properties.")
        ]
        public override string Text
        {
            get
            {
                return FTxtNumeric.Text;
            }

            set
            {
                FTxtNumeric.Text = value;
            }
        }

        /// <summary>
        /// Determines the number of decimal places (valid only for Decimal and Currency ControlModes).
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(2),
         Browsable(true),
         Description("Determines the number of decimal places (valid only for Decimal and Currency ControlModes).")]
        public int DecimalPlaces
        {
            get
            {
                return FTxtNumeric.DecimalPlaces;
            }

            set
            {
                FTxtNumeric.DecimalPlaces = value;
            }
        }

        /// <summary>
        /// Determines whether the control allows a null value, or not.
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(false),
         Browsable(true),
         Description("Determines whether the control allows a null value, or not.")]
        public bool NullValueAllowed
        {
            get
            {
                return FTxtNumeric.NullValueAllowed;
            }

            set
            {
                FTxtNumeric.NullValueAllowed = value;
            }
        }

        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0.00)]
        public decimal ? NumberValueDecimal
        {
            get
            {
                if (!DesignMode)
                {
                    if (FTxtNumeric.Text != String.Empty)
                    {
                        decimal? Ret = null;
                        try
                        {
                            Decimal LocalCultureVersion;

                            if (Decimal.TryParse(FTxtNumeric.Text, out LocalCultureVersion))
                            {
                                Ret = LocalCultureVersion;
                            }
                            else
                            {
                                Ret = Convert.ToDecimal(FTxtNumeric.Text, FTxtNumeric.Culture);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        return Ret;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return 0;
                }
            }

            set
            {
                if (value != null)
                {
                    FTxtNumeric.SetCurrencyValue(value.Value, FCurrencyDisplayFormat);
                }
                else
                {
                    if (FTxtNumeric.FNullValueAllowed)
                    {
                        ((TextBox)FTxtNumeric).Text = String.Empty;
                        return;
                    }
                    else
                    {
                        throw new ArgumentNullException(
                            "The 'NumberValueDecimal' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                    }
                }
            }
        }

        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0.00)]
        public double ? NumberValueDouble
        {
            get
            {
                return FTxtNumeric.NumberValueDouble;
            }

            set
            {
                FTxtNumeric.NumberValueDouble = value;
            }
        }

        /// <summary>
        /// Whether the TextBox Control is read-only, or not.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return FTxtNumeric.ReadOnly;
            }

            set
            {
                FTxtNumeric.ReadOnly = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determines the currency symbol.
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue("###"),
         Browsable(true),
         Description("Determines the currency symbol.")]
        public string CurrencyCode
        {
            get
            {
                return FLblCurrency.Text;
            }

            set
            {
                FLblCurrency.Text = value;

                if (value == String.Empty)
                {
                    FLblCurrency.Visible = false;
                }
                else
                {
                    FLblCurrency.Visible = true;
                }

                if (GCurrencyList != null)
                {
                    DataRow CurrencyDR = GCurrencyList.Rows.Find(value);

                    if (CurrencyDR != null)
                    {
                        FCurrencyName = (string)CurrencyDR[COLUMNNAME_CURRENCY_NAME];

                        tipCurrencyName.SetToolTip(FLblCurrency, FCurrencyName);

                        FCurrencyDisplayFormat = (string)CurrencyDR[COLUMNNAME_DISPLAYFORMAT_NAME];
                        int DecimalSeparatorPos = FCurrencyDisplayFormat.LastIndexOf('.');

                        if (DecimalSeparatorPos != -1)
                        {
                            this.DecimalPlaces = FCurrencyDisplayFormat.Length - DecimalSeparatorPos - 1;
                        }
                        else
                        {
                            this.DecimalPlaces = 0;
                        }
                    }
                    else
                    {
                        FCurrencyName = String.Empty;
                        FCurrencyDisplayFormat = "->>>,>>>,>>>,>>9.99";
                    }
                }
            }
        }

        /// <summary>
        /// The name of the currency. Only available after (1) assigning the RetrieveCurrencyList Delegate and
        /// (2) assigning the Currency Property and (3) that Currency was found in the Currency List retrieved by the Delegate.
        /// </summary>
        public string CurrencyName
        {
            get
            {
                return FCurrencyName;
            }
        }

        /// <summary>
        /// Gets or sets how text is aligned in the TextBox.
        /// </summary>
        public System.Windows.Forms.HorizontalAlignment TextAlign
        {
            get
            {
                return FTxtNumeric.TextAlign;
            }

            set
            {
                FTxtNumeric.TextAlign = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised whenever the TextBox raises the TextChanged Event.
        /// </summary>
        public new event EventHandler TextChanged;

        /// <summary>
        /// Loads a DataTable that contains the list of currencies.
        /// </summary>
        /// <remarks>See implementation in Ict.Petra.Client.CommonControls.TControlExtensions.RetrieveCurrencyList!</remarks>
        /// <returns>A DataTable that contains the list of currencies.</returns>
        public delegate DataTable TRetrieveCurrencyList();

        /// <summary>
        /// This property is used to provide a function which loads the list of Currencies.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TRetrieveCurrencyList RetrieveCurrencyList
        {
            get
            {
                return FRetrieveCurrencyList;
            }

            set
            {
                FRetrieveCurrencyList = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TTxtCurrencyTextBox()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.FLblCurrency.Text = Catalog.GetString("WWW");
            #endregion

            FTxtNumeric.TextChanged += new EventHandler(OnTextChanged);

            if (FRetrieveCurrencyList != null)
            {
                if (GCurrencyList == null)
                {
                    GCurrencyList = FRetrieveCurrencyList();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects all text in the TextBox.
        /// </summary>
        public void SelectAll()
        {
            FTxtNumeric.SelectAll();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Maintain the custom layout of the TextBox and the Label.
        /// </summary>
        private void MaintainLayoutOfContainedControls()
        {
            FOriginalTxtNumericWidth = FLblCurrency.Left + 4;
            FTxtNumeric.Width = FOriginalTxtNumericWidth;
        }

        #endregion

        #region Event Handlers

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }

        void TTxtCurrencyTextBoxLayout(object sender, LayoutEventArgs e)
        {
            FLblCurrency.Width = 37;
            FLblCurrency.Left = this.Size.Width - FLblCurrency.Width - 1;
            FTxtNumeric.Left = 0;

            if (FLastControlWidth != this.Size.Width)
            {
                FLastControlWidth = this.Size.Width;

                MaintainLayoutOfContainedControls();
            }

            if (FLblCurrency.Text != String.Empty)
            {
                FTxtNumeric.Width = FOriginalTxtNumericWidth;
            }
            else
            {
                FTxtNumeric.Width = this.Width;
            }
        }

//        /// <summary>
//        /// Only for debugging the layout of the Controls
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        void FLblCurrencyDoubleClick(object sender, EventArgs e)
//        {
//            MessageBox.Show(String.Format("this.Name: {0}    this.Width: {1}\r\nFLblCurrency.Width: {2}    FLblCurrency.Left: {3}\r\nFTxtNumeric.Width: {4}",
//                this.Name, this.Width, FLblCurrency.Width, FLblCurrency.Left, FTxtNumeric.Width));
//        }

        #endregion
    }
}