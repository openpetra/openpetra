//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Ict.Common;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Text;

namespace Ict.Common.Controls
{
    #region TTxtPartnerKeyTextBox

    /// <summary>
    /// The TTxtPartnerKeyTextBox is a TextBox with a label next to it,
    /// and it has a partner find button
    /// </summary>
    public class TTxtPartnerKeyTextBox : TTxtLabelledTextBox
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_DATA_BIND_SHORT_NAME = "Short Name not data bound! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_DATA_BIND_PARTNER_KEY = "Partner Key not data bound! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_SHORT_NAME_STRING = "Short Name has no data! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_PARTNER_KEY_STRING = "Partner Key has no data! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 WM_FONTCHANGE = 0x001d;

        /// <summary>
        /// todoComment
        /// </summary>
        public const String UNIT_SHORT_NAME_FONT = "Verdana";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String UNIT_PARTNER_KEY_FONT = "Courier New";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String UNIT_DEFAULT_PARTNER_KEY_STRING = "0123456789";

        /// <summary> Required designer variable. </summary>
        protected System.Int64 FPartnerKey;
        
        /// <summary>Partner Class.</summary>
        private String FPartnerClass;
        
        private Color? FOriginalPartnerClassColor = null;
        
        /**
         * This property gets or sets the maximum number of characters the user can
         * type or paste into the text box control.
         */
        [Category("Behavior")]
        [RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All)]
        [Browsable(true)]
        [Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
        public new int MaxLength
        {
            get
            {
                return this.txtTextBox.MaxLength;
            }

            set
            {
                this.txtTextBox.MaxLength = value;
            }
        }

        /**
         * This property gets or sets the PartnerKey of the control.
         */
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description(""),
         Browsable(true),
         DefaultValue(80),
         ReadOnly(false)]
        public System.Int64 PartnerKey
        {
            get
            {
                return this.FPartnerKey;
            }

            set
            {
                String mPartnerKeyString;

                this.FPartnerKey = value;

                if (this.FPartnerKey < 0)
                {
                    this.FPartnerKey = 0;
                }

                mPartnerKeyString = System.Convert.ToString(this.FPartnerKey);

                if ((mPartnerKeyString != "") && (this.FLabelString == null))
                {
                    this.txtTextBox.Text = StringHelper.FormatStrToPartnerKeyString(mPartnerKeyString);
                }
                else
                {
                    this.txtTextBox.Text = this.FLabelString;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the PartnerClass of the Control. The only purpose of this Property
        /// is to change the BackColor of the TextBox to <see cref="TCommonControlsHelper.PartnerClassPERSONColour" /> 
        /// if this Property is set to "PERSON".
        /// </summary>
        public string PartnerClass
        {
            get
            {
                return FPartnerClass;
            }
            
            set
            {
                FPartnerClass = value;
             
                TCommonControlsHelper.SetPartnerKeyBackColour(FPartnerClass, txtTextBox, FOriginalPartnerClassColor);
            }
        }

        /**
         * This property gets or sets the width of the TextBox within this control.
         */
        [Category("Layout"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("The width of the TextBox control may be changed here. The" +
             " default value is 80 pixels."),
         Browsable(true),
         DefaultValue(80),
         ReadOnly(false)]
        public new int TextBoxWidth
        {
            get
            {
                return base.TextBoxWidth;
            }

            set
            {
                base.TextBoxWidth = value;
            }
        }

        /**
         * This property gets or sets whether the text in the edit control can be changed or not.
         */
        [Category("Layout"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Controls whether the text in the edit control can be changed or not."),
         Browsable(true),
         ReadOnly(false)]
        public bool ReadOnly
        {
            get
            {
                return this.txtTextBox.ReadOnly;
            }

            set
            {
                this.txtTextBox.ReadOnly = value;
            }
        }

        /**
         * This property gets the DelegateFallbackTextBox value. This value determines
         * whether a delegate function is called after the data binding failed.
         */
        [Category("Data"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Determines whether the delegate should be use if the DataBinding for the TextBox fails, or not."),
         ReadOnly(true),
         Browsable(false),
         DefaultValue(false)]
        public new bool DelegateFallbackTextBox
        {
            get
            {
                return base.DelegateFallbackTextBox;
            }

            set
            {
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            //
            // txtTextBox
            //
            this.txtTextBox.Name = "txtTextBox";

            //
            // lblDescription
            //
            this.lblDescription.Font = new System.Drawing.Font("Verdana", 7.25f, System.Drawing.FontStyle.Bold);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Text = "Markus' World";
            this.lblDescription.Click += new System.EventHandler(this.LblDescription_Click);

            //
            // TtxtPartnerKeyTextBox
            //
            this.LabelText = "Markus' World";
            this.Name = "TtxtPartnerKeyTextBox";
            this.Click += new System.EventHandler(this.TtxtPartnerKeyTextBox_Click);
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TTxtPartnerKeyTextBox()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.SetTextBoxDefaultWidth();
            this.SetDefaultFont();
            this.DelegateFallbackTextBox = false;
            this.txtTextBox.MaxLength = 10;
            this.txtTextBox.KeyPress += new KeyPressEventHandler(this.TxtTextBox_KeyPress);
            this.txtTextBox.KeyUp += new KeyEventHandler(this.TxtTextBox_KeyUp);
            this.txtTextBox.Leave += new System.EventHandler(this.TxtTextBox_Leave);
            this.Paint += new PaintEventHandler(this.TxtPartnerKeyTextBox_Paint);
            this.txtTextBox.TabStop = false;
            this.lblDescription.TabStop = false;
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(bool Disposing)
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

        private void TtxtPartnerKeyTextBox_Click(System.Object sender, System.EventArgs e)
        {
            this.txtTextBox.Focus();
            this.txtTextBox.SelectAll();
        }

        private void LblDescription_Click(System.Object sender, System.EventArgs e)
        {
            this.txtTextBox.Focus();
            this.txtTextBox.SelectAll();
        }

        /// <summary>
        /// get the partner key
        /// </summary>
        protected void ObtainPartnerKey()
        {
            System.Int64 newPartnerKey;
            try
            {
                newPartnerKey = StringHelper.StrToPartnerKey(this.txtTextBox.Text);
            }
            catch (Exception)
            {
                newPartnerKey = 0;
            }
            this.PartnerKey = newPartnerKey;
            this.txtTextBox.Text = StringHelper.PartnerKeyToStr(newPartnerKey);

            if ((this.txtTextBox.Text == "0") || (this.PartnerKey == 0))
            {
                this.txtTextBox.Text = "0000000000";
            }
        }

        /// <summary>
        /// avoid recursion / stack overflow in Mono
        /// </summary>
        bool FSettingDefaultFont = false;

        /// <summary>
        /// This procedure sets the default font for this control.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetDefaultFont()
        {
            if (FSettingDefaultFont)
            {
                return;
            }

            FSettingDefaultFont = true;

            System.Drawing.Font mControlFont = this.Font;
            System.Drawing.Font mTextBoxFont = this.txtTextBox.Font;
            System.Single mFontSize = 9.25F;
            System.Drawing.Font mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);

            if ((!(mControlFont.Equals(mFont))) || (!(mTextBoxFont.Equals(mFont))))
            {
                // MessageBox.Show(mFont.FontFamily.GetName(-1) + " " + mControlFont.FontFamily.GetName(-1) + " " + mTextBoxFont.FontFamily.GetName(-1));
                // on Mono the message box displays: Courier New Lucida Console Microsoft Sans Serif
                this.Font = mFont;
                this.txtTextBox.Font = mFont;
            }

            FSettingDefaultFont = false;
        }

        /// <summary>
        /// This procedure sets the default width of the TextBox within this control.
        /// </summary>
        /// <returns>void</returns>
        protected void SetTextBoxDefaultWidth()
        {
            System.Drawing.Graphics mGraphics;
            System.Drawing.Font mFont;
            System.Drawing.SizeF mSizeF;
            System.Int32 mDefaultWidth;
            mGraphics = CreateGraphics();
            mSizeF = new System.Drawing.SizeF(0, 0);
            mFont = new System.Drawing.Font(UNIT_PARTNER_KEY_FONT, this.Font.Size, System.Drawing.FontStyle.Bold);
            mSizeF = mGraphics.MeasureString(UNIT_DEFAULT_PARTNER_KEY_STRING, mFont);
            mDefaultWidth = System.Convert.ToInt32(mSizeF.Width);
            this.TextBoxWidth = mDefaultWidth;
        }

        #endregion

        #region Events

        /// <summary>
        /// This event gets fired if the font is changed. Here it is used to ensure
        /// that the font of the textbox and the control in a whole are nothing else
        /// than Courier New 9.25 Bold.
        /// </summary>
        /// <param name="e">event arguments.</param>
        /// <returns>void</returns>
        protected override void OnFontChanged(System.EventArgs e)
        {
            this.SetDefaultFont();
        }

        /// <summary>
        /// This event gets fired if the font is changed. Here it is used to ensure
        /// that the font of the textbox and the control in a whole are nothing else
        /// than Courier New 9.25 Bold.
        /// </summary>
        /// <param name="e">event arguments.</param>
        /// <returns>void</returns>
        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            this.txtTextBox.Focus();
            this.txtTextBox.SelectAll();
        }

        /// <summary>
        /// This event gets fired when the control is painted.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments.</param>
        /// <returns>void</returns>
        protected void TxtPartnerKeyTextBox_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.ObtainPartnerKey();
        }

        /// <summary>
        /// This event gets fired in order to validate the input in the textbox.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments.</param>
        /// <returns>void</returns>
        protected void TxtTextBox_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((System.Char.IsDigit(e.KeyChar) == true) || (System.Char.IsControl(e.KeyChar) == true))
            {
                base.OnKeyPress(e);
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// This event gets fired in order to validate the input in the textbox.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments.</param>
        /// <returns>void</returns>
        protected void TxtTextBox_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyUp(e);

            System.Windows.Forms.Form mHostForm;

            if ((e.KeyCode == System.Windows.Forms.Keys.Return) || (e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                mHostForm = this.FindForm();
                mHostForm.SelectNextControl(this, true, false, true, true);
            }
        }

        /// <summary>
        /// This event gets fired if the text in the TextBox is left. If the
        /// PartnerKey is '0' ten zeros get displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The event arguments.</param>
        /// <returns>void</returns>
        protected void TxtTextBox_Leave(System.Object sender, System.EventArgs e)
        {
            this.ObtainPartnerKey();
        }

        #endregion
    }
    #endregion


    #region Exceptions

    /// <summary>
    /// todoComment
    /// </summary>
    public class ENoShortNameString : System.Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ENoShortNameString()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ENoShortNameString(String AMessage)
            : base(TTxtPartnerKeyTextBox.EXCEPTION_SHORT_NAME_STRING + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class ENoPartnerKeyString : System.Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ENoPartnerKeyString()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ENoPartnerKeyString(String AMessage)
            : base(TTxtPartnerKeyTextBox.EXCEPTION_PARTNER_KEY_STRING + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EDataBindShortName : System.ArgumentException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindShortName()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindShortName(String AMessage)
            : base(TTxtPartnerKeyTextBox.EXCEPTION_DATA_BIND_SHORT_NAME + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EDataBindPartnerKey : System.ArgumentException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindPartnerKey()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindPartnerKey(String AMessage)
            : base(TTxtPartnerKeyTextBox.EXCEPTION_DATA_BIND_PARTNER_KEY + AMessage)
        {
        }
    }
    #endregion
}