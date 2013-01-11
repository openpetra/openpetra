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
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Contains a TTxtNumericTextBox which restricts the data entry to decimal numbers and shows a currency symbol next to the TextBox.
    /// </summary>
    public partial class TTxtCurrencyTextBox : UserControl
    {
        private int FOriginalTxtNumericWidth;
        
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
                return FTxtNumeric.NumberValueDecimal;
            }

            set
            {
                FTxtNumeric.NumberValueDecimal = value;
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
        public string CurrencySymbol
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
            }
        }
        
        /// <summary>
        /// Gets or sets how text is aliagned in the TextBox.
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

        /// <summary>
        /// Gets or sets the width of the Control.
        /// </summary>
        public new int Width
        {
            get
            {
                return base.Width;
            }
            
            set
            {
                base.Width = value;
                
                MaintainLayoutOfContainedControls();
            }
        }

        /// <summary>
        /// Gets or sets the height and width of the Control.
        /// </summary>
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            
            set
            {
                base.Size = value;
                
                MaintainLayoutOfContainedControls();
            }
        }
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// Raised whenever the TextBox raises the TextChanged Event.
        /// </summary>
        public new event EventHandler TextChanged;
        
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
            
            FTxtNumeric.TextChanged += new EventHandler(OnTextChanged);
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
            
            if (FLblCurrency.Text != String.Empty)
            {
                FTxtNumeric.Width = FOriginalTxtNumericWidth;
            }
            else
            {
                FTxtNumeric.Width = this.Width;
            }
        }
        
        /// <summary>
        /// Only for debugging the layout of the Controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FLblCurrencyDoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("this.Name: {0}    this.Width: {1}\r\nFLblCurrency.Width: {2}    FLblCurrency.Left: {3}\r\nFTxtNumeric.Width: {4}",
                this.Name, this.Width, FLblCurrency.Width, FLblCurrency.Left, FTxtNumeric.Width));
        }

        #endregion
    }
}
