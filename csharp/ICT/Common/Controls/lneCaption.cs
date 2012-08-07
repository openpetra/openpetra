//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Draws a line with a left-aligned (optional) text (=caption).
    /// </summary>
    public class TLneCaption : System.Windows.Forms.UserControl
    {
        private System.ComponentModel.Container FComponents = null;

        /// <summary>
        /// The colour of the line.
        /// </summary>
        private readonly System.Drawing.Color LINECOLOUR =
            System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));

        /// <summary>
        /// Caption (optional)
        /// </summary>
        private string FCaption = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        public TLneCaption()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //
            // TLneCaption
            //
            this.Name = "TLneCaption";
            this.Size = new System.Drawing.Size(200, this.Font.Height);
        }

        #endregion

        /// <summary>
        /// Caption (optional).
        /// </summary>
        public string Caption
        {
            get
            {
                return FCaption;
            }

            set
            {
                FCaption = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Cleanup of resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (FComponents != null)
                {
                    FComponents.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Fired on resizing of the Control.
        /// </summary>
        /// <param name="e">Provided by WinForms.</param>
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            this.Height = this.Font.Height + 2;
            this.Invalidate();
        }

        /// <summary>
        /// Fired on Font change.
        /// </summary>
        /// <param name="e">Provided by WinForms.</param>
        protected override void OnFontChanged(System.EventArgs e)
        {
            this.OnResize(e);
            base.OnFontChanged(e);
        }

        /// <summary>
        /// Fired on paiting of the Control. The main work of this Control occurs in here!
        /// </summary>
        /// <param name="e">Provided by WinForms.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            int VerticalPos = Convert.ToInt32(Math.Ceiling((double)Size.Height / 2)) - 1;

            SizeF CaptionSize = e.Graphics.MeasureString(Caption, this.Font, this.Width, StringFormat.GenericDefault);
            int CaptionLen = Convert.ToInt32(CaptionSize.Width);

            int Remainder;

            if (Caption == "")
            {
                Remainder = -1;
            }
            else
            {
                Remainder = -1 + CaptionLen;
            }

            e.Graphics.DrawLines(new Pen(LINECOLOUR, 1),
                new Point[] { new Point(0, VerticalPos + 1), new Point(0, VerticalPos), new Point(-1, VerticalPos) });

            e.Graphics.DrawLines(new Pen(LINECOLOUR, 1),
                new Point[] { new Point(Remainder, VerticalPos), new Point(this.Width, VerticalPos) });

            if (Caption != "")
            {
                e.Graphics.DrawString(Caption, this.Font, new SolidBrush(this.ForeColor), -2, -1);
            }
        }
    }
}