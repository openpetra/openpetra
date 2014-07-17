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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Data;
using System.Globalization;

namespace Ict.Common.Controls
{
    /// <summary>
    /// The TCmbLabelled is a ComboBox which offers the possibility of having
    /// up to 4 columns in its drop down pane as well as a databound label next to it.
    /// Within these columns can be either some text or an 16 by 16 image. The
    /// TCmbLabelled has several new properties. The TCmbLabelled does
    /// not allowed the user to add new items to the ComboBox. The following
    /// properties are essential for this control to work:
    /// - DataSource:
    ///   Here the underlying table / view has to be set. This is mostly done in the
    ///   following way:
    ///     cmbLabelledComboBox.cmbComboBox.BeginUpdate();
    ///     cmbLabelledComboBox.cmbComboBox.DataSource := YOUR_DATA_TABLE / YOUR_DATA_VIEW;
    ///     cmbLabelledComboBox.cmbComboBox.DisplayMember := COLUMN NAME;
    ///     cmbLabelledComboBox.cmbComboBox.ValueMember := COLUMN NAME;
    ///     cmbLabelledComboBox.cmbComboBox.EndUpdate();
    ///   PLEASE remember that the actual Combobox is public and you have to make sure
    ///   that the data is really bound to the ComboBox and not to the UserControl itself.
    /// - DisplayInColumn1 ... DisplayIn Column4:
    ///   Here the colomn of the underlying DataSource table / view has to be set. If
    ///   no table column name for the 1st column of the drop down is given
    ///   DisplayMember respectively ValueMember is used.
    /// - ColumnWidthCol1 ... ColumnWidthCol4:
    ///   Here the width of the columns of the drop down pane is adjusted. Please note
    ///   that it is not allowed to set the with of a previous column to 0 and the next
    ///   column to a value other than 0 (ColumnWidthCol2 = 0, ColumnWidthCol3 = 100 is
    ///   a forbidden scenario.
    /// - GridLine... ;
    ///   If set to true a grid line in the drop down pane is drawn
    /// - LabelDisplaysColumn:
    ///   This property sets the content of the label. You can display any content of
    ///   your DataSource provided, you give the right column name of the dataSource.
    /// - Images:
    ///   Here an ImageList is needed. All images in that list have indices. If you note
    ///   these indices in one column of the DataSource the cmbVersatile ComboBox will
    ///   automatically grab these images and display them in the colomn you assign.
    /// - ImageColumn:
    ///   The column number of the drop down's column which holds the image indices.
    ///   Please note: 0 refers to no column!, 1 to column 1 ... 4 to column 4.
    /// </summary>
    public class TCmbLabelled : System.Windows.Forms.UserControl
    {
        private const Int32 UNIT_HEIGHT = 22;
        private const Int32 UNIT_LABEL_LEFT_OFFSET = 6;

        /// <summary>Denotes what this Control regards as the identifier string of inactive combobox items.</summary>
        private static readonly string FInactiveIdentifier = "";

        /// <summary>
        /// the Combobox that is part of this user control
        /// </summary>
        public TCmbVersatile cmbCombobox;

        /// <summary> Required designer variable. </summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary>
        /// the label that is part of this user control
        /// Note: this control is actually a TextBox disguised as a Label. This makes it possible to select and copy this control's text
        /// </summary>
        protected System.Windows.Forms.TextBox lblDescription;

        /// <summary>
        /// define which column of the combobox will be used for the label
        /// </summary>
        protected String FLabelDisplaysColumn;

        /// <summary>
        /// Denotes what this Control regards as the identifier string of inactive combobox items.
        /// </summary>
        public static string InactiveIdentifier
        {
            get
            {
                return FInactiveIdentifier;
            }
        }

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore null.
        ///
        /// </summary>
        public bool CaseSensitiveSearch
        {
            get
            {
                return this.cmbCombobox.CaseSensitiveSearch;
            }

            set
            {
                this.cmbCombobox.CaseSensitiveSearch = value;
            }
        }

        /// <summary>
        /// This property manages number of columns within the ComboBox.
        ///
        /// </summary>
        public int ColumnNum
        {
            get
            {
                return this.cmbCombobox.ColumnNum;
            }
        }

        /// <summary>
        /// set the width of a column within the ComboBox.
        /// </summary>
        public int ColumnWidthCol1
        {
            set
            {
                this.cmbCombobox.SetColumnWidth(0, value);
            }
            get
            {
                return this.cmbCombobox.GetColumnWidth(0);
            }
        }

