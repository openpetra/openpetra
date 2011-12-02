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
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.App.Gui;
using GNU.Gettext;

namespace Ict.Petra.Client.CommonControls
{
    #region TtxtPetraDate

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
        private Boolean FSuppressTextChangeEvent;
        private Boolean FAllowEmpty;
        private Boolean FAllowFutureDate;
        private Boolean FAllowPastDate;
        private Boolean FLeavingOnFailedValidationOK;
        private String FDateDescription;

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
                base.Text = value;
            }
        }

        /// <summary>
        /// / Custom Properties
        /// This property determines the Date that the Control should display.
        ///
        /// </summary>
        public DateTime ? Date
        {
            get
            {
                return FDate;
            }

            set
            {
                TPetraDateChangedEventArgs DateChangeArgs;

                // MessageBox.Show('Entering TtxtPetraDate.Set_Date...');
                this.Text = value.ToString();

                // MessageBox.Show('this.Text: ' + this.Text);
                try
                {
                    if (this.Text != "")
                    {
                        // Now update the TextBox's Text with the newly formatted date
                        // VerifyDate;
                        if (!FSuppressTextChangeEvent)
                        {
                            // MessageBox.Show('set_Date: calling VerifyDate...');
                            // Verify the Date. If it is OK, the Text will be changed to correspond to the
                            // 'Petra Date' format.
                            if (VerifyDate())
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

                        // Raise OnDateChanged Event whether the Date was valid or not!
                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, true));
                    }
                }
                catch (System.MissingMethodException)
                {
                }
                // ignore this Exception; it is thrown when the control runs in the Designer
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
            // MessageBox.Show('Entering TtxtPetraDate.Create');
            FDate = DateTime.Today;
            FDateDescription = "Date";
            FLeavingOnFailedValidationOK = true;
            FAllowFutureDate = true;
            FAllowPastDate = true;
            FAllowEmpty = true;

            // this.Text := DateTimeToLongDateString2(FDate);
            this.Date = FDate;

            // MessageBox.Show('TtxtPetraDate.Create: after assigning ''Text'' Property');
            this.Width = 94;
            this.Font = new System.Drawing.Font("Verdana", 8.25f, FontStyle.Bold);

            // MessageBox.Show('TtxtPetraDate.Create: after assigning ''Width'' Property');
            this.Validating += new CancelEventHandler(this.Date_Validating);
            this.DoubleClick += new EventHandler(TtxtPetraDate_DoubleClick);

            // MessageBox.Show('TtxtPetraDate.Create: after hooking up ''Validating'' Event');
            // Include(this.TextChanged, this.Date_TextChanged);
            // MessageBox.Show('TtxtPetraDate.Create: after hooking up ''TextChanged'' Event');
            AllowFutureDate = true;
            AllowPastDate = true;
            AllowEmpty = true;
            LeavingOnFailedValidationOK = true;
        }

        void TtxtPetraDate_DoubleClick(object sender, EventArgs e)
        {
            DialogResult ReplaceExistingDate;

            if (FDate == null)
            {
                this.Date = DateTime.Today;
            }
            else
            {
                ReplaceExistingDate =
                    MessageBox.Show(Catalog.GetString(
                            "The date is not empty. By double-clicking it you would set the date to today's date.\r\n\r\nAre you sure you want to set the date to today's date?"),
                        Catalog.GetString("Replace Date With Today's Date?"), MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (ReplaceExistingDate == DialogResult.Yes)
                {
                    this.Date = DateTime.Today;
                }
            }
        }

        /// <summary>
        /// procedure Date_TextChanged(sender: System.Object; e: System.EventArgs);
        /// </summary>
        /// <returns>void</returns>
        private void OnDateChanged(TPetraDateChangedEventArgs e)
        {
            // MessageBox.Show('Entering TtxtPetraDate.OnDateChanged...');
            if (DateChanged != null)
            {
                DateChanged(this, e);
            }
        }

        #region Event Handlers

        /// <summary>
        /// procedure TtxtPetraDate.Date_TextChanged(sender: System.Object; e: System.EventArgs);var  DateChangeArgs: TPetraDateChangedEventArgs;begin/  if DesignMode then/  begin/    return;/  end;/  if not FSuppressTextChangeEvent then
        /// beginMessageBox.Show('Entering TtxtPetraDate.Date_TextChanged...');     Verify the Date. If it is OK, the Text will be changed to correspond to the     'Petra Date' format.    if VerifyDate then    begin      DateChangeArgs := new
        /// TPetraDateChangedEventArgs.Create(FDate, true);    end    else    begin      DateChangeArgs := TPetraDateChangedEventArgs(DateTime.MinValue, false);    end;/     Raise OnDateChanged Event  whether the Date was valid or not!
        /// OnDateChanged(DateChangeArgs);  end;end;
        /// </summary>
        /// <returns>void</returns>
        private void Date_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            // MessageBox.Show('Entering TtxtPetraDate.Date_Validating...');
            if (VerifyDate())
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

        #endregion

        #region Helper Functions

        /// <summary>
        /// Verifies the Date value.
        /// </summary>
        /// <param name="AShowVerificationError">Set to true to show errors if verification
        /// failed, or to false to suppress error messages</param>
        /// <returns>true if the Control is not empty and has a valid date, otherwise false
        /// </returns>
        public Boolean ValidDate(Boolean AShowVerificationError)
        {
            // MessageBox.Show('Entering TtxtPetraDate.ValidDate...');
            return VerifyDate(AShowVerificationError);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <returns></returns>
        public Boolean ValidDate()
        {
            return ValidDate(true);
        }

        /// <summary>
        /// Verifies the Date value.
        ///
        /// </summary>
        /// <param name="AShowVerificationError">Set to true to show errors if verification
        /// failed, or to false to suppress error messages</param>
        /// <returns>true if the Control is not empty and has a valid date, otherwise false
        /// </returns>
        private Boolean VerifyDate(Boolean AShowVerificationError)
        {
            Boolean ReturnValue = true;
            DateTime? DateBeforeChange = FDate;
            DateTime Text2Date;
            TVerificationResult DateVerificationResult1;
            TVerificationResult DateVerificationResult2;

//MessageBox.Show("About to verify Date." + "\r\n" + "Calling LongDateStringToDateTime2...");
            // Convert TextBox's Text to Date
            Text2Date = DataBinding.LongDateStringToDateTime2(
                this.Text,
                FDateDescription,
                out DateVerificationResult1,
                AShowVerificationError, null);

            if (DateVerificationResult1 == null)
            {
                // Conversion was successful
//MessageBox.Show("Date conversion was successful: " + Text2Date.ToString());
                if (!AllowFutureDate)
                {
                    DateVerificationResult2 = TDateChecks.IsCurrentOrPastDate(Text2Date, FDateDescription);

                    if (DateVerificationResult2 != null)
                    {
                        if (AShowVerificationError)
                        {
                            // Show appropriate Error Message to the user
                            TMessages.MsgGeneralError(DateVerificationResult2, this.FindForm().GetType());
                        }

                        // Reset the Date to what it was before!
                        // this.Date := FDate;

                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, false));

                        return false;
                    }
                }

                if (!AllowPastDate)
                {
                    DateVerificationResult2 = TDateChecks.IsCurrentOrFutureDate(Text2Date, FDateDescription);

                    if (DateVerificationResult2 != null)
                    {
                        if (AShowVerificationError)
                        {
                            // Show appropriate Error Message to the user
                            TMessages.MsgGeneralError(DateVerificationResult2, this.FindForm().GetType());
                        }

                        // Reset the Date to what it was before!
                        // this.Date := FDate;

                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, false));

                        return false;
                    }
                }

                if (!FAllowEmpty)
                {
                    DateVerificationResult2 = TDateChecks.IsNotUndefinedDateTime(Text2Date, FDateDescription);

                    if (DateVerificationResult2 != null)
                    {
                        if (AShowVerificationError)
                        {
                            // Show appropriate Error Message to the user
                            TMessages.MsgGeneralError(DateVerificationResult2, this.FindForm().GetType());
                        }

                        // Reset the Date to what it was before!
                        // this.Date := FDate;

                        OnDateChanged(new TPetraDateChangedEventArgs(FDate, false));

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

                // Now update the TextBox's Text with the newly formatted date
                FSuppressTextChangeEvent = true;

                if (FDate != null)
                {
                    this.Text = DataBinding.DateTimeToLongDateString2(FDate.Value);
                }
                else
                {
                    this.Text = "";
                }

                FSuppressTextChangeEvent = false;
                ReturnValue = true;
            }
            else
            {
//MessageBox.Show("Date conversion was NOT successful!");
                ReturnValue = false;
            }

            if (DateBeforeChange != FDate)
            {
                OnDateChanged(new TPetraDateChangedEventArgs(FDate, true));
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <returns></returns>
        private Boolean VerifyDate()
        {
            return VerifyDate(true);
        }

        #endregion
    }

    #endregion

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


        #region TPetraDateChangedEventArgs

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

        #endregion
    }
}