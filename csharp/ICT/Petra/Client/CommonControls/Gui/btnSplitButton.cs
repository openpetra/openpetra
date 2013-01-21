//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using GNU.Gettext;
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
    public partial class SplitButton : System.Windows.Forms.UserControl
    {
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
                if (value == null) // This may happen during initialisation of a form
                {
                    this.SplitButtonBase1.Text = "";
                }
                else
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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.SplitButtonBase1.Text = Catalog.GetString("SplitButtonBase1");
            this.mnuMatchStartsWith.Text = Catalog.GetString("Starts with search term --*");
            this.mnuMatchEndsWith.Text = Catalog.GetString("Ends with search term *--");
            this.mnuMatchContains.Text = Catalog.GetString("Contains search term *-*");
            this.mnuMatchExact.Text = Catalog.GetString("Exactly matches search term ---");
            #endregion

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
    }
}