//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ict.Common.Controls
{
    /// <summary>
    /// this class shows a navigation button that can be checked, unchecked, and hovered over;
    /// it has an icon and a label;
    /// it has a gradient background
    /// </summary>
    public partial class TRbtNavigationButton : System.Windows.Forms.UserControl
    {
        /// see variable name
        public Color GradientColorTopUnchecked = Color.FromArgb(0xEF, 0xFB, 0xFF);

        /// see variable name
        public Color GradientColorBottomUnchecked = Color.FromArgb(0xB5, 0xCB, 0xE7);

        /// see variable name
        public Color GradientColorTopChecked = Color.FromArgb(0x9A, 0xB3, 0xDF);

        /// see variable name
        public Color GradientColorBottomChecked = Color.FromArgb(0x9A, 0xB3, 0xDF);

        /// see variable name
        public Color GradientColorTopHovering = Color.FromArgb(0xC5, 0xD4, 0xEF);

        /// see variable name
        public Color GradientColorBottomHovering = Color.FromArgb(0xC5, 0xD4, 0xEF);

        private bool FChecked = false;
        private bool FHovering = false;
        private CheckedChanging FCheckChangingDelegate = null;

        /// <summary>
        /// constructor
        /// </summary>
        public TRbtNavigationButton(CheckedChanging ACheckChangingDelegate)
        {
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion

            this.lblCaption.Enter += new System.EventHandler(this.PanelEnter);
            this.lblCaption.Leave += new System.EventHandler(this.PanelLeave);
            this.lblCaption.Click += new System.EventHandler(this.PanelClick);
            this.lblCaption.MouseLeave += new System.EventHandler(this.PanelLeave);
            this.lblCaption.MouseEnter += new System.EventHandler(this.PanelEnter);
            this.MouseLeave += new System.EventHandler(this.PanelLeave);
            this.MouseEnter += new System.EventHandler(this.PanelEnter);
            this.Enter += new System.EventHandler(this.PanelEnter);
            this.Leave += new System.EventHandler(this.PanelLeave);
            this.Click += new System.EventHandler(this.PanelClick);

            FCheckChangingDelegate = ACheckChangingDelegate;
        }

        /// Caption of the button
        public override string Text
        {
            get
            {
                return lblCaption.Text;
            }
            set
            {
                lblCaption.Text = value;
            }
        }

        /// <summary>
        /// checked state
        /// </summary>
        public bool Checked
        {
            get
            {
                return FChecked;
            }

            set
            {
                bool CheckChangeOK = true;

                if (FChecked != value)
                {
                    if (FCheckChangingDelegate != null)
                    {
                        CheckChangeOK = FCheckChangingDelegate(this);
                    }

                    if (CheckChangeOK)
                    {
                        FChecked = value;

                        if (FChecked)
                        {
                            // uncheck all other sibling controls of type TRbtNavigationButton
                            foreach (Control sibling in Parent.Controls)
                            {
                                if ((sibling.GetType() == typeof(TRbtNavigationButton)) && (sibling != this))
                                {
                                    ((TRbtNavigationButton)sibling).Checked = false;
                                }
                            }
                        }

                        Refresh();

                        if (CheckedChanged != null)
                        {
                            CheckedChanged(this, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// set the path of the icon that should be displayed on this button
        /// </summary>
        public string Icon
        {
            set
            {
                pbxIcon.Image = (new System.Drawing.Icon(value, 16, 16)).ToBitmap();
            }
        }

        /// <summary>
        /// Triggered when the Checked state is changing.
        /// </summary>
        public delegate bool CheckedChanging(TRbtNavigationButton ASender);

        /// <summary>
        /// Triggered when the Checked state has changed.
        /// </summary>
        public EventHandler CheckedChanged;

        /// <summary>
        /// overwrite OnPaint for the gradient background color
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Color GradientTop = GradientColorTopUnchecked;
            Color GradientBottom = GradientColorBottomUnchecked;

            if (FChecked)
            {
                GradientTop = GradientColorTopChecked;
                GradientBottom = GradientColorBottomChecked;
            }
            else if (FHovering)
            {
                GradientTop = GradientColorTopHovering;
                GradientBottom = GradientColorBottomHovering;
            }

            // don't show bottom line; otherwise there would be 2 pixel line between buttons;
            // the pnlMoreButtons below the buttons has a border as well
            Rectangle BaseRectangle = new Rectangle(1, 1, this.Width - 2, this.Height - 1);

            Brush Gradient_Brush =
                new LinearGradientBrush(
                    BaseRectangle,
                    GradientTop, GradientBottom,
                    LinearGradientMode.Vertical);

            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);
        }

        /// mouse is hovering over button
        protected void PanelEnter(object sender, EventArgs e)
        {
            if (!FHovering)
            {
                FHovering = true;
                Refresh();
            }
        }

        /// mouse moves away from button
        protected void PanelLeave(object sender, EventArgs e)
        {
            if (FHovering)
            {
                FHovering = false;
                Refresh();
            }
        }

        /// mouse got clicked over button
        protected void PanelClick(object sender, EventArgs e)
        {
            Checked = true;
        }
    }
}