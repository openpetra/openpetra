/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       markusm
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
using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This unit provides a Label that adjusts its width to the text it displays.
    /// The AdjustWidth property handles the behaviour of this Label. If the property
    /// is set to true then the Label adjusts its width to the text it displays. If it
    /// is set to false then it behaves like a normal Label.
    /// </summary>
    public class TLblVarioText : System.Windows.Forms.Label
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// should the label adjust its width to the length of the text it displays
        /// </summary>
        public bool FAdjustWidth;

        /// <summary>
        /// Property Methods
        /// This property determines whether the button resizes to the text length or not.
        ///
        /// </summary>
        public bool AdjustWidth
        {
            get
            {
                return this.FAdjustWidth;
            }

            set
            {
                this.FAdjustWidth = value;

                if (value == true)
                {
                    this.AlterWidth();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// This property sets or gets the size of the control in pixels..
        ///
        /// </summary>
        public new System.Drawing.Size Size
        {
            get
            {
                return base.Size;
            }

            set
            {
                if (this.FAdjustWidth == true)
                {
                    this.AlterWidth();
                    this.Invalidate();
                }
                else
                {
                    base.Size = value;
                }
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> Properties itself Property stuff <summary> Required method for Designer support  do not modify the contents of this
        /// method with the code editor. </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        #endregion

        #region Creation and Disposal

        /// <summary>
        /// Private Declarations }
        /// The constructor of this class
        /// }{*******************************************************************************
        /// The constructor of this class
        ///
        /// </summary>
        /// <returns>void</returns>
        TLblVarioText() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            this.FAdjustWidth = true;
            InitializeComponent();
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        /*
         * This procedure adjusts the width of the button.
         */

        /// <summary>
        /// This procedure adjusts the width of the button.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void AlterWidth()
        {
            System.Drawing.Graphics mGraphics;
            double mButtonWidth;
            mGraphics = this.CreateGraphics();
            mButtonWidth = mGraphics.MeasureString(this.Text, this.Font).Width;
            mGraphics.Dispose();

            // could add 12; 12 is for the margins
            this.Width = System.Convert.ToInt32(mButtonWidth);
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// The destructor of this class
        ///
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

        #endregion

        #region Events

        /// <summary>
        /// This event is called when the text of the button changes.
        /// }{*******************************************************************************
        /// This event is called when the text of the button changes.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected override void OnTextChanged(System.EventArgs e)
        {
            if (this.FAdjustWidth == true)
            {
                AlterWidth();
                base.OnTextChanged(e);
                this.Invalidate();
            }
            else
            {
                base.OnTextChanged(e);
            }
        }

        #endregion
    }
}