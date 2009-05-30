/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timh
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

//using SplitButtonBase;
using System.Globalization;

using Ict.Common;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// todoComment
    /// </summary>
    public enum TControlMode
    {
        /// <summary>todoComment</summary>
        Normal,

        /// <summary>todoComment</summary>
        Matches
    };

    /// <summary>todoComment</summary>
    public enum TMatches
    {
        /// <summary>todoComment</summary>
        BEGINS,

        /// <summary>todoComment</summary>
        ENDS,

        /// <summary>todoComment</summary>
        CONTAINS,

        /// <summary>todoComment</summary>
        EXACT
    };

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TRemoveJokersFromTextBox(SplitButton ASplitButton,
        TextBox AAssociatedTextBox,
        TMatches ALastSelection);

    /// <summary>
    /// A UserControl that is based on the C# Control 'SplitButtonBase'.
    /// It extends it by allowing pre-configured context menus
    ///   (selected by using the ControlMode proeprty)
    /// It also adds a SelectedValue property, and the means to show the context menu
    ///   from outside the control.
    /// </summary>
    [System.ComponentModel.DefaultBindingPropertyAttribute("SelectedValue")]
    public class SplitButton : System.Windows.Forms.UserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button SplitButtonBase1;
        private System.Windows.Forms.ContextMenu mnuMatchStyle;
        private System.Windows.Forms.MenuItem mnuMatchStartsWith;
        private System.Windows.Forms.MenuItem mnuMatchEndsWith;
        private System.Windows.Forms.MenuItem mnuMatchContains;
        private System.Windows.Forms.MenuItem mnuMatchExact;
        private TControlMode FControlMode;
        private System.Object FLastSelection;
        private bool FReturnToControl = false;
        private TextBox FAssociatedTextBox;

        /// <summary>todoComment</summary>
        public event TRemoveJokersFromTextBox RemoveJokersFromTextBox;

        /// <summary>todoComment</summary>
        public TControlMode ControlMode
        {
            get
            {
                return FControlMode;
            }

            set
            {
                FControlMode = value;

                if (FControlMode == TControlMode.Normal)
                {
                    this.SplitButtonBase1.ContextMenu = null;
                }

                if (FControlMode == TControlMode.Matches)
                {
                    this.SplitButtonBase1.ContextMenu = mnuMatchStyle;

                    // default value
                    MnuMatchStartsWith_Click(null, null);
                }
            }
        }

        /// <summary>todoComment</summary>
        public string SelectedValue
        {
            get
            {
                string ReturnValue = "";

                switch (this.ControlMode)
                {
                    case TControlMode.Normal:
                        ReturnValue = this.SplitButtonBase1.Text;
                        break;

                    case TControlMode.Matches:

                        if (FLastSelection != null)
                        {
                            ReturnValue = Enum.GetName(typeof(TMatches), (TMatches)(FLastSelection));
                        }
                        else
                        {
                            ReturnValue = "";
                        }

                        break;
                }

                return ReturnValue;
            }

            set
            {
                switch (this.ControlMode)
                {
                    case TControlMode.Normal:
                        this.SplitButtonBase1.Text = value;
                        break;

                    case TControlMode.Matches:
                        try
                        {
//                            TLogging.Log("SelectedValue for '" + this.Name + "' set to '" + value + "'");
                            switch ((TMatches)(Enum.Parse(typeof(TMatches), value)))
                            {
                                case TMatches.BEGINS:

//                                    TLogging.Log("SelectedValue setter for '" + this.Name + "': invoking 'MnuMatchStartsWith_Click'...");
                                    MnuMatchStartsWith_Click(null, null);
                                    break;

                                case TMatches.ENDS:

//                                    TLogging.Log("SelectedValue setter for '" + this.Name + "': invoking 'MnuMatchEndsWith_Click'...");
                                    MnuMatchEndsWith_Click(null, null);
                                    break;

                                case TMatches.CONTAINS:

//                                    TLogging.Log("SelectedValue setter for '" + this.Name + "': invoking 'MnuMatchContains_Click'...");
                                    MnuMatchContains_Click(null, null);
                                    break;

                                case TMatches.EXACT:

//                                    TLogging.Log("SelectedValue setter for '" + this.Name + "': invoking 'MnuMatchExact_Click'...");
                                    MnuMatchExact_Click(null, null);
                                    break;
                            }
                        }
                        finally
                        {
                        }
                        break;
                }
            }
        }

        /// <summary>todoComment</summary>
        public TextBox AssociatedTextBox
        {
            get
            {
                return FAssociatedTextBox;
            }

            set
            {
                FAssociatedTextBox = value;
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.SplitButtonBase1 = new System.Windows.Forms.Button();
            this.mnuMatchStyle = new System.Windows.Forms.ContextMenu();
            this.mnuMatchStartsWith = new System.Windows.Forms.MenuItem();
            this.mnuMatchEndsWith = new System.Windows.Forms.MenuItem();
            this.mnuMatchContains = new System.Windows.Forms.MenuItem();
            this.mnuMatchExact = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();

            //
            // SplitButtonBase1
            //
//TODO            this.SplitButtonBase1.AlwaysDropDown = true;
//            this.SplitButtonBase1.CalculateSplitRect = false;
            this.SplitButtonBase1.Dock = System.Windows.Forms.DockStyle.Fill;

//            this.SplitButtonBase1.HoverLuminosity = 10;
            this.SplitButtonBase1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SplitButtonBase1.ImageIndex = 0;
            this.SplitButtonBase1.Location = new System.Drawing.Point(0, 0);
            this.SplitButtonBase1.Name = "SplitButtonBase1";
            this.SplitButtonBase1.Size = new System.Drawing.Size(282, 76);

//            this.SplitButtonBase1.SplitHeight = 76;
//            this.SplitButtonBase1.SplitWidth = 12;
            this.SplitButtonBase1.TabIndex = 0;
            this.SplitButtonBase1.TabStop = false;
            this.SplitButtonBase1.Text = "SplitButtonBase1";
            this.SplitButtonBase1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // mnuMatchStyle
            //
            this.mnuMatchStyle.MenuItems.AddRange(new MenuItem[] { this.mnuMatchStartsWith, this.mnuMatchEndsWith, this.mnuMatchContains,
                                                                   this.mnuMatchExact });

            //
            // mnuMatchStartsWith
            //
            this.mnuMatchStartsWith.Index = 0;
            this.mnuMatchStartsWith.Text = "Starts with search term --*";
            this.mnuMatchStartsWith.Click += new System.EventHandler(MnuMatchStartsWith_Click);

            //
            // mnuMatchEndsWith
            //
            this.mnuMatchEndsWith.Index = 1;
            this.mnuMatchEndsWith.Text = "Ends with search term *--";
            this.mnuMatchEndsWith.Click += new System.EventHandler(MnuMatchEndsWith_Click);

            //
            // mnuMatchContains
            //
            this.mnuMatchContains.Index = 2;
            this.mnuMatchContains.Text = "Contains search term *-*";
            this.mnuMatchContains.Click += new System.EventHandler(this.MnuMatchContains_Click);

            //
            // mnuMatchExact
            //
            this.mnuMatchExact.Index = 3;
            this.mnuMatchExact.Text = "Exactly matches search term ---";
            this.mnuMatchExact.Click += new System.EventHandler(this.MnuMatchExact_Click);

            //
            // SplitButton
            //
            this.Controls.Add(this.SplitButtonBase1);
            this.Name = "SplitButton";
            this.Size = new System.Drawing.Size(282, 76);
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public SplitButton() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Default TControlMode
            // FControlMode:=TControlMode.Normal ;
        }

        #endregion

        #region Function Key - Show Menu - Go back to last control

        /// <summary>
        /// todoComment
        /// </summary>
        public void ShowContextMenu()
        {
            if (this.SplitButtonBase1.ContextMenu != null)
            {
                this.Focus();
                FReturnToControl = true;
                this.SplitButtonBase1.ContextMenu.Show(this.SplitButtonBase1, new System.Drawing.Point(0, this.SplitButtonBase1.Height));
            }
        }

        private void TryAndGoBack()
        {
            if ((FAssociatedTextBox != null)
                && (FReturnToControl))
            {
                FAssociatedTextBox.Focus();
                FReturnToControl = false;
            }
        }

        private void ClearAnyJokersFromTextBox()
        {
            if (RemoveJokersFromTextBox != null)
            {
                RemoveJokersFromTextBox(this, FAssociatedTextBox, (TMatches)FLastSelection);
            }
        }

        #endregion

        #region TControlMode.Matches Menu item click events
        private void MnuMatchExact_Click(System.Object sender, System.EventArgs e)
        {
            this.SplitButtonBase1.Text = "---";
            FLastSelection = TMatches.EXACT;

            if (sender != null)
            {
                ClearAnyJokersFromTextBox();
                TryAndGoBack();
            }
        }

        private void MnuMatchContains_Click(System.Object sender, System.EventArgs e)
        {
            this.SplitButtonBase1.Text = "*-*";
            FLastSelection = TMatches.CONTAINS;

            if (sender != null)
            {
                ClearAnyJokersFromTextBox();
                TryAndGoBack();
            }
        }

        private void MnuMatchEndsWith_Click(System.Object sender, System.EventArgs e)
        {
            this.SplitButtonBase1.Text = "*--";
            FLastSelection = TMatches.ENDS;

            if (sender != null)
            {
                ClearAnyJokersFromTextBox();
                TryAndGoBack();
            }
        }

        private void MnuMatchStartsWith_Click(System.Object sender, System.EventArgs e)
        {
            this.SplitButtonBase1.Text = "--*";
            FLastSelection = TMatches.BEGINS;

            if (sender != null)
            {
                ClearAnyJokersFromTextBox();
                TryAndGoBack();
            }
        }

        #endregion

        /// <summary> Clean up any resources being used. </summary>
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
    }
}