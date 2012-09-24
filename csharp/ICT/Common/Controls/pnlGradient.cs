//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//		 chadds
//		 ashleyc
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
    /// this panel has a gradient background
    /// </summary>
    public partial class TPnlGradient : System.Windows.Forms.Panel
    {
        private Color InternalGradientColorTop = Color.FromArgb(0xEF, 0xFB, 0xFF);
        /// see variable name
        public Color GradientColorTop {
            get
            {
                return InternalGradientColorTop;
            }
            set
            {
                InternalGradientColorTop = value;
            }
        }


        private Color InternalGradientColorBottom = Color.FromArgb(0xB5, 0xCB, 0xE7);
        /// see variable name
        public Color GradientColorBottom {
            get
            {
                return InternalGradientColorBottom;
            }
            set
            {
                InternalGradientColorBottom = value;
            }
        }


        private Pen InternalBorder = Pens.Transparent;
        /// <summary>
        /// color for the border; if transparent, no border is drawn
        /// </summary>
        public Pen Border {
            get
            {
                return InternalBorder;
            }
            set
            {
                InternalBorder = value;
            }
        }


        private bool InternalDontDrawBottomLine = false;
        /// <summary>
        /// sometimes, when stacking several panels on top of each other, you don't want bottom and top line to result in 2 lines
        /// </summary>
        public bool DontDrawBottomLine {
            get
            {
                return InternalDontDrawBottomLine;
            }
            set
            {
                InternalDontDrawBottomLine = value;
            }
        }


        private LinearGradientMode InternalGradientMode = LinearGradientMode.Vertical;
        /// <summary>
        /// direction of gradient
        /// </summary>
        public LinearGradientMode GradientMode {
            get
            {
                return InternalGradientMode;
            }
            set
            {
                InternalGradientMode = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPnlGradient()
        {
            DoubleBuffered = true;
        }

        /// <summary>
        /// overwrite OnPaint for the gradient background color
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle BaseRectangle = new Rectangle(1, 1, this.Width - 2, this.Height - 2);

            if (DontDrawBottomLine)
            {
                BaseRectangle = new Rectangle(1, 1, this.Width - 2, this.Height - 1);
            }

            if (Border != Pens.Transparent)
            {
                BaseRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            }

            Brush Gradient_Brush =
                new LinearGradientBrush(
                    BaseRectangle,
                    GradientColorTop, GradientColorBottom,
                    GradientMode);

            if (Border != Pens.Transparent)
            {
                e.Graphics.DrawRectangle(Border, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }

            e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);
        }
    }
}