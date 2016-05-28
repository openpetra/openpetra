//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using Ict.Common.Exceptions;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Extends a normal textbox and restricts text to numbers.
    /// Can be used as a normal textbox or as a textbox that restricts text to certain type of numbers
    /// by setting the ControlMode property.
    ///
    /// There are three ways this control can operate, determined by the ControlMode property
    /// ControlMode.NormalTextBox  - Behave as a completely normal textbox
    /// ControlMode.Integer        - accepts only numbers without digits
    /// ControlMode.Decimal        - accepts only numbers including digits and formats the number
    ///                              according to the DecimalPlaces Property.
    /// </summary>
    public class TTxtNumericTextBox : System.Windows.Forms.TextBox
    {
        private const Int32 CONTROL_CHARS_BACKSPACE = 8;
        private const int WM_PASTE = 0x0302;
        private const int WM_CUT = 0x0300;
        private const int WM_CLEAR = 0x0303;

        /// <summary>
        /// todoComment
        /// </summary>
        public enum TNumberPrecision
        {
            /// <summary>
            /// todoComment
            /// </summary>
            Decimal,
            /// <summary>
            /// todoComment
            /// </summary>
            Double
        }

        private TNumericTextBoxMode FControlMode = TNumericTextBoxMode.Decimal;
        private TNumberPrecision FNumberPrecision = TNumberPrecision.Decimal;
        private int FDecimalPlaces = 2;
        private String FContext = null;

        private bool FIsCurrencyTextBox = false;    // True if this numeric text box is associated with a TTxtCurrencyTextBox control

        /// <summary>
        /// Is it OK to show {null} in this control?
        /// </summary>
        public bool FNullValueAllowed = false;

        /// <summary>
        /// Is a negative value allowed? Default true.
        /// </summary>
        public bool FNegativeValueAllowed = true;

        private string FNumberDecimalSeparator = ".";
        private string FCurrencyDecimalSeparator = ".";
        private string FCurrencyDisplayFormat = "";
        private CultureInfo FCurrentCulture;
        private bool FShowPercentSign = false;
        private string FNumberPositiveSign = "+";
        private string FNumberNegativeSign = "-";

        // User preferences
        private static TRetrieveUserDefaultBoolean FRetrieveUserDefaultBoolean;
        private bool FShowCurrencyThousands = true;

        // Undo
        private string FCurrentUndoString = null;

        /// <summary>
        /// todoComment
        /// </summary>
        public enum TNumericTextBoxMode
        {
            /// <summary>
            /// todoComment
            /// </summary>
            NormalTextBox,
            /// <summary>
            /// todoComment
            /// </summary>
            Integer,
            /// <summary>
            /// todoComment
            /// </summary>
            LongInteger,
            /// <summary>
            /// todoComment
            /// </summary>
            Decimal,
            /// <summary>
            /// todoComment
            /// </summary>
            Currency
        }

        #region Properties

        /// <summary>
        /// This Property throws an exception unless ControlMode is 'NormalTextMode'!
        /// For all other cases, the value to be displayed needs to be set programmatically
        /// through the 'NumberValueDecimal' or 'NumberValueInt' Properties or the 'SetCurrencyValue'
        /// method.
        /// </summary>
        [Description(
             "This Property throws an exception unless ControlMode is 'NormalTextMode'! For all other cases, the value to be displayed needs to be set programmatically through the 'NumberValueDecimal' or 'NumberValueInt' Properties or the 'SetCurrencyValue' method.")
        ]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (FControlMode == TNumericTextBoxMode.NormalTextBox)
                {
                    base.Text = value;
                }
                else
                {
                    throw new Exception("Text property cannot be set directly in Numeric TextBox.");
                }
            }
        }

        /// <summary>
        /// This Culture came originally from the thread (ie from the user's locale setup)
        /// </summary>
        public CultureInfo Culture
        {
            set
            {
                // This gets modified as a test but I don't think we dynamically switch cultures in a given text box in a real screen situation (yet?)
                FCurrentCulture = value;
                FNumberDecimalSeparator = FCurrentCulture.NumberFormat.NumberDecimalSeparator;     // TODO: make this customisable in Client .config file
                FCurrencyDecimalSeparator = FCurrentCulture.NumberFormat.CurrencyDecimalSeparator; // TODO: make this customisable in Client .config file
            }

            get
            {
                return FCurrentCulture;
            }
        }

        /// <summary>
        /// Determines what input the Control accepts and how it formats it.
        /// Note that a ControlMode of Currency does not always imply that the box is a currency text box.
        /// A ControlMode of Currency means that the system currency format (decimal and thousands) is used to format the text.
        /// A ControlMode of decimal means that the system number format (decimal and thousands) is used to format the decimal point and separator.
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("Determines what input the Control accepts and how it formats it.")]
        public TNumericTextBoxMode ControlMode
        {
            get
            {
                return FControlMode;
            }

            set
            {
                FControlMode = value;

                if ((value == TNumericTextBoxMode.Decimal) || (value == TNumericTextBoxMode.Currency))
                {
                    // The proposed mode may be modified by the user settings and by the context
                    if (FRetrieveUserDefaultBoolean != null)
                    {
                        // At the time of doing this all finance reports are in Partner, so this IF statement is good
                        // enough to find Finance screens as opposed to Partner/Personnel and Conference
                        if (FContext.Contains(".MFinance") && !FContext.Contains("MReporting"))
                        {
                            // The context is a Finance screen
                            if (FIsCurrencyTextBox)
                            {
                                if (FRetrieveUserDefaultBoolean(StringHelper.FINANCE_CURRENCY_FORMAT_AS_CURRENCY, true))
                                {
                                    FControlMode = TNumericTextBoxMode.Currency;
                                }
                                else
                                {
                                    FControlMode = TNumericTextBoxMode.Decimal;
                                }
                            }
                            else
                            {
                                if (FRetrieveUserDefaultBoolean(StringHelper.FINANCE_DECIMAL_FORMAT_AS_CURRENCY, true))
                                {
                                    FControlMode = TNumericTextBoxMode.Currency;
                                }
                                else
                                {
                                    FControlMode = TNumericTextBoxMode.Decimal;
                                }
                            }
                        }
                        else
                        {
                            // The context is Partner/Personnel/Conference
                            if (FIsCurrencyTextBox)
                            {
                                if (FRetrieveUserDefaultBoolean(StringHelper.PARTNER_CURRENCY_FORMAT_AS_CURRENCY, false))
                                {
                                    FControlMode = TNumericTextBoxMode.Currency;
                                }
                                else
                                {
                                    FControlMode = TNumericTextBoxMode.Decimal;
                                }
                            }
                            else
                            {
                                if (FRetrieveUserDefaultBoolean(StringHelper.PARTNER_DECIMAL_FORMAT_AS_CURRENCY, false))
                                {
                                    FControlMode = TNumericTextBoxMode.Currency;
                                }
                                else
                                {
                                    FControlMode = TNumericTextBoxMode.Decimal;
                                }
                            }
                        }
                    }
                }

                if ((value == TNumericTextBoxMode.NormalTextBox)
                    || (value == TNumericTextBoxMode.Integer)
                    || (value == TNumericTextBoxMode.LongInteger))
                {
                    FDecimalPlaces = 0;
                }

                if (DesignMode)
                {
                    if (value != TNumericTextBoxMode.NormalTextBox)
                    {
                        base.Text = "1234";
                    }
                    else
                    {
                        base.Text = "NormalTextBox Mode";
                    }
                }

                FormatValue(RemoveNonNumeralChars());
            }
        }

        /// <summary>
        /// Determines the number of decimal places (valid only for Decimal ControlMode).
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(2),
         Browsable(true),
         Description("Determines the number of decimal places (valid only for Decimal ControlMode).")]
        public int DecimalPlaces
        {
            get
            {
                return FDecimalPlaces;
            }

            set
            {
                if ((FControlMode == TNumericTextBoxMode.Decimal) || (FControlMode == TNumericTextBoxMode.Currency))
                {
                    FDecimalPlaces = value;
                }
                else
                {
                    FDecimalPlaces = 0;
                }

                if (DesignMode)
                {
                    if (FControlMode != TNumericTextBoxMode.NormalTextBox)
                    {
                        base.Text = "1234";
                    }

                    if ((FControlMode == TNumericTextBoxMode.NormalTextBox)
                        || (FControlMode == TNumericTextBoxMode.Integer)
                        || (FControlMode == TNumericTextBoxMode.LongInteger))
                    {
                        if (value != 0)
                        {
                            FDecimalPlaces = 0;
                        }
                    }
                }

                FormatValue(RemoveNonNumeralChars());
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
                return FNullValueAllowed;
            }

            set
            {
                FNullValueAllowed = value;
            }
        }

        /// <summary>
        /// Sets the context for this control - the class of Form or UserControl on which it has been placed.
        /// The designer file sets this value automatically
        /// </summary>
        public object Context
        {
            set
            {
                FContext = value.ToString();
            }
        }

        /// <summary>
        /// Gets/sets if the text box is associated with a TTxtCurrencyTextBox.  This is set automatically.
        /// </summary>
        public Boolean IsCurrencyTextBox
        {
            get
            {
                return FIsCurrencyTextBox;
            }

            set
            {
                FIsCurrencyTextBox = value;
            }
        }

        /// <summary>
        /// Determines whether the control allows a negative value, or not. Default = true.
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true),
         Description("Determines whether the control allows a negative value, or not. Default = true.")]
        public bool NegativeValueAllowed
        {
            get
            {
                return FNegativeValueAllowed;
            }

            set
            {
                FNegativeValueAllowed = value;
            }
        }


        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0.00),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal ? NumberValueDecimal
        {
            get
            {
                string CleanedfromNonNumeralChars;

                if (!DesignMode)
                {
                    if (this.Text != String.Empty)
                    {
                        CleanedfromNonNumeralChars = RemoveNonNumeralChars();

                        if ((CleanedfromNonNumeralChars != "-")
                            && (CleanedfromNonNumeralChars != ".")
                            && (CleanedfromNonNumeralChars != String.Empty))
                        {
                            decimal Ret;

                            if (decimal.TryParse(CleanedfromNonNumeralChars, NumberStyles.Any, FCurrentCulture, out Ret))
                            {
                                return Ret;
                            }

                            return null;
                        }
                        else
                        {
                            return 0;
                        }
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
                if (FControlMode == TNumericTextBoxMode.Decimal)
                {
                    FNumberPrecision = TNumberPrecision.Decimal;

                    if (value == null)
                    {
                        if (FNullValueAllowed)
                        {
                            base.Text = String.Empty;
                            return;
                        }
                        else
                        {
                            throw new ArgumentNullException(
                                "The 'NumberValueDecimal' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    base.Text = ((decimal)value).ToString(FCurrentCulture);
                    FormatValue(RemoveNonNumeralChars());
                }
                else if (FControlMode == TNumericTextBoxMode.Currency)
                {
                    // The box is using currency format for decimal point but may be a plain numeric text box
                    if (value == null)
                    {
                        if (FNullValueAllowed)
                        {
                            base.Text = String.Empty;
                            return;
                        }
                        else
                        {
                            throw new ArgumentNullException(
                                "The 'NumberValueDecimal' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    if (FIsCurrencyTextBox)
                    {
                        // Currency formatted currency box
                        base.Text = StringHelper.FormatCurrency(new TVariant(value), FCurrencyDisplayFormat);
                    }
                    else
                    {
                        // Currency formatted decimal numeric box
                        base.Text = ((decimal)value).ToString(FCurrentCulture);
                        FormatValue(RemoveNonNumeralChars());
                    }
                }

                FCurrentUndoString = null;

// Sharp Developer Designer Bug makes Integer mode unusable
//                else
//                {
//                    if (!DesignMode)
//                    {
//                        throw new EOPAppException(
//                            "The 'NumberValueDecimal' Property can only be set if the 'ControlMode' Property is 'Decimal'!");
//                    }
//                }
            }
        }

        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0.00)]
        public double ? NumberValueDouble
        {
            get
            {
                if (!DesignMode)
                {
                    if (this.Text != String.Empty)
                    {
                        return Convert.ToDouble(RemoveNonNumeralChars(), FCurrentCulture);
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
                if (FControlMode == TNumericTextBoxMode.Decimal)
                {
                    FNumberPrecision = TNumberPrecision.Double;

                    if (value != null)
                    {
                        base.Text = ((double)value).ToString(FCurrentCulture);
                    }
                    else
                    {
                        if (FNullValueAllowed)
                        {
                            base.Text = String.Empty;
                            return;
                        }
                        else
                        {
                            throw new ArgumentNullException(
                                "The 'NumberValueDouble' Property must not be set to if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    FormatValue(RemoveNonNumeralChars());
                }
                else
                {
                    if (!DesignMode)
                    {
                        throw new EOPAppException(
                            "The 'NumberValueDouble' Property can only be set if the 'ControlMode' Property is 'Decimal'!");
                    }
                }

                FCurrentUndoString = null;
            }
        }

        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0)]
        public int ? NumberValueInt
        {
            get
            {
                if (!DesignMode)
                {
                    if (this.Text != String.Empty)
                    {
                        if (!FShowPercentSign)
                        {
                            return Convert.ToInt32(this.Text);
                        }
                        else
                        {
                            if (this.Text.EndsWith(" %"))
                            {
                                return Convert.ToInt32(this.Text.Substring(0, this.Text.Length - 2));
                            }
                            else if (this.Text.EndsWith("%"))
                            {
                                return Convert.ToInt32(this.Text.Substring(0, this.Text.Length - 1));
                            }
                            else
                            {
                                return Convert.ToInt32(this.Text);
                            }
                        }
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
                if (FControlMode == TNumericTextBoxMode.Integer)
                {
                    if (value != null)
                    {
                        base.Text = value.ToString();
                    }
                    else
                    {
                        if (FNullValueAllowed)
                        {
                            base.Text = String.Empty;
                            return;
                        }
                        else
                        {
                            throw new ArgumentNullException(
                                "The 'NumberValueInt' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    FormatValue(RemoveNonNumeralChars());
                    FCurrentUndoString = null;
                }
                else
                {
                    if (!DesignMode)
                    {
                        throw new EOPAppException(
                            "The 'NumberValueInt' Property can only be set if the 'ControlMode' Property is 'Integer'!");
                    }
                }
            }
        }


        /// This property gets hidden because it doesn't make sense in the Designer!
        [Browsable(false),
         DefaultValue(0),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ? NumberValueLongInt
        {
            get
            {
                if (!DesignMode)
                {
                    if (this.Text != String.Empty)
                    {
                        if (!FShowPercentSign)
                        {
                            return Convert.ToInt64(this.Text);
                        }
                        else
                        {
                            if (this.Text.EndsWith(" %"))
                            {
                                return Convert.ToInt64(this.Text.Substring(0, this.Text.Length - 2));
                            }
                            else if (this.Text.EndsWith("%"))
                            {
                                return Convert.ToInt64(this.Text.Substring(0, this.Text.Length - 1));
                            }
                            else
                            {
                                return Convert.ToInt64(this.Text);
                            }
                        }
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
                if ((FControlMode == TNumericTextBoxMode.LongInteger))
                {
                    if (value != null)
                    {
                        base.Text = value.ToString();
                    }
                    else
                    {
                        if (FNullValueAllowed)
                        {
                            base.Text = String.Empty;
                            return;
                        }
                        else
                        {
                            throw new ArgumentNullException(
                                "The 'NumberValueLongInt' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    FormatValue(RemoveNonNumeralChars());
                    FCurrentUndoString = null;
                }
                else
                {
                    if (!DesignMode)
                    {
                        throw new EOPAppException(
                            "The 'NumberValueLongInt' Property can only be set if the 'ControlMode' Property is 'LongInteger'!");
                    }
                }
            }
        }

        /// <summary>
        /// Determines where a percent sign ( % ) is shown. Only has an effect if ControlMode is 'Integer' or 'Decimal'.
        /// </summary>
        [Category("NumericTextBox"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(false),
         Browsable(true),
         Description(
             "Determines where a percent sign ( % ) is shown. Only has an effect if ControlMode is 'Integer' or 'Decimal'.")
        ]
        public bool ShowPercentSign
        {
            get
            {
                return FShowPercentSign;
            }

            set
            {
                FShowPercentSign = value;

                FormatValue(RemoveNonNumeralChars());
            }
        }

        /// <summary>
        /// Used by the User Settings dialog to dynamically show examples.  Not for general use.
        /// </summary>
        public bool ShowThousands
        {
            set
            {
                FShowCurrencyThousands = value;
                FormatValue(RemoveNonNumeralChars());
            }
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TTxtNumericTextBox()
        {
            NumberFormatInfo NfiCurrenThread = System.Globalization.NumberFormatInfo.CurrentInfo;

            FNumberDecimalSeparator = NfiCurrenThread.NumberDecimalSeparator;     // TODO: make this customisable in Client .config file
            FCurrencyDecimalSeparator = NfiCurrenThread.CurrencyDecimalSeparator; // TODO: make this customisable in Client .config file

            FCurrentCulture = Thread.CurrentThread.CurrentCulture;

            // Hook up Events
            this.KeyPress += new KeyPressEventHandler(OnKeyPress);
            this.Leave += new EventHandler(OnLeave);
            this.Enter += new EventHandler(OnEntering);

//            this.MouseDown += new MouseEventHandler(OnMouseDown);
//            this.LostFocus +=new EventHandler(OnLostFocus);

            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (Byte)0);

            FormatValue("0");
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (this.ControlMode == TNumericTextBoxMode.NormalTextBox)
            {
                // just be a textbox!
                e.Handled = false;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this, true, true, true, true);
                e.Handled = true;
            }

            // handle COPY
            if ((e.KeyCode == Keys.C) && (e.Modifiers == Keys.Control))
            {
                try
                {
                    Clipboard.SetDataObject(this.SelectedText);
                    e.Handled = true;
                }
                catch (Exception)
                {
//                  MessageBox.Show("Exception in OnKeyDown: " + exp.ToString());

                    // never mind
                }
            }

            // handle SELECT ALL
            if ((e.KeyCode == Keys.A) && (e.Modifiers == Keys.Control))
            {
                this.SelectAll();
                e.Handled = true;
            }

            // handle CUT
            if ((e.KeyCode == Keys.X) && (e.Modifiers == Keys.Control))
            {
                HandleCut();
                e.Handled = true;
            }

            // handle PASTE
            if (((e.KeyCode == Keys.V) && (e.Modifiers == Keys.Control))
                || ((e.KeyCode == Keys.Insert) && (e.Shift)))
            {
                HandlePaste();
                e.Handled = true;
            }

            if ((e.KeyCode == Keys.Z) && (e.Modifiers == Keys.Control))
            {
                UndoChanges();
                e.Handled = true;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            Char chrKeyPressed;
            Int32 intSelStart;
            Int32 intDelTo;
            bool DecimalPointEntered = false;
            String strText;
            bool bolDelete;
            bool bolDecimalplaceInvalid = false;
            int intActDecPlace = -1;

            if (this.ControlMode == TNumericTextBoxMode.NormalTextBox)
            {
                // just be a textbox!
                e.Handled = false;
                return;
            }

            if (this.ReadOnly == true)
            {
                // no further action
                e.Handled = true;
                return;
            }

            try
            {
                chrKeyPressed = e.KeyChar;

                // Original cursor position
                intSelStart = this.SelectionStart;

                if (this.SelectionLength == this.Text.Length)
                {
                    if (!(Control.ModifierKeys == Keys.Control))
                    {
                        //ClearBox();
                        base.Text = "";
//                        intSelStart = this.Text.Length;
                        intSelStart = 0;
                    }
                }

                // In case of a selection, delete text to this position
                intDelTo = intSelStart + this.SelectionLength - 1;
                strText = this.Text;

                // Used to avoid deletion of the selection when an invalid key is pressed
                bolDelete = false;

                e.Handled = true;

                if ((chrKeyPressed == (char)(CONTROL_CHARS_BACKSPACE))
                    && (this.SelectionStart != 0))
                {
                    bolDelete = true;

                    if ((intSelStart > 0) && (intDelTo < intSelStart))
                    {
                        intSelStart = intSelStart - 1;
                    }
                }

                switch (FControlMode)
                {
                    case TNumericTextBoxMode.Integer :
                    case TNumericTextBoxMode.LongInteger :
                    case TNumericTextBoxMode.Decimal :
                    case TNumericTextBoxMode.Currency :
                        {
                            #region Numeric Validation Rule

                            if (FControlMode == TNumericTextBoxMode.Decimal)
                            {
                                intActDecPlace = this.Text.IndexOf(FNumberDecimalSeparator);
                            }

                            if (FControlMode == TNumericTextBoxMode.Currency)
                            {
                                intActDecPlace = this.Text.IndexOf(FCurrencyDecimalSeparator);
                            }

                            // Check & Reset boolean if the decimal place does not exist
                            if (intActDecPlace != -1)
                            {
                                DecimalPointEntered = true;
                            }

//TLogging.Log("FDecimalPointEntered: " + DecimalPointEntered.ToString());
                            // If Keypressed is of type numeric or the decimal separator (usually ".")
                            if (Char.IsDigit(chrKeyPressed))
                            {
                                if ((this.ControlMode == TNumericTextBoxMode.NormalTextBox) && (FCurrentUndoString != null)
                                    && (FCurrentUndoString != this.Text))
                                {
                                    // We need to ascertain if the keypress is within the bounds of the current Undo block.
                                    // If not then the Undo string needs to be reset and we start a new edit in the new block.
                                    int selStart;
                                    int selLength;
                                    GetEditRange(FCurrentUndoString, this.Text, out selStart, out selLength);

                                    if ((this.SelectionStart < selStart) || (this.SelectionStart > selStart + selLength))
                                    {
                                        // Starting a new edit block in a new location within the text.
                                        ResetUndo();
                                    }
                                }

                                #region Decimal place check

                                if (FDecimalPlaces > 0)
                                {
                                    if ((this.SelectionLength == 0)
                                        && (intSelStart > intActDecPlace))
                                    {
                                        // Decimalplace validation ...
                                        // This checks to see if the inserted character would fit within the number of allowable decimal places.
                                        // If so, insert the character and push the remaining decimal places to the right.
                                        // The least significant digit will fall off the end
                                        if ((ControlMode == TNumericTextBoxMode.Decimal) || (ControlMode == TNumericTextBoxMode.Currency))
                                        {
                                            if (!ShowPercentSign)
                                            {
                                                bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace) > FDecimalPlaces;

                                                if (bolDecimalplaceInvalid && (intSelStart < this.Text.Length))
                                                {
                                                    base.Text = this.Text.Substring(0, intSelStart) + chrKeyPressed +
                                                                this.Text.Substring(intSelStart, this.Text.Length - intSelStart - 1);
                                                    this.SelectionStart = intSelStart + 1;
                                                    e.Handled = true;
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                if (this.Text.EndsWith(" %"))
                                                {
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace - 2) > FDecimalPlaces;

                                                    if (bolDecimalplaceInvalid && (intSelStart < this.Text.Length - 2))
                                                    {
                                                        base.Text = this.Text.Substring(0, intSelStart) + chrKeyPressed +
                                                                    this.Text.Substring(intSelStart, this.Text.Length - intSelStart - 3) + " %";
                                                        this.SelectionStart = intSelStart + 1;
                                                        e.Handled = true;
                                                        return;
                                                    }
                                                }
                                                else if ((this.Text.EndsWith("%")
                                                          || (this.Text.EndsWith(" "))))
                                                {
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace - 1) > FDecimalPlaces;

                                                    if (bolDecimalplaceInvalid && (intSelStart < this.Text.Length - 1))
                                                    {
                                                        base.Text = this.Text.Substring(0, intSelStart) + chrKeyPressed +
                                                                    this.Text.Substring(intSelStart, this.Text.Length - intSelStart - 2) + "%";
                                                        this.SelectionStart = intSelStart + 1;
                                                        e.Handled = true;
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    // same as if !ShowPercentSign
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace) > FDecimalPlaces;

                                                    if (bolDecimalplaceInvalid && (intSelStart < this.Text.Length))
                                                    {
                                                        base.Text = this.Text.Substring(0, intSelStart) + chrKeyPressed +
                                                                    this.Text.Substring(intSelStart, this.Text.Length - intSelStart - 1);
                                                        this.SelectionStart = intSelStart + 1;
                                                        e.Handled = true;
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                        if (DecimalPointEntered && bolDecimalplaceInvalid)
                                        {
                                            e.Handled = true;
                                            return;
                                        }
                                    }
                                    else if (this.SelectionLength == this.Text.Length)
                                    {
                                        e.Handled = false;
                                    }
                                }

                                #endregion

                                // Don't allow a digit to be entered after the percent sign
                                if (FShowPercentSign
                                    && (this.Text.IndexOf("%") > -1)
                                    && (intSelStart > this.Text.IndexOf("%")))
                                {
                                    e.Handled = true;
                                    return;
                                }

                                e.Handled = false;
                            }
                            else if (IsDecimalSeparator(chrKeyPressed))
                            {
                                #region Decimal validation

                                if ((FControlMode != TNumericTextBoxMode.Integer)
                                    && (FControlMode != TNumericTextBoxMode.LongInteger))
                                {
                                    if (DecimalPointEntered != true)
                                    {
                                        if (intSelStart > 0)
                                        {
                                            // Don't allow a decimal separator to be entered after any non-number
//TLogging.Log("IsDecimalSeparator: Char before intSelStart #1: " + this.Text.Substring(intSelStart - 1, 1).ToCharArray()[0].ToString());
                                            if (!Char.IsDigit(this.Text.Substring(intSelStart - 1, 1).ToCharArray()[0]))
                                            {
                                                e.Handled = true;
                                                return;
                                            }
                                            else
                                            {
                                                DecimalPointEntered = true;
                                                e.Handled = false;
                                            }
                                        }
                                        else
                                        {
                                            DecimalPointEntered = true;
                                            e.Handled = false;
                                        }
                                    }
                                    else
                                    {
                                        // Decimal point entered and we have one already.  If the current position is the dp we just move the selection on by one.
                                        // Otherwise we do nothing because you can't have two dp's!!
                                        if (intSelStart == intActDecPlace)
                                        {
                                            this.SelectionStart = intSelStart + 1;
                                            this.SelectionLength = 0;
                                        }

                                        e.Handled = true;
                                    }
                                }
                                else
                                {
                                    // no decimal point allowed with Integers
                                    e.Handled = true;
                                }

                                #endregion
                            }
                            else if (chrKeyPressed == FNumberNegativeSign[0])
                            {
                                // allow negative sign only in front of all digits, and only if it isn't there yet
                                if ((intSelStart == 0)
                                    && (!this.Text.Contains(FNumberNegativeSign))
                                    && NegativeValueAllowed)
                                {
                                    e.Handled = false;
                                }
                                else
                                {
                                    e.Handled = true;
                                }
                            }
                            else if (chrKeyPressed == FNumberPositiveSign[0])
                            {
                                // allow positive sign to be entered in front of all digits, and only if there is a negative sign
                                if ((intSelStart == 0)
                                    || (intSelStart == 1))
                                {
                                    if (this.Text.Substring(0, FNumberNegativeSign.Length) == FNumberNegativeSign)
                                    {
                                        base.Text = this.Text.Substring(1);
                                        e.Handled = false;
                                    }
                                    else
                                    {
                                        e.Handled = true;
                                    }
                                }
                                else
                                {
                                    e.Handled = true;
                                }
                            }
                            else if (chrKeyPressed == '%')
                            {
                                if (FShowPercentSign)
                                {
                                    // allow percent sign ( % ) only at end of all the text, and not as the only character, and only if it isn't there already
                                    if ((intSelStart == this.Text.Length)
                                        && !(intSelStart == 0)
                                        && !this.Text.Contains("%"))
                                    {
                                        e.Handled = false;
                                    }
                                    else
                                    {
                                        e.Handled = true;
                                    }
                                }
                            }
                            else
                            {
                                e.Handled = true;
                            }

                            #endregion

                            break;
                        }

                    case TNumericTextBoxMode.NormalTextBox:
                    {
                        //Nothing here..
                        break;
                    }
                }

                if (bolDelete == true)
                {
                    base.Text = strText.Substring(0, intSelStart) + strText.Substring(intSelStart + 1);
                    this.SelectionStart = intSelStart;
                    this.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OnKeyPress Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void ClearBox()
        {
            if (FNullValueAllowed)
            {
                base.Text = String.Empty;
            }
            else
            {
                switch (FControlMode)
                {
                    case TNumericTextBoxMode.NormalTextBox:
                        base.Text = String.Empty;
                        break;

                    case TNumericTextBoxMode.Integer:
                    case TNumericTextBoxMode.LongInteger:
                        FormatValue("0");
                        break;

                    case TNumericTextBoxMode.Decimal:
                    case TNumericTextBoxMode.Currency:
                        FormatValue("0" + FNumberDecimalSeparator + "00");
                        break;
                }
            }
        }

        private bool IsDecimalSeparator(char AKeyChar)
        {
            char[] DecimalSeparatorChars;

            if (FControlMode == TNumericTextBoxMode.Decimal)
            {
                DecimalSeparatorChars = FNumberDecimalSeparator.ToCharArray();
            }
            else
            {
                DecimalSeparatorChars = FCurrencyDecimalSeparator.ToCharArray();
            }

            List <char>DecimalSeparatorCharsList = new List <char>(DecimalSeparatorChars);

            if (DecimalSeparatorCharsList.Contains(AKeyChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void HandlePaste()
        {
            IDataObject clip;

            if (!this.ReadOnly)
            {
//          MessageBox.Show("HandlePaste");

                clip = Clipboard.GetDataObject();

                if (clip != null)
                {
                    // try and paste the contents
                    try
                    {
                        String str = (String)(clip.GetData(DataFormats.Text));
                        String NumberPattern = "0123456789-+%";
                        int maxPatternPos = NumberPattern.Length - 1;

                        if ((this.ControlMode == TNumericTextBoxMode.Decimal) || (this.ControlMode == TNumericTextBoxMode.Currency))
                        {
                            // We can include decimal separators in our pattern
                            if (((this.Text.Contains(FNumberDecimalSeparator) == false) && (this.Text.Contains(FCurrencyDecimalSeparator) == false))
                                || this.SelectedText.Contains(FNumberDecimalSeparator) || this.SelectedText.Contains(FCurrencyDecimalSeparator))
                            {
                                // Include the decimal/currency separators as well
                                NumberPattern += (FNumberDecimalSeparator + FCurrencyDecimalSeparator);
                            }
                        }

                        String OkStr = "";
                        Boolean alreadyPastedDecimalSep = false;

                        for (int i = 0; i < str.Length; i++)
                        {
                            int pos = NumberPattern.IndexOf(str[i]);

                            if (pos > maxPatternPos)
                            {
                                // It is a decimal separator
                                if (alreadyPastedDecimalSep == false)
                                {
                                    OkStr += str[i];
                                    alreadyPastedDecimalSep = true;
                                }
                            }
                            else if (pos >= 0)
                            {
                                OkStr += str[i];
                            }
                        }

                        int intPrevSelStart = this.SelectionStart;
                        int newSelStart = 0;
                        string newText;

                        newText = this.Text.Substring(0, this.SelectionStart) + OkStr + this.Text.Substring(
                            this.SelectionStart + this.SelectedText.Length);
                        newSelStart = intPrevSelStart + OkStr.Length;

                        // We may need to truncate the text if we now exceed the specified number of decimal places
                        if ((ControlMode == TNumericTextBoxMode.Decimal) || (ControlMode == TNumericTextBoxMode.Currency))
                        {
                            // find the position of the decimal separator
                            int decimalPos = newText.IndexOf(FNumberDecimalSeparator);

                            if (decimalPos == -1)
                            {
                                decimalPos = newText.IndexOf(FCurrencyDecimalSeparator);
                            }

                            if (decimalPos >= 0)
                            {
                                if (decimalPos == newText.Length - 1)
                                {
                                    // string ends in a decimal point, so chop it off
                                    newText = newText.Substring(0, newText.Length - 1);
                                }
                                else if (newText.Length - decimalPos - 1 > DecimalPlaces)
                                {
                                    // Too many decimal digits so truncate
                                    newText = newText.Substring(0, decimalPos + DecimalPlaces + 1);
                                }

                                if (newSelStart > newText.Length)
                                {
                                    newSelStart = newText.Length;
                                }
                            }
                        }

                        base.Text = newText;
                        this.SelectionStart = newSelStart;
                        this.SelectionLength = 0;
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show("Exception in TTxtNumericTextBox.HandlePaste: " + Exp.ToString());

                        // never mind
                    }
                }
            }
        }

        private void HandleCut()
        {
            try
            {
                if (this.SelectedText.Length > 0)
                {
                    Clipboard.SetDataObject(this.SelectedText);

                    if (!this.ReadOnly)
                    {
                        if (this.SelectionLength == this.Text.Length)
                        {
                            this.ClearBox();
                        }
                        else
                        {
                            int intPrevSelStart = this.SelectionStart;
                            base.Text = this.Text.Substring(0, this.SelectionStart) + this.Text.Substring(
                                this.SelectionStart + this.SelectedText.Length);
                            this.SelectionStart = intPrevSelStart;
                            this.SelectionLength = 0;
                        }
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show("Exception in Exception in TTxtNumericTextBox.HandleCut: " + Exp.ToString());

                // never mind
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                HandlePaste();
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void FormatValue(string AValue)
        {
            double NumberValueDouble;
            decimal NumberValueDecimal;
            string workText;

            if (AValue != String.Empty)
            {
//MessageBox.Show("FormatValue input string AValue: " + AValue);
                if (((AValue.Length == 1) && (AValue.IndexOfAny(new char[] { '-', '.', ',' }) != -1)))
                {
                    AValue = "0";
                }

                try
                {
                    switch (FControlMode)
                    {
                        case TNumericTextBoxMode.Decimal:
                        case TNumericTextBoxMode.Currency:

                            // When the ControlMode is Decimal we use the number format for decimal point.
                            //   In this situation this control may be a numeric or currency text box because we offer the option
                            //   to display currency amounts in numeric format in non-finance screens
                            // When the ControlMode is Currency we use the currency format for decimal point.
                            //   In this situation this control may be associated with a numeric text box, since we do offer
                            //   the option to display a decimal numeric box in currency format in finance screens

                            workText = String.Empty;

                            if (FNumberPrecision == TNumberPrecision.Double)
                            {
                                if (FIsCurrencyTextBox)
                                {
                                    throw new NotSupportedException("Currency text boxes are expected to be associated with decimal values.");
                                }
                                else
                                {
                                    // Convert to a double value and then format using numeric specifiers
                                    NumberValueDouble = Convert.ToDouble(AValue, FCurrentCulture);

                                    workText = NumberValueDouble.ToString("N" + FDecimalPlaces, FCurrentCulture);
                                }
                            }
                            else if (FNumberPrecision == TNumberPrecision.Decimal)
                            {
                                // Convert to a decimal and then format using numeric specifiers and then adjust for currencies/percents if needed
                                if (decimal.TryParse(AValue, NumberStyles.Any, FCurrentCulture, out NumberValueDecimal) == false)
                                {
                                    throw new ArgumentException(String.Format(Catalog.GetString(
                                                "Could not convert text '{0}' to a decimal number using standard digits and either of the following separator pairs: {1}{2} / {3}{4}"),
                                            CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, FNumberDecimalSeparator,
                                            CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, FCurrencyDecimalSeparator));
                                }

                                workText = NumberValueDecimal.ToString("N" + FDecimalPlaces, FCurrentCulture);

                                if (FControlMode == TNumericTextBoxMode.Currency)
                                {
                                    // Use the currency separator
                                    workText = workText.Replace(FNumberDecimalSeparator, FCurrencyDecimalSeparator);
                                }
                            }

                            if (FIsCurrencyTextBox)
                            {
                                if (FShowCurrencyThousands)
                                {
                                    // use the correct thousands separator
                                    workText = workText.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator,
                                        CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);
                                }
                                else
                                {
                                    // or no separator
                                    workText = workText.Replace(
                                        CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, String.Empty);
                                }
                            }
                            else if (FShowPercentSign)
                            {
                                workText = workText + " %";
                            }

                            base.Text = workText;

                            break;

                        case TNumericTextBoxMode.Integer:
                        case TNumericTextBoxMode.LongInteger:

                            if (FShowPercentSign)
                            {
                                if ((!AValue.EndsWith(" %"))
                                    && !AValue.EndsWith("%"))
                                {
                                    if (FControlMode == TNumericTextBoxMode.Integer)
                                    {
                                        base.Text = System.Int32.Parse(AValue.Trim()) + " %";
                                    }
                                    else
                                    {
                                        base.Text = System.Int64.Parse(AValue.Trim()) + " %";
                                    }
                                }
                                else if ((AValue.EndsWith("%")
                                          && !AValue.EndsWith(" %")))
                                {
                                    if (FControlMode == TNumericTextBoxMode.Integer)
                                    {
                                        base.Text = System.Int32.Parse(AValue.Substring(0, AValue.Length - 1)) + " %";
                                    }
                                    else
                                    {
                                        base.Text = System.Int64.Parse(AValue.Substring(0, AValue.Length - 1)) + " %";
                                    }
                                }
                                else
                                {
                                    if (!AValue.StartsWith("0" + FCurrencyDecimalSeparator) && !AValue.StartsWith("0" + FNumberDecimalSeparator))
                                    {
                                        if (FControlMode == TNumericTextBoxMode.Integer)
                                        {
                                            base.Text = System.Int32.Parse(AValue).ToString() + " %";
                                        }
                                        else
                                        {
                                            base.Text = System.Int64.Parse(AValue).ToString() + " %";
                                        }
                                    }
                                    else
                                    {
                                        base.Text = "0 %";
                                    }
                                }
                            }
                            else
                            {
                                if (!AValue.StartsWith("0" + FCurrencyDecimalSeparator) && !AValue.StartsWith("0" + FNumberDecimalSeparator))
                                {
                                    if (FControlMode == TNumericTextBoxMode.Integer)
                                    {
                                        base.Text = System.Int32.Parse(AValue).ToString();
                                    }
                                    else
                                    {
                                        base.Text = System.Int64.Parse(AValue).ToString();
                                    }
                                }
                                else
                                {
                                    base.Text = "0";
                                }
                            }

                            break;

                        default:
                            // No formatting is done
                            base.Text = AValue;

                            break;
                    }
                }
                catch (System.OverflowException)
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The value entered{0}{1}{2}exceeds the valid value range, i.e. is either too large or too small.{3}Please enter a value that is valid."),
                            Environment.NewLine + Environment.NewLine, "    " + AValue, Environment.NewLine + Environment.NewLine,
                            Environment.NewLine),
                        Catalog.GetString("Value Out of Range"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    base.Text = AValue;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                ClearBox();
            }
        }

        /// <summary>
        /// This method overrides the normal user formatting.  It is used by UserPreferences to show examples of formatting options
        /// </summary>
        public void OverrideNormalFormatting(bool AUseNumberFormatForCurrency, bool AShowThousands)
        {
            FShowCurrencyThousands = AShowThousands;
            FControlMode = AUseNumberFormatForCurrency ? TNumericTextBoxMode.Decimal : TNumericTextBoxMode.Currency;

            FormatValue(RemoveNonNumeralChars());
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLeave(object sender, EventArgs e)
        {
            if (!(FNullValueAllowed && (this.Text == String.Empty)))
            {
                try
                {
                    FormatValue(RemoveNonNumeralChars());
                }
                catch (System.FormatException)
                {
                    MessageBox.Show(Catalog.GetString("The number entered has an invalid number format!"), Catalog.GetString("Invalid Number Format"));
                }
                catch (Exception Exp)
                {
                    MessageBox.Show("Exception in TTxtNumericTextBox.OnLeave: " + Exp.ToString());
                }
            }
        }

        private string RemoveNonNumeralChars()
        {
            string ReturnValue = String.Empty;

            if (((FControlMode == TNumericTextBoxMode.Integer)
                 || (FControlMode == TNumericTextBoxMode.LongInteger)
                 || (FControlMode == TNumericTextBoxMode.Decimal)
                 || (FControlMode == TNumericTextBoxMode.Currency))
                && (this.Text != String.Empty))
            {
                ReturnValue = this.Text.TrimEnd(new char[] { ' ', '%' });
            }
            else
            {
                ReturnValue = this.Text;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEntering(object sender, EventArgs e)
        {
            if (this.ControlMode == TNumericTextBoxMode.NormalTextBox)
            {
                // Special case of a normal text box which is likely to be edited in more than one place in its length
                if (FCurrentUndoString == null)
                {
                    // Initialise the undo string - but only if we are entering for the first time.
                    // Otherwise our Undo needs to use the same buffer that it had when we left.
                    ResetUndo();
                }
            }
            else
            {
                // For numeric style text boxes we always set the undo string to the entire initial content
                ResetUndo();
            }

            this.SelectAll();
        }

        #region Methods relating to Undo

        /// <summary>
        /// Resets the Undo buffer to the current text
        /// </summary>
        private void ResetUndo()
        {
            FCurrentUndoString = this.Text;
        }

        /// <summary>
        /// Replaces the current text with the text in the Undo buffer
        /// </summary>
        private void UndoChanges()
        {
            // Undo replaces the current text with the Undo text and then sets the Undo text to what was the previous text.
            // So the next Undo is a Redo.
            string prevText = this.Text;

            base.Text = FCurrentUndoString;
            FCurrentUndoString = prevText;

            // Now we need to position the cursor and set the selection length to the fragment of text that was different.
            // We can use our friend GetEditRange for this...
            int selStart;
            int selLength;
            GetEditRange(prevText, this.Text, out selStart, out selLength);
            this.SelectionStart = selStart;
            this.SelectionLength = selLength;
        }

        /// <summary>
        /// Gets the current edit range by comparing the difference between the AInitialText and the current text
        /// </summary>
        /// <param name="AInitialText">Text that applied before editing</param>
        /// <param name="AEditedText">Text after editing AInitialText</param>
        /// <param name="AStart">The start location of the current edit</param>
        /// <param name="ALength">The length of the current edit</param>
        private void GetEditRange(string AInitialText, string AEditedText, out int AStart, out int ALength)
        {
            // Set these values that will apply if there is no difference
            AStart = Math.Min(AInitialText.Length, AEditedText.Length);
            ALength = Math.Max(AEditedText.Length - AInitialText.Length, 0);

            // Work from the left of the two strings and find the first difference
            int k = 0;

            for (int i = 0; k < AInitialText.Length && i < AEditedText.Length; i++)
            {
                if (AInitialText[k] != AEditedText[i])
                {
                    AStart = i;
                    break;
                }

                k++;
            }

            // Now work backwards from the last character and find the position of the character that doesn't match.
            // Then we will have the start and end of the non-matching block
            k = 0;

            for (int i = 0; k < AInitialText.Length && i < AEditedText.Length; i++)
            {
                if (AInitialText[AInitialText.Length - k - 1] != AEditedText[AEditedText.Length - i - 1])
                {
                    ALength = AEditedText.Length - i - AStart;
                    break;
                }

                k++;
            }
        }

        #endregion

        #region Delegate for getting User Preferences

        /// <summary>
        /// Declaration of a delegate to retrieve a boolean value from the user defaults
        /// </summary>
        public delegate Boolean TRetrieveUserDefaultBoolean(String AKey, Boolean ADefault);

        /// <summary>
        /// Get/set the function pointer for retrieving a boolean user default
        /// </summary>
        public static TRetrieveUserDefaultBoolean RetrieveUserDefaultBoolean
        {
            get
            {
                return FRetrieveUserDefaultBoolean;
            }

            set
            {
                FRetrieveUserDefaultBoolean = value;
            }
        }

        #endregion
    }
}