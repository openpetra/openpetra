//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh
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
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using Ict.Common;
using System.Drawing;

namespace Ict.Common.Controls
{
    #region TTxtMaskedTextBox

    /// <summary>
    /// Extends a normal textbox and adds mask functions
    /// Can be used as a normal textbox, a masked textbox, or a partnerkey textbox
    /// by setting the ControlMode property
    ///
    /// There are three ways this control can operate, determined by the ControlMode property
    /// ControlMode.NormalTextBox  - Behave as a completely normal textbox
    /// ControlMode.Masked         - Format the text according to the Mask property
    ///                              this consists of # - Digit
    ///                                               &amp; - Letter
    ///                                               ! - Lettor or Digit
    /// ControlMode.PartnerKey     - This is essentially Masked,
    ///                              but presets the mask and the default text
    ///
    /// This control is used as part of txtButtonLabel
    /// </summary>
    public class TTxtMaskedTextBox : System.Windows.Forms.TextBox
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public const Int32 CONTROL_CHARS_BACKSPACE = 8;
        private const int WM_PASTE = 0x0302;
        private const int WM_CUT = 0x0300;
        private const int WM_CLEAR = 0x0303;

        private String FMask;
        private Char[] FParams;
        private String FPlaceHolder;
        private System.ComponentModel.IContainer components = null;
        private TMaskedTextBoxMode FControlMode;