        /// <summary>
        /// set the width of a column within the ComboBox.
        /// </summary>
        public int ColumnWidthCol2
        {
            set
            {
                this.cmbCombobox.SetColumnWidth(1, value);
            }
            get
            {
                return this.cmbCombobox.GetColumnWidth(1);
            }
        }

        /// <summary>
        /// set the width of a column within the ComboBox.
        /// </summary>
        public int ColumnWidthCol3
        {
            set
            {
                this.cmbCombobox.SetColumnWidth(2, value);
            }
            get
            {
                return this.cmbCombobox.GetColumnWidth(2);
            }
        }

        /// <summary>
        /// set the width of a column within the ComboBox.
        /// </summary>
        public int ColumnWidthCol4
        {
            set
            {
                this.cmbCombobox.SetColumnWidth(3, value);
            }
            get
            {
                return this.cmbCombobox.GetColumnWidth(3);
            }
        }

        /// <summary>
        /// This property may be used to influence the width of the ComboBox.
        ///
        /// </summary>
        public int ComboBoxWidth
        {
            get
            {
                return this.cmbCombobox.Size.Width;
            }

            set
            {
                this.cmbCombobox.SetBounds(this.cmbCombobox.Bounds.X,
                    this.cmbCombobox.Bounds.Y,
                    value,
                    UNIT_HEIGHT);

                if (lblDescription.Visible)
                {
                    this.lblDescription.SetBounds(this.cmbCombobox.Bounds.X + value + UNIT_LABEL_LEFT_OFFSET,
                        this.cmbCombobox.Bounds.Y + GetYCoordStartLabel(),
                        this.Bounds.Width - value,
                        UNIT_HEIGHT);
                }
            }
        }

