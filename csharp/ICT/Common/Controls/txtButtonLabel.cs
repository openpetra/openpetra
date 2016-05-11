//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using Ict.Common;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This class is the base class for the txtAutopopulatedButtonLabel. It provides
    /// basic features for a Button, a TextBox and a Label. The Button is used to pull
    /// up another window to which is to provide contents for the TextBox and the
    /// Label.
    ///
    /// Currently there are two different control modes for the txtButtonLabel
    /// class: TwoTableMode, FunctionMode.
    /// TwoTableMode:
    /// This mode is used when the TextBox has a look up table. The databound
    /// table is responsible for the TextBox Text. The look up table denotes the
    /// Label Text.
    ///
    /// FunctionMode:
    /// This mode is used then the Label Text comes directly from a different
    /// function. The developer is responsible for implementing the "SetLabel"
    /// delegate function.
    ///
    /// Important:
    /// In all modes this (txtButtonLabel) class is databound by using this function:
    ///   PerformDataBinding(ADataSource: System.Data.DataView; ADataMember: System.String; AControlMode: TButtonLabelControlMode): System.Boolean;
    ///
    /// Usage:
    /// Please make sure that the following Properties of the txtButtonLabel
    /// class have the following values according the control mode used:
    /// | Property            | TwoTableMode                      | FunctionMode    |
    /// |---------------------|-----------------------------------|-----------------|
    /// | ADataSource         | A DataView                        | A DataView      |
    /// | ATextBoxDataMember  | A Column Name                     | A Column Name   |
    /// | TextBoxLookUpMember | A Column Name of LookUpDataSource | null / ''       |
    /// | LabelLookUpMember   | A Column Name of LookUpDataSource | null / ''       |
    /// | LookUpDataSource    | A DataView                        | null / ''       |
    /// | SetLabel            | null                              | A Function Name |
    /// </summary>
    public class TTxtButtonLabel : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// space between find button and textbox
        /// </summary>
        public const Int32 UNIT_SEPARATOR = 4;

        /// <summary>
        /// space between textbox and label
        /// </summary>
        public const Int32 UNIT_LABEL_SEPARATOR = 6;

        /// <summary>
        /// default height
        /// </summary>
        public const Int32 UNIT_DEFAULT_HEIGHT = 23;

        /// <summary>
        /// x position for button
        /// </summary>
        public const Int32 UNIT_BUTTON_X_START_COORD = 0;

        /// <summary>
        /// y position of textbox
        /// </summary>
        public const Int32 UNIT_TEXTBOX_HEIGHT_OFFSET = 1;

        /// <summary>
        /// height of textbox
        /// </summary>
        public const Int32 UNIT_TEXTBOX_HEIGHT = 20;

        /// <summary>
        /// label y pos
        /// </summary>
        public const Int32 UNIT_LABEL_HEIGHT_OFFSET = 0;

        /// <summary>
        /// label height
        /// </summary>
        public const Int32 UNIT_LABEL_HEIGHT = 20;

        /// <summary>
        /// no data available
        /// </summary>
        public const String UNIT_NO_DATA_MESSAGE = "";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_DATA_BINDING = "Control could not be databound!!!";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_DATA_BIND_LABEL = "Label not data bound! ";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_DATA_BIND_TEXTBOX = "TextBox not data bound! ";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_LABEL_STRING = "Label has no data! ";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_TEXTBOX_STRING = "TextBox has no data! ";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_BUTTON_CLICK = "Data not correct!";

        /// <summary>
        /// error message
        /// </summary>
        public const String EXCEPTION_WRONG_DATATYPE = "DataSource is not of type System.Data.DataTable or System.Data.DataView";

        /// <summary>
        /// the textbox that is used in this user control
        /// </summary>
        public TTxtMaskedTextBox txtTextBox;

        /// <summary>
        /// the label that is used in this user control
        /// </summary>
        public System.Windows.Forms.TextBox lblLabel;

        /// <summary> Required designer variable. </summary>
        protected System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Button for Find Screen
        /// </summary>
        public TbtnVarioText btnFindScreen;

        private Color? FOriginalPartnerClassColor = null;

        /// <summary>
        /// space between find button and text box
        /// </summary>
        protected int FSeparatorWidth;

        /// <summary>
        /// this text box allows this types of keys
        /// </summary>
        protected TKeyValuesEnum FKeyValues;

        /// <summary>
        /// space between textbox and label
        /// </summary>
        protected int FLabelSeparatorWidth;

        /// <summary>
        /// do we want data binding of label to textbox
        /// </summary>
        protected bool FBindLabelToTextBox;

        /// <summary>
        /// should the label be displayed
        /// </summary>
        protected bool FShowLabel = true;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected System.Data.DataView FLookUpDataSource;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected System.Data.DataView FDataBoundView;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected System.Data.DataTable FLookUpDataSourceTable;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected System.Data.DataSet FLookUpDataSourceDataSet;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected String FTextBoxLookUpMember;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected String FLabelLookUpMember;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected bool FTextChangedAlreadyCalled;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected int FTextBoxSelectionStart;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected int FTextBoxSelectionLength;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected TButtonLabelControlMode FControlMode;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected bool FDataBound;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected bool FCaseSensitive;

        /// <summary>
        /// TodoComment
        /// </summary>
        protected bool FAutomaticallyUpdateDataSource;

        /// <summary>
        /// This property determines whether the button resizes to the text length or not.
        ///
        /// </summary>
        public bool AdjustButtonWidth
        {
            get
            {
                return this.btnFindScreen.AdjustWidth;
            }

            set
            {
                this.btnFindScreen.AdjustWidth = value;
                this.RelocateTextBox();
                this.RelocateLabel();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public bool BindLabelToTextBox
        {
            get
            {
                return this.FBindLabelToTextBox;
            }

            set
            {
                this.FBindLabelToTextBox = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Drawing.ContentAlignment ButtonTextAlign
        {
            get
            {
                return this.btnFindScreen.TextAlign;
            }

            set
            {
                this.btnFindScreen.TextAlign = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Data.DataView LookUpDataSource
        {
            get
            {
                return this.FLookUpDataSource;
            }

            set
            {
                if (value != null)
                {
                    this.FLookUpDataSourceTable = value.Table.Copy();

                    // TLogging.Log('FLookUpDataSourceTable.TableName:  ' + this.FLookUpDataSourceTable.TableName, [TLoggingType.ToLogfile]);
                    // TLogging.Log('FLookUpDataSourceTable.ColumnName: ' + this.FLookUpDataSourceTable.Columns[0].ColumnName, [TLoggingType.ToLogfile]);
                    // TLogging.Log('FLookUpDataSourceTable.ColumnName: ' + this.FLookUpDataSourceTable.Columns[1].ColumnName, [TLoggingType.ToLogfile]);
                    this.FLookUpDataSource = this.FLookUpDataSourceTable.DefaultView;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public TButtonLabelControlMode ControlMode
        {
            get
            {
                return this.FControlMode;
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
        /// todoComment
        /// </summary>
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

        /// <summary>
        /// Gets or sets whether the TextBox part of the Control is Enabled.
        /// </summary>
        public bool TextBoxPartEnabled
        {
            get
            {
                return this.txtTextBox.Enabled;
            }

            set
            {
                this.txtTextBox.Enabled = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public String TextBoxLookUpMember
        {
            get
            {
                return this.FTextBoxLookUpMember;
            }

            set
            {
                this.FTextBoxLookUpMember = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public TKeyValuesEnum KeyValues
        {
            get
            {
                return this.FKeyValues;
            }

            set
            {
                this.FKeyValues = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public bool CaseSensitive
        {
            get
            {
                return this.FCaseSensitive;
            }

            set
            {
                this.FCaseSensitive = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public String LabelLookUpMember
        {
            get
            {
                return this.FLabelLookUpMember;
            }

            set
            {
                this.FLabelLookUpMember = value;
            }
        }

        /// <summary>
        /// This property determines whether the label is blank or not.
        ///
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

                if (value == false)
                {
                    this.LabelText = System.DBNull.Value.ToString();
                }

                this.lblLabel.Invalidate();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public string ButtonText
        {
            get
            {
                return this.btnFindScreen.Text;
            }

            set
            {
                this.btnFindScreen.Text = value;
                this.RelocateTextBox();
                this.RelocateLabel();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public int ButtonWidth
        {
            get
            {
                return this.btnFindScreen.Size.Width;
            }

            set
            {
                System.Drawing.Size mSize;
                System.Int32 mButtonHeight;
                System.Int32 mMaxButtonWidth;
                System.Int32 mButtonWidth;
                mMaxButtonWidth = this.Size.Width;

                if (value > mMaxButtonWidth)
                {
                    mButtonWidth = mMaxButtonWidth;
                }
                else
                {
                    mButtonWidth = value;
                }

                mButtonHeight = this.btnFindScreen.Size.Height;
                mSize = new System.Drawing.Size(mButtonWidth, mButtonHeight);
                this.btnFindScreen.Size = mSize;
                this.RelocateTextBox();
                this.RelocateLabel();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public event TDelegateButtonClick ButtonClick;

        /// <summary>
        /// This property sets or gets the Text property of the TextBox in this control.
        ///
        /// </summary>
        public string LabelText
        {
            get
            {
                return this.lblLabel.Text;
            }

            set
            {
                this.lblLabel.Text = value;
            }
        }

        /// <summary>
        /// This property sets or gets the Text property of the TextBox in this control.
        ///
        /// </summary>
        public string TextBoxText
        {
            get
            {
                return this.txtTextBox.Text;
            }

            set
            {
                this.txtTextBox.Text = value;
            }
        }

        /// <summary>
        /// This Property allows the control to automatically update the datasource. You normally DONT want this!
        ///
        /// </summary>
        public bool AutomaticallyUpdateDataSource
        {
            get
            {
                return FAutomaticallyUpdateDataSource;
            }

            set
            {
                FAutomaticallyUpdateDataSource = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public int SeparatorWidth
        {
            get
            {
                return this.FSeparatorWidth;
            }

            set
            {
                System.Int32 mOldSeparatorWidth;
                mOldSeparatorWidth = this.FSeparatorWidth;
                this.FSeparatorWidth = value;

                if (mOldSeparatorWidth != value)
                {
                    this.RelocateTextBox();
                    this.RelocateLabel();
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public int LabelSeparatorWidth
        {
            get
            {
                return this.FLabelSeparatorWidth;
            }

            set
            {
                this.FLabelSeparatorWidth = value;
            }
        }

        /// <summary>
        /// This property sets or gets the width property of the TextBox in this control.
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
                System.Drawing.Size mSize;
                System.Int32 mTextBoxHeight;
                System.Int32 mMaxTextBoxWidth;
                System.Int32 mTextBoxWidth;
                mMaxTextBoxWidth = this.Size.Width - this.btnFindScreen.Size.Width - this.FSeparatorWidth;

                if (value > mMaxTextBoxWidth)
                {
                    mTextBoxWidth = mMaxTextBoxWidth;
                }
                else
                {
                    mTextBoxWidth = value;
                }

                mTextBoxHeight = this.txtTextBox.Size.Height;
                mSize = new System.Drawing.Size(mTextBoxWidth, mTextBoxHeight);
                this.txtTextBox.Size = mSize;
                this.RelocateLabel();
            }
        }

        /// <summary>
        /// Sets the BorderStyle of the underlying TextBox.
        /// </summary>
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this.txtTextBox.BorderStyle;
            }
            set
            {
                this.txtTextBox.BorderStyle = value;
                RelocateLabel();
            }
        }

        /// <summary>
        /// Here the hosting form may provide a routine for setting the label without verification..
        ///
        /// </summary>
        public event TDelegateSetLabel SetLabel;

        /// <summary>
        /// This event will notify the hosting form of any faults while evaluating the content of the TextBox.
        ///
        /// </summary>
        public event TDelegateEvaluationFault EvaluationFault;

        /// <summary>
        /// This event will let the hosing control evaluate the textbox string.
        ///
        /// </summary>
        public event TDelegateEvaluateText EvaluateText;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.btnFindScreen = new Ict.Common.Controls.TbtnVarioText();
            this.SuspendLayout();

            //
            // btnFindScreen
            //
            this.btnFindScreen.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)(0));
            this.btnFindScreen.Location = new System.Drawing.Point(0, 0);
            this.btnFindScreen.Name = "btnFindScreen";
            this.btnFindScreen.Size = new System.Drawing.Size(33, 23);
            this.btnFindScreen.TabIndex = 0;
            this.btnFindScreen.Text = "bla";
            this.btnFindScreen.Click += new System.EventHandler(this.BtnFindScreen_Click);
            this.btnFindScreen.TextChanged += new System.EventHandler(this.BtnFindScreen_TextChanged);

            //
            // TtxtButtonLabel
            //
            this.Controls.Add(this.btnFindScreen);
            this.Name = "TtxtButtonLabel";
            this.Size = new System.Drawing.Size(384, 23);
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TTxtButtonLabel() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            FDataBound = false;
            this.FSeparatorWidth = UNIT_SEPARATOR;
            this.FLabelSeparatorWidth = UNIT_LABEL_SEPARATOR;
            this.FBindLabelToTextBox = true;
            this.FShowLabel = true;
            this.FLookUpDataSourceTable = new System.Data.DataTable();
            this.FLookUpDataSourceDataSet = new System.Data.DataSet("LookUpDataSet");
            this.FDataBoundView = new System.Data.DataView();
            this.FTextChangedAlreadyCalled = false;
            this.FCaseSensitive = false;
            this.FAutomaticallyUpdateDataSource = false;
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            if (!(DesignMode))
            {
            }

            // this.FLogger := new Ict.Common.Logging.TLogging('C:\logbook\2006\200611\TestScreen.log');
            // this.FLogger.Log('Hello from Test Screen!!! We wish you a warm welcome!!');
            this.InitializeTextBox();
            this.InitializeLabel();
        }

        private void BtnFindScreen_TextChanged(System.Object sender, System.EventArgs e)
        {
            // TLogging.Log('TtxtButtonLabel.btnFindScreen_TextChanged     === START ===', [TLoggingType.ToLogfile]);
            // TLogging.Log('TtxtButtonLabel.btnFindScreen_TextChanged     === END   ===', [TLoggingType.ToLogfile]);
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
        /// This procedure initializes the label. The label is faked by a textbox control.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void InitializeLabel()
        {
            System.Int32 mXCoordStartLabel;
            System.Int32 mYCoordStartLabel;
            mXCoordStartLabel = this.GetXCoordStartLabel();

            // TLogging.Log('mXCoordStartLabel: ' + mXCoordStartLabel.ToString, [TLoggingType.ToLogfile]);
            this.lblLabel = new System.Windows.Forms.TextBox();
            this.lblLabel.Font =
                new System.Drawing.Font("Verdana", 7.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)(0));
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.TabIndex = 2;
            this.lblLabel.Text = "lblLabel";
            this.lblLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.lblLabel.Multiline = false;
            this.lblLabel.WordWrap = false;
            this.lblLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblLabel.ReadOnly = true;
            this.lblLabel.Anchor =
                (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left |
                    System.Windows.Forms.AnchorStyles.Right);
            mYCoordStartLabel = this.GetYCoordStartLabel();
            this.lblLabel.Width = (this.Width - mXCoordStartLabel - 1);
            this.lblLabel.TabStop = false;

            // Tag needed to prevent CustomEnablingDisabling of 'customDisabling' this field
            this.lblLabel.Tag = "dontdisable";
            this.lblLabel.Location = new System.Drawing.Point(mXCoordStartLabel, mYCoordStartLabel);
            this.lblLabel.Resize += new System.EventHandler(this.LblLabel_Resize);
            this.Controls.Add(this.lblLabel);
        }

        private void InitializeTextBox()
        {
            System.Int32 mXCoordStartTextBox;
            mXCoordStartTextBox = this.GetXCoordStartTextbox();

            // this.txtTextBox := new System.Windows.Forms.TextBox();
            this.txtTextBox = new TTxtMaskedTextBox();
            this.txtTextBox.Location = new System.Drawing.Point(mXCoordStartTextBox, UNIT_TEXTBOX_HEIGHT_OFFSET);
            this.txtTextBox.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (Byte)(0));
            this.txtTextBox.Name = "txtTextBox";
            this.txtTextBox.TabIndex = 1;
            this.txtTextBox.Text = "txtTextBox";

            // Tag needed to make CustomEnablingDisabling 'customDisabling' this field although it was made invisible by CustomEnablingDisabling
            this.txtTextBox.Tag = "CustomDisableAlthoughInvisible";
            this.Controls.Add(this.txtTextBox);
            this.txtTextBox.Leave += new System.EventHandler(this.TxtTextBox_Leave);
            this.txtTextBox.TextChanged += new System.EventHandler(this.TxtTextBox_TextChanged);
            this.Layout += new LayoutEventHandler(this.OnLayout);
        }

        #endregion

        #region Position Helpers

        /// <summary>
        /// This function gets the start X - coordinate for the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected int GetXCoordStartLabel()
        {
            // TLogging.Log('UNIT_BUTTON_X_START_COORD: ' + System.Convert.ToString(UNIT_BUTTON_X_START_COORD), [TLoggingType.ToLogfile]);
            // TLogging.Log('this.btnFindScreen.Width:  ' + this.btnFindScreen.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.FSeparatorWidth:      ' + this.FSeparatorWidth.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.txtTextBox.Width:     ' + this.txtTextBox.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.FLabelSeparatorWidth: ' + this.FLabelSeparatorWidth.ToString, [TLoggingType.ToLogfile]);
            return UNIT_BUTTON_X_START_COORD + this.btnFindScreen.Width + this.FSeparatorWidth + this.txtTextBox.Width + this.FLabelSeparatorWidth;
        }

        /// <summary>
        /// This function gets the start Y - Coordinate for the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        private int GetYCoordStartLabel()
        {
            System.Drawing.Graphics mGraphics;
            System.Single mLabelHeight;
            System.Int32 mLabelHeightInt;
            System.Int32 mControlHight;
            System.Int32 mOffset;
            double mYCoord;
            mGraphics = this.lblLabel.CreateGraphics();
            mLabelHeight = mGraphics.MeasureString("SampleText", this.lblLabel.Font).Height;
            mGraphics.Dispose();
            mControlHight = this.Size.Height;
            mLabelHeightInt = System.Convert.ToInt32(mLabelHeight);
            mOffset = mControlHight - mLabelHeightInt;

            if (mOffset < 0)
            {
                mOffset = 0;
            }

            mYCoord = mOffset / 2.0;

            // In case the underlying control has a BoderStyle of BorderStyle.None we need to relocate the Label slightly
            if (this.txtTextBox.BorderStyle == BorderStyle.None)
            {
                mYCoord = mYCoord - 3;

                if (mYCoord < 0)
                {
                    mYCoord = 0;
                }
            }

            return System.Convert.ToInt32(mYCoord);
        }

        /// <summary>
        /// This function gets the start X - coordinate for the TextBox.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected int GetXCoordStartTextbox()
        {
            return UNIT_BUTTON_X_START_COORD + this.btnFindScreen.Width + this.FSeparatorWidth;
        }

        /// <summary>
        /// This function relocates the TextBox control.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RelocateLabel()
        {
            System.Int32 mLabelStartX;
            System.Int32 mLabelStartY;
            System.Drawing.Size mLabelSize;
            System.Int32 mLabelWidth;
            System.Drawing.Point mLabelNewPoint;
            mLabelStartX = this.GetXCoordStartLabel();
            mLabelStartY = this.GetYCoordStartLabel();
            mLabelWidth = this.Size.Width - mLabelStartX - 1;
            mLabelSize = new System.Drawing.Size(mLabelWidth, UNIT_DEFAULT_HEIGHT);
            mLabelNewPoint = new System.Drawing.Point(mLabelStartX, mLabelStartY);
            this.lblLabel.Location = mLabelNewPoint;
            this.lblLabel.Size = mLabelSize;
            this.Invalidate(true);
        }

        /// <summary>
        /// This function relocates the TextBox control.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RelocateTextBox()
        {
            System.Int32 mTextBoxStartX;
            System.Drawing.Point mTextBoxNewPoint;
            mTextBoxStartX = this.GetXCoordStartTextbox();
            mTextBoxNewPoint = new System.Drawing.Point(mTextBoxStartX, this.txtTextBox.Location.Y);
            this.txtTextBox.Location = mTextBoxNewPoint;
            this.Invalidate(true);
        }

        /// <summary>
        /// This procedure adjusts the labels width
        ///
        /// </summary>
        /// <returns>void</returns>
        private void LblLabel_Resize(System.Object sender, System.EventArgs e)
        {
            System.Int32 mXCoordStartLabel;
            System.Int32 mControlWidth;
            System.Int32 mControlHeight;

            // TLogging.Log('Start lblLabel_Resize', [TLoggingType.ToLogfile]);
            // TLogging.Log('this.Size.Width:               ' + this.Size.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.btnFindScreen.Size.Width: ' + this.btnFindScreen.Size.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.txtTextBox.Width:         ' + this.txtTextBox.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.lblLabel.Width:           ' + this.lblLabel.Width.ToString, [TLoggingType.ToLogfile]);
            mControlHeight = this.Size.Height;
            mXCoordStartLabel = this.GetXCoordStartLabel();
            mControlWidth = this.Size.Width - mXCoordStartLabel;
            this.lblLabel.Size = new System.Drawing.Size(mControlWidth, mControlHeight);

            // TLogging.Log('Haehae', [TLoggingType.ToLogfile]);
            // TLogging.Log('this.Size.Width:               ' + this.Size.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.btnFindScreen.Size.Width: ' + this.btnFindScreen.Size.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.txtTextBox.Width:         ' + this.txtTextBox.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('this.lblLabel.Width:           ' + this.lblLabel.Width.ToString, [TLoggingType.ToLogfile]);
            // TLogging.Log('End lblLabel_Resize', [TLoggingType.ToLogfile]);
        }

        #endregion

        #region Event handling

        /// <summary>
        /// check for keys being pressed with special functions
        /// </summary>
        /// <param name="AChar"></param>
        /// <returns></returns>
        protected bool HasAControlCharBeenPressed(System.Char AChar)
        {
            bool ReturnValue;

            // TLogging.Log('TtxtButtonLabel.HasAControlCharBeenPressed AChar:' + AChar.ToString, [TLoggingType.ToLogfile]);
            if ((AChar == (char)(3)) || (AChar == (char)(9)) || (AChar == (char)(11))
                || (AChar == (char)(13)) || (AChar == (char)(22))
                || (AChar == (char)(24)) || (AChar == (char)(26)))
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// TodoComment
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.FControlMode = this.CheckControlMode();
        }

        #region Events when leaving

        /// <summary>
        /// leaving the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtTextBox_Leave(System.Object sender, System.EventArgs e)
        {
            this.UpdateLabelText();
        }

        /// <summary>
        /// leaving the user control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(System.EventArgs e)
        {
            this.UpdateLabelText();
            base.OnLeave(e);
        }

        /// <summary>
        /// lost focus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(System.EventArgs e)
        {
            base.OnLostFocus(e);

            // TLogging.Log('TtxtButtonLabel.OnLostFocus', [TLoggingType.ToLogfile]);
        }

        #endregion

        /// <summary>
        /// text has been changed in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtTextBox_TextChanged(System.Object sender, System.EventArgs e)
        {
            // If the text changes, and the control HASN't got focus,
            // then it must be loading new data programatically
            // therefore try and update the label
            if (this.txtTextBox.Focused == false)
            {
                this.UpdateLabelText();
            }
        }

        /// <summary>
        /// layout of user control needs to be rearranged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLayout(System.Object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            // TLogging.Log('TtxtButtonLabel.OnLayout ============= Start', [TLoggingType.ToLogfile]);
            // this.UpdateLabelText();
            // TLogging.Log('TtxtButtonLabel.OnLayout ============= End', [TLoggingType.ToLogfile]);
        }

        /// <summary>
        /// resizing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            System.Drawing.Size mSize;
            System.Int32 mWidth;

            // this.RelocateLabel;
            // TLogging.Log('TtxtButtonLabel.OnResize: ', [TLoggingType.ToLogfile]);
            mWidth = this.Size.Width;

            // TLogging.Log('this.Size.Width: ' + mWidth.ToString, [TLoggingType.ToLogfile]);
            mSize = new System.Drawing.Size(mWidth, UNIT_DEFAULT_HEIGHT);
            this.Size = mSize;

            // TLogging.Log('TtxtButtonLabel.OnResize: Size' + this.Size.ToString, [TLoggingType.ToLogfile]);
        }

        /// <summary>
        /// Find Button has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnFindScreen_Click(System.Object sender, System.EventArgs e)
        {
            System.String mLabelStringOld;
            System.String mTextBoxStringOld;
            System.String mLabelStringNew;
            System.String mPartnerClass;
            System.String mTextBoxStringNew;
            System.String mExceptionString;

            // TLogging.Log('btnFindScreen_Click Start', [TLoggingType.ToLogfile]);
            if (DesignMode == false)
            {
                // TLogging.Log('DesignMode = false');
                if (ButtonClick != null)
                {
                    // TLogging.Log('Assigned: ButtonClick', [TLoggingType.ToLogfile]);
                    try
                    {
                        mLabelStringOld = this.lblLabel.Text;
                        mTextBoxStringOld = this.txtTextBox.Text;
                        ButtonClick(mLabelStringOld, mTextBoxStringOld, out mLabelStringNew, out mPartnerClass, out mTextBoxStringNew);

                        // TLogging.Log('New LabelString: >' + mLabelStringNew + '<', [TLoggingType.ToLogfile]);
                        // TLogging.Log('New TextBoxString: >' + mTextBoxStringNew + '<', [TLoggingType.ToLogfile]);
                        if (mTextBoxStringNew != "")
                        {
                            // new value returned from lookup
                            this.txtTextBox.Text = mTextBoxStringNew;

                            TCommonControlsHelper.SetPartnerKeyBackColour(mPartnerClass, this.txtTextBox, FOriginalPartnerClassColor);

                            // update the text
                            // if new label text is NOT returned, do a DB lookup to find it
                            if ((mLabelStringNew == null) || (mLabelStringNew == ""))
                            {
                                this.UpdateLabelText();
                            }
                            else
                            {
                                // new label text IS returned, use it
                                this.lblLabel.Text = mLabelStringNew;
                            }

                            this.txtTextBox.Focus();
                            this.txtTextBox.Modified = true;
                        }
                        else
                        {
                        }

                        // TLogging.Log('Delegate returned empty String', [TLoggingType.ToLogfile]);
                    }
                    catch (Exception E)
                    {
                        mExceptionString = E.ToString();

                        // TLogging.Log('Exception: ' + E.ToString, [TLoggingType.ToLogfile]);
                        throw new EButtonClick(mExceptionString);
                    }
                }
                else
                {
                }

                // TLogging.Log('NOT Assigned: ButtonClick', [TLoggingType.ToLogfile]);
            }

            // if (DesignMode = false) then
        }

        #endregion

        // Event handling

        #region DataBinding

        /// <summary>
        /// This procedure updates the text of the label within this control. This
        /// procedure is called, when the text of the textbox changes
        /// }{***************************************************************************
        /// This function gets a DataRow from a LookUpTable in which the search string is
        /// found. This function is needed in order to provide the control with the right
        /// column to look up the typed in value in the textbox.
        /// </summary>
        /// <param name="ALookUpMember">The column of the LookUptable where the string should be found.</param>
        /// <param name="ASearchString">The search string;</param>
        /// <param name="ACaseSensitive">Determines whether the search is case sensitive;
        /// </param>
        /// <returns>void</returns>
        public System.Data.DataRow GetLookUpRow(String ALookUpMember, String ASearchString, bool ACaseSensitive)
        {
            System.Data.DataRow ReturnValue;
            DataRow[] mFoundRows = null;

            String mSelectStr;
            String mSearchText;
            String mSortString;
            int mCount;

            // Initialization
            // TLogging.Log('TtxtButtonLabel.GetLookUpRow ================ Start', [TLoggingType.ToLogfile]);
            // TLogging.Log('  ALookUpMember: ' + ALookUpMember,  [TLoggingType.ToLogfile]);
            // TLogging.Log('  ASearchString: ' + ASearchString,  [TLoggingType.ToLogfile]);
            ReturnValue = null;
            mSearchText = ASearchString;

            // mFoundIndex := 1;
            mCount = -1;

            // Check whether LookUpDataSet exists.
            if (this.FLookUpDataSourceDataSet == null)
            {
                // TLogging.Log('  No LookUp DataSet present!!!', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            // Check whether the LookUpTable exists.
            if (this.FLookUpDataSourceTable == null)
            {
                // TLogging.Log('  No LookUp DataTabel present!!!', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            if (this.FLookUpDataSourceTable.Columns.Contains(ALookUpMember) == false)
            {
                // TLogging.Log('  Number of Columns: ' + this.FLookUpDataSourceTable.Columns.Count.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('  Name of Columns: ' + this.FLookUpDataSourceTable.Columns[0].ColumnName, [TLoggingType.ToLogfile]);
                // TLogging.Log('  Name of Columns: ' + this.FLookUpDataSourceTable.Columns[1].ColumnName, [TLoggingType.ToLogfile]);
                // TLogging.Log('  ALookUpMember    ' + ALookUpMember + ' is not contained in the LookUpTable!!!', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            if ((this.FTextBoxLookUpMember == null) || (this.FTextBoxLookUpMember == ""))
            {
                MessageBox.Show("Dear programmer, The following variable this.FTextBoxLookUpMember is empty!!!");

                // TLogging.Log('  this.FTextBoxLookUpMember is empty!!!', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            // Set case sensitivity to the desired sensitivity
            this.FLookUpDataSourceDataSet.CaseSensitive = ACaseSensitive;

            // Get the sort string
            mSortString = this.FLookUpDataSource.Sort;

            // Set our sort string
            this.FLookUpDataSource.Sort = ALookUpMember;

            // Assemble select string
            mSelectStr = this.FTextBoxLookUpMember + " = '" + mSearchText + "'";

            // assign select string
            try
            {
                mFoundRows = this.FLookUpDataSourceTable.Select(mSelectStr);
                mCount = mFoundRows.Length;

                // TLogging.Log('  Number of found rows: ' + mCount.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('SELECT: ' + mSelectStr, [TLoggingType.ToLogfile]);
            }
            catch (Exception)
            {
                // TLogging.Log('Exception:      ' + E.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('LookUpDataView: ' + this.FLookUpDataSource.Count.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('Sort String:    ' + this.FLookUpDataSource.Get_Sort(), [TLoggingType.ToLogfile]);
                // TLogging.Log('Select String:  ' + mSelectStr, [TLoggingType.ToLogfile]);
                // TLogging.Log('TextBoxLookUp:  ' + ALookUpMember, [TLoggingType.ToLogfile]);
                this.FLookUpDataSource.Sort = mSortString;
            }

            // Reset sort string
            this.FLookUpDataSource.Sort = mSortString;

            // Process findings
            if (mCount > 0)
            {
                ReturnValue = (mFoundRows[0]);
            }

            if (mCount <= 0)
            {
            }

            // TLogging.Log('TextBoxLookUp:  ' + ALookUpMember, [TLoggingType.ToLogfile]);

            /*
             * 1. Initialization
             * 1. Set Result := nil;
             * 2. Set Count variable to -1;
             * 2. Check whether the LookUpDataSet exists if not leave with result "nil"
             * 3. Check whether the LookUpDataTable exists if not leave with result "nil"
             * 4. Set DataSet.CaseSensitive := ACaseSensitive;
             * 5. Get SortString of the LookUpTable
             * 6. Set our Sort string;
             * 7. Assemble Select String;
             * 8. Get Result set of Select statement
             * 9  Get the first DataRow of the result set and crown it as the result.
             * 10. Reset the sort string to its former value.
             * 11.
             */

            // TLogging.Log('TtxtButtonLabel.GetLookUpRow ================ End ', [TLoggingType.ToLogfile]);
            return ReturnValue;
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the Label.
        /// </summary>
        /// <param name="ADataSource">The source to which the label and the textbox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the control to databind.</param>
        /// <param name="AControlMode">The name of the mode the control is currently in.
        /// </param>
        /// <returns>void</returns>
        public bool PerformDataBinding(System.Data.DataView ADataSource, String ADataMember, TButtonLabelControlMode AControlMode)
        {
            bool ReturnValue;

            if (AControlMode == TButtonLabelControlMode.TwoTableMode)
            {
                ReturnValue = this.PerformDataBindingTwoTableMode(ADataSource, ADataMember);
                this.UpdateLabelText();
            }
            else if (AControlMode == TButtonLabelControlMode.FunctionMode)
            {
                ReturnValue = this.PerformDataBindingFunctionMode(ADataSource, ADataMember);

                // TLogging.Log('  Perform Databinding: TextBox databound. Before updating the label', [TLoggingType.ToLogfile]);
                this.UpdateLabelText();
            }
            else
            {
                ReturnValue = false;
            }

            if (ReturnValue == true)
            {
                this.FDataBound = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the TextBox for the TwoTableMode.
        /// </summary>
        /// <param name="ADataSource">The source to which the TextBox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the binding.
        /// </param>
        /// <returns>void</returns>
        protected bool PerformDataBindingTwoTableMode(System.Data.DataView ADataSource, String ADataMember)
        {
            bool ReturnValue;
            String mExceptionString;
            String mErrorString;

            // Check Requirements for TwoTableMode
            ReturnValue = false;

            if (this.CheckTwoTableModeRequirements(out mErrorString) == false)
            {
                mExceptionString = "Could not data bind the TextBox and the Label!!! " + "\n" + mErrorString + "\n" + "Success: " +
                                   ReturnValue.ToString() + ';';
                throw new EDataBindingFailure(mExceptionString);
            }

            try
            {
                // Databind Textbox
                this.txtTextBox.DataBindings.Add("Text", ADataSource, ADataMember);

                // < DataBinding
                // Report databinding success
                ReturnValue = true;
            }
            catch (Exception E)
            {
                // Handle Exception
                // TLogging.Log('TextBox could not be databound. ' + E.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('ADataSource: ' + ADataSource.GetType().ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('ADataMember: ' + ADataMember, [TLoggingType.ToLogfile]);
                mExceptionString = "Could not data bind the TextBox and the Label!!! " + E.Message + ';' + "\n" + "\n" + E.StackTrace + "\n" +
                                   "Success: " + ReturnValue.ToString() + ';';
                throw new EDataBindingFailure(mExceptionString);
            }
            return ReturnValue;
        }

        /// <summary>
        /// This procedure databinds a DataSource, which may be either a DataTable or
        /// a DataView to the TextBox for the FunctionMode.
        /// </summary>
        /// <param name="ADataSource">The source to which the TextBox is to bind to.</param>
        /// <param name="ADataMember">The name of the data member needed for the binding.
        /// </param>
        /// <returns>void</returns>
        protected bool PerformDataBindingFunctionMode(System.Data.DataView ADataSource, String ADataMember)
        {
            bool ReturnValue;
            String mExceptionString;
            String mErrorString;

            // Check Requirements for FunctionMode
            ReturnValue = false;

            if (this.CheckFunctionModeRequirements(out mErrorString) == false)
            {
                mExceptionString = "Could not data bind the TextBox and the Label!!! " + "\n" + mErrorString + "\n" + "Success: " +
                                   ReturnValue.ToString() + ';';
                throw new EDataBindingFailure(mExceptionString);
            }

            try
            {
                // Databind Textbox
                this.txtTextBox.DataBindings.Add("Text", ADataSource, ADataMember);

                // < DataBinding
                // Report databinding success
                ReturnValue = true;
            }
            catch (Exception E)
            {
                // Handle Exception
                // TLogging.Log('TextBox could not be databound. ' + E.ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('ADataSource: ' + ADataSource.GetType().ToString, [TLoggingType.ToLogfile]);
                // TLogging.Log('ADataMember: ' + ADataMember, [TLoggingType.ToLogfile]);
                mExceptionString = "Could not data bind the TextBox and the Label!!! " + E.Message + ";" + "\n" + "\n" + E.StackTrace + "\n" +
                                   "Success: " + ReturnValue.ToString() + ';';
                throw new EDataBindingFailure(mExceptionString);
            }
            return ReturnValue;
        }

        /// <summary>
        /// update the label
        /// </summary>
        public void UpdateLabelText()
        {
            string mPartnerClass = String.Empty;

            // Initialization
            // TLogging.Log('TtxtButtonLabel.UpdateLabelText: Start', [TLoggingType.ToLogfile]);
            // TLogging.LogStackTrace([TLoggingType.ToLogfile]);
            #region Is control databound?

            if (this.FDataBound == false)
            {
                // TLogging.Log('Control is NOT databound!!!', [TLoggingType.ToLogfile]);
                return;
            }

            #endregion
            #region Update Label for TwoTableMode

            if (this.CheckControlMode() == TButtonLabelControlMode.TwoTableMode)
            {
                // TLogging.Log('Control is in TButtonLabelControlMode.TwoTableMode!!!', [TLoggingType.ToLogfile]);
                this.UpdateLabelTextFromLookUpTable();

                // TLogging.Log('Errorstring of UpdateRoutine: ' + mErrorString, [TLoggingType.ToLogfile]);
            }

            #endregion
            else if (this.CheckControlMode() == TButtonLabelControlMode.FunctionMode)
            {
                if (this.SetLabel != null)
                {
                    String mFoundText = this.lblLabel.Text;

                    // Get text from hosting control
                    // TLogging.Log('TtxtButtonLabel.UpdateLabelText: Text of control: ' + this.txtTextBox.Text);
                    SetLabel(this.txtTextBox.Text, ref mFoundText, ref mPartnerClass);

                    // TLogging.Log('TtxtButtonLabel.UpdateLabelText: Text from function: >' + mFoundText + '<', [TLoggingType.ToLogfile]);
                    // Set text
                    this.lblLabel.Text = mFoundText;
                }
            }
            #endregion
            else
            #region Update for ControlMode None
            {
            }

            // TLogging.Log('Control is not in a defined mode!!!', [TLoggingType.ToLogfile]);
            if (this.AutomaticallyUpdateDataSource == true)
            {
                if ((this.txtTextBox.DataBindings.Count == 1) && (this.txtTextBox.DataBindings[0].BindingManagerBase != null))
                {
                    this.txtTextBox.DataBindings[0].BindingManagerBase.EndCurrentEdit();
                }
            }

            #endregion
        }

        /// <summary>
        /// This procedure updates the text of the label within this control using
        /// a LookUpTable. If the text could be changed successful the return value
        /// is true, otherwise it is false. In the latter case the ErrorString will
        /// give more information about any causes.
        ///
        /// </summary>
        /// <returns>void</returns>
        public bool UpdateLabelTextFromLookUpTable()
        {
            bool ReturnValue;
            String mErrorText;
            bool mRequirementsOK;
            String mTextBoxText;
            String mLabelDisplayString;

            System.Data.DataRow mDataRow;
            #region Initialisation

            // TLogging.Log('TtxtButtonLabel.UpdateLabelTextFromLookUpTable: Start', [TLoggingType.ToLogfile]);
            mErrorText = "";
            mTextBoxText = "";
            mDataRow = null;
            ReturnValue = false;
            #endregion
            #region Requirements check
            mRequirementsOK = this.CheckTwoTableModeRequirements(out mErrorText);

            if (mRequirementsOK == false)
            {
                mErrorText = "The TwoTableMode Requirements were not met!! " + mErrorText;
                this.CallDelegateEvaluationFault(this.txtTextBox.Text, mErrorText, ReturnValue);
                return false;
            }

            #endregion

            // Get TextBox text
            mTextBoxText = this.txtTextBox.Text;

            // Switch off the "txtTextBox.TextChanged" event
            this.FTextChangedAlreadyCalled = true;

            // Search the text from the TextBox in the LookUpTable
            mDataRow = GetLookUpRow(this.TextBoxLookUpMember, mTextBoxText, false);

            // Check DataRow
            if (mDataRow == null)
            {
                mErrorText = "LookUp value not found in LookUpTable!!!";

                // TLogging.Log('  Error: No DataRow!!!!', [TLoggingType.ToLogfile]);
                this.lblLabel.Text = "";
            }
            else
            {
                // Get the label string
                mLabelDisplayString = mDataRow[this.LabelLookUpMember].ToString();
                ReturnValue = true;
                mErrorText = "";

                // Update label
                this.lblLabel.Text = mLabelDisplayString;
            }

            // Report success (Result = true) / failure (Result = false)
            this.CallDelegateEvaluationFault(this.txtTextBox.Text, mErrorText, ReturnValue);

            // Switch on the "txtTextBox.TextChanged" event
            this.FTextChangedAlreadyCalled = false;

            // TLogging.Log('TtxtButtonLabel.UpdateLabelTextFromLookUpTable: End', [TLoggingType.ToLogfile]);
            return ReturnValue;
        }

        /// <summary>
        /// This function verifies that the value entered in the TextBox is actually
        /// in the LookUpTable.
        /// </summary>
        /// <param name="ALookUpMember">The column of the LookUptable where the string should be found.</param>
        /// <param name="ASearchString">The search string;</param>
        /// <param name="ACaseSensitive">If set to true it searches casesensitive = less hits
        /// </param>
        /// <returns>void</returns>
        public string VerifyLookUpValue(string ALookUpMember, string ASearchString, Boolean ACaseSensitive)
        {
            string ReturnValue;

            DataRow[] mFoundRows = null;

            String mSelectStr;
            int mCount;
            System.Data.DataRow mRow;

            // Initialization
            // TLogging.Log('TtxtButtonLabel.VerifyLookUpValue ================ Start', [TLoggingType.ToLogfile]);
            // TLogging.Log('  ALookUpMember:  ' + ALookUpMember,  [TLoggingType.ToLogfile]);
            // TLogging.Log('  ASearchString:  ' + ASearchString,  [TLoggingType.ToLogfile]);
            // TLogging.Log('  ACaseSensitive: ' + ACaseSensitive.ToString, [TLoggingType.ToLogfile]);
            mRow = null;

            if (this.FLookUpDataSourceTable.DataSet == null)
            {
                this.FLookUpDataSourceDataSet.Tables.Add(this.FLookUpDataSourceTable);

                if (this.FLookUpDataSourceTable.DataSet == null)
                {
                }

                // TLogging.Log('  DataSet still nil!!!', [TLoggingType.ToLogfile]);
            }

            this.FLookUpDataSourceTable.DataSet.CaseSensitive = ACaseSensitive;

            // TLogging.Log('  DataSet CaseSensitive?: ' + this.FLookUpDataSourceTable.DataSet.CaseSensitive.ToString, [TLoggingType.ToLogfile]);
            ReturnValue = "";

            // Check of control mode
            if (this.CheckControlMode() != TButtonLabelControlMode.TwoTableMode)
            {
                // TLogging.Log('  Incorect mode', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            // Check whether the column is in the look up table
            if (this.FLookUpDataSourceTable.Columns.Contains(ALookUpMember) == false)
            {
                // TLogging.Log('  Column not in Table mode', [TLoggingType.ToLogfile]);
                return ReturnValue;
            }

            // Assemble filter expression
            mSelectStr = ALookUpMember + " = '" + ASearchString + "'";

            // TLogging.Log('  mSelectStr: ' + mSelectStr, [TLoggingType.ToLogfile]);
            // Get row
            mFoundRows = this.FLookUpDataSourceTable.Select(mSelectStr);
            mCount = mFoundRows.Length;

            // TLogging.Log('  mCount: ' + mCount.ToString, [TLoggingType.ToLogfile]);
            if (mCount < 1)
            {
                // TLogging.Log('  No Data!!!', [TLoggingType.ToLogfile]);
            }
            else
            {
                mRow = mFoundRows[0];
                ReturnValue = mRow[ALookUpMember].ToString();
            }

            // TLogging.Log('  Result: >' + Result +'<', [TLoggingType.ToLogfile]);
            // TLogging.Log('TtxtButtonLabel.VerifyLookUpValue ================ End ', [TLoggingType.ToLogfile]);
            return ReturnValue;
        }

        /// <summary>
        /// This procedure calls the DelegateEvaluateText. This function is needed in
        /// the function mode of the control.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void CallDelegateEvaluateText(String ATextBoxText, out String ALabelString, out bool ATextValid)
        {
            ALabelString = "";
            ATextValid = false;

            if (this.EvaluateText != null)
            {
                this.EvaluateText(ATextBoxText, out ALabelString, out ATextValid);
            }
        }

        /// <summary>
        /// This procedure calls the DelegateEvaluationFault. This way the hosting control
        /// gets informed when the textbox contains a string which is not valid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void CallDelegateEvaluationFault(String ATextBoxText, String AErrorMessage, bool AEvaluationResult)
        {
            if (this.EvaluationFault != null)
            {
                this.EvaluationFault(ATextBoxText, AErrorMessage, AEvaluationResult);
            }
        }

        #region Control Mode Checks

        /// <summary>
        /// This procedure checks in which DataBinding mode the control is. In sum
        /// there are three different databinding modes:
        /// - TwoTableMode
        /// - FunctionMode
        /// The function returns the appropriate mode. Below the criteria is listed
        /// with which
        ///
        /// Properties / Modes | TwoTableMode  | FunctionMode
        /// ------------------------------------------------------
        /// ADataSource        | A DataView    | A DataView      |
        /// ADataMember        | A column name | A column name   |
        /// TextBoxDataMember  | A column name | nil             |
        /// LabelDataMember    | A column name | nil             |
        /// ALookUpTable       | A DataView    | nil             |
        /// ADelegateFunction  | nil           | A function name |
        ///
        /// </summary>
        /// <returns>void</returns>
        public TButtonLabelControlMode CheckControlMode()
        {
            TButtonLabelControlMode ReturnValue;
            bool mTextBoxDataMemberExists;
            bool mLabelDataMemberExists;
            bool mLookUpTableExists;
            bool mDelegateFunctionExists;

            mTextBoxDataMemberExists = false;
            mLabelDataMemberExists = false;
            mLookUpTableExists = false;
            mDelegateFunctionExists = false;
            #region Configuration Check

            /*
             * Properties / Modes   | TwoTableMode  | FunctionMode
             * --------------------------------------------------------
             * ADataSource          | A DataView    | A DataView      |
             * ADataMember          | A column name | A column name   |
             * TextBoxLookUpMember  | A column name | nil             |
             * LabelDataMember      | A column name | nil             |
             * ALookUpTable         | A DataView    | nil             |
             * ADelegateFunction    | nil           | A function name |
             */

            // Check for TextBoxDataMember
            if ((this.TextBoxLookUpMember != "") || (this.TextBoxLookUpMember != null))
            {
                mTextBoxDataMemberExists = true;
            }

            // Check for LabelDataMemberExists
            if ((this.LabelLookUpMember != "") || (this.LabelLookUpMember != null))
            {
                mLabelDataMemberExists = true;
            }

            // Check for LookUpTableExists
            if (this.LookUpDataSource != null)
            {
                mLookUpTableExists = true;
            }

            // Check for DelegateFunctionExists
            if (this.SetLabel != null)
            {
                mDelegateFunctionExists = true;
            }

            #endregion
            #region Figure mode out

            if ((mTextBoxDataMemberExists == true) && (mLabelDataMemberExists == true) && (mLookUpTableExists == true)
                && (mDelegateFunctionExists == false))
            {
                ReturnValue = TButtonLabelControlMode.TwoTableMode;
            }
            else if ((mTextBoxDataMemberExists == false) && (mLabelDataMemberExists == false) && (mLookUpTableExists == false)
                     && (mDelegateFunctionExists == true))
            {
                ReturnValue = TButtonLabelControlMode.FunctionMode;
            }
            else
            {
                ReturnValue = TButtonLabelControlMode.None;
            }

            #endregion

//            if ((ReturnValue == TButtonLabelControlMode.None) && (DesignMode))
//            {
//
//                string mResultString = "mTextBoxDataMemberExists: " + mTextBoxDataMemberExists.ToString() + "\n" + "mLabelDataMemberExists: " +
//                                mLabelDataMemberExists.ToString() + "\nmLookUpTableExists: " + mLookUpTableExists.ToString() +
//                                "\nmDelegateFunctionExists: " +
//                                mDelegateFunctionExists.ToString();
//
//                MessageBox.Show(mResultString);
//            }

            return ReturnValue;
        }

        /// <summary>
        /// This function checks whether all requirements of the TwoTableMode do
        /// exist.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool CheckFunctionModeRequirements(out String ErrorString)
        {
            bool ReturnValue;
            bool mTextBoxDataMemberExists;
            bool mLabelDataMemberExists;
            bool mLookUpTableExists;
            bool mDelegateFunctionExists;

            #region Initialization
            ReturnValue = false;
            mTextBoxDataMemberExists = false;
            mLabelDataMemberExists = false;
            mLookUpTableExists = false;
            mDelegateFunctionExists = false;
            ErrorString = "";
            #endregion
            #region Check requirements

            if ((this.TextBoxLookUpMember != "") && (this.TextBoxLookUpMember != null))
            {
                mTextBoxDataMemberExists = true;
            }

            // Check for LabelDataMemberExists
            if ((this.LabelLookUpMember != "") && (this.LabelLookUpMember != null))
            {
                mLabelDataMemberExists = true;
            }

            // Check for LookUpTableExists
            if (this.LookUpDataSource != null)
            {
                mLookUpTableExists = true;
            }

            // Check for DelegateFunctionExists
            if (this.SetLabel != null)
            {
                mDelegateFunctionExists = true;
            }

            #endregion
            #region Build ErrorString
            ErrorString = "mTextBoxDataMemberExists: " + mTextBoxDataMemberExists.ToString() + "\n" + "mLabelDataMemberExists:   " +
                          mLabelDataMemberExists.ToString() + "\n" + "mLookUpTableExists:       " + mLookUpTableExists.ToString() + "\n" +
                          "mDelegateFunctionExists:  " + mDelegateFunctionExists.ToString();
            #endregion
            #region Assemble requirements

            if ((mTextBoxDataMemberExists == false) && (mLabelDataMemberExists == false) && (mLookUpTableExists == false)
                && (mDelegateFunctionExists == true))
            {
                ReturnValue = true;
            }

            #endregion
            return ReturnValue;
        }

        /// <summary>
        /// This function checks whether all requirements of the TwoTableMode do
        /// exist.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool CheckOneTableModeRequirements(out String ErrorString)
        {
            bool ReturnValue;
            bool mTextBoxDataMemberExists;
            bool mLabelDataMemberExists;
            bool mLookUpTableExists;
            bool mDelegateFunctionExists;

            #region Initialization
            ReturnValue = false;
            mTextBoxDataMemberExists = false;
            mLabelDataMemberExists = false;
            mLookUpTableExists = false;
            mDelegateFunctionExists = false;
            ErrorString = "";
            #endregion
            #region Check requirements

            if ((this.TextBoxLookUpMember != "") || (this.TextBoxLookUpMember != null))
            {
                mTextBoxDataMemberExists = true;
            }

            // Check for LabelDataMemberExists
            if ((this.LabelLookUpMember != "") || (this.LabelLookUpMember != null))
            {
                mLabelDataMemberExists = true;
            }

            // Check for LookUpTableExists
            if (this.LookUpDataSource != null)
            {
                mLookUpTableExists = true;
            }

            // Check for DelegateFunctionExists
            if (this.SetLabel != null)
            {
                mDelegateFunctionExists = true;
            }

            #endregion
            #region Build ErrorString
            ErrorString = "mTextBoxDataMemberExists: " + mTextBoxDataMemberExists.ToString() + "\n" + "mLabelDataMemberExists:   " +
                          mLabelDataMemberExists.ToString() + "\n" + "mLookUpTableExists:       " + mLookUpTableExists.ToString() + "\n" +
                          "mDelegateFunctionExists:  " + mDelegateFunctionExists.ToString();
            #endregion
            #region Assemble requirements

            if ((mTextBoxDataMemberExists == false) && (mLabelDataMemberExists == false) && (mLookUpTableExists == false)
                && (mDelegateFunctionExists == false))
            {
                ReturnValue = true;
            }

            #endregion
            return ReturnValue;
        }

        /// <summary>
        /// This function checks whether all requirements of the TwoTableMode do
        /// exist.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool CheckTwoTableModeRequirements(out String ErrorString)
        {
            bool ReturnValue;
            bool mTextBoxDataMemberExists;
            bool mLabelDataMemberExists;
            bool mLookUpTableExists;
            bool mDelegateFunctionExists;

            #region Initialization
            ReturnValue = false;
            mTextBoxDataMemberExists = false;
            mLabelDataMemberExists = false;
            mLookUpTableExists = false;
            mDelegateFunctionExists = false;
            ErrorString = "";
            #endregion
            #region Check requirements

            if ((this.TextBoxLookUpMember != "") || (this.TextBoxLookUpMember != null))
            {
                mTextBoxDataMemberExists = true;
            }

            // Check for LabelDataMemberExists
            if ((this.LabelLookUpMember != "") || (this.LabelLookUpMember != null))
            {
                mLabelDataMemberExists = true;
            }

            // Check for LookUpTableExists
            if (this.LookUpDataSource != null)
            {
                mLookUpTableExists = true;
            }

            // Check for DelegateFunctionExists
            if (this.SetLabel != null)
            {
                mDelegateFunctionExists = true;
            }

            #endregion
            #region Build ErrorString
            ErrorString = "mTextBoxDataMemberExists: " + mTextBoxDataMemberExists.ToString() + "\n" + "mLabelDataMemberExists:   " +
                          mLabelDataMemberExists.ToString() + "\n" + "mLookUpTableExists:       " + mLookUpTableExists.ToString() + "\n" +
                          "mDelegateFunctionExists:  " + mDelegateFunctionExists.ToString();
            #endregion
            #region Assemble requirements

            if ((mTextBoxDataMemberExists == true) && (mLabelDataMemberExists == true) && (mLookUpTableExists == true)
                && (mDelegateFunctionExists == false))
            {
                ReturnValue = true;
            }

            #endregion
            return ReturnValue;
        }

        #endregion
    }

    /// <summary>
    /// which values are allowed in the textbox
    /// </summary>
    public enum TKeyValuesEnum
    {
        /// <summary>
        /// digits
        /// </summary>
        OnlyDigits,

        /// <summary>
        /// numbers
        /// </summary>
        OnlyNumbers,

        /// <summary>
        /// letters
        /// </summary>
        OnlyLetters,

        /// <summary>
        /// letters or digits
        /// </summary>
        OnlyLettersOrDigits,

        /// <summary>
        /// printable
        /// </summary>
        OnlyPrintables,

        /// <summary>
        ///  all keys
        /// </summary>
        AllKeys
    };

    /// <summary>
    /// TODOCOMMENT
    /// </summary>
    public enum TButtonLabelControlMode
    {
        /// <summary>
        /// TODOCOMMENT
        /// </summary>
        None,

        /// <summary>
        /// TODOCOMMENT
        /// </summary>
        TwoTableMode,

        /// <summary>
        /// TODOCOMMENT
        /// </summary>
        FunctionMode
    };

    /// <summary>
    /// Here the hosting form has to provide a function which sets the Label's and
    /// TextBoxes Texts after the button is clicked.
    /// </summary>
    /// <param name="ALabelStringIn">Current Label text.</param>
    /// <param name="ATextBoxStringIn">Current TextBox text.</param>
    /// <param name="ALabelStringOut">Updated Label text.</param>
    /// <param name="APartnerClassOut">Updated Partner Class (if applicable).</param>
    /// <param name="ATextBoxStringOut">Updated TextBox text.</param>
    public delegate void TDelegateButtonClick(System.String ALabelStringIn,
        System.String ATextBoxStringIn,
        out System.String ALabelStringOut,
        out System.String APartnerClassOut,
        out System.String ATextBoxStringOut);

    /// <summary>
    /// Here the hosting form has to provide a text for the label
    /// </summary>
    /// <param name="ALookupText">A lookup text goes here</param>
    /// <param name="APartnerClass">Updated Partner Class (if applicable).</param>
    /// <param name="ALabelText">Updated Label text</param>
    public delegate void TDelegateSetLabel(String ALookupText, ref System.String ALabelText, ref System.String APartnerClass);

    /// <summary>
    /// This delegate is used to notify the hosting control of an error.
    /// </summary>
    /// <param name="ATextBoxText">The text in the textbox</param>
    /// <param name="AErrorMessage">An error message</param>
    /// <param name="AEvaluationResult">true if there is no error</param>
    public delegate void TDelegateEvaluationFault(String ATextBoxText, String AErrorMessage, bool AEvaluationResult);

    /// <summary>
    /// This event will let the hosing control evaluate the textbox string.
    /// </summary>
    /// <param name="ATextBoxText">The text in the textbox</param>
    /// <param name="ALabelString">A String the label should display</param>
    /// <param name="ATextValid">true if the text of the textbox is valid otherwise false.</param>
    public delegate void TDelegateEvaluateText(String ATextBoxText, out String ALabelString, out bool ATextValid);

    /// <summary>
    /// problem with data binding
    /// </summary>
    public class EDataBindingFailure : System.ArgumentException
    {
        #region Exceptions

        /// <summary>
        /// constructor
        /// </summary>
        public EDataBindingFailure() : base(TTxtButtonLabel.EXCEPTION_DATA_BINDING)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AMessage"></param>
        public EDataBindingFailure(String AMessage)
            : base(TTxtButtonLabel.EXCEPTION_DATA_BINDING + AMessage)
        {
        }
    }

    /// <summary>
    /// exception for button click
    /// </summary>
    public class EButtonClick : System.ArgumentException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EButtonClick()
            : base(TTxtButtonLabel.EXCEPTION_BUTTON_CLICK)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AMessage"></param>
        public EButtonClick(String AMessage)
            : base(TTxtButtonLabel.EXCEPTION_BUTTON_CLICK + AMessage)
        {
        }

        #endregion
    }
}