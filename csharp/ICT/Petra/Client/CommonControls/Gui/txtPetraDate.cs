//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using GNU.Gettext;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// A Control that derives from System.Windows.Forms.TextBox that allows entry
    /// and formatting of dates in the Petra-specific format and with all the Petra
    /// entry functionality (eg. +, -, =, today). It cannot be used to display
    /// 'normal' text.
    ///
    /// The Control provides internal date parsing and validation. Restrictions can
    /// be set on what the user is allowed to enter - this is governed by the
    /// Properties 'AllowEmptyDate', 'AllowFutureDate' and 'AllowPastDate' (grouped
    /// in the Designer's Object Inspector under 'Date Validation').
    /// The date can also be validated from outside by calling the 'ValidDate'
    /// function.
    ///
    /// </summary>
    public class TtxtPetraDate : System.Windows.Forms.TextBox
    {
        private DateTime? FDate;
        private DateTime? FDateBeforeUserEdit;
        private Boolean FSuppressTextChangeEvent;
        private Boolean FAllowEmpty;             // default is true
        private Boolean FAllowFutureDate;
        private Boolean FAllowPastDate;
        private Boolean FLeavingOnFailedValidationOK;

        /// show no warning for invalid text, handled by the validation framework instead
        private Boolean FShowWarningOnLostFocus = false;
        private String FDateDescription;
        private Boolean FAllowVerification = true;

        private DateTime minimalDateValue;
        private DateTime maximalDateValue;

        /// <summary>
        /// This property determines the Text that the Control should display.
        /// NOTE: Needs to be re-declared here so that we can assign a DefaultValue to it
        /// that is actually a Date!
        ///
        /// </summary>
        public new String Text
        {
            get
            {
                // MessageBox.Show('Entering TtxtPetraDate.Get_Text...');
                return base.Text;
            }

            set
            {
                // MessageBox.Show('Entering TtxtPetraDate.Set_Text...');
                // If this seems to be a change, I may need to raise DateChanged
                // (but I'll not do so unless it really is required...)
                if (value != this.Text)
                {
                    DateTime? HoldThis = FDate;
                    bool DateIsOk = VerifyDate(value, true);

                    if (FDate != HoldThis)
                    {
                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, DateIsOk));
                    }
                }
            }
        }

        /// <summary>
        /// This property determines the Date that the Control should display.
        /// </summary>
        public DateTime ? Date
        {
            get
            {
                if (!ValidDate(false))
                {
                    FDate = null;
                }

                return FDate;
            }

            set
            {
                TPetraDateChangedEventArgs DateChangeArgs;
                string NewDateAsText = string.Empty;

                if (value.HasValue)
                {
                    NewDateAsText = DataBinding.DateTimeToLongDateString2(value.Value);
                }
                else
                {
                    this.Clear();
                }

                try
                {
                    if (NewDateAsText != string.Empty)
                    {
                        // Now update the TextBox's Text with the correctly formatted date
                        if (!FSuppressTextChangeEvent)
                        {
                            // MessageBox.Show('set_Date: calling VerifyDate...');
                            // Verify the Date. If it is OK, the Text will correspond to the 'Petra Date' format.
                            if (VerifyDate(NewDateAsText, true))
                            {
                                DateChangeArgs = new TPetraDateChangedEventArgs(FDate, true);
                            }
                            else
                            {
                                DateChangeArgs = new TPetraDateChangedEventArgs(DateTime.MinValue, false);
                            }

                            // Raise OnDateChanged Event whether the Date was valid or not!
                            OnDateChanged(DateChangeArgs);
                        }
                    }
                    else
                    {
                        FDate = null;

                        // Raise On DateChanged Event whether the Date was valid or not!
                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, true));
                    }
                }
                catch (System.MissingMethodException)
                {
                    // ignore this Exception; it is thrown when the control runs in the Designer
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This property determines the description of the Date. This is used when
        /// showing a verification error.
        ///
        /// </summary>
        public String Description
        {
            get
            {
                return FDateDescription;
            }

            set
            {
                FDateDescription = value;
            }
        }

        /// <summary>
        /// This property determines whether a Date in the future is allowed to be entered. (Default: true)
        /// </summary>
        public Boolean AllowFutureDate
        {
            get
            {
                return FAllowFutureDate;
            }

            set
            {
                FAllowFutureDate = value;
            }
        }

        /// <summary>
        /// This property determines whether a Date in the past is allowed to be entered. (Default: true)
        /// </summary>
        public Boolean AllowPastDate
        {
            get
            {
                return FAllowPastDate;
            }

            set
            {
                FAllowPastDate = value;
            }
        }

        /// <summary>
        /// This property determines whether an empty value will be allowed to be entered. (Default: true)
        /// </summary>
        public Boolean AllowEmpty
        {
            get
            {
                return FAllowEmpty;
            }

            set
            {
                FAllowEmpty = value;
            }
        }

        /// <summary>
        /// This property determines whether verification of date is allowed. (Default: true)
        /// </summary>
        public Boolean AllowVerification
        {
            get
            {
                return FAllowVerification;
            }

            set
            {
                FAllowVerification = value;
            }
        }

        /// <summary>
        /// This property determines whether the user will be allowed to leave the Date
        /// TextBox if it contains an invalid date. (Default: true)
        ///
        /// </summary>
        public Boolean LeavingOnFailedValidationOK
        {
            get
            {
                return FLeavingOnFailedValidationOK;
            }

            set
            {
                FLeavingOnFailedValidationOK = value;
            }
        }

        /// <summary>
        /// This Event is thrown when the Date has changed.
        /// </summary>
        public event TPetraDateChangedEventHandler DateChanged;

        /// <summary>
        /// constructor
        /// </summary>
        public TtxtPetraDate() : base()
        {
            FDate = null;
            FDateDescription = "Date";
            FAllowVerification = true;

            minimalDateValue = DateTime.MinValue;
            maximalDateValue = DateTime.MaxValue;

            this.Width = 94;
            this.Font = new System.Drawing.Font("Verdana", 8.25f, FontStyle.Bold);

//          this.Validating += new CancelEventHandler(this.Date_Validating);
            this.DoubleClick += new EventHandler(TtxtPetraDate_DoubleClick);
            this.GotFocus += new EventHandler(TtxtPetraDate_GotFocus);
            this.LostFocus += new EventHandler(TtxtPetraDate_LostFocus);
            AllowFutureDate = true;
            AllowPastDate = true;
            AllowEmpty = true;
            LeavingOnFailedValidationOK = true;
        }

        void TtxtPetraDate_GotFocus(object sender, EventArgs e)
        {
            FDateBeforeUserEdit = FDate;
        }

        /// <summary>
        /// Selects all text in the TextBox on mouse double-click.
        /// </summary>
        /// <param name="sender">Passed in by WinForms.</param>
        /// <param name="e">Passed in by WinForms.</param>
        void TtxtPetraDate_DoubleClick(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        private void TtxtPetraDate_LostFocus(object sender, EventArgs e)
        {
            bool DateIsOk = VerifyDate(base.Text, FShowWarningOnLostFocus);

            if (FDate != FDateBeforeUserEdit)
            {
                OnDateChanged(new TPetraDateChangedEventArgs(FDate, DateIsOk));
            }
        }

        /// <summary>
        /// Date_TextChanged(sender: System.Object; e: System.EventArgs);
        /// </summary>
        /// <returns>void</returns>
        private void OnDateChanged(TPetraDateChangedEventArgs e)
        {
            if (DateChanged != null)
            {
                DateChanged(this, e);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>void</returns>
        private void Date_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (VerifyDate(base.Text, true))
            {
                e.Cancel = false;
            }
            else
            {
                if (!FLeavingOnFailedValidationOK)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Verifies the Date value.
        /// </summary>
        /// <param name="AShowVerificationError">Set to true to show errors if verification
        /// failed, or to false to suppress error messages</param>
        /// <returns>true if the Control is not empty and has a valid date, otherwise false
        /// </returns>
        public Boolean ValidDate(Boolean AShowVerificationError)
        {
            return VerifyDate(base.Text, AShowVerificationError);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <returns></returns>
        public Boolean ValidDate()
        {
            return ValidDate(true);
        }

        private Int32 CountVerifyDateRunningInstances = 0;

        /// <summary>
        /// Verifies the Date value.
        /// </summary>
        /// <param name="AValueText">the new value of the textbox</param>
        /// <param name="AShowVerificationError">Set to true to show errors if verification
        /// failed, or to false to suppress error messages</param>
        /// <returns>true if the Control has a valid date</returns>
        private Boolean VerifyDate(string AValueText, Boolean AShowVerificationError)
        {
            if (!FAllowVerification)
            {
                return true;
            }

            if (CountVerifyDateRunningInstances > 0)
            {
                // the LostFocus and the DateValidated events overlap, and would display too messageboxes if the text can not be parsed for a date
                return false;
            }

            CountVerifyDateRunningInstances++;

            try
            {
                TVerificationResult DateVerificationResult;
                // Convert TextBox's Text to Date
                DateTime Text2Date = DataBinding.LongDateStringToDateTime2(
                    AValueText,
                    FDateDescription,
                    out DateVerificationResult,
                    AShowVerificationError, null);

                if (DateVerificationResult != null)
                {
                    // Date conversion was NOT successful
                    return false;
                }
                else
                {
                    // Conversion was successful
                    if (!AllowFutureDate)
                    {
                        DateVerificationResult = TDateChecks.IsCurrentOrPastDate(Text2Date, FDateDescription);

                        if (DateVerificationResult != null)
                        {
                            if (AShowVerificationError)
                            {
                                // Show appropriate Error Message to the user
                                TMessages.MsgGeneralError(DateVerificationResult, this.FindForm().GetType());
                            }

                            return false;
                        }
                    }

                    if (!AllowPastDate)
                    {
                        DateVerificationResult = TDateChecks.IsCurrentOrFutureDate(Text2Date, FDateDescription);

                        if (DateVerificationResult != null)
                        {
                            if (AShowVerificationError)
                            {
                                // Show appropriate Error Message to the user
                                TMessages.MsgGeneralError(DateVerificationResult, this.FindForm().GetType());
                            }

                            return false;
                        }
                    }

                    if (!FAllowEmpty)
                    {
                        DateVerificationResult = TDateChecks.IsNotUndefinedDateTime(Text2Date, FDateDescription);

                        if (DateVerificationResult != null)
                        {
                            if (AShowVerificationError)
                            {
                                // Show appropriate Error Message to the user
                                TMessages.MsgGeneralError(DateVerificationResult, this.FindForm().GetType());
                            }

                            return false;
                        }
                    }

                    // Store the Date for later use
                    if (Text2Date != DateTime.MinValue)
                    {
                        FDate = Text2Date;
                    }
                    else
                    {
                        FDate = null;
                    }

                    // set tag to "SuppressChangeDetection" so text change is not detected by TFrmPetraEditUtils.MultiEventHandler
                    object OriginalTag = this.Tag;
                    this.Tag = MCommonResourcestrings.StrCtrlSuppressChangeDetection;
                    FSuppressTextChangeEvent = true;

                    // Now update the TextBox's Text with the newly formatted date
                    if (FDate != null)
                    {
                        if (DateTime.Compare(minimalDateValue, FDate.Value) > 0)
                        {
                            TMessages.DateValueMessageMinUnderrun(minimalDateValue);
                        }

                        if (DateTime.Compare(FDate.Value, maximalDateValue) > 0)
                        {
                            TMessages.DateValueMessageMaxOverrun(maximalDateValue);
                        }

                        String NewText = DataBinding.DateTimeToLongDateString2(FDate.Value);

                        if (this.Text != NewText) // Don't set anything that's unchanged
                        {
                            base.Text = NewText; // I'm not calling my own Text Property, because I don't want to end up back here...
                        }
                    }
                    else
                    {
                        if (this.Text != "")  // Don't set anything that's unchaged
                        {
                            base.Text = "";  // I'm not calling my own Text Property, because I don't want to end up back here...
                        }
                    }

                    // reset tag to original state
                    this.Tag = OriginalTag;
                    FSuppressTextChangeEvent = false;
                    return true;
                }
            }
            finally
            {
                CountVerifyDateRunningInstances--;
            }
        }
    }

    /// <summary>todoComment</summary>
    public delegate void TPetraDateChangedEventHandler(System.Object Sender, TPetraDateChangedEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public class TPetraDateChangedEventArgs : System.EventArgs
    {
        private DateTime? FDate;
        private Boolean FValidDate;

        /// <summary>todoComment</summary>
        public DateTime ? DateTime
        {
            get
            {
                return FDate;
            }

            set
            {
                FDate = value;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean ValidDate
        {
            get
            {
                return FValidDate;
            }

            set
            {
                FValidDate = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPetraDateChangedEventArgs() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADate"></param>
        /// <param name="AValidDate"></param>
        public TPetraDateChangedEventArgs(DateTime? ADate, Boolean AValidDate)
            : base()
        {
            FDate = ADate;
            FValidDate = AValidDate;
        }
    }
}