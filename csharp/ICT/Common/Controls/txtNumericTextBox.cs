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

        /// <summary>
        /// Is it OK to show {null} in this control?
        /// </summary>
        public bool FNullValueAllowed = false;

        private string FNumberDecimalSeparator = ".";
        private string FCurrencyDecimalSeparator = ".";
        private CultureInfo FCurrentCulture;
        private bool FShowPercentSign = false;
        private string FNumberPositiveSign = "+";
        private string FNumberNegativeSign = "-";

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
                    throw new Exception(
                        "to the developer: please use NumberValueDecimal or NumberValueInt or SetCurrencyValue to assign a value to the txtNumericTextBox");
                }
            }
        }

        /// <summary>
        /// set currency values with format
        /// </summary>
        public void SetCurrencyValue(decimal AValue, string ACurrencyFormat)
        {
            base.Text = StringHelper.FormatCurrency(new TVariant(AValue), ACurrencyFormat);
        }

        /// <summary>
        /// This Culture came originally from the thread (ie from the user's locale setup)
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return FCurrentCulture;
            }
        }

        /// <summary>
        /// Determines what input the Control accepts and how it formats it.
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
                            decimal? Ret = null;
                            try
                            {
                                Ret = Convert.ToDecimal(CleanedfromNonNumeralChars, FCurrentCulture);
                            }
                            catch (Exception)
                            {
                            }
                            return Ret;
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

                    if (value != null)
                    {
                        base.Text = ((decimal)value).ToString(FCurrentCulture);
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
                                "The 'NumberValueDecimal' Property must not be set to null if the 'NullValueAllowed' Property is false.");
                        }
                    }

                    FormatValue(RemoveNonNumeralChars());
                }

// Sharp Developer Designer Bug makes Integer mode unusable
//                else
//                {
//                    if (!DesignMode)
//                    {
//                        throw new ApplicationException(
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
                        throw new ApplicationException(
                            "The 'NumberValueDouble' Property can only be set if the 'ControlMode' Property is 'Decimal'!");
                    }
                }
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
                }
                else
                {
                    if (!DesignMode)
                    {
                        throw new ApplicationException(
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
                }
                else
                {
                    if (!DesignMode)
                    {
                        throw new ApplicationException(
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
                                #region Decimal place check

                                if (FDecimalPlaces > 0)
                                {
                                    if ((this.SelectionLength == 0)
                                        && (intSelStart > intActDecPlace))
                                    {
                                        // Decimalplace validation
                                        if (ControlMode == TNumericTextBoxMode.Decimal)
                                        {
                                            if (!ShowPercentSign)
                                            {
                                                bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace) > FDecimalPlaces;
                                            }
                                            else
                                            {
                                                if (this.Text.EndsWith(" %"))
                                                {
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace - 2) > FDecimalPlaces;
                                                }
                                                else if ((this.Text.EndsWith("%")
                                                          || (this.Text.EndsWith(" "))))
                                                {
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace - 1) > FDecimalPlaces;
                                                }
                                                else
                                                {
                                                    // same as if !ShowPercentSign
                                                    bolDecimalplaceInvalid = (this.Text.Length - intActDecPlace) > FDecimalPlaces;
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
                                    && (!this.Text.Contains(FNumberNegativeSign)))
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
                        String NumberPattern = "0123456789-+%" + FNumberDecimalSeparator + FCurrencyDecimalSeparator;
                        String OkStr = "";

                        for (int i = 0; i < str.Length; i++)
                        {
                            if (NumberPattern.IndexOf(str[i]) >= 0)
                            {
                                OkStr += str[i];
                            }
                        }

                        if (this.SelectionLength > 0)
                        {
                            this.SelectedText = OkStr;
//                              ProcessChangedText(this.Text);
                        }
                        else if (this.SelectionStart > 0)
                        {
                            base.Text = this.Text.Substring(0, this.SelectionStart) + OkStr + this.Text.Substring(this.SelectionStart);
                        }
                        else
                        {
                            base.Text = OkStr;
                        }
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
                            this.SelectedText = new String('0', this.SelectedText.Length);
//                            ProcessChangedText(this.Text);
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

            if (AValue != String.Empty)
            {
//MessageBox.Show("FormatValue input string AValue: " + AValue);
                try
                {
                    switch (FControlMode)
                    {
                        case TNumericTextBoxMode.Decimal:
                        case TNumericTextBoxMode.Currency:

                            if (FNumberPrecision == TNumberPrecision.Double)
                            {
                                NumberValueDouble = Convert.ToDouble(AValue, FCurrentCulture);
                                //                CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
                                //                NumberFormatInfo ni = ci.NumberFormat;

                                base.Text = NumberValueDouble.ToString("N" + FDecimalPlaces, FCurrentCulture);
                                //                string strnumformat = d.ToString("c", ni);
                                //                this.Text = strnumformat.Remove(0, 1);
                            }
                            else if (FNumberPrecision == TNumberPrecision.Decimal)
                            {
                                NumberValueDecimal = Convert.ToDecimal(AValue, FCurrentCulture);
                                //                CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
                                //                NumberFormatInfo ni = ci.NumberFormat;

                                base.Text = NumberValueDecimal.ToString("N" + FDecimalPlaces, FCurrentCulture);
                                //                string strnumformat = d.ToString("c", ni);
                                //                this.Text = strnumformat.Remove(0, 1);
                            }

                            if (FShowPercentSign)
                            {
                                base.Text = base.Text + " %";
                            }

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
                                    if (!AValue.StartsWith("0" + FCurrencyDecimalSeparator))
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
                                if (!AValue.StartsWith("0" + FCurrencyDecimalSeparator))
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
                 || (FControlMode == TNumericTextBoxMode.Decimal))
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
            this.SelectAll();
        }
    }
}