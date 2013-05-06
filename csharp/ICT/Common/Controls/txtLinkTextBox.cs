//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.ComponentModel;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// The TtxtLinkTextBox Control extends the TextBox Control with LinkType property which
    /// creates a clickable Hyperlink when user is not editing the text.
    /// </summary>
    public class TTxtLinkTextBox : TextBox
    {
        #region Fields

        private LinkLabel FLinkLabel;
        private TLinkTypes FLinkType = TLinkTypes.None;

        private bool FIsLinkClicked = false;

        #endregion

        #region Properties

        /// <summary>
        /// Type of Hyperlink.
        /// </summary>
        [DefaultValue(TLinkTypes.None)]
        public TLinkTypes LinkType
        {
            set
            {
                this.FLinkType = value;

                if (value == TLinkTypes.None)
                {
                    SwitchToEditMode(true);
                }
                else
                {
                    SwitchToEditMode(false);
                    FillLinkData();
                }
            }

            get
            {
                return this.FLinkType;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TTxtLinkTextBox()
        {
            // Create LinkLabel, add it to controls array,
            // position it so that it exactly overlaps
            // the text in the text box and
            // add event handler so we can correct strange tab behavior

            FLinkLabel = new LinkLabel();

            this.Controls.Add(FLinkLabel);

            FLinkLabel.AutoSize = true;
            FLinkLabel.Left = -1;
            FLinkLabel.Top = 1;
            FLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkLabel_LinkClicked);
            FLinkLabel.Visible = true;
            FLinkLabel.Text = this.Text;

            FLinkLabel.GotFocus += new EventHandler(LinkLabel_GotFocus);
            FLinkLabel.MouseDown += new MouseEventHandler(LinkLabel_MouseDown);
        }

        #endregion

        #region Focus overrides

        /// <summary>
        /// Event Handler for the GotFocus Event.
        /// </summary>
        /// <param name="e">Supplied by WinForms.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            // When control gets focus and we have active LinkType then switch to edit mode
            if (FLinkType != TLinkTypes.None)
            {
                this.SwitchToEditMode(true);
            }

        }

        /// <summary>
        /// Event Handler for the LostFocus Event.
        /// </summary>
        /// <param name="e">Supplied by WinForms.</param>
        protected override void OnLostFocus(System.EventArgs e)
        {
            base.OnLostFocus(e);

            // When control gets Focus and we have a hyperlink-LinkType then switch to clickable mode
            if (FLinkType != TLinkTypes.None)
            {
                this.SwitchToEditMode(false);
            }               
        }

        /// <summary>
        /// Event Handler for the TextChanged Event.
        /// </summary>
        /// <param name="e">Supplied by WinForms.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // When TextBox's Text changes, copy that data to the LinkLabel
            if (FLinkType != TLinkTypes.None)
            {
                FillLinkData();
            }
        }

        #endregion

        #region Click Handling

        /// <summary>
        /// Switch to Edit mode or to Clickable mode.
        /// </summary>
        /// <param name="AEditMode">Edit mode = true, Clickable mode = false</param>
        protected void SwitchToEditMode(bool AEditMode)
        {
            // Edit mode only means that LinkLabel is not visible
            FLinkLabel.Visible = !AEditMode;
        }

        /// <summary>
        /// Copy information from TextBox to LinkLabel.
        /// </summary>
        private void FillLinkData()
        {
            // Copy the text
            FLinkLabel.Text = this.Text;

            // Figure out if we need mailto: or http:// link
            string LinkType = "";

            switch (FLinkType)
            {
                case TLinkTypes.Http:

                    if ((this.Text.ToLower().IndexOf(@"http://") < 0) && (this.Text.ToLower().IndexOf(@"https://") < 0))
                    {
                        LinkType = @"http://";
                    }

                    break;

                case TLinkTypes.Ftp:

                    if (this.Text.ToLower().IndexOf(@"ftp://") < 0)
                    {
                        LinkType = @"ftp://";
                    }

                    break;

                case TLinkTypes.Email:

                    if (this.Text.ToLower().IndexOf("mailto:") < 0)
                    {
                        LinkType = "mailto:";
                    }

                    break;

                case TLinkTypes.Skype:
                    if (this.Text.ToLower().IndexOf("skype:") < 0)
                    {                    
                        LinkType = "skype:";
                    }
                    
                    break;
                    
            }

            // Clear old links and create a new one
            FLinkLabel.Links.Clear();
            FLinkLabel.Links.Add(0, FLinkLabel.Text.Length, LinkType + this.Text);
        }

        /// <summary>
        /// Try to "execute" supplied link. Throws ArgumentException if it fails.
        /// </summary>
        private void LaunchHyperlink()
        {
            string TheLink;
            
            try
            {
                if (FLinkLabel.Links.Count > 0)
                {
                    TheLink = FLinkLabel.Links[0].LinkData.ToString();
                    System.Diagnostics.Process.Start(TheLink);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Hyperlink cannot be launched!", ex);
            }
        }

        #endregion

        #region Link Activation

        /// <summary>
        /// Use the Hyperlink if user clicked on a LinkLabel.
        /// </summary>
        /// <param name="sender">Supplied by WinForms.</param>
        /// <param name="e">Supplied by WinForms.</param>
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (FLinkType != TLinkTypes.None)
            {
                LaunchHyperlink();
            }
        }

        /// <summary>
        /// If user clicked in the TextBox with Control key pressed, user HyperLink.
        /// </summary>
        /// <param name="e">Supplied by WinForms.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (FLinkType != TLinkTypes.None)
            {
                if ((e.Button == MouseButtons.Left) 
                    && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                {
                    LaunchHyperlink();
                }
                else
                {
                    base.OnMouseDown(e);
                }
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        #endregion

        #region Focus Handling

        private void LinkLabel_GotFocus(object sender, EventArgs e)
        {
            // If control got focus with tab and not because user clicked a link
            // then transfer focus to TextBox and clear the flag
            if (!FIsLinkClicked)
            {
                this.Focus();
                
                FIsLinkClicked = false;
            }
        }

        private void LinkLabel_MouseDown(object sender, MouseEventArgs e)
        {
            // Remember that user clicked on the label, so we can correct the focus of a label
            FIsLinkClicked = true;
        }

        #endregion
    }

    #region LinkTypes Enum

    /// <summary>
    /// Types of Hyperlinks that TtxtLinkTextBox supports.
    /// </summary>
    public enum TLinkTypes
    {
        /// <summary>
        /// Act as a regular TextBox
        /// </summary>
        None,

        /// <summary>
        /// Act as a http:// or https:// hyperlink
        /// </summary>
        Http,

        /// <summary>
        /// Act as a ftp:// hyperlink
        /// </summary>
        Ftp,

        /// <summary>
        /// Act as a mailto: hyperlink
        /// </summary>
        Email,
        
        /// <summary>
        /// Get the Skype.exe application to start a call to the supplied Skype ID
        /// </summary>
        Skype
    }

    #endregion
}