        /// <summary>
        /// set the content of a column in the combobox
        /// </summary>
        public void DisplayInColumn(int AColumnNr, string ADisplay)
        {
            this.cmbCombobox.DisplayInColumn(AColumnNr, ADisplay);
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn1
        {
            set
            {
                DisplayInColumn(0, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn2
        {
            set
            {
                DisplayInColumn(1, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn3
        {
            set
            {
                DisplayInColumn(2, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn4
        {
            set
            {
                DisplayInColumn(3, value);
            }
        }

        /// <summary>
        /// This property may be used to influence the DisplayMember.
        ///
        /// </summary>
        public string DisplayMember
        {
            get
            {
                return this.cmbCombobox.DisplayMember;
            }

            set
            {
                this.cmbCombobox.DisplayMember = value;
            }
        }

        /// <summary>
        /// This property manages the image column.
        ///
        /// </summary>
        public int ImageColumn
        {
            get
            {
                return this.cmbCombobox.ImageColumn;
            }

            set
            {
                this.cmbCombobox.ImageColumn = value;
            }
        }

        /// <summary>
        /// This property holds the images.
        ///
        /// </summary>
        public System.Windows.Forms.ImageList Images
        {
            get
            {
                return this.cmbCombobox.Images;
            }

            set
            {
                this.cmbCombobox.Images = value;
            }
        }

        /// <summary>
        /// This property may be used to read the width of the Label.
        ///
        /// </summary>
        public string LabelDisplaysColumn
        {
            get
            {
                return this.FLabelDisplaysColumn;
            }

            set
            {
                this.FLabelDisplaysColumn = value;
            }
        }

        /// <summary>
        /// This property may be used to read the width of the Label.
        ///
        /// </summary>
        public int LabelWidth
        {
            get
            {
                System.Int32 mComboBoxWidth;
                System.Int32 mTotalWidth;
                mComboBoxWidth = this.ComboBoxWidth;
                mTotalWidth = this.Size.Width;
                return mTotalWidth - mComboBoxWidth;
            }
        }

        /// <summary>
        /// This property manages the colour of the selection. If set to TRUE the
        /// selected item of the ComboBox is not coloured in selection mode colours.
        ///
        /// </summary>
        public bool SuppressSelectionColor
        {
            get
            {
                return this.cmbCombobox.SuppressSelectionColor;
            }

            set
            {
                this.cmbCombobox.SuppressSelectionColor = value;
            }
        }

        /// <summary>
        /// This property may be used to influence the ValueMember.
        ///
        /// </summary>
        public string ValueMember
        {
            get
            {
                return this.cmbCombobox.ValueMember;
            }

            set
            {
                this.cmbCombobox.ValueMember = value;
            }
        }

        /// drop down the combobox
        public void DropDown()
        {
            cmbCombobox.DroppedDown = true;
        }

        /// pass through the SelectedIndex property from the combobox
        public Int32 SelectedIndex
        {
            get
            {
                return this.cmbCombobox.SelectedIndex;
            }
            set
            {
                this.cmbCombobox.SelectedIndex = value;
            }
        }

        /// the number of items in the combobox items list
        public Int32 Count
        {
            get
            {
                return cmbCombobox.Items.Count;
            }
        }

        /// the label attached to the combobox
        public TextBox AttachedLabel
        {
            get
            {
                return this.lblDescription;
            }
        }

        /// the text displayed in the combobox label including an inactive prefix
        public string LabelTextFull
        {
            get
            {
                return this.lblDescription.Text;
            }
        }

        /// the text displayed in the combobox label with an inactive prefix removed
        public string LabelTextOriginal
        {
            get
            {
                string labelText = this.lblDescription.Text;

                if (labelText.StartsWith(FInactiveIdentifier))
                {
                    labelText = labelText.Substring(FInactiveIdentifier.Length);
                }

                return labelText;
            }
        }

        /// <summary>
        /// set the string that should be selected;
        /// uses TCmbVersatile.SetSelectedString
        /// </summary>
        public bool SetSelectedString(string ASelectedString, Int32 ADefaultIndex)
        {
            return this.cmbCombobox.SetSelectedString(ASelectedString, ADefaultIndex);
        }

        /// <summary>
        /// set the string that should be selected;
        /// uses TCmbVersatile.SetSelectedString
        /// </summary>
        /// <param name="ASelectedString"></param>
        public bool SetSelectedString(string ASelectedString)
        {
            return this.cmbCombobox.SetSelectedString(ASelectedString);
        }

        /// <summary>
        /// get the selected string
        /// uses TCmbVersatile.GetSelectedString
        /// </summary>
        public string GetSelectedString(Int32 Idx = -1)
        {
            return this.cmbCombobox.GetSelectedString(Idx);
        }

        /// <summary>
        /// get the selected description
        /// uses TCmbVersatile.GetSelectedDescription
        /// </summary>
        public string GetSelectedDescription()
        {
            return this.cmbCombobox.GetSelectedDescription();
        }

        /// <summary>
        /// Selects an item with the given Int32 value in the first column. Selects first element if the Int32 value is not existing.
        /// uses TCmbVersatile.SetSelectedInt32
        /// </summary>
        /// <param name="ANr"></param>
        public void SetSelectedInt32(System.Int32 ANr)
        {
            this.cmbCombobox.SetSelectedInt32(ANr);
        }

        /// <summary>
        /// gets the Int32 value of the selected item, first column
        /// uses TCmbVersatile.GetSelectedInt32
        /// </summary>
        public Int32 GetSelectedInt32()
        {
            return this.cmbCombobox.GetSelectedInt32();
        }

        /// <summary>
        /// Selects an item with the given Int64 value in the first column. Selects first element if the Int64 value is not existing.
        /// uses TCmbVersatile.SetSelectedInt64
        /// </summary>
        /// <param name="ANr"></param>
        public void SetSelectedInt64(System.Int64 ANr)
        {
            this.cmbCombobox.SetSelectedInt64(ANr);
        }

        /// <summary>
        /// gets the Int32 value of the selected item, first column
        /// uses TCmbVersatile.GetSelectedInt32
        /// </summary>
        public Int64 GetSelectedInt64()
        {
            return this.cmbCombobox.GetSelectedInt64();
        }

        #region Windows Form Designer generated code

        /// <summary> Required method for Designer support
        /// do not modify the contents of this method with the code editor.
        /// This procedure initializes this very component.
        /// </summary>
        /// <returns>void</returns>
        protected void InitializeComponent()
        {
            this.SuspendLayout();

            this.lblDescription = new System.Windows.Forms.TextBox();

            //
            // lblDescription
            //
            this.lblDescription.BackColor = System.Drawing.SystemColors.Control;
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(168, this.cmbCombobox.Height);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.TextAlign = HorizontalAlignment.Left;
            this.lblDescription.Paint += new PaintEventHandler(this.LblDescription_Paint);

            this.lblDescription.Multiline = false;
            this.lblDescription.WordWrap = false;
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblDescription.ReadOnly = true;
            this.lblDescription.Location = new System.Drawing.Point(GetLabelRectangle().Left + UNIT_LABEL_LEFT_OFFSET, GetYCoordStartLabel());
            this.lblDescription.TabStop = false;

            //
            // TCmbLabelled
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblDescription);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (Byte)0);
            this.Name = "TCmbLabelled";
            this.Size = new System.Drawing.Size(384, 21);
            this.ForeColorChanged += new System.EventHandler(this.TCmbLabelled_ForeColorChanged);
            this.VisibleChanged += new System.EventHandler(this.TCmbLabelled_VisibleChanged);
            this.BackColorChanged += new System.EventHandler(this.TCmbLabelled_BackColorChanged);
            this.Leave += new System.EventHandler(this.TCmbLabelled_Leave);
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal

        static TCmbLabelled()
        {
            if (TCommonControlsHelper.SetInactiveIdentifier != null)
            {
                FInactiveIdentifier = TCommonControlsHelper.SetInactiveIdentifier() + " ";
            }
            else
            {
                FInactiveIdentifier = "<INACTIVE> ";
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TCmbLabelled() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComboBox();

            InitializeComponent();

            this.Height = UNIT_HEIGHT;

            this.SetDefaultProperties();
        }

        /// <summary>
        /// This procedure initializes the ComboBox of this control. This procedure has
        /// to be called before calling the procedure InitializeComponent.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitializeComboBox()
        {
            this.cmbCombobox = new Ict.Common.Controls.TCmbVersatile();
            this.Controls.Add(this.cmbCombobox);

            //
            // cmbCombobox
            //
            this.cmbCombobox.DataSource = null;
            this.cmbCombobox.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCombobox.GridLineColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCombobox.Images = null;
            this.cmbCombobox.Location = new System.Drawing.Point(0, 0);
            this.cmbCombobox.Name = "cmbCombobox";
            this.cmbCombobox.Size = new System.Drawing.Size(121, 21);
            this.cmbCombobox.TabIndex = 0;
            this.cmbCombobox.Text = "";
            this.cmbCombobox.SelectionChangeCommitted += new System.EventHandler(this.TCmbLabelled_SelectionChangeCommitted);
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
        /// This function calculates the area of the label.
        /// </summary>
        /// <returns>The Rectangle of the label.
        /// </returns>
        protected System.Drawing.Rectangle GetLabelRectangle()
        {
            return new System.Drawing.Rectangle(
                0, 0, this.GetLabelWidth(), UNIT_HEIGHT);
        }

        /// <summary>
        /// This function calculates the width of the label.
        /// </summary>
        /// <returns>The width of the label.</returns>
        protected int GetLabelWidth()
        {
            return this.Size.Width - this.cmbCombobox.Width;
        }

        /// <summary>
        /// This procedure sets the default height for this UserControl.
        /// </summary>
        /// <returns>void</returns>
        private void SetDefaultHeight()
        {
            this.lblDescription.Height = UNIT_HEIGHT;
            this.Height = UNIT_HEIGHT;
        }

        /// <summary>
        /// This procedure sets the default properties for this UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetDefaultProperties()
        {
            this.SetDefaultHeight();
            this.SetLabelWidth();
            this.Invalidate();
        }

        #endregion

        /// <summary>
        /// This function sets the width of the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetLabelWidth()
        {
            Int32 mLabelWidth = this.Size.Width - this.ComboBoxWidth;

            if (mLabelWidth < 0)
            {
                mLabelWidth = 0;
            }

            this.lblDescription.Width = mLabelWidth;
        }

        /// <summary>
        /// This function gets the start Y - Coordinate for the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        private int GetYCoordStartLabel()
        {
            System.Single mLabelHeight;
            System.Int32 mOffset;
            double mYCoord;
            mLabelHeight = this.lblDescription.CreateGraphics().MeasureString("SampleText", this.lblDescription.Font).Height;
            mOffset = this.Size.Height - Convert.ToInt32(mLabelHeight);

            if (mOffset < 0)
            {
                mOffset = 0;
            }

            mYCoord = mOffset / 2.0;

            return Convert.ToInt32(mYCoord);
        }

        /// <summary>
        /// in some cases, we don't want the labelled combobox, the description and the display are the same
        /// </summary>
        public void RemoveDescriptionLabel()
        {
            cmbCombobox.Width = this.Width;
            lblDescription.Visible = false;
            this.Invalidate();
        }

        /// <summary>
        /// Clear all the contents of the combobox
        /// </summary>
        public void Clear()
        {
            this.cmbCombobox.DataSource = null;
            this.cmbCombobox.Text = string.Empty;
        }

        #region Event handling

        /// <summary>
        /// Forces a refresh of the combobox label
        /// </summary>
        public void RefreshLabel()
        {
            RefreshLabel(null, null);
            this.Invalidate();
        }

        private void RefreshLabel(object sender, EventArgs e)
        {
            string descr = cmbCombobox.GetSelectedDescription();

            this.lblDescription.Text = descr;

            if (this.lblDescription.Visible)
            {
                HighlightLabelForInactiveItems(descr);
            }
        }

        private void HighlightLabelForInactiveItems(string AItemDescription)
        {
            bool itemIsActive = !(AItemDescription.StartsWith(FInactiveIdentifier));

            if (itemIsActive && (this.lblDescription.BackColor != System.Drawing.SystemColors.Control))
            {
                this.lblDescription.BackColor = System.Drawing.SystemColors.Control;
            }
            else if (!itemIsActive && (this.lblDescription.BackColor != System.Drawing.Color.PaleVioletRed))
            {
                this.lblDescription.BackColor = System.Drawing.Color.PaleVioletRed;
            }
        }

        /// <summary>
        /// This event occurs when the control is created.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!(DesignMode))
            {
                if ((this.FLabelDisplaysColumn == null) || (this.FLabelDisplaysColumn == ""))
                {
                    this.FLabelDisplaysColumn = this.cmbCombobox.DisplayMember;
                }

                System.Drawing.Font mLabelFont = new System.Drawing.Font(this.Font.FontFamily, (this.Font.Size - 1), System.Drawing.FontStyle.Regular);
                this.lblDescription.Font = mLabelFont;

                this.cmbCombobox.DescriptionMember = this.FLabelDisplaysColumn;
                this.lblDescription.Text = this.cmbCombobox.GetSelectedDescription();
                this.cmbCombobox.SelectedIndexChanged += RefreshLabel;
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
            this.Size = new Size(this.Size.Width, UNIT_HEIGHT);
        }

        /// <summary>
        /// This procedure ensures that the background color cannot be changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected void TCmbLabelled_BackColorChanged(System.Object sender, System.EventArgs e)
        {
            this.lblDescription.BackColor = this.BackColor;
        }

        /// <summary>
        /// This procedure ensures that the foreground color cannot be changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event Arguments</param>
        /// <returns>void</returns>
        protected void TCmbLabelled_ForeColorChanged(System.Object sender, System.EventArgs e)
        {
            this.lblDescription.ForeColor = this.ForeColor;
        }

        #endregion

        #region Costumized events

        /// <summary>
        /// custom paint method for the label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LblDescription_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Getting all sorts of things for drawing the string
            StringFormat mStringFormat = new System.Drawing.StringFormat();

            mStringFormat.FormatFlags |= System.Drawing.StringFormatFlags.NoWrap;

            if (this.cmbCombobox.SelectedIndex == -1)
            {
                this.lblDescription.Text = "";
            }

            Rectangle mRectangle = this.GetLabelRectangle();
            int mXCoord = mRectangle.Left + UNIT_LABEL_LEFT_OFFSET;
            int mYCoord = mRectangle.Top + GetYCoordStartLabel();

            // Clear background
            e.Graphics.Clear(this.lblDescription.BackColor);

            // Draw String
            if (lblDescription.Visible)
            {
                e.Graphics.DrawString(
                    this.lblDescription.Text,
                    this.lblDescription.Font,
                    new System.Drawing.SolidBrush(this.lblDescription.ForeColor),
                    new System.Drawing.PointF(mXCoord, mYCoord),
                    mStringFormat);
            }
        }

        /// <summary>
        /// when the focus is lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TCmbLabelled_Leave(System.Object sender, System.EventArgs e)
        {
            if (this.cmbCombobox.Text == "")
            {
                this.lblDescription.Text = "";
                this.lblDescription.Refresh();
                this.lblDescription.Invalidate();
            }
        }

        /// <summary>
        /// This event goes off when the selection is committed to the combobox.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The event arguments.
        /// </param>
        /// <returns>void</returns>
        protected void TCmbLabelled_SelectionChangeCommitted(System.Object sender, System.EventArgs e)
        {
            this.cmbCombobox.SetSelectionColorLength();
        }

        /// <summary>
        /// when the control is displayed again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TCmbLabelled_VisibleChanged(System.Object sender, System.EventArgs e)
        {
            this.cmbCombobox.Invalidate();
            this.cmbCombobox.SetSelectionColorLength();
            this.lblDescription.Invalidate();
        }

        #endregion
    }
}