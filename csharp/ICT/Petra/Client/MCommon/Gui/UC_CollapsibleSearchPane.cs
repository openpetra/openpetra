//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;
using Ict.Petra.Client.App.Formatting;
using Ict.Common.Controls;
using System.Data;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// todoComment
    /// </summary>
    public partial class TUC_CollapsibleSearchPane : TGrpCollapsible
    {
        /// <summary>todoComment</summary>
        protected const Int32 constPanelMarginTop = 15;

        /// <summary>todoComment</summary>
        protected const Int32 constPanelMarginBottom = 3;

        /// <summary>todoComment</summary>
        protected const Int32 constPanelMarginLeft = 3;

        /// <summary>todoComment</summary>
        protected const Int32 constPanelMarginRight = 3;

        /// <summary>todoComment</summary>
        protected const Int32 constControlHeight = 23;

        /// <summary>todoComment</summary>
        protected const Int32 constDefaultLabelWidth = 150;

        /// <summary>todoComment</summary>
        protected const Int32 constDefaultButtonWidth = 100;

        /// <summary>todoComment</summary>
        protected const Int32 constDefaultTextBoxY = 1;

        /// <summary>todoComment</summary>
        protected const Int32 constRightLabelMargin = 5;

        /// <summary>todoComment</summary>
        protected const String constSingleQuote = "\"";

        /// <summary>todoComment</summary>
        protected const String constBlank = " ";

        /// <summary>todoComment</summary>
        protected const String constEquals = " = ";

        /// <summary>todoComment</summary>
        protected const String constAND = " AND ";

        /// <summary>todoComment</summary>
        protected int FPanelMarginTop;

        /// <summary>todoComment</summary>
        protected int FPanelMarginBottom;

        /// <summary>todoComment</summary>
        protected int FPanelMarginLeft;

        /// <summary>todoComment</summary>
        protected int FPanelMarginRight;

        /// <summary>todoComment</summary>
        protected int FMinWidth;

        /// <summary>todoComment</summary>
        protected int FMinHeight;

        /// <summary>todoComment</summary>
        protected int FYCoordPanelUpper;

        /// <summary>todoComment</summary>
        protected int FYCoordPanelMiddle;

        /// <summary>todoComment</summary>
        protected int FYCoordPanelLower;

        /// <summary>todoComment</summary>
        protected int FControlHeight;

        /// <summary>todoComment</summary>
        protected int FRightLabelMargin;

        /// <summary>todoComment</summary>
        protected bool FCollapsing;

        /// <summary>todoComment</summary>
        protected bool FCollapsed;

        /// <summary>todoComment</summary>
        protected bool FSelectWithEnter;

        /// <summary>todoComment</summary>
        protected bool FPreventCollapsing;

        /// <summary>todoComment</summary>
        protected string FDataMemberTextBoxMiddlePanel;

        /// <summary>todoComment</summary>
        protected System.Windows.Forms.Binding FDataBindingTextboxMiddlePanel;

        /// <summary>todoComment</summary>
        protected string FDataMemberTextBoxLowerPanel;

        /// <summary>todoComment</summary>
        protected System.Windows.Forms.Binding FDataBindingTextboxLowerPanel;

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public bool PanelVisibleUpper
        {
            get
            {
                return this.pnlPanelUpper.Visible;
            }

            set
            {
                this.pnlPanelUpper.Visible = value;
                this.OnResize(new System.EventArgs());
                this.OnLayout(new System.Windows.Forms.LayoutEventArgs(this.pnlCommonBackGround, "Controls"));
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public int LabelWidthUpperPanel
        {
            get
            {
                return this.lblPanelUpper.Width;
            }

            set
            {
                this.lblPanelUpper.Size = new System.Drawing.Size(value, this.lblPanelUpper.Height);
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public ContentAlignment LabelTextAlignUpperPanel
        {
            get
            {
                return this.lblPanelUpper.TextAlign;
            }

            set
            {
                this.lblPanelUpper.TextAlign = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public string LabelTextUpperPanel
        {
            get
            {
                return this.lblPanelUpper.Text;
            }

            set
            {
                this.lblPanelUpper.Text = value;
            }
        }

        /// <summary>
        /// The PanelVisibleMiddle determines whether the middle panel is visible or not.
        ///
        /// </summary>
        public bool PanelVisibleMiddle
        {
            get
            {
                return this.pnlPanelMiddle.Visible;
            }

            set
            {
                this.pnlPanelMiddle.Visible = value;
                this.OnResize(new System.EventArgs());
                this.OnLayout(new System.Windows.Forms.LayoutEventArgs(this.pnlCommonBackGround, "Controls"));
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public int LabelWidthMiddlePanel
        {
            get
            {
                return this.lblPanelMiddle.Width;
            }

            set
            {
                this.lblPanelMiddle.Size = new System.Drawing.Size(value, this.lblPanelMiddle.Height);
                this.PnlPanelMiddle_Resize(this.lblPanelMiddle, new System.EventArgs());
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public ContentAlignment LabelTextAlignMiddlePanel
        {
            get
            {
                return this.lblPanelMiddle.TextAlign;
            }

            set
            {
                this.lblPanelMiddle.TextAlign = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public string LabelTextMiddlePanel
        {
            get
            {
                return this.lblPanelMiddle.Text;
            }

            set
            {
                this.lblPanelMiddle.Text = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public int RightLabelMargin
        {
            get
            {
                return this.FRightLabelMargin;
            }

            set
            {
                this.FRightLabelMargin = value;
                this.TxtPanelMiddle_Layout(this.lblPanelMiddle, new System.Windows.Forms.LayoutEventArgs(this.txtPanelMiddle, "Location"));
                this.TxtPanelMiddle_Layout(this.lblPanelMiddle, new System.Windows.Forms.LayoutEventArgs(this.txtPanelMiddle, "Size"));
                this.TxtPanelLower_Layout(this.lblPanelLower, new System.Windows.Forms.LayoutEventArgs(this.txtPanelLower, "Location"));
                this.TxtPanelLower_Layout(this.lblPanelLower, new System.Windows.Forms.LayoutEventArgs(this.txtPanelLower, "Size"));
            }
        }

        /// <summary>
        /// The PanelVisibleLower determines whether the middle panel is visible or not.
        ///
        /// </summary>
        public bool PanelVisibleLower
        {
            get
            {
                return this.pnlPanelLower.Visible;
            }

            set
            {
                this.pnlPanelLower.Visible = value;
                this.OnResize(new System.EventArgs());
                this.OnLayout(new System.Windows.Forms.LayoutEventArgs(this.pnlCommonBackGround, "Controls"));
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public int LabelWidthLowerPanel
        {
            get
            {
                return this.lblPanelLower.Width;
            }

            set
            {
                this.lblPanelLower.Size = new System.Drawing.Size(value, this.lblPanelLower.Height);
                this.PnlPanelLower_Resize(this.lblPanelLower, new System.EventArgs());
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public ContentAlignment LabelTextAlignLowerPanel
        {
            get
            {
                return this.lblPanelLower.TextAlign;
            }

            set
            {
                this.lblPanelLower.TextAlign = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public string LabelTextLowerPanel
        {
            get
            {
                return this.lblPanelLower.Text;
            }

            set
            {
                this.lblPanelLower.Text = value;
            }
        }

        /// <summary>
        /// The PanelVisibleButton determines whether the middle panel is visible or not.
        ///
        /// </summary>
        public bool PanelVisibleButton
        {
            get
            {
                return this.pnlButtonPanel.Visible;
            }

            set
            {
                this.pnlButtonPanel.Visible = value;
                this.OnResize(new System.EventArgs());
                this.OnLayout(new System.Windows.Forms.LayoutEventArgs(this.pnlCommonBackGround, "Controls"));
            }
        }

        /// <summary>
        /// The SelectWithEnter determines whether the resulting search string is built
        /// and sent using the Enter Key after entering the search critera
        ///
        /// </summary>
        public bool SelectWithEnter
        {
            get
            {
                return this.FSelectWithEnter;
            }

            set
            {
                this.FSelectWithEnter = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public bool PreventCollapsing
        {
            get
            {
                return this.FPreventCollapsing;
            }

            set
            {
                this.FPreventCollapsing = value;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public bool Collapsed
        {
            get
            {
                return this.FCollapsed;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public int MinimalWidth
        {
            get
            {
                return this.FMinWidth;
            }
        }

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public event TSearchStringIssued SearchStringIssued;

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public event TTextBoxTextDeleting TextBoxTextDeleting;

        /// <summary>
        /// The PanelVisibleUpper determines whether the upper panel is visible or not.
        ///
        /// </summary>
        public event TTextBoxTextDeleted TextBoxTextDeleted;

        #region Creation and Disposal

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TUC_CollapsibleSearchPane() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnResetSearch.Text = Catalog.GetString("Reset Search");
            this.btnInitiateSearch.Text = Catalog.GetString("&Search");
            #endregion

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.InitializeUserControl();
            PanelVisibleUpper = true;
            LabelWidthUpperPanel = 150;
            LabelTextAlignUpperPanel = ContentAlignment.MiddleCenter;
            PanelVisibleMiddle = true;
            LabelWidthMiddlePanel = 150;
            LabelTextAlignMiddlePanel = ContentAlignment.MiddleRight;
            PanelVisibleLower = true;
            LabelWidthLowerPanel = 150;
            LabelTextAlignLowerPanel = ContentAlignment.MiddleRight;
            PanelVisibleButton = true;
            SelectWithEnter = true;
            PreventCollapsing = false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        protected int GetBackgroundPanelHeight()
        {
            int mHeight;

            mHeight = 0;

            if (this.pnlPanelUpper.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlPanelMiddle.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlPanelLower.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlButtonPanel.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            return mHeight;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void InitializeUserControl()
        {
            this.SetConstants();
            this.SetUpLabels();
            this.SetUpTextBoxes();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetConstants()
        {
            this.FSelectWithEnter = true;
            this.FRightLabelMargin = TUC_CollapsibleSearchPane.constRightLabelMargin;
            this.FPanelMarginTop = TUC_CollapsibleSearchPane.constPanelMarginTop;
            this.FPanelMarginBottom = TUC_CollapsibleSearchPane.constPanelMarginBottom;
            this.FPanelMarginLeft = TUC_CollapsibleSearchPane.constPanelMarginLeft;
            this.FPanelMarginRight = TUC_CollapsibleSearchPane.constPanelMarginRight;
            this.FControlHeight = TUC_CollapsibleSearchPane.constControlHeight;
            this.SetMinimalWidth();
            this.FMinHeight = TUC_CollapsibleSearchPane.constPanelMarginTop + TUC_CollapsibleSearchPane.constPanelMarginBottom;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetMinimalHeight()
        {
            int mHeight;

            mHeight = TUC_CollapsibleSearchPane.constPanelMarginTop + TUC_CollapsibleSearchPane.constPanelMarginBottom;

            if (this.pnlPanelUpper.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlPanelMiddle.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlPanelLower.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            if (this.pnlButtonPanel.Visible == true)
            {
                mHeight = mHeight + this.FControlHeight;
            }

            this.FMinHeight = mHeight;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetMinimalWidth()
        {
            this.FMinWidth = TUC_CollapsibleSearchPane.constPanelMarginLeft +
                             TUC_CollapsibleSearchPane.constPanelMarginRight +
                             TUC_CollapsibleSearchPane.constDefaultButtonWidth +
                             TUC_CollapsibleSearchPane.constRightLabelMargin +
                             TUC_CollapsibleSearchPane.constDefaultButtonWidth;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpLabels()
        {
            this.SetUpLabelUpperPanel();
            this.SetUpLabelMiddlePanel();
            this.SetUpLabelLowerPanel();
            this.SetUpButtonPanel();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpLabelUpperPanel()
        {
            this.lblPanelUpper = new System.Windows.Forms.Label();
            this.lblPanelUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPanelUpper.Location = new System.Drawing.Point(0, 0);
            this.lblPanelUpper.Name = "lblPanelUpper";
            this.lblPanelUpper.Text = "lblPanelUpper";
            this.lblPanelUpper.BackColor = System.Drawing.SystemColors.Control;
            this.lblPanelUpper.TextAlign = ContentAlignment.MiddleCenter;
            this.pnlPanelUpper.Controls.Add(this.lblPanelUpper);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpLabelMiddlePanel()
        {
            this.lblPanelMiddle = new System.Windows.Forms.Label();
            this.lblPanelMiddle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPanelMiddle.Location = new System.Drawing.Point(0, 0);
            this.lblPanelMiddle.Name = "lblPanelMiddle";
            this.lblPanelMiddle.Text = "lblPanelMiddle";
            this.lblPanelMiddle.BackColor = System.Drawing.SystemColors.Control;
            this.lblPanelMiddle.TextAlign = ContentAlignment.MiddleRight;

            // if (this.lblPanelMiddle.Width < this.constDefaultLabelWidth) then
            // begin
            // this.lblPanelMiddle.Size := new System.Drawing.Size(this.constDefaultLabelWidth, this.lblPanelMiddle.Height);
            // end;
            this.pnlPanelMiddle.Controls.Add(this.lblPanelMiddle);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpLabelLowerPanel()
        {
            this.lblPanelLower = new System.Windows.Forms.Label();
            this.lblPanelLower.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPanelLower.Location = new System.Drawing.Point(0, 0);
            this.lblPanelLower.Name = "lblPanelLower";
            this.lblPanelLower.Text = "lblPanelLower";
            this.lblPanelLower.BackColor = System.Drawing.SystemColors.Control;
            this.lblPanelLower.TextAlign = ContentAlignment.MiddleRight;

            // if (this.lblPanelLower.Width < this.constDefaultLabelWidth) then
            // begin
            // this.lblPanelLower.Size := new System.Drawing.Size(this.constDefaultLabelWidth, this.lblPanelLower.Height);
            // end;
            this.pnlPanelLower.Controls.Add(this.lblPanelLower);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpButtonPanel()
        {
            // Nothing to do at the moment
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpTextBoxes()
        {
            this.SetUpTextBoxMiddlePanel();
            this.SetUpTextBoxLowerPanel();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APanel"></param>
        /// <param name="ALabel"></param>
        /// <returns></returns>
        protected System.Drawing.Point GetTextBoxLocation(System.Windows.Forms.Panel APanel, System.Windows.Forms.Label ALabel)
        {
            int mXCoord;

            mXCoord = ALabel.Width + this.FRightLabelMargin;
            return new System.Drawing.Point(mXCoord, TUC_CollapsibleSearchPane.constDefaultTextBoxY);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APanel"></param>
        /// <param name="ALabel"></param>
        /// <returns></returns>
        protected System.Drawing.Size GetTextBoxSize(System.Windows.Forms.Panel APanel, System.Windows.Forms.Label ALabel)
        {
            int mXCoord;
            int mTextBoxWidth;

            mXCoord = ALabel.Width + this.FRightLabelMargin;
            mTextBoxWidth = APanel.Width - mXCoord;

            // System.Drawing.Point mTextBoxLocation = new System.Drawing.Point(mXCoord, TUC_CollapsibleSearchPane.constDefaultTextBoxY);

            return new System.Drawing.Size(mTextBoxWidth, TUC_CollapsibleSearchPane.constControlHeight);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpTextBoxMiddlePanel()
        {
            this.txtPanelMiddle = new System.Windows.Forms.TextBox();
            this.txtPanelMiddle.Location = this.GetTextBoxLocation(this.pnlPanelMiddle, this.lblPanelMiddle);
            this.txtPanelMiddle.Size = this.GetTextBoxSize(this.pnlPanelMiddle, this.lblPanelMiddle);
            this.txtPanelMiddle.Dock = System.Windows.Forms.DockStyle.None;
            this.txtPanelMiddle.Name = "txtPanelMiddle";
            this.txtPanelMiddle.Text = "txtPanelMiddle";
            this.txtPanelMiddle.Layout += new LayoutEventHandler(this.TxtPanelMiddle_Layout);
            this.txtPanelMiddle.KeyUp += new KeyEventHandler(this.TxtPanelMiddle_KeyUp);
            this.pnlPanelMiddle.Controls.Add(this.txtPanelMiddle);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void SetUpTextBoxLowerPanel()
        {
            this.txtPanelLower = new System.Windows.Forms.TextBox();
            this.txtPanelLower.Location = this.GetTextBoxLocation(this.pnlPanelLower, this.lblPanelLower);
            this.txtPanelLower.Size = this.GetTextBoxSize(this.pnlPanelLower, this.lblPanelLower);
            this.txtPanelLower.Dock = System.Windows.Forms.DockStyle.None;
            this.txtPanelLower.Name = "txtPanelLower";
            this.txtPanelLower.Text = "txtPanelLower";
            this.txtPanelLower.Layout += new LayoutEventHandler(this.TxtPanelLower_Layout);
            this.txtPanelLower.KeyUp += new KeyEventHandler(this.TxtPanelLower_KeyUp);
            this.pnlPanelLower.Controls.Add(this.txtPanelLower);
        }

        #endregion

        #region Resizing Events
        private void PnlPanelUpper_Resize(System.Object sender, System.EventArgs e)
        {
            this.pnlPanelUpper.Size = new System.Drawing.Size(this.pnlCommonBackGround.Width, TUC_CollapsibleSearchPane.constControlHeight);
        }

        private void PnlPanelMiddle_Resize(System.Object sender, System.EventArgs e)
        {
            // messagebox.Show('pnlPanelMiddle_Resize');
            this.pnlPanelMiddle.Size = new System.Drawing.Size(this.pnlCommonBackGround.Width, TUC_CollapsibleSearchPane.constControlHeight);
            this.txtPanelMiddle.Location = this.GetTextBoxLocation(this.pnlPanelMiddle, this.lblPanelMiddle);
            this.txtPanelMiddle.Size = this.GetTextBoxSize(this.pnlPanelMiddle, this.lblPanelMiddle);
        }

        private void PnlPanelLower_Resize(System.Object sender, System.EventArgs e)
        {
            // messagebox.Show('pnlPanelLower_Resize');
            this.pnlPanelLower.Size = new System.Drawing.Size(this.pnlCommonBackGround.Width, TUC_CollapsibleSearchPane.constControlHeight);
            this.txtPanelLower.Location = this.GetTextBoxLocation(this.pnlPanelLower, this.lblPanelLower);
            this.txtPanelLower.Size = this.GetTextBoxSize(this.pnlPanelLower, this.lblPanelLower);
        }

        private void PnlButtonPanel_Resize(System.Object sender, System.EventArgs e)
        {
            // messagebox.Show('pnlButtonPanel_Resize');
            this.pnlButtonPanel.Size = new System.Drawing.Size(this.pnlCommonBackGround.Width, TUC_CollapsibleSearchPane.constControlHeight);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            int mWidth;
            int mHeight;
            int mBackgroundPanelWidth;
            int mBackgroundPanelHeight;

            System.Drawing.Point mBackgroundPanelLocation;

            // Get new size
            mWidth = this.Width;
            mHeight = this.Height;

            // Check size
            if ((mWidth < this.FMinWidth) || (mHeight < this.FMinHeight))
            {
                this.pnlCommonBackGround.Visible = false;
            }
            else
            {
                // Background Panel Size and Location
                mBackgroundPanelLocation = new System.Drawing.Point(this.FPanelMarginLeft, this.FPanelMarginTop);
                this.pnlCommonBackGround.Location = mBackgroundPanelLocation;
                mBackgroundPanelWidth = this.Width - this.FPanelMarginLeft - this.FPanelMarginRight;
                mBackgroundPanelHeight = this.GetBackgroundPanelHeight();
                this.pnlCommonBackGround.Size = new System.Drawing.Size(mBackgroundPanelWidth, mBackgroundPanelHeight);
                this.pnlCommonBackGround.Visible = true;

                // Control Size and Location
                if (this.FCollapsing == false)
                {
                    mHeight = this.FPanelMarginTop + this.FPanelMarginBottom + mBackgroundPanelHeight;
                    this.Size = new System.Drawing.Size(this.Width, mHeight);
                }
            }

            if (mWidth < this.FMinWidth)
            {
                this.Size = new System.Drawing.Size(this.FMinWidth, this.Height);
            }

            this.FCollapsed = this.FCollapsing;
        }

        #endregion

        #region Other Events
        private void TUC_CollapsibleSearchPane_CollapsingEvent(System.Object sender, CollapsibleEventArgs args)
        {
            if (this.FPreventCollapsing == true)
            {
                args.Cancel = true;
                this.FCollapsing = false;
            }
            else
            {
                this.FCollapsing = args.WillCollapse;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtPanelMiddle_Layout(System.Object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            System.Drawing.Point mTextBoxLocation;
            System.Drawing.Size mTextBoxSize;

            // Messagebox.Show('Size: ' + this.pnlPanelMiddle.Size.ToString);
            mTextBoxLocation = new System.Drawing.Point(0, 0);
            mTextBoxSize = new System.Drawing.Size(0, 0);
            mTextBoxLocation = this.GetTextBoxLocation(pnlPanelMiddle, lblPanelMiddle);
            this.txtPanelMiddle.Location = mTextBoxLocation;
            mTextBoxSize = this.GetTextBoxSize(pnlPanelMiddle, lblPanelMiddle);
            this.txtPanelMiddle.Size = mTextBoxSize;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtPanelLower_Layout(System.Object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            System.Drawing.Point mTextBoxLocation;
            System.Drawing.Size mTextBoxSize;

            // Messagebox.Show('Size: ' + this.pnlPanelMiddle.Size.ToString);
            mTextBoxLocation = new System.Drawing.Point(0, 0);
            mTextBoxSize = new System.Drawing.Size(0, 0);
            mTextBoxLocation = this.GetTextBoxLocation(pnlPanelLower, lblPanelLower);
            this.txtPanelLower.Location = mTextBoxLocation;
            mTextBoxSize = this.GetTextBoxSize(pnlPanelLower, lblPanelLower);
            this.txtPanelLower.Size = mTextBoxSize;
        }

        #endregion

        #region Getting a search string from the control
        private void BtnInitiateSearch_Click(System.Object sender, System.EventArgs e)
        {
            this.GetSearchString();
        }

        private void BtnResetSearch_Click(System.Object sender, System.EventArgs e)
        {
            TDeletingTextBoxTextEventArgs mArgs;
            String mDataMemberMiddle;
            String mTextMiddle;
            String mDataMemberLower;
            String mTextLower;
            String mResult;

            mArgs = new TDeletingTextBoxTextEventArgs();
            this.BuildSearchString(
                out mDataMemberMiddle,
                out mTextMiddle,
                out mDataMemberLower,
                out mTextLower,
                out mResult);

            // messagebox.show('btnResetSearch_Click: ' + this.ClassName);
            mArgs.FDataMemberTextBoxMiddlePanel = mDataMemberMiddle;
            mArgs.FTextTextBoxMiddlePanel = mTextMiddle;
            mArgs.FDataMemberTextBoxLowerPanel = mDataMemberLower;
            mArgs.FTextTextBoxLowerPanel = mTextLower;
            mArgs.Handled = false;
            this.OnTextBoxTextDeleting(this, out mArgs);

            if (mArgs.Handled == false)
            {
                this.txtPanelMiddle.Text = "";
                this.txtPanelLower.Text = "";
            }

            this.OnTextBoxTextDeleted(sender, mArgs);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtPanelMiddle_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((this.FSelectWithEnter == true) && (this.txtPanelMiddle.Focused == true))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.pnlPanelLower.Visible == true)
                    {
                        this.txtPanelLower.Focus();
                    }
                    else
                    {
                        this.GetSearchString();
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TxtPanelLower_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((this.FSelectWithEnter == true) && (this.txtPanelLower.Focused == true))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.GetSearchString();
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataMemberMiddleTextBox"></param>
        /// <param name="ATextMiddleTextBox"></param>
        /// <param name="ADataMemberLowerTextBox"></param>
        /// <param name="ATextLowerTextBox"></param>
        /// <param name="AProposedSearchString"></param>
        public void BuildSearchString(out String ADataMemberMiddleTextBox,
            out String ATextMiddleTextBox,
            out String ADataMemberLowerTextBox,
            out String ATextLowerTextBox,
            out String AProposedSearchString)
        {
            String mSearchString1;
            String mSearchString2;

            ADataMemberMiddleTextBox = "";
            ATextMiddleTextBox = "";
            ADataMemberLowerTextBox = "";
            ATextLowerTextBox = "";
            AProposedSearchString = "";

            /*
             * Just a short reminder what the AProposedSearchString parameter is about. This
             * parameter provides a RowFilter string for a DataView.
             * Multiple conditions are imposed like this:
             * dv.RowFilter = "Emp_Type = 'Temporary' AND IsNull(Departure, '01/01/1900') = '01/01/1900'"
             */

            // If not not databound it is hard to tell what the search string might be
            if ((this.FDataBindingTextboxMiddlePanel == null) || (this.FDataBindingTextboxLowerPanel == null))
            {
                ADataMemberMiddleTextBox = this.FDataMemberTextBoxMiddlePanel;
                ATextMiddleTextBox = this.txtPanelMiddle.Text;
                ADataMemberLowerTextBox = this.FDataMemberTextBoxLowerPanel;
                ATextLowerTextBox = this.txtPanelLower.Text;
                return;
            }

            // If some panels are visible we might have to provide more information
            if ((this.pnlPanelMiddle.Visible == true) && (this.pnlPanelLower.Visible == true))
            {
                ADataMemberMiddleTextBox = this.FDataMemberTextBoxMiddlePanel;
                ATextMiddleTextBox = this.txtPanelMiddle.Text;
                ADataMemberLowerTextBox = this.FDataMemberTextBoxLowerPanel;
                ATextLowerTextBox = this.txtPanelLower.Text;
                mSearchString1 = ADataMemberMiddleTextBox +
                                 TUC_CollapsibleSearchPane.constEquals +
                                 TUC_CollapsibleSearchPane.constSingleQuote + ATextMiddleTextBox +
                                 TUC_CollapsibleSearchPane.constSingleQuote;
                mSearchString2 = ADataMemberLowerTextBox +
                                 TUC_CollapsibleSearchPane.constEquals +
                                 TUC_CollapsibleSearchPane.constSingleQuote + ATextLowerTextBox +
                                 TUC_CollapsibleSearchPane.constSingleQuote;
                AProposedSearchString = mSearchString1 +
                                        TUC_CollapsibleSearchPane.constAND + mSearchString2;
            }
            else if ((this.pnlPanelMiddle.Visible == false) && (this.pnlPanelLower.Visible == true))
            {
                ADataMemberLowerTextBox = this.FDataMemberTextBoxLowerPanel;
                ATextLowerTextBox = this.txtPanelLower.Text;
                mSearchString2 = ADataMemberLowerTextBox +
                                 TUC_CollapsibleSearchPane.constEquals +
                                 TUC_CollapsibleSearchPane.constSingleQuote +
                                 ATextLowerTextBox + TUC_CollapsibleSearchPane.constSingleQuote;
                AProposedSearchString = mSearchString2;
            }
            else if ((this.pnlPanelMiddle.Visible == true) && (this.pnlPanelLower.Visible == false))
            {
                ADataMemberMiddleTextBox = this.FDataMemberTextBoxMiddlePanel;
                ATextMiddleTextBox = this.txtPanelMiddle.Text;
                mSearchString1 = ADataMemberMiddleTextBox +
                                 TUC_CollapsibleSearchPane.constEquals +
                                 TUC_CollapsibleSearchPane.constSingleQuote + ATextMiddleTextBox + TUC_CollapsibleSearchPane.constSingleQuote;
                AProposedSearchString = mSearchString1;
            }
            else if ((this.pnlPanelMiddle.Visible == false) && (this.pnlPanelLower.Visible == false))
            {
                AProposedSearchString = "";
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSearchStringIssued(System.Object sender, TSearchStringIssuedEventArgs e)
        {
            if (SearchStringIssued != null)
            {
                this.SearchStringIssued(this.components, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnTextBoxTextDeleted(System.Object sender, TDeletingTextBoxTextEventArgs e)
        {
            if (TextBoxTextDeleted != null)
            {
                this.TextBoxTextDeleted(this.components, e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnTextBoxTextDeleting(System.Object sender, out TDeletingTextBoxTextEventArgs e)
        {
            e = null;

            if (TextBoxTextDeleting != null)
            {
                this.TextBoxTextDeleting(this.components, out e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public string GetSearchString()
        {
            TSearchStringIssuedEventArgs mArgs;
            String mDataMemberMiddle;
            String mTextMiddle;
            String mDataMemberLower;
            String mTextLower;
            String SearchString;

            mArgs = new TSearchStringIssuedEventArgs();
            this.BuildSearchString(out mDataMemberMiddle, out mTextMiddle, out mDataMemberLower, out mTextLower,
                out SearchString);
            mArgs.FDataMemberTextBoxMiddlePanel = mDataMemberMiddle;
            mArgs.FTextTextBoxMiddlePanel = mTextMiddle;
            mArgs.FDataMemberTextBoxLowerPanel = mDataMemberLower;
            mArgs.FTextTextBoxLowerPanel = mTextLower;
            mArgs.FSearchString = SearchString;
            this.OnSearchStringIssued(this, mArgs);
            return SearchString;
        }

        #endregion

        #region Perform DataBinding

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataSource"></param>
        /// <param name="ADataMemberTextBoxMiddlePanel"></param>
        /// <param name="ADataMemberTextBoxLowerPanel"></param>
        public void PerformDataBindingTextBoxes(System.Data.DataView ADataSource,
            String ADataMemberTextBoxMiddlePanel,
            String ADataMemberTextBoxLowerPanel)
        {
            this.FDataMemberTextBoxMiddlePanel = ADataMemberTextBoxMiddlePanel;
            this.PerformDataBindingTextBoxMiddlePanel(ADataSource, ADataMemberTextBoxMiddlePanel);
            this.FDataMemberTextBoxLowerPanel = ADataMemberTextBoxLowerPanel;
            this.PerformDataBindingTextBoxLowerPanel(ADataSource, ADataMemberTextBoxLowerPanel);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataSource"></param>
        /// <param name="ADataMember"></param>
        /// <returns></returns>
        public System.Windows.Forms.Binding PerformDataBindingTextBoxLowerPanel(System.Data.DataView ADataSource, String ADataMember)
        {
            this.FDataBindingTextboxLowerPanel = new System.Windows.Forms.Binding("Text", ADataSource, ADataMember);
            return this.FDataBindingTextboxLowerPanel;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADataSource"></param>
        /// <param name="ADataMember"></param>
        /// <returns></returns>
        public System.Windows.Forms.Binding PerformDataBindingTextBoxMiddlePanel(System.Data.DataView ADataSource, String ADataMember)
        {
            this.FDataBindingTextboxMiddlePanel = new System.Windows.Forms.Binding("Text", ADataSource, ADataMember);
            return this.FDataBindingTextboxMiddlePanel;
        }

        #endregion
    }


    /// <summary>todoComment</summary>
    public delegate void TSearchStringIssued(System.Object sender, TSearchStringIssuedEventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TTextBoxTextDeleting(System.Object sender, out TDeletingTextBoxTextEventArgs e);

    /// <summary>todoComment</summary>
    public delegate void TTextBoxTextDeleted(System.Object sender, TDeletingTextBoxTextEventArgs e);


    /// <summary>
    /// Events
    /// </summary>
    public class TSearchStringIssuedEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String FDataMemberTextBoxMiddlePanel;

        /// <summary>todoComment</summary>
        public String FTextTextBoxMiddlePanel;

        /// <summary>todoComment</summary>
        public String FDataMemberTextBoxLowerPanel;

        /// <summary>todoComment</summary>
        public String FTextTextBoxLowerPanel;

        /// <summary>todoComment</summary>
        public String FSearchString;

        /// <summary>todoComment</summary>
        public String DataMemberTextBoxMiddlePanel
        {
            get
            {
                return FDataMemberTextBoxMiddlePanel;
            }

            set
            {
                FDataMemberTextBoxMiddlePanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String TextTextBoxMiddlePanel
        {
            get
            {
                return FTextTextBoxMiddlePanel;
            }

            set
            {
                FTextTextBoxMiddlePanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String DataMemberTextBoxLowerPanel
        {
            get
            {
                return FDataMemberTextBoxLowerPanel;
            }

            set
            {
                FDataMemberTextBoxLowerPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String TextTextBoxLowerPanel
        {
            get
            {
                return FTextTextBoxLowerPanel;
            }

            set
            {
                FTextTextBoxLowerPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String SearchString
        {
            get
            {
                return FSearchString;
            }

            set
            {
                FSearchString = value;
            }
        }
    }

    /// <summary>todoComment</summary>
    public class TDeletingTextBoxTextEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String FDataMemberTextBoxMiddlePanel;

        /// <summary>todoComment</summary>
        public String FTextTextBoxMiddlePanel;

        /// <summary>todoComment</summary>
        public String FDataMemberTextBoxLowerPanel;

        /// <summary>todoComment</summary>
        public String FTextTextBoxLowerPanel;

        /// <summary>todoComment</summary>
        public bool FHandled;

        /// <summary>todoComment</summary>
        public String DataMemberTextBoxMiddlePanel
        {
            get
            {
                return FDataMemberTextBoxMiddlePanel;
            }

            set
            {
                FDataMemberTextBoxMiddlePanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String TextTextBoxMiddlePanel
        {
            get
            {
                return FTextTextBoxMiddlePanel;
            }

            set
            {
                FTextTextBoxMiddlePanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String DataMemberTextBoxLowerPanel
        {
            get
            {
                return FDataMemberTextBoxLowerPanel;
            }

            set
            {
                FDataMemberTextBoxLowerPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public String TextTextBoxLowerPanel
        {
            get
            {
                return FTextTextBoxLowerPanel;
            }

            set
            {
                FTextTextBoxLowerPanel = value;
            }
        }

        /// <summary>todoComment</summary>
        public bool Handled
        {
            get
            {
                return FHandled;
            }

            set
            {
                FHandled = value;
            }
        }
    }
}