        /// <summary>
        /// todoComment
        /// </summary>
        public string UnformattedText
        {
            get
            {
                Int32 i;
                String strText;

                strText = "";

                for (i = 0; i <= this.Mask.Length - 1; i += 1)
                {
                    if ((System.Array.IndexOf(FParams, this.Mask[i]) > -1) && (this.Text[i] != this.PlaceHolder[0]))
                    {
                        strText = strText + this.Text[i];
                    }
                }

                return strText;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public string PlaceHolder
        {
            get
            {
                return FPlaceHolder;
            }

            set
            {
                if (value.Length > 1)
                {
                    throw new Exception("Placeholder must be 0 or 1 characters long");
                }

                FPlaceHolder = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public string Mask
        {
            get
            {
                return FMask;
            }

            set
            {
                FMask = value;
                this.ClearBox();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public TMaskedTextBoxMode ControlMode
        {
            get
            {
                return FControlMode;
            }

            set
            {
                System.Single mFontSize;
                System.Drawing.Font mFont;
                FControlMode = value;

                if (FControlMode == TMaskedTextBoxMode.PartnerKey)
                {
                    this.Mask = "##########";
                    this.Text = "0000000000";
                    this.MaxLength = 10;
                    this.PlaceHolder = "0";
                    mFontSize = this.Font.Size;
                    mFont = new System.Drawing.Font("Courier New", mFontSize, System.Drawing.FontStyle.Bold);
                    this.Font = mFont;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public new string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                ProcessChangedText(value);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="value"></param>
        private void ProcessChangedText(string value)
        {
            if (value == null) // null value can occur during form initialisation
            {
                base.Text = "";
                return;
            }

            string CurrentText;

//                  MessageBox.Show("set_Text: value=" + value);

            if (DesignMode)
            {
                base.Text = value;
                return;
            }

            /*
             * In partner key mode, when the partner key is set programatically
             * it will bypass the formatting, that is why were here ensure it
             * has enough leading zero's!
             */
            if (this.ControlMode == TMaskedTextBoxMode.PartnerKey)
            {
                CurrentText = this.Text;

//                  MessageBox.Show("CurrentText:" + CurrentText);

                // ignore default placeholder
                if (value == "__________")
                {
                    return;
                }

                if (value.Length < 10)
                {
                    // pad string with leading zeros until maximum length
                    value = value.PadLeft(10, '0');

//                      MessageBox.Show("set_Text: PADDED value=" + value);
                }
                else
                {
                    // trim string to maximum length
                    value = value.Substring(0, 10);
                }

                try
                {
                    System.Convert.ToInt64(value);

//                      MessageBox.Show("set_Text: tmpKey=" + tmpKey.ToString());
                }
                catch (Exception)
                {
                    // if this isn't a number, we don't want it in here!

//                      MessageBox.Show("Exception in set_Text (value: " + value + "): " + exp.ToString() + "\r\n\r\nCurrentText:" + CurrentText);

                    // Reassign value as it was before (necessary in case the user pasted text by using the context menu)
                    // Text needs to be cleared before - otherwise we sometimes get the unpadded value in Text - very strange!
                    base.Text = "";
                    base.Text = CurrentText;
                    return;
                }

                // is a number, and is long enough
                // so it's ok

                // Assign Text property of base TextBox.
                // Text needs to be cleared before - otherwise we sometimes get the unpadded value in Text - very strange!
                base.Text = "";
                base.Text = value;
                return;
            }

            // other modes
            base.Text = value;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TTxtMaskedTextBox() : base()
        {
            FParams = new char[3];
            FParams[0] = '#';
            FParams[1] = '&';
            FParams[2] = '!';
            FPlaceHolder = "_";
            InitializeComponent();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Container"></param>
        public TTxtMaskedTextBox(System.ComponentModel.IContainer Container) : this()
        {
            Container.Add(this);
        }

        /// <summary>
        /// destructor
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                if (components == null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void ClearBox()
        {
            String tmp;

            tmp = this.Mask;

            if (tmp != null)
            {
                tmp = tmp.Replace("#", this.PlaceHolder).Replace("&", this.PlaceHolder).Replace("!", this.PlaceHolder);
                this.Text = tmp;
            }
            else
            {
                this.Text = "";
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if ((this.ControlMode == TMaskedTextBoxMode.NormalTextBox)
                || (this.ControlMode == TMaskedTextBoxMode.Extract))
            {
                // just be a textbox!
                e.Handled = false;
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (!this.ReadOnly)
                {
                    this.ClearBox();
                    this.SelectionLength = this.Text.Length;
                }

                e.Handled = true;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }

            // handle COPY
            if ((e.KeyCode == Keys.C) && (e.Modifiers == Keys.Control))
            {
                try
                {
                    Clipboard.SetDataObject(this.SelectedText);
                    e.Handled = true;
                }
                catch (Exception)
                {
//                  MessageBox.Show("Exception in OnKeyDown: " + exp.ToString());

                    // never mind
                }
            }

            // handle CUT
            if ((e.KeyCode == Keys.X) && (e.Modifiers == Keys.Control))
            {
                HandleCut();
                e.Handled = true;
            }

            // handle PASTE
            if ((e.KeyCode == Keys.V) && (e.Modifiers == Keys.Control))
            {
                HandlePaste();
                e.Handled = true;
            }
        }

        /// <summary>
        /// key has been pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            Char chrKeyPressed;
            Int32 intSelStart;
            Int32 intDelTo;
            String strText;
            bool bolDelete;
            Int32 i;
            bool HadEnough;

            HadEnough = false;

            if ((this.ControlMode == TMaskedTextBoxMode.NormalTextBox)
                || (this.ControlMode == TMaskedTextBoxMode.Extract))
            {
                // just be a textbox!
                e.Handled = false;
                return;
            }

            if (this.ReadOnly == true)
            {
                // no further action
                e.Handled = true;
                return;
            }

            chrKeyPressed = e.KeyChar;

            // Original cursor position
            intSelStart = this.SelectionStart;

            if (this.SelectionLength == this.Text.Length)
            {
                if (!(Control.ModifierKeys == Keys.Control))
                {
                    ClearBox();
                    intSelStart = this.Text.Length;
                }
            }

            if ((intSelStart == this.Text.Length) && (this.ControlMode == TMaskedTextBoxMode.PartnerKey))
            {
                // insert from right in partner key mode
                if ((chrKeyPressed != (char)(CONTROL_CHARS_BACKSPACE)) && (System.Char.IsDigit(chrKeyPressed) == true))
                {
                    strText = this.Text.Remove(0, 1);
                    strText = strText + this.PlaceHolder;
                    this.Text = strText;
                    intSelStart = intSelStart - 1;
                    this.SelectionStart = intSelStart;
                }
            }

            // In case of a selection, delete text to this position
            intDelTo = this.SelectionStart + this.SelectionLength - 1;
            strText = this.Text;

            // Used to avoid deletion of the selection when an invalid key is pressed
            bolDelete = false;

            e.Handled = true;

            if (chrKeyPressed == (char)(CONTROL_CHARS_BACKSPACE))
            {
                bolDelete = true;

                if ((intSelStart > 0) && (intDelTo < intSelStart))
                {
                    intSelStart = intSelStart - 1;
                }
            }

            // Find the Next Insertion point
            for (i = this.SelectionStart; i <= this.Mask.Length - 1; i += 1)
            {
                if (HadEnough == false)
                {
                    // Test for # or &
                    if ((this.Mask[i] == '#') && (System.Char.IsDigit(chrKeyPressed) == true))
                    {
                        strText = strText.Remove(i, 1).Insert(i, new String(chrKeyPressed, 1));
                        intSelStart = i + 1;
                        bolDelete = true;
                    }

                    if ((this.Mask[i] == '&') && (System.Char.IsLetter(chrKeyPressed) == true))
                    {
                        strText = strText.Remove(i, 1).Insert(i, new String(chrKeyPressed, 1));
                        intSelStart = i + 1;
                        bolDelete = true;
                    }

                    if ((this.Mask[i] == '!') && (System.Char.IsLetterOrDigit(chrKeyPressed) == true))
                    {
                        strText = strText.Remove(i, 1).Insert(i, new String(chrKeyPressed, 1));
                        intSelStart = i + 1;
                        bolDelete = true;
                    }
                }

                // end of HadEnought test

                // Prevent looping unitl the next available match when mixing # & ! on the same mask
                if (System.Array.IndexOf(FParams, this.Mask[i]) > -1)
                {
                    HadEnough = true;
                }
            }   // end of loop

            if (bolDelete == true)
            {
                for (i = intSelStart; i <= intDelTo; i += 1)
                {
                    if (System.Array.IndexOf(FParams, this.Mask[i]) > -1)
                    {
                        strText = strText.Remove(i, 1).Insert(i, this.PlaceHolder);
                    }
                }

                this.Text = strText;
                this.SelectionStart = intSelStart;
                this.SelectionLength = 0;
            }
        }

        /// <summary>
        /// This procedure captures WndProc Windows Messages. It is used here to
        /// intercept the commands from the default context menu that is present
        /// for a TextBox.
        /// </summary>
        /// <returns>void</returns>
        protected override void WndProc(ref Message m)
        {
            // TextBox's context menu, Cut command
            if (m.Msg == WM_CUT)
            {
                if (!this.ReadOnly)
                {
                    HandleCut();

                    // Raise a faked KeyUp Event.
                    // This is needed for Forms/UserControls where the contents of the Form/UserControl
                    // are updated if data in this control has changed.
                    base.OnKeyUp(new KeyEventArgs(Keys.ControlKey));
                }

                return;
            }

            // TextBox's context menu, Paste command
            if (m.Msg == WM_PASTE)
            {
                if (!this.ReadOnly)
                {
                    HandlePaste();

                    // Raise a faked KeyUp Event.
                    // This is needed for Forms/UserControls where the contents of the Form/UserControl
                    // are updated if data in this control has changed.
                    base.OnKeyUp(new KeyEventArgs(Keys.ControlKey));
                }

                return;
            }

            // TextBox's context menu, Delete command
            if (m.Msg == WM_CLEAR)
            {
                if (!this.ReadOnly)
                {
                    this.ClearBox();

                    // Raise a faked KeyUp Event.
                    // This is needed for Forms/UserControls where the contents of the Form/UserControl
                    // are updated if data in this control has changed.
                    base.OnKeyUp(new KeyEventArgs(Keys.ControlKey));
                }

                return;
            }

            base.WndProc(ref m);
        }

        private void HandlePaste()
        {
            String str;
            IDataObject clip;

            if (!this.ReadOnly)
            {
//          MessageBox.Show("HandlePaste");

                clip = Clipboard.GetDataObject();

                if (clip != null)
                {
                    // try and paste the contents
                    try
                    {
                        str = (String)(clip.GetData(DataFormats.Text));

                        if (this.SelectionLength > 0)
                        {
                            if (this.ControlMode == TMaskedTextBoxMode.PartnerKey)
                            {                            
                                try 
                                {
                                    if (System.Convert.ToInt64(str) != 0)
                                    {
                                        this.SelectedText = str;
                                    }
                                    
                                    ProcessChangedText(this.Text);
                                } 
                                catch (System.FormatException)
                                {                   
                                    // Ignore this Exception as it will be raised by System.Convert.ToInt64 when
                                    // the txtPartnerKey.Text Property holds something that isn't an Int64 (that
                                    // can happen when a user pastes any string from the Clipboard into the Partner Key TextBox!)
                                }
                                catch (Exception)
                                {                   
                                    throw;
                                }
                            }
                            else
                            {
                                this.SelectedText = str;
                                ProcessChangedText(this.Text);
                            }
                        }
                        else
                        {
                            if (this.ControlMode == TMaskedTextBoxMode.PartnerKey)
                            {                            
                                try 
                                {
                                    if (System.Convert.ToInt64(str) != 0)
                                    {
                                        this.Text = str;
                                    }
                                } 
                                catch (System.FormatException)
                                {                   
                                    // Ignore this Exception as it will be raised by System.Convert.ToInt64 when
                                    // the txtPartnerKey.Text Property holds something that isn't an Int64 (that
                                    // can happen when a user pastes any string from the Clipboard into the Partner Key TextBox!)
                                }
                                catch (Exception)
                                {                   
                                    throw;
                                }
                            }
                            else
                            {
                                this.Text = str;                                
                            }
                        }
                    }
                    catch (Exception)
                    {
//                  MessageBox.Show("Exception in HandlePaste: " + exp.ToString());

                        // never mind
                    }
                }
            }
        }

        private void HandleCut()
        {
            try
            {
                if (this.SelectedText.Length > 0)
                {
                    Clipboard.SetDataObject(this.SelectedText);

                    if (!this.ReadOnly)
                    {
                        if (this.SelectionLength == this.Text.Length)
                        {
                            this.ClearBox();
                        }
                        else
                        {
                            this.SelectedText = new String('0', this.SelectedText.Length);
                            ProcessChangedText(this.Text);
                        }
                    }
                }
            }
            catch (Exception)
            {
//              MessageBox.Show("Exception in HandleCut: " + exp.ToString());

                // never mind
            }
        }
    }
    #endregion

    /// <summary>
    /// todoComment
    /// </summary>
    public enum TMaskedTextBoxMode
    {
        /// <summary>
        /// todoComment
        /// </summary>
        NormalTextBox,

        /// <summary>
        /// todoComment
        /// </summary>
        PartnerKey,

        /// <summary>
        /// todoComment
        /// </summary>
        Masked,

        /// <summary>
        /// todoComment
        /// </summary>
        Extract
    };
}