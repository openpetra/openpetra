//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This special statusbar will monitor the active control
    /// and displays the help text for the active control in the statusbar
    /// see also http://msdn.microsoft.com/en-us/library/ms229066.aspx
    /// and http://www.vb-helper.com/howto_net_focus_status.html
    /// </summary>
    public class TExtStatusBarHelp : System.Windows.Forms.StatusStrip, IExtenderProvider
    {
        private Hashtable FControlTexts;
        private System.Windows.Forms.Control FActiveControl;
        private System.Windows.Forms.ToolStripStatusLabel FStatusLabel;
        private bool FUseOpenPetraToolStripRenderer = false;

        /// <summary>
        /// constructor
        /// </summary>
        public TExtStatusBarHelp()
        {
            this.FStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SuspendLayout();

            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.FStatusLabel
                });

            this.FStatusLabel.Name = "FStatusLabel";
            this.FStatusLabel.AutoSize = true;
            this.FStatusLabel.Spring = true;
            this.FStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.ResumeLayout(false);
            this.PerformLayout();

            FControlTexts = new Hashtable();
        }

        bool IExtenderProvider.CanExtend(object target)
        {
            if (target is Control
                && !(target is TExtStatusBarHelp))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Whehter the StatusBar should appear 'OpenPetra-styled'.
        /// </summary>
        public bool UseOpenPetraToolStripRenderer
        {
            get
            {
                return FUseOpenPetraToolStripRenderer;
            }

            set
            {
                FUseOpenPetraToolStripRenderer = value;

                if (value == true)
                {
                    this.Renderer = new TOpenPetraToolStripRenderer(new TOpenPetraMenuColours());
                }
                else
                {
                    this.Renderer = new System.Windows.Forms.ToolStripProfessionalRenderer();
                }
            }
        }

        //
        // <doc>
        // <desc>
        //      This is an event handler that responds to the OnControlEnter
        //      event.  We attach this to each control we are providing help
        //      text for.
        // </desc>
        // </doc>
        //
        private void OnControlEnter(object sender, EventArgs e)
        {
            FActiveControl = (Control)sender;
            UpdateLabel();
        }

        //
        // <doc>
        // <desc>
        //      This is an event handler that responds to the OnControlLeave
        //      event.  We attach this to each control we are providing help
        //      text for.
        // </desc>
        // </doc>
        //
        private void OnControlLeave(object sender, EventArgs e)
        {
            if (sender == FActiveControl)
            {
                FActiveControl = null;
                UpdateLabel();
            }
        }

        /// <summary>
        /// add a control and the text that should be displayed in the statusbar when the control is focused
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        public void SetHelpText(Control control, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (value.Length == 0)
            {
                FControlTexts.Remove(control);

                if (control.Name.StartsWith("spt"))
                {
                    control.GotFocus -= new EventHandler(OnControlEnter);
                }
                else
                {
                    control.Enter -= new EventHandler(OnControlEnter);
                    control.Leave -= new EventHandler(OnControlLeave);
                }
            }
            else
            {
                FControlTexts[control] = value;

                if (control.Name.StartsWith("spt"))
                {
                    control.GotFocus += new EventHandler(OnControlEnter);
                }
                else
                {
                    control.Enter += new EventHandler(OnControlEnter);
                    control.Leave += new EventHandler(OnControlLeave);
                }
            }

            if (control == FActiveControl)
            {
                UpdateLabel();
            }
        }

        void UpdateLabel()
        {
            if (FActiveControl != null)
            {
                string text = (string)FControlTexts[FActiveControl];

                if ((text != null) && (text != this.FStatusLabel.Text))
                {
                    this.FStatusLabel.Text = text;
                }
            }
            else
            {
                this.FStatusLabel.Text = "";
            }
        }

        /// show a message in the status bar, independent of the selected control
        public void ShowMessage(string msg)
        {
            FStatusLabel.Text = msg;
        }

        /// <summary>
        /// OpenPetra-styled ToolStripRenderer (paints a vertical gradient instead of a horizontal one)
        /// </summary>
        private class TOpenPetraToolStripRenderer : System.Windows.Forms.ToolStripProfessionalRenderer
        {
            // Brush that paints the background of the GridStrip control (needed for vertical gradient).
            private Brush backgroundBrush = null;

            /// <summary>
            /// Constructor.
            /// </summary>
            public TOpenPetraToolStripRenderer() : base()
            {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AColorTable">The <see cref="System.Windows.Forms.ProfessionalColorTable" /> to initialise the Renderer with.</param>
            public TOpenPetraToolStripRenderer(ProfessionalColorTable AColorTable) : base(AColorTable)
            {
            }

            /// <summary>
            /// Raises the <see cref="System.Windows.Forms.ToolStripRenderer.RenderToolStripBackground" /> event.
            /// </summary>
            /// <param name="e">Provided by WinForms.</param>
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                base.OnRenderToolStripBackground(e);

                if (e.ToolStrip is MenuStrip || e.ToolStrip is StatusStrip)
                {
                    if (this.backgroundBrush == null)
                    {
                        this.backgroundBrush = new LinearGradientBrush(e.AffectedBounds,
                            this.ColorTable.ToolStripGradientBegin,
                            this.ColorTable.ToolStripGradientEnd,
                            LinearGradientMode.Vertical);
                    }

                    e.Graphics.FillRectangle(this.backgroundBrush, e.AffectedBounds);
                }
                else
                {
                    using (LinearGradientBrush b =
                               new LinearGradientBrush(e.AffectedBounds, this.ColorTable.ToolStripGradientBegin, this.ColorTable.ToolStripGradientEnd,
                                   LinearGradientMode.Horizontal))
                        e.Graphics.FillRectangle(b, e.AffectedBounds);
                }
            }
        }
    }
}