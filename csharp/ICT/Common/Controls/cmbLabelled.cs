/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       markusm, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
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
    /// TODO: Known features from .Net:
    /// The databinding is a little bit troublesome. You can use it if the rows of the
    /// DataSource table have been added with the routine 'Add'. Otherwise the databinding
    /// just breaks. For additional information on this topic visit the Petra - Wiki.
    /// TODO: Here is the link to the complete article:
    /// http://ict.om.org/PetraWiki/current/index.php?title=Problems_in_DataBinding_a_View_to_a_ComboBox
    /// </summary>
    public class TCmbLabelled : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// the Combobox that is part of this user control
        /// </summary>
        public TCmbVersatile cmbCombobox;

        /// <summary> Required designer variable. </summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary>
        /// the label that is part of this user control
        /// </summary>
        protected System.Windows.Forms.Label lblDescription;

        /// <summary>
        /// define which column of the combobox will be used for the label
        /// </summary>
        protected String FLabelDisplaysColumn;

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
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol1
        {
            get
            {
                return this.cmbCombobox.ColumnWidthCol1;
            }

            set
            {
                this.cmbCombobox.ColumnWidthCol1 = value;
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol2
        {
            get
            {
                return this.cmbCombobox.ColumnWidthCol2;
            }

            set
            {
                this.cmbCombobox.ColumnWidthCol2 = value;
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol3
        {
            get
            {
                return this.cmbCombobox.ColumnWidthCol3;
            }

            set
            {
                this.cmbCombobox.ColumnWidthCol3 = value;
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol4
        {
            get
            {
                return this.cmbCombobox.ColumnWidthCol4;
            }

            set
            {
                this.cmbCombobox.ColumnWidthCol4 = value;
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
                    cmbLabelledComboBox.UNIT_HEIGHT);
                this.lblDescription.SetBounds(this.cmbCombobox.Bounds.X + value,
                    this.cmbCombobox.Bounds.Y,
                    this.Bounds.Width - value,
                    cmbLabelledComboBox.UNIT_HEIGHT);
            }
        }

        /// <summary>
        /// This property manages the content of the 1st column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn1
        {
            get
            {
                return cmbCombobox.DisplayInColumn1;
            }

            set
            {
                this.cmbCombobox.DisplayInColumn1 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 2nd column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn2
        {
            get
            {
                return this.cmbCombobox.DisplayInColumn2;
            }

            set
            {
                this.cmbCombobox.DisplayInColumn2 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 3rd column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn3
        {
            get
            {
                return this.cmbCombobox.DisplayInColumn3;
            }

            set
            {
                this.cmbCombobox.DisplayInColumn3 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 4th column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn4
        {
            get
            {
                return this.cmbCombobox.DisplayInColumn4;
            }

            set
            {
                this.cmbCombobox.DisplayInColumn4 = value;
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


        #region Windows Form Designer generated code

        /// <summary> Required method for Designer support
        /// do not modify the contents of this method with the code editor.
        /// This procedure initializes this very component.
        /// </summary>
        /// <returns>void</returns>
        protected void InitializeComponent()
        {
            this.SuspendLayout();

            this.lblDescription = new System.Windows.Forms.Label();

            //
            // lblDescription
            //
            this.lblDescription.BackColor = System.Drawing.SystemColors.Control;
            this.lblDescription.Location = new System.Drawing.Point(216, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(168, 22);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDescription.Paint += new PaintEventHandler(this.LblDescription_Paint);

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

            this.Height = cmbLabelledComboBox.UNIT_HEIGHT;

            this.SetDefaultProperties();

            // TLogging.Log('cmbVersatile log starts... ' + this.GetType().ToString);
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
            this.cmbCombobox.DisplayInColumn1 = null;
            this.cmbCombobox.DisplayInColumn2 = null;
            this.cmbCombobox.DisplayInColumn3 = null;
            this.cmbCombobox.DisplayInColumn4 = null;
            this.cmbCombobox.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCombobox.GridLineColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCombobox.Images = null;
            this.cmbCombobox.Location = new System.Drawing.Point(0, 0);
            this.cmbCombobox.Name = "cmbCombobox";
            this.cmbCombobox.Size = new System.Drawing.Size(121, 21);
            this.cmbCombobox.TabIndex = 0;
            this.cmbCombobox.Text = "";
            this.cmbCombobox.DataSourceChanged += new System.EventHandler(this.TCmbLabelled_DataSourceChanged);
            this.cmbCombobox.SelectionChangeCommitted += new System.EventHandler(this.TCmbLabelled_SelectionChangeCommitted);
        }

        /// <summary>
        /// This procedure adds this colomn to the DataBindings of the lblDescription.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void DataBindLabel(String AColumnName)
        {
            String mColumnToBind;
            Binding oldBinding;

            if (!(DesignMode))
            {
                if (this.cmbCombobox.DataSourceContainsColumn(AColumnName) == true)
                {
                    mColumnToBind = AColumnName;
                }
                else
                {
                    mColumnToBind = this.cmbCombobox.DisplayMember;
                }

                // Do the databinding
                // this.lblDescription.DataBindings.Clear;
                // don't add the binding twice; so first try to find the binding and remove it
                // otherwise the cmbcombobox.valuemember gets lost
                oldBinding = this.lblDescription.DataBindings["Text"];

                if (oldBinding != null)
                {
                    this.lblDescription.DataBindings.Remove(oldBinding);
                }

                this.lblDescription.DataBindings.Add("Text", this.cmbCombobox.DataSource, mColumnToBind);
                oldBinding = this.DataBindings["Text"];

                if (oldBinding != null)
                {
                    this.DataBindings.Remove(oldBinding);
                }

                this.DataBindings.Add("Text", this.cmbCombobox.DataSource, mColumnToBind);
            }
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
            mHeight = System.Convert.ToSingle(cmbLabelledComboBox.UNIT_HEIGHT);
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
            System.Int32 mControlWidth;
            System.Int32 mComboBoxWidth;
            mControlWidth = this.Size.Width;
            mComboBoxWidth = this.cmbCombobox.Width;
            return mControlWidth - mComboBoxWidth;
        }

        /// <summary>
        /// This procedure sets the default height for this UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetDefaultHeight()
        {
            this.lblDescription.Height = cmbLabelledComboBox.UNIT_HEIGHT;
            this.Height = cmbLabelledComboBox.UNIT_HEIGHT;
        }

        /// <summary>
        /// This procedure sets the default properties for this UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetDefaultProperties()
        {
            this.SetDefaultHeight();
            this.Set_LabelWidth();
            this.Invalidate();
        }

        #endregion

        /// <summary>
        /// This function writes the width of the Label.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void Set_LabelWidth()
        {
            System.Int32 mComboBoxWidth;
            System.Int32 mTotalWidth;
            System.Int32 mLabelWidth;
            mComboBoxWidth = this.ComboBoxWidth;
            mTotalWidth = this.Size.Width;
            mLabelWidth = mTotalWidth - mComboBoxWidth;

            if (mLabelWidth < 0)
            {
                mLabelWidth = 0;
            }

            this.lblDescription.Width = mLabelWidth;
        }

        #region Event handling

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

                if (this.cmbCombobox.DataSource != null)
                {
                    DataBindLabel(this.FLabelDisplaysColumn);
                }
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
            System.Drawing.Size mSize;
            System.Int32 mComboBoxWidth;
            mComboBoxWidth = this.ComboBoxWidth;
            this.ComboBoxWidth = mComboBoxWidth;
            mSize = this.Size;
            mSize.Height = cmbLabelledComboBox.UNIT_HEIGHT;
            this.Size = mSize;
        }

        /// <summary>
        /// This event goes off when the DataSource is fired.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The event arguments.
        /// </param>
        /// <returns>void</returns>
        protected void TCmbLabelled_DataSourceChanged(System.Object sender, System.EventArgs e)
        {
            if (!(DesignMode))
            {
                DataBindLabel(this.FLabelDisplaysColumn);
            }
        }

        /// <summary>
        /// This procedure ensures that the height of this control cannot be changed.
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
        /// This procedure ensures that the height of this control cannot be changed.
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
            mStringFormat.FormatFlags |= System.Drawing.StringFormatFlags.NoWrap;

            if (this.cmbCombobox.SelectedIndex == -1)
            {
                this.lblDescription.Text = "";
            }

            mRectangleF = this.GetLabelRectangleF();
            mXCoord = mRectangleF.Left + System.Convert.ToSingle(cmbLabelledComboBox.UNIT_LABEL_LEFT_OFFSET);
            mYCoord = mRectangleF.Top + System.Convert.ToSingle(cmbLabelledComboBox.UNIT_LABEL_TOP_OFFSET);
            mPoint = new System.Drawing.PointF(mXCoord, mYCoord);
            mBrush = new System.Drawing.SolidBrush(this.lblDescription.ForeColor);

            // Clear background
            e.Graphics.Clear(this.lblDescription.BackColor);

            // Draw String
            e.Graphics.DrawString(this.lblDescription.Text, mFont, mBrush, mPoint, mStringFormat);
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
            this.lblDescription.Invalidate();
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


    class cmbLabelledComboBox
    {
        public const Int32 UNIT_HEIGHT = 22;
        public const Int32 UNIT_LABEL_LEFT_OFFSET = 5;
        public const Int32 UNIT_LABEL_TOP_OFFSET = 3;
    }
}