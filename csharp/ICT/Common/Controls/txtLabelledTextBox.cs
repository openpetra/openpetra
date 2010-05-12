//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, berndr
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;

namespace Ict.Common.Controls
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate String TDelegateLabelString();

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate String TDelegateTextBoxString();

    /// <summary>
    /// This class provides a labelled TextBox
    ///
    /// Short description:
    /// The TTxtLabelledTextBox is a TextBox with a label next to it. It has the
    /// following custom properties:
    /// - DelegateFallbackLabel:   If this property is set to true the control calls a
    ///                            delegate routine after databinding which could provide
    ///                            a value for the label.
    /// - DelegateFallbackTextBox: If this property is set to true the control calls a
    ///                            delegate routine after databinding which could provide
    ///                            a value for the textbox.
    /// - LabelDelegate:           Here a delegate function is used to provide a string.
    ///                            The host of the txtLabelledTextBox has to provide the
    ///                            delegate.
    /// - LabelText:               Here a text can be entered which the label displays if
    ///                            it is not bound to the textbox.
    /// - ShowLabel:               This controls whether the label is active or not.
    /// - TextBoxReadOnly:         This controls whether the textbox is in ReadOnly mode
    ///                            or not.
    /// - TextBoxDelegate:         Here a delegate function is used to provide a string.
    ///                            The host of the txtLabelledTextBox has to provide the
    ///                            delegate.
    /// - TextBoxWidth:            This sets the width of the textbox.
    /// This control features also 6 different procedures to do the databinding.
    /// </summary>
    public class TTxtLabelledTextBox : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_DATA_BIND_LABEL = "Label not data bound! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_DATA_BIND_TEXTBOX = "TextBox not data bound! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_LABEL_STRING = "Label has no data! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const String EXCEPTION_TEXTBOX_STRING = "TextBox has no data! ";

        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 UNIT_HEIGHT = 22;

        /// <summary>
        /// todoComment
        /// </summary>
        public const String UNIT_LABEL_FONT = "Verdana";

        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 UNIT_LABEL_LEFT_OFFSET = 5;

        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 UNIT_LABEL_TOP_OFFSET = 4;

        /// <summary>
        /// text box in the user control
        /// </summary>
        public System.Windows.Forms.TextBox txtTextBox;

        /// <summary> Required designer variable. </summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary> Clean up any resources being used. </summary>
        protected System.Windows.Forms.Label lblDescription;

        /// <summary>
        /// todoComment
        /// </summary>
        protected bool FShowLabel;

        /// <summary>
        /// todoComment
        /// </summary>
        protected string FLabelString;

        /// <summary>
        /// todoComment
        /// </summary>
        protected bool FDelegateFallbackLabel;

        /// <summary>
        /// todoComment
        /// </summary>
        protected bool FDelegateFallbackTextBox;

        /// <summary>
        /// This property may be used to influence the width of the ComboBox.
        ///
        /// </summary>
        public int TextBoxWidth
        {
            get
            {
                return this.txtTextBox.Size.Width;
            }

            set
            {
                System.Int32 mWidth;
                System.Int32 mControlWidth;
                System.Drawing.Size mTextboxSize = new Size();
                mWidth = value;
                mControlWidth = this.Size.Width;
                mTextboxSize.Height = this.txtTextBox.Height;

                if (mWidth < 70)
                {
                    mWidth = 70;
                }
                else if (mWidth >= mControlWidth)
                {
                    mWidth = mControlWidth;
                }

                mTextboxSize.Width = mWidth;
                this.txtTextBox.Size = mTextboxSize;
                this.SetLabelLocation();
            }
        }

        /// <summary>
        /// the text of the label
        /// </summary>
        public string LabelText
        {
            get
            {
                return this.lblDescription.Text;
            }

            set
            {
                this.lblDescription.Text = value;
            }
        }

        /// <summary>
        /// This property gets or sets the maximum number of characters the user can
        /// type or paste into the text box control.
        ///
        /// </summary>
        public int MaxLength
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

        /// <summary>
        /// should the label be displayed
        /// </summary>
        public bool ShowLabel
        {
            get
            {
                return this.FShowLabel;
            }

            set
            {
                this.FShowLabel = value;
                this.lblDescription.Invalidate();
            }
        }

        /// <summary>
        /// is the text box read only
        /// </summary>
        public bool TextBoxReadOnly
        {
            get
            {
                return this.txtTextBox.ReadOnly;
            }

            set
            {
                if (value == true)
                {
                    // ReadOnly Mode
                    this.txtTextBox.ReadOnly = true;
                    this.txtTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    this.txtTextBox.Location = new System.Drawing.Point(0, 3);
                    this.txtTextBox.Invalidate();
                }
                else
                {
                    // Edit Mode
                    this.txtTextBox.ReadOnly = false;
                    this.txtTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    this.txtTextBox.Location = new System.Drawing.Point(0, 0);
                    this.txtTextBox.Invalidate();
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public event TDelegateLabelString LabelDelegate;

        /// <summary>
        /// todoComment
        /// </summary>
        public event TDelegateTextBoxString TextBoxDelegate;

        /// <summary>
        /// todoComment
        /// </summary>
        public bool DelegateFallbackLabel
        {
            get
            {
                return this.FDelegateFallbackLabel;
            }

            set
            {
                this.FDelegateFallbackLabel = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public bool DelegateFallbackTextBox
        {
            get
            {
                return this.FDelegateFallbackTextBox;
            }

            set
            {
                this.FDelegateFallbackTextBox = value;
            }
        }


        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ALabelDataSource">The source to which the label is to bind to.</param>
        /// <param name="ATextBoxDataSource">The source to which the textbox is to bind to.</param>
        /// <param name="ALabelDataMember">The name of the data member needed for the label to databind.</param>
        /// <param name="ATextBoxDataMember">The name of the data member needed for the textbox to databind.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(System.Object ALabelDataSource,
            System.Object ATextBoxDataSource,
            String ALabelDataMember,
            String ATextBoxDataMember)
        {
            this.PerformDataBindingLabel(ALabelDataSource, ALabelDataMember);
            this.PerformDataBindingTextBox(ATextBoxDataSource, ATextBoxDataMember);
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ALabelDataSource">The source to which the label is to bind to.</param>
        /// <param name="ATextBoxDataSource">The source to which the textbox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the label and the textbox to databind.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(System.Object ALabelDataSource, System.Object ATextBoxDataSource, String ADataMember)
        {
            this.PerformDataBindingLabel(ALabelDataSource, ADataMember);
            this.PerformDataBindingTextBox(ATextBoxDataSource, ADataMember);
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ADataSource">The source to which the label and the textbox is to bind to.</param>
        /// <param name="ALabelDataMember">The name of the data member needed for the label to databind.</param>
        /// <param name="ATextBoxDataMember">The name of the data member needed for the textbox to databind.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(System.Object ADataSource, String ALabelDataMember, String ATextBoxDataMember)
        {
            this.PerformDataBindingLabel(ADataSource, ALabelDataMember);
            this.PerformDataBindingTextBox(ADataSource, ATextBoxDataMember);
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ADataSource">The source to which the label and the textbox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the label and the textbox to databind.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(System.Object ADataSource, String ADataMember)
        {
            this.PerformDataBindingLabel(ADataSource, ADataMember);
            this.PerformDataBindingTextBox(ADataSource, ADataMember);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.lblDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();

            //
            // lblDescription
            //
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Ser" + "if",
                7.75f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (Byte)(0));
            this.lblDescription.Location = new System.Drawing.Point(96, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(128, 22);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDescription.Paint += new PaintEventHandler(this.LblDescription_Paint);

            //
            // TtxtLabelledTextBox
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblDescription);
            this.Font =
                new System.Drawing.Font("Lucida Console", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)(0));
            this.Name = "TtxtLabelledTextBox";
            this.Size = new System.Drawing.Size(232, 22);
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TTxtLabelledTextBox() : base()
        {
            this.FShowLabel = true;
            this.FLabelString = null;
            this.DelegateFallbackLabel = true;
            this.DelegateFallbackTextBox = true;
            this.InitializeTextBox();
            this.InitializeComponent();
            this.Controls.Add(this.txtTextBox);
            this.SetDefaultProperties();
            this.TextBoxReadOnly = true;
            TextBoxWidth = 70;
            ShowLabel = true;
            TextBoxReadOnly = true;
            DelegateFallbackLabel = true;
            DelegateFallbackTextBox = true;
        }

        /// <summary>
        /// This procedure initializes the TextBox of this control. This procedure has
        /// to be called before calling the procedure InitializeComponent.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitializeTextBox()
        {
            this.txtTextBox = new System.Windows.Forms.TextBox();

            //
            // cmbCombobox
            //
            this.txtTextBox.Name = "txtTextBox";
            this.txtTextBox.Size = new System.Drawing.Size(70, 20);
            this.txtTextBox.TabIndex = 0;
            this.txtTextBox.Text = "0123456789";
            this.txtTextBox.Visible = true;
        }

        /// <summary>
        /// This procedure processes the disposing of this class.
        /// </summary>
        /// <param name="Disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
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
        /// This function calculates the area of the label.
        /// </summary>
        /// <returns>The RectangleF of the label.
        /// </returns>
        protected System.Drawing.RectangleF GetLabelRectangleF()
        {
            System.Single mFloatX;
            System.Single mFloatY;
            System.Single mWidth;
            System.Single mHeight;
            System.Drawing.RectangleF mRectangle;
            mFloatX = System.Convert.ToSingle(0);
            mFloatY = System.Convert.ToSingle(0);
            mWidth = System.Convert.ToSingle(this.GetLabelWidth());
            mHeight = System.Convert.ToSingle(UNIT_HEIGHT);
            mRectangle = new System.Drawing.RectangleF(mFloatX, mFloatY, mWidth, mHeight);
            return mRectangle;
        }

        /// <summary>
        /// This function calculates the width of the label.
        /// </summary>
        /// <returns>The width of the label.
        /// </returns>
        protected int GetLabelWidth()
        {
            int ReturnValue;

            System.Int32 mControlWidth;
            System.Int32 mTextBoxWidth;
            mControlWidth = this.Width;
            mTextBoxWidth = this.txtTextBox.Width;
            ReturnValue = mControlWidth - mTextBoxWidth;

            if (ReturnValue < 0)
            {
                ReturnValue = 0;
                MessageBox.Show("Warning: Please increase the width of the user control!");
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function gets the X start coordinate of the location of the label.
        /// </summary>
        /// <returns>The X start coordinate of the location of the label
        /// </returns>
        protected int Get_LabelXStartCoord()
        {
            int ReturnValue;

            System.Int32 mControlWidth;
            System.Int32 mTextBoxWidth;
            mControlWidth = this.Width;
            mTextBoxWidth = this.txtTextBox.Width;
            ReturnValue = mTextBoxWidth + 1;

            if (ReturnValue > mControlWidth)
            {
                ReturnValue = mControlWidth;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure sets the default height for this UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetDefaultHeight()
        {
            System.Drawing.Size mLabelSize = new Size();
            System.Drawing.Size mControlSize = new Size();

            // Set label size
            mLabelSize.Height = UNIT_HEIGHT;
            mLabelSize.Width = this.lblDescription.Size.Width;
            this.lblDescription.Size = mLabelSize;

            // Set control size
            mControlSize.Height = UNIT_HEIGHT;
            mControlSize.Width = this.Size.Width;
            this.Size = mControlSize;
        }

        /// <summary>
        /// This procedure sets the default properties for this UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetDefaultProperties()
        {
            this.SetDefaultHeight();
            this.SetLabelLocation();
            this.Invalidate();
        }

        /// <summary>
        /// This procedure sets the font of the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetLabelFont()
        {
            System.Single mLabelFontSize;
            System.Drawing.Font mLabelFont;
            mLabelFontSize = this.Font.Size - 1;
            mLabelFont = new System.Drawing.Font(UNIT_LABEL_FONT, mLabelFontSize, System.Drawing.FontStyle.Bold);
            this.lblDescription.Font = mLabelFont;
        }

        /// <summary>
        /// This procedure sets the location of the label. It automatically gets the
        /// width of the textbox and then calculates the appropriate location for the
        /// label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetLabelLocation()
        {
            System.Drawing.Size mLabelSize = new Size();
            mLabelSize.Height = UNIT_HEIGHT;
            mLabelSize.Width = this.GetLabelWidth();
            this.lblDescription.Size = mLabelSize;
            this.lblDescription.Location = new System.Drawing.Point(this.Get_LabelXStartCoord(), 0);
        }

        /// <summary>
        /// This procedure writes the width of the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void Set_LabelWidth()
        {
            System.Int32 mTextBoxWidth;
            System.Int32 mTotalWidth;
            System.Int32 mLabelWidth;
            mTextBoxWidth = this.TextBoxWidth;
            mTotalWidth = this.Size.Width;
            mLabelWidth = mTotalWidth - mTextBoxWidth;

            if (mLabelWidth < 0)
            {
                mLabelWidth = 0;
            }

            this.lblDescription.Width = mLabelWidth;
        }

        #endregion

        #region Events
        private void LblDescription_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Font mFont;
            System.Drawing.RectangleF mRectangleF;
            System.Drawing.PointF mPoint;
            System.Single mXCoord;
            System.Single mYCoord;
            System.Drawing.Brush mBrush;
            System.Drawing.StringFormat mStringFormat;

            // Getting all sorts of things for drawing the string
            mFont = this.lblDescription.Font;
            mStringFormat = new System.Drawing.StringFormat();
            mStringFormat.FormatFlags = mStringFormat.FormatFlags | System.Drawing.StringFormatFlags.NoWrap;
            mRectangleF = this.GetLabelRectangleF();
            mXCoord = mRectangleF.Left + System.Convert.ToSingle(UNIT_LABEL_LEFT_OFFSET);
            mYCoord = mRectangleF.Top + System.Convert.ToSingle(UNIT_LABEL_TOP_OFFSET);
            mPoint = new System.Drawing.PointF(mXCoord, mYCoord);
            mBrush = new System.Drawing.SolidBrush(this.lblDescription.ForeColor);

            // Clear background
            e.Graphics.Clear(this.lblDescription.BackColor);

            // Draw String
            if (this.FShowLabel == true)
            {
                e.Graphics.DrawString(this.lblDescription.Text, mFont, mBrush, mPoint, mStringFormat);
            }
        }

        /// <summary>
        /// This event occurs when the control is created.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetLabelFont();
            this.lblDescription.Invalidate();
        }

        /// <summary>
        /// This event ensures that the height of the control cannot be changed.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnResize(System.EventArgs e)
        {
            System.Drawing.Size mTextBoxSize = new Size();
            System.Drawing.Size mLabelSize = new Size();
            System.Drawing.Size mControlSize = new Size();
            System.Int32 mTextBoxWidth;
            System.Int32 mLabelWidth;
            System.Int32 mControlWidth;
            mTextBoxWidth = this.txtTextBox.Size.Width;
            mControlWidth = this.Size.Width;

            if (mControlWidth < mTextBoxWidth)
            {
                mTextBoxWidth = mControlWidth;
                mLabelWidth = 0;
                mTextBoxSize.Height = this.txtTextBox.Size.Height;
                mTextBoxSize.Width = mTextBoxWidth;
                this.txtTextBox.Size = mTextBoxSize;
                mLabelSize.Height = UNIT_HEIGHT;
                mLabelSize.Width = mLabelWidth;
                this.lblDescription.Size = mLabelSize;
                mControlSize.Height = UNIT_HEIGHT;
                mControlSize.Width = mControlWidth;
                this.Size = mControlSize;
            }

            this.SetLabelLocation();
            this.SetDefaultHeight();
        }

        #endregion

        #region DataBinding Routines

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ADataSource">The source to which the label is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the binding.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBindingLabel(System.Object ADataSource, String ADataMember)
        {
            String mExceptionString;

            try
            {
                this.lblDescription.DataBindings.Add("Text", ADataSource, ADataMember);
            }
            catch (Exception E)
            {
                if (this.FDelegateFallbackLabel == true)
                {
                    // Use delegate function as a fallback
                    this.LoadLabelString();
                }
                else
                {
                    // Do not use delegate function as a fallback
                    mExceptionString = "Could not data bind the Label!!! " + E.Message + ';' + "\n" + "\n" + E.StackTrace;
                    throw new EDataBindLabel(mExceptionString);
                }
            }
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the TextBox.
        /// </summary>
        /// <param name="ADataSource">The source to which the TextBox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the binding.
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBindingTextBox(System.Object ADataSource, String ADataMember)
        {
            String mExceptionString;

            try
            {
                this.txtTextBox.DataBindings.Add("Text", ADataSource, ADataMember);
            }
            catch (Exception E)
            {
                if (this.FDelegateFallbackTextBox == true)
                {
                    // Use delegate function as a fallback
                    this.LoadTextBoxString();
                }
                else
                {
                    mExceptionString = "Could not data bind the TextBox!!! " + E.Message + ';' + "\n" + "\n" + E.StackTrace;
                    throw new EDataBindTextBox(mExceptionString);
                }
            }
        }

        /// <summary>
        /// This procedure loads the Label's Text field with a certain value. If the
        /// control is used this method should be used only in the OnCreate event.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void LoadLabelString()
        {
            if (DesignMode == false)
            {
                if (LabelDelegate != null)
                {
                    this.FLabelString = LabelDelegate();
                    this.lblDescription.Text = FLabelString;
                }
                else
                {
                    throw new ENoLabelString(
                        "The " + this.GetType().FullName + " control is not properly initialised yet. " +
                        "The LabelString property should return a string to be displayed by the label.");
                }
            }
        }

        /// <summary>
        /// This procedure loads the TextBox's Text field with a certain value. If the
        /// control is used this method should be used only in the OnCreate event.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void LoadTextBoxString()
        {
            if (DesignMode == false)
            {
                if (TextBoxDelegate != null)
                {
                    this.txtTextBox.Text = TextBoxDelegate();
                }
                else
                {
                    throw new ENoTextBoxString(
                        "The " + this.GetType().FullName + " control is not properly initialised yet. " +
                        "The TextBoxString property should return a string to be displayed by the textbox.");
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class ENoLabelString : System.Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ENoLabelString()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AMessage"></param>
        public ENoLabelString(String AMessage)
            : base(TTxtLabelledTextBox.EXCEPTION_LABEL_STRING + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class ENoTextBoxString : System.Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ENoTextBoxString()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ENoTextBoxString(String AMessage)
            : base(TTxtLabelledTextBox.EXCEPTION_TEXTBOX_STRING + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EDataBindLabel : System.ArgumentException
    {
        #region Exceptions

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindLabel()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindLabel(String AMessage)
            : base(TTxtLabelledTextBox.EXCEPTION_DATA_BIND_LABEL + AMessage)
        {
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class EDataBindTextBox : System.ArgumentException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindTextBox()
            : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindTextBox(String AMessage)
            : base(TTxtLabelledTextBox.EXCEPTION_DATA_BIND_TEXTBOX + AMessage)
        {
        }
    }
    #endregion
}